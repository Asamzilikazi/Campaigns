using Campaigns.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Campaigns.DataAccessLibrary.Repositories
{
    
        public interface ICampaignRepository
        {
            Task<Campaign> GetByIdAsync(int id);
            Task<IEnumerable<Campaign>> GetAllAsync();
            Task AddAsync(Campaign campaign);
            Task UpdateAsync(Campaign campaign);
            Task DeleteAsync(Campaign entity);
        }

}
