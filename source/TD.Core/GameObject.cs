
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using TD.GameLogic;

namespace TD.Core
{
    public enum GameDifficulty { Easy, Medium, Hard }

	public enum GameState { CreepAttack, NoAttack, Pause, GameOver, Complete, Error }
	
    public static class GameUtil
    {
        public static String DifficultyToString(GameDifficulty Diff)
        {
            if (Diff == GameDifficulty.Easy)
            {
                return "Easy";
            }
            else if (Diff == GameDifficulty.Medium)
            {
                return "Medium";
            }
            else if (Diff == GameDifficulty.Hard)
            {
                return "Hard";
            }
            else
            {
                return "Unset";
            }
        }
    }

	public class GameObject
    {
        #region fields
        public Map map { get; set; }
		public MapCoordList CreepPath { get; set; }
		public PlayerUnitDictionary PlayerUnits { get; set; }
		public CreepUnitList Creeps { get; set; }
        public ProjectileList Projectiles { get; set; }
        public XmlEnemyList CreepWaves { get; set; }
        public XmlCreepWave CurrentWave { get; private set; }
		public bool StartNewCreep=true;
		public long Tick=0;
        public String MapFile { get; set; }

        public AttackInfoList AttackInfos { get; private set; }
		public int Crystal { get; private set;}
		public int Gold { get; private set; }
		public int Wave { get; private set; }
        public int Score { get; private set; }
		public GameState State { get; private set; }
        public GameDifficulty Difficulty { get; private set; }
        public CombatLog LastCombatLog { get; private set; }
        public ScoreList MapTop10 { get; private set; }
        #endregion

        #region constructors

        public GameObject(String MapFile,GameDifficulty Difficulty)
		{
            this.MapFile = MapFile;
			NewGame(Difficulty);
		}
		
		public GameObject()
		{
			NewGame(GameDifficulty.Hard);
		}
		
		public void NewGame(GameDifficulty Difficulty)
		{

			map = new Map();
			CreepPath = new MapCoordList();
            PlayerUnits = new PlayerUnitDictionary();
            AttackInfos = new AttackInfoList();
            Projectiles = new ProjectileList();

			this.Difficulty = Difficulty;

            if (Difficulty == GameDifficulty.Easy)
            {
                Gold = 250;
                Crystal = 50;
            }
            else if (Difficulty == GameDifficulty.Medium)
            {
                Gold = 150;
                Crystal = 30;
            }
            else if (Difficulty == GameDifficulty.Hard)
            {
                Gold = 75;
                Crystal = 20;
            }

            Wave = 1;
            Score = 0;
            State = GameState.NoAttack;
			LastCombatLog = new CombatLog();
            LoadMap(MapFile);
		}

        public void NewGame(GameDifficulty Difficulty,String MapFile)
        {
            this.MapFile = MapFile;
            NewGame(Difficulty);
            LoadMap(MapFile);
        }

        /// <summary>
        /// Generate a new GameObject with the specified Difficulty
        /// </summary>
        /// <param name="Difficulty">Difficulty of the Game</param>
        public GameObject(GameDifficulty Difficulty)
        {
            NewGame(Difficulty);
        }
        #endregion

        #region methods

        public void LoadMap(String Path)
        {
            MapFile = Path;
            map = Map.LoadMap(Path);
            CreepWaves = XmlEnemyList.LoadList("../../../../assets/GameData/" + map.CreepListFile);
            
            try
            {
                CreepPath = Pathfinder.FindCreepPath(map);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                State = GameState.Error;
            }

            CurrentWave = CreepWaves.Wave[0];
        }

        /// <summary>
        /// Start the next wave
        /// </summary>
        public void StartWave()
        {
            if (State == GameState.NoAttack)
            {
                Tick = 0;
                StartNewCreep = true;
                int WaveId = Wave - 1;
                Creeps = new CreepUnitList(CreepWaves.Wave[Wave - 1].ToCreepUnit(), CreepWaves.Wave[Wave - 1].Numbers);
                State = GameState.CreepAttack;
                Score += Gold;
            }
        }

        public XmlCreepWave GetNextWaveInfo()
        {   
            XmlCreepWave WaveInfo = new XmlCreepWave();
            if (State == GameState.NoAttack)
            {
                WaveInfo = CreepWaves.Wave[Wave - 1];
            }

            return WaveInfo;
        }

