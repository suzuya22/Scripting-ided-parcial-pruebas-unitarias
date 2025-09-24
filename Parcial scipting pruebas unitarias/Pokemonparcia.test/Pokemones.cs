using System;
using System.Collections.Generic;

namespace PokemonBattle
{
    public enum PokemonType
    {
        Normal, Fire, Water, Electric, Grass, Ice, Fighting, Poison,
        Ground, Flying, Psychic, Bug, Rock, Ghost, Dragon, Dark, Steel, Fairy
    }

    public enum MoveType
    {
        Physical, Special
    }

    public class Pokemon
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int SpecialAttack { get; set; }
        public int SpecialDefense { get; set; }
        public List<PokemonType> Types { get; set; }
        public List<Move> Moves { get; set; }

        public Pokemon()
        {
            Name = "Unknown";
            Level = 1;
            Attack = 10;
            Defense = 10;
            SpecialAttack = 10;
            SpecialDefense = 10;
            Types = new List<PokemonType> { PokemonType.Normal };
            Moves = new List<Move> { new Move("Tackle", MoveType.Physical, PokemonType.Normal) };
        }

        public Pokemon(string name, int level = 1, int atk = 10, int def = 10,
                      int spAtk = 10, int spDef = 10)
        {
            ValidateParameters(level, atk, def, spAtk, spDef);

            Name = name;
            Level = level;
            Attack = atk;
            Defense = def;
            SpecialAttack = spAtk;
            SpecialDefense = spDef;
            Types = new List<PokemonType> { PokemonType.Normal };
            Moves = new List<Move> { new Move("Tackle", MoveType.Physical, PokemonType.Normal) };
        }

        private void ValidateParameters(int level, int atk, int def, int spAtk, int spDef)
        {
            if (level < 1 || level > 99)
                throw new ArgumentOutOfRangeException(nameof(level), "Level must be between 1 and 99");
            if (atk < 1 || atk > 255)
                throw new ArgumentOutOfRangeException(nameof(atk), "Attack must be between 1 and 255");
            if (def < 1 || def > 255)
                throw new ArgumentOutOfRangeException(nameof(def), "Defense must be between 1 and 255");
            if (spAtk < 1 || spAtk > 255)
                throw new ArgumentOutOfRangeException(nameof(spAtk), "Special Attack must be between 1 and 255");
            if (spDef < 1 || spDef > 255)
                throw new ArgumentOutOfRangeException(nameof(spDef), "Special Defense must be between 1 and 255");
        }
    }

    public class Move
    {
        public string Name { get; set; }
        public int Power { get; set; }
        public int Speed { get; set; }
        public PokemonType Type { get; set; }
        public MoveType MoveType { get; set; }

        public Move(string name, MoveType moveType, PokemonType type, int power = 100, int speed = 1)
        {
            Name = name;
            Power = power;
            Speed = speed;
            Type = type;
            MoveType = moveType;
        }
    }

    // Especies específicas
    public class Pikachu : Pokemon
    {
        public Pikachu() : base("Pikachu")
        {
            Types = new List<PokemonType> { PokemonType.Electric };
        }
    }

    public class Charizard : Pokemon
    {
        public Charizard() : base("Charizard")
        {
            Types = new List<PokemonType> { PokemonType.Fire, PokemonType.Flying };
        }
    }

    public class Blastoise : Pokemon
    {
        public Blastoise() : base("Blastoise")
        {
            Types = new List<PokemonType> { PokemonType.Water };
        }
    }

    public class Venusaur : Pokemon
    {
        public Venusaur() : base("Venusaur")
        {
            Types = new List<PokemonType> { PokemonType.Grass, PokemonType.Poison };
        }
    }

    public class Alakazam : Pokemon
    {
        public Alakazam() : base("Alakazam")
        {
            Types = new List<PokemonType> { PokemonType.Psychic };
        }
    }

    public static class TypeChart
    {
        public static double GetModifier(PokemonType attackType, PokemonType defenseType)
        {
            // Implementación básica de la tabla de tipos
            if (attackType == PokemonType.Water && defenseType == PokemonType.Fire) return 2.0;
            if (attackType == PokemonType.Water && defenseType == PokemonType.Ground) return 2.0;
            if (attackType == PokemonType.Electric && defenseType == PokemonType.Ground) return 0.0;
            if (attackType == PokemonType.Fire && defenseType == PokemonType.Water) return 0.5;
            if (attackType == PokemonType.Water && defenseType == PokemonType.Electric) return 2.0;

            return 1.0; // Neutral por defecto
        }

        public static double GetModifier(PokemonType attackType, List<PokemonType> defenseTypes)
        {
            double totalModifier = 1.0;
            foreach (var defenseType in defenseTypes)
            {
                totalModifier *= GetModifier(attackType, defenseType);
            }
            return totalModifier;
        }
    }

    public static class BattleCalculator
    {
        public static double CalculateDamage(Pokemon attacker, Pokemon defender, Move move, double modifier)
        {
            if (modifier == 0.0) return 0.0;

            double damage;

            if (move.MoveType == MoveType.Physical)
            {
                damage = ((2.0 * attacker.Level / 5.0 + 2) *
                         (move.Power * (double)attacker.Attack / defender.Defense + 2)) / 50.0;
            }
            else // Special
            {
                damage = ((2.0 * attacker.Level / 5.0 + 2) *
                         (move.Power * (double)attacker.SpecialAttack / defender.SpecialDefense + 2)) / 50.0;
            }

            return Math.Floor(damage * modifier);
        }

        public static double CalculateDamage(Pokemon attacker, Pokemon defender, Move move)
        {
            double modifier = TypeChart.GetModifier(move.Type, defender.Types);
            return CalculateDamage(attacker, defender, move, modifier);
        }
    }
}