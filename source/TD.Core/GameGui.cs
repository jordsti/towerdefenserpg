using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using SdlDotNet.Input;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Primitives;
using SdlDotNet.Core;

using TD.Gui;
using TD.GameLogic;

namespace TD.Core
{

    public class CombatLogBox : GuiItem
    {
        public CombatLog Log { get; set; }

        private BackgroundItem Background;
        private ButtonItem bOk;

        private LabelItem labelPhyDmg;
        private LabelItem labelMagDmg;
        private LabelItem labelCritDmg;
        private LabelItem labelCritTimes;
        private LabelItem labelGoldStolen;
        private List<GuiItem> Garbage;

        private WindowBar WinBar;

        private Point MousePos;
        private bool Drag = false;

        public CombatLogBox(CombatLog Log,List<GuiItem> Garbage) : base("CombatLogBox")
        {
            this.Garbage = Garbage;
            Width = 250;
            Height = 200;
            this.Log = Log;
            MousePos = new Point();
            WinBar = new WindowBar("Combat Log", Width);
            WinBar.CloseButton = false;

            Background = new BackgroundItem(Color.WhiteSmoke);
            Background.Width = Width;
            Background.Height = Height;

            bOk = new ButtonItem("Ok", 120, 22, "Ok");
            bOk.Y = 170;
            bOk.X = (Width - bOk.Width) / 2;

            labelPhyDmg = new LabelItem("Physical Damage : "+Log.PhysicalDamage);

            labelMagDmg = new LabelItem("Magic Damage : " + Log.MagicDamage);

            labelCritDmg = new LabelItem("Critical Damage : " + Log.CriticalDamage);

            labelCritTimes = new LabelItem("Critical Times : " + Log.CriticalTimes);

            labelGoldStolen = new LabelItem("Gold Stolen : " + Log.GoldStolen);

        }

        public void UpdateLabel()
        {
            labelPhyDmg.Caption = "Physical Damage : " + Log.PhysicalDamage;
            labelPhyDmg.Render();
            labelPhyDmg.X = (Width - labelPhyDmg.Width) / 2;
            labelPhyDmg.Y = 40;

            labelMagDmg.Caption = "Magic Damage : " + Log.MagicDamage;
            labelMagDmg.Render();
            labelMagDmg.X = (Width - labelMagDmg.Width) / 2;
            labelMagDmg.Y = 65;

            labelCritDmg.Caption = "Critical Damage : " + Log.CriticalDamage;
            labelCritDmg.Render();
            labelCritDmg.X = (Width - labelCritDmg.Width) / 2;
            labelCritDmg.Y = 90;

            labelCritTimes.Caption = "Critical Times : " + Log.CriticalTimes;
            labelCritTimes.Render();
            labelCritTimes.X = (Width - labelCritTimes.Width) / 2;
            labelCritTimes.Y = 115;

            labelGoldStolen.Caption = "Gold Stolen : " + Log.GoldStolen;
            labelGoldStolen.Render();
            labelGoldStolen.X = (Width - labelGoldStolen.Width) / 2;
            labelGoldStolen.Y = 140;
        }

        public override Surface Render()
        {
            Surface Buffer = new Surface(Width, Height);

            Buffer.Blit(Background.Render(),new Point(0,0));

            Buffer.Blit(WinBar.Render(),new Point(0,0));

            Buffer.Blit(labelPhyDmg.Render(), labelPhyDmg.GetPoint());
            Buffer.Blit(labelMagDmg.Render(), labelMagDmg.GetPoint());
            Buffer.Blit(labelCritDmg.Render(), labelCritDmg.GetPoint());
            Buffer.Blit(labelCritTimes.Render(), labelCritTimes.GetPoint());
            Buffer.Blit(labelGoldStolen.Render(), labelGoldStolen.GetPoint());

            if (bOk.GetRect(GetPoint()).Contains(MousePos))
            {
                Buffer.Blit(bOk.RenderHighlight(), bOk.GetPoint());
            }
            else
            {
                Buffer.Blit(bOk.Render(), bOk.GetPoint());
            }

            Buffer.AlphaBlending = true;
            Buffer.Alpha = 200;

            return Buffer;
        }

        public override Surface RenderHighlight()
        {
            return base.RenderHighlight();
        }

