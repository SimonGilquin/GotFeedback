using System;
using System.Collections.Generic;
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
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Configuration;
using System.Web;

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

        public ActionResult Index(TopicsOrderBy order = TopicsOrderBy.None)
        {
            var query = db.Topics.Select(t => new
            {
                Details = new TopicDetails
                {
                    Id = t.Id,
                    Category = t.Category,
                    CreatedDate = t.CreatedDate,
                    Username = t.User.UserName,
                    Title = t.Title,
                    IsOwner = t.User.UserName == User.Identity.Name,
                    ViewCount = t.ViewCount,
                    LikesCount = t.LikesCount,
                    TagLabels = t.Tags.Select(tag => tag.Label)
                },
                Email = t.User.Email,
            });
            switch (order)
            {
                case TopicsOrderBy.ViewCount:
                    query = query.OrderByDescending(t => t.Details.ViewCount);
                    break;
                case TopicsOrderBy.CreatedDate:
                    query = query.OrderByDescending(t => t.Details.CreatedDate);
                    break;
                default:
                    query = query.OrderByDescending(t => t.Details.CreatedDate);
                    break;
            }
            var topics = query.ToList().Select(t =>
            {
                var topic = t.Details;
                if (t.Email != null)
                {
                    topic.GravatarUrl = string.Format("http://www.gravatar.com/avatar/{0}",
                BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(t.Email.ToLowerInvariant())))
                    .Replace("-", "")
                    .ToLowerInvariant());

                }
                return topic;
            }).ToList();
            return ControllerContext.IsChildAction ? (ActionResult)PartialView(topics) : View(topics);
        }

        [HttpPost]
        public void UpdateTagsCollection(Topic topic)
        {
            if(topic == null) return;
            
            var tags = topic.TagsLiteral.Split(',');

            foreach (var tag in tags)
            {
                if (topic.Tags.Any(t => t.Label.Equals(tag, StringComparison.OrdinalIgnoreCase))) continue;

                Tag newTag = new Tag { TopicId = topic.Id, Label = tag };
                db.Tags.Add(newTag);
            }

            db.SaveChanges();
        }

        // GET: Topics/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var currentTopic = await db.Topics.SingleOrDefaultAsync(t => t.Id == id);
            if (currentTopic != null)
            {



                currentTopic.ViewCount++;
                db.Entry(currentTopic).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }

            var topic = await db.Topics.Select(t => new
            {
                Details = new TopicDetails
                {
                    Id = t.Id,
                    Category = t.Category,
                    CreatedDate = t.CreatedDate,
                    Username = t.User.UserName,
                    Title = t.Title,
                    IsOwner = t.User.UserName == User.Identity.Name,
                    Tags = t.Tags.ToList()
                },
                Email = t.User.Email
            }).SingleOrDefaultAsync(t => t.Details.Id == id);

            if (topic == null)
            {
                return HttpNotFound();
            }

            if (topic.Email != null)
            {
                topic.Details.GravatarUrl = string.Format("http://www.gravatar.com/avatar/{0}",
            BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(topic.Email.ToLowerInvariant())))
                .Replace("-", "")
                .ToLowerInvariant());

            }

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
                UpdateTagsCollection(topic);
                await db.SaveChangesAsync();
                return RedirectToAction("Details", new { topic.Id });
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

        [HttpPost]
        public void UploadImage(HttpPostedFileBase file)
        {
            string filename = System.IO.Path.GetFileName(file.FileName.ToString());

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
                       
            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference("mycontainer");

            // Retrieve reference to a blob named "myblob".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference("myblob");

            // Create or overwrite the "myblob" blob with contents from a local file.
            using (var fileStream = System.IO.File.OpenRead(filename))
            {
                blockBlob.UploadFromStream(fileStream);
            }

        }

        public void DownloadImage()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference("mycontainer");

            // Retrieve reference to a blob named "photo1.jpg".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference("photo1.jpg");

            // Save blob contents to a file.
            using (var fileStream = System.IO.File.OpenWrite(@"path\myfile"))
            {
                blockBlob.DownloadToStream(fileStream);
            }
        }

        public void ListImages()
        {
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);


            // Create the blob client. 
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference("photos");

            // Loop over items within the container and output the length and URI.
            foreach (IListBlobItem item in container.ListBlobs(null, false))
            {
                if (item.GetType() == typeof(CloudBlockBlob))
                {
                    CloudBlockBlob blob = (CloudBlockBlob)item;

                    Console.WriteLine("Block blob of length {0}: {1}", blob.Properties.Length, blob.Uri);

                }
                else if (item.GetType() == typeof(CloudPageBlob))
                {
                    CloudPageBlob pageBlob = (CloudPageBlob)item;

                    Console.WriteLine("Page blob of length {0}: {1}", pageBlob.Properties.Length, pageBlob.Uri);

                }
                else if (item.GetType() == typeof(CloudBlobDirectory))
                {
                    CloudBlobDirectory directory = (CloudBlobDirectory)item;

                    Console.WriteLine("Directory: {0}", directory.Uri);
                }
            }
        }

        public async Task<ActionResult> AddLike(int id)
        {
            Topic topic = await db.Topics.FindAsync(id);

            if (topic == null) return RedirectToAction("Index", "Topics");

            topic.LikesCount++;
            db.Entry(topic).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return RedirectToAction("Index", "Topics");
        }

        [ActionName("Search")]
        [ValidateAntiForgeryToken]
        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> Search(FormCollection formCollection)
        {
            var searchString = formCollection["searchString"];
            var topics = await
                db.Topics.Where(t => t.Title.Contains(searchString)).ToListAsync();

            return View("Index", topics);
        }

    }

    public enum TopicsOrderBy
    {
        ViewCount = 0,
        CreatedDate = 1,
        None = 2
    }

}

