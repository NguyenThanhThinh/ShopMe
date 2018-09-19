using ShopMe.Entities;
using ShopMe.Reository.Interface;

namespace ShopMe.Repository
{
    public class DbFactory : Disposable, IDbFactory
    {
        private ShopMeDbContext dbContext;

        public ShopMeDbContext Init()
        {
            return dbContext ?? (dbContext = new ShopMeDbContext());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}