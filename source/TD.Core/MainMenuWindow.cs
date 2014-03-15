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
    public class MainMenuWindow : GuiWindow
    {
        private SdlDotNet.Graphics.Font TitleFont = new SdlDotNet.Graphics.Font("../../../../assets/GameData/NorthwoodHigh.ttf", 72);

        private Surface TitleLabelLine1;
        private Surface TitleLabelLine2;
        private Point pLine1;
        private Point pLine2;

        private BackgroundItem Background;

        private ButtonItem NewGameButton;

        private ButtonItem LoadGameButton;

        private ButtonItem HighscoreButton;

        private ButtonItem MapEditorButton;

        private ButtonItem QuitButton;

        private LabelItem GameVersion;

        public MainMenuWindow(Surface Screen)
            : base("MainMenu", Screen.Width, Screen.Height, Screen)
        {
            TitleLabelLine1 = TitleFont.Render("Tower Defense", Color.White);
            TitleLabelLine2 = TitleFont.Render("RPG", Color.White);

            Background = new BackgroundItem(Color.DarkBlue);
            NewGameButton = new ButtonItem("NewGame", 200, 40, "New Game");
            LoadGameButton = new ButtonItem("LoadGame", 200, 40, "Load Game");
            HighscoreButton = new ButtonItem("Highscore", 200, 40, "High Score");
            MapEditorButton = new ButtonItem("MapEdit", 200, 40, "Map Editor");
            QuitButton = new ButtonItem("Quit", 200, 40, "Quit");
            GameVersion = new LabelItem("Version : r74");
            GameVersion.Foreground = Color.WhiteSmoke;

            PlaceItem();

            FillContainer();
        }

        public void FillContainer()
        {
            Container.Add(Background);
            Container.Add(GameVersion);
            Container.Add(NewGameButton);
            Container.Add(LoadGameButton);
            Container.Add(MapEditorButton);
            Container.Add(HighscoreButton);
            Container.Add(QuitButton);
        }

        public void PlaceItem()
        {
            pLine1 = new Point((Width - TitleLabelLine1.Width) / 2, 10);
            pLine2 = new Point((Width - TitleLabelLine2.Width) / 2, 85);

            Background.Width = Width;
            Background.Height = Height;

            NewGameButton.X = (Width - NewGameButton.Width) / 2;
            NewGameButton.Y = 200;

            LoadGameButton.X = (Width - LoadGameButton.Width) / 2;
            LoadGameButton.Y = 260;

            HighscoreButton.X = (Width - HighscoreButton.Width) / 2;
            HighscoreButton.Y = 320;

            MapEditorButton.X = (Width - MapEditorButton.Width) / 2;
            MapEditorButton.Y = 380;

            QuitButton.X = (Width - QuitButton.Width) / 2;
            QuitButton.Y = 440;

            GameVersion.Y = Height - 30;
            GameVersion.X = 20;

        }

        public override void SetEvents()
        {
            Events.Fps = 30;
            Events.MouseMotion += new EventHandler<MouseMotionEventArgs>(this.MouseMotion);
            Events.MouseButtonDown += new EventHandler<MouseButtonEventArgs>(this.MouseClick);
            Events.VideoResize += new EventHandler<VideoResizeEventArgs>(this.Resize);
            Events.Quit += new EventHandler<QuitEventArgs>(this.Quit);
            Events.Tick += new EventHandler<TickEventArgs>(this.Tick);

        }

        public override void UnsetEvents()
        {
            Events.MouseMotion -= new EventHandler<MouseMotionEventArgs>(this.MouseMotion);
            Events.MouseButtonDown -= new EventHandler<MouseButtonEventArgs>(this.MouseClick);
            Events.VideoResize -= new EventHandler<VideoResizeEventArgs>(this.Resize);
            Events.Quit -= new EventHandler<QuitEventArgs>(this.Quit);
            Events.Tick -= new EventHandler<TickEventArgs>(this.Tick);
        }

        public void Resize(object sender, VideoResizeEventArgs args)
        {
            Width = args.Width;
            Height = args.Height;
            PlaceItem();
            Screen = Video.SetVideoMode(args.Width, args.Height, true);
        }

        public override void MouseClick(object sender, MouseButtonEventArgs args)
        {
            String itemName = GetItemName(args);

            if (itemName == NewGameButton.ButtonName)
            {

                UnsetEvents();
                /*GameWindow GameWin = new GameWindow(Screen);
                GameWin.SetEvents();*/

                NewGameWindow NewGameWin = new NewGameWindow(Screen);
                NewGameWin.SetEvents();

            }
            else if (itemName == MapEditorButton.ButtonName)
            {
                UnsetEvents();

                EditorWindow MapEditWin = new EditorWindow(Screen);
                MapEditWin.SetEvents();

            }
            else if (itemName == QuitButton.ButtonName)
            {
                Events.QuitApplication();
            }
        }

        public override void Tick(object sender, TickEventArgs args)
        {

            CleanGarbage();

            foreach (GuiItem item in Container)
            {
                Surface Buff = RenderItem(item);

                Screen.Blit(Buff, item.GetRect());
            }

            Screen.Blit(TitleLabelLine1, pLine1);
            Screen.Blit(TitleLabelLine2, pLine2);

            Screen.Update();
        }
    }
}
