using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class LotDAO
    {
        private static LotDAO instance = null;
        private static readonly object instanceLock = new object();

        private LotDAO() { }
        public static LotDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new LotDAO();
                    }
                    return instance;
                }
            }
        }
        // lot
        public IEnumerable<Lot> GetAllLots()
        {
            var _dbContext = new PRN221_ProjectContext();
            return _dbContext.Lots
                .Include(c => c.Account)
                .Include(c => c.Partner)
                .Include(c => c.LotDetails)
                .ToList();
        }
        public IEnumerable<Lot> GetListLotByAccountID(int acID)
        {
            var _dbContext = new PRN221_ProjectContext();
            return _dbContext.Lots
                .Where(c => c.AccountId == acID)
                .Include(c => c.Account)
                .Include(c => c.Partner)
                .Include(c => c.LotDetails)
                .ToList();
        }
        public IEnumerable<Lot> GetListLotByPartnerID(int acID)
        {
            var _dbContext = new PRN221_ProjectContext();
            return _dbContext.Lots
                .Where(c => c.PartnerId == acID)
                .Include(c => c.Account)
                .Include(c => c.Partner)
                .Include(c => c.LotDetails)
                .ToList();
        }
        public Lot GetLotById(int id)
        {
            var _dbContext = new PRN221_ProjectContext();
            return _dbContext.Lots
                .Include(c => c.Account)
                .Include(c => c.Partner)
                .Include(c => c.LotDetails)
                .SingleOrDefault(c => c.LotId == id);
        }
        public Lot GetLotByAccountId(int id)
        {
            var _dbContext = new PRN221_ProjectContext();
            return _dbContext.Lots
                .Include(c => c.Account)
                .Include(c => c.Partner)
                .Include(c => c.Partner)
                .SingleOrDefault(c => c.AccountId == id);
        }
        public Lot GetLotByPartnerId(int id)
        {
            var _dbContext = new PRN221_ProjectContext();
            return _dbContext.Lots
                .Include(c => c.Account)
                .Include(c => c.Partner)
                .Include(c => c.Partner)
                .SingleOrDefault(c => c.PartnerId == id);
        }
        public Lot GetLotByLotCode(string code)
        {
            var _dbContext = new PRN221_ProjectContext();
            return _dbContext.Lots
                .Include(c => c.Account)
                .Include(c => c.Partner)
                .Include(c => c.Partner)
                .SingleOrDefault(c => c.LotCode == code);
        }
        public void AddLot(Lot lot)
        {
            var _dbContext = new PRN221_ProjectContext();
            var eLot = GetLotById(lot.LotId);
            if (eLot == null)
            {
                lot.DateIn = DateTime.Now;
                _dbContext.Lots.Add(lot);
                _dbContext.SaveChanges();
            }
            else
            {
                throw new Exception("Lot is already existed. (LotID duplicated)");
            }
        }
        public void UpdateLot(Lot lot)
        {
            var _dbContext = new PRN221_ProjectContext();
            var eLot = GetLotById(lot.LotId);
            if (eLot != null)
            {
                //Export Json Lot
                var jsonFormatContent = JsonConvert.SerializeObject(eLot, Formatting.None, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                string fileName = @"D:\Lot.txt";
                using (StreamWriter streamWriter = new StreamWriter(fileName, true))
                {
                    streamWriter.WriteLine(DateTime.Now);
                    streamWriter.WriteLine(jsonFormatContent);
                }

                lot.LotCode = eLot.LotCode;
                lot.DateIn = DateTime.Now;
                lot.Status = 1;
                _dbContext.Lots.Update(lot);
                _dbContext.SaveChanges();
                //Export Json Lot
                var jsonFormatContent2 = JsonConvert.SerializeObject(lot, Formatting.None, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                string fileName2 = @"D:\LotUpdate.txt";
                using (StreamWriter streamWriter = new StreamWriter(fileName2, true))
                {
                    streamWriter.WriteLine(DateTime.Now);
                    streamWriter.WriteLine(jsonFormatContent2);
                }
            }
            else
            {
                throw new Exception("Lot is not existed. (LotID does not exist)");
            }
        }
        public void DeleteLotPermanently(Lot lot)
        {
            var _dbContext = new PRN221_ProjectContext();
            var eLot = GetLotById(lot.LotId);
            if (eLot != null)
            {
                _dbContext.Lots.Remove(lot);
                _dbContext.SaveChanges();
            }
            else
            {
                throw new Exception("Lot is not exist.");
            }
        }
        public void DeleteLotStatus(Lot lot)
        {
            var _dbContext = new PRN221_ProjectContext();
            var eLot = GetLotById(lot.LotId);
            if (eLot != null)
            {
                LotDetail lotDetail = new LotDetail();
                lotDetail.PartnerId = lot.PartnerId;
                UpdateLotDetail(lotDetail);

                lot.Status = 0;
                _dbContext.Lots.Update(lot);
                _dbContext.SaveChanges();
            }
            else
            {
                throw new Exception("Lot is not exist.");
            }
        }

        //Lot detail 
        public IEnumerable<LotDetail> GetAllLotDetail()
        {
            var _dbContext = new PRN221_ProjectContext();
            return _dbContext.LotDetails
                .Include(c => c.Product)
                .Include(c => c.Partner)
                .Include(c => c.Lot)
                .ToList();
        }
        public IEnumerable<LotDetail> GetListLotDetailByProductID(int pID)
        {
            var _dbContext = new PRN221_ProjectContext();
            return _dbContext.LotDetails
                .Where(c => c.ProductId == pID)
                .Include(c => c.Product)
                .Include(c => c.Partner)
                .Include(c => c.Lot)
                .ToList();
        }
        public IEnumerable<LotDetail> GetListLotDetailByPartnerID(int parID)
        {
            var _dbContext = new PRN221_ProjectContext();
            return _dbContext.LotDetails
                .Where(c => c.PartnerId == parID)
                .Include(c => c.Product)
                .Include(c => c.Partner)
                .Include(c => c.Lot)
                .ToList();
        }
        public IEnumerable<LotDetail> GetListLotDetailByLotID(int lotID)
        {
            var _dbContext = new PRN221_ProjectContext();
            return _dbContext.LotDetails
                .Where(c => c.LotId == lotID)
                .Include(c => c.Product)
                .Include(c => c.Partner)
                .Include(c => c.Lot)
                .ToList();
        }
        public LotDetail GetLotDetailById(int id)
        {
            var _dbContext = new PRN221_ProjectContext();
            return _dbContext.LotDetails
                .Include(c => c.Product)
                .Include(c => c.Partner)
                .Include(c => c.Lot)
                .AsNoTracking()
                .SingleOrDefault(c => c.LotDetailId == id);
        }
        public LotDetail GetLotDetailByProductId(int pid)
        {
            var _dbContext = new PRN221_ProjectContext();
            return _dbContext.LotDetails
                .Include(c => c.Product)
                .Include(c => c.Partner)
                .Include(c => c.Lot)
                .SingleOrDefault(c => c.ProductId == pid);
        }
        public LotDetail GetLotDetailByPartnerId(int id)
        {
            var _dbContext = new PRN221_ProjectContext();
            return _dbContext.LotDetails
                .Include(c => c.Product)
                .Include(c => c.Partner)
                .Include(c => c.Lot)
                .SingleOrDefault(c => c.PartnerId == id);
        }
        public LotDetail GetLotDetailByLotId(int id)
        {
            var _dbContext = new PRN221_ProjectContext();
            return _dbContext.LotDetails
                .Include(c => c.Product)
                .Include(c => c.Partner)
                .Include(c => c.Lot)
                .SingleOrDefault(c => c.LotId == id);
        }
        public void AddLotDetail(LotDetail detail)
        {
            var _dbContext = new PRN221_ProjectContext();
            var edetailLot = GetLotDetailById(detail.LotDetailId);
            if (edetailLot == null)
            {
                _dbContext.LotDetails.Add(detail);
                _dbContext.SaveChanges();
            }
            else
            {
                throw new Exception("LotDetail is already existed. (LotDetailID duplicated)");
            }
        }
        public void UpdateLotDetail(LotDetail detail)
        {
            var _dbContext = new PRN221_ProjectContext();
            var edetailLot = GetLotDetailById(detail.LotDetailId);
            if (edetailLot != null)
            {
                // Export Json StockOutDetail
                var jsonFormatContent = JsonConvert.SerializeObject(edetailLot, Formatting.None, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                string fileName = @"D:\LotDetail.txt";
                using (StreamWriter streamWriter = new StreamWriter(fileName, true))
                {
                    streamWriter.WriteLine(DateTime.Now);
                    streamWriter.WriteLine(jsonFormatContent);
                }
                edetailLot.Quantity = detail.Quantity;
                edetailLot.PartnerId = detail.PartnerId;
                _dbContext.Entry(edetailLot).State = EntityState.Modified;
                _dbContext.SaveChanges();
                // Export Json StockOutDetail Update
                var jsonFormatContent2 = JsonConvert.SerializeObject(edetailLot, Formatting.None, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                string fileName2 = @"D:\LotDetailUpdate.txt";
                using (StreamWriter streamWriter = new StreamWriter(fileName2, true))
                {
                    streamWriter.WriteLine(DateTime.Now);
                    streamWriter.WriteLine(jsonFormatContent2);
                }
            }
            else
            {
                throw new Exception("LotDetail is not existed. (LotDetailID does not exist)");
            }
        }
        public void DeleteLotDetailPermanently(LotDetail detail)
        {
            var _dbContext = new PRN221_ProjectContext();
            var edetailLot = GetLotDetailById(detail.LotDetailId);
            if (edetailLot != null)
            {
                _dbContext.LotDetails.Remove(detail);
                _dbContext.SaveChanges();
            }
            else
            {
                throw new Exception("LotDeail is not exist.");
            }
        }
        public void DeleteLotDetailStatus(LotDetail detail)
        {
            var _dbContext = new PRN221_ProjectContext();
            var edetailLot = GetLotDetailById(detail.LotDetailId);
            if (edetailLot != null)
            {
                detail.Status = 0;
                _dbContext.LotDetails.Update(detail);
                _dbContext.SaveChanges();
            }
            else
            {
                throw new Exception("LotDeail is not exist.");
            }
        }
        public List<LotDetail> GetListLotDetailById(int id)
        {
            var _dbContext = new PRN221_ProjectContext();
            List<LotDetail> lotDetails = _dbContext.LotDetails
                .Include(c => c.Product)
                .Include(c => c.Partner)
                .Include(c => c.Lot)
                .Where(c => c.LotId == id).ToList();
            return lotDetails;
        }
    }
}
