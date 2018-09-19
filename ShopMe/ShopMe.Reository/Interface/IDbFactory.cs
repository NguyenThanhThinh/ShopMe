using ShopMe.Entities;
using System;

namespace ShopMe.Reository.Interface
{
    public interface IDbFactory : IDisposable
    {
        ShopMeDbContext Init();
    }
}