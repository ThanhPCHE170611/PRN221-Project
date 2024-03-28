using BusinessObject.Models;
using DAO;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class PartnerService : IPartnerService
    {
        private IPartnerRepository _partnerRepository;

        public PartnerService()
        {
            _partnerRepository = new PartnerRepository();
        }

        void IPartnerService.AddPartner(Partner partner) => _partnerRepository.AddPartner(partner);

        bool IPartnerService.BanPartner(int id) => _partnerRepository.BanPartner(id);

        Partner IPartnerService.GetPartnerByID(int id) => _partnerRepository.GetPartnerByID(id);

        List<Partner> IPartnerService.GetPartners() => _partnerRepository.GetPartners();

        bool IPartnerService.UpdatePartner(Partner partner) => _partnerRepository.UpdatePartner(partner);
    }
}
