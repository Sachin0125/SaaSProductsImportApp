using SaaSProductsImportApp.Modal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SaaSProductsImportApp.DAL
{
    public class DBSQLServer:IDB
    {
        public int Insert(IEnumerable<ProductDTO> product) {
            Thread.Sleep(2000);
            return 1;
        }
        public Boolean Update(ProductDTO product) { return true; }
        public Boolean Delete(int id) { return true; }
    }
}
