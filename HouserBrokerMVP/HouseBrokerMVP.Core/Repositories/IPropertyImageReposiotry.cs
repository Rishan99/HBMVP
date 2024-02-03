using HouseBrokerMVP.Core.Context;
using HouseBrokerMVP.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseBrokerMVP.Core.Repositories
{

    public interface IPropertyImageReposiotry
    {
        IQueryable<PropertyImage> GetByPropertyId(int propertyId, bool TrackEntity);
        Task<PropertyImage> Add(PropertyImage service);
        Task Delete(int id); 
        Task DeleteByPropertyId(int id);
        IQueryable<PropertyImage> GetById(int id);
    }
}
