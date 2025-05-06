using AgriNaviApi.Application.Profiles;
using AgriNaviApi.Application.Requests.Colors;
using AgriNaviApi.Application.Services;
using AgriNaviApi.Common.Enums;
using AgriNaviApi.Common.Exceptions;
using AgriNaviApi.Common.Interfaces;
using AgriNaviApi.Infrastructure.Persistence.Contexts;
using AgriNaviApi.Infrastructure.Persistence.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace AgriNaviApi.Application.UnitTests
{
    /// <summary>
    /// ��Ƀr�W�l�X���W�b�N�ȂǃT�[�r�X�w�̓�������؂��邱�Ƃɏœ_�𓖂ĂĂ��܂��B
    /// ���b�Z�[�W���e�̊m�F�̓R���g���[�����ōs�����߁A�������Ă��܂��B
    /// </summary>
    public class ColorServicesTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IUuidGenerator> _uuidGeneratorMock;
        private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;

        public ColorServicesTests()
        {
            // AutoMapper �̐ݒ�iColorProfile ���g�p�j
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ColorProfile>();
            });
            _mapper = config.CreateMapper();

            // IUuidGenerator �̃��b�N�ݒ�
            _uuidGeneratorMock = new Mock<IUuidGenerator>();
            _uuidGeneratorMock.Setup(x => x.GenerateUuid())
                .Returns(Guid.Parse("11111111-1111-1111-1111-111111111111"));

            // IDateTimeProvider �̃��b�N�ݒ�
            _dateTimeProviderMock = new Mock<IDateTimeProvider>();
            _dateTimeProviderMock.Setup(x => x.UtcNow)
                .Returns(new DateTime(2025, 1, 1));
        }

        /// <summary>
        /// InMemory �f�[�^�x�[�X�𗘗p���āAAppDbContext�𐶐����邽�߂̃w���p�[
        /// </summary>
        private AppDbContext CreateInMemoryContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                            .UseInMemoryDatabase(databaseName: dbName)
                            .Options;
            return new AppDbContext(options);
        }

        /// <summary>
        /// ����ɓo�^�ł��邱�Ƃ��m�F
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CreateColorAsync_Normal()
        {
            // �f�[�^�x�[�X���͑��e�X�g�P�[�X�Ɣ��Ȃ��悤��UUID�Ƃ���
            string dbName = Guid.NewGuid().ToString();
            using var context = CreateInMemoryContext(dbName);

            var service = new ColorService(
                context,
                _uuidGeneratorMock.Object,
                _mapper,
                _dateTimeProviderMock.Object);

            var request = new ColorCreateRequest
            {
                Name = "Blue",
                RedValue = 0,
                GreenValue = 0,
                BlueValue = 255
            };

            var result = await service.CreateColorAsync(request);

            Assert.NotNull(result);
            Assert.Equal("Blue", result.Name);

            // �R���e�L�X�g��ɓo�^���ꂽ�G���e�B�e�B���������ݒ肳��Ă��邩�m�F
            var createdEntity = await context.Colors.FirstOrDefaultAsync();
            Assert.NotNull(createdEntity);
            Assert.Equal(Guid.Parse("11111111-1111-1111-1111-111111111111"), createdEntity.Uuid);
            Assert.Equal(new DateTime(2025, 1, 1), createdEntity.CreatedAt);
            Assert.Equal(new DateTime(2025, 1, 1), createdEntity.LastUpdatedAt);
        }

        /// <summary>
        /// �����J���[���͓o�^�ł��Ȃ����Ƃ��m�F
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CreateColorAsync_Abnormality()
        {
            // �f�[�^�x�[�X���͑��e�X�g�P�[�X�Ɣ��Ȃ��悤��UUID�Ƃ���
            string dbName = Guid.NewGuid().ToString();
            using var context = CreateInMemoryContext(dbName);

            // ���O�ɓ����J���[�� ("Blue") �̃G���e�B�e�B��o�^���Ă���
            context.Colors.Add(new ColorEntity
            {
                Name = "Blue",
                Uuid = Guid.NewGuid(),
                RedValue = 0,
                GreenValue = 0,
                BlueValue = 255,
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow
            });
            await context.SaveChangesAsync();

            var service = new ColorService(
                context,
                _uuidGeneratorMock.Object,
                _mapper,
                _dateTimeProviderMock.Object);

            var request = new ColorCreateRequest
            {
                Name = "Blue", // �d�����閼�O
                RedValue = 10,
                GreenValue = 10,
                BlueValue = 10
            };

            // �������O�œo�^���悤�Ƃ���� DuplicateEntityException ���������邱�Ƃ����؂���
            await Assert.ThrowsAsync<DuplicateEntityException>(
                async () => await service.CreateColorAsync(request));
        }

        /// <summary>
        /// ����ɏڍ׏����擾�ł��邱�Ƃ��m�F
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetColorByIdAsync_Normal()
        {
            // �f�[�^�x�[�X���͑��e�X�g�P�[�X�Ɣ��Ȃ��悤��UUID�Ƃ���
            string dbName = Guid.NewGuid().ToString();
            using var context = CreateInMemoryContext(dbName);

            // �e�X�g�p�̃G���e�B�e�B���쐬����DB�ɒǉ�
            var testColor = new ColorEntity
            {
                Name = "Green",
                Uuid = Guid.NewGuid(),
                RedValue = 0,
                GreenValue = 255,
                BlueValue = 0,
                CreatedAt = new DateTime(2020, 1, 1),
                LastUpdatedAt = new DateTime(2020, 1, 1)
            };

            context.Colors.Add(testColor);
            await context.SaveChangesAsync();

            var service = new ColorService(
                context,
                _uuidGeneratorMock.Object,
                _mapper,
                _dateTimeProviderMock.Object);

            var detailDto = await service.GetColorByIdAsync(testColor.Id);

            Assert.NotNull(detailDto);
            Assert.Equal(testColor.Id, detailDto.Id);
            Assert.Equal(testColor.Name, detailDto.Name);
        }

        /// <summary>
        /// ���݂��Ȃ�ID�̏ڍ׏����擾�����ꍇ�̊m�F
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetColorByIdAsync_Abnormality()
        {
            // �f�[�^�x�[�X���͑��e�X�g�P�[�X�Ɣ��Ȃ��悤��UUID�Ƃ���
            string dbName = Guid.NewGuid().ToString();
            using var context = CreateInMemoryContext(dbName);

            var service = new ColorService(
                context,
                _uuidGeneratorMock.Object,
                _mapper,
                _dateTimeProviderMock.Object);

            int nonExistingId = 999; // ���݂��Ȃ�ID

            // KeyNotFoundException�ƂȂ邱�Ƃ��m�F
            await Assert.ThrowsAsync<KeyNotFoundException>(
                async () => await service.GetColorByIdAsync(nonExistingId));
        }

        /// <summary>
        /// ����ɍX�V�ł��邱�Ƃ��m�F
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UpdateColorAsync_Normal()
        {
            // �f�[�^�x�[�X���͑��e�X�g�P�[�X�Ɣ��Ȃ��悤��UUID�Ƃ���
            string dbName = Guid.NewGuid().ToString();
            using var context = CreateInMemoryContext(dbName);

            // ������Ԃ̃G���e�B�e�B���쐬����DB�ɓo�^
            var initialColor = new ColorEntity
            {
                Name = "Red",
                Uuid = Guid.NewGuid(),
                RedValue = 255,
                GreenValue = 0,
                BlueValue = 0,
                CreatedAt = new DateTime(2020, 1, 1),
                LastUpdatedAt = new DateTime(2020, 1, 1)
            };
            context.Colors.Add(initialColor);
            await context.SaveChangesAsync();

            var service = new ColorService(
                context,
                _uuidGeneratorMock.Object,
                _mapper,
                _dateTimeProviderMock.Object);

            // �X�V���N�G�X�g���쐬�i���̂�RGB�l�̕ύX�j
            var updateRequest = new ColorUpdateRequest
            {
                Id = initialColor.Id,
                Name = "Dark Red",
                RedValue = 200,
                GreenValue = 10,
                BlueValue = 10
            };

            var updateResult = await service.UpdateColorAsync(updateRequest);

            Assert.NotNull(updateResult);
            Assert.Equal(initialColor.Id, updateResult.Id);
            Assert.Equal("Dark Red", updateResult.Name);
            Assert.Equal(200, updateResult.RedValue);
            Assert.Equal(10, updateResult.GreenValue);
            Assert.Equal(10, updateResult.BlueValue);

            // �X�V���LastUpdatedAt�����b�N�Őݒ肵�������ɂȂ��Ă��邩�m�F
            var updatedEntity = await context.Colors.FindAsync(initialColor.Id);
            Assert.Equal(new DateTime(2025, 1, 1), updatedEntity.LastUpdatedAt);
            // �X�V���CreatedAt���X�V����Ă��Ȃ����Ƃ��m�F
            Assert.Equal(new DateTime(2020, 1, 1), updatedEntity.CreatedAt);
        }

        /// <summary>
        /// ���݂��Ȃ�ID�̏ꍇ���X�V�����ꍇ�̊m�F
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UpdateColorAsync_Abnormality()
        {
            // �f�[�^�x�[�X���͑��e�X�g�P�[�X�Ɣ��Ȃ��悤��UUID�Ƃ���
            string dbName = Guid.NewGuid().ToString();
            using var context = CreateInMemoryContext(dbName);

            var service = new ColorService(
                context,
                _uuidGeneratorMock.Object,
                _mapper,
                _dateTimeProviderMock.Object);

            var updateRequest = new ColorUpdateRequest
            {
                Id = 999, // ���݂��Ȃ�ID
                Name = "Nonexistent",
                RedValue = 50,
                GreenValue = 50,
                BlueValue = 50
            };

            // KeyNotFoundException�ƂȂ邱�Ƃ��m�F
            await Assert.ThrowsAsync<KeyNotFoundException>(
                async () => await service.UpdateColorAsync(updateRequest));
        }

        /// <summary>
        /// ����ɍ폜���ł��邱�Ƃ��m�F
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DeleteColorAsync_Normal()
        {
            // �f�[�^�x�[�X���͑��e�X�g�P�[�X�Ɣ��Ȃ��悤��UUID�Ƃ���
            string dbName = Guid.NewGuid().ToString();
            using var context = CreateInMemoryContext(dbName);

            // �e�X�g�p�G���e�B�e�B��DB�ɒǉ�
            var testColor = new ColorEntity
            {
                Name = "Yellow",
                Uuid = Guid.NewGuid(),
                RedValue = 255,
                GreenValue = 255,
                BlueValue = 0,
                CreatedAt = new DateTime(2020, 1, 1),
                LastUpdatedAt = new DateTime(2020, 1, 1)
            };
            context.Colors.Add(testColor);
            await context.SaveChangesAsync();

            var service = new ColorService(
                context,
                _uuidGeneratorMock.Object,
                _mapper,
                _dateTimeProviderMock.Object);

            var deleteResult = await service.DeleteColorAsync(testColor.Id);

            Assert.NotNull(deleteResult);
            Assert.Equal(testColor.Id, deleteResult.Id);
            Assert.True(deleteResult.IsDeleted);
            // �폜���b�Z�[�W�ɃJ���[�����܂܂�Ă��邱�Ƃ��m�F
            Assert.Contains(testColor.Name, deleteResult.Message);

            // DB�ォ��G���e�B�e�B���폜����Ă��邩�m�F
            var deletedEntity = await context.Colors.FindAsync(testColor.Id);
            Assert.Null(deletedEntity);
        }

        /// <summary>
        /// ���݂��Ȃ�ID�̏ꍇ���폜�����ꍇ�̊m�F
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DeleteColorAsync_Abnormality()
        {
            // �f�[�^�x�[�X���͑��e�X�g�P�[�X�Ɣ��Ȃ��悤��UUID�Ƃ���
            string dbName = Guid.NewGuid().ToString();
            using var context = CreateInMemoryContext(dbName);

            var service = new ColorService(
                context,
                _uuidGeneratorMock.Object,
                _mapper,
                _dateTimeProviderMock.Object);

            // InvalidOperationException�ƂȂ邱�Ƃ��m�F
            await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await service.DeleteColorAsync(999));
        }

        /// <summary>
        /// �S�����������ꍇ�̊m�F
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task SearchColorAsync_AllSearch()
        {
            // �f�[�^�x�[�X���͑��e�X�g�P�[�X�Ɣ��Ȃ��悤��UUID�Ƃ���
            string dbName = Guid.NewGuid().ToString();
            using var context = CreateInMemoryContext(dbName);

            // �e�X�g�p�̃G���e�B�e�B��o�^
            context.Colors.AddRange(new List<ColorEntity>
            {
                new ColorEntity { Name = "Blue", RedValue = 0, GreenValue = 0, BlueValue = 255, Uuid = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, LastUpdatedAt = DateTime.UtcNow },
                new ColorEntity { Name = "Red", RedValue = 255, GreenValue = 0, BlueValue = 0, Uuid = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, LastUpdatedAt = DateTime.UtcNow },
                new ColorEntity { Name = "Green", RedValue = 0, GreenValue = 255, BlueValue = 0, Uuid = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, LastUpdatedAt = DateTime.UtcNow }
            });
            await context.SaveChangesAsync();

            var service = new ColorService(
                context,
                _uuidGeneratorMock.Object,
                _mapper,
                _dateTimeProviderMock.Object);

            var searchRequest = new ColorSearchRequest
            {
                SearchColorName = "", // �������w��i�󕶎��ł� null �ł��j
                SearchMatchType = SearchMatchType.None
            };

            var result = await service.SearchColorAsync(searchRequest);

            Assert.NotNull(result);
            Assert.Equal(3, result.TotalCount);
            Assert.Equal(3, result.SearchItems.Count());
        }

        /// <summary>
        /// ���S��v���������ꍇ�̊m�F
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task SearchColorAsync_ExactMatch()
        {
            // �f�[�^�x�[�X���͑��e�X�g�P�[�X�Ɣ��Ȃ��悤��UUID�Ƃ���
            string dbName = Guid.NewGuid().ToString();
            using var context = CreateInMemoryContext(dbName);

            // "Blue" �� "BlueSky" ��o�^�i���S��v�Ȃ� "Blue" �݂̂��Y���j
            context.Colors.AddRange(new List<ColorEntity>
            {
                new ColorEntity { Name = "Blue", RedValue = 0, GreenValue = 0, BlueValue = 255, Uuid = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, LastUpdatedAt = DateTime.UtcNow },
                new ColorEntity { Name = "BlueSky", RedValue = 0, GreenValue = 100, BlueValue = 255, Uuid = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, LastUpdatedAt = DateTime.UtcNow },
                new ColorEntity { Name = "Red", RedValue = 255, GreenValue = 0, BlueValue = 0, Uuid = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, LastUpdatedAt = DateTime.UtcNow }
            });
            await context.SaveChangesAsync();

            var service = new ColorService(
                context,
                _uuidGeneratorMock.Object,
                _mapper,
                _dateTimeProviderMock.Object);

            var searchRequest = new ColorSearchRequest
            {
                SearchColorName = "Blue",
                SearchMatchType = SearchMatchType.EXACT
            };

            var result = await service.SearchColorAsync(searchRequest);

            Assert.NotNull(result);
            Assert.Equal(1, result.TotalCount);
            Assert.Single(result.SearchItems);

            var items = result.SearchItems.ToList();
            Assert.Equal("Blue", items[0].Name);
        }

        /// <summary>
        /// �O����v���������ꍇ�̊m�F
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task SearchColorAsync_PrefixMatch()
        {
            // �f�[�^�x�[�X���͑��e�X�g�P�[�X�Ɣ��Ȃ��悤��UUID�Ƃ���
            string dbName = Guid.NewGuid().ToString();
            using var context = CreateInMemoryContext(dbName);

            // "Blue" �� "BlueSky" �݂̂��O����v�Ńq�b�g����͂�
            context.Colors.AddRange(new List<ColorEntity>
            {
                new ColorEntity { Name = "Blue", RedValue = 0, GreenValue = 0, BlueValue = 255, Uuid = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, LastUpdatedAt = DateTime.UtcNow },
                new ColorEntity { Name = "BlueSky", RedValue = 0, GreenValue = 100, BlueValue = 255, Uuid = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, LastUpdatedAt = DateTime.UtcNow },
                new ColorEntity { Name = "Red", RedValue = 255, GreenValue = 0, BlueValue = 0, Uuid = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, LastUpdatedAt = DateTime.UtcNow }
            });
            await context.SaveChangesAsync();

            var service = new ColorService(
                context,
                _uuidGeneratorMock.Object,
                _mapper,
                _dateTimeProviderMock.Object);

            var searchRequest = new ColorSearchRequest
            {
                SearchColorName = "Blue",
                SearchMatchType = SearchMatchType.PREFIX
            };

            var result = await service.SearchColorAsync(searchRequest);

            Assert.NotNull(result);
            Assert.Equal(2, result.TotalCount);
            Assert.All(result.SearchItems, item => Assert.StartsWith("Blue", item.Name));
        }

        /// <summary>
        /// �����v���������ꍇ�̊m�F
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task SearchColorAsync_SuffixMatch()
        {
            // �f�[�^�x�[�X���͑��e�X�g�P�[�X�Ɣ��Ȃ��悤��UUID�Ƃ���
            string dbName = Guid.NewGuid().ToString();
            using var context = CreateInMemoryContext(dbName);

            // "SkyBlue" �� "NavyBlue" �͖����� "Blue" �ň�v����͂�
            context.Colors.AddRange(new List<ColorEntity>
            {
                new ColorEntity { Name = "SkyBlue", RedValue = 0, GreenValue = 0, BlueValue = 255, Uuid = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, LastUpdatedAt = DateTime.UtcNow },
                new ColorEntity { Name = "NavyBlue", RedValue = 0, GreenValue = 0, BlueValue = 139, Uuid = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, LastUpdatedAt = DateTime.UtcNow },
                new ColorEntity { Name = "Red", RedValue = 255, GreenValue = 0, BlueValue = 0, Uuid = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, LastUpdatedAt = DateTime.UtcNow }
            });
            await context.SaveChangesAsync();

            var service = new ColorService(
                context,
                _uuidGeneratorMock.Object,
                _mapper,
                _dateTimeProviderMock.Object);

            var searchRequest = new ColorSearchRequest
            {
                SearchColorName = "Blue",
                SearchMatchType = SearchMatchType.SUFFIX
            };

            var result = await service.SearchColorAsync(searchRequest);

            Assert.NotNull(result);
            Assert.Equal(2, result.TotalCount);
            Assert.All(result.SearchItems, item => Assert.EndsWith("Blue", item.Name));
        }

        /// <summary>
        /// ������v���������ꍇ�̊m�F
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task SearchColorAsync_PartialMatch()
        {
            // �f�[�^�x�[�X���͑��e�X�g�P�[�X�Ɣ��Ȃ��悤��UUID�Ƃ���
            string dbName = Guid.NewGuid().ToString();
            using var context = CreateInMemoryContext(dbName);

            // "Crimson" �� "CrimsonRed" �� "imson" ���܂ނ̂Ńq�b�g����
            context.Colors.AddRange(new List<ColorEntity>
            {
                new ColorEntity { Name = "Crimson", RedValue = 220, GreenValue = 20, BlueValue = 60, Uuid = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, LastUpdatedAt = DateTime.UtcNow },
                new ColorEntity { Name = "CrimsonRed", RedValue = 220, GreenValue = 20, BlueValue = 60, Uuid = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, LastUpdatedAt = DateTime.UtcNow },
                new ColorEntity { Name = "Blue", RedValue = 0, GreenValue = 0, BlueValue = 255, Uuid = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, LastUpdatedAt = DateTime.UtcNow }
            });
            await context.SaveChangesAsync();

            var service = new ColorService(
                context,
                _uuidGeneratorMock.Object,
                _mapper,
                _dateTimeProviderMock.Object);

            var searchRequest = new ColorSearchRequest
            {
                SearchColorName = "imson",
                SearchMatchType = SearchMatchType.PARTIAL
            };

            var result = await service.SearchColorAsync(searchRequest);

            Assert.NotNull(result);
            Assert.Equal(2, result.TotalCount);
            Assert.All(result.SearchItems, item => Assert.Contains("imson", item.Name, StringComparison.OrdinalIgnoreCase));
        }
    }
}