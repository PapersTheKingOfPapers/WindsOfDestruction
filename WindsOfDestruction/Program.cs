using WindsOfDestruction;
using Newtonsoft.Json;

JSONFighterReader rt = JsonConvert.DeserializeObject<JSONFighterReader>(File.ReadAllText(@".\fighterClasses.json"));
FightManager fm = new FightManager();

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
while (true)
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

    Unit ally = new Unit(name, statCall.baseDamage, statCall.baseHP, statCall.baseHPdepleteMultiplier, statCall.baseDamageMultiplier);

    fm.allies.Add(ally);
    Console.WriteLine($"Added {rt.fighters[fighterClass].fighterName} {name} to your Allies.");
    Console.WriteLine("--=====--");
}
#endregion

#region makeEnemies
int i = 0;
while (i < fm.allies.Count)
{
    Random random = new Random();
    int fighterClass = random.Next(rt.fighters.Count);

    var statCall = rt.fighters[fighterClass].stats;

    Unit enemy = new Unit(rt.fighters[fighterClass].fighterName, statCall.baseDamage, statCall.baseHP, statCall.baseHPdepleteMultiplier, statCall.baseDamageMultiplier);

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
    Console.WriteLine("--====--");
    Console.WriteLine("The enemies braces for an attack!");
    for (int x = 0; x < fm.allies.Count; x++)
    {
        Console.WriteLine($"{fm.allies[x].Name()}, ID: {x}");
    }
    Console.WriteLine("Choose your attacker! [ID]");
    int alliedAttacker = Convert.ToInt32(Console.ReadLine());

    for (int x = 0; x < fm.enemies.Count; x++)
    {
        Console.WriteLine($"{fm.enemies[x].Name()}, ID: {x}");
    }
    Console.WriteLine("Choose who to attack! [ID]");
    int enemyAttacked = Convert.ToInt32(Console.ReadLine());

    fm.Attack(fm.allies[alliedAttacker], fm.enemies[enemyAttacked]);

    if(fm.VictoryCheck() == true)
    {
        break;
    }

    #endregion

    #region enemyTurn

    Console.WriteLine("The enemy approaches for an attack!");

    int enemyAttacker = rnd.Next(fm.enemies.Count);
    int alliedAttacked = rnd.Next(fm.allies.Count);

    fm.Attack(fm.enemies[enemyAttacker],fm.allies[alliedAttacked]);

    if (fm.VictoryCheck() == true)
    {
        break;
    }
    #endregion
}
#endregion

Console.WriteLine("Press any key to close...");
Console.ReadLine();


