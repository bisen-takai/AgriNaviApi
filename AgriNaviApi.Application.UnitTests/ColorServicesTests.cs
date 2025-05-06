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
    /// 主にビジネスロジックなどサービス層の動作を検証することに焦点を当てています。
    /// メッセージ内容の確認はコントローラ側で行うため、割愛しています。
    /// </summary>
    public class ColorServicesTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IUuidGenerator> _uuidGeneratorMock;
        private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;

        public ColorServicesTests()
        {
            // AutoMapper の設定（ColorProfile を使用）
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ColorProfile>();
            });
            _mapper = config.CreateMapper();

            // IUuidGenerator のモック設定
            _uuidGeneratorMock = new Mock<IUuidGenerator>();
            _uuidGeneratorMock.Setup(x => x.GenerateUuid())
                .Returns(Guid.Parse("11111111-1111-1111-1111-111111111111"));

            // IDateTimeProvider のモック設定
            _dateTimeProviderMock = new Mock<IDateTimeProvider>();
            _dateTimeProviderMock.Setup(x => x.UtcNow)
                .Returns(new DateTime(2025, 1, 1));
        }

        /// <summary>
        /// InMemory データベースを利用して、AppDbContextを生成するためのヘルパー
        /// </summary>
        private AppDbContext CreateInMemoryContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                            .UseInMemoryDatabase(databaseName: dbName)
                            .Options;
            return new AppDbContext(options);
        }

        /// <summary>
        /// 正常に登録できることを確認
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CreateColorAsync_Normal()
        {
            // データベース名は他テストケースと被らないようにUUIDとする
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

            // コンテキスト上に登録されたエンティティが正しく設定されているか確認
            var createdEntity = await context.Colors.FirstOrDefaultAsync();
            Assert.NotNull(createdEntity);
            Assert.Equal(Guid.Parse("11111111-1111-1111-1111-111111111111"), createdEntity.Uuid);
            Assert.Equal(new DateTime(2025, 1, 1), createdEntity.CreatedAt);
            Assert.Equal(new DateTime(2025, 1, 1), createdEntity.LastUpdatedAt);
        }

        /// <summary>
        /// 同じカラー名は登録できないことを確認
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CreateColorAsync_Abnormality()
        {
            // データベース名は他テストケースと被らないようにUUIDとする
            string dbName = Guid.NewGuid().ToString();
            using var context = CreateInMemoryContext(dbName);

            // 事前に同じカラー名 ("Blue") のエンティティを登録しておく
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
                Name = "Blue", // 重複する名前
                RedValue = 10,
                GreenValue = 10,
                BlueValue = 10
            };

            // 同じ名前で登録しようとすると DuplicateEntityException が発生することを検証する
            await Assert.ThrowsAsync<DuplicateEntityException>(
                async () => await service.CreateColorAsync(request));
        }

        /// <summary>
        /// 正常に詳細情報を取得できることを確認
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetColorByIdAsync_Normal()
        {
            // データベース名は他テストケースと被らないようにUUIDとする
            string dbName = Guid.NewGuid().ToString();
            using var context = CreateInMemoryContext(dbName);

            // テスト用のエンティティを作成してDBに追加
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
        /// 存在しないIDの詳細情報を取得した場合の確認
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetColorByIdAsync_Abnormality()
        {
            // データベース名は他テストケースと被らないようにUUIDとする
            string dbName = Guid.NewGuid().ToString();
            using var context = CreateInMemoryContext(dbName);

            var service = new ColorService(
                context,
                _uuidGeneratorMock.Object,
                _mapper,
                _dateTimeProviderMock.Object);

            int nonExistingId = 999; // 存在しないID

            // KeyNotFoundExceptionとなることを確認
            await Assert.ThrowsAsync<KeyNotFoundException>(
                async () => await service.GetColorByIdAsync(nonExistingId));
        }

        /// <summary>
        /// 正常に更新できることを確認
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UpdateColorAsync_Normal()
        {
            // データベース名は他テストケースと被らないようにUUIDとする
            string dbName = Guid.NewGuid().ToString();
            using var context = CreateInMemoryContext(dbName);

            // 初期状態のエンティティを作成してDBに登録
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

            // 更新リクエストを作成（名称やRGB値の変更）
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

            // 更新後のLastUpdatedAtがモックで設定した日時になっているか確認
            var updatedEntity = await context.Colors.FindAsync(initialColor.Id);
            Assert.Equal(new DateTime(2025, 1, 1), updatedEntity.LastUpdatedAt);
            // 更新後のCreatedAtが更新されていないことを確認
            Assert.Equal(new DateTime(2020, 1, 1), updatedEntity.CreatedAt);
        }

        /// <summary>
        /// 存在しないIDの場合を更新した場合の確認
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UpdateColorAsync_Abnormality()
        {
            // データベース名は他テストケースと被らないようにUUIDとする
            string dbName = Guid.NewGuid().ToString();
            using var context = CreateInMemoryContext(dbName);

            var service = new ColorService(
                context,
                _uuidGeneratorMock.Object,
                _mapper,
                _dateTimeProviderMock.Object);

            var updateRequest = new ColorUpdateRequest
            {
                Id = 999, // 存在しないID
                Name = "Nonexistent",
                RedValue = 50,
                GreenValue = 50,
                BlueValue = 50
            };

            // KeyNotFoundExceptionとなることを確認
            await Assert.ThrowsAsync<KeyNotFoundException>(
                async () => await service.UpdateColorAsync(updateRequest));
        }

        /// <summary>
        /// 正常に削除ができることを確認
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DeleteColorAsync_Normal()
        {
            // データベース名は他テストケースと被らないようにUUIDとする
            string dbName = Guid.NewGuid().ToString();
            using var context = CreateInMemoryContext(dbName);

            // テスト用エンティティをDBに追加
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
            // 削除メッセージにカラー名が含まれていることを確認
            Assert.Contains(testColor.Name, deleteResult.Message);

            // DB上からエンティティが削除されているか確認
            var deletedEntity = await context.Colors.FindAsync(testColor.Id);
            Assert.Null(deletedEntity);
        }

        /// <summary>
        /// 存在しないIDの場合を削除した場合の確認
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DeleteColorAsync_Abnormality()
        {
            // データベース名は他テストケースと被らないようにUUIDとする
            string dbName = Guid.NewGuid().ToString();
            using var context = CreateInMemoryContext(dbName);

            var service = new ColorService(
                context,
                _uuidGeneratorMock.Object,
                _mapper,
                _dateTimeProviderMock.Object);

            // InvalidOperationExceptionとなることを確認
            await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await service.DeleteColorAsync(999));
        }

        /// <summary>
        /// 全件検索した場合の確認
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task SearchColorAsync_AllSearch()
        {
            // データベース名は他テストケースと被らないようにUUIDとする
            string dbName = Guid.NewGuid().ToString();
            using var context = CreateInMemoryContext(dbName);

            // テスト用のエンティティを登録
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
                SearchColorName = "", // 条件未指定（空文字でも null でも可）
                SearchMatchType = SearchMatchType.None
            };

            var result = await service.SearchColorAsync(searchRequest);

            Assert.NotNull(result);
            Assert.Equal(3, result.TotalCount);
            Assert.Equal(3, result.SearchItems.Count());
        }

        /// <summary>
        /// 完全一致検索した場合の確認
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task SearchColorAsync_ExactMatch()
        {
            // データベース名は他テストケースと被らないようにUUIDとする
            string dbName = Guid.NewGuid().ToString();
            using var context = CreateInMemoryContext(dbName);

            // "Blue" と "BlueSky" を登録（完全一致なら "Blue" のみが該当）
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
        /// 前方一致検索した場合の確認
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task SearchColorAsync_PrefixMatch()
        {
            // データベース名は他テストケースと被らないようにUUIDとする
            string dbName = Guid.NewGuid().ToString();
            using var context = CreateInMemoryContext(dbName);

            // "Blue" と "BlueSky" のみが前方一致でヒットするはず
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
        /// 後方一致検索した場合の確認
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task SearchColorAsync_SuffixMatch()
        {
            // データベース名は他テストケースと被らないようにUUIDとする
            string dbName = Guid.NewGuid().ToString();
            using var context = CreateInMemoryContext(dbName);

            // "SkyBlue" と "NavyBlue" は末尾が "Blue" で一致するはず
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
        /// 部分一致検索した場合の確認
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task SearchColorAsync_PartialMatch()
        {
            // データベース名は他テストケースと被らないようにUUIDとする
            string dbName = Guid.NewGuid().ToString();
            using var context = CreateInMemoryContext(dbName);

            // "Crimson" と "CrimsonRed" は "imson" を含むのでヒットする
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