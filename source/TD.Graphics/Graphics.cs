using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

using System.Xml;
using System.Xml.Serialization;

using SdlDotNet.Core;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Primitives;
using SdlDotNet.Graphics.Sprites;

using TD.GameLogic;

namespace TD.Graphics
{

    public class CreepTextureEntry
    {
        [XmlAttribute("Name")] public String Name { get; set; }

        [XmlArray("FrameUp")] public List<String> FrameUp { get; set; }
        [XmlArray("FrameDown")] public List<String> FrameDown { get; set; }
        [XmlArray("FrameLeft")] public List<String> FrameLeft { get; set; }
        [XmlArray("FrameRight")]  public List<String> FrameRight { get; set; }

        public CreepTextureEntry()
        {
            Name = "none";

            FrameUp = new List<String>();
            FrameDown = new List<String>();
            FrameLeft = new List<String>();
            FrameRight = new List<String>();
        }
    }

    public class CreepTextureIndex
    {
        [XmlArray("Entry")] public List<CreepTextureEntry> Index { get; set; }

        public CreepTextureIndex()
        {
            Index = new List<CreepTextureEntry>();
        }

        public CreepTextureEntry GetEntry(String GfxName)
        {
            CreepTextureEntry Entry = new CreepTextureEntry();

            foreach (CreepTextureEntry e in Index)
            {
                if (e.Name == GfxName)
                {
                    return e;
                }
            }

            return Entry;
        }

        public static void SaveIndex(CreepTextureIndex Index,String Path)
        {
            XmlSerializer Xs = new XmlSerializer(typeof(CreepTextureIndex));

            TextWriter writer = new StreamWriter(Path);

            Xs.Serialize(writer, Index);

            writer.Close();
        }

        public static CreepTextureIndex LoadIndex(String Path)
        {
            CreepTextureIndex Index = new CreepTextureIndex();

            XmlSerializer Xs = new XmlSerializer(typeof(CreepTextureIndex));

            StreamReader reader = new StreamReader(Path);

            Index = (CreepTextureIndex)Xs.Deserialize(reader);

            reader.Close();

            return Index;

        }
    }

    public class DirectionnalSurfaces
    {
        public List<Surface> Up { get; set; }
        public List<Surface> Left { get; set; }
        public List<Surface> Right { get; set; }
        public List<Surface> Down { get; set; }

        public DirectionnalSurfaces()
        {
            Up = new List<Surface>();
            Left = new List<Surface>();
            Right = new List<Surface>();
            Down = new List<Surface>();
        }
    }

    public class DirectionnalSprite : Sprite
    {
        public Direction Direction { get; set; }

        public List<Surface> SurfaceUp { get; set; }
        public List<Surface> SurfaceLeft { get; set; }
        public List<Surface> SurfaceRight { get; set; }
        public List<Surface> SurfaceDown { get; set; }

        public int FramePause { get; set; }
        private int Tick = 0;
        private int Pause = 0;

        public DirectionnalSprite(DirectionnalSurfaces Textures)
            : base()
        {
            SurfaceUp = Textures.Up;
            SurfaceDown = Textures.Down;
            SurfaceLeft = Textures.Left;
            SurfaceRight = Textures.Right;
            Direction = Direction.Unset;
            AllowDrag = false;
            Surface = new Surface(1, 1);

            FramePause = 2;
        }

        public void UpdateFrame()
        {
            List<Surface> toUse = new List<Surface>();

            if (Pause == FramePause)
            {

                if (Direction == Direction.Up)
                {
                    toUse = SurfaceUp;
                }
                else if (Direction == Direction.Down)
                {
                    toUse = SurfaceDown;
                }
                else if (Direction == Direction.Left)
                {
                    toUse = SurfaceLeft;
                }
                else if (Direction == Direction.Right)
                {
                    toUse = SurfaceRight;
                }

                if (toUse.Count != 0)
                {
                    int Fid = Tick % toUse.Count;

                    Surface = toUse[Fid];
                }

                Pause = 0;
                Tick++;
            }
            else
            {
                Pause++;
            }
        }

    }
}
