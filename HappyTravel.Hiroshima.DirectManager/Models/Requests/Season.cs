﻿using System;
using System.ComponentModel.DataAnnotations;

namespace HappyTravel.Hiroshima.DirectManager.Models.Requests
{
    public class Season
    {
        [Required]
        public int SeasonId { get; set; }
        
        [Required]
        public DateTime StartDate { get; set; }
        
        [Required]
        public DateTime EndDate { get; set; }
    }
}