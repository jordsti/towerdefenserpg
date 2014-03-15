using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using SdlDotNet.Core;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Primitives;
using SdlDotNet.Input;

namespace TD.Gui
{
    public class TextInputDialogBox : GuiItem
    {
        public String Title { get; set; }
        public String Caption { get; set; }
        public String TextEntry { get; set; }

        public WindowBar WinBar { get; set; }
        public LabelItem CaptionLabel { get; set; }
        public BackgroundItem Background { get; set; }
        public ButtonItem ButtonOk { get; set; }
        public bool Drag = false;
        public Point Mouse = new Point();

        public TextInputDialogBox(String Title,String Caption) : base("TextInputDialogBox")
        {
            this.Title = Title;
            this.Caption = Caption;

            this.Width = 360;
            this.Height = 150;

            TextEntry = "";

            WinBar = new WindowBar(Caption, Width);
            WinBar.CloseButton = false;
            Background = new BackgroundItem(Color.WhiteSmoke);
            Background.Width = Width;
            Background.Height = Height;

            CaptionLabel = new LabelItem(Caption);

            ButtonOk = new ButtonItem("OK_Dialog", 120, 22, "Ok");
        }

        public TextInputDialogBox(String Title, String Caption,int Width, int Height) : base("TextInputDialogBox")
        {
            this.Title = Title;
            this.Caption = Caption;

            this.Width = Width;
            this.Height = Height;

            TextEntry = "";

            WinBar = new WindowBar(Caption, Width);
            WinBar.CloseButton = false;
            Background = new BackgroundItem(Color.WhiteSmoke);
            Background.Width = Width;
            Background.Height = Height;

            CaptionLabel = new LabelItem(Caption);

            ButtonOk = new ButtonItem("OK_Dialog", 120, 22, "Ok");
        }

        public virtual void SetEvents()
        {
            Events.KeyboardDown += new EventHandler<KeyboardEventArgs>(this.KeyboardDown);
            Events.MouseButtonDown += new EventHandler<MouseButtonEventArgs>(this.MouseClickDown);
            Events.MouseButtonUp += new EventHandler<MouseButtonEventArgs>(this.MouseClickUp);
            Events.MouseMotion += new EventHandler<MouseMotionEventArgs>(this.MouseMotion);
        }

        public virtual void KeyboardDown(object sender, KeyboardEventArgs args)
        {
            String Entry = Keyboard.GetChar(args);

            if (Entry == "backspace" && TextEntry.Length != 0)
            {
                TextEntry = TextEntry.Substring(0, TextEntry.Length - 1);
            }
            else if(Entry != "backspace" )
            {
                TextEntry += Entry;
            }

        }
        public virtual void MouseClickUp(object sender, MouseButtonEventArgs args)
        {
            Drag = false;
        }

        public virtual void MouseClickDown(object sender, MouseButtonEventArgs args)
        {
            Rectangle Bar = new Rectangle(new Point(X, Y), new Size(WinBar.Width, WinBar.Height));

            if (Bar.Contains(args.Position))
            {
                Drag = true;
            }
        }

        public virtual void MouseMotion(object sender, MouseMotionEventArgs args)
        {
            if (Drag)
            {
                int xdiff = Mouse.X - args.X;
                int ydiff = Mouse.Y - args.Y;

                X -= xdiff;
                Y -= ydiff;


                Mouse = args.Position;
            }
            else
            {
                Mouse = args.Position;
            }
        }

        public virtual void UnsetEvents()
        {
            Events.KeyboardUp -= new EventHandler<KeyboardEventArgs>(this.KeyboardDown);
            Events.MouseButtonDown -= new EventHandler<MouseButtonEventArgs>(this.MouseClickDown);
            Events.MouseButtonUp -= new EventHandler<MouseButtonEventArgs>(this.MouseClickUp);
            Events.MouseMotion -= new EventHandler<MouseMotionEventArgs>(this.MouseMotion);
        }

        public Surface RenderStatic()
        {
            Surface Buffer = new Surface(Width, Height);

            Buffer.Blit(Background.Render(), new Point(0, 0));
            Buffer.Blit(WinBar.Render(), new Point(0, 0));

            Buffer.Blit(CaptionLabel.Render(), new Point(16, 40));

            Rectangle TextBox = new Rectangle(new Point(10,80),new Size(340,22));

            Line Top = new Line(new Point(10, 80), new Point(Width - 11, 80));
            Line Bottom = new Line(new Point(10, 105), new Point(Width - 11, 105));
            Line Left = new Line(new Point(10, 80), new Point(10, 105));
            Line Right = new Line(new Point(Width - 11, 80), new Point(Width - 11, 105));

            Surface Text = DefaultStyle.GetFont().Render(TextEntry,Color.Black);

            Buffer.Draw(Top, Color.Black);
            Buffer.Draw(Bottom, Color.Black);
            Buffer.Draw(Left, Color.Black);
            Buffer.Draw(Right, Color.Black);

            Buffer.Blit(Text,new Point(14,83));

            return Buffer;
        }

        public override Surface Render()
        {
            Surface Buffer = RenderStatic();

            Buffer.Blit(ButtonOk.Render(), new Point((Width - 120) / 2, Height - 30));

            return Buffer;
        }

        public override Surface RenderHighlight()
        {
            Surface Buffer = RenderStatic();

            Buffer.Blit(ButtonOk.RenderHighlight(), new Point((Width - 120) / 2, Height - 30));

            return Buffer;
        }

        public Rectangle GetButtonOkRect()
        {
            return new Rectangle(new Point(((Width - 120) / 2) + X, (Height - 30) + Y), new Size(ButtonOk.Width, ButtonOk.Height));
        }
    }
}
