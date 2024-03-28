using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface ILotRepo
    {
        //Lot
        IEnumerable<Lot> GetAllLots();
        IEnumerable<Lot> GetListLotByAccountID(int acID);
        IEnumerable<Lot> GetListLotByPartnerID(int acID);
        Lot GetLotById(int id);
        Lot GetLotByAccountId(int id);
        Lot GetLotByLotCode(string code);
        Lot GetLotByPartnerId(int id);
        void AddLot(Lot lot);
        void UpdateLot(Lot lot);
        void DeleteLotPermanently(Lot lot);
        void DeleteLotStatus(Lot lot);

        //Detail
        IEnumerable<LotDetail> GetAllLotDetail();
        IEnumerable<LotDetail> GetListLotDetailByProductID(int pID);
        IEnumerable<LotDetail> GetListLotDetailByPartnerID(int parID);
        IEnumerable<LotDetail> GetListLotDetailByLotID(int lotID);
        LotDetail GetLotDetailById(int id);
        LotDetail GetLotDetailByProductId(int pid);
        LotDetail GetLotDetailByPartnerId(int id);
        LotDetail GetLotDetailByLotId(int id);
        void AddLotDetail(LotDetail detail);
        void UpdateLotDetail(LotDetail detail);
        void DeleteLotDetailPermanently(LotDetail detail);
        void DeleteLotDetailStatus(LotDetail detail);
        List<LotDetail> GetListLotDetailById(int id);
    }
}
