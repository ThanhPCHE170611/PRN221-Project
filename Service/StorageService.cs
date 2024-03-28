using BusinessObject.Models;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class StorageService : IStorageService
    {
        private readonly IStorageRepo _storageRepository;

        public StorageService()
        {
            _storageRepository = new StorageAreaRepo();
        }

        public List<StorageArea> GetStorageAreas() => _storageRepository.GetStorageAreas();

        public StorageArea GetStorageAreaByID(int id) => _storageRepository.GetStorageAreaByID(id);

        public bool AddStorageArea(StorageArea storage) => _storageRepository.AddStorageArea(storage);

        public bool UpdateStorageArea(StorageArea storage) => _storageRepository.UpdateStorageArea(storage);

        public bool BanStorageAreaStatus(int areaId) => _storageRepository.BanStorageAreaStatus(areaId);
        public List<StorageArea> LoadArea() => _storageRepository.LoadArea();
    }
}
