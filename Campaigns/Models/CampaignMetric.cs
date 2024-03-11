using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Campaigns.Models
{
    public class CampaignMetric
    {
        [Key]
        public int MetricId { get; set; }
        public int CampaignId { get; set; }
        public DateTime Date { get; set; }
        public int Clicks { get; set; }
        public int Impressions { get; set; }
        public int Conversions { get; set; }
        public decimal Revenue { get; set; }
    }

}
