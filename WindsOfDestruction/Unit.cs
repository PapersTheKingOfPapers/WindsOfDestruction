using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace WindsOfDestruction
{
    public class Unit
    {
        #region Variables
        //Name
        public string _name { get; set; }
        //Iniciating damage value
        public float _baseDamage { get; set; }
        //Iniciating HP value
        public int _baseHP { get; set; }
        //0 -> 0.1* damage recieved multiplier, 1 = 1* damage recieved multiplier, 0.1 = 0% and 1 = 100% value, 2 = 200% value
        public double _baseHPdepleteMultiplier { get; set; }
        // 0 -> 0.1* damage given multiplier, 1 = 1* damage given multiplier, 0.1 = 0% and 1 = 100% value, 2 = 200% value
        public double _baseDamageMultiplier { get; set; }
        //Applied Attack damage value, affected by multipliers.
        public float _currentDamage { get; set; }
        //Applied HP value, affected by multipliers.
        public int _currentHP { get; set; }
        //0 -> 0.1* damage recieved multiplier, 1 = 1* damage recieved multiplier, 0.1 = 0% and 1 = 100% value, 2 = 200% value
        public double _currentHPdepleteMultiplier { get; set; }
        // 0 -> 0.1* damage given multiplier, 1 = 1* damage given multiplier, 0.1 = 0% and 1 = 100% value, 2 = 200% value
        public double _currentDamageMultiplier { get; set; }
        public List<SpecialAttack> _specialAttacks { get; set; }
        public bool shieldAttackActive { get; set; }
        public double _counterDamage { get; set; }
        public string _UnitSpecificKey { get; set; }
        public bool _alive { get; set; }
        #endregion

        public Unit(string name, float baseDamage, int baseHP, double baseHPdepleteMultiplier, double baseDamageMultiplier, List<SpecialAttack> specialAttacks)
        {
            this._name = name;
            this._baseDamage = baseDamage;
            this._baseHP = baseHP;
            this._baseHPdepleteMultiplier = baseHPdepleteMultiplier;
            this._baseDamageMultiplier = baseDamageMultiplier;

            this._currentDamage = this._baseDamage;
            this._currentHP = this._baseHP;

            this._currentHPdepleteMultiplier = Math.Clamp(this._baseHPdepleteMultiplier, 0, 2);
            this._currentDamageMultiplier = Math.Clamp(this._baseDamageMultiplier, 0.1, 2);

            this._specialAttacks = specialAttacks.Clone();
            this.shieldAttackActive = false;
            this._counterDamage = 0;

            this._UnitSpecificKey = Guid.NewGuid().ToString();
            this._alive = true;
        }

        //Check Methods

        //Math Methods
        public int Damage(int recievedDamage)
        {
            //Console.WriteLine("Recieved damage value: " + recievedDamage + ", Current HP before attack: " + this._currentHP);
            this._currentHP -= Convert.ToInt32(Math.Ceiling(recievedDamage * this._currentHPdepleteMultiplier));
            //Console.WriteLine("HP After attack: " + this._currentHP + ", Defence multiplier: " + this._currentHPdepleteMultiplier);
            return this._currentHP;
        }
        public int Attack()
        {
            int sentDamage = Convert.ToInt32(Math.Ceiling(this._currentDamage * this._currentDamageMultiplier));
            //Console.WriteLine("Calculated Damage to give: " + this._currentDamage);
            return Convert.ToInt32(sentDamage);
        }

        //Update methods
        public void LoadWholeUnitFile(string name, float baseDamage, int baseHP, double baseHPdepleteMultiplier, double baseDamageMultiplier, 
            float currentDamage, int currentHP, double currentHPdepleteMultiplier, double currentDamageMultiplier, 
            List<SpecialAttack> specialAttacks, bool shieldAttackActive, double counterDamage, string unitSpecificKey, bool alive)
        {
            this._name = name;
            this._baseDamage = baseDamage;
            this._baseHP = baseHP;
            this._baseHPdepleteMultiplier = baseHPdepleteMultiplier;
            this._baseDamageMultiplier = baseDamageMultiplier;

            this._currentDamage = currentDamage;
            this._currentHP = currentHP;

            this._currentHPdepleteMultiplier = Math.Clamp(currentHPdepleteMultiplier, 0, 2);
            this._currentDamageMultiplier = Math.Clamp(currentDamageMultiplier, 0.1, 2);

            this._specialAttacks = specialAttacks.Clone();
            this.shieldAttackActive = shieldAttackActive;
            this._counterDamage = counterDamage;

            this._UnitSpecificKey = unitSpecificKey;
            this._alive = alive;
        }
        public void ChangeDamage(float newDamageValue)
        {
            this._currentDamage = newDamageValue;
        }
        public void ResetDamage()
        {
            this._currentDamage = this._baseDamage;
        }
        public void ChangeDamageMultiplier(double newDamageMultiplier)
        {
            this._currentDamageMultiplier = newDamageMultiplier;
        }
        public void ResetDamageMultiplier()
        {
            this._currentDamageMultiplier = this._baseDamageMultiplier;
        }
        public void ChangeDefenceMultiplier(double newHPdepleteMultiplier)
        {
            this._currentHPdepleteMultiplier = newHPdepleteMultiplier;
        }
        public void ResetDefenceMultiplier()
        {
            this._currentHPdepleteMultiplier = this._baseHPdepleteMultiplier;
        }
        public void UpdateCoolDowns()
        {
            if (this._specialAttacks != null)
            {
                for (int i = 0; i < this._specialAttacks.Count; i++)
                {
                    if (this._specialAttacks[i].currentCoolDown > 0)
                    {
                        //Console.WriteLine($"DEBUG : {this._name}'s {this._specialAttacks[i].attackName} current cooldown is {this._specialAttacks[i].currentCoolDown}");

                        if (this._specialAttacks[i].currentCoolDown == 1)
                        {
                            Console.WriteLine($"{this._name}'s {this._specialAttacks[i].attackName} is ready to use!");
                        }
                        this._specialAttacks[i].currentCoolDown--;
                    }

                    if (this._specialAttacks[i].currentPersistTime > 0)
                    {
                        //Console.WriteLine($"DEBUG : {this._name}'s {this._specialAttacks[i].attackName} current persist time is {this._specialAttacks[i].currentPersistTime}");

                        if (this._specialAttacks[i].currentPersistTime > 2)
                        {
                            Console.WriteLine($"{this._name}'s '{this._specialAttacks[i].attackName}' is currently active!");
                        }

                        if (this._specialAttacks[i].currentPersistTime == 2)
                        {
                            Console.WriteLine($"{this._name}'s '{this._specialAttacks[i].attackName}' is about to end!");
                        }

                        this._specialAttacks[i].currentPersistTime--;

                        if(this._specialAttacks[i].currentPersistTime == 0)
                        {
                            Console.WriteLine($"{this._name}'s '{this._specialAttacks[i].attackName}' has ended!");
                            switch (this._specialAttacks[i].attackType)
                            {
                                case 2:
                                    this.shieldAttackActive = false;
                                    ResetDefenceMultiplier();
                                    break;
                            }
                        }
                    }
                }
            }
        }
        public void EnableShields()
        {
            this.shieldAttackActive = true;
            //Console.WriteLine($"DEBUG : {this._name}'s shields have activated!");
        }
        public void SetCounterDamage(double counterDamageValue)
        {
            this._counterDamage = counterDamageValue;
        }
        public void KillUnit()
        {
            this._alive = false;
        }

        //Information methods
        public void PrintSpecialAttacks()
        {
            for (int i = 0; i < this._specialAttacks.Count; i++)
            {
                Console.WriteLine($"Index: {i}, {this._specialAttacks[i].attackName}, {this._specialAttacks[i].attackDescription}");
                Console.WriteLine($"Current Cooldown: {this._specialAttacks[i].currentCoolDown}");
            }
        }
        public int CurrentHP()
        {
            return this._currentHP;
        }
        public int MaxHP()
        {
            return this._baseHP;
        }
        public string Name()
        {
            return this._name;
        }
        public bool ShieldsStatus()
        {
            return this.shieldAttackActive;
        }
        public double CounterDamageValue()
        {
            return this._counterDamage;
        }
        public bool AliveStatus()
        {
            return this._alive;
        }
    }
}
