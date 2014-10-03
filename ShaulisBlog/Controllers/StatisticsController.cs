using ShaulisBlog.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShaulisBlog.Controllers
{
    public class StatisticsController : Controller
    {

        private BlogDBContext db = new BlogDBContext();

        //
        // GET: /Statistics/

        public ActionResult Index()
        {
            //generate statistic files
            GenerateStatisticFiles();

            return View();
        }

        private void GenerateStatisticFiles() {
            //How many posts were added per month?    -- get the data
            var postsPerMonth = db.Posts.GroupBy(a => a.PublishDate.Month).Select(g => new { StateId = g.Key, Count = g.Count() });
            //write to file
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\miri_k\Source\Repos\ShaulisBlog\ShaulisBlog\postsPerMonth.tsv", false))
            {
               string line = string.Format("{0}\t{1}", "Month", "PostsCount"); //do headline
               file.WriteLine(line);

               foreach (var data in postsPerMonth) {
                   line = string.Format("{0}\t{1}", data.StateId, data.Count); //do headline
                   file.WriteLine(line);
               }
            }

            //How many comments were added per month?   --get the data
            var commentsPerMonth =db.Comments.GroupBy(x=>x.Post.PublishDate.Month).Select(g => new { StateId = g.Key, Count = g.Count() });
            //write to file
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\miri_k\Source\Repos\ShaulisBlog\ShaulisBlog\commentsPerMonth.tsv", true))
            {
                string line = string.Format("{0}\t{1}", "Month", "CommentsCount"); //do headline
                file.WriteLine(line);

                foreach (var data in postsPerMonth)
                {
                    line = string.Format("{0}\t{1}", data.StateId, data.Count); //do headline
                    file.WriteLine(line);
                }
            }

            //How many fans join us each day?
            //var fansPerDay = db.Comments.GroupBy(x => x.Post.PublishDate.Month).Select(g => new { StateId = g.Key, Count = g.Count() });
            ////write to file
            //using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\miri_k\Source\Repos\ShaulisBlog\ShaulisBlog\fansPerDay.tsv", true))
            //{
            //    string line = string.Format("{0}\t{1}", "Date", "FansCount"); //do headline
            //    file.WriteLine(line);

            //    foreach (var data in postsPerMonth)
            //    {
            //        line = string.Format("{0}\t{1}", data.StateId, data.Count); //do headline
            //        file.WriteLine(line);
            //    }
            //}
        }

    }
}
