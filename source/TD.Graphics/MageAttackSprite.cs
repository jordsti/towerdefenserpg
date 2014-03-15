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

    public class MageAttackSurfaces : SurfaceCollection
    {
        public MageAttackSurfaces()
            : base()
        {
            Add(new Surface("../../../../assets/Sprite/mageattack/magefire0.png"));
            Add(new Surface("../../../../assets/Sprite/mageattack/magefire1.png"));
            Add(new Surface("../../../../assets/Sprite/mageattack/magefire2.png"));
            Add(new Surface("../../../../assets/Sprite/mageattack/magefire3.png"));
            Add(new Surface("../../../../assets/Sprite/mageattack/magefire4.png"));
            Add(new Surface("../../../../assets/Sprite/mageattack/magefire5.png"));
            Add(new Surface("../../../../assets/Sprite/mageattack/magefire6.png"));
            Add(new Surface("../../../../assets/Sprite/mageattack/magefire7.png"));
        }
    }

    public class MageAttackSprites
    {
        public MageAttackSurfaces Surfaces { get; set; }
        public List<MageAttackSprite> Sprites { get; set; }

        public MageAttackSprites()
        {
            Surfaces = new MageAttackSurfaces();
            Sprites = new List<MageAttackSprite>();
        }

        public void Add(Point p)
        {
            MageAttackSprite spr = new MageAttackSprite(new Point(p.X,p.Y));
            spr.Surfaces = Surfaces;
            Sprites.Add(spr);
        }

        public void Update(TickEventArgs args)
        {
            List<MageAttackSprite> toRemove = new List<MageAttackSprite>();

            foreach (MageAttackSprite spr in Sprites)
            {
                if (spr.SurfaceIndex != Surfaces.Count)
                {
                    spr.Update(args);
                }
                else
                {
                    toRemove.Add(spr);
                }
            }

            foreach (MageAttackSprite spr in toRemove)
            {
                Sprites.Remove(spr);
            }
        }
    }

    public class MageAttackSprite : Sprite
    {
        public MageAttackSurfaces Surfaces { get; set; }
        public int SurfaceIndex { get; private set; }
        private int Tick = 0;

        public MageAttackSprite()
            : base()
        {
            Visible = false;
            SurfaceIndex = 0;
        }

        public MageAttackSprite(Point Pos)
            : base()
        {
            Visible = true;
            X = Pos.X;
            Y = Pos.Y-50;
            SurfaceIndex = 0;
        }

        public override void Update(TickEventArgs args)
        {
            if (Tick % 2 == 0)
            {
                if (SurfaceIndex == Surfaces.Count)
                {
                    Visible = false;
                    Kill();
                }
                else
                {
                    Surface = Surfaces[SurfaceIndex];
                    SurfaceIndex++;
                }
            }
            Tick++;
        }
    }
}
