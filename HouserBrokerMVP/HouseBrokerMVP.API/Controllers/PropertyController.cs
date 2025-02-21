using HouseBrokerMVP.Business.DTO;
using HouseBrokerMVP.Business.Services;
using HouseBrokerMVP.Business.Services.FilePathProvider;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HouseBrokerMVP.API.Controllers
{
    [Route("api/property")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PropertyController : ControllerBase
    {
        private readonly IPropertyService _propertyService;
        private readonly IReadFilePathProviderService _fileService;

        public PropertyController(IPropertyService propertyService, IReadFilePathProviderService fileService)
        {
            _propertyService = propertyService;
            _fileService = fileService;
        }

        [HttpGet, Route("search")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchProperty(string? location, decimal? minPrice, decimal? maxPrice, int? propertyType)
        {

            var data = (await _propertyService.SearchProperty(location, minPrice, maxPrice, propertyType)).ToList();
            data.ForEach(x =>
            {
                x.Images = x.Images.Select(y =>
                {
                    return _fileService.GetPropertyImageFilePath() + "/" + y;
                }).ToList();
            });
            return Ok(data);
        }

        [HttpGet, Route("")]
        public async Task<IActionResult> GetList()
        {
            var data = (await _propertyService.GetList()).ToList();
            data.ForEach(x =>
            {
                x.Images = x.Images.Select(y =>
                {
                    return _fileService.GetPropertyImageFilePath() + "/" + y;
                }).ToList();
            });
            return Ok(data);
        }

        [HttpGet, Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _propertyService.GetById(id);
            return Ok(data);
        }

        [HttpPost, Route("")]
        [Authorize(Roles = "Broker")]
        public async Task<IActionResult> AddPropertyType([FromForm] PropertyInsertDto formData)
        {
            var response = await _propertyService.Create(formData);
            return Ok(response);

        }

        [HttpPut, Route("{id}")]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = "Broker")]
        public async Task<IActionResult> UpdatePropType(int id, [FromForm] PropertyUpdateDto formData)
        {
            formData.Id = id;
            var response = await _propertyService.Update(formData);
            return Ok(response);

        }
        [HttpDelete, Route("{id}")]
        [Authorize(Roles = "Broker")]
        public async Task<IActionResult> DeletePropType(int id)
        {
            await _propertyService.Delete(id);
            return Ok("Property Type has been deleted");
        }
    }
}
