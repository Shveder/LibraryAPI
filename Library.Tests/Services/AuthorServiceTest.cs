using Bogus;
using Library.API.Controllers;
using Library.Common;
using Library.Core.DTO;
using Library.Core.Models;
using Library.Infrastructure.Services;
using Library.Tests.Infrastructure.Base;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Library.Tests.Services
{
    public class AuthorControllerTest : BaseTest
    {
        private Mock<IAuthorService> _authorServiceMock;
        private AuthorController _controller;

        [SetUp]
        public void SetUp()
        {
            _authorServiceMock = new Mock<IAuthorService>();
            _controller = new AuthorController(_authorServiceMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _controller?.Dispose();
        }

        [Test]
        public async Task GetByIdAsyncTest()
        {
            // Arrange
            var dto = await CreateTestAuthor();
            _authorServiceMock.Setup(s => s.GetByIdAsync(dto.Id)).ReturnsAsync(dto);

            // Act
            var result = await _controller.GetByIdAsync(dto.Id);

            // Assert
            AssertResponseDtoIsSuccess<AuthorDto>(result, true);
            var responseDto = GetResultData<AuthorDto>(result);
            Assert.That(responseDto!.Data!.Name, Is.EqualTo(dto.Name));
            Assert.That(responseDto.Data!.Id, Is.EqualTo(dto.Id));
        }

        [Test]
        public async Task PostAsync_ValidDto_ReturnsSuccessMessageAndData()
        {
            // Arrange
            var authorDto = new AuthorDto { Name = "John Doe" };
            var author = new Author { Name = "John Doe" };
    
            var serviceMock = new Mock<IAuthorService>();
            serviceMock.Setup(s => s.PostAsync(authorDto)).ReturnsAsync(authorDto);
    
            var controller = new AuthorController(serviceMock.Object);
    
            // Act
            var result = await controller.PostAsync(authorDto);
    
            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult, "Expected OkObjectResult");
    
            var response = okResult?.Value as ResponseDto<AuthorDto>;
            Assert.IsNotNull(response, "Expected ResponseDto<AuthorDto>");
            Assert.That(response?.Message, Is.EqualTo(CommonStrings.SuccessResultPost), "Unexpected success message");
        }
      
        [Test]
        public async Task DeleteAsync_ValidId_ReturnsSuccessMessage()
        {
            // Arrange
            var id = Guid.NewGuid();
            var serviceMock = new Mock<IAuthorService>();
            serviceMock.Setup(s => s.DeleteByIdAsync(id)).Returns(Task.CompletedTask);
    
            var controller = new AuthorController(serviceMock.Object);
    
            // Act
            var result = await controller.DeleteAsync(id);
    
            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult, "Expected OkObjectResult");
    
            var response = okResult?.Value as ResponseDto<string>;
            Assert.IsNotNull(response, "Expected ResponseDto<string>");
            Assert.That(response?.Message, Is.EqualTo(CommonStrings.SuccessResultDelete), "Unexpected success message");
        }


        [Test]
        public async Task PutAsyncTest()
        {
            // Arrange
            var dto = await CreateTestAuthor();
            dto.Name = "Updated Name";
            _authorServiceMock.Setup(s => s.PutAsync(dto)).ReturnsAsync(dto);

            // Act
            var result = await _controller.PutAsync(dto);

            // Assert
            AssertResponseDtoIsSuccess<AuthorDto>(result, true);
            var responseDto = GetResultData<AuthorDto>(result);
            Assert.That(responseDto!.Data!.Name, Is.EqualTo("Updated Name"));
        }

        private async Task<AuthorDto> CreateTestAuthor()
        {
            var dto = new Faker<AuthorDto>()
                .RuleFor(a => a.Name, f => f.Name.FullName())
                .Generate();

            // Simulating adding a test author using the mock
            _authorServiceMock.Setup(s => s.PostAsync(dto)).ReturnsAsync(dto);
            await _controller.PostAsync(dto);

            // Adjust the test to ensure the GetAllAsync call is properly handled
            _authorServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<AuthorDto> { dto });

            var result = await _controller.GetAllAsync();
            var okResult = result as OkObjectResult;
            var response = okResult?.Value as ResponseDto<IEnumerable<AuthorDto>>;

            if (response == null || !response.Data.Any())
            {
                throw new InvalidOperationException("No authors were returned.");
            }

            return response.Data.First();
        }
    }
}
