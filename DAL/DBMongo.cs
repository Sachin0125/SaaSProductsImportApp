using SaaSProductsImportApp.Modal;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaaSProductsImportApp.DAL
{
    public class DBMongo:IDB
    {
        public int Insert(IEnumerable<ProductDTO> product) { return 1; }
        public Boolean Update(ProductDTO product) { return true; }
        public Boolean Delete(int id) { return true; }
    }
}
