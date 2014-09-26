using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShaulisBlog.Models
{
    public class Comment
    {
        public int CommentID { get; set; }
        public int PostID { get; set; }
        public string Title { get; set; }
        public string Writer { get; set; }
        public string WriterWebSite { get; set; }
        public string Content { get; set; }

        public virtual Post Post { get; set; }
    }
}