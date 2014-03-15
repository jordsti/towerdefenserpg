using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TD.GameLogic
{
    public enum DamageType { Physical,Magic,None }

    public enum Direction { Up, Left, Right, Down, Unset }

    public static class UnitDmg
    {
        public static String GetDamageType(DamageType Type)
        {
            if (Type == DamageType.None)
            {
                return "None";
            }
            else if (Type == DamageType.Magic)
            {
                return "Magic";
            }
            else
            {
                return "Physical";
            }
        }
    }

	public class AttackInfoList : List<AttackInfo>
	{
		public AttackInfoList() : base()
		{
			
		}
	}
	
	public class CombatLog 
	{
		public long MagicDamage {get; private set;}
		public long PhysicalDamage {get; private set;}
		public long GoldStolen {get; private set;}
		public int CriticalTimes {get; private set;}
		public long CriticalDamage {get; private set;}
		
		public CombatLog()
		{
			MagicDamage = 0;
			PhysicalDamage = 0;
			GoldStolen = 0;
			CriticalTimes = 0;
			CriticalDamage = 0;
		}
		
		public CombatLog(AttackInfoList List)
		{
			MagicDamage = 0;
			PhysicalDamage = 0;
			GoldStolen = 0;
			CriticalTimes = 0;
			CriticalDamage = 0;
			foreach(AttackInfo Info in List)
			{
				if(Info.Type == DamageType.Magic)
				{
					MagicDamage += Info.Damage;
					if(Info.isCritical)
					{
						CriticalTimes++;
						CriticalDamage += Info.Damage;
					}
				}
				else if(Info.Type == DamageType.Physical && Info.GoldStolen == 0)
				{
					PhysicalDamage += Info.Damage;
					
					if(Info.isCritical)
					{
						CriticalTimes++;
						CriticalDamage += Info.Damage;
					}
				}
				else
				{
					GoldStolen += Info.GoldStolen;
				}
			}
		}
		
	}
	
    public class AttackInfo
    {
        public int Damage { get; set; }
        public DamageType Type { get; set; }
        public bool isCritical { get; set; }
        public int GoldStolen { get; set; }
        public UnitClasses UnitClass { get; set; }
        public int X;
        public int Y;

        public AttackInfo()
        {
            Damage = 0;
            Type = DamageType.None;
            isCritical = false;
            GoldStolen = 0;
            this.UnitClass = UnitClasses.None;
        }

        public AttackInfo(int Damage,DamageType Type)
        {
            this.Damage = Damage;
            this.Type = Type;
            this.isCritical = false;
            this.GoldStolen = 0;
            this.UnitClass = UnitClasses.None;
        }

        public AttackInfo(int Damage, DamageType Type,bool isCritical)
        {
            this.Damage = Damage;
            this.Type = Type;
            this.isCritical = isCritical;
            this.GoldStolen = 0;
            this.UnitClass = UnitClasses.None;
        }

        public AttackInfo(int Damage, DamageType Type, bool isCritical, UnitClasses UnitClass)
        {
            this.Damage = Damage;
            this.Type = Type;
            this.isCritical = isCritical;
            this.GoldStolen = 0;
            this.UnitClass = UnitClass;
        }

        public AttackInfo(int Damage, DamageType Type, bool isCritical,int GoldStealed)
        {
            this.Damage = Damage;
            this.Type = Type;
            this.isCritical = isCritical;
            this.GoldStolen = GoldStealed;
            this.UnitClass = UnitClasses.Thieft;
        }
    }

    public abstract class Unit
    {
        public int Health { get; set; }
        public int Level { get; set; }
        public int Speed { get; set; }
        public int Range { get; set; }
        public int MinusRange { get; set; }

        public Unit()
        {
            Health = 0;
            Level = 0;
            Speed = 0;
            Range = 0;
            MinusRange = 0;
        }

        public Unit(int Health, int Speed, int Range)
        {
            Level = 0;
            this.MinusRange = 0;
            this.Health = Health;
            this.Speed = Speed;
            this.Range = Range;
        }

        public virtual void Attack(Unit Target)
        {
            Random r = new Random();
            int AttackPower = Level * 5;
            int Crit = r.Next(10);

            if(Crit>8)
            {
                AttackPower += 10;
            }
            
            Target.Health -= AttackPower;

        }

    }

    public class NoUnit : Unit
    {
        public NoUnit()
            : base()
        {
            Level = 0;
            Speed = 0;
        }
    }
}
