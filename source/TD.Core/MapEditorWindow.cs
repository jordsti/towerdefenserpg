using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

using SdlDotNet.Core;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Primitives;
using SdlDotNet.Input;

using TD.Gui;
using TD.GameLogic;
using TD.Graphics;

namespace TD.Core
{
    public class EditorWindow : GuiWindow
    {
        private Map map;
        private String AppsDirectory;
        private MapEditRenderer mapRenderer;
        private SurfaceDictionary Textures;
        private TextureIndex TexIndex;

        private Grid MapGrid;

        private ButtonItem LoadMapButton;
        private ButtonItem SaveMap;
        private ButtonItem NewMap;
        private ButtonItem ChangeMapName;
        private ButtonItem SetDefaultTexture;
        private ButtonItem QuitButton;

        //private WindowBar WinBar;
        private BackgroundItem ButtonBg;
        private BackgroundItem Background;

        private LabelItem MapNameLabel;
        private LabelItem RowLabel;
        private LabelItem ColumnLabel;

        private TextInputDialogBox MapNameDialog;
        private DialogBox MessageBox;
        private TileMenu TileContextMenu;
        private TextureMenu TileTextureMenu;

        private OpenFileDialog openFile;

        public EditorWindow(Surface Screen)
            : base("Map Editor", 640, 800, Screen)
        {
            AppsDirectory = Directory.GetCurrentDirectory();

            TexIndex = TextureIndex.LoadTextureIndex("../../../../assets/Textures/MapTiles.xml");

            Textures = TexIndex.LoadTextures();

            map = new Map();
            ButtonBg = new BackgroundItem(Color.LightGray);
            ButtonBg.Width = Width;
            ButtonBg.Height = 100;
            ButtonBg.X = 0;
            ButtonBg.Y = 0;

            Background = new BackgroundItem(Color.WhiteSmoke);
            Background.Width = Width;
            Background.Height = Height;

            //WinBar = new WindowBar("Map Editor", Width);
            //WinBar.CloseButton = true;


            LoadMapButton = new ButtonItem("LoadMap", 120, 22, "Load Map");
            LoadMapButton.X = 20;
            LoadMapButton.Y = 30;

            SaveMap = new ButtonItem("SaveMap", 120, 22, "Save Map");
            SaveMap.X = 146;
            SaveMap.Y = 30;

            NewMap = new ButtonItem("NewMap", 120, 22, "New Map");
            NewMap.X = 272;
            NewMap.Y = 30;

            ChangeMapName = new ButtonItem("ChangeMapName", 140, 22, "Change Map Name");
            ChangeMapName.X = 398;
            ChangeMapName.Y = 30;

            SetDefaultTexture = new ButtonItem("SetDefaultTexture", 140, 22, "Set Default Texture");
            SetDefaultTexture.X = 20;
            SetDefaultTexture.Y = 4;

            QuitButton = new ButtonItem("Quit", 100, 22, "Quit");
            QuitButton.X = 166;
            QuitButton.Y = 4;

            mapRenderer = new MapEditRenderer(map,Textures);
            mapRenderer.X = 20;
            mapRenderer.Y = 120;

            MapGrid = new Grid(40, 40);
            MapGrid.Width = 40 * map.Columns;
            MapGrid.Height = 40 * map.Rows;
            MapGrid.X = 20;
            MapGrid.Y = 120;

            MapNameLabel = new LabelItem("Map Name : " + map.MapName);
            MapNameLabel.X = 16;
            MapNameLabel.Y = 65;

            RowLabel = new LabelItem("Row : -");
            RowLabel.X = 20;
            RowLabel.Y = 610; 
            ColumnLabel = new LabelItem("Column : -");
            ColumnLabel.X = 20;
            ColumnLabel.Y = 630; 

            MapNameDialog = new TextInputDialogBox("Map Name", "Map Name");
            MapNameDialog.X = (Width - MapNameDialog.Width) / 2;
            MapNameDialog.Y = (Height - MapNameDialog.Height) / 2;

            MessageBox = new DialogBox("Message", "Not set");
            MessageBox.X = (Width - MessageBox.Width) / 2;
            MessageBox.Y = (Height - MessageBox.Height) / 2;

            openFile = new OpenFileDialog();
            openFile.InitialDirectory = @"..\..\..\..\Assets\Maps\";
            openFile.Multiselect = false;
            openFile.Title = "Load a Map";
            openFile.Filter = "XML T.D. Map (*.xml) | *.xml";

            Container.Add(Background);
            Container.Add(ButtonBg);

            //Container.Add(WinBar);

            Container.Add(LoadMapButton);
            Container.Add(SaveMap);
            Container.Add(NewMap);
            Container.Add(ChangeMapName);
            Container.Add(SetDefaultTexture);
            Container.Add(QuitButton);

            Container.Add(MapNameLabel);
            Container.Add(RowLabel);
            Container.Add(ColumnLabel);

            Container.Add(mapRenderer);
            Container.Add(MapGrid);

            //Container.Add(MapNameDialog);

            MapNameDialog.SetEvents();
        }

