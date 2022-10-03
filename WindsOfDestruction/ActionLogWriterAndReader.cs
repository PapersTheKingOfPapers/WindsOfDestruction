using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WindsOfDestruction
{
    public static class ActionLogWriterAndReader
    {
        public static void WriteLogTest(List<Unit> allies)
        {
            List<List<Unit>> units = new List<List<Unit>>();
            units.Add(allies);
            var json = JsonConvert.SerializeObject(units.ToArray());

            System.IO.File.WriteAllText(@".\ActionLog.json", json);
        }

        public static void WriteAttackLog(Unit attacked, Unit attacker, int hpBeforeAttack, int attackDamage)
        {
            var list = JsonConvert.DeserializeObject<List<Unit>>(File.ReadAllText(@".\fighterClasses.json"));
            
        }
    }
}