using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindsOfDestruction
{
    public class JSONFighterReader
    {
        public List<Fighter> fighters { get; set; }
    }

    public class Fighter
    {
        public string fighterName { get; set; }
        public int id { get; set; }
        public Stats stats { get; set; }
    }

    public class Stats
    {
        public float baseDamage { get; set; }
        public int baseHP { get; set; }
        public double baseHPdepleteMultiplier { get; set; }
        public double baseDamageMultiplier { get; set; }

    }
}
