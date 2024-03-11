using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Campaigns.Models;

namespace Campaigns.DTOs
    {
        public class CampaignDto
        {
        
            [Required(ErrorMessage = "Campaign ID is required")]
            public int CampaignId { get; set; }

            [Required(ErrorMessage = "Name is required")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Description is required")]
            public string Description { get; set; }

            [Required(ErrorMessage = "Start date is required")]
            public DateTime StartDate { get; set; }

            [Required(ErrorMessage = "End date is required")]
            public DateTime EndDate { get; set; }

            [Required(ErrorMessage = "Client ID is required")]
            public int ClientId { get; set; }

            [Required(ErrorMessage = "User ID is required")]
            public int UserId { get; set; }

        //// Navigation property to Client if needed
        //    [Required(ErrorMessage = "Client ID is required")]
        //    public Client Client { get; set; }
    }
    }

