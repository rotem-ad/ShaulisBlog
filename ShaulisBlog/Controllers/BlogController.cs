using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShaulisBlog.Models;
using ShaulisBlog.DAL;

namespace ShaulisBlog.Controllers
{
    public class BlogController : Controller
    {
        private BlogDBContext db = new BlogDBContext();

        //
        // GET: /Blog/

        public ActionResult Index()
        {
            return View(db.Posts.ToList());
        }

        //
        // POST: /Blog/FilterPosts
        /*
        * Method which handles posts searches from Index view
        */
        [HttpPost]
        public ActionResult FilterPosts(int minComments, DateTime fromDate, DateTime untilDate, string postTitle = "", string wordsInComments = "")
        {
            IEnumerable<Post> filteredPosts = db.Posts; // Holds the result set

            // If both "Title" and "Comments contain" were given as filter
            if ((postTitle != string.Empty) && (wordsInComments != string.Empty))
            {
                filteredPosts = from p in db.Posts
                                join c in db.Comments on p.PostID equals c.PostID
                                where p.Comments.Count() >= minComments &&
                                p.PublishDate >= fromDate &&
                                p.PublishDate <= untilDate &&
                                p.Title.ToLower() == postTitle.ToLower() &&
                                c.Content.Contains(wordsInComments)
                                select p;
            }

            // If "Comments contain" was given as filter and "Title" field was blank
            if ((postTitle == string.Empty) && (wordsInComments != string.Empty))
            {
                filteredPosts = from p in db.Posts
                                join c in db.Comments on p.PostID equals c.PostID
                                where p.Comments.Count() >= minComments &&
                                p.PublishDate >= fromDate &&
                                p.PublishDate <= untilDate &&
                                c.Content.Contains(wordsInComments)
                                select p;
            }

            // If "Title" was given as filter and "Comments contain" field was blank
            if ((postTitle != string.Empty) && (wordsInComments == string.Empty))
            {
                filteredPosts = from p in db.Posts
                                where p.Comments.Count() >= minComments &&
                                p.PublishDate >= fromDate &&
                                p.PublishDate <= untilDate &&
                                p.Title.ToLower() == postTitle.ToLower() 
                                select p;
            }

            // If neither "Title" or "Comments contain" were given as filter
            if ((postTitle == string.Empty) && (wordsInComments == string.Empty))
            {
                filteredPosts = from p in db.Posts
                                where p.Comments.Count() >= minComments &&
                                p.PublishDate >= fromDate &&
                                p.PublishDate <= untilDate
                                select p;
            }

            // Make sure to return list with distinct values to avoid duplicate posts in the view
            return View("Index", filteredPosts.ToList().Distinct());
        }
       
        
        //only server side
        /*
        * Method which handles postTitle searches by writer comments
        */
        public ActionResult FilterPostsByCommentWriter(string commentWriter = "")
        {
            IEnumerable<Post> filteredPost = db.Posts; // Holds the result set

            
            if (commentWriter != string.Empty)
            {
                filteredPost = from p in db.Posts
                                join c in db.Comments on p.PostID equals c.PostID
                                where c.Writer.ToUpper() == commentWriter.ToUpper()
                                select p;
            }

            

            // Make sure to return list with distinct values to avoid duplicate posts in the view
            return View("Index", filteredPost.ToList().Distinct());

        }

        /*
        *  Method which group by  number of comments for writer
        */
        public ActionResult FilterNumberOfComments(string commentWriter = "")
        {
           // IEnumerable<Post> filteredComments = db.Comments;
            int number = 0;
            if (commentWriter != string.Empty)
            {
               // filteredComments = from c in db.Comments 
                   //      group c by c.Writer.Count();
                        // where c.Writer.ToUpper() == commentWriter.ToUpper()
                       //  select c.CommentID.Count();   
           

                
            }
            // Make sure to return list with distinct values to avoid duplicate posts in the view
            return View(number);

        }

       

        //
        // GET: /Blog/Admin
        /*
         * Method which handles the Admin view
         */
        public ActionResult Admin()
        {
            return View(db.Posts.ToList());
        }

        //
        // GET: /Blog/ManageComments
        /*
         * Method which handles the Manage Comments view
         */
        public ActionResult ManageComments(int id = 0)
        {
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        //
        // POST: /Blog/DeleteComment/5
        /*
        * Method which handles the comments deletion
        */
        [HttpPost, ActionName("DeleteComment")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteComment(int id = 0)
        {
            Comment comment = db.Comments.Find(id);
            // Before deletion, get this comment's related post ID
            int postID = comment.PostID;
            // Delete comment from DB
            db.Comments.Remove(comment);
            db.SaveChanges();

            // Display Manage Comments view of the deleted comment
            return RedirectToAction("ManageComments", new { id = postID });
        }

        //
        // GET: /Blog/Details/5

        public ActionResult Details(int id = 0)
        {
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        //
        // GET: /Blog/AddNewPost

        public ActionResult AddNewPost()
        {
            return View();
        }

        //
        // POST: /Blog/AddNewPost

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewPost(Post post)
        {
            // Get current date and update post PublishDate property 
            DateTime currDate = DateTime.Now;
            post.PublishDate = currDate;

            ModelState["PublishDate"].Errors.Clear(); // Required to make ModelState valid

            if (ModelState.IsValid)
            {
                // Add new post to Post table in DB
                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Admin");
            }

            return View(post);
        }

        
        
        // POST: /Blog/CreateComment
        /*
         * Method which adds new comment to given post.
         * Called after submitting the form in _PostComment partial view 
         */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateComment(Comment comment)
        {
            if (ModelState.IsValid)
            {
                // Add new comment to Comment table in DB
                db.Comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(comment);
        }

        //
        // GET: /Blog/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        //
        // POST: /Blog/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Admin");
            }
            return View(post);
        }

        //
        // GET: /Blog/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        //
        // POST: /Blog/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Admin");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}