        public void SetEvents()
        {
            Events.MouseButtonDown += new EventHandler<MouseButtonEventArgs>(this.MouseDown);
            Events.MouseButtonUp += new EventHandler<MouseButtonEventArgs>(this.MouseClick);
            Events.MouseMotion += new EventHandler<MouseMotionEventArgs>(this.MouseMotion);
        }

        public void UnsetEvents()
        {
            Events.MouseButtonDown -= new EventHandler<MouseButtonEventArgs>(this.MouseDown);
            Events.MouseButtonUp -= new EventHandler<MouseButtonEventArgs>(this.MouseClick);
            Events.MouseMotion -= new EventHandler<MouseMotionEventArgs>(this.MouseMotion);
        }

        public void MouseMotion(object sender, MouseMotionEventArgs args)
        {
            if (Drag)
            {
                int xdiff = MousePos.X - args.X;
                int ydiff = MousePos.Y - args.Y;
                X -= xdiff;
                Y -= ydiff;
            }
            MousePos = args.Position;

        }

        public void MouseClick(object sender, MouseButtonEventArgs args)
        {
            Drag = false;

            if (bOk.GetRect(GetPoint()).Contains(args.Position))
            {
                UnsetEvents();
                Garbage.Add(this);
            }
        }

        public void MouseDown(object sender, MouseButtonEventArgs args)
        {
            Rectangle wBar = new Rectangle(new Point(X, Y), new Size(WinBar.Width, WinBar.Height));

            if (wBar.Contains(args.Position))
            {
                Drag = true;
            }
        }

    }

    public class GameCompleteBox : GuiItem
    {
        private GameObject GameObj;

        private BackgroundItem Background;
        private ButtonItem bNewGame;
        private ButtonItem bQuit;
        private SdlDotNet.Graphics.Font font;
        private List<GuiItem> Garbage;

        private Point MousePos;

        public GameCompleteBox(GameObject GameObj, List<GuiItem> Garbage)
            : base("GameCompleteBox")
        {
            Width = 400;
            Height = 400;
            this.Garbage = Garbage;
            this.GameObj = GameObj;
            MousePos = new Point();
            Background = new BackgroundItem(Color.WhiteSmoke);
            Background.Width = Width;
            Background.Height = Height;
            font = new SdlDotNet.Graphics.Font("../../../../assets/GameData/VeraSe.ttf", 36);
            bNewGame = new ButtonItem("NewGame", 120, 22, "New Game");
            bNewGame.X = 100;
            bNewGame.Y = 300;

            bQuit = new ButtonItem("Quit", 120, 22, "Quit");
            bQuit.X = 230;
            bQuit.Y = 300;


        }

        public void MouseMotion(object sender, MouseMotionEventArgs args)
        {
            MousePos = args.Position;
        }

        public void MouseClick(object sender, MouseButtonEventArgs args)
        {
            if (bNewGame.GetRect(GetPoint()).Contains(args.Position))
            {
                Garbage.Add(this);
                UnsetEvents();
                GameObj.NewGame(GameDifficulty.Hard);
            }
            else if (bQuit.GetRect(GetPoint()).Contains(args.Position))
            {
                Events.QuitApplication();
            }
        }

        public void SetEvents()
        {
            Events.MouseButtonDown += new EventHandler<MouseButtonEventArgs>(this.MouseClick);
            Events.MouseMotion += new EventHandler<MouseMotionEventArgs>(this.MouseMotion);
        }

        public void UnsetEvents()
        {
            Events.MouseButtonDown -= new EventHandler<MouseButtonEventArgs>(this.MouseClick);
            Events.MouseMotion -= new EventHandler<MouseMotionEventArgs>(this.MouseMotion);
        }

