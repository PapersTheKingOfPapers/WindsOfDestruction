using WindsOfDestruction;
using Newtonsoft.Json;
using System.Reflection.Emit;
using System.Linq.Expressions;
using System.Collections;
using System.Globalization;
using Microsoft.CSharp.RuntimeBinder;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;

Console.ForegroundColor = SystemExtension.defaultForegroundColor;
Console.BackgroundColor = SystemExtension.defaultBackgroundColor;
//Console.TreatControlCAsInput = true;

JSONFighterReader rt = JsonConvert.DeserializeObject<JSONFighterReader>(File.ReadAllText(@".\fighterClasses.json"));
FightManager fm = new FightManager();

#region Variables

int z = 0;

object input = 0;

int tempInt = 0;

Int32 number;

#endregion

#region MainMenu
Console.WriteLine("======================================");
Console.WriteLine("Winds of Destruction");
Console.WriteLine("");
Console.WriteLine("Press any key to start!");
Console.WriteLine("Press CTRL+Z to exit!");
Console.WriteLine("");
Console.WriteLine("======================================");

#region InputCheck
SystemExtension.cki = Console.ReadKey(true);
input = SystemExtension.Input<int>();
if (input.GetType() == typeof(string))
{
    if (input == "UNDO")
    {
        Environment.Exit(0);
    }
}
#endregion

Console.Clear();
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
while (true || z < 10)
{
    Console.WriteLine("Warrior's name? (Empty to start fight)");
    string name = Console.ReadLine();
    if (UnitLists.allies.Count <= 0 && name == "")
    {
        name = "NULL";
    }
    else if(name == "")
    {
        break;
    }
    Console.WriteLine("Warrior's class' id?");
    #region InputCheck
    SystemExtension.cki = Console.ReadKey(true);
    input = SystemExtension.Input<int>();
    int fighterClass = 0;
    //Int Check
    if (!Char.IsNumber(SystemExtension.cki.KeyChar))
    {
        input = 0;
    }
    else
    {
        fighterClass = Convert.ToInt32(input);
    }
    //Index inbound check
    try
    {
        int c = rt.fighters[fighterClass].id;
    }
    catch (IndexOutOfRangeException)
    {
        fighterClass = 0;
    }
    #endregion

    Console.WriteLine();
    if (!rt.fighters.Any(item=>item.id == fighterClass))
    {
        fighterClass = 0;
    }
    var statCall = rt.fighters[fighterClass].stats;

    Unit ally = new Unit(name, statCall.baseDamage, statCall.baseHP, statCall.baseHPdepleteMultiplier, statCall.baseDamageMultiplier, rt.fighters[fighterClass].specialAttacks);

    UnitLists.allies.Add(ally);
    Console.WriteLine($"Added {rt.fighters[fighterClass].fighterName} {name} to your Allies.");
    Console.WriteLine("--=====--");
    z++;
}
#endregion

#region makeEnemies
int i = 0;
while (i < (UnitLists.allies.Count))
{
    Random random = new Random();
    int fighterClass = random.Next(rt.fighters.Count);
    var statCall = rt.fighters[fighterClass].stats;

    Unit enemy = new Unit(rt.fighters[fighterClass].fighterName, statCall.baseDamage, statCall.baseHP, statCall.baseHPdepleteMultiplier, statCall.baseDamageMultiplier, rt.fighters[fighterClass].specialAttacks);

    UnitLists.enemies.Add(enemy);
    i++;
}
#endregion

#endregion

#endregion

