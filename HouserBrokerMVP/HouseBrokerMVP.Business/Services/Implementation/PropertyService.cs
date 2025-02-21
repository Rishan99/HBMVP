using AutoMapper;
using HouseBrokerMVP.Business.DTO;
using HouseBrokerMVP.Business.Exceptions;
using HouseBrokerMVP.Business.Services.FilePathProvider;
using HouseBrokerMVP.Core.Entities;
using HouseBrokerMVP.Core.Repositories;
using HouseBrokerMVP.Core.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace HouseBrokerMVP.Business.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly ICoreService _coreService;
        private readonly IPropertyReposiotry _propertyReposiotry;
        private readonly IAuthRepository _authRepository;
        private readonly IMapper _mapper;
        private readonly IPropertyTypeReposiotry _propertyTypeReposiotry;
        private readonly IPropertyImageReposiotry _propertyImageReposiotry;
        private readonly ISaveFilePathProviderService _saveFilePathProvider;
        private readonly IFileService _fileService;

        public PropertyService(IPropertyReposiotry propertyReposiotry, IMapper mapper, IHttpContextAccessor contextAccessor, IPropertyTypeReposiotry propertyTypeReposiotry, IAuthRepository authRepository, IPropertyImageReposiotry propertyImageReposiotry, ICoreService coreService, ISaveFilePathProviderService saveFilePathProvider, IFileService fileService)
        {
            _propertyReposiotry = propertyReposiotry;
            _mapper = mapper;
            _propertyTypeReposiotry = propertyTypeReposiotry;
            _authRepository = authRepository;
            _propertyImageReposiotry = propertyImageReposiotry;
            _coreService = coreService;
            _saveFilePathProvider = saveFilePathProvider;
            _fileService = fileService;
        }
        public async Task<PropertyListDto> Create(PropertyInsertDto data)
        {

            var model = _mapper.Map<Property>(data);
            var propType = await _propertyTypeReposiotry.GetById(data.PropertyTypeId).FirstOrDefaultAsync();
            if (propType is null) throw new NotFoundException("Invalid Property Type");
            var loggedInUserName = _coreService.GetLoggedInUserName();
                var user = await _authRepository.GetUserByUsername(loggedInUserName);
            if (user is null) throw new NotFoundException("User Not Found");
            model.BrokerId = user.Id;
            List<PropertyImage> images = new List<PropertyImage>();
            foreach (var image in data.Images)
            {
                var name = await _fileService.SaveFile(_saveFilePathProvider.GetPropertyImageFilePath(), image);
                images.Add(new PropertyImage()
                {
                    ImageName = name,
                });
            }
            model.PropertyImages = images;
            await _propertyReposiotry.Insert(model);
            await _propertyReposiotry.SaveChanges();
            return _mapper.Map<PropertyListDto>(model);
        }

        public async Task<PropertyListDto> Update(PropertyUpdateDto data)
        {
            var propType = await _propertyTypeReposiotry.GetById(data.PropertyTypeId).FirstOrDefaultAsync();
            if (propType is null) throw new NotFoundException("Invalid Property Id");
            var serviceData = await _propertyReposiotry.GetById(data.Id).FirstOrDefaultAsync();
            if (serviceData == null)
                throw new NotFoundException("Property not Found");
            TransactionScope trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                await _propertyImageReposiotry.DeleteByPropertyId(serviceData.Id);
                serviceData.Name = data.Name;
                serviceData.Price = data.Price;
                serviceData.Address = data.Address;
                serviceData.Description = data.Description;
                List<PropertyImage> images = new List<PropertyImage>();
                foreach (var image in data.Images)
                {
                    var name = await _fileService.SaveFile(_saveFilePathProvider.GetPropertyImageFilePath(), image);
                    images.Add(new PropertyImage()
                    {
                        ImageName = name,
                    });
                }
                serviceData.PropertyImages = images;
                _propertyReposiotry.Update(serviceData);
                await _propertyReposiotry.SaveChanges();
                trans.Complete();
                return _mapper.Map<PropertyListDto>(serviceData);
            }
            finally
            {
                trans.Dispose();
            }
        }
        public async Task Delete(int id)
        {
            using TransactionScope trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var propImages = await _propertyImageReposiotry.GetByPropertyId(id, false).ToListAsync();
            _propertyImageReposiotry.RemoveRange(propImages);
            await _propertyImageReposiotry.SaveChanges();
            await _propertyReposiotry.RemoveById(id);
            trans.Complete();
            //remove from physical location
        }
        public async Task<IEnumerable<PropertyListDto>> SearchProperty(string? location, decimal? minPrice, decimal? maxPrice, int? propertyType)
        {
            location = location?.ToLower();
            var propertyQueryable = _propertyReposiotry.Get(false)
                .Include(x => x.Broker)
                .Include(x => x.PropertyImages)
                .Include(x => x.PropertyType).AsQueryable();
            if (!string.IsNullOrEmpty(location))
            {
                propertyQueryable = propertyQueryable.Where(x => x.Address.ToLower().Contains(location));
            }

            if (minPrice != null)
            {
                propertyQueryable = propertyQueryable.Where(x => x.Price >= minPrice);
            }

            if (maxPrice != null)
            {
                propertyQueryable = propertyQueryable.Where(x => x.Price <= maxPrice);
            }

            if (propertyType != null)
            {
                propertyQueryable = propertyQueryable.Where(x => x.PropertyTypeId == propertyType);
            }
            var data = await propertyQueryable.ToListAsync();
            var mappedData = _mapper.Map<IEnumerable<PropertyListDto>>(data);
            return mappedData;
        }

        public async Task<IEnumerable<PropertyListDto>> GetList()
        {
            var data = await _propertyReposiotry.Get(false)
                .Include(x => x.PropertyImages)
                .Include(x => x.Broker)
                .Include(x => x.PropertyType).ToListAsync();
            var mappedData = _mapper.Map<IEnumerable<PropertyListDto>>(data);
            return mappedData;
        }

        public async Task<PropertyListDto> GetById(int id)
        {
            var originalData = await _propertyReposiotry.GetById(id)
                .Include(x => x.Broker)
                .Include(x => x.PropertyType).FirstOrDefaultAsync();
            if (originalData == null)
                throw new Exception("Property not Found");
            var data = _mapper.Map<PropertyListDto>(originalData);
            return data;
        }
    }
}
