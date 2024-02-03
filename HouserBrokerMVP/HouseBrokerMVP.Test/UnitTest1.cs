using AutoMapper;
using HouseBrokerMVP.Business.DTO;
using HouseBrokerMVP.Business.Services;
using HouseBrokerMVP.Core.Entities;
using HouseBrokerMVP.Core.Repositories;
using Microsoft.AspNetCore.Http;
using Moq;

namespace HouseBrokerMVP.Test
{
    public class UnitTest1
    {
        [Fact]
        public async Task Create_ValidInput_ReturnsPropertyListDto()
        {
            // Arrange
            var propertyRepositoryMock = new Mock<IPropertyReposiotry>();
            var authRepositoryMock = new Mock<IAuthRepository>();
            var mapperMock = new Mock<IMapper>();
            var propertyTypeRepositoryMock = new Mock<IPropertyTypeReposiotry>();
            var propertyImageRepositoryMock = new Mock<IPropertyImageReposiotry>();
            var contextAccessorMock = new Mock<IHttpContextAccessor>();

            var propertyService = new PropertyService(
                propertyRepositoryMock.Object,
                mapperMock.Object,
                contextAccessorMock.Object,
                propertyTypeRepositoryMock.Object,
                authRepositoryMock.Object,
                propertyImageRepositoryMock.Object
            );

            var propertyInsertDto = new PropertyInsertDto
            {
                Address = "Nepal,Ktm",
                Description = "Feature 1: One Feature",
                Name = "Road Side Villa",
                Price = 200,
                PropertyTypeId = 1

            };
            var propertyType = new PropertyType
            {
             Id=1,
             Name="Temp"
            };
            propertyTypeRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).Returns(new List<PropertyType>() { propertyType }.AsQueryable());
            //authRepositoryMock.Setup(repo => repo.GetUserByUsername(It.IsAny<string>())).ReturnsAsync(new User());

            // Act
            var result = await propertyService.Create(propertyInsertDto);

            // Assert
            Assert.Equal(propertyInsertDto.Name, result.Name);
            // Add more assertions based on your requirements
        }

        // Similar tests for other methods in PropertyService
    }
}
