using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Freightware.WebApi.Repository
{
    public class FWRepositoryGeneric
    {
        public string ReturnMessage { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string InRequest { get; set; } = "30";

        /// <summary>
        /// Convert an object to XML
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string GetXmlFromObject(object o)
        {
            //var serializer = new XmlSerializer(o.GetType());
            //using var writer = new Utf8StringWriter();
            //serializer.Serialize(writer, o);
            //var utf8 = writer.ToString();
            //return utf8;
            var serializer = new XmlSerializer(o.GetType());
            var str = new StringWriter();
            using XmlWriter writer = XmlWriter.Create(str, new XmlWriterSettings { OmitXmlDeclaration = true });
            serializer.Serialize(writer, o);
            return str.ToString();
        }

        public class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding => Encoding.UTF8;
        }

    }
}
