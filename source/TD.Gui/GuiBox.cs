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
    public class DialogBox : GuiItem
    {
        public String Caption { get; set; }
        public String Title { get; set; }
        public WindowBar WinBar;
        public BackgroundItem Background;
        public LabelItem CaptionLabel;
        public ButtonItem ButtonOk;
        public bool Drag = false;
        public Point Mouse = new Point();

        public DialogBox(String Title,String Caption): base("DialogBox")
        {
            this.Title = Title;
            this.Caption = Caption;

            this.Width = 360;
            this.Height = 120;

            WinBar = new WindowBar(Caption, Width);
            WinBar.CloseButton = false;
            Background = new BackgroundItem(Color.WhiteSmoke);
            Background.Width = Width;
            Background.Height = Height;

            CaptionLabel = new LabelItem(Caption);

            ButtonOk = new ButtonItem("OK_Dialog", 120, 22, "Ok");
        }

        public DialogBox(String Title, String Caption,int Width,int Height) : base("DialogBox",Width,Height)
        {
            this.Title = Title;
            this.Caption = Caption;

            this.Width = Width;
            this.Height = Height;

            WinBar = new WindowBar(Caption, Width);
            WinBar.CloseButton = false;
            Background = new BackgroundItem(Color.WhiteSmoke);
            Background.Width = Width;
            Background.Height = Height;

            CaptionLabel = new LabelItem(Caption);

            ButtonOk = new ButtonItem("OK_Dialog", 120, 22, "Ok");
        }

        public Surface RenderStatic()
        {
            CaptionLabel.Caption = Caption;
            WinBar.Caption = Title;

            Surface Buffer = new Surface(Width, Height);

            Buffer.Blit(Background.Render(), new Point(0, 0));
            Buffer.Blit(WinBar.Render(), new Point(0, 0));

            Buffer.Blit(CaptionLabel.Render(), new Point(16, 40));

            return Buffer;
        }

        public virtual void MouseClickUp(object sender, MouseButtonEventArgs args)
        {
            Drag = false;
        }

        public virtual void MouseClickDown(object sender, MouseButtonEventArgs args)
        {
            Rectangle Bar = new Rectangle(new Point(X,Y), new Size(WinBar.Width, WinBar.Height));

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


        public virtual void SetEvents()
        {
            Events.MouseButtonDown += new EventHandler<MouseButtonEventArgs>(this.MouseClickDown);
            Events.MouseButtonUp += new EventHandler<MouseButtonEventArgs>(this.MouseClickUp);
            Events.MouseMotion += new EventHandler<MouseMotionEventArgs>(this.MouseMotion);
        }

        public virtual void UnsetEvents()
        {
            Events.MouseButtonDown += new EventHandler<MouseButtonEventArgs>(this.MouseClickDown);
            Events.MouseButtonUp += new EventHandler<MouseButtonEventArgs>(this.MouseClickUp);
            Events.MouseMotion += new EventHandler<MouseMotionEventArgs>(this.MouseMotion);
        }

        public override Surface Render()
        {
            Surface Buffer = RenderStatic();

            Buffer.Blit(ButtonOk.Render(), new Point( (Width-120)/2, Height-30)  );

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
            return new Rectangle(new Point(((Width - 120)/2)+X, (Height - 30)+Y), new Size(ButtonOk.Width,ButtonOk.Height));
        }


    }
}
