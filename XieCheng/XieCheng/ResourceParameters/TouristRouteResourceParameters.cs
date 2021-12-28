using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace XieCheng.ResourceParameters
{
    public class TouristRouteResourceParameters
    {
        //private int pageNumber = 1;
        //public int PageNumber
        //{
        //    get { return pageNumber; }
        //    set
        //    {
        //        if (value >= 1)
        //        {
        //            pageNumber = value;
        //        }
        //    }
        //}

        //const int maxPageSize = 50;
        //private int pageSize = 10;
        //public int PageSize
        //{
        //    get
        //    {
        //        return pageSize;
        //    }
        //    set
        //    {
        //        if (value >= 1)
        //        {
        //            pageSize = (value > maxPageSize) ? maxPageSize : value;
        //        }
        //    }
        //}
        public string Keyword { get; set; }
        public string RatingOperator { get; set; }
        public int? RatingValue { get; set; }

        private string rating;
        public string Rating
        {
            get { return this.rating; }
            set 
            {
                if (!string.IsNullOrEmpty(value))
                {

                    Regex regex = new Regex(@"([A-Za-z0-9\-]+)(\d+)");
                    Match match = regex.Match(value);

                    if (match.Success)
                    {
                        RatingOperator = match.Groups[1].Value;
                        RatingValue = int.Parse(match.Groups[2].Value);
                    }
                }

                this.rating = value; 
            }
        }
    }
}
