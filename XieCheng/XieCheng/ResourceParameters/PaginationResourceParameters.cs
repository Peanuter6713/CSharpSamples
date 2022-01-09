using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XieCheng.ResourceParameters
{
    public class PaginationResourceParameters
    {
        private int pageNumber = 1;
        public int PageNumber
        {
            get { return pageNumber; }
            set
            {
                if (value >= 1)
                {
                    pageNumber = value;
                }
            }
        }

        const int maxPageSize = 50;
        private int pageSize = 10;
        public int PageSize
        {
            get
            {
                return pageSize;
            }
            set
            {
                if (value >= 1)
                {
                    pageSize = (value > maxPageSize) ? maxPageSize : value;
                }
            }
        }
    }
}
