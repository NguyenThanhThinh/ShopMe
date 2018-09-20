namespace ShopMe.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addfeed : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Feedbacks", "UserID", "dbo.Users");
            DropIndex("dbo.Feedbacks", new[] { "UserID" });
            DropPrimaryKey("dbo.Feedbacks");
            AddColumn("dbo.Feedbacks", "FeedbackID", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Feedbacks", "UserID", c => c.String(maxLength: 128));
            AddPrimaryKey("dbo.Feedbacks", "FeedbackID");
            CreateIndex("dbo.Feedbacks", "UserID");
            AddForeignKey("dbo.Feedbacks", "UserID", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Feedbacks", "UserID", "dbo.Users");
            DropIndex("dbo.Feedbacks", new[] { "UserID" });
            DropPrimaryKey("dbo.Feedbacks");
            AlterColumn("dbo.Feedbacks", "UserID", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.Feedbacks", "FeedbackID");
            AddPrimaryKey("dbo.Feedbacks", new[] { "ProductID", "UserID" });
            CreateIndex("dbo.Feedbacks", "UserID");
            AddForeignKey("dbo.Feedbacks", "UserID", "dbo.Users", "Id", cascadeDelete: true);
        }
    }
}
