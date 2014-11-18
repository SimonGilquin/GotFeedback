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
                new Topic { Id = 1, Title = "Comment je peux r�initialiser mon mot de passe?", LikesCount = 0, ViewCount = 5, CreatedDate = DateTime.Now },
                new Topic { Id = 2, Title = "Je n'ai pas re�u l'email de confirmation.", LikesCount = 12, ViewCount = 9, CreatedDate = DateTime.Now });

            context.Comments.AddOrUpdate(c => c.Id,
                new Comment { TopicId = 1, Message = "Exemple de commentaire un...", Date = DateTime.Now},
                new Comment { TopicId = 1, Message = "Commentaire...", Date = DateTime.Now },
                new Comment { TopicId = 1, Message = "Exemple de commentaire 2...", Date = DateTime.Now },
                new Comment { TopicId = 2, Message = "Commentaire Topic 2.", Date = DateTime.Now });

            context.Tags.AddOrUpdate(t => t.Id, 
                new Tag{ TopicId = 1, Label = "Question"},
                new Tag{ TopicId = 1, Label = "Password"},
                new Tag{ TopicId = 1, Label = "Account"},
                new Tag{ TopicId = 2, Label = "Access"},
                new Tag{ TopicId = 2, Label = "Avatar"}
                );
        }
    }
}
