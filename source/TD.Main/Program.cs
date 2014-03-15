using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using SdlDotNet.Core;
using SdlDotNet.Input;
using SdlDotNet.Windows;
using SdlDotNet.Audio;
using SdlDotNet.Graphics;

using System.Xml;
using System.Xml.Serialization;

using System.IO;

using TD.Gui;
using TD.GameLogic;
using TD.Graphics;
using TD.Audio;
using TD.Core;

namespace TD.Main
{

    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {

            MidiIndex midis = MidiIndex.LoadMidiIndex("../../../../assets/Midi/Music.xml");
            try
            {
                List<Music> musiclist = midis.LoadMidiList();
                MusicPlayer.EnableMusicFinishedCallback();
                musiclist[0].Play();
            }
            catch (SdlException ex)
            {
                Console.WriteLine(ex.StackTrace);
            }

            //player.NextSong();

            Video.WindowCaption = "Tower Defense";
            Video.WindowIcon(new Icon("../../../../assets/Icon/TDMain.ico"));
            //Surface Screen = Video.SetVideoMode(900,700,false,false,false,true);
            Surface Screen = Video.SetVideoMode(900, 700, true);
            /*GameWindow win = new GameWindow(Screen);
            win.SetEvents();*/
            MainMenuWindow win = new MainMenuWindow(Screen);
            win.SetEvents();
            Events.Run();

            /*XmlEnemyList list = XmlEnemyList.LoadList("GameData/map3creep.xml");

            foreach (XmlCreepWave wave in list.Wave)
            {
                wave.GfxName = "creep1";
            }


            XmlEnemyList.SaveList(list, "GameData/map3creep.xml");*/

            /*CreepTextureIndex index = new CreepTextureIndex();

            CreepTextureEntry entry = new CreepTextureEntry();

            entry.Name = "creep1";

            entry.FrameUp.Add("creep1/Up0.png");
            entry.FrameUp.Add("creep1/Up1.png");
            entry.FrameUp.Add("creep1/Up2.png");
            entry.FrameUp.Add("creep1/Up3.png");

            entry.FrameDown.Add("creep1/Down0.png");
            entry.FrameDown.Add("creep1/Down1.png");
            entry.FrameDown.Add("creep1/Down2.png");
            entry.FrameDown.Add("creep1/Down3.png");

            entry.FrameLeft.Add("creep1/Left0.png");
            entry.FrameLeft.Add("creep1/Left1.png");
            entry.FrameLeft.Add("creep1/Left2.png");
            entry.FrameLeft.Add("creep1/Left3.png");

            entry.FrameRight.Add("creep1/Right0.png");
            entry.FrameRight.Add("creep1/Right1.png");
            entry.FrameRight.Add("creep1/Right2.png");
            entry.FrameRight.Add("creep1/Right3.png");

            index.Index.Add(entry);

            entry = new CreepTextureEntry();

            entry.Name = "creep2";

            entry.FrameUp.Add("creep2/Up0.png");
            entry.FrameUp.Add("creep2/Up1.png");
            entry.FrameUp.Add("creep2/Up2.png");
            entry.FrameUp.Add("creep2/Up3.png");

            entry.FrameDown.Add("creep2/Down0.png");
            entry.FrameDown.Add("creep2/Down1.png");
            entry.FrameDown.Add("creep2/Down2.png");
            entry.FrameDown.Add("creep2/Down3.png");

            entry.FrameLeft.Add("creep2/Left0.png");
            entry.FrameLeft.Add("creep2/Left1.png");
            entry.FrameLeft.Add("creep2/Left2.png");
            entry.FrameLeft.Add("creep2/Left3.png");

            entry.FrameRight.Add("creep2/Right0.png");
            entry.FrameRight.Add("creep2/Right1.png");
            entry.FrameRight.Add("creep2/Right2.png");
            entry.FrameRight.Add("creep2/Right3.png");

            index.Index.Add(entry);

            CreepTextureIndex.SaveIndex(index, "text.xml");*/

			/*
			Map m = Map.LoadMap("Maps/firstmap.xml");
			m.CreepListFile = "CreepList1.xml";
			Map.SaveMap(m, "Maps/firstmap.xml");*/

            /*CreepUnit unit;

            XmlEnemyList eList = new XmlEnemyList();

            unit = new CreepUnit("1", 150, 1);
            unit.Level = 1;
            unit.Tips = "No Tips";
            eList.Wave.Add(new XmlCreepWave(unit, 5));

            unit = new CreepUnit("2", 200, 1);
            unit.Level = 1;
            unit.Tips = "No Tips";
            eList.Wave.Add(new XmlCreepWave(unit, 5));

            unit = new CreepUnit("3", 275, 1);
            unit.Level = 1;
            unit.Tips = "No Tips";
            eList.Wave.Add(new XmlCreepWave(unit, 5));

            unit = new CreepUnit("4", 350, 1);
            unit.Level = 1;
            unit.Tips = "No Tips";
            eList.Wave.Add(new XmlCreepWave(unit, 5));

            unit = new CreepUnit("5", 450, 1);
            unit.Level = 1;
            unit.Tips = "No Tips";
            eList.Wave.Add(new XmlCreepWave(unit, 5));

            XmlSerializer Xs = new XmlSerializer(typeof(XmlEnemyList));
            TextWriter writer = new StreamWriter("GameData/CreepList1.xml");

            Xs.Serialize(writer, eList);

            writer.Close();*/

			/*CreepUnit c = new CreepUnit("Oui",800,1);
			XmlEnemyList l = new XmlEnemyList();
			l.Unit = c.ToXml();
			l.Numbers = 20;
			
			Serializer.SaveCreep(l,"test.xml");*/
			
            /*Map map = Map.LoadMap("Maps/secondmap.xml");

            MapCoordList list = Pathfinder.FindCreepPath(map);

			GameObject obj = new GameObject();
			obj.CreepPath = list;
			obj.map = map;
			obj.Creeps = new CreepUnitList(new CreepUnit("Test",1000,1),25);
			
            for (int i = 0; i < 1200; i++ )
            {
                obj.CreepMoveTick();
				obj.CheckCreepAtEnd();
            }


            Console.WriteLine(list);*/

            /*CreepUnit Monster = new CreepUnit("Test", 1000, 1);
            int total = 0;
            Thieft sol = new Thieft();
            sol.LevelUp();
            for (int i = 0; i < 100; i++)
            {
                AttackInfo info = sol.StealGold(Monster);
                total += info.Damage;

                Console.WriteLine("asd");
            }*/
         


            /*Video.WindowCaption = "HUD Test";
            Video.WindowIcon();
            Surface Screen = Video.SetVideoMode(600, 600, false);

            GuiWindow win = new GuiWindow("SDL Test", 400, 400, Screen);
            BackgroundItem Background = new BackgroundItem(Color.White);
            Background.Width = win.Width;
            Background.Height = win.Height;

            win.AddGuiItem(Background);

            ButtonItem b = new ButtonItem("1", 120, 22, "Button1");
            b.X = 10;
            b.Y = 25;
            win.AddGuiItem(b);

            b = new ButtonItem("2", 120, 22, "Button2");
            b.X = 200;
            b.Y = 25;
            win.AddGuiItem(b);


            b = new ButtonItem("4", 120, 22, "Button2");
            b.X = 200;
            b.Y = 66;
            win.AddGuiItem(b);

            MenuItem mi = new MenuItem("22", "MenuItem Yeah");
            mi.X = 200;
            mi.Y = 200;
            mi.Width = 120;
            mi.Height = 22;
            win.AddGuiItem(mi);




            DialogBox box = new DialogBox("Titre", "Mon message ici !");
            box.X = 20;
            box.Y = 120;
            box.SetEvents();

            DialogBox box2 = new DialogBox("Titre", "Mon message ici !");
            box2.X = 20;
            box2.Y = 320;
            box2.SetEvents();

            TextInputDialogBox box4 = new TextInputDialogBox("Enter Map Name", "Entrez un nom");

            win.AddGuiItem(box4);

            box4.X = 10;
            box4.Y = 160;
            box4.SetEvents();

            win.AddGuiItem(box);
            win.AddGuiItem(box2);

            ContextMenu Menu = new ContextMenu("Not Implemented Yet Baby");

            TextureIndex index = new TextureIndex();

            index.Add("Normal 1", "Textures/nor1.png");
            
            index.Add("CreepPath 1", "Textures/cp1.png");
            index.Add("CreepPath 2", "Textures/cp2.png");
            index.Add("CreepPath 3", "Textures/cp3.png");
            index.Add("CreepPath 4", "Textures/cp4.png");
            index.Add("CreepPath 5", "Textures/cp5.png");
            
            index.Add("Obstacle 1", "Textures/obs1.png");
            index.Add("Obstacle 2", "Textures/obs2.png");
            index.Add("Obstacle 3", "Textures/obs3.png");

            Serializer.SaveTextureIndex(index, "Textures/MapTiles.xml");

            /*MidiIndex index = new MidiIndex();

            index.Add("IntoBattle", "Midi/into_the_battle.mid");
            index.Add("TaleOfRevenge", "Midi/tale_of_revenge.mid");
            index.Add("HeroInADream", "Midi/hero_in_a_dream.mid");
            index.Add("BattleSong", "Midi/battle_song.mid");
            index.Add("HuntingSong", "Midi/hunting_song.mid");
            index.Add("BeerBeer", "Midi/beer_beer.mid");
            index.Add("Zeromus", "Midi/ff4_zeromus.mid");

            Serializer.SaveMidiIndex(index, "Midi/Music.xml");

            //MidiDictonary midi = index.LoadMidi();

            //midi["IntoBattle"].Play();

            Menu.X = 20;
            Menu.Y = 300;

            Menu.MenuItems.Add(new MenuItem("m2", "Allo 2"));
            Menu.MenuItems.Add(new MenuItem("me", "Allo 2 rtyt"));
            Menu.MenuItems.Add(new MenuItem("mr", "Allo 2 f ff"));
            Menu.MenuItems.Add(new MenuItem("mt", "Allo 2 fff"));

            Menu.FixAttribute();

            //Test Map

            Map m = new Map("MapTestingYeah");

            m.Tiles[0][0] = new Tile("Oui", TileType.CreepStart);
            m.Tiles[0][1] = new Tile("Oui1", TileType.Normal);
            m.Tiles[0][2] = new Tile("Oui2", TileType.CreepPath);
            m.Tiles[0][3] = new Tile("Oui3", TileType.Normal);
            m.Tiles[0][4] = new Tile("Oui4", TileType.Obstacle);
            m.Tiles[0][5] = new Tile("Oui5", TileType.Normal);
            m.Tiles[11][0] = new Tile("End", TileType.CreepEnd);
            Serializer.SaveMap(m, "map.xml");

            Mage mage = new Mage();
            mage.LevelUp();

            Soldier sol = new Soldier();
            sol.LevelUp();

            win.AddGuiItem(Menu);
            Menu.SetEvents();
            win.SetEvents();*/

        }
    }
}

