using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frame_for_WP.Model
{
    class JSONPicture
    {
        public int POST;
        public string Picture;
        public double Lat;
        public double Lon;
        public string User;
        public string Tags;

        public JSONPicture(int POST, string Picture, double Lat, double Lon, string User, string Tags)
        {
            this.POST = POST;
            this.Picture = Picture;
            this.Lat = Lat;
            this.Lon = Lon;
            this.User = User;
            this.Tags = Tags;
        }
    }
}
