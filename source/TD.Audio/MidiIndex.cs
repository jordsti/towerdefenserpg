using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using System.Xml;
using System.Xml.Serialization;

using SdlDotNet.Core;
using SdlDotNet.Graphics;
using SdlDotNet.Audio;

namespace TD.Audio
{
    public class MidiDictonary : Dictionary<String, Music>
    {
        public MidiDictonary() : base()
        {

        }
    }

    public class MidiEntry
    {
        [XmlAttribute("Name")] public String Name { get; set; }
        [XmlAttribute("Path")] public String Path { get; set; }

        public MidiEntry()
        {
            Name = "None";
            Path = "None";
        }

        public MidiEntry(String Name, String Path)
        {
            this.Name = Name;
            this.Path = Path;
        }
    }

    [XmlRoot("TD.Audio.MidiIndex")]
    public class MidiIndex
    {
        [XmlArrayItem("Midi")] public List<MidiEntry> MidiList { get; set; }

        public MidiIndex()
        {
            MidiList = new List<MidiEntry>();
        }

        public void Add(MidiEntry Entry)
        {
            MidiList.Add(Entry);
        }

        public void Add(String Name, String Path)
        {
            MidiList.Add(new MidiEntry(Name,Path));
        }

        public static void SaveMidiIndex(MidiIndex Index, String Filename)
        {
            XmlSerializer XS = new XmlSerializer(typeof(MidiIndex));
            TextWriter stream = new StreamWriter(Filename);
            XS.Serialize(stream, Index);
            stream.Close();
        }

        public static MidiIndex LoadMidiIndex(String Filename)
        {
            MidiIndex Index;
            XmlSerializer XS = new XmlSerializer(typeof(MidiIndex));

            TextReader stream = new StreamReader(Filename);

            Index = (MidiIndex)XS.Deserialize(stream);

            stream.Close();

            return Index;
        }

        public MidiDictonary LoadMidi()
        {
            MidiDictonary Midis = new MidiDictonary();

            foreach (MidiEntry Entry in MidiList)
            {
                Midis.Add(Entry.Name, new Music(Entry.Path));
            }


            return Midis;
        }

        public List<Music> LoadMidiList()
        {
            List<Music> Midis = new List<Music>();

            foreach (MidiEntry Entry in MidiList)
            {
                Midis.Add(new Music(Entry.Path));
            }

            for (int i = 0; i < Midis.Count - 1; i++)
            {
                Midis[i].QueuedMusic = Midis[i + 1];
            }

            Midis[Midis.Count - 1].QueuedMusic = Midis[0];


            return Midis;
        }
    }
}
