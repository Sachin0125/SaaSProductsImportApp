using System;
using System.Collections.Generic;
using System.Text;
using SaaSProductsImportApp.Modal;
using SaaSProductsImportApp.DAL;
namespace SaaSProductsImportApp.BAL
{
    public class ImportProducts
    {
        /// <summary>
        /// Select provider as per the user input command
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="filePath"></param>
        public void SelectProvider(string provider, string filePath)
        {
            switch (provider)
            {
                case "capterra":
                    ImportProductByProvider(new CapterraCreator(), filePath);
                    break;
                case "softwareadvice":
                    ImportProductByProvider(new SoftwareadviceCreator(), filePath);
                    break;
                case "Others": //other providers
                default:
                    Console.WriteLine("Comming soon...");
                    break;
            }
        }

        /// <summary>
        /// Used to analyse user input feed and return the productDTO collection
        /// </summary>
        /// <param name="creatorObj"></param>
        /// <param name="filePath"></param>
        public void ImportProductByProvider(Creator creatorObj, string filePath)
        {
            IEnumerable<ProductDTO> importedProducts = (creatorObj).ImportProducts(filePath);
            ShowProgress(importedProducts);
            ConnectDB.Insert(importedProducts);
        }

        /// <summary>
        /// Push data in DB and display information on console
        /// </summary>
        /// <param name="importedProducts"></param>
        public void ShowProgress(IEnumerable<ProductDTO> products)
        {
            foreach (var item in products)
                Console.WriteLine($"importing: Name: { item.Name}; Categories: {item.Categories}; Twitter: {item.Twitter}");
        }
    }
}
