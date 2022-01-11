using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Kairos.Library.Serialization
{
    public class XmlSerialization
    {
        public string ObjectToXmlString(object ObjectSource)
        { 
            StringWriter Output = new StringWriter(new StringBuilder());
            try
            {
                XmlSerializer SerializerObj = new XmlSerializer(ObjectSource.GetType());
                SerializerObj.Serialize(Output, ObjectSource);
            }
            catch { throw; }
            return Output.ToString();
        }

        public void ObjectToXmlFile(object ObjectSource, string AddressOutputFile)
        {
            try
            {
                TextWriter WriteFileStream = new StreamWriter(AddressOutputFile); // (@"C:\Downloads\test.xml");
                XmlSerializer SerializerObj = new XmlSerializer(ObjectSource.GetType());
                SerializerObj.Serialize(WriteFileStream, ObjectSource);
                // Cleanup
                WriteFileStream.Close();
            }
            catch { throw; }
        }            

        public Object XmlStringToObject(String XmlStringSource, Object ObjectToCreate)
        {
            XmlSerializer SerializerObj = null;
            Object objectOutput = null;
            try
            {
                StringReader input = new StringReader(XmlStringSource);
                SerializerObj = new XmlSerializer(ObjectToCreate.GetType());
                objectOutput = SerializerObj.Deserialize(input);
            }
            catch (Exception) { throw; }
            return objectOutput;
        }

        public Object XmlFileToObject(string AddressOutputFile, object ObjectToCreate)
        {
            XmlSerializer SerializerObj = new XmlSerializer(ObjectToCreate.GetType());
            Object objectOutput = null;
            try
            {
                FileStream ReadFileStream = new FileStream(AddressOutputFile, FileMode.Open, FileAccess.Read, FileShare.Read);
                objectOutput = SerializerObj.Deserialize(ReadFileStream);
                // Cleanup
                ReadFileStream.Close();
            }
            catch (Exception) { throw; }
            return objectOutput;
        }
    }

}