        public override Surface Render()
        {
            Surface Buffer = new Surface(Width, Height);

            Buffer.Blit(Background.Render(), Background.GetPoint());

            Surface text1 = font.Render("You WIN !", Color.Black);
            Surface text2 = font.Render("Score : " + GameObj.Score, Color.Black);
            Surface text3 = font.Render("Crystals Left : " + GameObj.Crystal , Color.Black);

            Point pt = new Point((Width - text1.Width) / 2, 10);

            Buffer.Blit(text1, pt);

            pt = new Point((Width - text2.Width) / 2, 50);

            Buffer.Blit(text2, pt);

            pt = new Point((Width - text2.Width) / 2, 90);

            Buffer.Blit(text3, pt);

            for (int i = 0; i < GameObj.MapTop10.ScoreEntries.Count; i++)
            {
                ScoreEntry Entry = GameObj.MapTop10.ScoreEntries[i];
                String ScoreStr = (i + 1) + " - " + Entry.PlayerName + "    " + Entry.Wave + "    " + Entry.Score;
                Surface ScoreText = DefaultStyle.GetFont().Render(ScoreStr, Color.Black);

                Point p = new Point(20, 160 + (i * 20));

                Buffer.Blit(ScoreText, new Rectangle(p, new Size(ScoreText.Width, ScoreText.Height)));
            }

            if (bNewGame.GetRect(GetPoint()).Contains(MousePos))
            {
                Buffer.Blit(bNewGame.RenderHighlight(), bNewGame.GetPoint());
                Buffer.Blit(bQuit.Render(), bQuit.GetPoint());
            }
            else if (bQuit.GetRect(GetPoint()).Contains(MousePos))
            {
                Buffer.Blit(bNewGame.Render(), bNewGame.GetPoint());
                Buffer.Blit(bQuit.RenderHighlight(), bQuit.GetPoint());
            }
            else
            {
                Buffer.Blit(bNewGame.Render(), bNewGame.GetPoint());
                Buffer.Blit(bQuit.Render(), bQuit.GetPoint());
            }

            Buffer.AlphaBlending = true;
            Buffer.Alpha = 175;

            return Buffer;
        }

        public override Surface RenderHighlight()
        {
            return base.RenderHighlight();
        }


    }
	
	public class GameOverBox : GuiItem
	{
		private GameObject GameObj;
		
		private BackgroundItem Background;
		private ButtonItem bContinue;
		private ButtonItem bQuit;
		private SdlDotNet.Graphics.Font font;
		private List<GuiItem> Garbage;
		
		private Point MousePos;
		
		public GameOverBox(GameObject GameObj,List<GuiItem> Garbage) : base("GameOverBox")
		{
			Width = 400;
			Height = 400;
			this.Garbage = Garbage;
			this.GameObj = GameObj;
			MousePos = new Point();
			Background = new BackgroundItem(Color.WhiteSmoke);
			Background.Width = Width;
			Background.Height = Height;
            font = new SdlDotNet.Graphics.Font("../../../../assets/GameData/VeraSe.ttf", 36);
			bContinue = new ButtonItem("Continue",120,22,"Try Again");
			bContinue.X = 100;
			bContinue.Y = 300;
			
			bQuit = new ButtonItem("Quit",120,22,"Quit");
			bQuit.X = 230;
			bQuit.Y = 300;
		}
		
		public void MouseMotion(object sender,MouseMotionEventArgs args)
		{
			MousePos = args.Position;
		}
		
		public void MouseClick(object sender,MouseButtonEventArgs args)
		{
			if(bContinue.GetRect(GetPoint()).Contains(args.Position))
			{
				Garbage.Add(this);
				UnsetEvents();
				GameObj.NewGame(GameDifficulty.Hard);
			}
			else if(bQuit.GetRect(GetPoint()).Contains(args.Position))
			{
				Events.QuitApplication();
			}
		}
		
		public void SetEvents()
        {
            Events.MouseButtonDown += new EventHandler<MouseButtonEventArgs>(this.MouseClick);
            Events.MouseMotion += new EventHandler<MouseMotionEventArgs>(this.MouseMotion);
        }

        public void UnsetEvents()
        {
            Events.MouseButtonDown -= new EventHandler<MouseButtonEventArgs>(this.MouseClick);
            Events.MouseMotion -= new EventHandler<MouseMotionEventArgs>(this.MouseMotion);
        }
		
