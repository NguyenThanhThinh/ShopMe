namespace ShopMe.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class addstatic : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("GetRevenueStatisticSP", p => new
            {
                fromDate = p.String(),
                toDate = p.String()
            }, @" select
                o.CreatedDate as Date,
                sum(od.Quantity*od.Price) as Revenues,
                sum((od.Quantity*od.Price)-(od.Quantity*p.OriginalPrice)) as Benefit
                from Orders o
                inner join OrderDetails od
                on o.OrderId = od.OrderId
                inner join Products p
                on od.ProductID  = p.ProductID
                where o.CreatedDate <= cast(@toDate as datetime) and o.CreatedDate >= cast(@fromDate as datetime)
                group by o.CreatedDate");
        }

        public override void Down()
        {
            DropStoredProcedure("dbo.GetRevenueStatisticSP");
        }
    }
}