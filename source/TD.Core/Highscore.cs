using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

using System.Xml;
using System.Xml.Serialization;

namespace TD.Core
{
    public class ScoreEntry
    {
        [XmlAttribute("Name")] public String PlayerName { get; set; }
        [XmlAttribute("Difficulty")] public GameDifficulty Difficulty { get; set; }
        [XmlAttribute("Map")] public String MapName { get; set; }
        [XmlAttribute("Wave")] public int Wave { get; set; }
        [XmlAttribute("Score")] public int Score { get; set; }

        public ScoreEntry()
        {
            PlayerName = "None";
            Difficulty = GameDifficulty.Easy;
            MapName = "None";
            Score = -1;
            Wave = -1;
        }

        public ScoreEntry(String PlayerName, int Score, int Wave, GameDifficulty Difficulty, String MapName)
        {
            this.PlayerName = PlayerName;
            this.Score = Score;
            this.MapName = MapName;
            this.Difficulty = Difficulty;
            this.Wave = Wave;
        }
    }

    public class ScoreList
    {
        [XmlArray("ScoreEntries")] public List<ScoreEntry> ScoreEntries { get; set; }
        
        public ScoreList()
        {
            ScoreEntries = new List<ScoreEntry>();
        }

        public static void SaveScoreList(ScoreList List, String Path)
        {
            XmlSerializer Xs = new XmlSerializer(typeof(ScoreList));

            TextWriter writer = new StreamWriter(Path);
            Xs.Serialize(writer, List);

            writer.Close();
        }

        public static ScoreList LoadScoreList(String Path)
        {

            ScoreList List = new ScoreList();
            XmlSerializer Xs = new XmlSerializer(typeof(ScoreList));
            
            if (File.Exists(Path))
            {
                TextReader reader = new StreamReader(Path);
                List = (ScoreList)Xs.Deserialize(reader);

                reader.Close();
            }

            return List;
        }

        public void OrderDesc()
        {
            List<ScoreEntry> inOrder = new List<ScoreEntry>();

            while (ScoreEntries.Count != 0)
            {
                ScoreEntry entry = GetHighestScore();

                if (entry.Score != -1)
                {
                    inOrder.Add(entry);
                    ScoreEntries.Remove(entry);
                }
            }

            ScoreEntries.Clear();
            ScoreEntries.AddRange(inOrder);

        }

        public ScoreList TrimList(String MapName, GameDifficulty Difficulty)
        {
            ScoreList Trimed = new ScoreList();


            foreach(ScoreEntry Entry in ScoreEntries)
            {
                if (Entry.MapName == MapName && Entry.Difficulty == Difficulty)
                {
                    Trimed.ScoreEntries.Add(Entry);
                }
            }

            return Trimed;
        }

        public void KeepTop10()
        {
            OrderDesc();

            for (int i = 9; i < ScoreEntries.Count; i++)
            {
                ScoreEntries.RemoveAt(i);
            }
        }

        public ScoreEntry GetHighestScore()
        {
            ScoreEntry Score = new ScoreEntry();

            int Highest = 0;
            int Index = -1;

            for (int i = 0; i < ScoreEntries.Count; i++)
            {
                if (ScoreEntries[i].Score > Highest)
                {
                    Highest = ScoreEntries[i].Score;
                    Index = i;
                }
            }

            if (Index != -1)
            {
                Score = ScoreEntries[Index];
            }

            return Score;
        }
    }


}
