using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using SdlDotNet.Core;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Primitives;
using SdlDotNet.Graphics.Sprites;

using TD.GameLogic;
using TD.Gui;

namespace TD.Graphics
{
    public class CriticalDmgSprite : TextSprite
    {
        public AttackInfo CriticalAttack { get; set; }
        private ushort Tick=0;

        public CriticalDmgSprite() : base(DefaultStyle.GetBoldFont())
        {
            CriticalAttack = new AttackInfo();
            Visible = false;
            AllowDrag = false;
            AntiAlias = true;
        }

        public CriticalDmgSprite(AttackInfo CriticalAttack)
            : base(DefaultStyle.GetBoldFont())
        {
            this.CriticalAttack = CriticalAttack;
            Visible = true;
            AllowDrag = false;

            Text = CriticalAttack.Damage.ToString();
            AntiAlias = true;

            if (CriticalAttack.Type == DamageType.Magic)
            {
                this.Color = Color.MediumBlue;
            }
            else if (CriticalAttack.Type == DamageType.Physical)
            {
                this.Color = Color.IndianRed;
            }

            X = CriticalAttack.X - 10;
            Y = CriticalAttack.Y - 8;
        }

        public void UpdateSprite()
        {
            Tick++;

            if (Tick % 50 == 0)
            {
                Visible = false;
            }
            else
            {
                if (Tick % 3 == 0)
                {
                    Y--;
                }
            }
        }
    }

    public class GoldStolenSprite : TextSprite
    {
        public AttackInfo CriticalAttack { get; set; }
        private ushort Tick = 0;

        public GoldStolenSprite()
            : base(DefaultStyle.GetBoldFont())
        {
            CriticalAttack = new AttackInfo();
            Visible = false;
            AllowDrag = false;
            AntiAlias = true;
        }

        public GoldStolenSprite(AttackInfo CriticalAttack)
            : base(DefaultStyle.GetBoldFont())
        {
            this.CriticalAttack = CriticalAttack;
            Visible = true;
            AllowDrag = false;

            Text = CriticalAttack.GoldStolen.ToString();
            AntiAlias = true;
            this.Color = Color.Yellow;

            X = CriticalAttack.X - 10;
            Y = CriticalAttack.Y - 8;
        }

        public void UpdateSprite()
        {
            Tick++;

            if (Tick % 50 == 0)
            {
                Visible = false;
            }
            else
            {
                if (Tick % 3 == 0)
                {
                    Y--;
                }
            }
        }
    }

    public class TextSpriteList : List<TextSprite>
    {
        public TextSpriteList() : base()
        {

        }

        public void Update()
        {
            List<TextSprite> toDelete = new List<TextSprite>();
            
            foreach (TextSprite Spr in this)
            {
                if (Spr.Visible == false)
                {
                    toDelete.Add(Spr);
                }
                else
                {
                    if (Spr is CriticalDmgSprite)
                    {
                        ((CriticalDmgSprite)Spr).UpdateSprite();
                    }
                    else if (Spr is GoldStolenSprite)
                    {
                        ((GoldStolenSprite)Spr).UpdateSprite();
                    }
                }
            }

            foreach (TextSprite Spr in toDelete)
            {
                Remove(Spr);
                Spr.Kill();
            }
        }

        public Surface RenderSprites(Size size)
        {
            Update();
            Surface Buffer = new Surface(size);
            Buffer.Transparent = true;
            //Buffer.TransparentColor = Color.Blue;
            //Buffer.Fill(Color.Blue);


            foreach (TextSprite Spr in this)
            {
                if (Spr.Visible)
                {
                    Buffer.Blit(Spr, new Rectangle(Spr.Position,Spr.Size));
                }
            }


            return Buffer;
        }

    }
}