        public void UpdateMapPosLabel(int Row, int Column)
        {
            RowLabel.Caption = "Row : " + Row;
            ColumnLabel.Caption = "Column : " + Column;
        }

        public void SetAllDefaultTexture(String TextureName)
        {
            for (int i = 0; i < map.Rows; i++)
            {
                for (int j = 0; j < map.Columns; j++)
                {
                    map.Tiles[i][j].Texture = TextureName;
                }
            }
        }

        public override void TextInputOk(TextInputDialogBox Box)
        {
            if (Box == MapNameDialog)
            {
                map.MapName = Box.TextEntry;
                MapNameLabel.Caption = "Map Name : " + Box.TextEntry;
                Box.TextEntry = "";
            }
        }

        public void SetMapName(String MapName)
        {
            MapNameLabel.Caption = "Map Name : " + MapName;
        }

        public void LoadMap()
        {
            openFile.ShowDialog();
            if (File.Exists(openFile.FileName))
            {
                map = Map.LoadMap(openFile.FileName);
                mapRenderer.map = map;
                SetMapName(map.MapName);
                MessageBox.Caption = "Map (" + map.MapName + ") loaded with success";
                MessageBox.Title = "Load a Map";
                MessageBox.SetEvents();
            }
            else
            {
                MessageBox.Caption = "This file doesn't exist !";
                MessageBox.Title = "Load a Map";
                MessageBox.SetEvents();
            }

            Container.Add(MessageBox);

            Directory.SetCurrentDirectory(AppsDirectory);
        }

        public override void MouseClick(object sender, MouseButtonEventArgs args)
        {
            if (MapGrid.GetGridRect().Contains(args.Position) && 
                !Container.Contains(MapNameDialog) && 
                !Container.Contains(TileContextMenu) && 
                !Container.Contains(MessageBox))
            {
                int Column = (args.X - MapGrid.X) / MapGrid.boxWidth;
                int Row = (args.Y - MapGrid.Y) / MapGrid.boxHeight;

                UpdateMapPosLabel(Row, Column);

                if (args.Button == MouseButton.PrimaryButton && !Container.Contains(TileTextureMenu))
                {
                    TileContextMenu = new TileMenu(map.Tiles[Row][Column], Garbage);

                    Container.Add(TileContextMenu);
                    TileContextMenu.SetEvents();
                    TileContextMenu.X = args.X;
                    TileContextMenu.Y = args.Y;
                }
                else if (args.Button == MouseButton.SecondaryButton && !Container.Contains(TileContextMenu))
                {
                    TileTextureMenu = new TextureMenu(map.Tiles[Row][Column], TexIndex, Garbage);
                    
                    Container.Add(TileTextureMenu);
                    TileTextureMenu.SetEvents();
                    TileTextureMenu.X = args.X;
                    TileTextureMenu.Y = args.Y;
                }
            }

            String ItemName = GetItemName(args);

            if (!Container.Contains(MapNameDialog) && 
                !Container.Contains(TileContextMenu) && 
                !Container.Contains(TileTextureMenu) && 
                !Container.Contains(MessageBox))
            {
                if (ItemName == "SaveMap")
                {
                    Map.SaveMap(map, "Maps/" + map.MapName + ".xml");
                    MessageBox.Title = "Map Saved !";
                    MessageBox.Caption = map.MapName + " has been saved with success !";
                    MessageBox.SetEvents();
                    Container.Add(MessageBox);
                }
                else if (ItemName == "NewMap")
                {
                    map = new Map();
                    mapRenderer.map = map;
                    Container.Add(MapNameDialog);
                }
                else if (ItemName == "LoadMap")
                {
                    LoadMap();
                }
                else if (ItemName == "ChangeMapName")
                {
                    Container.Add(MapNameDialog);
                }
                else if (ItemName == "SetDefaultTexture")
                {
                    SetAllDefaultTexture("Normal 1");
                }
                else if (ItemName == "Quit")
                {
                    MainMenuWindow window = new MainMenuWindow(Screen);
                    UnsetEvents();
                    window.SetEvents();
                }
            }
        }

        public override Surface CustomItemRender(GuiItem Item)
        {
            Surface Buffer;

            if (Item is MapEditRenderer)
            {
                MapEditRenderer MapRender = (MapEditRenderer)Item;

                Buffer = MapRender.Render();
            }
            else
            {
                Buffer = Item.Render();
            }

            return Buffer;
        }

    }
}
