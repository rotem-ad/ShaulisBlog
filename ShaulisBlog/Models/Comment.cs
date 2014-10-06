using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShaulisBlog.Models
{
    //Comment for a post
    public class Comment
    {
        public int CommentID { get; set; }
        public int PostID { get; set; } //fk
        public string Title { get; set; }
        public string Writer { get; set; }
        public string WriterWebSite { get; set; }
        public string Content { get; set; }

        //post that the commant is attached to
        public virtual Post Post { get; set; }
    }
}