namespace FacebookFeatures
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Runtime.Serialization.Json;
    using System.IO;

    /// <summary>
    /// This is a concrete strategy
    /// </summary>
    public class JsonStrategySerializer : IFileTypeSerializer
    {
        public void SerializeList(List<string> i_ListToSerialize, string i_jsonFileNameToSave)
        {
            StringBuilder fileNameToSave = new StringBuilder();
            fileNameToSave.AppendFormat("{0}{1}", i_jsonFileNameToSave, ".json");

            DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(List<string>));
            using (FileStream fileStream = File.OpenWrite(fileNameToSave.ToString()))
            {
                fileStream.SetLength(0);
                fileStream.Seek(0, SeekOrigin.Begin);
                dataContractJsonSerializer.WriteObject(fileStream, i_ListToSerialize);                
            }
        }

        public void DeserializeList(List<string> i_ListToDeserialize, string i_jsonFileNameToLoad)
        {
            StringBuilder fileNameToLoadWithExtention = new StringBuilder();
            fileNameToLoadWithExtention.AppendFormat("{0}{1}", i_jsonFileNameToLoad, ".json");
            string fileName = fileNameToLoadWithExtention.ToString();

            if (File.Exists(fileName))
            {
                DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(List<string>));
                using (FileStream fileStream = File.OpenRead(fileName))
                {
                    i_ListToDeserialize.Clear();
                    i_ListToDeserialize.AddRange(dataContractJsonSerializer.ReadObject(fileStream) as List<string>);
                }
            }
        }
    }
}