#region Battle
ActionLogWriterAndReader.ResetLog();
while (true) //Battle Loop
{
    #region ALWAR
    RoundStart:
    ActionLogWriterAndReader.roundID++;
    ActionLogWriterAndReader.WriteTeamStatusLog(UnitLists.allies, UnitLists.deadAllies, UnitLists.enemies, UnitLists.deadEnemies);
    RestartRound:
    if(ActionLogWriterAndReader.loadRoundToggled == true && ActionLogWriterAndReader.roundID > 1)
    {
        ActionLogWriterAndReader.LoadTeamStatusLog();
        Console.Clear();
        ActionLogWriterAndReader.loadRoundToggled = false;
    }
    else if(ActionLogWriterAndReader.loadRoundToggled == true && ActionLogWriterAndReader.roundID == 1)
    {
        ActionLogWriterAndReader.LoadTeamStatusBase();
        Console.Clear();
        ActionLogWriterAndReader.loadRoundToggled = false;
    }
    #endregion
    Random rnd = new Random();
    Console.WriteLine("Current Round ID: " + ActionLogWriterAndReader.roundID);
    #region playerTurn

    Console.WriteLine("--!!--");
    Console.WriteLine("What will you do!"); // Choose what the fuck yo gonna do
    Console.WriteLine("0: Attack!");
    Console.WriteLine("1: Use a special action!");
    Console.WriteLine("CTRL + Z: Undo Possible in this section!");
    #region InputCheck
    SystemExtension.cki = Console.ReadKey(true);
    input = SystemExtension.Input<int>();
    if(input.GetType() == typeof(string))
    {
        if (input == "UNDO")
        {
            ActionLogWriterAndReader.loadRoundToggled = true;
            goto RestartRound;
            //goto RoundStart;
        }
    }
    int turnChoice = 0;
    //Int Check
    if (!Char.IsNumber(SystemExtension.cki.KeyChar))
    {
        input = 0;
    }
    else
    {
        turnChoice = Convert.ToInt32(input);
    }
    #endregion
    Console.Clear();
    switch (turnChoice)
    {
        default:
        case 0:
            Console.WriteLine("--!!--");
            Console.WriteLine("The enemies braces for an attack!");
            fm.PrintTeam(UnitLists.allies);
            Console.WriteLine("Choose your attacker! [ID]");

            #region InputCheck
            SystemExtension.cki = Console.ReadKey(true);
            input = SystemExtension.Input<int>();
            int alliedAttacker = 0;
            //Int Check
            if (!Char.IsNumber(SystemExtension.cki.KeyChar))
            {
                input = 0;
            }
            else
            {
                alliedAttacker = Convert.ToInt32(input);
            }
            //Index too big check
            try
            {
                tempInt = UnitLists.allies[alliedAttacker].CurrentHP();
            }
            catch (IndexOutOfRangeException)
            {
                alliedAttacker = 0;
            }
            #endregion

            Console.WriteLine();
            fm.PrintTeam(UnitLists.enemies);
            Console.WriteLine("Choose who to attack! [ID]");

            #region InputCheck
            SystemExtension.cki = Console.ReadKey(true);
            input = SystemExtension.Input<int>();
            int enemyAttacked = 0;
            //Int Check
            if (!Char.IsNumber(SystemExtension.cki.KeyChar))
            {
                input = 0;
            }
            else
            {
                enemyAttacked = Convert.ToInt32(input);
            }
            //Index too big check
            try
            {
                tempInt = UnitLists.enemies[enemyAttacked].CurrentHP();
            }
            catch (IndexOutOfRangeException)
            {
                enemyAttacked = 0;
            }
            #endregion
            Console.WriteLine();

            fm.Attack(UnitLists.allies[alliedAttacker], UnitLists.enemies[enemyAttacked]);
            break;
        case 1:
            Console.WriteLine("--!!--");
            Console.WriteLine("The enemies braces for an attack!");
            fm.PrintTeam(UnitLists.allies);
            Console.WriteLine("Choose your attacker! [ID]");

            #region InputCheck
            SystemExtension.cki = Console.ReadKey(true);
            input = SystemExtension.Input<int>();
            int alliedAttacker1 = 0;
            //Int Check
            if (!Char.IsNumber(SystemExtension.cki.KeyChar))
            {
                input = 0;
            }
            else
            {
                alliedAttacker1 = Convert.ToInt32(input);
            }
            //Index too big check
            try
            {
                tempInt = UnitLists.allies[alliedAttacker1].CurrentHP();
            }
            catch (IndexOutOfRangeException)
            {
                alliedAttacker = 0;
            }
            #endregion
            Console.WriteLine();
            Console.WriteLine("What will they do!");
            Console.WriteLine();
            if (UnitLists.allies[alliedAttacker1]._specialAttacks == null)
            {
                Console.Clear();
                Console.WriteLine("This character has no special attacks!");
                goto AfterPlayerTurn;
            }
            UnitLists.allies[alliedAttacker1].PrintSpecialAttacks();

            #region InputCheck
            SystemExtension.cki = Console.ReadKey(true);
            input = SystemExtension.Input<int>();
            int specialAttackIndex = 0;
            //Int Check
            if (!Char.IsNumber(SystemExtension.cki.KeyChar))
            {
                input = 0;
            }
            else
            {
                specialAttackIndex = Convert.ToInt32(input);
            }
            //Index too big check
            try
            {
                int c = UnitLists.allies[alliedAttacker1]._specialAttacks[specialAttackIndex].attackType;
            }
            catch (IndexOutOfRangeException)
            {
                specialAttackIndex = 0;
            }
            #endregion

            Console.WriteLine();

            fm.AttackDeployer(UnitLists.allies[alliedAttacker1], specialAttackIndex);
            break;
    }
    AfterPlayerTurn:
    if (fm.VictoryCheck() == true)
    {
        break; //Ends battle
    }
    UpdateCooldowns(UnitLists.allies);
    #endregion
    #region enemyTurn
    Console.WriteLine("--!!--");
    Console.WriteLine("The enemy approaches for an attack!");

    int enemyAttacker = rnd.Next(UnitLists.enemies.Count);
    int alliedAttacked = rnd.Next(UnitLists.allies.Count);
    
    fm.Attack(UnitLists.enemies[enemyAttacker], UnitLists.allies[alliedAttacked]);
    if (fm.VictoryCheck() == true)
    {
        break; //Ends battle
    }
    UpdateCooldowns(UnitLists.enemies);
    #endregion
}
#endregion

#region Methods

void UpdateCooldowns(List<Unit> team)
{
    Console.WriteLine("--!!-- STATUS ALERT! --!!--");
    foreach (Unit unit in team)
    {
        unit.UpdateCoolDowns();
    }
}

#endregion

#region EndScreen
Console.WriteLine("======================================");
Console.WriteLine("");
Console.WriteLine("Press any key to restart!");
Console.WriteLine("Press CTRL+Z to exit!");
Console.WriteLine("");
Console.WriteLine("======================================");

#region InputCheck
SystemExtension.cki = Console.ReadKey(true);
input = SystemExtension.Input<int>();
if (input.GetType() == typeof(string))
{
    if (input == "UNDO")
    {
        Environment.Exit(0);
    }
}
#endregion

Console.Clear();

// Starts a new instance of the program itself
System.Diagnostics.Process.Start(@".\WindsOfDestruction.exe");

// Closes the current process
Environment.Exit(0);
#endregion


