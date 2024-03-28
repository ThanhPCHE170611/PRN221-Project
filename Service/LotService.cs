using BusinessObject.Models;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class LotService : ILotService
    {
        private readonly ILotRepo _lotRepo;

        public LotService()
        {
            _lotRepo = new LotRepo();
        }

        public void AddLot(Lot lot)
        {
            _lotRepo.AddLot(lot);
        }

        public void AddLotDetail(LotDetail detail)
        {
            _lotRepo.AddLotDetail(detail);
        }

        public void DeleteLotDetailPermanently(LotDetail detail)
        {
            _lotRepo.DeleteLotDetailPermanently(detail);
        }

        public void DeleteLotDetailStatus(LotDetail detail)
        {
            _lotRepo.DeleteLotDetailPermanently(detail);
        }

        public void DeleteLotPermanently(Lot lot)
        {
            _lotRepo.DeleteLotPermanently(lot);
        }

        public void DeleteLotStatus(Lot lot)
        {
            _lotRepo.DeleteLotStatus(lot);
        }

        public IEnumerable<LotDetail> GetAllLotDetail()
        {
            return _lotRepo.GetAllLotDetail();
        }

        public IEnumerable<Lot> GetAllLots()
        {
            return _lotRepo.GetAllLots();
        }

        public IEnumerable<Lot> GetListLotByAccountID(int acID)
        {
            return _lotRepo.GetListLotByAccountID(acID);
        }

        public IEnumerable<Lot> GetListLotByPartnerID(int acID)
        {
            return _lotRepo.GetListLotByPartnerID(acID);
        }

        public IEnumerable<LotDetail> GetListLotDetailByLotID(int lotID)
        {
            return _lotRepo.GetListLotDetailByLotID(lotID);
        }

        public IEnumerable<LotDetail> GetListLotDetailByPartnerID(int parID)
        {
            return _lotRepo.GetListLotDetailByPartnerID(parID);
        }

        public IEnumerable<LotDetail> GetListLotDetailByProductID(int pID)
        {
            return _lotRepo.GetListLotDetailByProductID(pID);
        }

        public Lot GetLotByAccountId(int id)
        {
            return _lotRepo.GetLotByAccountId(id);
        }

        public Lot GetLotById(int id)
        {
            return _lotRepo.GetLotById(id);
        }

        public Lot GetLotByLotCode(string code)
        {
            return _lotRepo.GetLotByLotCode(code);
        }

        public Lot GetLotByPartnerId(int id)
        {
            return _lotRepo.GetLotByPartnerId(id);
        }

        public LotDetail GetLotDetailById(int id)
        {
            return _lotRepo.GetLotDetailById(id);
        }

        public LotDetail GetLotDetailByLotId(int id)
        {
            return _lotRepo.GetLotDetailByLotId(id);
        }

        public LotDetail GetLotDetailByPartnerId(int id)
        {
            return _lotRepo.GetLotDetailByPartnerId(id);
        }

        public LotDetail GetLotDetailByProductId(int pid)
        {
            return _lotRepo.GetLotDetailByProductId(pid);
        }

        public void UpdateLot(Lot lot)
        {
            _lotRepo.UpdateLot(lot);
        }

        public void UpdateLotDetail(LotDetail detail)
        {
            _lotRepo.UpdateLotDetail(detail);
        }

        public List<LotDetail> GetListLotDetailById(int id)
        {
            return _lotRepo.GetListLotDetailById(id);
        }

    }
}
