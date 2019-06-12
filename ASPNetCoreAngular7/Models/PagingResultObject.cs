using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNetCoreAngular7
{ 
    public class PagingResultObject
    {
        public int PageNum { get; set; }
        public int PageSize { get; set; }
        public int[] PageIds { get; set; }  
        public NewsArticle[] PageItems { get; set; }
        public int PageStartItemNum { get; set; }
        public int PageEndItemNum { get; set; }
        public int NumPagesTotal { get; set; }
        public int NumItemsTotal { get; set; }
    }
}
