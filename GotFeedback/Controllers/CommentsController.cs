using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using System.Web.Routing;
using GotFeedback.Hubs;
using GotFeedback.Models;
using Microsoft.AspNet.SignalR;

namespace GotFeedback.Controllers
{
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly IHubContext topicHub;

        public CommentsController()
        {
            topicHub = GlobalHost.ConnectionManager.GetHubContext<TopicHub>(); 

        }

        // GET: Comments
        public ActionResult Index(int id)
        {
            var result = db.Comments.Where(c => c.TopicId == id).OrderByDescending(c=>c.Date).ToList();
            ViewBag.TopicId = id;
            return ControllerContext.IsChildAction ? (ActionResult)PartialView(result) : View(result);
        }

        // GET: Comments/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = await db.Comments.FindAsync(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Comments/New

        public ActionResult New()
        {
            return ControllerContext.IsChildAction ? (ActionResult)PartialView() : View();
        }

        // POST: Comments/New
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> New([Bind(Include = "TopicId,Message")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.Date = DateTime.Now;
                db.Comments.Add(comment);
                await db.SaveChangesAsync();

                var topic = await db.Topics.SingleOrDefaultAsync(t => t.Id == comment.TopicId);
                topicHub.Clients.All.notify(new
                {
                    topicId = topic.Id,
                    topic = topic.Title,
                    comment = comment.Message,
                    commentId = comment.Id
                });

                return RedirectToAction("Details", "Topics", new { id = comment.TopicId });
            }

            return View(comment);
        }

        // GET: Comments/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = await db.Comments.FindAsync(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,TopicId,Message")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(comment);
        }

        // GET: Comments/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = await db.Comments.FindAsync(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Comment comment = await db.Comments.FindAsync(id);
            db.Comments.Remove(comment);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
