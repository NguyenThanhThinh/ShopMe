namespace ShopMe.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addProdcutcapcity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "ProductCappacity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "ProductCappacity");
        }
    }
}
