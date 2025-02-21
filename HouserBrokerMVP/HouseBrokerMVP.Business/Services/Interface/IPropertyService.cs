using HouseBrokerMVP.Business.DTO;

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
}
