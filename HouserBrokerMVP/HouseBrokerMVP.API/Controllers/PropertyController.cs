using HouseBrokerMVP.Business.DTO;
using HouseBrokerMVP.Business.Services;
using HouseBrokerMVP.Core.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HouseBrokerMVP.API.Controllers
{
    [Route("api/property")]
    [ApiController]
    //[Authorize(Roles = nameof(RoleEnum.SuperAdmin) + "," + nameof(RoleEnum.Broker))]
    public class PropertyController : ControllerBase
    {
        public IPropertyService _propertyService;

        public PropertyController(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }

        [HttpGet, Route("search")]
        public async Task<IActionResult> SearchProperty(string? location, decimal? minPrice, decimal? maxPrice, int? propertyType)
        {
            try
            {
                var data = (await _propertyService.SearchProperty(location, minPrice, maxPrice, propertyType)).ToList();
                data.ForEach(x =>
                {
                    x.Images = x.Images.Select(y =>
                    {
                        y = Request.Scheme + "://" + Request.Host + "/" + "PropertyImage" + "/" + y;
                        return y;
                    }).ToList();
                });
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet, Route("")]
        public async Task<IActionResult> GetList()
        {
            try
            {
                var data = (await _propertyService.GetList()).ToList();
                data.ForEach(x =>
                {
                    x.Images = x.Images.Select(y =>
                    {
                        y = Request.Scheme + "://" + Request.Host + "/" + "PropertyImage" + "/" + y;
                        return y;
                    }).ToList();
                });
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet, Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var data = await _propertyService.GetById(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost, Route("")]
        public async Task<IActionResult> AddPropertyType()
        {
            try
            {
                PropertyInsertDto data = JsonConvert.DeserializeObject<PropertyInsertDto>(HttpContext.Request.Form["data"]);
                var files = Request.Form.Files;
                data.Images = files.ToList();
                var response = await _propertyService.Create(data);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut, Route("{id}")]
        public async Task<IActionResult> UpdatePropType(int id)
        {
            try
            {
                PropertyUpdateDto data = JsonConvert.DeserializeObject<PropertyUpdateDto>(HttpContext.Request.Form["data"]);
                var files = Request.Form.Files;
                data.Images = files.ToList();
                data.Id = id;
                var response = await _propertyService.Update(data);
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
                await _propertyService.Delete(id);
                return Ok("Property Type has been deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
