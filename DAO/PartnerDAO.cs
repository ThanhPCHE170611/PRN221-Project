using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class PartnerDAO
    {
        private static PartnerDAO instance = null;

        public PartnerDAO() { }

        public static PartnerDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PartnerDAO();
                }
                return instance;
            }
        }

        public List<Partner> GetPartners()
        {
            List<Partner> partner;
            try
            {
                var context = new PRN221_ProjectContext();
                partner = context.Partners
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return partner;
        }

        public Partner GetPartnerByID(int id)
        {
            Partner partner = null;
            try
            {
                var db = new PRN221_ProjectContext();
                partner = db.Partners.SingleOrDefault(u => u.PartnerId == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return partner;
        }

        public void AddPartner(Partner partner)
        {
            try
            {
                bool existingPartner = GetPartners()
                    .Any(a => a.PartnerCode.ToLower().Equals(partner.PartnerCode.ToLower()));

                if (existingPartner == false)
                {
                    partner.Status = 1;

                    using (var db = new PRN221_ProjectContext())
                    {
                        db.Partners.Add(partner);
                        db.SaveChanges();
                    }
                }
                else
                {
                    throw new Exception("Partner already exists!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in Add Partner: {ex.Message}", ex);
            }
        }

        public bool UpdatePartner(Partner partner)
        {
            try
            {
                using (var db = new PRN221_ProjectContext())
                {

                    bool existingPartner = GetPartners()
                        .Where(p => p.PartnerId != partner.PartnerId)
                        .Any(p => p.PartnerCode.ToLower().Equals(partner.PartnerCode.ToLower()));

                    if (!existingPartner)
                    {
                        db.Partners.Update(partner);
                        db.SaveChanges();
                        return true;
                    }
                    else
                    {
                        throw new Exception("Partner code already exists in another partner!");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool BanPartner(int id)
        {
            try
            {
                using (var db = new PRN221_ProjectContext())
                {
                    Partner partner = db.Partners.SingleOrDefault(p => p.PartnerId == id);

                    if (partner != null)
                    {
                        partner.Status = (partner.Status == 0) ? 1 : 0;

                        db.Entry(partner).State = EntityState.Modified;
                        db.SaveChanges();
                        Console.WriteLine("Partner status updated successfully!");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Partner does not exist!");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in BanPartner: {ex.Message}");
                return false;
            }
        }
    }
}