        /// <summary>
        /// This method is call by GameTick when all creeps are dead
        /// </summary>
        public void FinishedWave()
        {
            Score += 1000;
            Score += Crystal * 1500;
            Score += Wave * 500;

            Wave++;

            State = GameState.NoAttack;

            LastCombatLog = new CombatLog(AttackInfos);
            AttackInfos.Clear();

            Projectiles.Clear();

            if (Wave == CreepWaves.Wave.Count + 1)
            {
                //Complete
                State = GameState.Complete;
            }
            else
            {
                CurrentWave = CreepWaves.Wave[Wave - 1];
            }
        }

        /// <summary>
        /// Remove the creep that have no more health
        /// </summary>
		public void RemoveDeadCreeps()
        {
            int DeadCreeps = 0;
            CreepUnitList toRemove = new CreepUnitList();

            foreach (CreepUnit Unit in Creeps)
            {
                if (Unit.Health <= 0)
                { 
                    toRemove.Add(Unit);
                    DeadCreeps++;
                    Gold += Unit.Level * 3;
                    Score += Unit.Level * 200;
                }
            }


            foreach(CreepUnit Unit in toRemove)
            {
                Unit.Position = new Point();
                Unit.Health = 0;
                Creeps.Remove(Unit);
            }

        }

        /// <summary>
        /// Increase the level of a Player Unit at the specified position
        /// </summary>
        /// <param name="Coord">Position of Unit</param>
        public void IncreaseUnitLevel(MapCoord Coord)
        {
            if (PlayerUnits.ContainsCoord(Coord) && PlayerUnits.GetUnitAt(Coord).Class != UnitClasses.None)
            {
                int Cost = PlayerUnit.LevelCost(PlayerUnits.GetUnitAt(Coord).Level+1);

                if (Gold >= Cost)
                {
                    PlayerUnits.GetUnitAt(Coord).LevelUp();
                    Gold -= Cost;
                    Score += PlayerUnits.GetUnitAt(Coord).Level * 100;
                }
            }
        }

        /// <summary>
        /// Increase the level of a Player Unit
        /// </summary>
        /// <param name="Unit">PlayerUnit</param>
        public void IncreaseUnitLevel(PlayerUnit Unit)
        {
            if (PlayerUnits.ContainsValue(Unit) && Unit.Class != UnitClasses.None)
            {
                int Cost = PlayerUnit.LevelCost(Unit.Level + 1);

                if (Gold >= Cost)
                {
                    Unit.LevelUp();
                    Gold -= Cost;
                    Score += Unit.Level * 100;
                }
            }
        }

        /// <summary>
        /// Add a PlayerUnit to the specified position and class
        /// </summary>
        /// <param name="Coord">Position of Unit</param>
        /// <param name="UnitClass">Class of Unit</param>
        public void AddPlayerUnit(MapCoord Coord, UnitClasses UnitClass)
        {
            if (map.Tiles[Coord.Row][Coord.Column].Type == TileType.Normal && !PlayerUnits.ContainsCoord(Coord))
            {
                if (Gold >= PlayerUnit.LevelCost(1) && UnitClass == UnitClasses.Thieft && PlayerUnits.CountClass(UnitClasses.Thieft) < 4)
                {
                    Gold -= PlayerUnit.LevelCost(1);
                    PlayerUnits.Add(Coord, new Thieft());
                    PlayerUnits[Coord].Position = Coord.ToPointMiddle();
                    Score += 100;
                }
                else if (Gold >= PlayerUnit.LevelCost(1) && UnitClass != UnitClasses.Thieft)
                {
                    Gold -= PlayerUnit.LevelCost(1);
                    Score += 100;

                    if (UnitClass == UnitClasses.Archer)
                    {
                        PlayerUnits.Add(Coord, new Archer());
                        PlayerUnits[Coord].Position = Coord.ToPointMiddle();
                    }
                    else if (UnitClass == UnitClasses.Mage)
                    {
                        PlayerUnits.Add(Coord, new Mage());
                        PlayerUnits[Coord].Position = Coord.ToPointMiddle();
                    }
                    else if (UnitClass == UnitClasses.Soldier)
                    {
                        PlayerUnits.Add(Coord, new Soldier());
                        PlayerUnits[Coord].Position = Coord.ToPointMiddle();
                    }
                    else if (UnitClass == UnitClasses.Paladin)
                    {
                        PlayerUnits.Add(Coord, new Paladin());
                        PlayerUnits[Coord].Position = Coord.ToPointMiddle();
                    }
                }
            }
        }

