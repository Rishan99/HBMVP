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
    public class PropertyImageRepository : RepositoryBase<PropertyImage, int>, IPropertyImageReposiotry
    {
        public PropertyImageRepository(AppDbContext appDbContext) : base(appDbContext)
        {

        }
        public async Task Delete(int id)
        {
            var data = await GetById(id).FirstAsync();
            base.Remove(data);
            await base.SaveChanges();
        }
        public async Task DeleteByPropertyId(int id)
        {
            await _appDbContext.PropertyImages.Where(x => x.PropertyId == id).ExecuteDeleteAsync();
        }

        public IQueryable<PropertyImage> GetByPropertyId(int propertyId, bool TrackEntity)
        {
            var dataList = _appDbContext.PropertyImages.Where(x => x.PropertyId == propertyId);
            if (!TrackEntity)
                dataList = dataList.AsNoTracking();
            return dataList;
        }
    }
}
