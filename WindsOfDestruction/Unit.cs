using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindsOfDestruction
{
    public class Unit
    {
        private string _name { get; set; }
        //Iniciating damage value
        private float _baseDamage { get; set; }
        //Iniciating HP value
        private int _baseHP { get; set; }
        //0 -> 0.1* damage recieved multiplier, 1 = 1* damage recieved multiplier, 0.1 = 0% and 1 = 100% value, 2 = 200% value
        private double _baseHPdepleteMultiplier { get; set; }
        // 0 -> 0.1* damage given multiplier, 1 = 1* damage given multiplier, 0.1 = 0% and 1 = 100% value, 2 = 200% value
        private double _baseDamageMultiplier { get; set; }

        //Applied Attack damage value, affected by multipliers.
        private float _currentDamage { get; set; }

        //Applied HP value, affected by multipliers.
        private int _currentHP { get; set; }

        //0 -> 0.1* damage recieved multiplier, 1 = 1* damage recieved multiplier, 0.1 = 0% and 1 = 100% value, 2 = 200% value
        private double _currentHPdepleteMultiplier { get; set; }
        // 0 -> 0.1* damage given multiplier, 1 = 1* damage given multiplier, 0.1 = 0% and 1 = 100% value, 2 = 200% value
        private double _currentDamageMultiplier { get; set; }

        public Unit(string name, float baseDamage, int baseHP, double baseHPdepleteMultiplier, double baseDamageMultiplier)
        {
            this._name = name;
            this._baseDamage = baseDamage;
            this._baseHP = baseHP;
            this._baseHPdepleteMultiplier = baseHPdepleteMultiplier;
            this._baseDamageMultiplier = baseDamageMultiplier;

            this._currentDamage = this._baseDamage;
            this._currentHP = this._baseHP;

            this._currentHPdepleteMultiplier = Math.Clamp(this._baseHPdepleteMultiplier, 0.1, 2);
            this._currentDamageMultiplier = Math.Clamp(this._baseDamageMultiplier, 0.1, 2);
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
            this._currentDamage = Convert.ToInt32(Math.Ceiling(this._baseDamage * this._currentDamageMultiplier));
            //Console.WriteLine("Calculated Damage to give: " + this._currentDamage);
            return Convert.ToInt32(this._currentDamage);
        }

        //Information methods
        public int CurrentHP()
        {
            return this._currentHP;
        }
        public string Name()
        {
            return this._name;
        }
    }
}
