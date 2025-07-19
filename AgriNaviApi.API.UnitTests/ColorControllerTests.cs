using AgriNaviApi.API.Controllers;
using AgriNaviApi.Application.Responses.Colors;
using AgriNaviApi.Application.Interfaces;
using AgriNaviApi.Application.Requests.Colors;
using AgriNaviApi.Common.Enums;
using AgriNaviApi.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AgriNaviApi.API.UnitTests
{
    public class ColorControllerTests
    {
        private readonly Mock<IColorService> _colorServiceMock;
        private readonly ColorController _controller;

        public ColorControllerTests()
        {
            _colorServiceMock = new Mock<IColorService>();
            _controller = new ColorController(_colorServiceMock.Object);
        }

        /// <summary>
        /// 正常に登録した場合の確認
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CreateColor_Normal()
        {
            var request = new ColorCreateRequest
            {
                Name = "Blue",
                RedValue = 0,
                GreenValue = 0,
                BlueValue = 255
            };
            var createdDto = new ColorCreateDto
            {
                Id = 1,
                Name = "Blue"
                // サービス層でテスト済みなのでその他省略
            };

            _colorServiceMock
                .Setup(s => s.CreateColorAsync(request))
                .ReturnsAsync(createdDto);

            var actionResult = await _controller.CreateColor(request);

            var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            Assert.Equal(nameof(_controller.GetColorById), createdResult.ActionName);
            var returnedDto = Assert.IsType<ColorCreateDto>(createdResult.Value);
            Assert.Equal(createdDto.Id, returnedDto.Id);
            Assert.Equal("Blue", returnedDto.Name);
        }

        /// <summary>
        /// 登録済みのカラー名を登録した場合の確認
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CreateColor_DuplicateEntityExceptionThrown()
        {
            var request = new ColorCreateRequest
            {
                Name = "Blue",
                RedValue = 0,
                GreenValue = 0,
                BlueValue = 255
            };
            var exceptionMessage = "「カラー」は既に利用されています。";
            _colorServiceMock
                .Setup(s => s.CreateColorAsync(request))
                .ThrowsAsync(new DuplicateEntityException(exceptionMessage));

            var actionResult = await _controller.CreateColor(request);

            var conflictResult = Assert.IsType<ConflictObjectResult>(actionResult.Result);
            var resultValue = conflictResult.Value;
            var messageProperty = resultValue.GetType().GetProperty("message");
            Assert.NotNull(messageProperty);
            var actualMessage = messageProperty.GetValue(resultValue, null)?.ToString();
            Assert.Equal(exceptionMessage, actualMessage);
        }

        /// <summary>
        /// 正常に詳細情報を取得した場合の確認
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetColorById_Normal()
        {
            // Arrange
            var detailDto = new ColorDetailDto
            {
                Id = 1,
                Name = "Blue"
                // サービス層でテスト済みなのでその他省略
            };

            _colorServiceMock
                .Setup(s => s.GetColorByIdAsync(1))
                .ReturnsAsync(detailDto);

            var actionResult = await _controller.GetColorById(1);

            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnedDto = Assert.IsType<ColorDetailDto>(okResult.Value);
            Assert.Equal(1, returnedDto.Id);
            Assert.Equal("Blue", returnedDto.Name);
        }

        /// <summary>
        /// カラーが見つからなかった場合の確認
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetColorById_KeyNotFoundExceptionThrown()
        {
            // Arrange
            var exceptionMessage = "カラーが見つかりません。";
            _colorServiceMock
                .Setup(s => s.GetColorByIdAsync(1))
                .ThrowsAsync(new KeyNotFoundException(exceptionMessage));

            var actionResult = await _controller.GetColorById(1);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            var resultValue = notFoundResult.Value;
            var messageProperty = resultValue.GetType().GetProperty("message");
            Assert.NotNull(messageProperty);
            var actualMessage = messageProperty.GetValue(resultValue, null)?.ToString();
            Assert.Equal(exceptionMessage, actualMessage);
        }

        /// <summary>
        /// 正常に更新した場合の確認
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UpdateColor_Normal()
        {
            var updateRequest = new ColorUpdateRequest
            {
                Id = 1,
                Name = "Updated Blue",
                // サービス層でテスト済みなのでその他省略
            };
            var updateDto = new ColorUpdateResponse
            {
                Id = 1,
                Name = "Updated Blue"
                // サービス層でテスト済みなのでその他省略
            };

            _colorServiceMock
                .Setup(s => s.UpdateColorAsync(updateRequest))
                .ReturnsAsync(updateDto);

            var actionResult = await _controller.UpdateColor(1, updateRequest);

            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnedDto = Assert.IsType<ColorUpdateResponse>(okResult.Value);
            Assert.Equal("Updated Blue", returnedDto.Name);
        }

        /// <summary>
        /// 更新するカラーが見つからなかった場合の確認
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UpdateColor_NotFoundObjectResultThrown()
        {
            var updateRequest = new ColorUpdateRequest { Id = 1, Name = "Updated Blue" };
            var exceptionMessage = "カラーが見つかりません。";
            _colorServiceMock
                .Setup(s => s.UpdateColorAsync(updateRequest))
                .ThrowsAsync(new KeyNotFoundException(exceptionMessage));

            var actionResult = await _controller.UpdateColor(1, updateRequest);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            var resultValue = notFoundResult.Value;
            var messageProperty = resultValue.GetType().GetProperty("message");
            Assert.NotNull(messageProperty);
            var actualMessage = messageProperty.GetValue(resultValue, null)?.ToString();
            Assert.Equal(exceptionMessage, actualMessage);
        }

        /// <summary>
        /// 正常に削除した場合の確認
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DeleteColor_Normal()
        {
            var deleteDto = new ColorDeleteDto
            {
                Id = 1,
                IsDeleted = true,
                Message = "カラーを削除しました。"
            };

            _colorServiceMock
                .Setup(s => s.DeleteColorAsync(1))
                .ReturnsAsync(deleteDto);

            var actionResult = await _controller.DeleteColor(1);

            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var returnedDto = Assert.IsType<ColorDeleteDto>(okResult.Value);
            Assert.True(returnedDto.IsDeleted);
            Assert.Equal("カラーを削除しました。", returnedDto.Message);
        }

        /// <summary>
        /// 削除するカラーが見つからなかった場合の確認
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DeleteColor_InvalidOperationExceptionThrown()
        {
            var exceptionMessage = "カラーが見つかりません。";
            _colorServiceMock
                .Setup(s => s.DeleteColorAsync(1))
                .ThrowsAsync(new InvalidOperationException(exceptionMessage));

            var actionResult = await _controller.DeleteColor(1);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult);
            var resultValue = notFoundResult.Value;
            var messageProperty = resultValue.GetType().GetProperty("message");
            Assert.NotNull(messageProperty);
            var actualMessage = messageProperty.GetValue(resultValue, null)?.ToString();
            Assert.Equal(exceptionMessage, actualMessage);
        }

        /// <summary>
        /// 検索した場合の確認
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task SearchColors_Normal()
        {
            var searchResultDto = new ColorSearchDto
            {
                TotalCount = 1,
                SearchItems = new List<ColorListItemDto>
                {
                    new ColorListItemDto { Id = 1, Name = "Blue" }
                }
            };

            // どの条件であっても検索処理を呼ぶので、It.IsAny<ColorSearchRequest>()を利用
            _colorServiceMock
                .Setup(s => s.SearchColorAsync(It.IsAny<ColorSearchRequest>()))
                .ReturnsAsync(searchResultDto);

            var actionResult = await _controller.SearchColors("Blue", SearchMatchType.EXACT);

            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var returnedDto = Assert.IsType<ColorSearchDto>(okResult.Value);
            Assert.Equal(1, returnedDto.TotalCount);
            Assert.Single(returnedDto.SearchItems);

            var items = returnedDto.SearchItems.ToList();
            Assert.Equal("Blue", items[0].Name);
        }
    }
}