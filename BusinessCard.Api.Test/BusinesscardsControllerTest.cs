using Xunit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BusinessCard.core.IServieces;
using AutoMapper;
using BusinessCard.Api.Controllers;
using System.Collections.Generic;
using BusinessCard.core.DTO.BusinessCards;
using BusinessCard.core.Data;
using Moq;
using System.Text;
using BusinessCard.core.IReposetory;
using Microsoft.AspNetCore.Http;
namespace BusinessCard.Api.Test
{
    public class BusinesscardsControllerTest
    {

        private readonly BusinesscardsController _controller;
        private readonly Mock<IBusinessCardServiece> _serviceMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
      

        public BusinesscardsControllerTest()
        {
            _controller = new BusinesscardsController(_serviceMock.Object, _mapperMock.Object);
        }

        // Test Getbusinesscards method
        [Fact]
        public async Task Getbusinesscards_ReturnsOk_WithListOfBusinessCards()
        {
            // Arrange
            var businessCards = new List<Businesscards> { new Businesscards() }; // mock business card data
            var businessCardsDto = new List<GetBusinessCardDto> { new GetBusinessCardDto() };

            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(businessCards);
            _mapperMock.Setup(m => m.Map<List<GetBusinessCardDto>>(It.IsAny<List<Businesscards>>())).Returns(businessCardsDto);

            // Act
            var result = await _controller.Getbusinesscards();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<GetBusinessCardDto>>(okResult.Value);
            Assert.Single(returnValue);
        }


        // Test GetBusinesscards by id method
        [Fact]
        public async Task GetBusinesscards_ReturnsBusinessCard_WhenValidId()
        {
            // Arrange
            var businessCard = new Businesscards { Id = 1 };
            var businessCardDto = new GetBusinessCardDto { Id = 1 };

            _serviceMock.Setup(s => s.GetAsync(1)).ReturnsAsync(businessCard);
            _mapperMock.Setup(m => m.Map<GetBusinessCardDto>(It.IsAny<Businesscards>())).Returns(businessCardDto);

            // Act
            var result = await _controller.GetBusinesscards(1);

            // Assert
            var okResult = Assert.IsType<ActionResult<GetBusinessCardDto>>(result);
            Assert.Equal(1, result.Value.Id);
        }

