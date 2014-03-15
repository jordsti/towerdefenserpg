using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TD.GameLogic
{
    public class Thieft : PlayerUnit
    {
        public Thieft()
            : base()
        {
            Class = UnitClasses.Thieft;
            Speed = 10;
            MinusRange = 0;
            DmgType = DamageType.Physical;

            LevelUp();
        }

        public override void LevelUp()
        {
            Level++;
            Strength = (Level * 3) + 3;
            Luck = (Level * 5) + 4;
            Intellect = (Level * 0) + 1;

            Speed = (Level * 2) + 10;

            Health = (Level * 5) + 5;
			
			if(Level < 5)
			{
            	Range = (Level * 4) + 45;
			}
        }

        public override AttackInfoList Defend(CreepUnitList InRange)
        {
            AttackInfoList infos = new AttackInfoList();
            if (InRange.Count > 3)
            {
                Random rand = new Random();
                int toAtk = rand.Next(2);
                toAtk += 2;

                for (int i = 0; i < toAtk; i++)
                {
                    infos.Add(Defend(InRange[i]));
                }
            }
            else
            {
                for (int i = 0; i < InRange.Count; i++)
                {
                    infos.Add(Defend(InRange[i]));
                }
            }

            return infos;
        }

        public override AttackInfo Defend(CreepUnit Target)
        {
            double AtkAmp = 1;
            double CritAmp = 1;
            double Attack;
            bool isCritical = false;
            int AtkPower = Strength + (Level * 1);

            if (Target.Weakness == DmgType)
            {
                AtkAmp += 0.25;
            }
            if (Target.Resist == DmgType)
            {
                AtkAmp -= 0.25;
            }

            Random Rand = new Random();
            int Crit = Rand.Next(30 + (Level * 2) + Luck);

            if (Crit < Luck)
            {
                isCritical = true;
                CritAmp += Rand.NextDouble();

                if (CritAmp > 1.4)
                {
                    CritAmp = 1.4;
                }
                else if (CritAmp < 1.25)
                {
                    CritAmp = 1.25;
                }
            }

            AtkPower += (AtkPower / 2) + Rand.Next(AtkPower / 2);

            Attack = (double)AtkPower * AtkAmp * CritAmp;

            Target.Health -= (int)Attack;

            AttackInfo info = new AttackInfo((int)Attack, DmgType, isCritical, Class);
            info.X = Target.Position.X;
            info.Y = Target.Position.Y;
            return info;
        }


        public AttackInfoList StealGold(CreepUnitList InRange)
        {
            AttackInfoList infos = new AttackInfoList();
            if (InRange.Count > 3)
            {
                Random rand = new Random();
                int toSteal = rand.Next(2);
                toSteal += 2;

                for (int i = 0; i < toSteal; i++)
                {
                    infos.Add(StealGold(InRange[i]));
                }
            }
            else
            {
                for (int i = 0; i < InRange.Count; i++)
                {
                    infos.Add(StealGold(InRange[i]));
                }
            }

            return infos;
        }

        public AttackInfo StealGold(CreepUnit Target)
        {
            double Miss = 1;
            double StealAmp = 1;
            double CritAmp = 1;
            bool isCritical = false;
            int StealPower = Luck / 3;


            if (Target.Weakness == DmgType)
            {
                StealAmp += 0.35;
            }
            if (Target.Resist == DmgType)
            {
                StealAmp -= 0.35;
            }

            Random Rand = new Random();
            int Crit = Rand.Next(30 + (Level * 2) + Luck);

            if (Crit < Luck)
            {
                isCritical = true;
                CritAmp += Rand.NextDouble();

                if (CritAmp > 1.3)
                {
                    CritAmp = 1.3;
                }
                else if (CritAmp < 1.15)
                {
                    CritAmp = 1.15;
                }
            }

            int MissRand = Rand.Next(Luck);

            if (MissRand > Luck / 2)
            {
                Miss = 0;
            }

            double StealAmount = (double)StealPower * StealAmp * CritAmp * Miss;

            AttackInfo info = new AttackInfo(0, DmgType, isCritical, (int)StealAmount);
            info.X = Target.Position.X;
            info.Y = Target.Position.Y;
            return info;

            
        }
    }

    public class Paladin : PlayerUnit
    {
        public Paladin()
            : base()
        {
            Class = UnitClasses.Paladin;
            Speed = 10;
            DmgType = DamageType.Magic;

            LevelUp();
        }

        public override void LevelUp()
        {
            Level++;
            Strength = (Level * 4) + 2;
            Luck = (Level * 3) + 5;
            Intellect = (Level * 1) + 1;

            Speed = (Level * 2) + 10;

            Health = (Level * 5) + 5;
            
			if(Level < 5)
			{
            	Range = (Level * 4) + 45;
			}
        }


        public override AttackInfoList Defend(CreepUnitList InRange)
        {
            AttackInfoList infos = new AttackInfoList();
            if (InRange.Count > 3)
            {
                Random rand = new Random();
                int toAtk = rand.Next(2);
                toAtk += 2;

                for (int i = 0; i < toAtk; i++)
                {
                    infos.Add(Defend(InRange[i]));
                }
            }
            else
            {
                for (int i = 0; i < InRange.Count; i++)
                {
                    infos.Add(Defend(InRange[i]));
                }
            }

            return infos;
        }

        public override AttackInfo Defend(CreepUnit Target)
        {
            double MagicAmp = 1;
            double CritAmp = 1;
            double Attack;
            bool isCritical = false;
            int MagicPower = Strength + Intellect + (Level * 1);

            if (Target.Weakness == DmgType)
            {
                MagicAmp += 0.25;
            }
            if (Target.Resist == DmgType)
            {
                MagicAmp -= 0.25;
            }

            Random Rand = new Random();
            int Crit = Rand.Next(30 + (Level * 2) + Luck);

            if (Crit < Luck)
            {
                isCritical = true;
                CritAmp += Rand.NextDouble();

                if (CritAmp > 1.4)
                {
                    CritAmp = 1.4;
                }
                else if (CritAmp < 1.25)
                {
                    CritAmp = 1.25;
                }
            }

            MagicPower += (MagicPower / 2) + Rand.Next(MagicPower / 2);

            Attack = (double)MagicPower * MagicAmp * CritAmp;

            Target.Health -= (int)Attack;

            AttackInfo info = new AttackInfo((int)Attack, DmgType, isCritical, Class);
            info.X = Target.Position.X;
            info.Y = Target.Position.Y;
            return info;
        }

    }

    public class Archer : PlayerUnit
    {
        public Archer(): base()
        {
            Class = UnitClasses.Archer;
            Speed = 10;
            MinusRange = 50;
            DmgType = DamageType.Physical;

            LevelUp();
        }

        public override void LevelUp()
        {
            Level++;
            Strength = (Level * 4) + 2;
            Luck = (Level * 3) + 5;
            Intellect = (Level * 1) + 1;

            Speed = (Level * 2) + 10;

            Health = (Level * 5) + 5;
            
			if(Level < 5)
			{
            	Range = (Level * 5) + 95;
			}
        }


        public override ProjectileList ThrowProjectile(CreepUnitList InRange)
        {
            ProjectileList Projectiles = new ProjectileList();
            if (InRange.Count > 3)
            {
                Random rand = new Random();
                int toAtk = rand.Next(2);
                toAtk += 2;

                for (int i = 0; i < toAtk; i++)
                {
                    Projectiles.Add(ThrowProjectile(InRange[i]));
                }
            }
            else
            {
                for (int i = 0; i < InRange.Count; i++)
                {
                    Projectiles.Add(ThrowProjectile(InRange[i]));
                }
            }

            return Projectiles;
        }

        public override Projectile ThrowProjectile(CreepUnit Target)
        {
            double AtkAmp = 1;
            double CritAmp = 1;
            double Attack;
            bool isCritical = false;
            int AtkPower = Strength + Intellect + (Level * 1);

            if (Target.Weakness == DmgType)
            {
                AtkAmp += 0.25;
            }
            if (Target.Resist == DmgType)
            {
                AtkAmp -= 0.25;
            }

            Random Rand = new Random();
            int Crit = Rand.Next(30 + (Level * 2) + Luck);

            if (Crit < Luck)
            {
                isCritical = true;
                CritAmp += Rand.NextDouble();

                if (CritAmp > 1.4)
                {
                    CritAmp = 1.4;
                }
                else if (CritAmp < 1.25)
                {
                    CritAmp = 1.25;
                }
            }

            AtkPower += (AtkPower / 2) + Rand.Next(AtkPower / 2);

            Attack = (double)AtkPower * AtkAmp * CritAmp;

            Projectile pro = new Projectile(Position, Target, 3, (int)Attack, this, isCritical);

            return pro;
        }

        public override AttackInfoList Defend(CreepUnitList InRange)
        {
            AttackInfoList infos = new AttackInfoList();
            if (InRange.Count > 3)
            {
                Random rand = new Random();
                int toAtk = rand.Next(2);
                toAtk += 2;

                for (int i = 0; i < toAtk; i++)
                {
                    infos.Add(Defend(InRange[i]));
                }
            }
            else
            {
                for (int i = 0; i < InRange.Count; i++)
                {
                    infos.Add(Defend(InRange[i]));
                }
            }

            return infos;
        }

        public override AttackInfo Defend(CreepUnit Target)
        {
            double AtkAmp = 1;
            double CritAmp = 1;
            double Attack;
            bool isCritical = false;
            int AtkPower = Strength + Intellect + (Level * 1);

            if (Target.Weakness == DmgType)
            {
                AtkAmp += 0.25;
            }
            if (Target.Resist == DmgType)
            {
                AtkAmp -= 0.25;
            }

            Random Rand = new Random();
            int Crit = Rand.Next(30 + (Level * 2) + Luck);

            if (Crit < Luck)
            {
                isCritical = true;
                CritAmp += Rand.NextDouble();

                if (CritAmp > 1.4)
                {
                    CritAmp = 1.4;
                }
                else if (CritAmp < 1.25)
                {
                    CritAmp = 1.25;
                }
            }

            AtkPower += (AtkPower / 2) + Rand.Next(AtkPower / 2);

            Attack = (double)AtkPower * AtkAmp * CritAmp;

            Target.Health -= (int)Attack;
            AttackInfo info = new AttackInfo((int)Attack, DmgType, isCritical, Class);
            info.X = Target.Position.X;
            info.Y = Target.Position.Y;
            return info;
        }
    }


    public class Soldier : PlayerUnit
    {
        public Soldier() : base()
        {
            Class = UnitClasses.Soldier;
            Speed = 10;
            DmgType = DamageType.Physical;

            LevelUp();
        }

        public override void LevelUp()
        {
            Level++;
            Strength = (Level * 5) + 2;
            Luck = (Level * 3) + 5;
            Intellect = (Level * 0) + 1;

            Speed = (Level * 2) + 10;

            Health = (Level * 5) + 5;
            
			if(Level < 5)
			{
            	Range = (Level * 4) + 45;
			}
        }


        public override AttackInfoList Defend(CreepUnitList InRange)
        {
            AttackInfoList infos = new AttackInfoList();
            if (InRange.Count > 3)
            {
                Random rand = new Random();
                int toAtk = rand.Next(2);
                toAtk += 2;

                for (int i = 0; i < toAtk; i++)
                {
                    infos.Add(Defend(InRange[i]));
                }
            }
            else
            {
                for (int i = 0; i < InRange.Count; i++)
                {
                    infos.Add(Defend(InRange[i]));
                }
            }

            return infos;
        }

        public override AttackInfo Defend(CreepUnit Target)
        {
            double AtkAmp = 1;
            double CritAmp = 1;
            double Attack;
            bool isCritical = false;
            int AtkPower = Strength + (Level * 1);

            if (Target.Weakness == DmgType)
            {
                AtkAmp += 0.25;
            }
            if (Target.Resist == DmgType)
            {
                AtkAmp -= 0.25;
            }

            Random Rand = new Random();
            int Crit = Rand.Next(30+(Level*2)+Luck);

            if (Crit < Luck)
            {
                isCritical = true;
                CritAmp += Rand.NextDouble();

                if (CritAmp > 1.4)
                {
                    CritAmp = 1.4;
                }
                else if (CritAmp < 1.25)
                {
                    CritAmp = 1.25;
                }
            }

            AtkPower +=  (AtkPower / 2) + Rand.Next(AtkPower/2);

            Attack = (double)AtkPower * AtkAmp * CritAmp;

            Target.Health -= (int)Attack;

            AttackInfo info = new AttackInfo((int)Attack, DmgType, isCritical, Class);
            info.X = Target.Position.X;
            info.Y = Target.Position.Y;
            return info;
        }
    }

    public class Mage : PlayerUnit
    {
        public Mage()
            : base()
        {
            Class = UnitClasses.Mage;
            Speed = 10;
            MinusRange = 50;
            DmgType = DamageType.Magic;

            LevelUp();
        }

        public override void LevelUp()
        {
            Level++;
            Strength = (Level * 0) + 1;
            Luck = (Level * 4) + 4;
            Intellect = (Level * 4) + 3;

            Speed = (Level * 2) + 10;

            Health = (Level * 5) + 5;
            
			if(Level < 5)
			{
            	Range = (Level * 5) + 95;
			}
        }


        public override AttackInfoList Defend(CreepUnitList InRange)
        {
            AttackInfoList infos = new AttackInfoList();
            if (InRange.Count > 3)
            {
                Random rand = new Random();
                int toAtk = rand.Next(2);
                toAtk += 2;

                for (int i = 0; i < toAtk; i++)
                {
                    infos.Add(Defend(InRange[i]));
                }
            }
            else
            {
                for (int i = 0; i < InRange.Count; i++)
                {
                    infos.Add(Defend(InRange[i]));
                }
            }

            return infos;
        }

        public override AttackInfo Defend(CreepUnit Target)
        {
            double MagicAmp = 1;
            double CritAmp = 1;
            double Attack;
            bool isCritical = false;
            int MagicPower = Intellect + (Level * 1);

            if (Target.Weakness == DmgType)
            {
                MagicAmp += 0.25;
            }
            if (Target.Resist == DmgType)
            {
                MagicAmp -= 0.25;
            }

            Random Rand = new Random();
            int Crit = Rand.Next(30 + (Level * 2) + Luck);

            if (Crit < Luck)
            {
                isCritical = true;
                CritAmp += Rand.NextDouble();

                if (CritAmp > 1.4)
                {
                    CritAmp = 1.4;
                }
                else if (CritAmp < 1.25)
                {
                    CritAmp = 1.25;
                }
            }

            MagicPower += (MagicPower / 2) + Rand.Next(MagicPower / 2);

            Attack = (double)MagicPower * MagicAmp * CritAmp;

            Target.Health -= (int)Attack;

            AttackInfo info = new AttackInfo((int)Attack, DmgType, isCritical, Class);
            info.X = Target.Position.X;
            info.Y = Target.Position.Y;
            return info;
        }
    }
}
