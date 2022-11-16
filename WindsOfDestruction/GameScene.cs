using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace WindsOfDestruction
{
    public class GameScene : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public GameScene()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Window.AllowUserResizing = false;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            Console.ForegroundColor = SystemExtension.defaultForegroundColor;
            Console.BackgroundColor = SystemExtension.defaultBackgroundColor;
            //Console.TreatControlCAsInput = true;

            JSONFighterReader rt = JsonConvert.DeserializeObject<JSONFighterReader>(File.ReadAllText(@".\fighterClasses.json"));
            FightManager fm = new FightManager();

            base.Update(gameTime);

            #region Variables

            int z = 0;

            object input = 0;

            int tempInt = 0;

            #endregion

            #region MainMenu
            Console.WriteLine("======================================");
            Console.WriteLine("Winds of Destruction");
            Console.WriteLine("");
            Console.WriteLine("Press any key to start!");
            Console.WriteLine("Press CTRL+Z to exit!");
            Console.WriteLine("");
            Console.WriteLine("======================================");
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
            while (true || z < 3)
            {
                Console.WriteLine("Warrior's name? (Empty to start fight)");
                string name = Console.ReadLine();
                if (UnitLists.allies.Count <= 0 && name == "")
                {
                    name = "NULL";
                }
                else if (name == "")
                {
                    break;
                }
                Console.WriteLine("Warrior's class' id?");

                int fighterClass = 1;

                Console.WriteLine();
                if (!rt.fighters.Any(item => item.id == fighterClass))
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

            base.Update(gameTime);

            #region Battle
            ActionLogWriterAndReader.ResetLog();
            while (true) //Battle Loop
            {
                #region ALWAR
                ActionLogWriterAndReader.roundID++;
                ActionLogWriterAndReader.WriteTeamStatusLog(UnitLists.allies, UnitLists.deadAllies, UnitLists.enemies, UnitLists.deadEnemies);
            RestartRound:
                if (ActionLogWriterAndReader.loadRoundToggled == true && ActionLogWriterAndReader.roundID > 1)
                {
                    ActionLogWriterAndReader.LoadTeamStatusLog();
                    Console.Clear();
                    ActionLogWriterAndReader.loadRoundToggled = false;
                }
                else if (ActionLogWriterAndReader.loadRoundToggled == true && ActionLogWriterAndReader.roundID == 1)
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

                int turnChoice = 0;

                Console.Clear();
                switch (turnChoice)
                {
                    default:
                    case 0:
                        Console.WriteLine("--!!--");
                        Console.WriteLine("The enemies braces for an attack!");
                        fm.PrintTeam(UnitLists.allies);
                        Console.WriteLine("Choose your attacker! [ID]");

                        int alliedAttacker = 0;

                        Console.WriteLine();
                        fm.PrintTeam(UnitLists.enemies);
                        Console.WriteLine("Choose who to attack! [ID]");

                        int enemyAttacked = 0;

                        Console.WriteLine();

                        fm.Attack(UnitLists.allies[alliedAttacker], UnitLists.enemies[enemyAttacked]);
                        break;
                    case 1:
                        Console.WriteLine("--!!--");
                        Console.WriteLine("The enemies braces for an attack!");
                        fm.PrintTeam(UnitLists.allies);
                        Console.WriteLine("Choose your attacker! [ID]");

                        int alliedAttacker1 = 0;

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

                        int specialAttackIndex = 0;

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

            base.Update(gameTime);

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
            Console.Clear();

            // Starts a new instance of the program itself
            //System.Diagnostics.Process.Start(@".\WindsOfDestruction.exe");

            // Closes the current process
            //Environment.Exit(0);
            #endregion

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
