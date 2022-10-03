using WindsOfDestruction;
using Newtonsoft.Json;
using System.Reflection.Emit;
using System.Linq.Expressions;

Console.ForegroundColor = SystemExtension.defaultForegroundColor;
Console.BackgroundColor = SystemExtension.defaultBackgroundColor;

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
    int fighterClass = Convert.ToInt32(Console.ReadKey().KeyChar.ToString());
    try
    {
        int c = rt.fighters[fighterClass].id;
    }
    catch (IndexOutOfRangeException)
    {
        fighterClass = 0;
    }
    Console.WriteLine();
    if (!rt.fighters.Any(item=>item.id == fighterClass))
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
while (true) //Battle Loop
{
    ActionLogWriterAndReader.WriteLogTest(fm.allies);
    Random rnd = new Random();
    #region playerTurn
    TurnType();
    if (fm.VictoryCheck() == true)
    {
        break; //Ends battle
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
        break; //Ends battle
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
    int turnChoice = Convert.ToInt32(Console.ReadKey().KeyChar.ToString());
    try
    {
        int c = turnChoice;
    }
    catch (IndexOutOfRangeException)
    {
        turnChoice = 0;
    }
    Console.Clear();
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
    AttackTurnStart:
    Console.WriteLine("--!!--");
    Console.WriteLine("The enemies braces for an attack!");
    fm.PrintTeam(fm.allies);
    Console.WriteLine("Choose your attacker! [ID]");
    int alliedAttacker = Convert.ToInt32(Console.ReadKey().KeyChar.ToString());

    try
    {
        int c = fm.allies[alliedAttacker].CurrentHP();
    }
    catch (IndexOutOfRangeException)
    {
        alliedAttacker = 0;
    }

    Console.WriteLine();
    fm.PrintTeam(fm.enemies);
    Console.WriteLine("Choose who to attack! [ID]");
    int enemyAttacked = Convert.ToInt32(Console.ReadKey().KeyChar.ToString());

    try
    {
        int c = fm.allies[enemyAttacked].CurrentHP();
    }
    catch (IndexOutOfRangeException)
    {
        enemyAttacked = 0;
    }

    Console.WriteLine();

    if (SystemExtension.UndoCheck())
    {
        Console.Clear();
        goto AttackTurnStart;
    }

    fm.Attack(fm.allies[alliedAttacker], fm.enemies[enemyAttacked]);
}

void SpecialAttackTurn()
{
    SpecialAttackTurnStart:
    Console.WriteLine("--!!--");
    Console.WriteLine("The enemies braces for an attack!");
    fm.PrintTeam(fm.allies);
    Console.WriteLine("Choose your attacker! [ID]");
    int alliedAttacker = Convert.ToInt32(Console.ReadKey().KeyChar.ToString());
    Console.WriteLine();
    Console.WriteLine("What will they do!");

    try
    {
        int c = fm.allies[alliedAttacker].CurrentHP();
    }
    catch (IndexOutOfRangeException)
    {
        alliedAttacker = 0;
    }

    Console.WriteLine();
    if(fm.allies[alliedAttacker]._specialAttacks == null)
    {
        Console.Clear();
        Console.WriteLine("This character has no special attacks!");
        return;
    }
    fm.allies[alliedAttacker].PrintSpecialAttacks();
    int specialAttackIndex = Convert.ToInt32(Console.ReadKey().KeyChar.ToString());

    try
    {
        int c = fm.allies[alliedAttacker]._specialAttacks[specialAttackIndex].attackType;
    }
    catch (IndexOutOfRangeException)
    {
        specialAttackIndex = 0;
    }

    Console.WriteLine();

    if (SystemExtension.UndoCheck())
    {
        Console.Clear();
        goto SpecialAttackTurnStart;
    }

    fm.AttackDeployer(fm.allies[alliedAttacker], specialAttackIndex);
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