		public override Surface Render ()
		{
			Surface Buffer = new Surface(Width,Height);
			
			Buffer.Blit(Background.Render(),Background.GetPoint());
			
			Surface text1 = font.Render("GAME OVER !", Color.Black);
			Surface text2 = font.Render("Score : " + GameObj.Score , Color.Black);
            Surface text3 = font.Render("NOOB", Color.Black);
			Point pt = new Point((Width - text1.Width) / 2, 10);
			
			Buffer.Blit(text1,pt);
			
			pt = new Point((Width - text2.Width) /2, 50);
			
			Buffer.Blit(text2,pt);

            pt = new Point((Width - text2.Width) / 2, 90);

            for (int i = 0; i < GameObj.MapTop10.ScoreEntries.Count; i++)
            {
                ScoreEntry Entry = GameObj.MapTop10.ScoreEntries[i];
                String ScoreStr = (i+1)+" - "+Entry.PlayerName+"    "+Entry.Wave+"    "+Entry.Score;
                Surface ScoreText = DefaultStyle.GetFont().Render(ScoreStr,Color.Black);

                Point p = new Point(20, 160 + (i * 20));

                Buffer.Blit(ScoreText, new Rectangle(p, new Size(ScoreText.Width, ScoreText.Height)));
            }

            Buffer.Blit(text3, pt);
			
			if(bContinue.GetRect(GetPoint()).Contains(MousePos))
			{
				Buffer.Blit(bContinue.RenderHighlight(),bContinue.GetPoint());
				Buffer.Blit(bQuit.Render(),bQuit.GetPoint());
			}
			else if(bQuit.GetRect(GetPoint()).Contains(MousePos))
			{
				Buffer.Blit(bContinue.Render(),bContinue.GetPoint());
				Buffer.Blit(bQuit.RenderHighlight(),bQuit.GetPoint());
			}
			else
			{
				Buffer.Blit(bContinue.Render(),bContinue.GetPoint());
				Buffer.Blit(bQuit.Render(),bQuit.GetPoint());
			}

            Buffer.AlphaBlending = true;
            Buffer.Alpha = 175;

			return Buffer;
		}

		public override Surface RenderHighlight ()
		{
			return base.RenderHighlight ();
		}

		
	}


    public class CreepNextWaveBox : GuiItem
    {
        public const int INFOBOX_HEIGHT = 105;

        public XmlEnemyList WaveList { get; set; }
        public int CurrentWave { get; set; }

        private BackgroundItem Background;

        private LabelItem BoxTitle;

        private List<Surface> WavesGfx;

        private List<GuiItem> Container;

        public CreepNextWaveBox(XmlEnemyList WaveList) : base("CreepNextWaveBox")
        {
            this.WaveList = WaveList;
            CurrentWave = 0;

            Width = 185;
            Height = 480;

            Background = new BackgroundItem(Color.AliceBlue);
            Background.FromSize(new Size(Width, Height));

            BoxTitle = new LabelItem("Incoming creeps");
            BoxTitle.Render();
            BoxTitle.X = (Width - BoxTitle.Width) / 2;
            BoxTitle.Y = 4;

            WavesGfx = new List<Surface>();

            foreach(XmlCreepWave wave in WaveList.Wave)
            {
                Surface gfx = new Surface("../../../../assets/Sprite/" + wave.GfxName + ".png");

                WavesGfx.Add(gfx);
            }
        }


