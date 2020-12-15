using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyTravel.Hiroshima.DirectManager.Models.Requests
{
    public class ManagerWithServiceSupplier
    {
        public Manager Manager { get; set; }
        public ServiceSupplier ServiceSupplier { get; set; }
    }
}
