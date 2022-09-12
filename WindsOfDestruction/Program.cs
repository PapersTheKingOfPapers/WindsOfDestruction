using WindsOfDestruction;
using Newtonsoft.Json;

JSONFighterReader rt = JsonConvert.DeserializeObject<JSONFighterReader>(File.ReadAllText(@".\fighterClasses.json"));
FightManager fm = new FightManager();

#region Variables

int z = 0;

#endregion

#region preBattle
Console.WriteLine("Possible unit classes:");
//Prints each enemy
foreach (Fighter fighter in rt.fighters)
{
    Console.WriteLine($"{fighter.fighterName}, ID: {fighter.id}");
}
#region makeUnits

#region makeAllies
Console.WriteLine("Make your team:");
Console.WriteLine(); //Empty Line
while (true || z < 9)
{
    Console.WriteLine("Warrior's name? (Empty to start fight)");
    string name = Console.ReadLine();
    if (fm.allies.Count <= 0 && name == "")
    {
        name = "NULL";
    }
    else if(name == "")
    {
        break;
    }
    Console.WriteLine("Warrior's class' id?");
    string fighterClassString = Console.ReadLine();
    int fighterClass = 0;
    try
    {
        fighterClass = Convert.ToInt32(fighterClassString);
    }
    catch(FormatException)
    {
        fighterClass = 0;
    }

    if(!rt.fighters.Any(item=>item.id == fighterClass))
    {
        fighterClass = 0;
    }
    var statCall = rt.fighters[fighterClass].stats;

    Unit ally = new Unit(name, statCall.baseDamage, statCall.baseHP, statCall.baseHPdepleteMultiplier, statCall.baseDamageMultiplier, rt.fighters[fighterClass].specialAttacks);

    fm.allies.Add(ally);
    Console.WriteLine($"Added {rt.fighters[fighterClass].fighterName} {name} to your Allies.");
    Console.WriteLine("--=====--");
    z++;
}
#endregion

#region makeEnemies
int i = 0;
while (i < (fm.allies.Count))
{
    Random random = new Random();
    int fighterClass = random.Next(rt.fighters.Count);
    var statCall = rt.fighters[fighterClass].stats;

    Unit enemy = new Unit(rt.fighters[fighterClass].fighterName, statCall.baseDamage, statCall.baseHP, statCall.baseHPdepleteMultiplier, statCall.baseDamageMultiplier, rt.fighters[fighterClass].specialAttacks);

    fm.enemies.Add(enemy);
    i++;
}
#endregion

#endregion

#endregion

#region Battle
while (true)
{
    Random rnd = new Random();
    #region playerTurn

    TurnType();

    if (fm.VictoryCheck() == true)
    {
        break;
    }
    UpdateCooldowns(fm.allies);

    #endregion

    #region enemyTurn

    Console.WriteLine("--!!--");
    Console.WriteLine("The enemy approaches for an attack!");

    int enemyAttacker = rnd.Next(fm.enemies.Count);
    int alliedAttacked = rnd.Next(fm.allies.Count);

    fm.Attack(fm.enemies[enemyAttacker],fm.allies[alliedAttacked]);

    if (fm.VictoryCheck() == true)
    {
        break;
    }
    UpdateCooldowns(fm.enemies);
    #endregion
}
#endregion

#region Methods

void TurnType()
{
    Console.WriteLine("--!!--");
    Console.WriteLine("What will you do!"); // Choose what the fuck yo gonna do
    Console.WriteLine("0: Attack!");
    Console.WriteLine("1: Use a special action!");
    string turnChoiceString = Console.ReadLine();
    int turnChoice = 0;
    try
    {
        turnChoice = Convert.ToInt32(turnChoiceString);
    }
    catch (FormatException)
    {
        turnChoice = 0;
    }

    switch (turnChoice)
    {
        default:
            AttackTurn();
            break;
        case 0:
            AttackTurn();
            break;
        case 1:
            SpecialAttackTurn();
            break;
    }

}

void AttackTurn()
{
    Console.WriteLine("--!!--");
    Console.WriteLine("The enemies braces for an attack!");
    PrintTeam(fm.allies);
    Console.WriteLine("Choose your attacker! [ID]");
    int alliedAttacker = Convert.ToInt32(Console.ReadLine());
    PrintTeam(fm.enemies);
    Console.WriteLine("Choose who to attack! [ID]");
    int enemyAttacked = Convert.ToInt32(Console.ReadLine());
    fm.Attack(fm.allies[alliedAttacker], fm.enemies[enemyAttacked]);
}

void SpecialAttackTurn()
{
    Console.WriteLine("--!!--");
    Console.WriteLine("The enemies braces for an attack!");
    PrintTeam(fm.allies);
    Console.WriteLine("Choose your attacker! [ID]");
    int alliedAttacker = Convert.ToInt32(Console.ReadLine());
    Console.WriteLine("What will they do!");
    fm.allies[alliedAttacker].PrintSpecialAttacks();
    int specialAttackIndex = Convert.ToInt32(Console.ReadLine());
    fm.AttackDeployer(fm.allies[alliedAttacker], specialAttackIndex);
}

void PrintTeam(List<Unit> team)
{
    for (int x = 0; x < team.Count; x++)
    {
        double currentHP = team[x].CurrentHP();
        double temp = currentHP / team[x].MaxHP() * 100;
        int healthPercent = Convert.ToInt32(Math.Ceiling(temp));
        Console.WriteLine($"{team[x].Name()}, ID: {x}");
        ValueBar.WriteProgressBar(healthPercent);
        Console.WriteLine($" {currentHP} / {team[x].MaxHP()}");
    }
}

void UpdateCooldowns(List<Unit> team)
{
    Console.WriteLine("--!!-- STATUS ALERT! --!!--");
    foreach (Unit unit in team)
    {
        unit.UpdateCoolDowns();
    }
}

#endregion

Console.WriteLine("Press any key to close...");
Console.ReadLine();


