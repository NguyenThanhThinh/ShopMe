namespace ShopMe.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addupdateuser2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Users", "PasswordBefor");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "PasswordBefor", c => c.String(maxLength: 100));
        }
    }
}
