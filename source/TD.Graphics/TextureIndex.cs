using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using System.Xml;
using System.Xml.Serialization;

using SdlDotNet.Core;
using SdlDotNet.Graphics;

namespace TD.Graphics
{
    public class SurfaceDictionary : Dictionary<String,Surface>
    {
        public SurfaceDictionary() : base()
        {

        }
    }

    public class TextureEntry
    {
        [XmlAttribute("Name")] public String Name { get; set; }
        [XmlAttribute("Path")] public String Path { get; set; }

        public TextureEntry()
        {
            Name = "None";
            Path = "None";
        }

        public TextureEntry(String Name, String Path)
        {
            this.Name = Name;
            this.Path = Path;
        }
    }

    [XmlRoot("TD.Graphics.TextureIndex")]
    public class TextureIndex
    {
        [XmlArrayItem("Texture")] public List<TextureEntry> TextureList { get; set; }

        public TextureIndex()
        {
            TextureList = new List<TextureEntry>();
        }

        public void Add(String Name, String Path)
        {
            TextureList.Add(new TextureEntry(Name, Path));
        }

        public void Add(TextureEntry Entry)
        {
            TextureList.Add(Entry);
        }

        public SurfaceDictionary LoadTextures()
        {
            SurfaceDictionary Textures = new SurfaceDictionary();

            foreach (TextureEntry Entry in TextureList)
            {
                Surface Tex = new Surface("../../../../assets/" + Entry.Path);
                Textures.Add(Entry.Name, Tex);
            }

            return Textures;
        }

        public static void SaveTextureIndex(TextureIndex Index, String Filename)
        {
            XmlSerializer XS = new XmlSerializer(typeof(TextureIndex));
            TextWriter stream = new StreamWriter(Filename);
            XS.Serialize(stream, Index);
            stream.Close();
        }

        public static TextureIndex LoadTextureIndex(String Filename)
        {
            TextureIndex Index;
            XmlSerializer XS = new XmlSerializer(typeof(TextureIndex));

            TextReader stream = new StreamReader(Filename);

            Index = (TextureIndex)XS.Deserialize(stream);

            stream.Close();

            return Index;
        }
    }
}
