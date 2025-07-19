using Xunit;
using Microsoft.EntityFrameworkCore;
using AgriNaviApi.Application.Services;
using AgriNaviApi.Infrastructure.Persistence.Contexts;
using AgriNaviApi.Infrastructure.Persistence.Entities;
using AgriNaviApi.Application.Requests.Colors;
using AgriNaviApi.Shared.Exceptions;
using AutoMapper;
using AgriNaviApi.Shared.Enums;

public class ColorServiceTests
{
    private AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "ColorServiceTestDb")
            .Options;
        return new AppDbContext(options, null!);
    }

    private IMapper CreateMapper()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<ColorCreateRequest, ColorEntity>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
            cfg.CreateMap<ColorUpdateRequest, ColorEntity>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        });
        return config.CreateMapper();
    }

    /// <summary>
    /// 有効なリクエストでカラーを正常に登録できることを検証する
    /// </summary>
    [Fact]
    public async Task CreateColorAsync_ValidRequest_Success()
    {
        var db = CreateDbContext();
        var mapper = CreateMapper();
        var service = new ColorService(db, mapper);

        var request = new ColorCreateRequest
        {
            Name = "Red",
            RedValue = 255,
            GreenValue = 0,
            BlueValue = 0
        };

        var result = await service.CreateColorAsync(request, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("Red", result.Name);
    }

    /// <summary>
    /// カラー名が重複している場合に例外がスローされることを検証する
    /// </summary>
    [Fact]
    public async Task CreateColorAsync_DuplicateName_ThrowsException()
    {
        var db = CreateDbContext();
        var mapper = CreateMapper();
        var service = new ColorService(db, mapper);

        db.Colors.Add(new ColorEntity { Name = "Blue" });
        await db.SaveChangesAsync(CancellationToken.None);

        var request = new ColorCreateRequest
        {
            Name = "Blue",
            RedValue = 0,
            GreenValue = 0,
            BlueValue = 255
        };

        await Assert.ThrowsAsync<DuplicateEntityException>(() =>
            service.CreateColorAsync(request, CancellationToken.None));
    }

    /// <summary>
    /// カラー名が空・空白・nullの場合に例外がスローされることを検証する
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task CreateColorAsync_InvalidName_ThrowsException(string name)
    {
        var db = CreateDbContext();
        var mapper = CreateMapper();
        var service = new ColorService(db, mapper);

        var request = new ColorCreateRequest
        {
            Name = name,
            RedValue = 100,
            GreenValue = 100,
            BlueValue = 100
        };

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.CreateColorAsync(request, CancellationToken.None));
    }

    /// <summary>
    /// RGB値が範囲外の場合に例外がスローされることを検証する
    /// </summary>
    [Theory]
    [InlineData(-1, 100, 100)]
    [InlineData(100, 256, 100)]
    [InlineData(100, 100, -5)]
    public async Task CreateColorAsync_InvalidRgb_ThrowsException(int r, int g, int b)
    {
        var db = CreateDbContext();
        var mapper = CreateMapper();
        var service = new ColorService(db, mapper);

        var request = new ColorCreateRequest
        {
            Name = "TestColor",
            RedValue = r,
            GreenValue = g,
            BlueValue = b
        };

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.CreateColorAsync(request, CancellationToken.None));
    }

    /// <summary>
    /// 有効なリクエストでカラーを正常に更新できることを検証する
    /// </summary>
    [Fact]
    public async Task UpdateColorAsync_ValidRequest_Success()
    {
        var db = CreateDbContext();
        var mapper = CreateMapper();
        var service = new ColorService(db, mapper);

        var color = new ColorEntity { Name = "Green" };
        db.Colors.Add(color);
        await db.SaveChangesAsync(CancellationToken.None);

        var updateRequest = new ColorUpdateRequest
        {
            Name = "LightGreen",
            RedValue = 100,
            GreenValue = 200,
            BlueValue = 100
        };

        var result = await service.UpdateColorAsync(color.Id, updateRequest, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("LightGreen", result.Name);
    }

    /// <summary>
    /// 存在しないIDで更新した場合に例外がスローされることを検証する
    /// </summary>
    [Fact]
    public async Task UpdateColorAsync_NotFound_ThrowsException()
    {
        var db = CreateDbContext();
        var mapper = CreateMapper();
        var service = new ColorService(db, mapper);

        var updateRequest = new ColorUpdateRequest
        {
            Name = "Yellow",
            RedValue = 255,
            GreenValue = 255,
            BlueValue = 0
        };

        await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            service.UpdateColorAsync(999, updateRequest, CancellationToken.None));
    }

    /// <summary>
    /// 更新時にカラー名が重複している場合に例外がスローされることを検証する
    /// </summary>
    [Fact]
    public async Task UpdateColorAsync_DuplicateName_ThrowsException()
    {
        var db = CreateDbContext();
        var mapper = CreateMapper();
        var service = new ColorService(db, mapper);

        var color1 = new ColorEntity { Name = "Orange" };
        var color2 = new ColorEntity { Name = "Yellow" };
        db.Colors.Add(color1);
        db.Colors.Add(color2);
        await db.SaveChangesAsync(CancellationToken.None);

        var updateRequest = new ColorUpdateRequest
        {
            Name = "Yellow",
            RedValue = 255,
            GreenValue = 200,
            BlueValue = 0
        };

        await Assert.ThrowsAsync<DuplicateEntityException>(() =>
            service.UpdateColorAsync(color1.Id, updateRequest, CancellationToken.None));
    }

    /// <summary>
    /// 有効なリクエストでカラーを正常に削除できることを検証する
    /// </summary>
    [Fact]
    public async Task DeleteColorAsync_ValidRequest_Success()
    {
        var db = CreateDbContext();
        var mapper = CreateMapper();
        var service = new ColorService(db, mapper);

        var color = new ColorEntity { Name = "Purple" };
        db.Colors.Add(color);
        await db.SaveChangesAsync(CancellationToken.None);

        var result = await service.DeleteColorAsync(color.Id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.True(result.IsDeleted);
    }

    /// <summary>
    /// 存在しないIDで削除した場合に例外がスローされることを検証する
    /// </summary>
    [Fact]
    public async Task DeleteColorAsync_NotFound_ThrowsException()
    {
        var db = CreateDbContext();
        var mapper = CreateMapper();
        var service = new ColorService(db, mapper);

        await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            service.DeleteColorAsync(999, CancellationToken.None));
    }

    /// <summary>
    /// カラー名で検索した場合に正しく検索結果が返ることを検証する
    /// </summary>
    [Fact]
    public async Task SearchColorAsync_ByName_Success()
    {
        var db = CreateDbContext();
        var mapper = CreateMapper();
        var service = new ColorService(db, mapper);

        db.Colors.Add(new ColorEntity { Name = "Red" });
        db.Colors.Add(new ColorEntity { Name = "Blue" });
        db.Colors.Add(new ColorEntity { Name = "Green" });
        await db.SaveChangesAsync(CancellationToken.None);

        var searchRequest = new ColorSearchRequest
        {
            SearchName = "Red",
            SearchMatchType = SearchMatchType.Exact,
            Page = 1
        };

        var result = await service.SearchColorAsync(searchRequest, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Single(result.SearchItems);
        Assert.Equal("Red", result.SearchItems.First().Name);
    }

    /// <summary>
    /// 検索条件に一致しない場合に空の検索結果が返ることを検証する
    /// </summary>
    [Fact]
    public async Task SearchColorAsync_NoMatch_ReturnsEmpty()
    {
        var db = CreateDbContext();
        var mapper = CreateMapper();
        var service = new ColorService(db, mapper);

        db.Colors.Add(new ColorEntity { Name = "Red" });
        await db.SaveChangesAsync(CancellationToken.None);

        var searchRequest = new ColorSearchRequest
        {
            SearchName = "Yellow",
            SearchMatchType = SearchMatchType.Exact,
            Page = 1
        };

        var result = await service.SearchColorAsync(searchRequest, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Empty(result.SearchItems);
    }
}