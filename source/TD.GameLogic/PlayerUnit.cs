using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TD.GameLogic
{
    public enum UnitClasses { Archer,Mage,Paladin,Soldier,Thieft,None }

    public class PlayerUnit : Unit
    {

        public const int LEVEL_COST = 25;
        public const int LEVEL_INIT = 5;

        public int Strength { get; set; }
        public int Intellect { get; set; }
        public int Luck { get; set; }
        public UnitClasses Class { get; set; }
        public DamageType DmgType { get; set; }
        public CreepUnit CurrentTarget { get; set; }
        public Point Position { get; set; }

        public PlayerUnit(): base(0,0,0)
        {
            Level = 0;
            Strength = 0;
            Intellect = 0;
            Luck = 0;
            DmgType = DamageType.Physical;
            Class = UnitClasses.None;
            Position = new Point();

            CurrentTarget = new EmptyCreepUnit();
        }

        public static PlayerUnit CreateUnit(UnitClasses UnitClass)
        {
            PlayerUnit Unit = new PlayerUnit();

            switch (UnitClass)
            {
                case UnitClasses.Archer: return new Archer();
                case UnitClasses.Mage: return new Mage();
                case UnitClasses.Paladin: return new Paladin();
                case UnitClasses.Soldier: return new Soldier();
                case UnitClasses.Thieft: return new Thieft();
            }

            return Unit;
        }

        public static String GetClassString(UnitClasses UnitClass)
        {
            switch (UnitClass)
            {
                case UnitClasses.Archer: return "Archer";
                case UnitClasses.Mage: return "Mage";
                case UnitClasses.Paladin: return "Paladin";
                case UnitClasses.Soldier: return "Soldier";
                case UnitClasses.Thieft: return "Thieft";
                case UnitClasses.None: return "None";
            }

            return "None";
        }
        public virtual AttackInfo Defend(CreepUnit Target)
        {
            return new AttackInfo();
        }
        public virtual AttackInfoList Defend(CreepUnitList InRange)
        {
            return new AttackInfoList();
        }

        public virtual Projectile ThrowProjectile(CreepUnit Target)
        {
            return new Projectile();
        }

        public virtual ProjectileList ThrowProjectile(CreepUnitList InRange)
        {
            return new ProjectileList();
        }


        public virtual void LevelUp()
        {

        }

        public static int LevelCost(int Level)
        {
            return (LEVEL_COST * Level) + LEVEL_INIT;
        }
    }
}
