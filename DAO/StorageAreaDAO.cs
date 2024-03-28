using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class StorageAreaDAO
    {
        private static StorageAreaDAO instance = null;

        public StorageAreaDAO() { }

        public static StorageAreaDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new StorageAreaDAO();
                }
                return instance;
            }
        }

        public List<StorageArea> GetStorageAreas()
        {
            List<StorageArea> storageArea;
            try
            {
                var context = new PRN221_ProjectContext();
                storageArea = context.StorageAreas
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return storageArea;
        }

        public StorageArea GetStorageAreaByID(int id)
        {
            StorageArea storage = null;
            try
            {
                var db = new PRN221_ProjectContext();
                storage = db.StorageAreas.SingleOrDefault(u => u.AreaId == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return storage;
        }

        public bool AddStorageArea(StorageArea storage)
        {
            try
            {
                using (var db = new PRN221_ProjectContext())
                {
                    bool existingStorageArea = db.StorageAreas
                        .Any(a => a.AreaCode.ToLower().Equals(storage.AreaCode.ToLower()) || a.AreaName.ToLower().Equals(storage.AreaName.ToLower()));

                    if (!existingStorageArea)
                    {
                        storage.Status = 1;
                        db.StorageAreas.Add(storage);
                        db.SaveChanges();
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("StorageArea already exists!");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Add StorageArea: {ex.Message}", ex);
                return false;
            }
        }

        public bool UpdateStorageArea(StorageArea storage)
        {
            try
            {
                using (var db = new PRN221_ProjectContext())
                {
                    var existing = db.StorageAreas.SingleOrDefault(x => x.AreaId == storage.AreaId);
                    if (existing != null)
                    {
                        existing.AreaCode = storage.AreaCode;
                        existing.AreaName = storage.AreaName;
                        existing.Status = storage.Status;

                        db.SaveChanges();
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("StorageArea not found for updating.");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateStorageArea: {ex.Message}", ex);
                return false;
            }
        }

        public bool BanStorageAreaStatus(int areaId)
        {
            try
            {
                using (var db = new PRN221_ProjectContext())
                {
                    StorageArea storageArea = db.StorageAreas.SingleOrDefault(s => s.AreaId == areaId);

                    if (storageArea != null)
                    {
                        bool area = db.Products.Any(x => x.AreaId == areaId);
                        if (!area)
                        {
                            storageArea.Status = (storageArea.Status == 0) ? 1 : 0;

                            db.Entry(storageArea).State = EntityState.Modified;
                            db.SaveChanges();
                            return true;
                        } else
                        {
                            throw new Exception("Can not ban!!");
                        }
                    }
                    else
                    {
                        throw new Exception("Storage does not exist!");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in BanStorage: {ex.Message}");
            }
        }

        public List<StorageArea> LoadArea()
        {
            List<StorageArea> loadArea;
            try
            {
                var myStoreDB = new PRN221_ProjectContext();
                loadArea = myStoreDB.StorageAreas
               .Where(area => area.AreaId != null)
               .Select(area => new StorageArea
               {
                   AreaId = area.AreaId,
                   AreaName = area.AreaName
               })
               .Distinct()
               .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return loadArea;
        }
    }
}
