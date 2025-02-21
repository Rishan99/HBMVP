using HouseBrokerMVP.Business.DTO;
using HouseBrokerMVP.Business.Services;
using HouseBrokerMVP.Core.Enum;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HouseBrokerMVP.API.Controllers
{
    [Route("api/property-type")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PropertyTypeController : ControllerBase
    {
        public IPropertyTypeService _propertyTypeService;

        public PropertyTypeController(IPropertyTypeService propertyTypeService)
        {
            _propertyTypeService = propertyTypeService;
        }

        [HttpGet, Route("")]
        public async Task<IActionResult> GetList()
        {

            var data = await _propertyTypeService.GetList();
            return Ok(data);

        }
        [HttpGet, Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {

            var data = await _propertyTypeService.GetById(id);
            return Ok(data);

        }
        [HttpPost, Route("")]
        public async Task<IActionResult> AddPropertyType(PropertyTypeInsertDto data)
        {
            var response = await _propertyTypeService.Create(data);
            return Ok(response);

        }

        [HttpPut, Route("{id}")]
        public async Task<IActionResult> UpdatePropType(int id, PropertyTypeUpdateDto data)
        {
            try
            {
                data.Id = id;
                var response = await _propertyTypeService.Update(data);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete, Route("{id}")]
        public async Task<IActionResult> DeletePropType(int id)
        {
            try
            {
                await _propertyTypeService.Delete(id);
                return Ok("Property Type has been deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
