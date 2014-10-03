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
            //How many posts were added per month?   
            var postsPerMonth = db.Posts.GroupBy(a => a.PublishDate.Month).Select(g => new { StateId = g.Key, Count = g.Count() });


            // Example #4: Append new text to an existing file. 
            // The using statement automatically closes the stream and calls  
            // IDisposable.Dispose on the stream object. 
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\miri_k\Source\Repos\ShaulisBlog\ShaulisBlog\postsPerMonth.tsv", true))
            {
               string line = string.Format("{0}\t{1}", "Month", "PostsCount"); //do headline
               file.WriteLine(line);

               foreach (var data in postsPerMonth) {
                   line = string.Format("{0}\t{1}", data.StateId, data.Count); //do headline
                   file.WriteLine(line);
               }
            }
        }

    }
}
