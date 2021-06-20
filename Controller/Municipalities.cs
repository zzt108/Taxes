using System.Collections.Generic;
using System.Linq;
using DataAccessLayer;
using Model;

namespace Controller
{
    public static class Municipalities
    {

        public static Municipality GetByName(string name)
        {

            using (var uw = new UnitOfWork())
            {
                return uw.MunicipalityRepository.Get(municipality => municipality.Name.ToLower() == name.ToLower()).FirstOrDefault();
            }
        }

        public static void Add(Municipality municipality)
        {
            using (var uw = new UnitOfWork())
            {
                if (null == GetByName(municipality.Name))
                {
                    uw.MunicipalityRepository.Add(municipality);
                    uw.SaveChanges();
                }
            }
        }
        public static IList<Municipality> GetAll()
        {
            using (var uw = new UnitOfWork())
            {
                return uw.MunicipalityRepository.Get(municipality => true).ToList();
            }

        }

    }
}