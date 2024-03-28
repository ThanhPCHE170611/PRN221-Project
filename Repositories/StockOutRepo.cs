using BusinessObject.Models;
using DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class StockOutRepo : IStockOutRepo
    {
        private StockOutDAO stockOutDAO = null;

        public StockOutRepo()
        {
            stockOutDAO = new StockOutDAO();
        }

        public bool AddStockOut(StockOut stockOut) => stockOutDAO.AddStockOut(stockOut);
        public bool AddOneStockOutDetail(StockOutDetail detail) => stockOutDAO.AddOneStockOutDetail(detail);

        public StockOut GetStockOutById(int id) => stockOutDAO.GetStockOutById(id);

        public List<StockOut> GetStockOuts() => stockOutDAO.GetStockOuts();

        public List<StockOutDetail> GetStockOutsDetail() => stockOutDAO.GetStockOutsDetail();

        public List<StockOutDetail> GetStockOutDetailById(int id) => stockOutDAO.GetStockOutDetailById(id);

        public void UpdateStockOuts(StockOut stockOut) => stockOutDAO.UpdateStockOuts(stockOut);
        public void UpdateStockOutsDetail(int stockOutDetailsId, int Quantity) => stockOutDAO.UpdateStockOutsDetail(stockOutDetailsId, Quantity);

        public bool AddStockOutDetail(int stockOutId, List<StockOutDetail> stockOutDetails) => stockOutDAO.AddStockOutDetail(stockOutId, stockOutDetails);

        public void DeleteStockOutPermanently(StockOut stockOut) => stockOutDAO.DeleteStockOutPermanently(stockOut);

        public void DeleteStockOutDetailsPermanently(StockOutDetail stockOutDetail) => stockOutDAO.DeleteStockOutDetailsPermanently(stockOutDetail);
    }
}
