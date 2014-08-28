namespace FacebookFeatures
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    ///This is the strategy interface
    /// </summary>
    public interface IFileTypeSerializer
    {
        void SerializeList(List<string> i_ListToSerialize, string i_FileNameToSave);

        void DeserializeList(List<string> i_ListToDeserialize, string i_FileNameToLoad);
    }
}
