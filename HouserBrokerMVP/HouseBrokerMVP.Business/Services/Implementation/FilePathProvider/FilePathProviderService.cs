using HouseBrokerMVP.Business.Services.FilePathProvider;

namespace HouseBrokerMVP.Business.Services.FilePathProvider
{
    public abstract class FilePathProviderService : IFilePathProviderService
    {
        private readonly string _BaseUrl;

        protected FilePathProviderService(string baseUrl)
        {
            _BaseUrl = baseUrl;
        }

        public string GetPropertyImageFilePath()
        {
            return _BaseUrl + "/Images/PropertyImage";
        }
    }
}
