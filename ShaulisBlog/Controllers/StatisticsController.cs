using ShaulisBlog.DAL;
using ShaulisBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShaulisBlog.Controllers
{
    public class StatisticsController : Controller
    {

        Statistics statistics = new Statistics();


        //
        // GET: /Statistics/

        public ActionResult Index()
        {
            //generate statistic files
            //statistics.GenerateStatisticFiles();

            return View();
        }


        public string ReGenerateStatisticFiles()
        {
            try
            {
                statistics.GenerateStatisticFiles(); //Generate new statistic files
                return "OK";
            }
            catch (Exception ex) {
                return "ERROR";
            }
        }
    }

   
}
