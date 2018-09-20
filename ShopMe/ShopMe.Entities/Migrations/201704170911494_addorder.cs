namespace ShopMe.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addorder : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "PaymentStatus", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "PaymentStatus", c => c.String());
        }
    }
}
