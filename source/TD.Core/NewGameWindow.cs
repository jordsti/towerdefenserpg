using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using SdlDotNet.Core;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Primitives;
using SdlDotNet.Graphics.Sprites;
using SdlDotNet.Input;
using SdlDotNet.Windows;

using TD.GameLogic;
using TD.Graphics;
using TD.Gui;

namespace TD.Core
{

    public class ListItem : GuiItem
    {
        public String Caption { get; set; }
        public int Index { get; set; }

        public ListItem(int Index,String Caption)
            : base("ListItem")
        {
            this.Caption = Caption;
            this.Index = Index;
        }

        public override Surface Render()
        {
            Box outline = new Box(new Point(0, 0), new Size(Width-1, Height-1));

            Surface Buffer = new Surface(Width, Height);

            Buffer.Fill(Color.Blue);

            Buffer.Draw(outline, Color.WhiteSmoke);

            Surface CaptionSur = DefaultStyle.GetFont().Render(Caption, Color.White);

            Buffer.Blit(CaptionSur, new Point( (Width - CaptionSur.Width) / 2, (Height - CaptionSur.Height) / 2) );

            return Buffer;
        }

        public override Surface RenderHighlight()
        {
            Box outline = new Box(new Point(0, 0), new Size(Width-1, Height-1));

            Surface Buffer = new Surface(Width, Height);

            Buffer.Fill(Color.YellowGreen);

            Buffer.Draw(outline, Color.WhiteSmoke);

            Surface CaptionSur = DefaultStyle.GetFont().Render(Caption, Color.White);

            Buffer.Blit(CaptionSur, new Point((Width - CaptionSur.Width) / 2, (Height - CaptionSur.Height) / 2));

            return Buffer;
        }
    }

    public class ListGuiItem : GuiItem
    {
        public List<String> Content { get; set; }
        public int SelectedIndex { get; private set; }
        private List<ListItem> Items;

        public static int ITEM_WIDTH = 280;
        public static int ITEM_HEIGHT = 20;
        public static int SPACING = 4;

        public ListGuiItem()
            : base("ListGuiItem")
        {
            Content = new List<String>();
            SelectedIndex = 0;
            Items = new List<ListItem>();
        }

        public void Fill()
        {
            Items.Clear();

            for (int i = 0; i < Content.Count; i++)
            {
                ListItem item = new ListItem(i, Content[i]);
                item.X = 0;
                item.Y = (i * ITEM_HEIGHT) + (SPACING*i);

                item.Width = ITEM_WIDTH;
                item.Height = ITEM_HEIGHT;

                Items.Add(item);
            }
        }

        public void SetEvents()
        {
            //Events.MouseMotion += new EventHandler<MouseMotionEventArgs>(this.MouseMotion);
            Events.MouseButtonDown += new EventHandler<MouseButtonEventArgs>(this.MouseClick);
        }

        public void UnsetEvents()
        {
            //Events.MouseMotion -= new EventHandler<MouseMotionEventArgs>(this.MouseMotion);
            Events.MouseButtonDown -= new EventHandler<MouseButtonEventArgs>(this.MouseClick);
        }

        public void MouseClick(object sender, MouseButtonEventArgs args)
        {
            foreach (ListItem item in Items)
            {
                if (item.GetRect(GetPoint()).Contains(args.Position))
                {
                    SelectedIndex = item.Index;
                }
            }
        }

        public override Surface Render()
        {
            Surface Buffer = new Surface(Width, Height);

            Box line = new Box(new Point(0, 0), new Size(Width - 1, Height - 1));

            Buffer.Transparent = true;
            Buffer.TransparentColor = Color.Magenta;

            Buffer.Fill(Color.Magenta);

            Buffer.Draw(line, Color.Snow);

            for (int i=0; i<Items.Count; i++)
            {
                Surface ibff;

                if (SelectedIndex == i)
                {
                    ibff = Items[i].RenderHighlight();
                    Buffer.Blit(ibff, Items[i].GetRect());
                }
                else
                {
                    ibff = Items[i].Render();
                    Buffer.Blit(ibff, Items[i].GetRect());
                }
            }

            return Buffer;
        }
    }

    public class NewGameWindow : GuiWindow
    {
        private SdlDotNet.Graphics.Font TitleFont = new SdlDotNet.Graphics.Font("../../../../assets/GameData/NorthwoodHigh.ttf", 38);

        private Surface LabelTitle;
        private Point ptTitle;

        private Surface Separator;
        private Point ptSep;
        private Point ptSep2;
		private Point ptSep3;

        private BackgroundItem Background;

        private LabelItem LabelDifficulty;
        private ButtonItem EasyDiffButton;
        private ButtonItem MediumDiffButton;
        private ButtonItem HardDiffButton;
		
		private ButtonItem MainMenuButton;
		private ButtonItem StartGameButton;
		
		
        private ListGuiItem MapGuiList;

        public GameDifficulty Difficulty;
        public String MapName;

        public List<String> Maps;

