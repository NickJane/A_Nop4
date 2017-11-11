using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Data
{
    public class Pager
    { 

        public int PageIndex
        {
            get;set;
        }

        public int PageSize
        {
            get;set;
        }


        public Pager()
        {
            PageIndex = 1;
            PageSize = 20;
        }
    }
}
