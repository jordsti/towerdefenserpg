using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using SdlDotNet.Core;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Primitives;
using SdlDotNet.Input;

using TD.GameLogic;
using TD.Graphics;
using TD.Gui;

namespace TD.Core
{
    public class MapEditRenderer : GuiItem
    {
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public SurfaceDictionary Textures { get; set; }
        public Map map;

        public MapEditRenderer(Map map, SurfaceDictionary Textures) : base("MapRenderer")
        {
            this.map = map;
            this.Textures = Textures;
            TileWidth = 40;
            TileHeight = 40;
            Width = map.Columns * TileWidth;
            Height = map.Rows * TileHeight;
        }

        public MapEditRenderer(Map map, int TileWidth, int TileHeight) : base("MapRenderer")
        {
            this.map = map;
            this.TileWidth = TileWidth;
            this.TileHeight = TileHeight;
            Width = map.Columns * TileWidth;
            Height = map.Rows * TileHeight;
        }

        public Surface RenderTypeText(Tile tile)
        {
            String Type = "US";

            if (tile.Type == TileType.Normal)
            {
                Type = "Nor";
            }
            else if (tile.Type == TileType.CreepStart)
            {
                Type = "CrS";
            }
            else if (tile.Type == TileType.CreepEnd)
            {
                Type = "CrE";
            }
            else if (tile.Type == TileType.CreepPath)
            {
                Type = "CrP";
            }
            else if (tile.Type == TileType.Obstacle)
            {
                Type = "Obs";
            }

            Surface Buffer = DefaultStyle.DefFont.Render(Type, Color.Black);

            return Buffer;
        }

        public Surface FillBackground()
        {
            Surface Buffer = new Surface(Width, Height);

            for (int i = 0; i < map.Rows; i++)
            {
                for (int j = 0; j < map.Columns; j++)
                {
                    Buffer.Blit(Textures[map.DefaultTileTexture], new Point(j * TileWidth, i * TileHeight));
                }
            }
            return Buffer;
        }

        public override Surface Render()
        {
            Surface Buffer = FillBackground();
            //Buffer.Fill(Color.WhiteSmoke);
            for (int i = 0; i < map.Rows; i++)
            {
                for (int j = 0; j < map.Columns; j++)
                {
                    Point p = new Point(j*TileWidth,i*TileHeight);
                    Surface Block = new Surface(TileWidth, TileHeight);
                    Block.Fill(Color.LightCoral);
                    Block.TransparentColor = Color.LightCoral;
                    Block.Transparent = true;

                    if (map.Tiles[i][j].Texture != "None")
                    {
                        String TextureName = map.Tiles[i][j].Texture;
                        Block = new Surface(Textures[TextureName]);
                    }
                    /*else
                    {
                        if (map.Tiles[i][j].Type == TileType.Normal)
                        {
                            Block.Fill(Color.LightBlue);
                        }
                        else if (map.Tiles[i][j].Type == TileType.CreepPath)
                        {
                            Block.Fill(Color.LightYellow);
                        }
                        else if (map.Tiles[i][j].Type == TileType.CreepStart)
                        {
                            Block.Fill(Color.Red);
                        }
                        else if (map.Tiles[i][j].Type == TileType.CreepEnd)
                        {
                            Block.Fill(Color.Orange);
                        }
                        else if (map.Tiles[i][j].Type == TileType.Obstacle)
                        {
                            Block.Fill(Color.SeaGreen);
                        }
                        else if (map.Tiles[i][j].Type == TileType.Unset)
                        {
                            Block.Fill(Color.White);
                        }
                    }*/

                    Surface TypeText = RenderTypeText(map.Tiles[i][j]);
                    Block.Blit(TypeText, new Point((Block.Width - TypeText.Width) / 2, (Block.Height - TypeText.Height) / 2));

                    Buffer.Blit(Block, p);
                }
            }

            return Buffer;
        }

        public override Surface RenderHighlight()
        {
            return base.RenderHighlight();
        }
    }
}
