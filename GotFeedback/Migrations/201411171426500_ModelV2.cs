namespace GotFeedback.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelV2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Topics", "Title", c => c.String(nullable: false));
            DropColumn("dbo.Topics", "Message");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Topics", "Message", c => c.String());
            DropColumn("dbo.Topics", "Title");
        }
    }
}
