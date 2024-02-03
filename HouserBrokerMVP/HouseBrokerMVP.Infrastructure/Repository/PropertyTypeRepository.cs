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
    public class PropertyTypeRepository : IPropertyTypeReposiotry
    {
        private readonly AppDbContext _appDbContext;

        public PropertyTypeRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IQueryable<PropertyType> GetList(bool TrackEntity)
        {
            var data = _appDbContext.PropertyTypes;
            IQueryable<PropertyType> dataList = data;
            if (!TrackEntity)
                dataList = data.AsNoTracking();
            return dataList;
        }
        public IQueryable<PropertyType> GetById(int id)
        {

            return _appDbContext.PropertyTypes.AsNoTracking().Where(x => x.Id == id);


        }
        public async Task<PropertyType> Add(PropertyType PropertyType)
        {
            try
            {
                await _appDbContext.AddAsync(PropertyType);
                await _appDbContext.SaveChangesAsync();
                return PropertyType;
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
        public async Task<PropertyType> Update(PropertyType PropertyType)
        {
            _appDbContext.PropertyTypes.Update(PropertyType);
            await _appDbContext.SaveChangesAsync();
            return PropertyType;
        }
    }
}
