using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using SdlDotNet.Core;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Primitives;
using SdlDotNet.Input;

using System.Xml;
using System.Xml.Serialization;

namespace TD.Gui
{
    public class MenuBar : GuiItem
    {
        public List<ContextMenu> MenuList { get; set; }

        public MenuBar(int Width) : base("MenuBar")
        {
            MenuList = new List<ContextMenu>();
            Height = 22;
            this.Width = Width;
        }

        public override Surface Render()
        {
            Surface Buffer = RenderBackground();
            int curx=5;
            foreach (ContextMenu m in MenuList)
            {
                Surface Text = DefaultStyle.GetFont().Render(m.MenuName, Color.Black);

                Buffer.Blit(Text, new Point(curx, 4));

                curx += 5;
                curx += Text.Width;
            }

            return Buffer;
        }

        public Surface RenderBackground()
        {
            Surface Buffer = new Surface(Width, Height);

            Buffer.Fill(Color.Coral);

            return Buffer;
        }

        public override Surface RenderHighlight()
        {
            return base.RenderHighlight();
        }

    }

    public class ContextMenu : GuiItem
    {
        public String MenuName { get; set; }
        public List<MenuItem> MenuItems { get; set; }

        public ContextMenu(String MenuName) : base("ContextMenu")
        {
            this.MenuName = MenuName;
            this.MenuItems = new List<MenuItem>();
        }

        public void FixAttribute()
        {
            int CharLength = 1;

            foreach (MenuItem i in MenuItems)
            {
                if (i.Caption.Length > CharLength)
                {
                    CharLength = i.Caption.Length;
                }
            }

            Width = 8 * CharLength;
            Height = 22 * MenuItems.Count;

            foreach (MenuItem i in MenuItems)
            {
                i.Width = Width;
            }

        }

        public void CheckFocus(Point p)
        {
            int i = 0;
            foreach (MenuItem Item in MenuItems)
            {
                Rectangle rect = new Rectangle(new Point(X, Y + (i * 22)), new Size(Width, 22));

                if (rect.Contains(p))
                {
                    Item.Focus = true;
                }
                else
                {
                    Item.Focus = false;
                }
                i++;
            }
        }

        public virtual void MouseClick(object sender, MouseButtonEventArgs args)
        {
            //Console.WriteLine(hasFocus(args.Position));
        }

        public String hasFocus(Point p)
        {
            String ItemName = "None";
            int i=0;
            foreach (MenuItem Item in MenuItems)
            {
                Rectangle rect = new Rectangle(new Point(X, Y + (i * 22)), new Size(Width, 22));
                if (Item.Focus && rect.Contains(p))
                {
                    ItemName = Item.ButtonName;
                    break;
                }
                i++;
            }


            return ItemName;

        }

        public virtual void SetEvents()
        {
            Events.MouseButtonDown += new EventHandler<MouseButtonEventArgs>(this.MouseClick);
            Events.MouseMotion += new EventHandler<MouseMotionEventArgs>(this.MouseMotion);
        }

        public virtual void UnsetEvents()
        {
            Events.MouseMotion -= new EventHandler<MouseMotionEventArgs>(this.MouseMotion);
            Events.MouseButtonDown -= new EventHandler<MouseButtonEventArgs>(this.MouseClick);
        }

        public virtual void MouseMotion(object sender, MouseMotionEventArgs args)
        {
            CheckFocus(new Point(args.X,args.Y));
        }


        public override Surface Render()
        {
            Surface Buffer = new Surface(Width, Height);
            int i = 0;
            foreach(MenuItem Item in MenuItems)
            {
                if (Item.Focus)
                {
                    Buffer.Blit(Item.RenderHighlight(), new Point(0, i * Item.Height));
                }
                else
                {
                    Buffer.Blit(Item.Render(), new Point(0, i * Item.Height));
                }
                i++;
            }

            Line Top = new Line(new Point(0, 0), new Point(Width-1, 0));
            Line Bottom = new Line(new Point(0, Height-1), new Point(Width - 1, Height -1));

            Buffer.Draw(Top, Color.Black);
            Buffer.Draw(Bottom, Color.Black);

            return Buffer;
        }

        public override Surface RenderHighlight()
        {
            throw new NotImplementedException();
        }

    }

    public class MenuItem : GuiItem
    {
        public String ButtonName { get; set; }
        public String Caption { get; set; }
        public Color Foreground { get; set; }
        public Color Background { get; set; }
        public Color HighlightForeground { get; set; }
        public Color HighlightBackground { get; set; }
        public bool Focus = false;

        public MenuItem(String ButtonName, String Caption) : base("MenuItem")
        {
            this.ButtonName = ButtonName;
            this.Caption = Caption;
            this.Height = 22;
            Foreground = DefaultStyle.GetForeground();
            Background = DefaultStyle.GetBackground();
            HighlightForeground = DefaultStyle.GetHighlightForeground();
            HighlightBackground = DefaultStyle.GetHighlightBackground();
        }

        public override Surface Render()
        {
            Surface buffer = new Surface(Width, Height);
            buffer.Fill(Background);

            Surface Label = DefaultStyle.GetFont().Render(Caption, Foreground);

            Point dest = new Point();

            dest.X = (Width - Label.Width)/2;
            dest.Y = (Height - Label.Height) / 2;

            buffer.Blit(Label, dest);

            Line left = new Line(new Point(0, 0), new Point(0, Height));
            Line right = new Line(new Point(Width - 1, 0), new Point(Width - 1, Height));

            left.Draw(buffer, Foreground);
            right.Draw(buffer, Foreground);

            return buffer;
        }

        public override Surface RenderHighlight()
        {
            Surface buffer = new Surface(Width, Height);
            buffer.Fill(HighlightBackground);

            Surface Label = DefaultStyle.GetFont().Render(Caption, HighlightForeground);

            Point dest = new Point();

            dest.X = (Width - Label.Width) / 2;
            dest.Y = (Height - Label.Height) / 2;

            buffer.Blit(Label, dest);

            Line left = new Line(new Point(0, 0), new Point(0, Height));
            Line right = new Line(new Point(Width - 1, 0), new Point(Width - 1, Height));

            left.Draw(buffer, HighlightForeground);
            right.Draw(buffer, HighlightForeground);

            return buffer;
        }
    }
}
