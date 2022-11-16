using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindsOfDestruction
{
    public static class UnitLists
    {
        public static List<Unit> allies = new List<Unit>();
        public static List<Unit> enemies = new List<Unit>();
        public static List<Unit> deadAllies = new List<Unit>();
        public static List<Unit> deadEnemies = new List<Unit>();
    }
    public class FightManager
    {
        object input = 0;
        int tempInt = 0;
        public void UpdateLists()
        {
            //allies to dead allies
            var alliedDead = UnitLists.allies.Where(unit => unit.AliveStatus() == false).ToList();
            alliedDead.ForEach(item => UnitLists.allies.Remove(item));
            UnitLists.deadAllies.AddRange(alliedDead);

            //Dead allies to allies
            var alliedAlive = UnitLists.deadAllies.Where(unit => unit.AliveStatus() == true).ToList();
            alliedAlive.ForEach(item => UnitLists.deadAllies.Remove(item));
            UnitLists.allies.AddRange(alliedAlive);

            //enemies to Dead enemies
            var enemiesDead = UnitLists.enemies.Where(unit => unit.AliveStatus() == false).ToList();
            enemiesDead.ForEach(item => UnitLists.enemies.Remove(item));
            UnitLists.deadEnemies.AddRange(enemiesDead);

            //dead enemies to Enemies
            var enemiesAlive = UnitLists.deadEnemies.Where(unit => unit.AliveStatus() == true).ToList();
            enemiesAlive.ForEach(item => UnitLists.deadEnemies.Remove(item));
            UnitLists.enemies.AddRange(enemiesAlive);
        }

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
                attacked.KillUnit();
            }
        }
        public bool VictoryCheck()
        {
            UpdateLists();
            if (UnitLists.enemies.Count == 0)
            {
                Victory("allied");
                return true;
            }
            else if (UnitLists.allies.Count == 0)
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
        public void PrintTeam(List<Unit> team)
        {
            for (int x = 0; x < team.Count; x++)
            {
                if (team[x].AliveStatus() == true)
                {
                    double currentHP = team[x].CurrentHP();
                    double temp = currentHP / team[x].MaxHP() * 100;
                    int healthPercent = Convert.ToInt32(Math.Ceiling(temp));
                    Console.WriteLine($"{team[x].Name()}, ID: {x}");
                    ValueBar.WriteProgressBar(healthPercent, ConsoleColor.DarkYellow);
                    Console.WriteLine($" {currentHP} / {team[x].MaxHP()}");
                }
            }
        }

        //Special Attacks
        public void AttackDeployer(Unit unit, int attackIndex)
        {
            int attackType = unit._specialAttacks[attackIndex].attackType;
            if(unit._specialAttacks[attackIndex].currentCoolDown == 0)
            {
                switch (attackType)
                {
                    case 1: //Area Attack
                        AreaAttack(unit, attackIndex);
                        break;
                    case 2: //Shield Attack
                        ShieldAttack(unit, attackIndex);
                        break;
                    case 3: //Extra Damage for Next turn
                        ExtraDamageAttack(unit, attackIndex);
                        break;
                    case 4: //Invincible
                        Invincible(unit, attackIndex);
                        break;
                }
            }
            else
            {
                if(unit._specialAttacks[attackIndex].currentPersistTime != 0)
                {
                    Console.WriteLine("This status is already active!");
                }
                else
                {
                    Console.WriteLine("This attack is in cooldown!");
                }
            }
        }
        public void ExtraDamageAttack(Unit unit, int attackIndex)
        {
            unit.ChangeDamageMultiplier(unit._specialAttacks[attackIndex].Stat1);
            if (UnitLists.allies.Contains(unit))
            {
                PrintTeam(UnitLists.enemies);
                Console.WriteLine("Choose who to attack! [ID]");

                int enemyAttacked = 0;
                Console.WriteLine();

                Attack(unit, UnitLists.enemies[enemyAttacked]);
            }
            else if (UnitLists.enemies.Contains(unit))
            {
                Random rnd = new Random();
                int ally = rnd.Next(UnitLists.allies.Count);
                Attack(unit, UnitLists.allies[ally]);
            }
            unit.ResetDamageMultiplier();
            unit._specialAttacks[attackIndex].currentCoolDown = unit._specialAttacks[attackIndex].attackCoolDown + 1;
        }
        public void ShieldAttack(Unit unit, int attackIndex)
        {
            unit.EnableShields();

            unit.SetCounterDamage(unit._specialAttacks[attackIndex].Stat1);
            unit.ChangeDefenceMultiplier(unit._specialAttacks[attackIndex].Stat2);

            unit._specialAttacks[attackIndex].currentPersistTime = unit._specialAttacks[attackIndex].persistTime + 1;
            unit._specialAttacks[attackIndex].currentCoolDown = unit._specialAttacks[attackIndex].attackCoolDown + 1;
        }
        public void AreaAttack(Unit unit, int attackIndex)
        {
            unit.ChangeDamage(Convert.ToSingle(unit._specialAttacks[attackIndex].Stat1));
            if (UnitLists.allies.Contains(unit))
            {
                foreach(Unit enemy in UnitLists.enemies)
                {
                    Attack(unit, enemy);
                }
            }
            else if (UnitLists.enemies.Contains(unit))
            {
                foreach (Unit ally in UnitLists.allies)
                {
                    Attack(unit, ally);
                }
            }
            unit.ResetDamage();
            unit._specialAttacks[attackIndex].currentCoolDown = unit._specialAttacks[attackIndex].attackCoolDown + 1;
        }
        public void Invincible(Unit unit, int attackIndex)
        {
            unit.ChangeDefenceMultiplier(unit._specialAttacks[attackIndex].Stat1);

            unit._specialAttacks[attackIndex].currentPersistTime = unit._specialAttacks[attackIndex].persistTime + 1;
            unit._specialAttacks[attackIndex].currentCoolDown = unit._specialAttacks[attackIndex].attackCoolDown + 1;
        }
    }
}
