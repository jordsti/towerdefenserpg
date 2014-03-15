using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

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
    public class GameWindow : GuiWindow
    {

        //Game Object
        private GameObject GameObj;

        //Game Graphics
        private SurfaceDictionary MapTiles;
        private MapRenderer MapRender;
        private CreepRenderer CreepRender;
        private TextSpriteList CritSprites;
        private CreepTextureIndex CreepTextures;
        private MageAttackSprites MageSprites;
        private Surface MapBackground;
        private int LastAttackInfo = 0;
        private String PlayerName = "John Smith";
        private bool AskedPlayerName = false;

        //GUI ITEM

        //Background
        private BackgroundItem Background;
        private BackgroundItem ButtonPanel;
        private BackgroundItem HudPanel;

        //Button
        private ButtonItem CloseButton;
        private ButtonItem PauseButton;
        private ButtonItem StartWaveButton;
        private ButtonItem CombatLogButton;
        private ButtonItem BuyUnitButton;
        private ButtonItem ResetButton;

        //Game Hud
        private LabelItem LabelScore;
        private LabelItem LabelCrystal;
        private LabelItem LabelWave;
        private LabelItem LabelGold;

        //Fps
        private LabelItem LabelFps;

        //Game GUI
        private CombatLogBox LogBox;
        private PlayerUnitInfoBox UnitInfoBox;
        private UnitClassesChooser UnitClassesChooserBox;
        private CreepInfoBox CreepBox;
        private WaveInfoBox WaveBox;
        private TextInputDialogBox PlayerNameDialog;
        private CreepNextWaveBox NextWaveBox;
        private bool isNewWave = true;

        private Point GameStartP;
        private Rectangle GameRectangle;

        private MapCoord SelectedTile;

        public List<GuiItem> TopLevelContainer { get; set; }

		public GameWindow(Surface Screen, String MapName, GameDifficulty Difficulty):base("GameWindow", 900, 700, Screen)
		{
            if (File.Exists("../../../../assets/Maps/" + MapName + ".xml"))
			{
                GameObj = new GameObject("../../../../assets/Maps/" + MapName + ".xml", Difficulty);
                GameObj.LoadMap("../../../../assets/Maps/" + MapName + ".xml");
			}
			else
			{
                GameObj = new GameObject("../../../../assets/Maps/thirdmap.xml", Difficulty);
                GameObj.LoadMap("../../../../assets/Maps/thirdmap.xml");
			}
			
			Init();
			
		}
		
        public GameWindow(Surface Screen)
            : base("GameWindow", 900, 700, Screen)
        {
            //Game Init
            GameObj = new GameObject();
            GameObj.LoadMap("../../../../assets/Maps/thirdmap.xml");

			//Graphics Init
			Init();
			//Gonna be remove from the constructor soon...
            /*TextureIndex Textures = TextureIndex.LoadTextureIndex("Textures/MapTiles.xml");
            CreepTextures = CreepTextureIndex.LoadIndex("GameData/CreepSpriteIndex.xml");
            MapTiles = Textures.LoadTextures();

            GameStartP = new Point(200, 100);

            CritSprites = new TextSpriteList();

            MapRender = new MapRenderer(GameObj.map, MapTiles);
            CreepRender = new CreepRenderer(CreepTextures.GetEntry(GameObj.CurrentWave.GfxName));

            GameRectangle = new Rectangle(GameStartP,new Size(GameObj.map.Columns*Tile.TILE_WIDTH,GameObj.map.Rows*Tile.TILE_HEIGHT));
            SelectedTile = new MapCoord();

            MageSprites = new MageAttackSprites();

            TopLevelContainer = new List<GuiItem>();


            MapBackground = MapRender.Render();

            Background = new BackgroundItem(Color.WhiteSmoke);
            Background.X = 0;
            Background.Y = 0;
            Background.Width = Width;
            Background.Height = Height;

            ButtonPanel = new BackgroundItem(Color.LightGray);
            ButtonPanel.X = 0;
            ButtonPanel.Y = 0;
            ButtonPanel.Width = Width;
            ButtonPanel.Height = 80;

            HudPanel = new BackgroundItem(Color.SteelBlue);
            HudPanel.X = 0;
            HudPanel.Y = 640;
            HudPanel.Width = Width;
            HudPanel.Height = 120;


            CloseButton = new ButtonItem("Close",120,22,"Quit");
            CloseButton.X = 10;
            CloseButton.Y = 20;

            PauseButton = new ButtonItem("Pause", 120, 22, "Pause");
            PauseButton.X = 140;
            PauseButton.Y = 20;

            StartWaveButton = new ButtonItem("StartWave", 120, 22, "Start Wave");
            StartWaveButton.X = 270;
            StartWaveButton.Y = 20;

            CombatLogButton = new ButtonItem("CombatLog", 120, 22, "View Combat Log");
            CombatLogButton.X = 10;
            CombatLogButton.Y = 50;

            BuyUnitButton = new ButtonItem("BuyUnit", 120, 22, "Buy a Unit");
            BuyUnitButton.X = 140;
            BuyUnitButton.Y = 50;

            ResetButton = new ButtonItem("Reset", 120, 22, "Restart");
            ResetButton.X = 270;
            ResetButton.Y = 50;

            LabelScore = new LabelItem("Score : 0");
            LabelScore.X = 10;
            LabelScore.Y = 650;
            LabelScore.Foreground = Color.White;

            LabelWave = new LabelItem("Wave : " + GameObj.Wave);
            LabelWave.X = 350;
            LabelWave.Y = 650;
            LabelWave.Foreground = Color.White;

            LabelCrystal = new LabelItem("Crystal Left : " + GameObj.Crystal);
            LabelCrystal.X = 10;
            LabelCrystal.Y = 680;
            LabelCrystal.Foreground = Color.White;


            LabelGold = new LabelItem("Gold : " + GameObj.Gold);
            LabelGold.X = 350;
            LabelGold.Y = 680;
            LabelGold.Foreground = Color.White;

            LabelFps = new LabelItem("Fps : 30");
            LabelFps.X = Width - 70;
            LabelFps.Y = 4;

            LogBox = new CombatLogBox(GameObj.LastCombatLog, Garbage);

            UnitInfoBox = new PlayerUnitInfoBox(new Point(GameStartP.X + GameRectangle.Width + 10,GameStartP.Y));

            CreepBox = new CreepInfoBox();
            CreepBox.FromPoint(new Point(GameStartP.X + GameRectangle.Width + 10, GameStartP.Y + UnitInfoBox.Height + 10));

            UnitClassesChooserBox = new UnitClassesChooser(Garbage);
            UnitClassesChooserBox.X = GameStartP.X + ((GameRectangle.Width - UnitClassesChooserBox.Width) / 2); ;
            UnitClassesChooserBox.Y = GameStartP.Y + ((GameRectangle.Height - UnitClassesChooserBox.Height) / 2); ;

            WaveBox = new WaveInfoBox(GameObj.GetNextWaveInfo(),Garbage);
            WaveBox.X = GameStartP.X + ((GameRectangle.Width - WaveBox.Width) / 2);
            WaveBox.Y = GameStartP.Y + ((GameRectangle.Height - WaveBox.Height) / 2);

            NextWaveBox = new CreepNextWaveBox(GameObj.CreepWaves);
            NextWaveBox.X = 10;
            NextWaveBox.Y = GameStartP.Y;

            PlayerNameDialog = new TextInputDialogBox("Highscore Entry", "Player Name : ");
            PlayerNameDialog.X = (Width - PlayerNameDialog.Width) / 2;
            PlayerNameDialog.Y = (Height - PlayerNameDialog.Height) / 2;
            PlayerNameDialog.TextEntry = PlayerName;

            Container.Add(Background);
            Container.Add(ButtonPanel);
            Container.Add(HudPanel);

            Container.Add(CloseButton);
            Container.Add(PauseButton);
            Container.Add(StartWaveButton);
            Container.Add(CombatLogButton);
            Container.Add(BuyUnitButton);
            Container.Add(ResetButton);

            Container.Add(LabelScore);
            Container.Add(LabelCrystal);
            Container.Add(LabelWave);
            Container.Add(LabelGold);

            Container.Add(LabelFps);

            Container.Add(NextWaveBox);
            Container.Add(UnitInfoBox);
            Container.Add(CreepBox);*/

        }
		
		public void Init()
		{
            TextureIndex Textures = TextureIndex.LoadTextureIndex("../../../../assets/Textures/MapTiles.xml");
            CreepTextures = CreepTextureIndex.LoadIndex("../../../../assets/GameData/CreepSpriteIndex.xml");
            MapTiles = Textures.LoadTextures();

            GameStartP = new Point(200, 100);

            CritSprites = new TextSpriteList();

            MapRender = new MapRenderer(GameObj.map, MapTiles);
            CreepRender = new CreepRenderer(CreepTextures.GetEntry(GameObj.CurrentWave.GfxName));

            GameRectangle = new Rectangle(GameStartP,new Size(GameObj.map.Columns*Tile.TILE_WIDTH,GameObj.map.Rows*Tile.TILE_HEIGHT));
            SelectedTile = new MapCoord();

            MageSprites = new MageAttackSprites();

            TopLevelContainer = new List<GuiItem>();


            MapBackground = MapRender.Render();

            Background = new BackgroundItem(Color.WhiteSmoke);
            Background.X = 0;
            Background.Y = 0;
            Background.Width = Width;
            Background.Height = Height;

            ButtonPanel = new BackgroundItem(Color.LightGray);
            ButtonPanel.X = 0;
            ButtonPanel.Y = 0;
            ButtonPanel.Width = Width;
            ButtonPanel.Height = 80;

            HudPanel = new BackgroundItem(Color.SteelBlue);
            HudPanel.X = 0;
            HudPanel.Y = 640;
            HudPanel.Width = Width;
            HudPanel.Height = 120;


            CloseButton = new ButtonItem("Close",120,22,"Quit");
            CloseButton.X = 10;
            CloseButton.Y = 20;

            PauseButton = new ButtonItem("Pause", 120, 22, "Pause");
            PauseButton.X = 140;
            PauseButton.Y = 20;

            StartWaveButton = new ButtonItem("StartWave", 120, 22, "Start Wave");
            StartWaveButton.X = 270;
            StartWaveButton.Y = 20;

            CombatLogButton = new ButtonItem("CombatLog", 120, 22, "View Combat Log");
            CombatLogButton.X = 10;
            CombatLogButton.Y = 50;

            BuyUnitButton = new ButtonItem("BuyUnit", 120, 22, "Buy a Unit");
            BuyUnitButton.X = 140;
            BuyUnitButton.Y = 50;

            ResetButton = new ButtonItem("Reset", 120, 22, "Restart");
            ResetButton.X = 270;
            ResetButton.Y = 50;

            LabelScore = new LabelItem("Score : 0");
            LabelScore.X = 10;
            LabelScore.Y = 650;
            LabelScore.Foreground = Color.White;

            LabelWave = new LabelItem("Wave : " + GameObj.Wave);
            LabelWave.X = 350;
            LabelWave.Y = 650;
            LabelWave.Foreground = Color.White;

            LabelCrystal = new LabelItem("Crystal Left : " + GameObj.Crystal);
            LabelCrystal.X = 10;
            LabelCrystal.Y = 680;
            LabelCrystal.Foreground = Color.White;


            LabelGold = new LabelItem("Gold : " + GameObj.Gold);
            LabelGold.X = 350;
            LabelGold.Y = 680;
            LabelGold.Foreground = Color.White;

            LabelFps = new LabelItem("Fps : 30");
            LabelFps.X = Width - 70;
            LabelFps.Y = 4;

            LogBox = new CombatLogBox(GameObj.LastCombatLog, Garbage);

            UnitInfoBox = new PlayerUnitInfoBox(new Point(GameStartP.X + GameRectangle.Width + 10,GameStartP.Y));

            CreepBox = new CreepInfoBox();
            CreepBox.FromPoint(new Point(GameStartP.X + GameRectangle.Width + 10, GameStartP.Y + UnitInfoBox.Height + 10));

            UnitClassesChooserBox = new UnitClassesChooser(Garbage);
            UnitClassesChooserBox.X = GameStartP.X + ((GameRectangle.Width - UnitClassesChooserBox.Width) / 2); ;
            UnitClassesChooserBox.Y = GameStartP.Y + ((GameRectangle.Height - UnitClassesChooserBox.Height) / 2); ;

            WaveBox = new WaveInfoBox(GameObj.GetNextWaveInfo(),Garbage);
            WaveBox.X = GameStartP.X + ((GameRectangle.Width - WaveBox.Width) / 2);
            WaveBox.Y = GameStartP.Y + ((GameRectangle.Height - WaveBox.Height) / 2);

            NextWaveBox = new CreepNextWaveBox(GameObj.CreepWaves);
            NextWaveBox.X = 10;
            NextWaveBox.Y = GameStartP.Y;

            PlayerNameDialog = new TextInputDialogBox("Highscore Entry", "Player Name : ");
            PlayerNameDialog.X = (Width - PlayerNameDialog.Width) / 2;
            PlayerNameDialog.Y = (Height - PlayerNameDialog.Height) / 2;
            PlayerNameDialog.TextEntry = PlayerName;

            Container.Add(Background);
            Container.Add(ButtonPanel);
            Container.Add(HudPanel);

            Container.Add(CloseButton);
            Container.Add(PauseButton);
            Container.Add(StartWaveButton);
            Container.Add(CombatLogButton);
            Container.Add(BuyUnitButton);
            Container.Add(ResetButton);

            Container.Add(LabelScore);
            Container.Add(LabelCrystal);
            Container.Add(LabelWave);
            Container.Add(LabelGold);

            Container.Add(LabelFps);

            Container.Add(NextWaveBox);
            Container.Add(UnitInfoBox);
            Container.Add(CreepBox);
			
		}

        public override void SetEvents()
        {
            Events.Fps = 30;
            Events.MouseMotion += new EventHandler<MouseMotionEventArgs>(this.MouseMotion);
            Events.MouseButtonDown += new EventHandler<MouseButtonEventArgs>(this.MouseClick);
            Events.Quit += new EventHandler<QuitEventArgs>(this.Quit);
            Events.Tick += new EventHandler<TickEventArgs>(this.Tick);
        }

        public override void UnsetEvents()
        {
            Events.MouseMotion -= new EventHandler<MouseMotionEventArgs>(this.MouseMotion);
            Events.MouseButtonDown -= new EventHandler<MouseButtonEventArgs>(this.MouseClick);
            Events.Quit -= new EventHandler<QuitEventArgs>(this.Quit);
            Events.Tick -= new EventHandler<TickEventArgs>(this.Tick);
        }

        public MapCoord GetCoordFromPoint(Point p)
        {
            int rel_x = p.X - GameRectangle.X;
            int rel_y = p.Y - GameRectangle.Y;

            int col = rel_x / Tile.TILE_WIDTH;
            int row = rel_y / Tile.TILE_HEIGHT;

            return new MapCoord(row, col);
        }

        public override void MouseClick(object sender, MouseButtonEventArgs args)
        {
            if (args.Button == MouseButton.PrimaryButton)
            {
                String ItemName = GetItemName(args);
                if (ItemName == CloseButton.ButtonName)
                {
                    //Events.QuitApplication();
                    MainMenuWindow mainWindow = new MainMenuWindow(Screen);

                    UnsetEvents();
                    mainWindow.SetEvents();

                }
                else if (ItemName == StartWaveButton.ButtonName && TopLevelContainer.Count == 0)
                {
                    if (GameObj.State == GameState.NoAttack)
                    {
                        GameObj.StartWave();
                        SelectedTile = new MapCoord();
                    }
                }
                else if (ItemName == ResetButton.ButtonName)
                {
                    GameObj.NewGame(GameObj.Difficulty);
                    LastAttackInfo = 0;
                    AskedPlayerName = false;
                }
                else if (ItemName == CombatLogButton.ButtonName)
                {
                    if (GameObj.State == GameState.NoAttack && !TopLevelContainer.Contains(LogBox))
                    {
                        LogBox.Y = GameStartP.Y;
                        LogBox.X = (Width - LogBox.Width) / 2;
                        LogBox.Log = GameObj.LastCombatLog;
                        LogBox.UpdateLabel();
                        TopLevelContainer.Add(LogBox);
                        LogBox.SetEvents();
                    }
                }
                else if (ItemName == BuyUnitButton.ButtonName && TopLevelContainer.Count == 0)
                {
                    if (GameObj.State == GameState.NoAttack)
                    {
                        TopLevelContainer.Add(UnitClassesChooserBox);
                        UnitClassesChooserBox.SetEvents();
                    }
                }
                else if (TopLevelContainer.Contains(PlayerNameDialog) && PlayerNameDialog.GetButtonOkRect().Contains(args.Position))
                {
                    PlayerName = PlayerNameDialog.TextEntry;
                    PlayerNameDialog.UnsetEvents();
                    TopLevelContainer.Remove(PlayerNameDialog);
                }
                else if (GameObj.State == GameState.NoAttack && UnitClassesChooserBox.ChoosenClass != UnitClasses.None && TopLevelContainer.Count == 0)
                {
                    if (GameRectangle.Contains(MousePos))
                    {
                        Point relpoint = new Point(MousePos.X - GameStartP.X, MousePos.Y - GameStartP.Y);

                        MapCoord Coord = new MapCoord(relpoint);

                        GameObj.AddPlayerUnit(Coord, UnitClassesChooserBox.ChoosenClass);

                        UnitClassesChooserBox.Reset();

                        UnitInfoBox.ChangeUnit(GameObj.PlayerUnits.GetUnitAt(Coord));
                    }
                }
                else if (UnitInfoBox.GetIncLvlRect().Contains(args.Position) && GameObj.State == GameState.NoAttack && TopLevelContainer.Count == 0)
                {
                    GameObj.IncreaseUnitLevel(UnitInfoBox.Unit);
                }
                else if (GameRectangle.Contains(args.Position) && TopLevelContainer.Count == 0)
                {
                    SelectedTile = GetCoordFromPoint(args.Position);

                    if (GameObj.PlayerUnits.GetUnitAt(SelectedTile).Class != UnitClasses.None)
                    {
                        UnitInfoBox.ChangeUnit(GameObj.PlayerUnits.GetUnitAt(SelectedTile));
                    }
                    else
                    {
                        UnitInfoBox.ChangeUnit(new PlayerUnit());
                    }

                    if (GameObj.State == GameState.CreepAttack)
                    {
                        foreach (CreepUnit Unit in GameObj.Creeps)
                        {
                            if (!Unit.Position.IsEmpty)
                            {
                                Rectangle rect = new Rectangle(new Point(GameStartP.X + Unit.Position.X, GameStartP.Y + Unit.Position.Y), new Size(CreepUnit.CREEP_WIDTH_PX, CreepUnit.CREEP_HEIGHT_PX));

                                if (rect.Contains(args.Position))
                                {
                                    CreepBox.ChangeUnit(Unit);
                                }

                            }
                        }
                    }

                }
                else if (TopLevelContainer.Contains(WaveBox))
                {
                    Garbage.Add(WaveBox);
                }
                //Removed
                /*else if (GameRectangle.Contains(args.Position) && GameObj.State == GameState.NoAttack && TopLevelContainer.Count == 0 && SelectedTile.SameCoord(GetCoordFromPoint(args.Position)))
                {
                    if (GameObj.PlayerUnits.ContainsCoord(SelectedTile) && GameObj.PlayerUnits.GetUnitAt(SelectedTile).Class != UnitClasses.None)
                    {
                        PlayerUnitMenu m = new PlayerUnitMenu(GameObj, SelectedTile, Garbage);
                        m.X += GameStartP.X;
                        m.Y += GameStartP.Y;
                        TopLevelContainer.Add(m);
                        m.SetEvents();
                    }
                    else if (GameObj.map.Tiles[SelectedTile.Row][SelectedTile.Column].Type == TileType.Normal)
                    {
                        EmptyTileMenu m = new EmptyTileMenu(GameObj, SelectedTile, TopLevelContainer, Garbage);
                        m.X += GameStartP.X;
                        m.Y += GameStartP.Y;
                        TopLevelContainer.Add(m);
                        m.SetEvents();
                    }
                }*/
            }
        }

        public override void CleanGarbage()
        {


            foreach (GuiItem Item in Garbage)
            {
                Container.Remove(Item);
                TopLevelContainer.Remove(Item);
            }

            Garbage.Clear();
        }

        public void DrawUnitRange(Surface GameSurface, MapCoord Coord, PlayerUnit Unit)
        {
            Circle MaxRange = new Circle(Coord.ToPointMiddle(), (short)Unit.Range);
            GameSurface.Draw(MaxRange, Color.Aqua, true);

            if (Unit.MinusRange != 0)
            {
                Circle MinusRange = new Circle(Coord.ToPointMiddle(), (short)Unit.MinusRange);
                GameSurface.Draw(MinusRange, Color.Red, true);
            }
        }

        public void UpdateAttackSpriteCollection()
        {
            for (int i = LastAttackInfo; i < GameObj.AttackInfos.Count; i++)
            {
                LastAttackInfo++;

                if (GameObj.AttackInfos[i].isCritical && GameObj.AttackInfos[i].GoldStolen == 0)
                {
                    CritSprites.Add(new CriticalDmgSprite(GameObj.AttackInfos[i]));
                }
                else if (GameObj.AttackInfos[i].GoldStolen != 0)
                {
                    CritSprites.Add(new GoldStolenSprite(GameObj.AttackInfos[i]));
                }

                if (GameObj.AttackInfos[i].UnitClass == UnitClasses.Mage)
                {
                    MageSprites.Add(new Point(GameObj.AttackInfos[i].X, GameObj.AttackInfos[i].Y));
                }
            }
            CritSprites.Update();
        }

        public override void Tick(object sender, TickEventArgs args)
        {
            //Game Logic Tick
            GameObj.GameTick();


            //HUD Update
            LabelScore.Caption = "Score : " + GameObj.Score;
            LabelWave.Caption = "Wave : " + GameObj.Wave;
            LabelCrystal.Caption = "Crystals Left : " + GameObj.Crystal;
            LabelGold.Caption = "Gold : " + GameObj.Gold;

            LabelFps.Caption = "Fps : " + args.Fps;

            NextWaveBox.CurrentWave = GameObj.Wave - 1;

            CleanGarbage();

            //Normal GUI Render
            foreach (GuiItem Item in Container)
            {
                Surface RenderedItem = RenderItem(Item);

                Screen.Blit(RenderedItem, Item.GetRect());
            }

            //Game Graphics Render
            //Surface GameSurface = MapRender.Render();
            Surface GameSurface = new Surface(MapBackground);
            //GameSurface.Blit(MapBackground, new Point(0, 0));
            //Surface GameSurface = new Surface(MapRenderer.MAP_WIDTH_PX, MapRenderer.MAP_HEIGHT_PX);
            if (GameObj.State == GameState.CreepAttack)
            {
                isNewWave = true;
                Surface CreepsLayer = CreepRender.Render(GameObj.Creeps,GameObj.map);
                GameSurface.Blit(CreepsLayer, new Point(0, 0));
            }
            else if (GameObj.State == GameState.NoAttack)
            {
                if (isNewWave)
                {
                    LastAttackInfo = 0;
                    isNewWave = false;
                    WaveBox.Wave = GameObj.GetNextWaveInfo();
                    WaveBox.UpdateCaption();

                    CreepRender.Clean();
                    CreepRender = new CreepRenderer(CreepTextures.GetEntry(GameObj.CurrentWave.GfxName));

                    TopLevelContainer.Add(WaveBox);
                }

                if (UnitClassesChooserBox.ChoosenClass != UnitClasses.None)
                {
                    if (GameRectangle.Contains(MousePos))
                    {
                        Point relpoint = new Point(MousePos.X - GameStartP.X, MousePos.Y - GameStartP.Y);

                        MapCoord Coord = new MapCoord(relpoint);

                        PlayerUnit tmpUnit = PlayerUnit.CreateUnit(UnitClassesChooserBox.ChoosenClass);
                        UnitInfoBox.ChangeUnit(tmpUnit);
                        Surface UnitGfx = PlayerUnitRenderer.Render(tmpUnit);


                        if (GameObj.map.Tiles[Coord.Row][Coord.Column].Type == TileType.Normal)
                        {
                            GameSurface.Blit(UnitGfx, new Rectangle(new Point(Coord.ToPoint().X + (Tile.TILE_WIDTH - UnitGfx.Width) / 2, Coord.ToPoint().Y + (Tile.TILE_HEIGHT - UnitGfx.Height) / 2), new Size(UnitGfx.Width, UnitGfx.Height)));

                            DrawUnitRange(GameSurface, Coord, tmpUnit);
                        }
                    }
                }
            }
            else if (GameObj.State == GameState.GameOver && TopLevelContainer.Count == 0)
            {
                if (!AskedPlayerName)
                {
                    PlayerNameDialog.TextEntry = PlayerName;

                    TopLevelContainer.Add(PlayerNameDialog);
                    PlayerNameDialog.SetEvents();
                    AskedPlayerName = true;
                }
                else
                {
                    GameOverBox box = new GameOverBox(GameObj, Garbage);
                    box.SetEvents();
                    box.X = (Width - box.Width) / 2;
                    box.Y = (Height - box.Height) / 2;
                    TopLevelContainer.Add(box);

                    //Saving Score , must ask for name before
                    GameObj.SaveScore(PlayerName);
                    LastAttackInfo = 0;
                    AskedPlayerName = false;
                }
            }
            else if (GameObj.State == GameState.Complete && TopLevelContainer.Count == 0)
            {
                if (!AskedPlayerName)
                {
                    PlayerNameDialog.TextEntry = PlayerName;

                    TopLevelContainer.Add(PlayerNameDialog);
                    PlayerNameDialog.SetEvents();
                    AskedPlayerName = true;
                }
                else
                {
                    GameCompleteBox box = new GameCompleteBox(GameObj, Garbage);
                    box.SetEvents();
                    box.X = (Width - box.Width) / 2;
                    box.Y = (Height - box.Height) / 2;
                    TopLevelContainer.Add(box);

                    //Saving Score , must ask for name before
                    GameObj.SaveScore(PlayerName);
                    LastAttackInfo = 0;
                    AskedPlayerName = false;
                }
            }
			
			foreach (MapCoord Coord in GameObj.PlayerUnits.Keys)
            {
                Surface Unit = PlayerUnitRenderer.Render(GameObj.PlayerUnits[Coord]);
                Point Dest = new Point(Coord.ToPoint().X + ((Tile.TILE_WIDTH - Unit.Width) / 2), Coord.ToPoint().Y + ((Tile.TILE_HEIGHT - Unit.Height) / 2));
                Rectangle Clip = new Rectangle(Dest, Unit.Size);
                GameSurface.Blit(Unit, Clip);

                if (!SelectedTile.isEmpty && Coord.SameCoord(SelectedTile))
                {
                    DrawUnitRange(GameSurface, Coord, GameObj.PlayerUnits[Coord]);
                }

            }

            if (!SelectedTile.isEmpty && GameObj.map.Tiles[SelectedTile.Row][SelectedTile.Column].Type == TileType.Normal)
            {
                Surface Sqr = new Surface(Tile.TILE_WIDTH, Tile.TILE_HEIGHT);
                Sqr.Alpha = 100;
                Sqr.AlphaBlending = true;
                Sqr.Fill(Color.LightBlue);

                Box Border = new Box(new Point(0, 0), new Size(Sqr.Width-1, Sqr.Height-1));
                Sqr.Draw(Border, Color.Black);

                Rectangle Clip = new Rectangle(SelectedTile.ToPoint(), new Size(Tile.TILE_WIDTH, Tile.TILE_HEIGHT));

                GameSurface.Blit(Sqr, Clip);
            }

            UpdateAttackSpriteCollection();

            MageSprites.Update(args);

            if (GameObj.State == GameState.CreepAttack)
            {
                foreach (TextSprite Spr in CritSprites)
                {
                    GameSurface.Blit(Spr, new Rectangle(Spr.Position, Spr.Size));
                
                }

                foreach (MageAttackSprite Spr in MageSprites.Sprites)
                {
                    GameSurface.Blit(Spr);
                }

                foreach (Projectile pro in GameObj.Projectiles)
                {
                    Circle c = new Circle(pro.Position, 3);
                    Circle d = new Circle(pro.Position, 2);
                    GameSurface.Draw(c, Color.Blue);
                    GameSurface.Draw(d, Color.BlueViolet);
                }
            }

            
            Screen.Blit(GameSurface, GameRectangle);


            //Top Level gui Render (Menu to Add Tower and Upgrade Tower)
            foreach (GuiItem Item in TopLevelContainer)
            {
                Surface RenderedItem = RenderItem(Item);

                Screen.Blit(RenderedItem, Item.GetRect());
            }


            Screen.Update();
        }

        public override Surface CustomItemRender(GuiItem Item)
        {
            Surface Buffer;

            Buffer = Item.Render();

            return Buffer;
        }

    }
}
