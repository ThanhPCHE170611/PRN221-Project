using BusinessObject.Models;
using DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class StorageAreaRepo : IStorageRepo
    {

        private readonly StorageAreaDAO _storageAreaDAO;

        public StorageAreaRepo()
        {
            _storageAreaDAO = new StorageAreaDAO();
        }

        public List<StorageArea> GetStorageAreas() => _storageAreaDAO.GetStorageAreas();

        public StorageArea GetStorageAreaByID(int id) => _storageAreaDAO.GetStorageAreaByID(id);

        public bool AddStorageArea(StorageArea storage) => _storageAreaDAO.AddStorageArea(storage);

        public bool UpdateStorageArea(StorageArea storage) => _storageAreaDAO.UpdateStorageArea(storage);

        public bool BanStorageAreaStatus(int areaId) => _storageAreaDAO.BanStorageAreaStatus(areaId);
        public List<StorageArea> LoadArea() => _storageAreaDAO.LoadArea();
    }
}
