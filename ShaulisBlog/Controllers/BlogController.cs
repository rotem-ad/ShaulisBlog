using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShaulisBlog.Models;
using ShaulisBlog.DAL;
using ShaulisBlog.Filters;

namespace ShaulisBlog.Controllers
{
    public class BlogController : Controller
    {
        private BlogDBContext db = new BlogDBContext();

        //
        // GET: /Blog/
        [InitializeSimpleMembership]
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
        public ActionResult FilterPosts(int minComments, DateTime fromDate, DateTime untilDate, string postTitle = "", string wordsInComments = "", string commentWriter = "")
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
                                p.Title.ToLower().Contains(postTitle.ToLower()) &&
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
                                p.Title.ToLower().Contains(postTitle.ToLower()) 
                                select p;
            }

            // If "Comment Writer" was given as filter and "Title", "Comments contain" were NOT given as filter
            if ((commentWriter != string.Empty) && (postTitle == string.Empty) && (wordsInComments == string.Empty))
            {
                filteredPosts = from p in db.Posts
                                join c in db.Comments on p.PostID equals c.PostID
                                where c.Writer.ToUpper().Contains(commentWriter.ToUpper()) &&
                                p.PublishDate >= fromDate &&
                                p.PublishDate <= untilDate
                                select p;
            }

            // If none of "Title", "Comments contain", "Comment Writer" were given as filter
            if ((commentWriter == string.Empty) && (postTitle == string.Empty) && (wordsInComments == string.Empty))
            {
                filteredPosts = from p in db.Posts
                                where p.Comments.Count() >= minComments &&
                                p.PublishDate >= fromDate &&
                                p.PublishDate <= untilDate
                                select p;
            }

            // Make sure to return list with distinct values to avoid duplicate posts in the view
            return PartialView("_PostsList", filteredPosts.ToList().Distinct());
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

        //
        // POST: /Blog/FilterComments
        /*
        * Method which handles comments searches from ManageComments view
        */
        [HttpPost]
        public ActionResult FilterComments(string writer, string contains, int PostId)
        {
            Post post = db.Posts.Find(PostId);  // Holds the relevant post, according to PostId

            IEnumerable<Comment> filteredComments = post.Comments; // Holds all of its comments

            // If "Writer" was given as input parameter
            if (!String.IsNullOrWhiteSpace(writer))
                filteredComments = filteredComments.Where(f => f.Writer.ToLower().Contains(writer.ToLower()));
            
            // If "Contains" was given as input parameter
            if (!String.IsNullOrWhiteSpace(contains))
                filteredComments = filteredComments.Where(f => f.Content.ToLower().Contains(contains.ToLower()));

            // Update ViewBag.commentList for "_CommentList" partial view
            ViewBag.commentList = filteredComments.ToList();

            // Return "_CommentList" partial view
            return PartialView("_CommentList");

        }      

        //
        // GET: /Blog/Admin
        /*
         * Method which handles the Admin view
         */
        [Authorize(Roles="Administrator")]
        public ActionResult Admin()
        {
            return View(db.Posts.ToList());
        }

        //
        // GET: /Blog/ManageComments
        /*
         * Method which handles the Manage Comments view
         */
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
        public ActionResult AddNewPost()
        {
            return View();
        }

        //
        // POST: /Blog/AddNewPost

        [HttpPost]
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Admin");
        }

         public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}