        public override Surface Render()
        {
            Surface Buffer = new Surface(Width, Height);

            Buffer.Blit(Background.Render(), Background.GetRect());

            Buffer.Blit(BoxTitle.Render(), BoxTitle.GetRect());

            int j = 0;
            for (int i = CurrentWave; i < WaveList.Wave.Count; i++)
            {
                if (j == 5)
                {
                    break;
                }
                XmlCreepWave wave = WaveList.Wave[i];
                //Surface label = DefaultStyle.GetFont().Render(wave.Name,Color.Black);

                //Point p = new Point((Width - label.Width) / 2, (j * INFOBOX_HEIGHT) + 20);

                if (j == 0)
                {
                    Rectangle bg = new Rectangle(new Point(10, (j * INFOBOX_HEIGHT) + 20), new Size(Width - 20, INFOBOX_HEIGHT - 10));
                    Buffer.Fill(bg, Color.LightYellow);
                }

                Box Border = new Box(new Point(10,(j*INFOBOX_HEIGHT) + 20),new Size(Width - 20, INFOBOX_HEIGHT - 10));

                Surface CreepGfx = WavesGfx[i];

                Rectangle GfxRect = new Rectangle(new Point((Width - CreepGfx.Width) / 2, (j * INFOBOX_HEIGHT) + 25), CreepGfx.Size);

                Surface HealthLabel = DefaultStyle.GetFont().Render("Health : " + wave.Health, Color.Black);

                Point HealthP = new Point((Width - HealthLabel.Width) / 2, (j * INFOBOX_HEIGHT) + 55);

                Surface WeaknessLabel = DefaultStyle.GetFont().Render("Weakness : " + UnitDmg.GetDamageType(wave.Weakness), Color.Black);

                Point WeaknessP = new Point((Width - WeaknessLabel.Width) / 2, (j * INFOBOX_HEIGHT) + 70);

                Surface ResistLabel = DefaultStyle.GetFont().Render("Resist : " + UnitDmg.GetDamageType(wave.Resist), Color.Black);

                Point ResistP = new Point((Width - ResistLabel.Width) / 2, (j * INFOBOX_HEIGHT) + 85);

                Surface creepsNb = DefaultStyle.GetBoldFont().Render("x" + wave.Numbers, Color.OrangeRed);
                Point creepsNbP = new Point(GfxRect.X + GfxRect.Width + 5, (j * INFOBOX_HEIGHT) + 35);


                if (wave.StealAmount != 1)
                {
                    Surface stealLabel = DefaultStyle.GetFont().Render("Steal Multiple Crystal", Color.Red);
                    Point stealP = new Point((Width - stealLabel.Width) / 2, (j * INFOBOX_HEIGHT) + 100);

                    Buffer.Blit(stealLabel, stealP);
                }
                else if (wave.Speed != 1)
                {
                    Surface speedLabel = DefaultStyle.GetFont().Render("Moving Faster", Color.Red);
                    Point speedP = new Point((Width - speedLabel.Width) / 2, (j * INFOBOX_HEIGHT) + 100);

                    Buffer.Blit(speedLabel, speedP);
                }

                Buffer.Blit(CreepGfx, GfxRect);
                Buffer.Blit(HealthLabel, HealthP);

                Buffer.Blit(WeaknessLabel, WeaknessP);
                Buffer.Blit(ResistLabel, ResistP);

                Buffer.Blit(creepsNb, creepsNbP);

                Buffer.Draw(Border, Color.Black);

                j++;
            }


                return Buffer;
        }

    }

    public class PlayerUnitInfoBox : GuiItem
    {
        public PlayerUnit Unit { get; set; }
        private BackgroundItem Background;

        private LabelItem labelClass;
        private LabelItem labelLevel;
        private LabelItem labelStr;
        private LabelItem labelInt;
        private LabelItem labelLuck;
        private LabelItem labelDmgType;
        private LabelItem labelNxLvlCost;
        private ButtonItem buttonIncLevel;
        public Point MousePos { get; set; }

        public PlayerUnitInfoBox(Point Pos) : base("PlayerUnitInfoBox")
        {
            this.Unit = new PlayerUnit();
            this.X = Pos.X;
            this.Y = Pos.Y;
            Width = 185;
            Height = 300;
            MousePos = Mouse.MousePosition;
            Background = new BackgroundItem(Color.PaleGreen);
            Background.Width = Width;
            Background.Height = Height;

            labelClass = new LabelItem();

            labelLevel = new LabelItem();

            labelStr = new LabelItem();

            labelInt = new LabelItem();

            labelLuck = new LabelItem();

            labelDmgType = new LabelItem();

            labelNxLvlCost = new LabelItem();

            buttonIncLevel = new ButtonItem("levelup", 160, 22, "Increase to Level " + (Unit.Level + 1));
            buttonIncLevel.Y = 220;
            buttonIncLevel.X = (Width - buttonIncLevel.Width) / 2;
            UpdateCaption();
        }

        public void UpdateCaption()
        {
            labelClass.Caption = PlayerUnit.GetClassString(Unit.Class);
            labelLevel.Caption = "Level : " + Unit.Level;
            labelStr.Caption = "Strength : " + Unit.Strength;
            labelInt.Caption = "Intellect : " + Unit.Intellect;
            labelLuck.Caption = "Luck : " + Unit.Luck;
            labelDmgType.Caption = "Damage Type : " + UnitDmg.GetDamageType(Unit.DmgType);

            labelNxLvlCost.Caption = "Next Level Cost : " + PlayerUnit.LevelCost(Unit.Level + 1) +"g";

            buttonIncLevel.Caption = "Increase to Level " + (Unit.Level + 1);
        }

