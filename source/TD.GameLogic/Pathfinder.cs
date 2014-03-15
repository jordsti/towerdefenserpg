using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TD.GameLogic
{
    public static class Pathfinder
    {
        public static int MaxTry = 1024*32;

        public static MapCoordList FindCreepPath(Map map)
        {
            MapCoordList Path = new MapCoordList();
            MapCoord CreepStart = new MapCoord();
            MapCoord LastCoord = new MapCoord();
            int Tick = 0;

            for (int i = 0; i < map.Rows; i++)
            {
                for (int j = 0; j < map.Columns; j++)
                {
                    if (map.Tiles[i][j].Type == TileType.CreepStart)
                    {
                        CreepStart = new MapCoord(i, j);

                    }
                }

                if (!CreepStart.isEmpty)
                    break;
            }

            if (CreepStart.isEmpty)
            {
                throw new Exception("CreepStart Not Found !");
            }

            Path.Add(CreepStart);
            LastCoord = CreepStart;
            while (map.Tiles[LastCoord.Row][LastCoord.Column].Type != TileType.CreepEnd)
            {
                bool isFound = false;
                LastCoord = Path[Path.Count - 1];

                if (LastCoord.Row > 0 && !isFound)
                {
                    if (map.Tiles[LastCoord.Row - 1][LastCoord.Column].Type == TileType.CreepPath ||
                       map.Tiles[LastCoord.Row - 1][LastCoord.Column].Type == TileType.CreepEnd)
                    {
                        if (!Path.ContainsCoord(new MapCoord(LastCoord.Row - 1, LastCoord.Column)))
                        {
                            isFound = true;
                            Path.Add(new MapCoord(LastCoord.Row - 1, LastCoord.Column));
                        }
                    }
                }

                if (LastCoord.Row < map.Rows - 1 && !isFound)
                {
                    if (map.Tiles[LastCoord.Row + 1][LastCoord.Column].Type == TileType.CreepPath ||
                        map.Tiles[LastCoord.Row + 1][LastCoord.Column].Type == TileType.CreepEnd)
                    {
                        if (!Path.ContainsCoord(new MapCoord(LastCoord.Row + 1, LastCoord.Column)))
                        {
                            isFound = true;
                            Path.Add(new MapCoord(LastCoord.Row + 1, LastCoord.Column));
                        }
                    }
                }

                if (LastCoord.Column < map.Columns - 1 && !isFound)
                {
                    if (map.Tiles[LastCoord.Row][LastCoord.Column + 1].Type == TileType.CreepPath ||
                        map.Tiles[LastCoord.Row][LastCoord.Column + 1].Type == TileType.CreepEnd)
                    {
                        if (!Path.ContainsCoord(new MapCoord(LastCoord.Row, LastCoord.Column+1)))
                        {
                            isFound = true;
                            Path.Add(new MapCoord(LastCoord.Row, LastCoord.Column + 1));
                        }
                    }
                }

                if (LastCoord.Column > 0 && !isFound)
                {
                    if (map.Tiles[LastCoord.Row][LastCoord.Column - 1].Type == TileType.CreepPath ||
                        map.Tiles[LastCoord.Row][LastCoord.Column - 1].Type == TileType.CreepEnd)
                    {
                        if (!Path.ContainsCoord(new MapCoord(LastCoord.Row, LastCoord.Column - 1)))
                        {
                            isFound = true;
                            Path.Add(new MapCoord(LastCoord.Row, LastCoord.Column - 1));
                        }
                    }
                }

                if (Tick == MaxTry)
                {
                    throw new Exception("CreepEnd not found after " +MaxTry+ " Tries !");
                }

                Tick++;
            }

            return Path;       
        }
    }
}
