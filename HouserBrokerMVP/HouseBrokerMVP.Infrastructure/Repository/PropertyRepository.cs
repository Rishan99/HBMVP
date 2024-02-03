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
    public class PropertyRepository : IPropertyReposiotry
    {
        private readonly AppDbContext _appDbContext;

        public PropertyRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IQueryable<Property> GetList(bool TrackEntity)
        {
            var data = _appDbContext.Properties;
            IQueryable<Property> dataList = data;
            if (!TrackEntity)
                dataList = data.AsNoTracking();
            return dataList;
        }
        public IQueryable<Property> GetById(int id)
        {

            return _appDbContext.Properties.AsNoTracking().Where(x => x.Id == id);


        }
        public async Task<Property> Add(Property Property)
        {
            try
            {
                await _appDbContext.AddAsync(Property);
                await _appDbContext.SaveChangesAsync();
                return Property;
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
        public async Task<Property> Update(Property Property)
        {
            _appDbContext.Properties.Update(Property);
            await _appDbContext.SaveChangesAsync();
            return Property;
        }
    }
}
