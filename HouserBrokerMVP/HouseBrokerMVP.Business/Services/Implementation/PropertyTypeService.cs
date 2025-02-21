using AutoMapper;
using HouseBrokerMVP.Business.DTO;
using HouseBrokerMVP.Business.Exceptions;
using HouseBrokerMVP.Core.Entities;
using HouseBrokerMVP.Core.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace HouseBrokerMVP.Business.Services
{
    public class PropertyTypeService : IPropertyTypeService
    {
        private readonly IPropertyTypeReposiotry _propertyTypeReposiotry;
        private readonly IMapper _mapper;
        public IHttpContextAccessor _contextAccessor;

        public PropertyTypeService(IPropertyTypeReposiotry propertyTypeReposiotry, IMapper mapper, IHttpContextAccessor contextAccessor)
        {
            _propertyTypeReposiotry = propertyTypeReposiotry;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
        }
        public async Task<PropertyTypeListDto> Create(PropertyTypeInsertDto data)
        {
            var model = _mapper.Map<PropertyType>(data);
            await _propertyTypeReposiotry.Insert(model);
            await _propertyTypeReposiotry.SaveChanges();
            return _mapper.Map<PropertyTypeListDto>(model);
        }

        public async Task<PropertyTypeListDto> Update(PropertyTypeUpdateDto data)
        {
            var serviceData = await _propertyTypeReposiotry.GetById(data.Id, true).FirstOrDefaultAsync();
            if (serviceData == null)
                throw new NotFoundException("Property Type not Found");
            serviceData.Name = data.Name;
            _propertyTypeReposiotry.Update(serviceData);
            await _propertyTypeReposiotry.SaveChanges();
            return _mapper.Map<PropertyTypeListDto>(serviceData);
        }

        public async Task Delete(int id)
        {
            var data = await _propertyTypeReposiotry.GetById(id, true).FirstOrDefaultAsync();
            if (data == null)
                throw new NotFoundException("Property Type not Found");
            await _propertyTypeReposiotry.RemoveById(data.Id);
        }

        public async Task<IEnumerable<PropertyTypeListDto>> GetList()
        {
            var data = await _propertyTypeReposiotry.Get().ToListAsync();
            var mappedData = _mapper.Map<IEnumerable<PropertyTypeListDto>>(data);
            return mappedData;
        }

        public async Task<PropertyTypeListDto> GetById(int id)
        {
            var originalData = await _propertyTypeReposiotry.GetById(id).FirstOrDefaultAsync();
            if (originalData == null)
                throw new NotFoundException("Property Type not Found");
            return _mapper.Map<PropertyTypeListDto>(originalData);
        }
    }
}
