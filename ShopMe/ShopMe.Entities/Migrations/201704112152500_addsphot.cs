namespace ShopMe.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addsphot : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("ProductHotSP", @"Select top(8) p.ProductID,p.Name,p.CategoryID,p.Alias,p.Image,p.Quantity,p.ProductRating,p.price,p.ViewRating,p.ViewCount,SUM(o.Quantity)as soluong
From  OrderDetails o
Left join Products p
on o.ProductID=p.ProductID
group by p.ProductID,p.Name,p.CategoryID,p.Image,p.Quantity,p.ProductRating,p.Alias,p.price,p.ViewRating,p.ViewCount
ORDER BY soluong DESC");
        }

        public override void Down()
        {
            DropStoredProcedure("dbo.ProductHotSP");
        }
    }
}