        public Rectangle GetIncLvlRect()
        {
            return buttonIncLevel.GetRect(GetPoint());
        }

        public override Surface Render()
        {
            MousePos = Mouse.MousePosition;
            UpdateCaption();
            Surface Buffer = Background.Render();

            if (Unit.Class != UnitClasses.None)
            {
                labelClass.FromPoint(new Point((Width - labelClass.Width) / 2, 10));
                labelLevel.FromPoint(new Point((Width - labelLevel.Width) / 2, 40));
                labelStr.FromPoint(new Point((Width - labelStr.Width) / 2, 70));
                labelInt.FromPoint(new Point((Width - labelInt.Width) / 2, 100));
                labelLuck.FromPoint(new Point((Width - labelLuck.Width) / 2, 130));
                labelDmgType.FromPoint(new Point((Width - labelNxLvlCost.Width) / 2, 160));
                labelNxLvlCost.FromPoint(new Point((Width - labelNxLvlCost.Width) / 2, 190));


                Buffer.Blit(labelClass.Render(), labelClass.GetRect());
                Buffer.Blit(labelLevel.Render(), labelLevel.GetRect());
                Buffer.Blit(labelStr.Render(), labelStr.GetRect());
                Buffer.Blit(labelInt.Render(), labelInt.GetRect());
                Buffer.Blit(labelLuck.Render(), labelLuck.GetRect());
                Buffer.Blit(labelNxLvlCost.Render(), labelNxLvlCost.GetRect());

                if (buttonIncLevel.GetRect(GetPoint()).Contains(MousePos))
                {
                    Buffer.Blit(buttonIncLevel.RenderHighlight(), buttonIncLevel.GetRect());
                }
                else
                {
                    Buffer.Blit(buttonIncLevel.Render(), buttonIncLevel.GetRect());
                }

            }
            
            return Buffer;
        }

        public override Surface RenderHighlight()
        {
            return base.RenderHighlight();
        }

        public void ChangeUnit(PlayerUnit Unit)
        {
            this.Unit = Unit;
            UpdateCaption();
        }

    }

    public class UnitClassesChooser : GuiItem
    {
        private ButtonItem bSoldier;
        private ButtonItem bPaladin;
        private ButtonItem bArcher;
        private ButtonItem bMage;
        private ButtonItem bThieft;
        private ButtonItem bCancel;

        private WindowBar WinBar;

        private List<ButtonItem> Buttons;
        private BackgroundItem Background;

        private List<GuiItem> Garbage;

        private Point MousePos;
        private bool Drag = false;

        public UnitClasses ChoosenClass { get; private set; }

        public UnitClassesChooser(List<GuiItem> Garbage)
            : base("UnitClassesBox")
        {
            Buttons = new List<ButtonItem>();

            Width = 400;
            Height = 160;
            this.Garbage = Garbage;

            MousePos = new Point();

            ChoosenClass = UnitClasses.None;

            WinBar = new WindowBar("Choose a Class", Width);
            WinBar.CloseButton = false;

            Background = new BackgroundItem(Color.LimeGreen);
            Background.X = 0;
            Background.Y = 0;
            Background.Width = Width;
            Background.Height = Height;

            bSoldier = new ButtonItem("Soldier", 120, 30, "Soldier");
            bSoldier.X = 10;
            bSoldier.Y = 30;

            bPaladin = new ButtonItem("Paladin", 120, 30, "Paladin");
            bPaladin.X = 140;
            bPaladin.Y = 30;

            bArcher = new ButtonItem("Archer", 120, 30, "Archer");
            bArcher.X = 270;
            bArcher.Y = 30;

            bMage = new ButtonItem("Mage", 120, 30, "Mage");
            bMage.X = 10;
            bMage.Y = 70;

            bThieft = new ButtonItem("Thieft", 120, 30, "Thieft");
            bThieft.X = 140;
            bThieft.Y = 70;

            bCancel = new ButtonItem("Cancel",120,30,"Cancel");
            bCancel.X = (Width - bCancel.Width) / 2;
            bCancel.Y = 110;

            Buttons.Add(bSoldier);
            Buttons.Add(bPaladin);
            Buttons.Add(bArcher);
            Buttons.Add(bMage);
            Buttons.Add(bThieft);
            Buttons.Add(bCancel);
        }

