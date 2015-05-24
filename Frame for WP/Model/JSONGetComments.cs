using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frame_for_WP.Model
{
    class JSONGetComments
    {
        public int GET;
        public int Comment;
        public int ID;

        public JSONGetComments(int GET, int Comment, int ID)
        {
            this.GET = GET;
            this.Comment = Comment;
            this.ID = ID;
        }
    }
}
