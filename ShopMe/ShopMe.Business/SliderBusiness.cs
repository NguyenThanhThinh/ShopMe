using ShopMe.Entities.Models;
using ShopMe.Reository.Interface;
using ShopMe.Reository.Repositories;
using System.Collections.Generic;

namespace ShopMe.Business
{
    public interface ISliderBusiness
    {
        Slide Add(Slide slider);

        void Update(Slide slider);

        Slide Delete(int id);

        IEnumerable<Slide> GetAll();

        IEnumerable<Slide> GetAll(string keyword);

     

        Slide GetById(int id);

        void Save();
    }
    public class SliderBusiness : ISliderBusiness
    {
        private readonly ISliderRepository _sliderRepository;
        private readonly IUnitOfWork _unitOfWork;
        public SliderBusiness(ISliderRepository sliderRepository, IUnitOfWork unitOfWork)
        {
            this._sliderRepository = sliderRepository;
            this._unitOfWork = unitOfWork;
        }
        public Slide Add(Slide slider)
        {
            return _sliderRepository.Add(slider);
        }

        public Slide Delete(int id)
        {
            return _sliderRepository.Delete(id);
        }

        public IEnumerable<Slide> GetAll()
        {
            return _sliderRepository.GetAll();
        }

        public IEnumerable<Slide> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _sliderRepository.GetMulti(x => x.Description.Contains(keyword));
            else
                return _sliderRepository.GetAll();
        }


        public Slide GetById(int id)
        {
            return _sliderRepository.GetSingleById(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(Slide slider)
        {
            _sliderRepository.Update(slider);
        }
    }
}
