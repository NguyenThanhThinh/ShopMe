using ShopMe.Entities.Models;
using ShopMe.Reository.Interface;
using ShopMe.Repository.Repositories;

namespace ShopMe.Business
{
    public interface IErrorBusiness
    {
        Error Create(Error error);
        void Save();
    }
    public class ErrorBusiness : IErrorBusiness
    {
        readonly IErrorRepository _errorRepository;
        readonly IUnitOfWork _unitOfWork;
        public ErrorBusiness(IErrorRepository errorRepository, IUnitOfWork unitOfWork)
        {
            this._errorRepository = errorRepository;
            this._unitOfWork = unitOfWork;
        }
        public Error Create(Error error)
        {
            return _errorRepository.Add(error);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
