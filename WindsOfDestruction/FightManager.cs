using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindsOfDestruction
{
    public class FightManager
    {
        public List<Unit> allies = new List<Unit>();
        public List<Unit> enemies = new List<Unit>();

        public void Attack(Unit attacker, Unit attacked)
        {
            int hpBeforeAttack = attacked.CurrentHP();
            attacked.Damage(attacker.Attack());
            int attackDamage = hpBeforeAttack - attacked.CurrentHP();

            Console.WriteLine($"{attacker.Name()} attacked {attacked.Name()} for {attackDamage} HP");

            SurviveCheck(attacked);
        }

        public void SurviveCheck(Unit attacked)
        {
            if (attacked.CurrentHP() <= 0)
            {
                Console.WriteLine("!-==!==-!");
                Console.WriteLine($"{attacked.Name()} died.");
                Console.WriteLine("!-==!==-!");
                if (allies.Contains(attacked))
                {
                    allies.Remove(attacked);
                }
                else if (enemies.Contains(attacked))
                {
                    enemies.Remove(attacked);
                }
            }
        }
        public bool VictoryCheck()
        {
            if (enemies.Count == 0)
            {
                Victory("allied");
                return true;
            }
            else if (allies.Count == 0)
            {
                Victory("enemies");
                return true;
            }
            return false;
        }

        public void Victory(string teamName)
        {
            Console.WriteLine("--====--");
            Console.WriteLine(); // Empty Line


            Console.WriteLine($"The {teamName} won!");
        }
    }
}
