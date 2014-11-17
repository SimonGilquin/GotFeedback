using System;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using GotFeedback.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GotFeedback.Controllers
{
    public class TopicsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public TopicsController()
        {
            db = new ApplicationDbContext();
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        // GET: Topics
        public async Task<ActionResult> Index()
        {
            return View(await db.Topics.ToListAsync());
        }

        // GET: Topics/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var topic = await db.Topics.Select(t=>new
            {
                Details = new TopicDetails
                {
                    Id = t.Id,
                    Category = t.Category,
                    CreatedDate = t.CreatedDate,
                    Username = t.User.UserName
                },
                Email = t.User.Email
            }).SingleOrDefaultAsync(t=>t.Details.Id==id);

            if (topic == null)
            {
                return HttpNotFound();
            }

            topic.Details.GravatarUrl = string.Format("http://www.gravatar.com/avatar/{0}",
                BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(topic.Email.ToLowerInvariant())))
                    .Replace("-", "")
                    .ToLowerInvariant());

         //   ViewBag.Comments = db.Comments.Where(c => c.TopicId == topic.Id);

            return View(topic.Details);
        }

        // GET: Topics/New
        public ActionResult New()
        {
            return ControllerContext.IsChildAction ? (ActionResult)PartialView() : View();
        }

        // POST: Topics/New
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> New([Bind(Include = "Id,Title,CreatedDate,Category")] Topic topic)
        {
            if (ModelState.IsValid)
            {
                topic.CreatedDate = DateTime.Now;
                topic.User = userManager.FindById(User.Identity.GetUserId()); ;

                db.Topics.Add(topic);
                await db.SaveChangesAsync();
                TempData["SuccessMessage"] =
                    "Your topic have been created. You can now add details to help the community help you.";
                return RedirectToAction("Details", new { id = topic.Id });
            }
            return View(topic);
        }

        // GET: Topics/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = await db.Topics.FindAsync(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }

        // POST: Topics/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,CreatedDate")] Topic topic)
        {
            if (ModelState.IsValid)
            {
                db.Entry(topic).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(topic);
        }

        // GET: Topics/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = await db.Topics.FindAsync(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }

        // POST: Topics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Topic topic = await db.Topics.FindAsync(id);
            db.Topics.Remove(topic);
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
