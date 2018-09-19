using ShopMe.Entities.Models;
using ShopMe.Reository.Interface;
using ShopMe.Repository;

namespace ShopMe.Reository.Repositories
{
    public  interface IListSugggetRepository:IRepository<DanhSachGoiSanPham>
    {
    }
    public class ListSugggetRepository : RepositoryBase<DanhSachGoiSanPham>, IListSugggetRepository
    {
        public ListSugggetRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
