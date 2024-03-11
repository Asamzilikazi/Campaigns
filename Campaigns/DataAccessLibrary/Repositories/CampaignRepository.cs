using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using Campaigns.DataAccessLibrary.Repositories;
using Campaigns.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks; 


namespace Campaigns.DataAccessLibrary.Repositories
{
    public class CampaignRepository : ICampaignRepository
    {
        private readonly CampaignsDbContext _context;

        public CampaignRepository(CampaignsDbContext context)
        {
            _context = context;
        }

        public async Task<Campaign> GetByIdAsync(int id)
        {
            return await _context.Campaigns
               .Where(c => c.CampaignId == id)
               .Select(c => new Campaign
               {
                   CampaignId = c.CampaignId,
                   Name = c.Name,
                   Description = c.Description,
                   StartDate = c.StartDate,
                   EndDate = c.EndDate,
                   ClientId = c.ClientId,
                   UserId = c.UserId,
                   //ClientName = c.Client.Name,
                   //Client = c.Client
               })
               .FirstOrDefaultAsync();
                }

        public async Task<IEnumerable<Campaign>> GetAllAsync()
        {
            return await _context.Campaigns
                .Select(c => new Campaign
                {
                    CampaignId = c.CampaignId,
                    Name = c.Name,
                    Description = c.Description,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    ClientId = c.ClientId,
                    UserId = c.UserId,
                    //ClientName = c.Client.Name,
                    //Client = c.Client
                })
                .ToListAsync();
            }

        public async Task AddAsync(Campaign entity)
        {
            await _context.Campaigns.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Campaign entity)
        {
            _context.Campaigns.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Campaign entity)
        {
            _context.Campaigns.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

}
