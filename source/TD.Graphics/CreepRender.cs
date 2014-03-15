using System;
using System.Collections.Generic;
using System.Collections;
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

    public class CreepRenderer
    {
        public const int HEALTHBAR_WIDTH = 30, HEALTHBAR_HEIGHT = 5;

        public DirectionnalSurfaces Textures { get; set; }
        public Dictionary<CreepUnit, DirectionnalSprite> Sprites;

        public CreepRenderer()
        {
            Sprites = new Dictionary<CreepUnit, DirectionnalSprite>();
            Textures = new DirectionnalSurfaces();

            Textures.Up.Add(new Surface("../../../../assets/Sprite/creep1/Up0.png"));
            Textures.Up.Add(new Surface("../../../../assets/Sprite/creep1/Up1.png"));
            Textures.Up.Add(new Surface("../../../../assets/Sprite/creep1/Up2.png"));
            Textures.Up.Add(new Surface("../../../../assets/Sprite/creep1/Up3.png"));

            Textures.Down.Add(new Surface("../../../../assets/Sprite/creep1/Down0.png"));
            Textures.Down.Add(new Surface("../../../../assets/Sprite/creep1/Down1.png"));
            Textures.Down.Add(new Surface("../../../../assets/Sprite/creep1/Down2.png"));
            Textures.Down.Add(new Surface("../../../../assets/Sprite/creep1/Down3.png"));

            Textures.Left.Add(new Surface("../../../../assets/Sprite/creep1/Left0.png"));
            Textures.Left.Add(new Surface("../../../../assets/Sprite/creep1/Left1.png"));
            Textures.Left.Add(new Surface("../../../../assets/Sprite/creep1/Left2.png"));
            Textures.Left.Add(new Surface("../../../../assets/Sprite/creep1/Left3.png"));

            Textures.Right.Add(new Surface("../../../../assets/Sprite/creep1/Right0.png"));
            Textures.Right.Add(new Surface("../../../../assets/Sprite/creep1/Right1.png"));
            Textures.Right.Add(new Surface("../../../../assets/Sprite/creep1/Right2.png"));
            Textures.Right.Add(new Surface("../../../../assets/Sprite/creep1/Right3.png"));

        }

        public void RemoveCreep(CreepUnit Unit)
        {
            Sprites.Remove(Unit);
        }

        public CreepRenderer(CreepTextureEntry Entry)
        {
            Sprites = new Dictionary<CreepUnit, DirectionnalSprite>();
            Textures = new DirectionnalSurfaces();

            string SpritePath = "../../../../assets/Sprite/";

            foreach(string str in Entry.FrameUp)
            {
                Textures.Up.Add(new Surface(SpritePath + str));
            }

            foreach (string str in Entry.FrameDown)
            {
                Textures.Down.Add(new Surface(SpritePath + str));
            }

            foreach (string str in Entry.FrameLeft)
            {
                Textures.Left.Add(new Surface(SpritePath + str));
            }

            foreach (string str in Entry.FrameRight)
            {
                Textures.Right.Add(new Surface(SpritePath + str));
            }
        }

        public static Surface RenderHealthBar(int TotalHealth, int Health)
        {
            Surface Buffer = new Surface(HEALTHBAR_WIDTH, HEALTHBAR_HEIGHT);
            Buffer.Fill(Color.Red);

            int CurHealthWidth = (Buffer.Width * Health) / TotalHealth;

            Rectangle LifeRect = new Rectangle(new Point(0, 0), new Size(CurHealthWidth, HEALTHBAR_HEIGHT));

            Buffer.Fill(LifeRect, Color.LimeGreen);

            Box Border = new Box(new Point(0, 0), new Size(Buffer.Width - 1, Buffer.Height - 1));

            Buffer.Draw(Border, Color.Black, true);

            return Buffer;
        }

        public void Clean()
        {
            Sprites.Clear();
        }

        public Surface Render(CreepUnitList ToRender, Map map)
        {
            Surface CreepsLayer = new Surface(map.Columns*Tile.TILE_WIDTH,map.Rows*Tile.TILE_HEIGHT);
            CreepsLayer.Fill(Color.Magenta);
            CreepsLayer.TransparentColor = Color.Magenta;
            CreepsLayer.Transparent = true;

            foreach (CreepUnit Unit in ToRender)
            {
                if (!Unit.Position.IsEmpty && Unit.Health > 0)
                {
                    if (!Sprites.ContainsKey(Unit))
                    {
                        Sprites.Add(Unit, new DirectionnalSprite(Textures));
                    }
                    /*Surface Creep = new Surface(CreepUnit.CREEP_WIDTH_PX, CreepUnit.CREEP_HEIGHT_PX);
                    Creep.Fill(Color.Red);
                    Circle c = new Circle(11, 11, 3);

                    Creep.Draw(c, Color.SeaGreen, true);
                    Rectangle Clip = new Rectangle(Unit.Position, Creep.Size);
                    CreepsLayer.Blit(Creep, Clip);*/
                }
                else if (Unit.Health <= 0 || Unit.Position.IsEmpty)
                {
                    if (Sprites.ContainsKey(Unit))
                    {
                        Sprites[Unit].Visible = false;
                        Sprites.Remove(Unit);
                    }
                }
                else if (Unit.Position.X == 0 && Unit.Position.Y == 0 && !Unit.Position.IsEmpty)
                {
                    if (Sprites.ContainsKey(Unit))
                    {
                        Sprites[Unit].Visible = false;
                        Sprites.Remove(Unit);
                    }
                }
            }

            foreach(CreepUnit Unit in Sprites.Keys)
            {
                Sprites[Unit].Direction = Unit.Direction;

                Sprites[Unit].X = Unit.Position.X;
                Sprites[Unit].Y = Unit.Position.Y;

                Sprites[Unit].UpdateFrame();

                if(Sprites[Unit].Position.X != 0 && Sprites[Unit].Position.Y != 0)
                {
                    CreepsLayer.Blit(Sprites[Unit]);
                }

            }

                //Healh Bar Render
                foreach (CreepUnit Unit in ToRender)
                {
                    if (!Unit.Position.IsEmpty && Unit.Health != 0)
                    {
                        if (Unit.Health != 0)
                        {
                            Surface HealthBSur = RenderHealthBar(Unit.TotalHealth, Unit.Health);
                            Point HealthBarP = new Point(Unit.Position.X, Unit.Position.Y - 6);
                            Rectangle Clip = new Rectangle(HealthBarP, HealthBSur.Size);
                            CreepsLayer.Blit(RenderHealthBar(Unit.TotalHealth, Unit.Health), Clip);
                        }
                    }
                }

            return CreepsLayer;

        }

    }
}
