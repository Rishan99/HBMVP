using AutoMapper;
using HouseBrokerMVP.Business.DTO;
using HouseBrokerMVP.Core.Entities;
using HouseBrokerMVP.Core.Repositories;
using HouseBrokerMVP.Core.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseBrokerMVP.Business.Services
{
    public interface IPropertyTypeService
    {
        Task<PropertyTypeListDto> Create(PropertyTypeInsertDto data);
        Task<PropertyTypeListDto> Update(PropertyTypeUpdateDto data);
        Task Delete(int id);
        Task<IEnumerable<PropertyTypeListDto>> GetList();
        Task<PropertyTypeListDto> GetById(int id);
    }
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
            try
            {

                var model = _mapper.Map<PropertyType>(data);
                model.CreatedBy = model.CreatedBy = GeneralUtility.GetLoggedInUsername(_contextAccessor);
                var response = await _propertyTypeReposiotry.Add(model);
                return _mapper.Map<PropertyTypeListDto>(response);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<PropertyTypeListDto> Update(PropertyTypeUpdateDto data)
        {
            var serviceData = await _propertyTypeReposiotry.GetById(data.Id).FirstOrDefaultAsync();
            if (serviceData == null)
                throw new Exception("Property Type not Found");
            try
            {
                serviceData.Name = data.Name;
                serviceData.ModifiedBy = GeneralUtility.GetLoggedInUsername(_contextAccessor);
                var responseModel = await _propertyTypeReposiotry.Update(serviceData);
                return _mapper.Map<PropertyTypeListDto>(responseModel);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task Delete(int id)
        {
            var data = await _propertyTypeReposiotry.GetById(id).FirstOrDefaultAsync();
            if (data == null)
                throw new Exception("Property Type not Found");
            await _propertyTypeReposiotry.Delete(data.Id);
        }

        public async Task<IEnumerable<PropertyTypeListDto>> GetList()
        {
            var data = await _propertyTypeReposiotry.GetList(false).ToListAsync();
            var mappedData = _mapper.Map<IEnumerable<PropertyTypeListDto>>(data);
            return mappedData;
        }

        public async Task<PropertyTypeListDto> GetById(int id)
        {
            var originalData = await _propertyTypeReposiotry.GetById(id).FirstOrDefaultAsync();
            if (originalData == null)
                throw new Exception("Property Type not Found");

            var data = _mapper.Map<PropertyTypeListDto>(originalData);
            return data;
        }
    }
}
