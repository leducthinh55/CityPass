using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Utils;

namespace WebAPI.Utils
{
    public class DefaultSearch
    {
        public String SortBy { get; set; }
        public int SortDir { get; set; } = -1;
        public int PageSize { get; set; } = Constants.PAGE_SIZE; 
        public int PageIndex { get; set; } = Constants.PAGE_INDEX;
    }
}
