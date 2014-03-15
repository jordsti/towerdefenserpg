using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TD.GameLogic
{

    public class Projectile
    {
        public Point StartPoint { get; set; }
        public CreepUnit Target { get; set; }
        public PlayerUnit Source { get; set; }
        public bool isCritical { get; set; }

        public int Speed { get; set; }
        public Point Position { get; set; }
        public int Damage { get; set; }
        public DamageType DmgType { get; set; }

        public bool isAlive { get; set; }

        public Projectile()
        {
            StartPoint = new Point();
            Speed = 0;
            Position = new Point();
            Damage = 0;
            DmgType = DamageType.None;
            isCritical = false;
            isAlive = false;
        }

        public Projectile(Point StartPoint, CreepUnit Target, int Speed, int Damage)
        {
            this.StartPoint = StartPoint;
            Position = new Point(StartPoint.X,StartPoint.Y);
            this.Target = Target;
            this.Speed = Speed;
            this.Damage = Damage;
            this.DmgType = DamageType.Physical;
            isCritical = false;
            isAlive = false;
            Source = new PlayerUnit();
        }

        public Projectile(Point StartPoint, CreepUnit Target, int Speed, int Damage,PlayerUnit Source,bool isCritical)
        {
            isAlive = true;
            this.StartPoint = StartPoint;
            Position = new Point(StartPoint.X, StartPoint.Y);
            this.Target = Target;
            this.Speed = Speed;
            this.Damage = Damage;
            this.DmgType = DamageType.Physical;
            this.Source = Source;
            this.isCritical = isCritical;
            isAlive = true;
        }

        public bool EndPosition()
        {
            Point CreepPos = Target.MiddlePosition();
            int diff_x = CreepPos.X - Position.X;
            int diff_y = CreepPos.Y - Position.Y;

            if (Math.Abs(diff_x) <= CreepUnit.CREEP_WIDTH_PX/2 && Math.Abs(diff_y) <= CreepUnit.CREEP_HEIGHT_PX/2)
            {
                return true;
            }
            else if (Target.Health <= 0 || Target.Position.IsEmpty)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void MoveTick()
        {
            Point CreepPos = Target.MiddlePosition();
            int dest_x, dest_y, diff_x, diff_y,nx_x,nx_y;

            nx_x = Position.X;
            nx_y = Position.Y;

            dest_x = CreepPos.X;
            dest_y = CreepPos.Y;

            diff_x = nx_x - dest_x;
            diff_y = nx_y - dest_y;

            if (diff_x < 0 && Math.Abs(diff_x) >= Speed)
            {
                nx_x += Speed;
            }
            else if (diff_x > 0 && Math.Abs(diff_x) >= Speed)
            {
                nx_x -= Speed;
            }
            else
            {
                nx_x = dest_x;
            }

            if (diff_y < 0 && Math.Abs(diff_y) >= Speed)
            {
                nx_y += Speed;
            }
            else if (diff_y > 0 && Math.Abs(diff_y) >= Speed)
            {
                nx_y -= Speed;
            }
            else
            {
                nx_y = dest_y;
            }

            Position = new Point(nx_x, nx_y);

        }

        public AttackInfo Attack()
        {
            if (isAlive)
            {
                isAlive = false;
                Target.Health -= Damage;
                AttackInfo info = new AttackInfo(Damage, DmgType, isCritical, Source.Class);
                info.X = Target.Position.X;
                info.Y = Target.Position.Y;
                return info;
            }
            else
            {
                return new AttackInfo();
            }
        }
    }

    public class ProjectileList : List<Projectile>
    {
        public ProjectileList()
            : base()
        {

        }

        public AttackInfoList Update()
        {
            AttackInfoList AtkInfoList = new AttackInfoList();

            ProjectileList toRemove = new ProjectileList();

            foreach (Projectile pro in this)
            {
                if (pro.isAlive)
                {
                    if (pro.EndPosition())
                    {
                        AtkInfoList.Add(pro.Attack());

                        toRemove.Add(pro);
                    }
                    else
                    {
                        pro.MoveTick();
                    }
                }
                else
                {
                    toRemove.Add(pro);
                }
            }

            foreach (Projectile pro in toRemove)
            {
                Remove(pro);
            }

            return AtkInfoList;
        }
    }
}
