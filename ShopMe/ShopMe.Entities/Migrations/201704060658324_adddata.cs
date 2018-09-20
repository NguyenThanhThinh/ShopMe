namespace ShopMe.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adddata : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SAN_PHAM_GOI_Y",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.String(maxLength: 128),
                        ProductID = c.Int(nullable: false),
                        Rating = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Districts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProvinceID = c.Int(nullable: false),
                        Name = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Provinces", t => t.ProvinceID, cascadeDelete: true)
                .Index(t => t.ProvinceID);
            
            CreateTable(
                "dbo.Provinces",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Feedbacks",
                c => new
                    {
                        ProductID = c.Int(nullable: false),
                        UserID = c.String(nullable: false, maxLength: 128),
                        CreatedDate = c.DateTime(),
                        Title = c.String(maxLength: 128),
                        Content = c.String(maxLength: 1048),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProductID, t.UserID })
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.ProductID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                        Alias = c.String(nullable: false, maxLength: 256),
                        CategoryID = c.Int(nullable: false),
                        ProductBrandID = c.Int(nullable: false),
                        ProductUnitID = c.Int(nullable: false),
                        Status = c.Boolean(nullable: false),
                        Image = c.String(maxLength: 256),
                        OriginalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(),
                        CreatedDate = c.DateTime(),
                        Description = c.String(maxLength: 500),
                        Content = c.String(),
                        ViewCount = c.Int(),
                        ViewRating = c.Int(),
                        ProductRating = c.Decimal(precision: 18, scale: 2),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.ProductID)
                .ForeignKey("dbo.ProductBrands", t => t.ProductBrandID, cascadeDelete: true)
                .ForeignKey("dbo.ProductCategories", t => t.CategoryID, cascadeDelete: true)
                .ForeignKey("dbo.ProductUnits", t => t.ProductUnitID, cascadeDelete: true)
                .Index(t => t.CategoryID)
                .Index(t => t.ProductBrandID)
                .Index(t => t.ProductUnitID);
            
            CreateTable(
                "dbo.ProductBrands",
                c => new
                    {
                        ProductBrandID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        Phone = c.String(),
                        Address = c.String(),
                        Alias = c.String(nullable: false, maxLength: 256),
                        Description = c.String(),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.ProductBrandID);
            
            CreateTable(
                "dbo.ProductCategories",
                c => new
                    {
                        CategoryID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                        Alias = c.String(nullable: false, maxLength: 256),
                        Description = c.String(maxLength: 500),
                        ParentID = c.Int(),
                        DisplayOrder = c.Int(),
                        Status = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.CategoryID);
            
            CreateTable(
                "dbo.ProductUnits",
                c => new
                    {
                        ProductUnitID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        Status = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.ProductUnitID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FullName = c.String(nullable: false, maxLength: 256),
                        Address = c.String(maxLength: 256),
                        Provinces = c.String(maxLength: 100),
                        Districts = c.String(maxLength: 100),
                        BirthDay = c.DateTime(),
                        IsDelete = c.Boolean(nullable: false),
                        Email = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserClaims",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        Id = c.Int(nullable: false),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.UserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(),
                        ProviderKey = c.String(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.PearsonScores",
                c => new
                    {
                        UserID_1 = c.String(nullable: false, maxLength: 128),
                        UserID_2 = c.String(nullable: false, maxLength: 128),
                        Sim_Pearson_Score = c.Double(),
                    })
                .PrimaryKey(t => new { t.UserID_1, t.UserID_2 })
                .ForeignKey("dbo.Users", t => t.UserID_1, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID_2)
                .Index(t => t.UserID_1)
                .Index(t => t.UserID_2);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        IdentityRole_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.IdentityRole_Id)
                .Index(t => t.UserId)
                .Index(t => t.IdentityRole_Id);
            
            CreateTable(
                "dbo.UserRatingAverages",
                c => new
                    {
                        UserID = c.String(nullable: false, maxLength: 128),
                        RatingAverage = c.Double(),
                    })
                .PrimaryKey(t => t.UserID)
                .ForeignKey("dbo.Users", t => t.UserID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                        Description = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.OrderDetails",
                c => new
                    {
                        OrderID = c.Int(nullable: false),
                        ProductID = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => new { t.OrderID, t.ProductID })
                .ForeignKey("dbo.Orders", t => t.OrderID, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.OrderID)
                .Index(t => t.ProductID);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderID = c.Int(nullable: false, identity: true),
                        CustomerName = c.String(nullable: false, maxLength: 256),
                        CustomerAddress = c.String(nullable: false, maxLength: 256),
                        CustomerEmail = c.String(nullable: false, maxLength: 256),
                        CustomerMobile = c.String(maxLength: 50),
                        CustomerProvince = c.String(),
                        CustomerDistricts = c.String(),
                        PaymentMethod = c.String(maxLength: 256),
                        UserID = c.String(maxLength: 128),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(),
                        PaymentStatus = c.String(),
                        Status = c.Int(),
                    })
                .PrimaryKey(t => t.OrderID)
                .ForeignKey("dbo.Users", t => t.UserID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.RoleGroups",
                c => new
                    {
                        GroupId = c.Int(nullable: false),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.GroupId, t.RoleId })
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.GroupId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Description = c.String(maxLength: 256),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Slides",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Description = c.String(maxLength: 256),
                        Image = c.String(maxLength: 256),
                        DisplayOrder = c.Int(),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SystemConfigs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 50, unicode: false),
                        ValueString = c.String(),
                        ValueInt = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.UserBarnd",
                c => new
                    {
                        UserID = c.String(nullable: false, maxLength: 128),
                        ProductBrandID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserID, t.ProductBrandID })
                .ForeignKey("dbo.ProductBrands", t => t.ProductBrandID, cascadeDelete: true)
                .Index(t => t.ProductBrandID);
            
            CreateTable(
                "dbo.UserCategorys",
                c => new
                    {
                        UserID = c.String(nullable: false, maxLength: 128),
                        CategoryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserID, t.CategoryID })
                .ForeignKey("dbo.ProductCategories", t => t.CategoryID, cascadeDelete: true)
                .Index(t => t.CategoryID);
            
            CreateTable(
                "dbo.UserGroups",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.GroupId })
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.UserRatings",
                c => new
                    {
                        UserID = c.String(nullable: false, maxLength: 128),
                        ProductID = c.Int(nullable: false),
                        Rating = c.Double(),
                    })
                .PrimaryKey(t => new { t.UserID, t.ProductID })
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.ProductID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRoles", "IdentityRole_Id", "dbo.Roles");
            DropForeignKey("dbo.UserRatings", "UserID", "dbo.Users");
            DropForeignKey("dbo.UserRatings", "ProductID", "dbo.Products");
            DropForeignKey("dbo.UserGroups", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserGroups", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.UserCategorys", "CategoryID", "dbo.ProductCategories");
            DropForeignKey("dbo.UserBarnd", "ProductBrandID", "dbo.ProductBrands");
            DropForeignKey("dbo.RoleGroups", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.RoleGroups", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.OrderDetails", "ProductID", "dbo.Products");
            DropForeignKey("dbo.OrderDetails", "OrderID", "dbo.Orders");
            DropForeignKey("dbo.Orders", "UserID", "dbo.Users");
            DropForeignKey("dbo.Feedbacks", "UserID", "dbo.Users");
            DropForeignKey("dbo.UserRatingAverages", "UserID", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.PearsonScores", "UserID_2", "dbo.Users");
            DropForeignKey("dbo.PearsonScores", "UserID_1", "dbo.Users");
            DropForeignKey("dbo.UserLogins", "User_Id", "dbo.Users");
            DropForeignKey("dbo.UserClaims", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Feedbacks", "ProductID", "dbo.Products");
            DropForeignKey("dbo.Products", "ProductUnitID", "dbo.ProductUnits");
            DropForeignKey("dbo.Products", "CategoryID", "dbo.ProductCategories");
            DropForeignKey("dbo.Products", "ProductBrandID", "dbo.ProductBrands");
            DropForeignKey("dbo.Districts", "ProvinceID", "dbo.Provinces");
            DropIndex("dbo.UserRatings", new[] { "ProductID" });
            DropIndex("dbo.UserRatings", new[] { "UserID" });
            DropIndex("dbo.UserGroups", new[] { "GroupId" });
            DropIndex("dbo.UserGroups", new[] { "UserId" });
            DropIndex("dbo.UserCategorys", new[] { "CategoryID" });
            DropIndex("dbo.UserBarnd", new[] { "ProductBrandID" });
            DropIndex("dbo.RoleGroups", new[] { "RoleId" });
            DropIndex("dbo.RoleGroups", new[] { "GroupId" });
            DropIndex("dbo.Orders", new[] { "UserID" });
            DropIndex("dbo.OrderDetails", new[] { "ProductID" });
            DropIndex("dbo.OrderDetails", new[] { "OrderID" });
            DropIndex("dbo.UserRatingAverages", new[] { "UserID" });
            DropIndex("dbo.UserRoles", new[] { "IdentityRole_Id" });
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropIndex("dbo.PearsonScores", new[] { "UserID_2" });
            DropIndex("dbo.PearsonScores", new[] { "UserID_1" });
            DropIndex("dbo.UserLogins", new[] { "User_Id" });
            DropIndex("dbo.UserClaims", new[] { "User_Id" });
            DropIndex("dbo.Products", new[] { "ProductUnitID" });
            DropIndex("dbo.Products", new[] { "ProductBrandID" });
            DropIndex("dbo.Products", new[] { "CategoryID" });
            DropIndex("dbo.Feedbacks", new[] { "UserID" });
            DropIndex("dbo.Feedbacks", new[] { "ProductID" });
            DropIndex("dbo.Districts", new[] { "ProvinceID" });
            DropTable("dbo.UserRatings");
            DropTable("dbo.UserGroups");
            DropTable("dbo.UserCategorys");
            DropTable("dbo.UserBarnd");
            DropTable("dbo.SystemConfigs");
            DropTable("dbo.Slides");
            DropTable("dbo.Roles");
            DropTable("dbo.RoleGroups");
            DropTable("dbo.Orders");
            DropTable("dbo.OrderDetails");
            DropTable("dbo.Groups");
            DropTable("dbo.UserRatingAverages");
            DropTable("dbo.UserRoles");
            DropTable("dbo.PearsonScores");
            DropTable("dbo.UserLogins");
            DropTable("dbo.UserClaims");
            DropTable("dbo.Users");
            DropTable("dbo.ProductUnits");
            DropTable("dbo.ProductCategories");
            DropTable("dbo.ProductBrands");
            DropTable("dbo.Products");
            DropTable("dbo.Feedbacks");
            DropTable("dbo.Provinces");
            DropTable("dbo.Districts");
            DropTable("dbo.SAN_PHAM_GOI_Y");
        }
    }
}
