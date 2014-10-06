using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace ShaulisBlog.Models
{

    public class Post
    {
        public int PostID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string AuthorWebSite { get; set; }
        public DateTime PublishDate { get; set; }

        // Required to force EF to create a ntext column, not a nvarchar(n)
        //Enshure that when db is create the langth of field is max
        [MaxLength]
        public string Content { get; set; }

        // Required to force EF to create an image column, not a binary(n)
        [MaxLength]
        public byte[] Photo { get; set; }
        

        public virtual ICollection<Comment> Comments { get; set; }
    }
}