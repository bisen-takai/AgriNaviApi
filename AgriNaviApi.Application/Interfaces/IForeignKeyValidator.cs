using AgriNaviApi.Application.Requests.ShipmentLines;

namespace AgriNaviApi.Application.Interfaces
{
    public interface IForeignKeyValidator
    {
        Task ValidateColorExistsAsync(int colorId, CancellationToken cancellationToken);

        Task ValidateGroupExistsAsync(int groupId, CancellationToken cancellationToken);

        Task ValidateCropExistsAsync(int cropId, CancellationToken cancellationToken);

        Task ValidateFieldExistsAsync(int fieldId, CancellationToken cancellationToken);

        Task ValidateSeasonScheduleExistsAsync(int seasonScheduleId, CancellationToken cancellationToken);

        Task ValidateShipDestinationExistsAsync(int shipDestinationId, CancellationToken cancellationToken);

        Task ValidateQualityStandardExistsAsync(int qualityStandardId, CancellationToken cancellationToken);

        Task ValidateUnitExistsAsync(int unitId, CancellationToken cancellationToken);

        Task ValidateShipmentExistsAsync(int shipmentId, CancellationToken cancellationToken);

        Task ValidateShipmentWithLineExistsAsync(IEnumerable<ShipmentLineCreateRequest> lines, CancellationToken cancellationToken);

        Task ValidateShipmentWithLineExistsAsync(IEnumerable<ShipmentLineWithUuidRequest> lines, CancellationToken cancellationToken);
    }
}
