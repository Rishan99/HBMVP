using AutoMapper;
using HouseBrokerMVP.Business.DTO;
using HouseBrokerMVP.Business.Services;
using HouseBrokerMVP.Business.Services.FilePathProvider;
using HouseBrokerMVP.Core.Context;
using HouseBrokerMVP.Core.Entities;
using HouseBrokerMVP.Core.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace HouseBrokerMVP.Test
{
    public class UnitTest
    {
        private readonly Mock<IPropertyReposiotry> _propertyRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IAuthRepository> _authRepositoryMock;
        private readonly Mock<IPropertyTypeReposiotry> _propertyTypeRepositoryMock;
        private readonly Mock<IPropertyImageReposiotry> _propertyImageRepositoryMock;
        private readonly Mock<ICoreService> _coreServiceMock;
        private readonly Mock<ISaveFilePathProviderService> _saveFilePathProviderMock;
        private readonly Mock<IFileService> _fileServiceMock;
        private readonly Mock<IHttpContextAccessor> _httpContext;
        private readonly PropertyService _propertyService;

        public UnitTest()
        {
            _propertyRepositoryMock = new Mock<IPropertyReposiotry>();
            _mapperMock = new Mock<IMapper>();
            _authRepositoryMock = new Mock<IAuthRepository>();
            _propertyTypeRepositoryMock = new Mock<IPropertyTypeReposiotry>();
            _propertyImageRepositoryMock = new Mock<IPropertyImageReposiotry>();
            _coreServiceMock = new Mock<ICoreService>();
            _saveFilePathProviderMock = new Mock<ISaveFilePathProviderService>();
            _fileServiceMock = new Mock<IFileService>();
            _httpContext = new Mock<IHttpContextAccessor>();

            _propertyService = new PropertyService(
                _propertyRepositoryMock.Object,
                _mapperMock.Object,
               _httpContext.Object,
                _propertyTypeRepositoryMock.Object,
                _authRepositoryMock.Object,
                _propertyImageRepositoryMock.Object,
                _coreServiceMock.Object,
                _saveFilePathProviderMock.Object,
                _fileServiceMock.Object
            );
        }

        private async Task InitialSeedSetup(AppDbContext context)
        {
            var uid = Guid.NewGuid().ToString();
            var user = new ApplicationUser()
            {
                Id = uid,
                UserName = "testuser",
                Email = "testuser@example.com",
                NormalizedUserName = "TESTUSER",
                NormalizedEmail = "TESTUSER@EXAMPLE.COM",
                PhoneNumber = "1234567890",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                LockoutEnabled = true,
                AccessFailedCount = 0,
                PasswordHash = "temphash",
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            await context.Users.AddAsync(user);

            var propertyTypes = new List<PropertyType>
        {
            new PropertyType { Id = 1, Name = "Residential",CreatedDate=DateTime.Now,CreatedBy="admin" },
            new PropertyType { Id = 2, Name = "Commercial",CreatedDate=DateTime.Now,CreatedBy="admin" }
        };

            var properties = new List<Property>
        {
            new Property
            {
                Name = "Prop 1",
                Price = 130000,
                Address = "New York",
                PropertyTypeId = 1,
                BrokerId=uid,
                Description="hello",
                PropertyType = propertyTypes.First(pt => pt.Id == 1)
            },
            new Property
            {
                Name = "Prop 2",
                Price = 200000,
                Address = "Los Angeles",
                BrokerId=uid,
                Description="hello",
                PropertyTypeId = 2,
                PropertyType = propertyTypes.First(pt => pt.Id == 2)
            }
        };

            context.PropertyTypes.AddRange(propertyTypes);
            context.Properties.AddRange(properties);

            await context.SaveChangesAsync();
        }
        [Fact]
        public async Task SearchPropertyFilter()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                            .UseInMemoryDatabase(databaseName: "TestMemDB")
                            .Options;

            using (var context = new AppDbContext(options))
            {
                await InitialSeedSetup(context);

                _propertyRepositoryMock.Setup(repo => repo.Get(false)).Returns(context.Properties);

                _mapperMock.Setup(m => m.Map<IEnumerable<PropertyListDto>>(It.IsAny<List<Property>>()))
                           .Returns((List<Property> src) => src.Select(p => new PropertyListDto()).ToList());

                var result = await _propertyService.SearchProperty("New", 120000, 180000, 1);

                Assert.NotNull(result);
                Assert.Single(result);
            }

        }

    }
}
