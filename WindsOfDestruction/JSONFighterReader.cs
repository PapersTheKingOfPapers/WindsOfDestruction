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
        public List<SpecialAttack> specialAttacks { get; set; }
    }

    public class Stats
    {
        public float baseDamage { get; set; }
        public int baseHP { get; set; }
        public double baseHPdepleteMultiplier { get; set; }
        public double baseDamageMultiplier { get; set; }

    }

    public class SpecialAttack
    {
        public string attackName { get; set; }
        public string attackDescription { get; set; }
        public int attackType { get; set; }
        public AttackStats attackStats { get; set; }
    }

    public class AttackStats
    {
        public int currentCoolDown { get; set; }
        public int attackCoolDown { get; set; }
        public int currentPersistTime { get; set; }
        public int persistTime { get; set; }
        public double Stat1 { get; set; }
        public double Stat2 { get; set; }
    }
}
