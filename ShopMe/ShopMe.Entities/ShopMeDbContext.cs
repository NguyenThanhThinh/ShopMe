using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using ShopMe.Entities.Models;

namespace ShopMe.Entities
{
    public class ShopMeDbContext : DbContext
    {
        public ShopMeDbContext() : base("shopmedbcontext")
        {
            Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Group> Groups { get; set; }
        public DbSet<SystemConfig> SystemConfigs { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<PearsonScore> PearsonScores { get; set; }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductUnit> ProductUnit { get; set; }

        public DbSet<ProductBrand> ProductBrands { get; set; }
        public DbSet<ProductCategory> ProductCategorys { get; set; }

        //public DbSet<ProductDetail> ProductDetails { get; set; }


        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleGroup> RoleGroups { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<Slide> Slide { get; set; }

        public DbSet<UserRating> UserRatings { get; set; }
        public DbSet<UserRatingAverage> UserRatingAverages { get; set; }
        public DbSet<Districts> Districtss { get; set; }
        public DbSet<Provinces> Provincess { get; set; }

        public DbSet<DanhSachGoiSanPham> DanhSachGoiSanPhams { get; set; }
        //public DbSet<ThongTinHoTro> ThongTinHoTros { get; set; }

        public DbSet<UserBarnd> UserBarnds { get; set; }
        public DbSet<UserCategory> UserCategorys { get; set; }

        //public DbSet<Post> Posts { get; set; }
        //public DbSet<PostCategory> PostCategorys { get; set; }

        public static ShopMeDbContext Create()
        {
            return new ShopMeDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(e => e.PearsonScore)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.UserID_1);

            modelBuilder.Entity<User>()
                .HasMany(e => e.PearsonScore1)
                .WithRequired(e => e.User1)
                .HasForeignKey(e => e.UserID_2)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<IdentityUserRole>().HasKey(i => new {i.UserId, i.RoleId}).ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserLogin>().HasKey(i => i.UserId).ToTable("UserLogins");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserClaim>().HasKey(i => i.UserId).ToTable("UserClaims");
        }
    }
}