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
    public class GuiWindow
    {
        public String WindowName {get; set;}
        public int Width { get; set; }
        public int Height { get; set; }
        public List<GuiItem> Container { get; set; }
        public List<GuiItem> Garbage { get; set; }

        public Surface Screen { get; set; }
        public Point MousePos = new Point();

        public GuiWindow(String WindowName,int Width,int Height)
        {
            Container = new List<GuiItem>(); 
            Garbage = new List<GuiItem>();
            this.WindowName = WindowName;
            this.Width = Width;
            this.Height = Height;
            //Video.WindowCaption = WindowName;
            //Video.WindowIcon();
            //Screen = Video.SetVideoMode(Width, Height);
        }

        public GuiWindow(String WindowName, int Width, int Height,Surface Screen)
        {
            Container = new List<GuiItem>();
            Garbage = new List<GuiItem>();
            this.WindowName = WindowName;

            this.Screen = Screen;

            if (Width < Screen.Width || Height < Screen.Height)
            {
                this.Width = Screen.Width;
                this.Height = Screen.Height;
            }
            else
            {
                this.Width = Width;
                this.Height = Height;
            }


        }

        public virtual void SetEvents()
        {
            Events.Fps = 30;
            Events.MouseMotion += new EventHandler<MouseMotionEventArgs>(this.MouseMotion);
            Events.MouseButtonDown += new EventHandler<MouseButtonEventArgs>(this.MouseClick);
            Events.Quit += new EventHandler<QuitEventArgs>(this.Quit);
            Events.Tick += new EventHandler<TickEventArgs>(this.Tick);
            Events.Run();
        }

        public virtual void UnsetEvents()
        {
            Events.MouseMotion -= new EventHandler<MouseMotionEventArgs>(this.MouseMotion);
            Events.MouseButtonDown -= new EventHandler<MouseButtonEventArgs>(this.MouseClick);
            Events.Quit -= new EventHandler<QuitEventArgs>(this.Quit);
            Events.Tick -= new EventHandler<TickEventArgs>(this.Tick);
        }

        public virtual void MouseMotion(object sender, MouseMotionEventArgs args)
        {
            MousePos = new Point(args.X, args.Y);
        }

        public Surface ErrorMessage(String Message)
        {
            Surface text = DefaultStyle.GetFont().Render(Message, Color.Red);

            return text;
        }

        public virtual void Tick(object sender, TickEventArgs args)
        {
            Screen.Fill(Color.Black);

            CleanGarbage();

            if (Container.Count == 0)
            {
                Screen.Blit(ErrorMessage("There's no GuiItem Object in Container..."), new Point(10, 10));
            }
            else
            {
                //Window Updating...

                foreach (GuiItem i in Container)
                {
                    Surface RenderedItem = RenderItem(i);

                    Screen.Blit(RenderedItem, new Point(i.X, i.Y));
                }
            }


            Screen.Update();
        }

        public virtual void MouseClick(object sender, MouseButtonEventArgs args)
        {
            if (args.Button == MouseButton.PrimaryButton)
            {
                String ItemName = GetItemName(args);
                //Condition for button
            }
        }

        public virtual void CleanGarbage()
        {
            foreach(GuiItem Item in Garbage)
            {
                Container.Remove(Item);
            }

            Garbage.Clear();
        }

        public String GetItemName(MouseButtonEventArgs args)
        {
            int X = args.X;
            int Y = args.Y;

            String ItemName = "none";

            foreach (GuiItem Item in Container)
            {
                if (Item is ButtonItem)
                {
                    ButtonItem Button = (ButtonItem)Item;
                    if ((X >= Button.X && X <= Button.X + Button.Width) && (Y >= Button.Y && Y <= Button.Y + Button.Height))
                    {
                        ItemName = Button.ButtonName;
                    }
                }
                else if (Item is WindowBar)
                {
                    WindowBar Bar = (WindowBar)Item;
                    if (Bar.GetCloseButtonRect().Contains(new Point(X, Y)))
                    {
                        Events.QuitApplication();
                    }
                }
                else if (Item is DialogBox)
                {
                    DialogBox Box = (DialogBox)Item;
                    
                    if (Box.GetButtonOkRect().Contains(new Point(X, Y)))
                    {
                        Box.UnsetEvents();
                        Garbage.Add(Box);
                    }
                }
                else if (Item is TextInputDialogBox)
                {
                    TextInputDialogBox Box = (TextInputDialogBox)Item;

                    if (Box.GetButtonOkRect().Contains(new Point(X, Y)))
                    {
                        TextInputOk(Box);
                        Garbage.Add(Box);
                    }
                }
            }

            return ItemName;
        }

        public virtual void TextInputOk(TextInputDialogBox Box)
        {
            //Console.WriteLine(Box.TextEntry);
        }

        public Surface RenderItem(GuiItem Item)
        {
            Surface buffer = new Surface(0,0);

            if (Item is LabelItem)
            {
                LabelItem Label = (LabelItem)Item;

                buffer = Label.Render();

            }
            else if (Item is ButtonItem)
            {
                ButtonItem Button = (ButtonItem)Item;

                if ((MousePos.X >= Button.X && MousePos.X <= Button.X + Button.Width) && (MousePos.Y >= Button.Y && MousePos.Y <= Button.Y + Button.Height))
                {
                    buffer = Button.RenderHighlight();
                }
                else
                {
                    buffer = Button.Render();
                }
            }
            else if (Item is MenuItem)
            {
                MenuItem Menu = (MenuItem)Item;

                if ((MousePos.X >= Menu.X && MousePos.X <= Menu.X + Menu.Width) && (MousePos.Y >= Menu.Y && MousePos.Y <= Menu.Y + Menu.Height))
                {
                    buffer = Menu.RenderHighlight();
                }
                else
                {
                    buffer = Menu.Render();
                }

            }
            else if (Item is PictureItem)
            {
                PictureItem Picture = (PictureItem)Item;

                buffer = Picture.Render();
            }
            else if (Item is BackgroundItem)
            {
                BackgroundItem Background = (BackgroundItem)Item;

                buffer = Background.Render();
            }
            else if (Item is WindowBar)
            {
                WindowBar Bar = (WindowBar)Item;

                if (Bar.GetCloseButtonRect().Contains(new Point(MousePos.X, MousePos.Y)))
                {
                    buffer = Bar.RenderHighlight();
                }
                else
                {
                    buffer = Bar.Render();
                }

            }
            else if (Item is Grid)
            {
                Grid g = (Grid)Item;
                buffer = g.Render();
            }
            else if (Item is DialogBox)
            {
                DialogBox box = (DialogBox)Item;

                if (box.GetButtonOkRect().Contains(new Point(MousePos.X, MousePos.Y)))
                {
                    buffer = box.RenderHighlight();
                }
                else
                {
                    buffer = box.Render();
                }
            }
            else if (Item is ContextMenu)
            {
                ContextMenu Menu = (ContextMenu)Item;

                buffer = Menu.Render();
            }
            else if (Item is TextInputDialogBox)
            {
                TextInputDialogBox box = (TextInputDialogBox)Item;

                if (box.GetButtonOkRect().Contains(new Point(MousePos.X, MousePos.Y)))
                {
                    buffer = box.RenderHighlight();
                }
                else
                {
                    buffer = box.Render();
                }
            }
            else
            {
                buffer = CustomItemRender(Item);
            }

            return buffer;
        }

        public virtual Surface CustomItemRender(GuiItem Item)
        {
            Surface buffer = new Surface(Item.Width, Item.Height);
            buffer.Fill(Color.Aqua);

            return buffer;
        }

        public virtual void Quit(object sender, QuitEventArgs args)
        {
            Events.QuitApplication();
        }

        public void AddGuiItem(GuiItem item)
        {
            Container.Add(item);
        }
    }
}
