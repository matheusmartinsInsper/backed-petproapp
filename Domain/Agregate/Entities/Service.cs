using app.Domain.Agregate.ObjectValues;
using app.Domain.DTO.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace app.Domain.Agregate.Entities
{
    public class Service
    {
        private CategoryService categoryService;
        private Price Price;
        private object _nameCategory;
        private string _codeCategory;
        private string _status;
        private bool _wasdeleted;
        private List<VaccineDbDTO> _vaccines;
        private List<string> _attendancemodelspermission = new List<string> { "Online","Presencial","Domiciliar" };
        private List<string> _attendancemodels;
        public bool havesubcategory;
        public List<ServiceSubCategoryDb> subcategories;
        public float? price;
        public string titleService;
        public string idService;
        public string description;
        public string idUser;

        public List<VaccineDbDTO> vaccines { get {  return _vaccines; } }
        public object nameCategory { get { return _nameCategory; } }
        public string codeCategory { get { return _codeCategory; } }
        public List<string> attendancemodels { get { return _attendancemodels; } }
        public string status { get { return _status; } }
        private Service() { }
        public static Service create(ServiceDTO servicedto)
        {
            string idservice = Guid.NewGuid().ToString("N");
            Service service = new Service();
            service.Price = new Price();
            VaccineService vaccines = new VaccineService();
            service.categoryService = new CategoryService();
            List<VaccineDbDTO> vacciensofservice = service.buildVaccines(servicedto.vaccines, service.Price, vaccines, idservice);
            List<ServiceSubCategoryDb> subcategorysofservice = service.buildServiceSubcategory(servicedto.subcategories, service.Price); 
            service._codeCategory = servicedto.codecategory;
            service._status = "Rascunho";
            service.subcategories = service.vallidsubcategory(servicedto,subcategorysofservice);
            service._nameCategory = service.categoryService.GetCategory(service._codeCategory);
            service.titleService = servicedto.title;
            service.idService = idservice;
            service.idUser = servicedto.idUser;
            service.description = servicedto.description;
            service._wasdeleted = false;
            service.price = service.validPriceOfService(servicedto);
            service._vaccines = service.vallidvaccine(servicedto, vacciensofservice);
            service._attendancemodels = service.setAttendanceModel(servicedto.typeofatendimento);
            return service;
        }
        public void post()
        {
            if (_status == "Rascunho")
            {
                _status = "Postado";
                return;
            }
            throw new Exception("Serviço ja postado");
        }

        private List<ServiceSubCategoryDb> vallidsubcategory(ServiceDTO service, List<ServiceSubCategoryDb> subcategorysofservice)
        {
            return service.codecategory == "C04"
                                        && service.subcategories.Count() != 0
                                        ? throw new Exception("Esse serviço nao permite subcategorias")
                                        : subcategorysofservice;
        }
        private List<ServiceSubCategoryDb> buildServiceSubcategory(List<ServiceSubCategory> subcategories, Price price)
        {
            List<ServiceSubCategoryDb> subcategorysofservice = new List<ServiceSubCategoryDb>();
            foreach (ServiceSubCategory subcategory in subcategories)
            {
                ServiceSubCategoryDb subcategoryofservice = new ServiceSubCategoryDb();
                subcategoryofservice.idsubcategory = Guid.NewGuid().ToString("N");
                subcategoryofservice.title = subcategory.namesubcategory;
                subcategoryofservice.price = price.getValue(subcategory.price);
                subcategorysofservice.Add(subcategoryofservice);
            }
            return subcategorysofservice;
        }
        private List<VaccineDbDTO> vallidvaccine(ServiceDTO service, List<VaccineDbDTO> vacciensofservice)
        {
            return service.codecategory != "C04"
                                        && service.vaccines.Count() != 0 ? throw new Exception("this service does not allow vaccines")
                                        : service.codecategory == "C04" && service.vaccines.Count() == 0 ? throw new Exception("this service require vaccines")
                                        : vacciensofservice;
        }
        private List<VaccineDbDTO> buildVaccines(List<VaccineDTO> vaccines, Price price, VaccineService vaccineservice,string idservice)
        {
            List<VaccineDbDTO> vaccinesofservice = new List<VaccineDbDTO>();
            foreach (VaccineDTO vaccine in vaccines)
            {
                VaccineDbDTO vaccineofservice = new VaccineDbDTO();
                vaccineofservice.idvaccine = Guid.NewGuid().ToString("N");
                vaccineofservice.nameofvaccine = vaccineservice.GetVaccine(vaccine.codeofvaccine);
                vaccineofservice.price = price.getValue(vaccine.price);
                vaccineofservice.idservice = idservice;
                vaccineofservice.codevaccine = vaccine.codeofvaccine;
                vaccinesofservice.Add(vaccineofservice);
            }
            return vaccinesofservice;
        }
        private float? validPriceOfService(ServiceDTO service)
        {
            return (service.vaccines.Count() != 0 || service.subcategories.Count() != 0)
                                                  && service.price != null
                                                  ? throw new Exception("Coloque preço somente nas vacinas ou nos subserviços")
                                                  : service.price == null ? 0
                                                  : service.price;
        }
        private List<string> setAttendanceModel(List<string> atendimentos)
        {
            foreach(string atendimento in atendimentos)
            {
                if (!_attendancemodelspermission.Contains(atendimento))
                    throw new Exception($"Modelo de atendimento {atendimento} invalido");
            }
            return atendimentos;
        }
        private bool validAttendanceModel(string atendimento)
        {
            bool valid = true;
                if (!_attendancemodelspermission.Contains(atendimento))
                    valid = false;
            return valid;
        }
        private List<string> converteToAttendance(List<AttendanceModel> attendancemodels)
        {
            List<string> result = new List<string>();
            foreach(AttendanceModel att in attendancemodels)
            {
                result.Add(att.model);
            }
            return result;
        }
        private void setNewSubcategories(List<ServiceSubCategoryDb> subcategoriesnew)
        {
            foreach(ServiceSubCategoryDb subcategory in subcategories)
            {
                List<ServiceSubCategoryDb> subCategoryDb = subcategoriesnew.FindAll(x => x.idsubcategory == subcategory.idsubcategory);
                if (subCategoryDb.IsNullOrEmpty())
                {
                    subcategories.Remove(subcategory);
                }
                else
                {
                    subcategory.title = subCategoryDb[0].title;
                    subcategory.price = subCategoryDb[0].price;
                }
            }
            foreach(ServiceSubCategoryDb subcategory in subcategoriesnew)
            {
                if(subcategory.idsubcategory == null || subcategory.idsubcategory == "")
                    subcategory.idsubcategory = Guid.NewGuid().ToString("N");
                    subcategories.Add(subcategory);

            }
        }
        public List<ServiceSubCategoryDb> subcategoriesOfOs(List<string> ids)
        {
            return subcategories.Where(x=>ids.Contains(x.idsubcategory)).ToList();
        }
        public List<VaccineDbDTO> selectvaccines(List<string> ids)
        {
            return vaccines.Where(x => ids.Contains(x.idvaccine)).ToList();
        }
        public void update(ServiceUpdateDTO servicedto)
        {
            _codeCategory = servicedto.codecategory;
            _nameCategory = categoryService.GetCategory(servicedto.codecategory);
            titleService = servicedto.title;
            description = servicedto.description;
            price = Price.getValue(servicedto.price);
        }
        public static Service restore(ServiceDb servicedto, List<ServiceSubCategoryDb> subcategories, List<VaccineDbDTO> vaccines,List<AttendanceModel> attendences)
        {
            Service service = new Service();
            service.idService = servicedto.idservice;
            service.idUser = servicedto.iduser;
            service.titleService = servicedto.title;
            service.price = servicedto.price;
            service.description = servicedto.description;
            service._codeCategory = servicedto.codecategory;
            service._nameCategory = servicedto.namecategory;
            service._vaccines = vaccines;
            service.subcategories = subcategories;
            service._status = servicedto.status;
            service._attendancemodels = service.converteToAttendance(attendences);
            return service;
        }
    }
}
