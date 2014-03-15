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


    public class EmptyCreepUnit : CreepUnit
    {
        public EmptyCreepUnit()
            : base("empty",0,0)
        {

        }
    }


	public class CreepUnitList : List<CreepUnit>
	{

		public int CreepsTotal { get; private set; }
		
		
		public CreepUnitList () : base() 
		{
			
			
		}
		public CreepUnitList(CreepUnit Unit,int UnitCount) : base()
		{
			CreepsTotal = UnitCount;
			for(int i=0; i<UnitCount; i++)
			{
				CreepUnit newUnit = new CreepUnit(Unit.Name,Unit.Health,Unit.StealAmount);
				newUnit.Tips = Unit.Tips;
				newUnit.Speed = Unit.Speed;
				newUnit.Level = Unit.Level;
				newUnit.Resist = Unit.Resist;
				newUnit.Weakness = Unit.Weakness;
				Add(newUnit);
			}
		}
		
		public CreepUnitList InRectangle(Rectangle Rect)
		{
			CreepUnitList InRect = new CreepUnitList();
					
			foreach(CreepUnit Unit in this)
			{
				if(Rect.Contains(Unit.Position) && Unit.Health != 0)
				{
					InRect.Add(Unit);
				}
			}
			
			return InRect;
		}
		
		public CreepUnitList EnemyInRange(MapCoord Coord,int RangePx,int MinusRangePx)
        {
            CreepUnitList InRange = new CreepUnitList();

            foreach (CreepUnit Unit in this)
            {
                List<Point> RectCorner = new List<Point>();
                RectCorner.Add(new Point(Unit.Position.X, Unit.Position.Y));
                RectCorner.Add(new Point(Unit.Position.X, Unit.Position.Y + CreepUnit.CREEP_HEIGHT_PX));
                RectCorner.Add(new Point(Unit.Position.X + CreepUnit.CREEP_WIDTH_PX, Unit.Position.Y));
                RectCorner.Add(new Point(Unit.Position.X + CreepUnit.CREEP_WIDTH_PX, Unit.Position.Y + CreepUnit.CREEP_HEIGHT_PX));
                int CreepDiff = Coord.DistanceWith(Unit.Position);

                foreach (Point p in RectCorner)
                {
                    int tmpDist = Coord.DistanceWith(p);
                    if (tmpDist < CreepDiff && tmpDist >= MinusRangePx)
                    {
                        CreepDiff = tmpDist;
                    }

                }

                RectCorner.Clear();

                if (CreepDiff < RangePx && CreepDiff >= MinusRangePx)
                {
                    InRange.Add(Unit);
                }
            }

            return InRange;
        }
		
	}
	
	public class EnemyWave 
	{
		public List<CreepUnit> Creeps { get; set;}
        public MapCoordList CreepPath { get; set; }
        public long Tick = 0;
        public bool StartNewCreep = true;

        public EnemyWave(MapCoordList CreepPath)
		{
            this.CreepPath = CreepPath;
			Creeps = new List<CreepUnit>();
		}
		
		public EnemyWave(MapCoordList CreepPath,CreepUnit Unit)
		{
            this.CreepPath = CreepPath;
			Creeps = new List<CreepUnit>();
			
			for(int i=0; i<20; i++)
			{
				CreepUnit newUnit = new CreepUnit(Unit.Name,Unit.Health,Unit.StealAmount);
				newUnit.Tips = Unit.Tips;
				newUnit.Speed = Unit.Speed;
				newUnit.Level = Unit.Level;
				newUnit.Resist = Unit.Resist;
				newUnit.Weakness = Unit.Weakness;
				Creeps.Add(newUnit);
			}
		}

        public void CreepMoveTick()
        {

            if (Tick % 5 == 0)
            {
                StartNewCreep = true;
            }
			
            foreach (CreepUnit Unit in Creeps)
            {
                if (StartNewCreep && Unit.Position.IsEmpty)
                {
                    StartNewCreep = false;
                    Unit.Position = CreepPath[0].ToPoint();
                }
                else if(!Unit.Position.IsEmpty)
                {
                    MapCoord UnitCoord = new MapCoord(Unit.Position);
                    int Next = CreepPath.GetIndex(UnitCoord)+1;

                    MapCoord Diff = UnitCoord.Diff(CreepPath[Next]);

                    if (Diff.Row == 1)
                    {
                        Unit.Position = new Point(Unit.Position.X, Unit.Position.Y + Unit.Speed);
                    }
                    else if (Diff.Row == -1)
                    {
                        Unit.Position = new Point(Unit.Position.X, Unit.Position.Y - Unit.Speed);
                    }
                    else if (Diff.Column == 1)
                    {
                        Unit.Position = new Point(Unit.Position.X + Unit.Speed, Unit.Position.Y);
                    }
                    else if (Diff.Column == -1)
                    {
                        Unit.Position = new Point(Unit.Position.X - Unit.Speed, Unit.Position.Y);
                    }
                }

            }

            Tick++;
        }

        public List<CreepUnit> EnemyInRange(MapCoord Coord,int RangePx,int MinusRangePx)
        {
            List<CreepUnit> InRange = new List<CreepUnit>();
            foreach (CreepUnit Unit in Creeps)
            {
                int CreepDiff = Coord.DistanceWith(Unit.Position);

                if (CreepDiff <= RangePx && CreepDiff >= MinusRangePx)
                {
                    InRange.Add(Unit);
                }
            }

            return InRange;
        }

        public int RemoveDeadCreeps()
        {
            int DeadCreeps = 0;
            List<Unit> toRemove = new List<Unit>();

            foreach (CreepUnit Unit in Creeps)
            {
                if (Unit.Health == 0)
                {
                    toRemove.Add(Unit);
                    DeadCreeps++;
                }
            }

            foreach(CreepUnit Unit in toRemove)
            {
                Creeps.Remove(Unit);
            }

            return DeadCreeps;
        }
        
		

	}
	
	
    public class CreepUnit : Unit
    {
        public const int CREEP_WIDTH_PX = 30;
        public const int CREEP_HEIGHT_PX = 30;

        public String Name { get; set; }
		public String Tips { get; set; }
        public DamageType Weakness { get; set; }
        public DamageType Resist { get; set; }
        public int StealAmount { get; set; }
        public int TotalHealth { get; set; }
        public Point Position { get; set; }
        public Direction Direction { get; set; }

        public CreepUnit(String Name,int Health,int StealAmount)
            : base()
        {
            this.Name = Name;
            this.Health = Health;
            this.TotalHealth = Health;
            this.StealAmount = StealAmount;
            Level = 1;
            Speed = 1;
            Weakness = DamageType.None;
            Resist = DamageType.None;
            Position = new Point();
            Direction = Direction.Unset;
        }


        public Point MiddlePosition()
        {
            return new Point(Position.X + CreepUnit.CREEP_WIDTH_PX / 2, Position.Y + CreepUnit.CREEP_WIDTH_PX / 2);
        }
		
		public XmlCreepWave ToXml()
		{
			return new XmlCreepWave(this);
		}
    }
	
    [XmlRoot("TD.GameLogic.XmlEnemyList")]
	public class XmlEnemyList
	{
        [XmlArray("CreepWave")] public List<XmlCreepWave> Wave { get; set; }
		
		public XmlEnemyList()
		{
            Wave = new List<XmlCreepWave>();
		}

        public static void SaveList(XmlEnemyList List,String Name)
        {
            XmlSerializer Xs = new XmlSerializer(typeof(XmlEnemyList));

            TextWriter writer = new StreamWriter(Name);

            Xs.Serialize(writer, List);

            writer.Close();
        }

        public static XmlEnemyList LoadList(String Name)
        {
            XmlEnemyList List = new XmlEnemyList();
            XmlSerializer Xs = new XmlSerializer(typeof(XmlEnemyList));

            TextReader reader = new StreamReader(Name);

            List = (XmlEnemyList)Xs.Deserialize(reader);

            reader.Close();

            return List;
        }
	}
	
	public class XmlCreepWave 
	{
        [XmlAttribute("Numbers")] public int Numbers { get; set; }
		[XmlAttribute("Health")] public int Health { get; set; }
        [XmlAttribute("Level")] public int Level { get; set; }
        [XmlAttribute("Speed")] public int Speed { get; set; }
		[XmlAttribute("Name")] public String Name { get; set; }
        [XmlAttribute("GfxName")] public String GfxName { get; set; }
		[XmlAttribute("Tips")] public String Tips { get; set; }
        [XmlAttribute("Weakness")] public DamageType Weakness { get; set; }
        [XmlAttribute("Resist")] public DamageType Resist { get; set; }
        [XmlAttribute("StealAmount")] public int StealAmount { get; set; }
		
		
		public XmlCreepWave()
		{
			Health = 0;
			Level = 0;
			Speed = 0;
			Name = "None";
			Tips = "None";
			Weakness = DamageType.None;
			Resist = DamageType.None;
			StealAmount = 0;
            Numbers = 5;
            GfxName = "default";
		}
		
		public XmlCreepWave(CreepUnit Unit)
		{
			Health = Unit.Health;
			Level = Unit.Level;
			Speed = Unit.Speed;
			Name = Unit.Name;
			Tips = Unit.Tips;
			Weakness = Unit.Weakness;
			Resist = Unit.Weakness;
			StealAmount = Unit.StealAmount;
            Numbers = 5;
            GfxName = "default";
		}

        public XmlCreepWave(CreepUnit Unit,int Numbers)
        {
            Health = Unit.Health;
            Level = Unit.Level;
            Speed = Unit.Speed;
            Name = Unit.Name;
            Tips = Unit.Tips;
            Weakness = Unit.Weakness;
            Resist = Unit.Weakness;
            StealAmount = Unit.StealAmount;
            this.Numbers = Numbers;
            GfxName = "default";
        }
		
		public CreepUnit ToCreepUnit()
		{
			CreepUnit Unit = new CreepUnit(Name,Health,StealAmount);
			Unit.Tips = Tips;
			Unit.Speed = Speed;
			Unit.Level = Level;
			Unit.Weakness = Weakness;
			Unit.Resist = Resist;
			
			return Unit;
			
		}
	}
}
