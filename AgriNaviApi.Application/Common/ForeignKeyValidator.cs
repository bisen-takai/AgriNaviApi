using AgriNaviApi.Application.Interfaces;
using AgriNaviApi.Application.Requests.ShipmentLines;
using AgriNaviApi.Application.Validators;
using AgriNaviApi.Infrastructure.Interfaces;
using AgriNaviApi.Infrastructure.Persistence.Contexts;
using AgriNaviApi.Infrastructure.Persistence.Entities;
using AgriNaviApi.Shared.Exceptions;
using AgriNaviApi.Shared.Resources;
using Microsoft.EntityFrameworkCore;

namespace AgriNaviApi.Application.Common
{
    public class ForeignKeyValidator : IForeignKeyValidator
    {
        private readonly AppDbContext _context;

        public ForeignKeyValidator(AppDbContext context)
        {
            _context = context;
        }

        public Task ValidateColorExistsAsync(int colorId, CancellationToken cancellationToken) =>
            ValidateExistsAsync<ColorEntity>(colorId, "カラー", cancellationToken);

        public Task ValidateGroupExistsAsync(int groupId, CancellationToken cancellationToken) =>
            ValidateExistsAsync<GroupEntity>(groupId, "グループ", cancellationToken);

        public Task ValidateCropExistsAsync(int cropId, CancellationToken cancellationToken) =>
            ValidateExistsAsync<CropEntity>(cropId, "作付", cancellationToken);

        public Task ValidateFieldExistsAsync(int fieldId, CancellationToken cancellationToken) =>
            ValidateExistsAsync<FieldEntity>(fieldId, "圃場", cancellationToken);

        public Task ValidateSeasonScheduleExistsAsync(int seasonScheduleId, CancellationToken cancellationToken) =>
            ValidateExistsAsync<SeasonScheduleEntity>(seasonScheduleId, "作付計画", cancellationToken);

        public Task ValidateShipDestinationExistsAsync(int shipDestinationId, CancellationToken cancellationToken) =>
            ValidateExistsAsync<ShipDestinationEntity>(shipDestinationId, "出荷先", cancellationToken);

        public Task ValidateQualityStandardExistsAsync(int qualityStandardId, CancellationToken cancellationToken) =>
            ValidateExistsAsync<QualityStandardEntity>(qualityStandardId, "品質・規格", cancellationToken);

        public Task ValidateUnitExistsAsync(int unitId, CancellationToken cancellationToken) =>
            ValidateExistsAsync<UnitEntity>(unitId, "単位", cancellationToken);

        public Task ValidateShipmentExistsAsync(int shipmentId, CancellationToken cancellationToken) =>
            ValidateExistsAsync<ShipmentEntity>(shipmentId, "出荷記録", cancellationToken);

        public async Task ValidateShipmentWithLineExistsAsync(IEnumerable<ShipmentLineCreateRequest> lines, CancellationToken cancellationToken)
        {
            await ValidateShipmentLineForeignKeysAsync(
                lines.Select(l => new ShipmentLineForeignKey(l.ShipDestinationId, l.QualityStandardId, l.UnitId)),
                cancellationToken);
        }

        public async Task ValidateShipmentWithLineExistsAsync(IEnumerable<ShipmentLineWithUuidRequest> lines, CancellationToken cancellationToken)
        {
            await ValidateShipmentLineForeignKeysAsync(
                lines.Select(l => new ShipmentLineForeignKey(l.ShipDestinationId, l.QualityStandardId, l.UnitId)),
                cancellationToken);
        }

        private async Task ValidateExistsAsync<TEntity>(
            int id,
            string label,
            CancellationToken cancellationToken)
            where TEntity : class, IHasId
        {
            var exists = await _context.Set<TEntity>()
                .AsNoTracking()
                .AnyAsync(e => e.Id == id, cancellationToken);

            if (!exists)
                throw new EntityNotFoundException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.NotFoundErrorMessage),
                    args: new[] { label }
                );
        }

        public async Task ValidateShipmentLineForeignKeysAsync<T>(
            IEnumerable<T> lines,
            CancellationToken cancellationToken)
            where T : IShipmentLineForeignKeys
        {
            var destinationIds = lines.Select(l => l.ShipDestinationId).ToHashSet();
            var qualityIds = lines.Select(l => l.QualityStandardId).ToHashSet();
            var unitIds = lines.Select(l => l.UnitId).ToHashSet();

            var validDestinations = (await _context.ShipDestinations
                .Where(d => destinationIds.Contains(d.Id))
                .Select(d => d.Id)
                .ToListAsync(cancellationToken))
                .ToHashSet();

            var validQualities = (await _context.QualityStandards
                .Where(q => qualityIds.Contains(q.Id))
                .Select(q => q.Id)
                .ToListAsync(cancellationToken))
                .ToHashSet();

            var validUnits = (await _context.Units
                .Where(u => unitIds.Contains(u.Id))
                .Select(u => u.Id)
                .ToListAsync(cancellationToken))
                .ToHashSet();

            foreach (var line in lines)
            {
                if (!validDestinations.Contains(line.ShipDestinationId))
                    throw new EntityNotFoundException(
                        resourceType: typeof(CommonErrorMessages),
                        resourceKey: nameof(CommonErrorMessages.NotFoundErrorMessage),
                        args: new[] { "出荷先" });

                if (!validQualities.Contains(line.QualityStandardId))
                    throw new EntityNotFoundException(
                        resourceType: typeof(CommonErrorMessages),
                        resourceKey: nameof(CommonErrorMessages.NotFoundErrorMessage),
                        args: new[] { "品質・規格" });

                if (!validUnits.Contains(line.UnitId))
                    throw new EntityNotFoundException(
                        resourceType: typeof(CommonErrorMessages),
                        resourceKey: nameof(CommonErrorMessages.NotFoundErrorMessage),
                        args: new[] { "単位" });
            }
        }
    }
}
