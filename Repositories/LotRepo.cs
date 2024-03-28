using BusinessObject.Models;
using DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class LotRepo : ILotRepo
    {
        public void AddLot(Lot lot) => LotDAO.Instance.AddLot(lot);

        public void AddLotDetail(LotDetail detail) => LotDAO.Instance.AddLotDetail(detail);

        public void DeleteLotDetailPermanently(LotDetail detail) => LotDAO.Instance.DeleteLotDetailPermanently(detail);

        public void DeleteLotDetailStatus(LotDetail detail) => LotDAO.Instance.DeleteLotDetailStatus(detail);

        public void DeleteLotPermanently(Lot lot) => LotDAO.Instance.DeleteLotPermanently(lot);

        public void DeleteLotStatus(Lot lot) => LotDAO.Instance.DeleteLotStatus(lot);

        public IEnumerable<LotDetail> GetAllLotDetail() => LotDAO.Instance.GetAllLotDetail();

        public IEnumerable<Lot> GetAllLots() => LotDAO.Instance.GetAllLots();

        public IEnumerable<Lot> GetListLotByAccountID(int acID) => LotDAO.Instance.GetListLotByAccountID(acID);

        public IEnumerable<Lot> GetListLotByPartnerID(int acID) => LotDAO.Instance.GetListLotByPartnerID(acID);

        public IEnumerable<LotDetail> GetListLotDetailByLotID(int lotID) => LotDAO.Instance.GetListLotDetailByLotID(lotID);

        public IEnumerable<LotDetail> GetListLotDetailByPartnerID(int parID) => LotDAO.Instance.GetListLotDetailByPartnerID(parID);

        public IEnumerable<LotDetail> GetListLotDetailByProductID(int pID) => LotDAO.Instance.GetListLotDetailByProductID(pID);

        public Lot GetLotByAccountId(int id) => LotDAO.Instance.GetLotByAccountId(id);

        public Lot GetLotById(int id) => LotDAO.Instance.GetLotById(id);

        public Lot GetLotByLotCode(string code) => LotDAO.Instance.GetLotByLotCode(code);

        public Lot GetLotByPartnerId(int id) => LotDAO.Instance.GetLotByPartnerId(id);

        public LotDetail GetLotDetailById(int id) => LotDAO.Instance.GetLotDetailById(id);

        public LotDetail GetLotDetailByLotId(int id) => LotDAO.Instance.GetLotDetailByLotId(id);

        public LotDetail GetLotDetailByPartnerId(int id) => LotDAO.Instance.GetLotDetailByPartnerId(id);

        public LotDetail GetLotDetailByProductId(int pid) => LotDAO.Instance.GetLotDetailByProductId(pid);

        public void UpdateLot(Lot lot) => LotDAO.Instance.UpdateLot(lot);

        public void UpdateLotDetail(LotDetail detail) => LotDAO.Instance.UpdateLotDetail(detail);

        public List<LotDetail> GetListLotDetailById(int id) => LotDAO.Instance.GetListLotDetailById(id);
    }
}
