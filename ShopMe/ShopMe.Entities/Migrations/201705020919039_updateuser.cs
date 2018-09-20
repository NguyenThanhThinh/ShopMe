namespace ShopMe.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateuser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "PasswordBefor", c => c.String(maxLength: 100));
        }

        public override void Down()
        {
            DropColumn("dbo.Users", "PasswordBefor");
        }
    }
}