        public override Surface RenderHighlight()
        {
            return Render();
        }

        public void Reset()
        {
            ChoosenClass = UnitClasses.None;
        }

        public override Surface Render()
        {
            Surface Buffer = new Surface(Width, Height);

            Buffer.Blit(Background.Render(), new Point(0, 0));

            foreach (ButtonItem b in Buttons)
            {
                Rectangle bRect = new Rectangle(new Point(b.X + X, b.Y + Y), new Size(b.Width, b.Height));
                Surface bRender;
                if (bRect.Contains(MousePos) && !MousePos.IsEmpty)
                {
                    bRender = b.RenderHighlight();
                }
                else
                {
                    bRender = b.Render();
                }

                Buffer.Blit(bRender, new Point(b.X, b.Y));
            }

            Buffer.Blit(WinBar.Render(), new Point(0, 0));

            Buffer.AlphaBlending = true;
            Buffer.Alpha = 200;

            return Buffer;
        }

        public void SetEvents()
        {
            Reset();
            Events.MouseButtonDown += new EventHandler<MouseButtonEventArgs>(this.MouseDown);
            Events.MouseButtonUp += new EventHandler<MouseButtonEventArgs>(this.MouseClick);
            Events.MouseMotion += new EventHandler<MouseMotionEventArgs>(this.MouseMotion);
        }

        public void UnsetEvents()
        {
            Events.MouseButtonDown -= new EventHandler<MouseButtonEventArgs>(this.MouseDown);
            Events.MouseButtonUp -= new EventHandler<MouseButtonEventArgs>(this.MouseClick);
            Events.MouseMotion -= new EventHandler<MouseMotionEventArgs>(this.MouseMotion);
        }

        public String GetButtonName(Point Click)
        {
            String Name = "none";
            foreach (ButtonItem b in Buttons)
            {
                Rectangle bRect = new Rectangle(new Point(b.X + X, b.Y + Y), new Size(b.Width, b.Height));
                if (bRect.Contains(Click))
                {
                    Name = b.ButtonName;
                }
            }

            return Name;
        }

        public void MouseDown(object sender, MouseButtonEventArgs args)
        {
            Rectangle wBar = new Rectangle(new Point(X, Y), new Size(WinBar.Width, WinBar.Height));

            if (wBar.Contains(args.Position))
            {
                Drag = true;
            }
        }

        public void MouseClick(object sender, MouseButtonEventArgs args)
        {
            Drag = false;
            String ItemName = GetButtonName(args.Position);

            if (ItemName == "Cancel")
            {
                UnsetEvents();
                Garbage.Add(this);
            }
            else if (ItemName == "Soldier")
            {
                ChoosenClass = UnitClasses.Soldier;
                UnsetEvents();
                Garbage.Add(this);
            }
            else if (ItemName == "Paladin")
            {
                ChoosenClass = UnitClasses.Paladin;
                UnsetEvents();
                Garbage.Add(this);
            }
            else if (ItemName == "Mage")
            {
                ChoosenClass = UnitClasses.Mage;
                UnsetEvents();
                Garbage.Add(this);
            }
            else if (ItemName == "Archer")
            {
                ChoosenClass = UnitClasses.Archer;
                UnsetEvents();
                Garbage.Add(this);
            }
            else if (ItemName == "Thieft")
            {
                ChoosenClass = UnitClasses.Thieft;
                UnsetEvents();
                Garbage.Add(this);
            }
        }

        public void MouseMotion(object sender, MouseMotionEventArgs args)
        {
            if (Drag)
            {
                int xdiff = MousePos.X - args.X;
                int ydiff = MousePos.Y - args.Y;
                X -= xdiff;
                Y -= ydiff;
            }
            MousePos = args.Position;

        }
    }

    //Will be deprecated soon... replaced by CreepNextWaveBox
    public class WaveInfoBox : GuiItem
    {
        public const int TIMEOUT = 120;

        public XmlCreepWave Wave { get; set; }
        private List<GuiItem> Garbage;

        private BackgroundItem Background;

        private LabelItem labelHealth;
        private LabelItem labelResist;
        private LabelItem labelWeakness;
        private LabelItem labelNumbers;
        private LabelItem labelName;
        private LabelItem labelTips;