        /// <summary>
        /// Tick of the GameObject
        /// </summary>
        public void GameTick()
        {

            if (Crystal <= 0)
            {
                Crystal = 0;
                State = GameState.GameOver;
            }

            if (State == GameState.CreepAttack)
            {
                RemoveDeadCreeps();
                CheckCreepAtEnd();
                CreepMoveTick();
                ProjectileTick();

                if (Tick % 15 == 0)
                {
                    PlayerUnitAttack();
                }
            }
        }

        public void ProjectileTick()
        {
            AttackInfoList info = Projectiles.Update();
            AttackInfos.AddRange(info);
        }

        public void SaveScore(String PlayerName)
        {
            ScoreEntry Entry = new ScoreEntry(PlayerName, Score, Wave - 1, Difficulty, map.MapName);

            ScoreList HighscoreList = ScoreList.LoadScoreList("../../../../assets/GameData/ScoreHistory.xml");

            HighscoreList.ScoreEntries.Add(Entry);

            ScoreList.SaveScoreList(HighscoreList, "../../../../assets/GameData/ScoreHistory.xml");

            MapTop10 = HighscoreList.TrimList(map.MapName, Difficulty);
            MapTop10.KeepTop10();
        }

        /// <summary>
        /// Do the Attack of the PlayerUnit
        /// </summary>
        public void PlayerUnitAttack()
        {
            foreach (MapCoord Coord in PlayerUnits.Keys)
            {
                CreepUnitList InRange = Creeps.EnemyInRange(Coord, PlayerUnits[Coord].Range, PlayerUnits[Coord].MinusRange);
                PlayerUnit PlayerClasses = PlayerUnits[Coord];

                if (PlayerClasses is Soldier)
                {
                    Soldier Sol = (Soldier)PlayerClasses;
                    AttackInfoList info = Sol.Defend(InRange);
                    AttackInfos.AddRange(info);
                }
                else if (PlayerClasses is Paladin)
                {
                    Paladin Pal = (Paladin)PlayerClasses;
                    AttackInfoList info = Pal.Defend(InRange);
                    AttackInfos.AddRange(info);
                }
                else if (PlayerClasses is Mage)
                {
                    Mage mage = (Mage)PlayerClasses;
                    AttackInfoList info = mage.Defend(InRange);
                    AttackInfos.AddRange(info);
                }
                else if (PlayerClasses is Thieft)
                {
                    Thieft thieft = (Thieft)PlayerClasses;
                    AttackInfoList info = thieft.Defend(InRange);
                    AttackInfos.AddRange(info);
                    if (Tick % 60 == 0)
                    {
                        AttackInfoList steal = thieft.StealGold(InRange);

                        foreach (AttackInfo atkInf in steal)
                        {
                            Gold += atkInf.GoldStolen;
                        }

                        AttackInfos.AddRange(steal);
                    }
                }
                else if (PlayerClasses is Archer)
                {
                    Archer arch = (Archer)PlayerClasses;
                    //AttackInfoList info = arch.Defend(InRange);
                    ProjectileList list = arch.ThrowProjectile(InRange);

                    Projectiles.AddRange(list);

                    //AttackInfos.AddRange(info);
                }
            }
        }
		
