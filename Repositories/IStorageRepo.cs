using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IStorageRepo
    {
        List<StorageArea> GetStorageAreas();
        StorageArea GetStorageAreaByID(int id);
        bool AddStorageArea(StorageArea storage);
        bool UpdateStorageArea(StorageArea storage);
        bool BanStorageAreaStatus(int areaId);
        List<StorageArea> LoadArea();
    }
}
