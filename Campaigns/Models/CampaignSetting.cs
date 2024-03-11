using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Campaigns.Models
{
    public class CampaignSetting
    {
        [Key]
        public int SettingId { get; set; }
        public int CampaignId { get; set; }
        public string SettingName { get; set; }
        public string SettingValue { get; set; }
    }

}
