using HouseBrokerMVP.Core.Context;
using HouseBrokerMVP.Core.Entities;
using HouseBrokerMVP.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseBrokerMVP.Infrastructure.Repository
{
    public class PropertyRepository : RepositoryBase<Property, int>, IPropertyReposiotry
    {
        public PropertyRepository(AppDbContext appDbContext) : base(appDbContext)
        {

        }
    }
}
