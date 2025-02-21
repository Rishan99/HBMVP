using Microsoft.AspNetCore.Http;

namespace HouseBrokerMVP.Business.Services.FilePathProvider
{
    public class ReadFilePathProviderService : FilePathProviderService, IReadFilePathProviderService
    {
        public ReadFilePathProviderService(IHttpContextAccessor httpContextAccessor) :
            base(httpContextAccessor.HttpContext.Request.Scheme + "://" + httpContextAccessor.HttpContext.Request.Host)
        {

        }
    }
}
