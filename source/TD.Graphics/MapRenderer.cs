using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;

using SdlDotNet.Core;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Primitives;

using TD.GameLogic;
using TD.Gui;

namespace TD.Graphics
{
    public class MapRenderer : GuiItem
    {
        public const int MAP_WIDTH_PX = 480;
        public const int MAP_HEIGHT_PX = 480;

        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public SurfaceDictionary Textures { get; set; }
        public Map map;

        public MapRenderer(Map map, SurfaceDictionary Textures) : base("MapRenderer")
        {
            this.map = map;
            this.Textures = Textures;
            TileWidth = Tile.TILE_WIDTH;
            TileHeight = Tile.TILE_HEIGHT;
            Width = map.Columns * TileWidth;
            Height = map.Rows * TileHeight;
        }

        public override Surface Render()
        {
 	        Surface Buffer = new Surface(Width,Height);

            for (int i = 0; i < map.Rows; i++)
            {
                for (int j = 0; j < map.Columns; j++)
                {
                    String TexName = map.DefaultTileTexture;
                    Rectangle Clip = new Rectangle(new Point(j * TileWidth, i * TileHeight), new Size(Tile.TILE_WIDTH,Tile.TILE_HEIGHT));
                    Buffer.Blit(Textures[TexName], Clip);
                }
            }


            for(int i=0; i<map.Rows; i++)
            {
                for(int j=0; j<map.Columns; j++)
                {
                    String TexName = map.Tiles[i][j].Texture;
                    Rectangle Clip = new Rectangle(new Point(j * TileWidth, i * TileHeight), new Size(Tile.TILE_WIDTH, Tile.TILE_HEIGHT));
                    Buffer.Blit(Textures[TexName], Clip);
                }
            }

            Box Border = new Box(new Point(0, 0), new Size(Width - 1, Height - 1));

            Buffer.Draw(Border, Color.Black, true);

            return Buffer;
        }

        public override Surface  RenderHighlight()
        {
 	        return base.RenderHighlight();
        }
		
	}

}