        /// <summary>
        /// Remove the creep when there are at the end of the path
        /// </summary>
		public void CheckCreepAtEnd()
		{
			MapCoord EndCoord = CreepPath[CreepPath.Count-1];
			MapCoord Diff = CreepPath[CreepPath.Count-2].Diff(EndCoord);
			Point Pos = new Point();
			Size RectSize = new Size(0,0);
			if(Diff.Row == -1)
			{
				Pos = new Point(EndCoord.ToPoint().X,EndCoord.ToPoint().Y + 20);
				RectSize = new Size(40,20);
			}
			else if(Diff.Row == 1)
			{
				Pos = new Point(EndCoord.ToPoint().X,EndCoord.ToPoint().Y);
				RectSize = new Size(40,20);
			}
			else if(Diff.Column == -1)
			{
				Pos = new Point(EndCoord.ToPoint().X,EndCoord.ToPoint().Y);
				RectSize = new Size(20,40);
			}
			else if(Diff.Column == 1)
			{
				Pos = new Point(EndCoord.ToPoint().X + 20,EndCoord.ToPoint().Y);
				RectSize = new Size(20,40);
			}
			
			CreepUnitList InRect = Creeps.InRectangle(new Rectangle(Pos,RectSize));
			
			foreach(CreepUnit Unit in InRect)
			{
                Unit.Position = new Point();
				Creeps.Remove(Unit);
				
				Crystal -= Unit.StealAmount;
			}
			
			
		}
		
        /// <summary>
        /// Creep mouvement
        /// </summary>
		public void CreepMoveTick()
        {
            if (Tick % 30 == 0)
            {
                StartNewCreep = true;
            }
			
            foreach (CreepUnit Unit in Creeps)
            {
                if (StartNewCreep && Unit.Position.IsEmpty)
                {
                    StartNewCreep = false;
                    Unit.Position = CreepPath[0].ToPoint();
                }
                else if(!Unit.Position.IsEmpty)
                {
                    MapCoord UnitCoord = new MapCoord(Unit.Position);
                    MapCoord before = CreepPath.Before(UnitCoord);
					MapCoord Diff = new MapCoord();
                    int Next = CreepPath.GetIndex(UnitCoord);
					
					if(Next == CreepPath.Count-1)
					{
						Diff = CreepPath[Next-1].Diff(UnitCoord);
					}
					else
					{
						Diff = UnitCoord.Diff(CreepPath[Next+1]);
					}

                    if (Diff.Row == 1)
                    {
                        Unit.Position = new Point(Unit.Position.X, Unit.Position.Y + Unit.Speed);
                        Unit.Direction = Direction.Down;
                    }
                    else if (Diff.Row == -1)
                    {
                       	Unit.Position = new Point(Unit.Position.X, Unit.Position.Y - Unit.Speed);
                        Unit.Direction = Direction.Up;
                    }
                    else if (Diff.Column == 1)
					{
                        if (!before.isEmpty)
						{
                            if (Unit.Position.Y % Tile.TILE_HEIGHT != 0 && before.Diff(UnitCoord).Row == -1)
							{
								Unit.Position = new Point(Unit.Position.X, Unit.Position.Y - Unit.Speed);
                                Unit.Direction = Direction.Up;
							}
							else
							{
								Unit.Position = new Point(Unit.Position.X + (Unit.Speed*Diff.Column), Unit.Position.Y);
                                Unit.Direction = Direction.Left;
							}
						}
						else
						{
                        	Unit.Position = new Point(Unit.Position.X + Unit.Speed, Unit.Position.Y);
                            Unit.Direction = Direction.Left;
                    	}
					}
                    else if (Diff.Column == -1)
                    {
                        if (!before.isEmpty)
						{
                            if (Unit.Position.Y % Tile.TILE_HEIGHT != 0 && before.Diff(UnitCoord).Row == -1)
							{
                                int mustX = UnitCoord.ToPoint().X;

                                if (mustX == Unit.Position.X)
                                {
                                    Unit.Position = new Point(Unit.Position.X, Unit.Position.Y - Unit.Speed);
                                    Unit.Direction = Direction.Up;
                                }
                                else
                                {
                                    Unit.Position = new Point(Unit.Position.X + (Unit.Speed * Diff.Column), Unit.Position.Y);
                                    Unit.Direction = Direction.Right;
                                }
							}
							else
							{
								Unit.Position = new Point(Unit.Position.X + (Unit.Speed*Diff.Column), Unit.Position.Y);
                                Unit.Direction = Direction.Right;
							}
						}
						else
						{
                        	Unit.Position = new Point(Unit.Position.X - Unit.Speed, Unit.Position.Y);
                            Unit.Direction = Direction.Right;
                    	}
                    }
                }

            }

            if (Creeps.Count == 0)
            {
                FinishedWave();
            }

            Tick++;
        }

        #endregion
    }
}
