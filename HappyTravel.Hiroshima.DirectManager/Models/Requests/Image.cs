﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace HappyTravel.Hiroshima.DirectManager.Models.Requests
{
    public class Image 
    {
        public int AccommodationId { get; set; }
        public FormFile UploadedFile { get; set; }
    }
}
