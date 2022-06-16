using System;
using System.Collections.Generic;
using System.Text;
using SaaSProductsImportApp.Modal;

namespace SaaSProductsImportApp.DAL
{
    public interface IDB
    {
        int Insert(IEnumerable<ProductDTO> product);
        Boolean Update(ProductDTO product);
        Boolean Delete(int id);
    }
}
