using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using SdlDotNet.Core;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Primitives;
using SdlDotNet.Input;

namespace TD.Gui
{
    public static class DefaultStyle
    {
        public static SdlDotNet.Graphics.Font DefFont = new SdlDotNet.Graphics.Font("../../../../assets/GameData/VeraSe.ttf", 12);
        public static SdlDotNet.Graphics.Font BoldFont = new SdlDotNet.Graphics.Font("../../../../assets/GameData/VeraSe.ttf", 15);

        public static SdlDotNet.Graphics.Font GetFont()
        {
            return DefFont;
        }

        public static SdlDotNet.Graphics.Font GetBoldFont()
        {
            if (BoldFont.Bold != true)
            {
                BoldFont.Bold = true;
            }
            return BoldFont;
        }

        public static Color GetForeground()
        {
            return Color.Black;
        }

        public static Color GetBackground()
        {
            return Color.WhiteSmoke;
        }

        public static Color GetHighlightForeground()
        {
            return Color.DarkRed;
        }

        public static Color GetHighlightBackground()
        {
            return Color.LightBlue;
        }

    }

    public abstract class GuiItem
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public String Name { get; set; }

        /// <summary>
        /// Create a GuiItem
        /// </summary>
        /// <param name="Name">Name of the Item</param>
        public GuiItem(String Name)
        {
            this.Name = Name;
            Width = 0;
            Height = 0;
        }

        /// <summary>
        /// Create a GuiItem
        /// </summary>
        /// <param name="Name">Name of the Item</param>
        /// <param name="Width">Width (Px)</param>
        /// <param name="Height">Height (Px)</param>
        public GuiItem(String Name,int Width,int Height)
        {
            this.Name = Name;
            this.Width = Width;
            this.Height = Height;
        }

        /// <summary>
        /// Define X and Y from a Point
        /// </summary>
        /// <param name="p">Position</param>
        public virtual void FromPoint(Point p)
        {
            X = p.X;
            Y = p.Y;
        }

        /// <summary>
        /// Get Position of the item
        /// </summary>
        /// <returns>Position</returns>
        public virtual Point GetPoint()
        {
            return new Point(X,Y);
        }

        /// <summary>
        /// Get GuiItem Rectangle
        /// </summary>
        /// <returns>Rectangle</returns>
        public virtual Rectangle GetRect()
        {
            return new Rectangle(GetPoint(),GetSize());
        }

        /// <summary>
        /// Set the Position and Size of the Item
        /// </summary>
        /// <param name="Rect">Rectangle of the item</param>
        public virtual void FromRect(Rectangle Rect)
        {
            FromPoint(Rect.Location);
            FromSize(Rect.Size);
        }

        /// <summary>
        /// Get GuiItem Rectangle and Add a Point
        /// </summary>
        /// <param name="ToAdd">X and Y to Add</param>
        /// <returns>Rectangle</returns>
        public virtual Rectangle GetRect(Point ToAdd)
        {
            Point p = new Point(ToAdd.X+X,ToAdd.Y+Y);
            return new Rectangle(p, GetSize());
        }

        /// <summary>
        /// Get the dimension of the Item
        /// </summary>
        /// <returns>Size of the Item</returns>
        public virtual Size GetSize()
        {
            return new Size(Width,Height);
        }

        /// <summary>
        /// Set the dimension of the Item with a Size
        /// </summary>
        /// <param name="size">Size of the item</param>
        public virtual void FromSize(Size size)
        {
            Width = size.Width;
            Height = size.Height;
        }

        /// <summary>
        /// Render the Item Graphics
        /// </summary>
        /// <returns>Rendered Surface</returns>
        public virtual Surface Render()
        {
            Surface Buffer = new Surface(Width, Height);
            Buffer.Fill(Color.Purple);

            return Buffer;
        }

