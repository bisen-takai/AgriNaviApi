using System.Threading;
using System.Threading.Tasks;
using AgriNaviApi.Api.Controllers;
using AgriNaviApi.Application.Interfaces;
using AgriNaviApi.Application.Requests.Colors;
using AgriNaviApi.Application.Responses;
using AgriNaviApi.Application.Responses.Colors;
using AgriNaviApi.Shared.Enums;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class ColorsControllerTests
{
    private readonly Mock<IColorService> _colorServiceMock;
    private readonly ColorsController _controller;

    public ColorsControllerTests()
    {
        _colorServiceMock = new Mock<IColorService>();
        _controller = new ColorsController(_colorServiceMock.Object);
    }

    /// <summary>
    /// CreateColor: �J���[�V�K�o�^���ACreatedAtActionResult���ԋp����邱�Ƃ����؂���
    /// </summary>
    [Fact]
    public async Task CreateColor_ReturnsCreatedAtActionResult()
    {
        var request = new ColorCreateRequest { Name = "Red", RedValue = 255, GreenValue = 0, BlueValue = 0 };
        var response = new ColorCreateResponse { Id = 1, Name = "Red", RedValue = 255, GreenValue = 0, BlueValue = 0 };
        _colorServiceMock.Setup(s => s.CreateColorAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(response);

        var result = await _controller.CreateColor(request, CancellationToken.None);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(response, createdResult.Value);
    }

    /// <summary>
    /// GetColorById: �w��ID�̃J���[�擾���AOkObjectResult���ԋp����邱�Ƃ����؂���
    /// </summary>
    [Fact]
    public async Task GetColorById_ReturnsOkObjectResult()
    {
        var response = new ColorDetailResponse { Id = 1, Name = "Red", RedValue = 255, GreenValue = 0, BlueValue = 0 };
        _colorServiceMock.Setup(s => s.GetColorByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(response);

        var result = await _controller.GetColorById(1, CancellationToken.None);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(response, okResult.Value);
    }

    /// <summary>
    /// UpdateColor: �J���[�X�V���AOkObjectResult���ԋp����邱�Ƃ����؂���
    /// </summary>
    [Fact]
    public async Task UpdateColor_ReturnsOkObjectResult()
    {
        var request = new ColorUpdateRequest { Name = "Blue", RedValue = 0, GreenValue = 0, BlueValue = 255 };
        var response = new ColorUpdateResponse { Id = 1, Name = "Blue", RedValue = 0, GreenValue = 0, BlueValue = 255 };
        _colorServiceMock.Setup(s => s.UpdateColorAsync(1, request, It.IsAny<CancellationToken>())).ReturnsAsync(response);

        var result = await _controller.UpdateColor(1, request, CancellationToken.None);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(response, okResult.Value);
    }

    /// <summary>
    /// DeleteColor: �J���[�폜���AOkObjectResult���ԋp����邱�Ƃ����؂���
    /// </summary>
    [Fact]
    public async Task DeleteColor_ReturnsOkObjectResult()
    {
        var response = new DeleteResponse { Id = 1, IsDeleted = true, Message = "Deleted", DeletedAt = System.DateTime.Now };
        _colorServiceMock.Setup(s => s.DeleteColorAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(response);

        var result = await _controller.DeleteColor(1, CancellationToken.None);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(response, okResult.Value);
    }

    /// <summary>
    /// SearchColors: �J���[�������AOkObjectResult���ԋp����邱�Ƃ����؂���
    /// </summary>
    [Fact]
    public async Task SearchColors_ReturnsOkObjectResult()
    {
        var request = new ColorSearchRequest { SearchName = "Red", Page = 1, PageSize = 10, SearchMatchType = SearchMatchType.Exact };
        var response = new SearchResponse<ColorListItemResponse>
        {
            SearchItems = new[] { new ColorListItemResponse { Id = 1, Name = "Red", RedValue = 255, GreenValue = 0, BlueValue = 0 } },
            TotalCount = 1,
            Page = 1,
            PageSize = 10
        };
        _colorServiceMock.Setup(s => s.SearchColorAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(response);

        var result = await _controller.SearchColors(request, CancellationToken.None);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(response, okResult.Value);
    }
}
