using app.Domain.Agregate.Entities;
using app.Domain.DTO.Service;

namespace app.Domain.DomainService
{
    public class SumPriceOS
    {
        private Service _service;
        private List<ServiceSubCategoryDb> _subCategories;
        private List<VaccineDbDTO> _vaccines;
        public SumPriceOS(Service service, List<ServiceSubCategoryDb> subCategories, List<VaccineDbDTO> vaccines)
        {
            _service = service;
            _subCategories = subCategories;
            _vaccines = vaccines;
        }

        public float? sum()
        {
            if (_service.codeCategory == "C04")
            {
                return countSum(_vaccines);
            }else if(_service.subcategories.Count != 0)
            {
                return countSum(_subCategories);
            }
            return _service.price;
        }
        private float countSum(List<ServiceSubCategoryDb> subCategories)
        {
            float sum = 0;
            foreach (var subCategory in subCategories)
            {
                sum += subCategory.price;
            }
            return sum;
        }
        private float countSum(List<VaccineDbDTO> vaccines)
        {
            float sum = 0;
            foreach (var vaccine in vaccines)
            {
                sum += vaccine.price;
            }
            return sum;
        }
    }
}
