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

    public interface IPropertyTypeReposiotry
    {
        IQueryable<PropertyType> GetList(bool TrackEntity);
        Task<PropertyType> Add(PropertyType service);
        Task Delete(int id);
        Task<PropertyType> Update(PropertyType service);
        IQueryable<PropertyType> GetById(int id);
    }
}
