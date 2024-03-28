using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IPartnerRepository
    {
        List<Partner> GetPartners();
        Partner GetPartnerByID(int id);
        void AddPartner(Partner partner);
        bool UpdatePartner(Partner partner);
        bool BanPartner(int id);
    }
}
