using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WindsOfDestruction
{
    public static class ActionLogWriterAndReader
    {
        public static int roundID = 0;
        public static bool loadRoundToggled = false;
        public static void WriteLogTest(List<Unit> allies)
        {
            List<List<Unit>> units = new List<List<Unit>>();
            units.Add(allies);
            var json = JsonConvert.SerializeObject(units.ToArray());

            System.IO.File.WriteAllText(@".\ActionLog.json", json);
        }

        public static void WriteTeamStatusLog(List<Unit> allies, List<Unit> deadAllies, List<Unit> enemies, List<Unit> deadEnemies)
        {
            Turns list = JsonConvert.DeserializeObject<Turns>(File.ReadAllText(@".\ActionLog.json"));
            Debug.Assert(list.turnContents != null);

            TurnContent turnContent = new TurnContent();
            turnContent.turnID = roundID;
            turnContent.allies = allies;
            turnContent.deadAllies = deadAllies;
            turnContent.enemies = enemies;
            turnContent.deadEnemies = deadEnemies;

            list.turnContents.Add(turnContent);
            //list.Add(turnContent);

            var json = JsonConvert.SerializeObject(list);

            System.IO.File.WriteAllText(@".\ActionLog.json", json);
        }
        public static void ResetLog()
        {
            Turns turns = new Turns();



            var json = JsonConvert.SerializeObject(turns);

            System.IO.File.WriteAllText(@".\ActionLog.json", json);
        }
    }
    public class Turns
    {
        public List<TurnContent> turnContents { get; set; }
        public Turns()
        {
            this.turnContents = new List<TurnContent>();
        }
    }

    public class TurnContent
    {
        public int turnID { get; set; }
        public List<Unit> allies { get; set; }
        public List<Unit> deadAllies { get; set; }
        public List<Unit> enemies { get; set; }
        public List<Unit> deadEnemies { get; set; }
    }
}