        /// <summary>
        /// Render Highlighted the Item Graphics
        /// </summary>
        /// <returns>Rendered Surface</returns>
        public virtual Surface RenderHighlight()
        {
            return Render();
        }
    }

    public class Grid : GuiItem
    {
        public int boxWidth { get; set; }
        public int boxHeight { get; set; }
        public Color LineColor;

        public Grid(int boxWidth,int boxHeight) : base("Grid")
        {
            this.boxWidth = boxWidth;
            this.boxHeight = boxHeight;
            LineColor = Color.Black;
        }

        public Rectangle GetGridRect()
        {
            return new Rectangle(new Point(X, Y), new Size(Width, Height));
        }

        public override Surface Render()
        {
            Surface Buffer = new Surface(Width, Height);
            Buffer.Fill(Color.Magenta);
            Buffer.TransparentColor = Color.Magenta;
            Buffer.Transparent = true;
            for (int i = 0; i <= Width; i += boxWidth)
            {
                for (int j = 0; j <= Height; j += boxHeight)
                {
                    Line l = new Line(new Point(0, j), new Point(Width-1, j));
                    Buffer.Draw(l, LineColor);
                }
                Line li = new Line(new Point(i, 0), new Point(i, Height-1));
                Buffer.Draw(li, LineColor);
            }

            Line Right = new Line(new Point(Width - 1, 0), new Point(Width - 1, Height - 1));
            Line Bottom = new Line(new Point(0, Height - 1), new Point(Width - 1, Height - 1));

            Buffer.Draw(Right, LineColor);
            Buffer.Draw(Bottom, LineColor);

            return Buffer;
        }


        public override Surface RenderHighlight()
        {
            throw new NotImplementedException();
        }
    }

    public class BackgroundItem : GuiItem
    {
        public Color Background { get; set;}

        public BackgroundItem(Color Background): base("BackgroundColorItem")
        {
            this.Background = Background;
        }


        public override Surface Render()
        {
            Surface buffer = new Surface(Width, Height);
            buffer.Fill(Background);


            Line Top = new Line(new Point(0, 0), new Point(Width - 1, 0));
            Line Bottom = new Line(new Point(0, Height - 1), new Point(Width - 1, Height - 1));
            Line Left = new Line(new Point(0, 0), new Point(0, Height - 1));
            Line Right = new Line(new Point(Width - 1, 0), new Point(Width - 1, Height - 1));

            Top.Draw(buffer, Color.LightGray);
            Left.Draw(buffer, Color.LightGray);

            Bottom.Draw(buffer, Color.DarkGray);
            Right.Draw(buffer, Color.DarkGray);

            return buffer;
        }

        public override Surface RenderHighlight()
        {
            return Render();
        }

    }

    public class LabelItem : GuiItem 
    {
        public String Caption { get; set; }
        public Color Foreground { get; set; }
        public Color Background { get; set; }
        public Color HighlightBackground { get; set; }
        public Color HighlightForeground { get; set; }

        public LabelItem()
            : base("LabelItem")
        {
            this.Caption = "";
            Foreground = DefaultStyle.GetForeground();
            HighlightForeground = DefaultStyle.GetHighlightForeground();
        }
        
        public LabelItem(String Caption)
            : base("LabelItem")
        {
            this.Caption = Caption;
            Foreground = DefaultStyle.GetForeground();
            HighlightForeground = DefaultStyle.GetHighlightForeground();
        }

        public override Surface Render()
        {
            Surface text = DefaultStyle.GetFont().Render(Caption, Foreground);

            Width = text.Width;
            Height = text.Height;

            return text;
        }

        public override Surface RenderHighlight()
        {
            Surface text = DefaultStyle.GetFont().Render(Caption, HighlightForeground);

            Width = text.Width;
            Height = text.Height;

            return text;
        }

    }

    public class PictureItem : GuiItem
    {
        public String Filename { get; set; }
        public Surface Image;

        public PictureItem(String Filename)
            : base("PictureItem")
        {
            this.Filename = Filename;
            Image = new Surface(Filename);
            Width = Image.Width;
            Height = Image.Height;
        }

        public override Surface Render()
        {
            return Image;
        }

        public override Surface RenderHighlight()
        {
            throw new NotImplementedException();
        }

    }

    public class WindowBar : GuiItem
    {
        public String Caption { get; set; }
        public bool CloseButton { get; set; }

        public WindowBar(String Caption,int Width) : base("WindowBar")
        {
            this.Caption = Caption;
            this.X = 0;
            this.Y = 0;
            this.Height = 22;
            this.Width = Width;
            CloseButton = true;
        }

        public Rectangle GetCloseButtonRect()
        {
            return new Rectangle(new Point(Width-23,2),new Size(18,18));
        }

        public Surface CloseButtonRender()
        {
            Surface Buffer = new Surface(18,18);

            Buffer.Fill(Color.RoyalBlue);

            Line Top = new Line(new Point(0, 0), new Point(18 - 1, 0));
            Line Bottom = new Line(new Point(0, Height - 1), new Point(18 - 1, 18 - 1));
            
            Line Left = new Line(new Point(0, 0), new Point(0, 18 - 1));
            Line Right = new Line(new Point(18 - 1, 0), new Point(18 - 1, 18 - 1));

            Buffer.Draw(Top, Color.LightBlue);
            Buffer.Draw(Bottom, Color.DarkBlue);
            Buffer.Draw(Left, Color.LightBlue);
            Buffer.Draw(Right, Color.DarkBlue);

            Surface Caption = DefaultStyle.GetFont().Render("X", Color.White);

            int x = (Buffer.Width - Caption.Width) / 2;
            int y = (Buffer.Height - Caption.Height) / 2;

            Buffer.Blit(Caption, new Point(x+1, y+1));

            return Buffer;
        }

        public Surface CloseButtonRenderHighlight()
        {
            Surface Buffer = new Surface(18, 18);

            Buffer.Fill(Color.Snow);

            Line Top = new Line(new Point(0, 0), new Point(18 - 1, 0));
            Line Bottom = new Line(new Point(0, 18 - 1), new Point(18 - 1, 18 - 1));
            Line Left = new Line(new Point(0, 0), new Point(0, 18 - 1));
            Line Right = new Line(new Point(18 - 1, 0), new Point(18 - 1, 18 - 1));

            Buffer.Draw(Top, Color.Black);
            Buffer.Draw(Bottom, Color.Black);
            Buffer.Draw(Left, Color.Black);
            Buffer.Draw(Right, Color.Black);

            Surface Caption = DefaultStyle.GetFont().Render("X", Color.Black);

            int x = (Buffer.Width - Caption.Width) / 2;
            int y = (Buffer.Height - Caption.Height) / 2;

            Buffer.Blit(Caption, new Point(x + 1, y + 1));

            return Buffer;
        }

        public override Surface Render()
        {
            Surface Buffer = new Surface(Width, Height);
            Surface CaptionText = DefaultStyle.GetFont().Render(Caption, Color.White);
            int r=30, g=30, b=150;
            for (int i = 0; i < Height; i++)
            {
                Line l = new Line(new Point(0, i), new Point(Width, i));

                l.Draw(Buffer, Color.FromArgb(r, g, b));

                //r++;
                g++;
                b+=2;
            }

            Buffer.Blit(CaptionText,new Point(4,((Height-CaptionText.Height)/2)));

            if (CloseButton)
            {
                Surface XBut = CloseButtonRender();

                Buffer.Blit(XBut, new Point(Width - 23, 2));

            }
           
            return Buffer;
        }

        public override Surface RenderHighlight()
        {
            Surface Buffer = new Surface(Width, Height);
            Surface CaptionText = DefaultStyle.GetFont().Render(Caption, Color.White);
            int r = 30, g = 30, b = 150;
            for (int i = 0; i < Height; i++)
            {
                Line l = new Line(new Point(0, i), new Point(Width, i));

                l.Draw(Buffer, Color.FromArgb(r, g, b));

                //r++;
                g++;
                b+=2;
            }

            Buffer.Blit(CaptionText, new Point(4, ((Height - CaptionText.Height) / 2)));

            if (CloseButton)
            {
                Surface XBut = CloseButtonRenderHighlight();

                Buffer.Blit(XBut, new Point(Width - 23, 2));

            }

            return Buffer;
        }
    }

    public class ButtonItem : GuiItem
    {
        public String Caption { get; set; }
        public String ButtonName { get; set; }
        public Color Foreground { get; set; }
        public Color Background { get; set; }
        public Color HighlightBackground { get; set; }
        public Color HighlightForeground { get; set; }

        public ButtonItem(String ButtonName,int Width,int Height,String Caption) : base("ButtonItem")
        {
            this.ButtonName = ButtonName;
            this.Caption = Caption;
            this.Width = Width;
            this.Height = Height;
            Foreground = DefaultStyle.GetForeground();
            Background = DefaultStyle.GetBackground();
            HighlightBackground = DefaultStyle.GetHighlightBackground();
            HighlightForeground = DefaultStyle.GetHighlightForeground();
            
        }

        public override Surface Render()
        {
            Surface buffer, font;

            font = DefaultStyle.GetFont().Render(Caption, Foreground);

            buffer = new Surface(Width, Height);
            buffer.Fill(Background);

            int PosX = (Width - font.Width) / 2;
            int PosY = (Height - font.Height) / 2;

            buffer.Blit(font, new Point(PosX, PosY));

            Line Top = new Line(new Point(0, 0),new Point(Width-1,0));
            Line Bottom = new Line(new Point(0, Height - 1), new Point(Width - 1, Height - 1));
            Line Left = new Line(new Point(0, 0), new Point(0, Height - 1));
            Line Right = new Line(new Point(Width - 1, 0), new Point(Width - 1, Height - 1));



            Top.Draw(buffer, Color.LightGray);
            Bottom.Draw(buffer, Color.DarkGray);
            Left.Draw(buffer, Color.LightGray);
            Right.Draw(buffer, Color.DarkGray);

            return buffer;
        }

        public override Surface RenderHighlight()
        {
            Surface buffer, font;

            font = DefaultStyle.GetFont().Render(Caption, HighlightForeground);

            buffer = new Surface(Width, Height);
            buffer.Fill(HighlightBackground);

            int PosX = (Width - font.Width) / 2;
            int PosY = (Height - font.Height) / 2;

            buffer.Blit(font, new Point(PosX, PosY));

            Line Top = new Line(new Point(0, 0), new Point(Width - 1, 0));
            Line Bottom = new Line(new Point(0, Height - 1), new Point(Width - 1, Height - 1));
            Line Left = new Line(new Point(0, 0), new Point(0, Height - 1));
            Line Right = new Line(new Point(Width - 1, 0), new Point(Width - 1, Height - 1));
            

            Top.Draw(buffer, HighlightForeground);
            Bottom.Draw(buffer, HighlightForeground);
            Left.Draw(buffer, HighlightForeground);
            Right.Draw(buffer, HighlightForeground);

            return buffer;
        }
    }
}