        // Test PutBusinesscards method (Update)
        [Fact]
        public async Task PutBusinesscards_ReturnsOk_WhenUpdateIsSuccessful()
        {
            // Arrange
            var updateDto = new UpdateBusinessCardDto
            {
                Id = 1,
                Name = "Ahmad",
                Gender = "M",
                Email = "Islam@gmail.com",
                DateOfBirth = DateTime.Now,
                Photo = "Issla.PNG",
                Address = "Amman",
                Phone = "0785400139",
                Notes = "Note1",
                UpdatedAt = DateTime.Now,
                CreatedAt = DateTime.Now,
               
            };
            var businessCard = new Businesscards
            {
                Id = 1,
                Name = "Islam",
                Gender = "M",
                Email = "Islam@gmail.com",
                DateOfBirth = DateTime.Now,
                Photo = "Issla.PNG",
                Address = "Amman",
                Phone = "0785400139",
                Notes = "Note1",
                UpdatedAt = DateTime.Now,
                CreatedAt = DateTime.Now,
               
            };

            _serviceMock.Setup(s => s.GetAsync(1)).ReturnsAsync(businessCard);
            _serviceMock.Setup(s => s.UpdateAsync(businessCard)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.PutBusinesscards(1, updateDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }


        // Test PostBusinesscards method (Create)
        [Fact]
        public async Task PostBusinesscards_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var createDto = new CreateBusinessCardDto();
            var businessCard = new Businesscards { Id = 1 ,Name= "Ahmad" ,Gender="M",Email="Islam@gmail.com",
                DateOfBirth=DateTime.Now,Photo="Issla.PNG",Address="Amman",
                Phone="0785400139",Notes="Note1",
                UpdatedAt=DateTime.Now,CreatedAt=DateTime.Now,UserId =1};

            _mapperMock.Setup(m => m.Map<Businesscards>(It.IsAny<CreateBusinessCardDto>())).Returns(businessCard);
            _serviceMock.Setup(s => s.AddAsync(businessCard)).ReturnsAsync(businessCard);

            // Act
            var result = await _controller.PostBusinesscards(createDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal("GetBusinesscards", createdAtActionResult.ActionName);
        }

        // Test DeleteBusinesscards method
        [Fact]
        public async Task DeleteBusinesscards_ReturnsNoContent_WhenDeleteIsSuccessful()
        {
            // Arrange
            var businessCard = new Businesscards { Id = 1 };

            _serviceMock.Setup(s => s.GetAsync(1)).ReturnsAsync(businessCard);
            _serviceMock.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteBusinesscards(1);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, noContentResult.StatusCode);
        }

        // Test FilterBusinesscards method
        [Fact]
        public async Task FilterBusinesscards_ReturnsOk_WithFilteredBusinessCards()
        {
            // Arrange
            var filteredBusinessCards = new List<Businesscards>
            {
              new Businesscards { Id = 1, Name = "Ahmad", Gender = "M", Email = "ahmad@example.com" }
            };

            var filteredBusinessCardsDto = new List<GetBusinessCardDto>
            {
                new GetBusinessCardDto { Id = 1, Name = "Ahmad", Gender = "M", Email = "ahmad@example.com" }
            };

            string name = "Ahmad";
            DateTime? dob = null; // You can adjust this based on your requirements
            string phone = null;
            string gender = null;
            string email = null;

            _serviceMock.Setup(s => s.FilterAsync(name, dob, phone, gender, email)).ReturnsAsync(filteredBusinessCards);
            _mapperMock.Setup(m => m.Map<List<GetBusinessCardDto>>(It.IsAny<List<Businesscards>>())).Returns(filteredBusinessCardsDto);

            // Act
            var result = await _controller.FilterBusinesscards(name, dob, phone, gender, email);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<GetBusinessCardDto>>(okResult.Value);
            Assert.Single(returnValue);
            Assert.Equal("Ahmad", returnValue[0].Name);
        }



        // Test FilterBusinesscards method for not found case
        [Fact]
        public async Task FilterBusinesscards_ReturnsNotFound_WhenNoMatchingBusinessCards()
        {
            // Arrange
            var name = "Nonexistent Name";
            DateTime? dob = null; // You can adjust this based on your requirements
            string phone = null;
            string gender = null;
            string email = null;

            _serviceMock.Setup(s => s.FilterAsync(name, dob, phone, gender, email)).ReturnsAsync(new List<Businesscards>()); // No matches

            // Act
            var result = await _controller.FilterBusinesscards(name, dob, phone, gender, email);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("No matching business cards found.", notFoundResult.Value);
        }

        // Test ExportToCsv method
        [Fact]
        public async Task ExportToCsv_ReturnsFileContentResult()
        {
            // Arrange
            var csvData = new byte[] { 1, 2, 3 };
            _serviceMock.Setup(s => s.ExportToCsvAsync()).ReturnsAsync(csvData);

            // Act
            var result = await _controller.ExportToCsv();

            // Assert
            var fileResult = Assert.IsType<FileContentResult>(result);
            Assert.Equal("text/csv", fileResult.ContentType);
            Assert.Equal("businesscards.csv", fileResult.FileDownloadName);
        }

        // Test ExportToXml method
        [Fact]
        public async Task ExportToXml_ReturnsFileContentResult()
        {
            // Arrange
            var xmlData = Encoding.UTF8.GetBytes("<BusinessCards>\r\n<BusinessCard>\r\n<Id>16661</Id>\r\n<Name>IslamNew</Name>\r\n<Phone>0785400139</Phone>\r\n<Email>Islam@gmail.com</Email>\r\n<DateOfBirth>1997-02-10</DateOfBirth>\r\n<Gender>M</Gender>\r\n<Address>Aaman</Address>\r\n<Notes>Not1</Notes>\r\n<Photo>Islam.png</Photo>\r\n<UserId>1</UserId>\r\n</BusinessCard>\r\n<BusinessCard>\r\n<Id>16662</Id>\r\n<Name>IslamNew</Name>\r\n<Phone>785400139</Phone>\r\n<Email>IslamNew@gmail.com</Email>\r\n<DateOfBirth>1997-02-10</DateOfBirth>\r\n<Gender>M</Gender>\r\n<Address>Aaman</Address>\r\n<Notes>Not1</Notes>\r\n<Photo>Islam.png</Photo>\r\n<UserId>1</UserId>\r\n</BusinessCard>\r\n<BusinessCard>\r\n<Id>16663</Id>\r\n<Name>Islam</Name>\r\n<Phone>785400139</Phone>\r\n<Email>Islam@gmail.com</Email>\r\n<DateOfBirth>1997-02-10</DateOfBirth>\r\n<Gender>M</Gender>\r\n<Address>Aaman</Address>\r\n<Notes>Not2</Notes>\r\n<Photo>Islam.png</Photo>\r\n<UserId>1</UserId>\r\n</BusinessCard>\r\n</BusinessCards>"); // Sample XML data
            _serviceMock.Setup(s => s.ExportToXmlAsync()).ReturnsAsync(xmlData);

            // Act
            var result = await _controller.ExportToXml();

            // Assert
            var fileResult = Assert.IsType<FileContentResult>(result);
            Assert.Equal("application/xml", fileResult.ContentType);
            Assert.Equal("businesscards.xml", fileResult.FileDownloadName);
            Assert.Equal(xmlData, fileResult.FileContents); // Optionally, check if the file contents are correct
        }

        // Test ImportCsv method (File uploaded)
        [Fact]
        public async Task ImportCsv_ReturnsOk_WhenFileIsUploaded()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            var fileName = "test.csv";
            var fileContent = "Name,Gender,Email\nJohn Doe,M,johndoe@example.com";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            await writer.WriteAsync(fileContent);
            await writer.FlushAsync();
            stream.Position = 0; // Reset the stream position to the beginning
            mockFile.Setup(f => f.OpenReadStream()).Returns(stream);
            mockFile.Setup(f => f.FileName).Returns(fileName);
            mockFile.Setup(f => f.Length).Returns(stream.Length);

            // Act
            var result = await _controller.ImportCsv(mockFile.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("CSV file imported successfully.", okResult.Value);
            _serviceMock.Verify(s => s.ImportFromCsvAsync(It.IsAny<Stream>()), Times.Once);
        }


        // Test ImportCsv method (No file uploaded)
        [Fact]
        public async Task ImportCsv_ReturnsBadRequest_WhenNoFileUploaded()
        {
            // Arrange
            IFormFile mockFile = null; // Simulate no file

            // Act
            var result = await _controller.ImportCsv(mockFile);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("No file uploaded.", badRequestResult.Value);
        }

        // Test ImportXml method (File uploaded)
        [Fact]
        public async Task ImportXml_ReturnsOk_WhenFileIsUploaded()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            var fileName = "test.xml";
            var fileContent = "<BusinessCards>\r\n<BusinessCard>\r\n<Id>16661</Id>\r\n<Name>IslamNew</Name>\r\n<Phone>0785400139</Phone>\r\n<Email>Islam@gmail.com</Email>\r\n<DateOfBirth>1997-02-10</DateOfBirth>\r\n<Gender>M</Gender>\r\n<Address>Aaman</Address>\r\n<Notes>Not1</Notes>\r\n<Photo>Islam.png</Photo>\r\n<UserId>1</UserId>\r\n</BusinessCard>\r\n<BusinessCard>\r\n<Id>16662</Id>\r\n<Name>IslamNew</Name>\r\n<Phone>785400139</Phone>\r\n<Email>IslamNew@gmail.com</Email>\r\n<DateOfBirth>1997-02-10</DateOfBirth>\r\n<Gender>M</Gender>\r\n<Address>Aaman</Address>\r\n<Notes>Not1</Notes>\r\n<Photo>Islam.png</Photo>\r\n<UserId>1</UserId>\r\n</BusinessCard>\r\n<BusinessCard>\r\n<Id>16663</Id>\r\n<Name>Islam</Name>\r\n<Phone>785400139</Phone>\r\n<Email>Islam@gmail.com</Email>\r\n<DateOfBirth>1997-02-10</DateOfBirth>\r\n<Gender>M</Gender>\r\n<Address>Aaman</Address>\r\n<Notes>Not2</Notes>\r\n<Photo>Islam.png</Photo>\r\n<UserId>1</UserId>\r\n</BusinessCard>\r\n</BusinessCards>";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            await writer.WriteAsync(fileContent);
            await writer.FlushAsync();
            stream.Position = 0; // Reset the stream position to the beginning
            mockFile.Setup(f => f.OpenReadStream()).Returns(stream);
            mockFile.Setup(f => f.FileName).Returns(fileName);
            mockFile.Setup(f => f.Length).Returns(stream.Length);

            // Act
            var result = await _controller.ImportXml(mockFile.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("XML file imported successfully.", okResult.Value);
            _serviceMock.Verify(s => s.ImportFromXmlAsync(It.IsAny<Stream>()), Times.Once);
        }

        // Test ImportXml method (No file uploaded)
        [Fact]
        public async Task ImportXml_ReturnsBadRequest_WhenNoFileUploaded()
        {
            // Arrange
            IFormFile mockFile = null; // Simulate no file

            // Act
            var result = await _controller.ImportXml(mockFile);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("No file uploaded.", badRequestResult.Value);
        }



    }
}