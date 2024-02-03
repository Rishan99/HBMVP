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
    public class PropertyImageRepository : IPropertyImageReposiotry
    {
        private readonly AppDbContext _appDbContext;

        public PropertyImageRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IQueryable<PropertyImage> GetById(int id)
        {
            return _appDbContext.PropertyImages.AsNoTracking().Where(x => x.Id == id);
        }
        public async Task<PropertyImage> Add(PropertyImage PropertyImage)
        {
            try
            {
                await _appDbContext.AddAsync(PropertyImage);
                await _appDbContext.SaveChangesAsync();
                return PropertyImage;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task Delete(int id)
        {
            var data = await GetById(id).FirstAsync();
            _appDbContext.Remove(data);
            await _appDbContext.SaveChangesAsync();
        }
        public async Task DeleteByPropertyId(int id)
        {
            await _appDbContext.PropertyImages.Where(x => x.PropertyId == id).ExecuteDeleteAsync();
        }


        public IQueryable<PropertyImage> GetByPropertyId(int propertyId, bool TrackEntity)
        {
            var data = _appDbContext.PropertyImages;
            IQueryable<PropertyImage> dataList = data;
            if (!TrackEntity)
                dataList = data.AsNoTracking();
            dataList = dataList.Where(x => x.PropertyId == propertyId);
            return dataList;
        }
    }
}
