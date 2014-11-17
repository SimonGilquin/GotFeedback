using GotFeedback.Models;

namespace GotFeedback.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<GotFeedback.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "GotFeedback.Models.ApplicationDbContext";
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(GotFeedback.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            context.Topics.AddOrUpdate(t => t.Id,
                new Topic { Id = 1, Title = "Comment je peux réinitialiser mon mot de passe?", CreatedDate = DateTime.Now},
                new Topic { Id = 2, Title = "Je n'ai pas reçu l'email de confirmation.", CreatedDate = DateTime.Now });

            context.Comments.AddOrUpdate(c => c.Id,
                new Comment { TopicId = 1, Message = "Exemple de commentaire un..." },
                new Comment { TopicId = 1, Message = "Commentaire..." },
                new Comment { TopicId = 1, Message = "Exemple de commentaire 2..." },
                new Comment { TopicId = 2, Message = "Commentaire Topic 2." });
        }
    }
}
