using HouseBrokerMVP.Business.DTO;

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
}
