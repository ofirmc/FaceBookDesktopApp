namespace FacebookFeatures
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using System.IO;
   
    /// <summary>
    /// This is the context part of strategy pattern
    /// </summary>
    public class ObjectSerializer
    {
        public ObjectSerializer(IFileTypeSerializer i_FileTypeSerializer)
        {
            FileTypeSerializer = i_FileTypeSerializer;
        }

        public IFileTypeSerializer FileTypeSerializer { get; set; }

        public void SerializeFromList(List<string> i_Playlist, string i_FileNameToSave)
        {            
            FileTypeSerializer.SerializeList(i_Playlist, i_FileNameToSave);
        }

        public void DeserializeToList(List<string> i_Playlist, string i_FileNameToLoad)
        {            
            FileTypeSerializer.DeserializeList(i_Playlist, i_FileNameToLoad);
        }
    }
}
