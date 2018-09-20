namespace ShopMe.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class addSP : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("RatingProductSP", p => new
            {
                ProductID = p.Int(),
                UserID = p.String(),
                Rating = p.Int()
            }
                ,
                @"IF exISTS (SELECT * FROM UserRatings krs WHERE krs.ProductID = @ProductID AND krs.UserID =@UserID)
	BEGIN
		UPDATE UserRatings
		SET
			Rating = @Rating
		WHERE UserID = @UserID AND ProductID = @ProductID
	END
	ELSE
	BEGIN
	Insert Into UserRatings
		(ProductID,UserID,Rating)
	Values
		(@ProductID,@UserID,@Rating)		
	END
	-- UPDATE RATE SAN PHAM
	DECLARE @Rating_SP FLOAT
	DECLARE @SONGUOI_RATE INT
	
	SELECT @Rating_SP = AVG(krs.Rating)
	FROM UserRatings krs
	WHERE krs.ProductID = @ProductID
	
	SELECT @SONGUOI_RATE = COUNT(*) FROM UserRatings krs WHERE krs.ProductID = @ProductID
	
	UPDATE Products
	SET
		ViewRating = @SONGUOI_RATE,
		ProductRating = @Rating_SP
	WHERE ProductID = @ProductID");
        }

        public override void Down()
        {
            DropStoredProcedure("dbo.RatingProductSP");
        }
    }
}
