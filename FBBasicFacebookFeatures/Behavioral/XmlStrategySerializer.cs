namespace FacebookFeatures
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using System.IO;

    /// <summary>
    /// This is a concrete strategy
    /// </summary>
    public class XmlStrategySerializer : IFileTypeSerializer
    {
        public void SerializeList(List<string> i_ListToSerialize, string i_XmlFileNameToSave)
        {
            StringBuilder fileNameToSave = new StringBuilder();
            fileNameToSave.AppendFormat("{0}{1}", i_XmlFileNameToSave, ".xml");

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<string>));
            using (FileStream fileStream = File.OpenWrite(fileNameToSave.ToString()))
            {
                fileStream.SetLength(0);
                fileStream.Seek(0, SeekOrigin.Begin);
                xmlSerializer.Serialize(fileStream, i_ListToSerialize);
            }
        }

        public void DeserializeList(List<string> i_ListToDeserialize, string i_FileNameToLoad)
        {
            StringBuilder fileNameToLoadWithExtention = new StringBuilder();
            fileNameToLoadWithExtention.AppendFormat("{0}{1}", i_FileNameToLoad, ".xml");
            string fileName = fileNameToLoadWithExtention.ToString();

            if (File.Exists(fileName))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<string>));
                using (FileStream fileStream = File.OpenRead(fileName))
                {
                    i_ListToDeserialize.Clear();
                    i_ListToDeserialize.AddRange(xmlSerializer.Deserialize(fileStream) as List<string>);
                }
            }
        }
    }
}
