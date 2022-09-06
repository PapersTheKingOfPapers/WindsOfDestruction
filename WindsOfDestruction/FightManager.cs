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
            if (attacked.ShieldsStatus())
            {
                int hpBeforeAttack = attacker.CurrentHP();
                attacked.ChangeDamage(Convert.ToSingle(attacked.CounterDamageValue()));
                attacker.Damage(attacked.Attack());
                attacked.ResetDamage();
                int attackDamage = hpBeforeAttack - attacker.CurrentHP();

                Console.WriteLine($"{attacker.Name()} tried to attack {attacked.Name()}, but failed!");
                Console.WriteLine($"{attacked.Name()}'s shields caused {attackDamage} HPs worth of damage to {attacker.Name()}");

                SurviveCheck(attacker);
            }
            else
            {
                int hpBeforeAttack = attacked.CurrentHP();
                attacked.Damage(attacker.Attack());
                int attackDamage = hpBeforeAttack - attacked.CurrentHP();

                Console.WriteLine($"{attacker.Name()} attacked {attacked.Name()} for {attackDamage} HP");

                SurviveCheck(attacked);
            }
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

        //Special Attacks
        public void AttackDeployer(Unit unit, int attackIndex)
        {
            int attackType = unit._specialAttacks[attackIndex].attackType;
            if(unit._specialAttacks[attackIndex].attackStats.currentCoolDown == 0)
            {
                switch (attackType)
                {
                    case 1:
                        AreaAttack(unit, attackIndex);
                        break;
                    case 2:
                        ShieldAttack(unit, attackIndex);
                        break;
                    case 3:
                        ExtraDamageAttack(unit);
                        break;
                }
            }
            else
            {
                Console.WriteLine("This attack is in cooldown!");
            }
        }
        public void ExtraDamageAttack(Unit unit)
        {

        }
        public void ShieldAttack(Unit unit, int attackIndex)
        {
            unit.EnableShields();

            unit.SetCounterDamage(unit._specialAttacks[attackIndex].attackStats.Stat1);
            unit.ChangeDefenceMultiplier(unit._specialAttacks[attackIndex].attackStats.Stat2);

            unit._specialAttacks[attackIndex].attackStats.currentPersistTime = unit._specialAttacks[attackIndex].attackStats.persistTime + 1;
            unit._specialAttacks[attackIndex].attackStats.currentCoolDown = unit._specialAttacks[attackIndex].attackStats.attackCoolDown + 1;
        }
        public void AreaAttack(Unit unit, int attackIndex)
        {
            unit.ChangeDamage(Convert.ToSingle(unit._specialAttacks[attackIndex].attackStats.Stat1));
            if (allies.Contains(unit))
            {
                foreach(Unit enemy in enemies)
                {
                    Attack(unit, enemy);
                }
            }
            else if (enemies.Contains(unit))
            {
                foreach (Unit ally in allies)
                {
                    Attack(unit, ally);
                }
            }
            unit.ResetDamage();
            unit._specialAttacks[attackIndex].attackStats.currentCoolDown = unit._specialAttacks[attackIndex].attackStats.attackCoolDown + 1;
        }
    }
}
