using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using YamlDotNet.Serialization;
using Newtonsoft.Json;
using SaaSProductsImportApp.Modal;

namespace SaaSProductsImportApp
{
    /// <summary>
    ///
    /// </summary>
    public abstract class Creator
    {
        public abstract IProvider GetProviderObj();
        public IEnumerable<ProductDTO> ImportProducts(string feedURI)
        {
            var providerObj = GetProviderObj();
            providerObj.feedURI = feedURI;
            XDocument oldDocument = providerObj.ParseInputFile();
            XDocument newDocument = new XDocument();
            providerObj.ParseXML(ref oldDocument, ref newDocument);
            return providerObj.MapDTO(newDocument);
        }
    }
    /// <summary>
    /// Create Capterra provider object
    /// </summary>
    class CapterraCreator : Creator
    {
        public override IProvider GetProviderObj()
        {
            return new Capterra();
        }
    }

    /// <summary>
    /// Create Softwareadvice provider object
    /// </summary>
    class SoftwareadviceCreator : Creator
    {
        public override IProvider GetProviderObj()
        {
            return new Softwareadvice();
        }
    }

    /// <summary>
    /// Actions to parse any format file to XML file
    /// </summary>
    public interface IProvider
    {
        string feedFormat { get; }
        string providerName { get; }
        string feedURI { get; set; }
        XDocument ParseInputFile();
        string LoadXSL(string provider);
        void ParseXML(ref XDocument Source, ref XDocument Target);
        IEnumerable<ProductDTO> MapDTO(XDocument xmlDocument);


    }

    /// <summary>
    /// Provide defualt implementation
    /// </summary>
    public abstract class Provider : IProvider
    {
        /// <summary>
        /// Fommat of incomming feed
        /// </summary>
        public abstract string feedFormat { get; }

        /// <summary>
        /// Provider Name
        /// </summary>
        public abstract string providerName { get; }
        /// <summary>
        /// Incomming feed URI (filepath/URL)
        /// </summary>
        public virtual string feedURI { get; set; }

        /// <summary>
        /// Parse input file(any format) into xml type file
        /// </summary>
        /// <returns></returns>
        public abstract XDocument ParseInputFile();

        /// <summary>
        /// Load provider specific XSL file
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public virtual string LoadXSL(string provider)
        {
            XmlDocument xmlDoc = new XmlDocument();
            string workingDirectory = Environment.CurrentDirectory;
            string filePath = Path.GetFullPath(string.Format(@".\XSL\{0}.xslt", this.providerName), workingDirectory);
            xmlDoc.Load(filePath);
            string xslString = string.Empty;
            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xw = new XmlTextWriter(sw))
                {
                    xmlDoc.WriteTo(xw);
                    xslString = sw.ToString();
                }
            }
            return xslString;
        }
        /// <summary>
        /// Sanitize (as per XSL) and create new XML file
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Target"></param>
        public virtual void ParseXML(ref XDocument Source, ref XDocument Target)
        {
            string xslString = LoadXSL("");
            using (var xslStringReader = new StringReader(xslString))
            {
                using (XmlReader xslReader = XmlReader.Create(xslStringReader))
                {
                    var transformer = new XslCompiledTransform();
                    transformer.Load(xslReader);
                    using (XmlReader sourceDocumentReader = Source.CreateReader())
                    {
                        using (XmlWriter targetDocumentWriter = Target.CreateWriter())
                        {
                            transformer.Transform(sourceDocumentReader, targetDocumentWriter);
                        }
                    }

                }
            }
        }

        /// <summary>
        /// Fetch data from XML and map to the ProductDTO object
        /// </summary>
        /// <param name="xmlDocument"></param>
        /// <returns></returns>
        public IEnumerable<ProductDTO> MapDTO(XDocument xmlDocument)
        {
            return (from c in xmlDocument.Descendants("Product")
                    select new ProductDTO()
                    {
                        Name = c.Element("Name").Value,
                        Categories = c.Element("Categories").Value,
                        Twitter = c.Element("Twitter").Value
                    }).ToList<ProductDTO>();
        }
    }

    /// <summary>
    /// Provide provider specific configuration
    /// </summary>
    public class Capterra : Provider
    {
        public override string feedFormat
        {
            get => "yaml";
        }
        public override string providerName
        {
            get => "capterra";
        }
        public override XDocument ParseInputFile()
        {
            string getFullPath = Path.GetFullPath(this.feedURI, Environment.CurrentDirectory);
            // Convert YAML string to YAML object
            string text = File.ReadAllText(getFullPath);
            using (StringReader stringReader = new StringReader(text))
            {
                var deserializer = new DeserializerBuilder().Build();
                var yamlObject = deserializer.Deserialize(stringReader);
                var serializer = new SerializerBuilder().JsonCompatible().Build();

                //yamlObject -> jsonObject -> xmlDocument
                var jsonObejct = serializer.Serialize(yamlObject);
                return JsonConvert.DeserializeXNode("{\"root \":" + jsonObejct + "}", "root");
            }
        }
    }
    /// <summary>
    /// Provide provider specific configuration
    /// </summary>
    public class Softwareadvice : Provider
    {
        public override string feedFormat
        {
            get => "json";
        }
        public override string providerName
        {
            get => "softwareadvice";
        }
        public override XDocument ParseInputFile()
        {
            string getFullPath = Path.GetFullPath(this.feedURI, Environment.CurrentDirectory);
            using (StreamReader reader = new StreamReader(getFullPath))
            {
                string json = reader.ReadToEnd();
                return JsonConvert.DeserializeXNode(json, "Root");
            }
        }

    }
}
