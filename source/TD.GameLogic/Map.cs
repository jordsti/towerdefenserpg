using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

using System.Xml;
using System.Xml.Serialization;

namespace TD.GameLogic
{

    public class MapCoordList : List<MapCoord>
    {
        public MapCoordList() : base()
        {

        }


        public bool ContainsCoord(MapCoord Coord)
        {
            foreach(MapCoord c in this)
            {
                if (c.Row == Coord.Row && c.Column == Coord.Column)
                {
                    return true;
                }
            }

            return false;
        }
		public MapCoord Before(MapCoord Coord)
		{
			int index = GetIndex(Coord);
			MapCoord before = new MapCoord();
			if(index > 0)
			{
				before = this.ElementAt(index-1);
			}
			
			return before;
		}
        public int GetIndex(MapCoord Coord)
        {
            int Index = -1;
            int i = 0;
            foreach (MapCoord c in this)
            {
                if (c.Row == Coord.Row && c.Column == Coord.Column)
                {
                    Index = i;
                    break;
                }

                i++;
            }

            return Index;
        }

    }

    public class MapCoord
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public bool isEmpty { get; set; }

        public MapCoord()
        {
            Row = -1;
            Column = -1;
            isEmpty = true;
        }

        public MapCoord(Point p)
        {
            Row = p.Y/Tile.TILE_HEIGHT;
            Column = p.X/Tile.TILE_WIDTH;
            isEmpty = false;
        }

        public MapCoord(int Row, int Column)
        {
            this.Row = Row;
            this.Column = Column;
            this.isEmpty = false;
        }
		
		public override string ToString ()
		{
			return string.Format("[MapCoord: Row={0}, Column={1}, isEmpty={2}]", Row, Column, isEmpty);
		}


        public Point ToPoint()
        {
            return new Point(Column * Tile.TILE_WIDTH, Row * Tile.TILE_HEIGHT);
        }

        public Point ToPointMiddle()
        {
            return new Point((Column * Tile.TILE_WIDTH) + (Tile.TILE_WIDTH / 2) - 1, (Row * Tile.TILE_HEIGHT) + (Tile.TILE_HEIGHT / 2) - 1);
        }

        public double DistanceWith(MapCoord Coord)
        {
            double Distance = 0;

            double rowdiff = Math.Abs(Row - Coord.Row);
            double coldiff = Math.Abs(Column - Coord.Column);
            rowdiff *= rowdiff;
            coldiff *= coldiff;


            Distance = Math.Sqrt(rowdiff + coldiff);


            return Distance;
        }

        public int DistanceWith(Point p)
        {
            double Distance = 0;
            double xdiff = Math.Abs(p.X - ToPointMiddle().X);
            double ydiff = Math.Abs(p.Y - ToPointMiddle().Y);
            xdiff *= xdiff;
            ydiff *= ydiff;

            Distance = Math.Sqrt(xdiff + ydiff);

            return (int)Math.Floor(Distance);
        }

        public MapCoord Diff(MapCoord Coord)
        {
            int rowdiff = Coord.Row - Row;
            int coldiff = Coord.Column - Column;

            return new MapCoord(rowdiff, coldiff);
        }

        public bool SameCoord(MapCoord Coord)
        {
            if (Coord.Row == Row && Coord.Column == Column)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }

    public enum TileType { Normal,CreepPath,CreepStart,CreepEnd,Obstacle,Unset }

    public class Tile
    {
        public const int TILE_WIDTH = 40, TILE_HEIGHT = 40;
        [XmlAttribute("Texture")] public String Texture { get; set; }
        [XmlAttribute("TileType")] public TileType Type { get; set; }
        [XmlIgnore()] public Unit UnitOn { get; set; }

        public Tile()
        {
            Texture = "None";
            Type = TileType.Normal;
            UnitOn = new NoUnit();
        }

        public Tile(String Texture, TileType Type)
        {
            this.Texture = Texture;
            this.Type = Type;
            UnitOn = new NoUnit();
        }
    }

    [XmlRoot("TD.GameLogic.Map")]
    public class Map
    {
        [XmlAttribute("Rows")] public int Rows { get; set; }
        [XmlAttribute("Columns")] public int Columns { get; set; }
        [XmlArray("Tiles")][XmlArrayItem("TilesRow")] public Tile[][] Tiles { get; set; }
        [XmlAttribute("MapName")] public String MapName { get; set; }
		[XmlAttribute("CreepListFile")] public String CreepListFile {get; set;}
        [XmlAttribute("DefaultTileTexture")] public String DefaultTileTexture { get; set; }

        public Map()
        {
            this.MapName = "NoName";
            DefaultTileTexture = "Normal 1";
            Rows = 12;
            Columns = 12;
			CreepListFile = "DefaultCreepsList.xml";

            EmptyMap();
        }

        public Map(String MapName)
        {
            this.MapName = MapName;
            DefaultTileTexture = "Normal 1";
            Rows = 12;
            Columns = 12;

            EmptyMap();
        }

        public Map(String MapName, int Rows, int Columns)
        {
            DefaultTileTexture = "Normal 1";
            this.MapName = MapName;
            this.Rows = Rows;
            this.Columns = Columns;
            EmptyMap();
        }

        public static void SaveMap(Map map, String Filename)
        {
            XmlSerializer XS = new XmlSerializer(typeof(Map));
            TextWriter stream = new StreamWriter(Filename);
            XS.Serialize(stream, map);
            stream.Close();
        }

        public static Map LoadMap(String Filename)
        {
            Map map;
            XmlSerializer XS = new XmlSerializer(typeof(Map));

            if (File.Exists(Filename))
            {

                TextReader stream = new StreamReader(Filename);

                map = (Map)XS.Deserialize(stream);

                stream.Close();
            }
            else
            {
                map = new Map();
            }

            return map;
        }

        public static List<String> GetMapsList(String Folder)
        {
            List<String> Maps = new List<String>();

            String[] path = Directory.GetFiles(Folder);

            foreach (String str in path)
            {
                if (str.EndsWith(".xml"))
                {
                    String tmp_str = str.Substring(0, str.Length - 4);

                    String[] arr = tmp_str.Split('/');

                    tmp_str = arr[arr.Length - 1];

                    String[] arr2 = tmp_str.Split('\\');

                    tmp_str = arr2[arr2.Length - 1];

                    Maps.Add(tmp_str);
                }
            }

            return Maps;
        }

        public void EmptyMap()
        {
            Tiles = new Tile[Rows][];

            for (int i = 0; i < Rows; i++)
            {
                Tiles[i] = new Tile[Columns];

                for (int j = 0; j < Columns; j++)
                {
                    Tiles[i][j] = new Tile();
                }
            }
        }
    }
}
