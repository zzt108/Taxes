using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using DataAccessLayer;
using Model;

namespace Controller
{
    public static class Municipalities
    {

        public static int GetMunicipalityId(string name)
        {

            using (var uw = new UnitOfWork())
            {
                var first = uw.MunicipalityRepository.Get(municipality => municipality.Name.ToLower() == name.ToLower()).FirstOrDefault();
                return first?.Id ?? 0;        // zero id is not possible. better performance than throwing an exception
            }
        }

        public static void AddMunicipality(Municipality municipality)
        {
            using (var uw = new UnitOfWork())
            {
                if (0 == GetMunicipalityId(municipality.Name))
                {
                    uw.MunicipalityRepository.Add(municipality);
                    uw.SaveChanges();
                }
            }
        }
    }
}