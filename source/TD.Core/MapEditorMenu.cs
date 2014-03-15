using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using SdlDotNet.Core;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Primitives;
using SdlDotNet.Input;

using TD.Gui;
using TD.Graphics;
using TD.GameLogic;


namespace TD.Core
{
    public class TextureMenu : ContextMenu
    {
        public Tile TileSelected;
        public TextureIndex Index;
        public List<GuiItem> Garbage;
        public TextureMenu(Tile TileSelected, TextureIndex Index ,List<GuiItem> Garbage)
            : base("TileMenu")
        {
            this.Index = Index;
            this.Garbage = Garbage;
            this.TileSelected = TileSelected;

            foreach (TextureEntry Entry in Index.TextureList)
            {
                if (TileSelected.Type == TileType.Normal)
                {
                    if (Entry.Name.StartsWith("Normal"))
                    {
                        MenuItems.Add(new MenuItem(Entry.Name, Entry.Name));
                    }
                }
                else if(TileSelected.Type == TileType.Obstacle)
                {
                    if (Entry.Name.StartsWith("Obstacle"))
                    {
                        MenuItems.Add(new MenuItem(Entry.Name, Entry.Name));
                    }
                }
                else if (TileSelected.Type == TileType.CreepEnd || TileSelected.Type == TileType.CreepStart || TileSelected.Type == TileType.CreepPath)
                {
                    if (Entry.Name.StartsWith("Creep"))
                    {
                        MenuItems.Add(new MenuItem(Entry.Name, Entry.Name));
                    }
                }
            }

            FixAttribute();
        }

        public override void MouseClick(object sender, MouseButtonEventArgs args)
        {
            String TextureName = hasFocus(args.Position);

            if (TextureName != "None")
            {
                TileSelected.Texture = TextureName;
            }

            UnsetEvents();
            Garbage.Add(this);
        }
    }

    public class TileMenu : ContextMenu
    {
        public Tile TileSelected;
        public List<GuiItem> Garbage;
        public TileMenu(Tile TileSelected, List<GuiItem> Garbage)
            : base("TileMenu")
        {
            this.Garbage = Garbage;
            this.TileSelected = TileSelected;
            MenuItems.Add(new MenuItem("CreepStart", "Creep Start"));
            MenuItems.Add(new MenuItem("CreepEnd", "Creep End"));
            MenuItems.Add(new MenuItem("CreepPath", "Creep Path"));
            MenuItems.Add(new MenuItem("Normal", "Normal"));
            MenuItems.Add(new MenuItem("Obstacle", "Obstacle"));

            FixAttribute();
        }

        public override void MouseClick(object sender, MouseButtonEventArgs args)
        {
            String Type = hasFocus(args.Position);

            if (Type == "CreepStart")
            {
                TileSelected.Type = TileType.CreepStart;
            }
            else if (Type == "CreepEnd")
            {
                TileSelected.Type = TileType.CreepEnd;
            }
            else if (Type == "CreepPath")
            {
                TileSelected.Type = TileType.CreepPath;
            }
            else if (Type == "Normal")
            {
                TileSelected.Type = TileType.Normal;
            }
            else if (Type == "Obstacle")
            {
                TileSelected.Type = TileType.Obstacle;
            }

            UnsetEvents();
            Garbage.Add(this);
        }
    }
}
