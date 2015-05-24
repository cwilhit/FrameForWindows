using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frame_for_WP.Model
{
    public class JSONGetPosts
    {
        public int GET;
        public int Bottom;
        public double Lat;
        public double Lon;
        public string Filter;
        public int Sort;

        public JSONGetPosts(int GET, int Bottom, double Lat, double Lon, string Filter, int Sort)
        {
            this.GET = GET;
            this.Bottom = Bottom;
            this.Lat = Lat;
            this.Lon = Lon;
            this.Filter = Filter;
            this.Sort = Sort;
        }
    }
}
