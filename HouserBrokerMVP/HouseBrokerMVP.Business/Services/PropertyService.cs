using AutoMapper;
using HouseBrokerMVP.Business.DTO;
using HouseBrokerMVP.Core.Entities;
using HouseBrokerMVP.Core.Repositories;
using HouseBrokerMVP.Core.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace HouseBrokerMVP.Business.Services
{
    public interface IPropertyService
    {
        Task<PropertyListDto> Create(PropertyInsertDto data);
        Task<PropertyListDto> Update(PropertyUpdateDto data);
        Task Delete(int id);
        Task<IEnumerable<PropertyListDto>> GetList();
        Task<IEnumerable<PropertyListDto>> SearchProperty(string? location, decimal? minPrice, decimal? maxPrice, int? propertyType);
        Task<PropertyListDto> GetById(int id);
    }
    public class PropertyService : IPropertyService
    {
        private readonly IPropertyReposiotry _propertyReposiotry;
        private readonly IAuthRepository _authRepository;
        private readonly IMapper _mapper;
        private readonly IPropertyTypeReposiotry _propertyTypeReposiotry;
        private readonly IPropertyImageReposiotry _propertyImageReposiotry;
        public IHttpContextAccessor _contextAccessor;

        public PropertyService(IPropertyReposiotry propertyReposiotry, IMapper mapper, IHttpContextAccessor contextAccessor, IPropertyTypeReposiotry propertyTypeReposiotry, IAuthRepository authRepository, IPropertyImageReposiotry propertyImageReposiotry)
        {
            _propertyReposiotry = propertyReposiotry;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
            _propertyTypeReposiotry = propertyTypeReposiotry;
            _authRepository = authRepository;
            _propertyImageReposiotry = propertyImageReposiotry;
        }
        public async Task<PropertyListDto> Create(PropertyInsertDto data)
        {
            try
            {
                var model = _mapper.Map<Property>(data);
                var propType = await _propertyTypeReposiotry.GetById(data.PropertyTypeId).FirstOrDefaultAsync();
                if (propType is null) throw new Exception("Invalid Property Type");
                var loggedInUserName = GeneralUtility.GetLoggedInUsername(_contextAccessor);
                var user = await _authRepository.GetUserByUsername(loggedInUserName);
                if (user is null) throw new Exception("Error, Please try again");
                model.BrokerId = user.Id;
                model.CreatedBy = loggedInUserName;
                List<PropertyImage> images = new List<PropertyImage>();
                foreach (var image in data.Images)
                {
                    var name = await GeneralUtility.SaveImageFile("PropertyImage", image);
                    images.Add(new PropertyImage()
                    {
                        CreatedBy = GeneralUtility.GetLoggedInUsername(_contextAccessor),
                        ImageName = name,
                    });
                }
                model.PropertyImages = images;
                var response = await _propertyReposiotry.Add(model);
                return _mapper.Map<PropertyListDto>(response);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PropertyListDto> Update(PropertyUpdateDto data)
        {
            var propType = await _propertyTypeReposiotry.GetById(data.PropertyTypeId).FirstOrDefaultAsync();
            if (propType is null) throw new Exception("Invalid Property Id");
            var serviceData = await _propertyReposiotry.GetById(data.Id).FirstOrDefaultAsync();
            if (serviceData == null)
                throw new Exception("Property not Found");
            TransactionScope trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                await _propertyImageReposiotry.DeleteByPropertyId(serviceData.Id);
                serviceData.Name = data.Name;
                serviceData.Price = data.Price;
                serviceData.Address = data.Address;
                serviceData.Description = data.Description;
                serviceData.ModifiedBy = GeneralUtility.GetLoggedInUsername(_contextAccessor);
                List<PropertyImage> images = new List<PropertyImage>();
                foreach (var image in data.Images)
                {
                    var name = await GeneralUtility.SaveImageFile("PropertyImage", image);
                    images.Add(new PropertyImage()
                    {
                        CreatedBy = GeneralUtility.GetLoggedInUsername(_contextAccessor),
                        ImageName = name,
                    });
                }
                serviceData.PropertyImages = images;
                var responseModel = await _propertyReposiotry.Update(serviceData);
                trans.Complete();
                return _mapper.Map<PropertyListDto>(responseModel);
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                trans.Dispose();
            }

        }

        public async Task Delete(int id)
        {
            var data = await _propertyReposiotry.GetById(id).FirstOrDefaultAsync();
            if (data == null)
                throw new Exception("Property not Found");
            await _propertyReposiotry.Delete(data.Id);
        }
        public async Task<IEnumerable<PropertyListDto>> SearchProperty(string? location, decimal? minPrice, decimal? maxPrice, int? propertyType)
        {
            location = location?.ToLower();
            var data = await _propertyReposiotry.GetList(false)
                .Include(x => x.Broker)
                .Include(x=>x.PropertyImages)
                .Include(x => x.PropertyType)
                .Where(x => (string.IsNullOrEmpty(location) ? true : (x.Address.ToLower().Contains(location))) &&
                (minPrice == null ? true : (x.Price >= minPrice)) &&
                (maxPrice == null ? true : x.Price <= maxPrice) &&
                (propertyType == null ? true : x.PropertyTypeId == propertyType))
                .ToListAsync();
            var mappedData = _mapper.Map<IEnumerable<PropertyListDto>>(data);
            return mappedData;
        }

        public async Task<IEnumerable<PropertyListDto>> GetList()
        {
            var data = await _propertyReposiotry.GetList(false)
                .Include(x => x.PropertyImages)
                .Include(x => x.Broker)
                .Include(x => x.PropertyType).ToListAsync();
            var mappedData = _mapper.Map<IEnumerable<PropertyListDto>>(data);
            return mappedData;
        }

        public async Task<PropertyListDto> GetById(int id)
        {
            var originalData = await _propertyReposiotry.GetById(id).Include(x => x.Broker).Include(x => x.PropertyType).FirstOrDefaultAsync();
            if (originalData == null)
                throw new Exception("Property not Found");
            var data = _mapper.Map<PropertyListDto>(originalData);
            return data;
        }
    }
}