        private LabelItem labelTitle;

        private Rectangle TimeBar;
        private Box TimeBarBorder;

        private int Tick = 0;

        public WaveInfoBox(XmlCreepWave Wave, List<GuiItem> Garbage)
            : base("WaveInfoBox")
        {
            this.Wave = Wave;
            this.Garbage = Garbage;
            Width = 300;
            Height = 300;

            Background = new BackgroundItem(Color.WhiteSmoke);
            Background.Width = Width;
            Background.Height = Height;

            TimeBar = new Rectangle(new Point(10,220),new Size(Width-20,22));
            TimeBarBorder = new Box(TimeBar.Location, TimeBar.Size);

            labelHealth = new LabelItem();
            labelResist = new LabelItem();
            labelWeakness = new LabelItem();
            labelNumbers = new LabelItem();
            labelName = new LabelItem();
            labelTips = new LabelItem();
            labelTitle = new LabelItem("Next Wave");
        }

        public void UpdateCaption()
        {
            labelHealth.Caption = "Health : " + Wave.Health;
            labelResist.Caption = "Resist : " + UnitDmg.GetDamageType(Wave.Resist);
            labelWeakness.Caption = "Weakness : " + UnitDmg.GetDamageType(Wave.Weakness);
            labelNumbers.Caption = "Numbers : " + Wave.Numbers;
            labelName.Caption = "Name : " + Wave.Name;
            labelTips.Caption = Wave.Tips;
        }

        public override Surface Render()
        {
            Tick++;
            Surface Buffer = Background.Render();

            Surface tmp = labelTitle.Render();
            Buffer.Blit(tmp, new Point((Width - tmp.Width) / 2, 10));

            tmp = labelName.Render();
            Buffer.Blit(tmp, new Point((Width - tmp.Width) / 2, 40));

            tmp = labelHealth.Render();
            Buffer.Blit(tmp, new Point((Width - tmp.Width) / 2, 70));

            tmp = labelTips.Render();
            Buffer.Blit(tmp, new Point((Width - tmp.Width) / 2, 100));

            tmp = labelResist.Render();
            Buffer.Blit(tmp, new Point((Width - tmp.Width) / 2, 130));

            tmp = labelWeakness.Render();
            Buffer.Blit(tmp, new Point((Width - tmp.Width) / 2, 160));

            tmp = labelNumbers.Render();
            Buffer.Blit(tmp, new Point((Width - tmp.Width) / 2, 190));


            /*Buffer.Fill(TimeBar, Color.Yellow);

            double pcent = ((double)Tick * (double)TimeBar.Width) / (double)TIMEOUT;

            Rectangle CurProgress = new Rectangle(TimeBar.Location, new Size((int)pcent,TimeBar.Height));
            Buffer.Fill(CurProgress, Color.Green);
            Buffer.Draw(TimeBarBorder, Color.Black);*/


            Buffer.AlphaBlending = true;
            Buffer.Alpha = 230;

            if (Tick == TIMEOUT)
            {
                Tick = 0;
                Garbage.Add(this);
            }

            return Buffer;
        }

    }

    public class CreepInfoBox : GuiItem
    {
        private BackgroundItem Background;

        private CreepUnit Unit;

        public CreepInfoBox() : base("CreepInfoBox")
        {
            Width = 185;
            Height = 50;

            Unit = new CreepUnit("None", 0, 0);

            Background = new BackgroundItem(Color.LemonChiffon);
            Background.Width = Width;
            Background.Height = Height;
        }

        public void ChangeUnit(CreepUnit Unit)
        {
            this.Unit = Unit;
        }

        public override Surface Render()
        {
            Surface Buffer = Background.Render();
            Surface Text = DefaultStyle.GetFont().Render("Creep Info", Color.Black);
            Buffer.Blit(Text, new Point((Width - Text.Width) / 2, 5));
            if(Unit.TotalHealth != 0 && Unit.Health >= 0 && !Unit.Position.IsEmpty)
            {
                Text = DefaultStyle.GetFont().Render("Life : "+Unit.Health+"/"+Unit.TotalHealth,Color.Black);

                Buffer.Blit(Text, new Point((Width - Text.Width) / 2, 25));
            }

            return Buffer;
        }
    }

}
