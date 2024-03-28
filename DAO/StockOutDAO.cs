using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class StockOutDAO
    {
        private static StockOutDAO instance = null;
        private static PRN221_ProjectContext dbContext = new PRN221_ProjectContext();
        public StockOutDAO() { }
        public static StockOutDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new StockOutDAO();
                }
                return instance;
            }
        }

        public List<StockOut> GetStockOuts()
        {
            List<StockOut> stockOuts = null;
            try
            {
                using (var db = new PRN221_ProjectContext())
                {
                    stockOuts = db.StockOuts
                    .Include(x => x.StockOutDetails)
                    .Include(x => x.Account)
                    .Include(x => x.Partner)
                    .OrderByDescending(x => x.Status)
                    .ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return stockOuts;
        }
        public List<StockOutDetail> GetStockOutsDetail()
        {
            List<StockOutDetail> stockOutsDetail = null;
            try
            {
                using (var dbContext = new PRN221_ProjectContext())
                {
                    stockOutsDetail = dbContext.StockOutDetails
                        .Include(x => x.Product)
                        .Include(x => x.StockOut)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return stockOutsDetail;
        }

        public StockOut GetStockOutById(int id)
        {
            StockOut stockOut = null;

            try
            {
                stockOut = dbContext.StockOuts
                    .Include(x => x.StockOutDetails)
                    .Include(x => x.Account)
                    .Include(x => x.Partner)
                    .SingleOrDefault(x => x.StockOutId == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return stockOut;
        }

        public bool AddStockOut(StockOut stockOut)
        {
            try
            {
                using (var dbContext = new PRN221_ProjectContext())
                {
                    stockOut.DateOut = DateTime.Now;
                    stockOut.Status = 1;
                    dbContext.StockOuts.Add(stockOut);
                    dbContext.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }
        public bool AddOneStockOutDetail(StockOutDetail stockoutDetail)
        {
            try
            {
                using (var dbContext = new PRN221_ProjectContext())
                {
                    dbContext.StockOutDetails.Add(stockoutDetail);
                    dbContext.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }
        public bool AddStockOutDetail(int stockOutId, List<StockOutDetail> stockOutDetails)
        {
            using var transaction = dbContext.Database.BeginTransaction();

            try
            {
                using (var dbContext = new PRN221_ProjectContext())
                {
                    var stockOut = dbContext.StockOuts.Find(stockOutId);

                    if (stockOut == null)
                    {
                        transaction.Rollback();
                        throw new Exception($"StockOut with ID {stockOutId} not found.");
                    }

                    foreach (var detail in stockOutDetails)
                    {
                        var product = dbContext.Products.Find(detail.ProductId);

                        if (product == null || product.Quantity < detail.Quantity)
                        {
                            transaction.Rollback();
                            throw new Exception("Invalid product or insufficient quantity.");
                        }

                        product.Quantity -= detail.Quantity;

                        detail.StockOutId = stockOutId;
                        dbContext.StockOutDetails.Add(detail);
                    }

                    dbContext.SaveChanges();

                    transaction.Commit();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                transaction.Rollback();
                return false;
            }
        }

        public List<StockOutDetail> GetStockOutDetailById(int id)
        {
            List<StockOutDetail> stockOutDetail = null;

            try
            {
                stockOutDetail = dbContext.StockOutDetails
                    .Include(x => x.StockOut)
                    .Include(x => x.Product)
                    .Where(x => x.StockOutId == id).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return stockOutDetail;
        }

        public void UpdateStockOuts(StockOut NewstockOut)
        {
            var _dbContext = new PRN221_ProjectContext();
            try
            {
                StockOut OldStockOut = GetStockOutById(NewstockOut.StockOutId);

                // Export Json StockOut 
                var jsonFormatContent = JsonConvert.SerializeObject(OldStockOut, Formatting.None, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                string fileName = @"D:\StockOuts.txt";
                if (System.IO.File.Exists(fileName) == false)
                {
                    System.IO.File.WriteAllText(fileName, DateTime.Now + "\n" + jsonFormatContent + "\n");
                }
                else
                {
                    System.IO.File.AppendAllText(fileName, jsonFormatContent);
                }

                NewstockOut.DateOut = DateTime.Now;
                NewstockOut.Status = 1;
                _dbContext.StockOuts.Update(NewstockOut);
                _dbContext.SaveChanges();

                // Export Json StockOut Update
                var jsonFormatContent2 = JsonConvert.SerializeObject(NewstockOut, Formatting.None, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                string fileName2 = @"D:\StockOutsUpdate.txt";

                if (System.IO.File.Exists(fileName2) == false)
                {
                    System.IO.File.WriteAllText(fileName2, DateTime.Now + "\n" + jsonFormatContent2 + "\n");
                }
                else
                {
                    System.IO.File.AppendAllText(fileName2, DateTime.Now + "\n" + jsonFormatContent2 + "\n");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateStockOutsDetail(int stockOutDetailsId, int Quantity)
        {
                var _dbContext = new PRN221_ProjectContext();
                try
                {
                    StockOutDetail stockOutDetailsList = _dbContext.StockOutDetails
                        .FirstOrDefault(s => s.StockOutDetailId == stockOutDetailsId);

                    if (stockOutDetailsList != null)
                    {
                        // Export Json StockOutDetail
                        var jsonFormatContent = JsonConvert.SerializeObject(stockOutDetailsList, Formatting.None, new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
                        string fileName = @"D:\StockOutsDetail.txt";
                        using (StreamWriter streamWriter = new StreamWriter(fileName, true))
                        {
                            streamWriter.WriteLine(DateTime.Now);
                            streamWriter.WriteLine(jsonFormatContent);
                        }

                        stockOutDetailsList.Quantity = Quantity;
                        _dbContext.Entry(stockOutDetailsList).State = EntityState.Modified;
                        _dbContext.SaveChanges();

                        // Export Json StockOutDetail After Update
                        var jsonFormatContent2 = JsonConvert.SerializeObject(stockOutDetailsList, Formatting.None, new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
                        string fileName2 = @"D:\StockOutsDetailUpdate.txt";
                        using (StreamWriter streamWriter2 = new StreamWriter(fileName2, true))
                        {
                            streamWriter2.WriteLine(DateTime.Now);
                            streamWriter2.WriteLine(jsonFormatContent2);
                        }
                    }
                }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public StockOutDetail GetStockOutsDetailById(int id)
        {
            StockOutDetail stockOutDetail = null;

            try
            {
                stockOutDetail = dbContext.StockOutDetails
                    .Include(x => x.StockOut)
                    .Include(x => x.Product)
                    .SingleOrDefault(x => x.StockOutId == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return stockOutDetail;
        }
        public void DeleteStockOutPermanently(StockOut stock)
        {
            var _dbContext = new PRN221_ProjectContext();
            var eStock = GetStockOutById(stock.StockOutId);
            if (eStock != null)
            {
                _dbContext.StockOuts.Remove(stock);
                _dbContext.SaveChanges();
            }
            else
            {
                throw new Exception("stockout is not exist.");
            }
        }
        public void DeleteStockOutDetailsPermanently(StockOutDetail detail)
        {
            var _dbContext = new PRN221_ProjectContext();
            var eStockDetail = GetStockOutsDetailById(detail.StockOutId);
            if (eStockDetail != null)
            {
                _dbContext.StockOutDetails.Remove(detail);
                _dbContext.SaveChanges();
            }
            else
            {
                throw new Exception("stockout is not exist.");
            }
        }
    }
}
