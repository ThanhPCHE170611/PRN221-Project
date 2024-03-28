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
    public class StockOutService : IStockOutService
    {
        private IStockOutRepo stockOutRepo = null;

        public StockOutService()
        {
            stockOutRepo = new StockOutRepo();
        }
        public bool AddStockOut(StockOut stockOut) => stockOutRepo.AddStockOut(stockOut);
        public bool AddOneStockOutDetail(StockOutDetail detail) => stockOutRepo.AddOneStockOutDetail(detail);

        public StockOut GetStockOutById(int id) => stockOutRepo.GetStockOutById(id);

        public List<StockOut> GetStockOuts() => stockOutRepo.GetStockOuts();

        public List<StockOutDetail> GetStockOutsDetail() => stockOutRepo.GetStockOutsDetail();

        public List<StockOutDetail> GetStockOutDetailById(int id) => stockOutRepo.GetStockOutDetailById(id);

        public void UpdateStockOuts(StockOut stockOut) => stockOutRepo.UpdateStockOuts(stockOut);

        public void UpdateStockOutsDetail(int stockOutDetailsId, int Quantity) => stockOutRepo.UpdateStockOutsDetail(stockOutDetailsId, Quantity);

        public bool AddStockOutDetail(int stockOutId, List<StockOutDetail> stockOutDetails) => stockOutRepo.AddStockOutDetail(stockOutId, stockOutDetails);
        public void DeleteStockOutPermanently(StockOut stockOut) => stockOutRepo.DeleteStockOutPermanently(stockOut);

        public void DeleteStockOutDetailsPermanently(StockOutDetail stockOutDetail) => stockOutRepo.DeleteStockOutDetailsPermanently(stockOutDetail);
    }
}
