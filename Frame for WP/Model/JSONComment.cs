using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frame_for_WP.Model
{
    public class JSONComment
    {
        public int POST;
        public int ID;
        public string Comment;
        public string User;

        public JSONComment(int post, int id, string comment, string user)
        {
            this.POST = post;
            this.ID = id;
            this.Comment = comment;
            this.User = user;
        }
    }
}
