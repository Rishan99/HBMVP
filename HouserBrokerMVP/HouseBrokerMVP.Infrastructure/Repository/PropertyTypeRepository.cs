using HouseBrokerMVP.Core.Context;
using HouseBrokerMVP.Core.Entities;
using HouseBrokerMVP.Core.Repositories;

namespace HouseBrokerMVP.Infrastructure.Repository
{
    public class PropertyTypeRepository : RepositoryBase<PropertyType, int>, IPropertyTypeReposiotry
    {
        public PropertyTypeRepository(AppDbContext appDbContext) : base(appDbContext)
        {

        }

    }
}
