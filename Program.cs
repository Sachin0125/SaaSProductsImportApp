using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using System.Linq;
using YamlDotNet.Serialization;
using Newtonsoft.Json;
using System.Threading;
using SaaSProductsImportApp.Modal;
using SaaSProductsImportApp.DAL;
using SaaSProductsImportApp.BAL;
namespace SaaSProductsImportApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //$ import capterra feed-products/capterra.yaml
            Console.WriteLine("Enter Provider and FeedLocation(-999 to exit):");
            while (true)
            {
                //Get user input command
                string userInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(userInput))
                {
                    if (int.TryParse(userInput, out int i) && i == -999)
                        break;

                    string[] userInputs = userInput?.Split(' ');
                    if (userInputs?.Length == 3 && userInputs[0]?.ToLower() == "import")
                    {
                        string provider = userInputs[1];
                        string filePath = userInputs[2];

                        try
                        {
                            ImportProducts product = new ImportProducts();
                            //Analyse user input
                            product.SelectProvider(provider, filePath);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Message: {ex.Message}, InnerException: {ex.InnerException}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid command");
                    }
                }
                Console.WriteLine("Enter Provider and FeedLocation:");
            }
        }
    }
}