        public NewGameWindow(Surface Screen)
            : base("NewGame", Screen.Width, Screen.Height, Screen)
        {
            Difficulty = GameDifficulty.Easy;
            MapName = "firstmap.xml";

            Maps = Map.GetMapsList("../../../../assets/Maps");

            LabelTitle = TitleFont.Render("Tower Defense RPG - New Game", Color.White);
            ptTitle = new Point(10, 10);

            Separator = new Surface(Width,2);

            Separator.Fill(Color.Gray);

            ptSep = new Point(0, (ptTitle.Y + LabelTitle.Height) + 5);
            ptSep2 = new Point(0, (ptTitle.Y + LabelTitle.Height) + 35);


            Background = new BackgroundItem(Color.DarkBlue);
            Background.Width = Width;
            Background.Height = Height;

            LabelDifficulty = new LabelItem("Difficulty : " + GameUtil.DifficultyToString(Difficulty));
            
            LabelDifficulty.X = 10;
            LabelDifficulty.Y = ptSep.Y + 8;
            LabelDifficulty.Foreground = Color.White;

            LabelDifficulty.Render();

            EasyDiffButton = new ButtonItem("EasyDiff", 120, 20, "Easy");
            EasyDiffButton.X = LabelDifficulty.X + LabelDifficulty.Width + 40;
            EasyDiffButton.Y = ptSep.Y + 5;

            MediumDiffButton = new ButtonItem("MediumDiff", 120, 20, "Medium");
            MediumDiffButton.X = LabelDifficulty.X + LabelDifficulty.Width + 170;
            MediumDiffButton.Y = ptSep.Y + 5;

            HardDiffButton = new ButtonItem("HardDiff", 120, 20, "Hard");
            HardDiffButton.X = LabelDifficulty.X + LabelDifficulty.Width + 300;
            HardDiffButton.Y = ptSep.Y + 5;


            MapGuiList = new ListGuiItem();

            MapGuiList.Width = 300;
            MapGuiList.Height = 400;

            MapGuiList.X = (Width - MapGuiList.Width) / 2;
            MapGuiList.Y = ptSep2.Y + 10;

            MapGuiList.Content = Maps;

            MapGuiList.Fill();
			
			
			
			MainMenuButton = new ButtonItem("MainMenu", 120,20, "Back");
			MainMenuButton.X = 10;
			MainMenuButton.Y = Height-MainMenuButton.Height - 10;
     
			StartGameButton = new ButtonItem("StartGame", 120,20, "Start Game");
			StartGameButton.X = Width - StartGameButton.Width - 10;
			StartGameButton.Y = Height - StartGameButton.Height - 10;
			
			ptSep3 = new Point(0,StartGameButton.Y - 5);
			
            Container.Add(Background);
            Container.Add(LabelDifficulty);
            Container.Add(EasyDiffButton);
            Container.Add(MediumDiffButton);
            Container.Add(HardDiffButton);
			Container.Add(StartGameButton);
			Container.Add(MainMenuButton);
        }

        public override void MouseClick(object sender, MouseButtonEventArgs args)
        {
            String ItemName = GetItemName(args);

            if (ItemName == EasyDiffButton.ButtonName)
            {
                Difficulty = GameDifficulty.Easy;
            }
            else if (ItemName == MediumDiffButton.ButtonName)
            {
                Difficulty = GameDifficulty.Medium;
            }
            else if (ItemName == HardDiffButton.ButtonName)
            {
                Difficulty = GameDifficulty.Hard;
            }
			else if(ItemName == MainMenuButton.ButtonName)
			{
				UnsetEvents();
				MainMenuWindow MainMenuWin = new MainMenuWindow(Screen);
				MainMenuWin.SetEvents();
			}
			else if(ItemName == StartGameButton.ButtonName)
			{
				UnsetEvents();
				
				MapName = Maps[MapGuiList.SelectedIndex];
				
				GameWindow GameWin = new GameWindow(Screen, MapName, Difficulty);
				GameWin.SetEvents();
			}
        }

        public override void Tick(object sender, TickEventArgs args)
        {
            CleanGarbage();

            LabelDifficulty.Caption = "Difficulty : " + GameUtil.DifficultyToString(Difficulty);

            foreach (GuiItem item in Container)
            {
                Surface Buff = RenderItem(item);

                Screen.Blit(Buff, item.GetRect());
            }


            Screen.Blit(LabelTitle, ptTitle);
            Screen.Blit(Separator, ptSep);
            Screen.Blit(Separator, ptSep2);

			
			
            Screen.Blit(MapGuiList.Render(), MapGuiList.GetRect());
            
			//Screen.Blit(Separator, ptSep3);
			
			Screen.Update();
        }

        public override void SetEvents()
        {
            Events.Fps = 30;
            Events.MouseMotion += new EventHandler<MouseMotionEventArgs>(this.MouseMotion);
            Events.MouseButtonDown += new EventHandler<MouseButtonEventArgs>(this.MouseClick);
            Events.Quit += new EventHandler<QuitEventArgs>(this.Quit);
            Events.Tick += new EventHandler<TickEventArgs>(this.Tick);

            MapGuiList.SetEvents();
        }

        public override void UnsetEvents()
        {
            Events.MouseMotion -= new EventHandler<MouseMotionEventArgs>(this.MouseMotion);
            Events.MouseButtonDown -= new EventHandler<MouseButtonEventArgs>(this.MouseClick);
            Events.Quit -= new EventHandler<QuitEventArgs>(this.Quit);
            Events.Tick -= new EventHandler<TickEventArgs>(this.Tick);

            MapGuiList.UnsetEvents();
        }
    
    
    }
}
