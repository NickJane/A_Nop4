using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Services.Customer
{
    public class CustomerInfo
    {
        public Guid CustomerGuid { get; set; }

        public bool Deleted { get; set; }
        public bool Active { get; set; }
    }
}