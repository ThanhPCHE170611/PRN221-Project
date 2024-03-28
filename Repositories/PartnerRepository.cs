using BusinessObject.Models;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class PartnerRepository : IPartnerRepository
    {
        private PartnerDAO partnerDAO = null;
        public PartnerRepository()
        {
            partnerDAO = new PartnerDAO();
        }

        public void AddPartner(Partner partner) => partnerDAO.AddPartner(partner);

        public bool BanPartner(int id) => partnerDAO.BanPartner(id);

        public Partner GetPartnerByID(int id) => partnerDAO.GetPartnerByID(id);

        public List<Partner> GetPartners() => partnerDAO.GetPartners();

        public bool UpdatePartner(Partner partner) => partnerDAO.UpdatePartner(partner);
    }
}
