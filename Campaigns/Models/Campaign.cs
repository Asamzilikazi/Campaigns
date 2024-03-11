using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Campaigns.Models
{
    public class Campaign
    {
        [Key]
        public int CampaignId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ClientId { get; set; }
        public int UserId { get; set; }

        // Navigation property to Client if needed
        //public Client Client { get; set; }

    }

}
