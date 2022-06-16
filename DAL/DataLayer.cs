using System;
using System.Collections.Generic;
using System.Text;
using SaaSProductsImportApp.Modal;

namespace SaaSProductsImportApp.DAL
{
    public class DataLayer
    {
        private IDB _DBService;
        public DataLayer(IDB service)
        {
            this._DBService = service;
        }
        public int Insert(IEnumerable<ProductDTO> product)
        {
           return this._DBService.Insert(product);
        }
        public Boolean Update(ProductDTO product)
        {
            return this._DBService.Update(product);
        }
        public Boolean Delete(int id)
        {
            return this._DBService.Delete(id);
        }
    }
    public class ConnectDB
    {
        public static int Insert(IEnumerable<ProductDTO> prod) {         
             return new DataLayer(new DBSQLServer()).Insert(prod);
        }
    }
}
