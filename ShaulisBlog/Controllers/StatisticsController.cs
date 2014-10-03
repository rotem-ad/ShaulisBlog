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
            var postsPerMonth = db.Posts.GroupBy(x => new { x.PublishDate.Year, x.PublishDate.Month }).Select(g => new { Key = g.Key, Count = g.Count() }).OrderBy(x => x.Key.Year).ThenBy(x => x.Key.Month);
            string path = Server.MapPath("ShowPostsPerMonthResult.tsv");
            
            //write to file
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, false))
            {
               string line = string.Format("{0}\t{1}", "Item", "Count"); //do headline
               file.WriteLine(line);

               foreach (var data in postsPerMonth) {
                   line = string.Format("{0}/{1}\t{2}", data.Key.Month, data.Key.Year, data.Count); 
                   file.WriteLine(line);
               }
            }

            //How many comments were added per month?   --get the data
            var commentsPerMonth = db.Comments.GroupBy(x => new { x.Post.PublishDate.Year, x.Post.PublishDate.Month }).Select(g => new { Key = g.Key, Count = g.Count() }).OrderBy(x => x.Key.Year).ThenBy(x => x.Key.Month);
            path = Server.MapPath("ShowCommentsPerMonthResult.tsv");
            //write to file
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, false))
            {
                string line = string.Format("{0}\t{1}", "Item", "Count"); //do headline
                file.WriteLine(line);

                foreach (var data in commentsPerMonth)
                {
                    line = string.Format("{0}/{1}\t{2}", data.Key.Month, data.Key.Year, data.Count);
                    file.WriteLine(line);
                }
            }

            //How many fans do we have?
            var fansCount = db.Fans.GroupBy(x=> x.Seniority).Select(g=> new {Key = g.Key, Count = g.Count()}).OrderBy(x=>x.Key);
            path = Server.MapPath("ShowFansResult.tsv");
            //write to file
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, false))
            {
                string line = string.Format("{0}\t{1}", "Item", "Count"); //do headline
                file.WriteLine(line);

                foreach (var data in fansCount)
                {
                    line = string.Format("Seniority:{0}\t{1}", data.Key, data.Count); 
                    file.WriteLine(line);
                }
            }
        }

    }

   
}
