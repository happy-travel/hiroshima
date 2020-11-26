using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyTravel.Hiroshima.Common.Models.Images
{
    public class SlimImage
    {
        public Guid Id { get; set; }
        public string LargeImageURL { get; set; }
        public string SmallImageURL { get; set; }
        public MultiLanguage<string> Description { get; set; } = new MultiLanguage<string> { Ar = string.Empty, En = string.Empty, Ru = string.Empty };
    }
}
