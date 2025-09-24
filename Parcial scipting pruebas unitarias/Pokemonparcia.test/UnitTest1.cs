

using NUnit.Framework;
// de la nada esto se me puso gris y no me esta validando las pruebas, revise el .csproj y tiene incluido todo, me perdi y perdi muchisimo timpo en esto, cree hasta un archivo nuevo con unit 3 y nada
namespace PokemonBattle.Tests
{
    public class PokemonTests
    {
        [Test]
        public void Pokemon_PorDefecto_casosdelpunto3dadosporelprofe()
        {
            var p = new Pokemon();
            Assert.AreEqual(1, p.Level);
        }

        [Test]
        public void Move_PorDefecto_SegunelenunciadotieneATK100()
        {
            var m = new Move("Tackle", MoveType.Physical, PokemonType.Normal);
            Assert.AreEqual(100, m.Power);
        }

        [Test]
        public void Pikachu_eselectrico()
        {
            var pika = new Pikachu();
            Assert.AreEqual("Pikachu", pika.Name);
            Assert.AreEqual(PokemonType.Electric, pika.Type[0]);
        }

        [Test]
        public void Modificador_WaterVsFire_DeberiaSerx2()
        {
            double mod = TypeChart.GetModifier(PokemonType.Water, PokemonType.Fire);
            Assert.AreEqual(2.0, mod);
        }

        [Test]
        public void Modificador_ElectricVsGround_DeberiaSer0()
        {
            double mod = TypeChart.GetModifier(PokemonType.Electric, PokemonType.Ground);
            Assert.AreEqual(0.0, mod);
        }

        [Test]
        public void Modificador_WaterVsFireGround_DeberiaSerx4()
        {
            double mod = TypeChart.GetModifier(PokemonType.Water, PokemonType.Fire, PokemonType.Ground);
            Assert.AreEqual(4.0, mod);
        }

        [Test]
        public void FormulaDaño_Fisico_Mod1()
        {
            var attacker = new Pokemon("Attacker", level: 50, atk: 128);
            var defender = new Pokemon("Defender", def: 128);
            var move = new Move("Golpe", MoveType.Physical, PokemonType.Normal, power: 128);

            double damage = BattleCalculator.CalculateDamage(attacker, defender, move);
            Assert.AreEqual(50, damage); // valor esperado del enunciado
        }
    }

    public class BattleCalculatorTests
    {
        // 🔹 40 casos de prueba
        [TestCase(1, 1, 1, 1, 0, 0, 1)]
        [TestCase(5, 50, 100, 50, 1, 1, 2)]
        [TestCase(5, 100, 100, 50, 1, 16, 3)]
        [TestCase(10, 20, 30, 15, 1, 5, 4)]
        [TestCase(5, 50, 100, 50, 2, 2, 5)]
        [TestCase(12, 40, 60, 80, 2, 9, 6)]
        [TestCase(20, 80, 120, 100, 4, 40, 7)]
        [TestCase(35, 100, 50, 100, 4, 58, 8)]
        [TestCase(50, 128, 200, 150, 1, 58, 9)]
        [TestCase(40, 128, 200, 100, 1, 37, 10)]
        [TestCase(50, 128, 200, 100, 4, 455, 11)]
        [TestCase(70, 180, 250, 200, 2, 435, 12)]
        [TestCase(70, 180, 250, 200, 2, 435, 13)]
        [TestCase(80, 200, 225, 90, 1, 33, 14)]
        [TestCase(90, 255, 200, 50, 2, 1554, 15)]
        [TestCase(99, 255, 255, 1, 2, 108206, 16)]
        [TestCase(99, 255, 255, 255, 0, 0, 17)]
        [TestCase(99, 255, 255, 255, 0, 0, 18)]
        [TestCase(99, 255, 255, 1, 1, 856, 19)]
        [TestCase(45, 60, 10, 200, 1, 2, 20)]
        [TestCase(20, 30, 5, 250, 1, 1, 21)]
        [TestCase(2, 10, 1, 255, 1, 1, 22)]
        [TestCase(3, 5, 2, 3, 1, 1, 23)]
        [TestCase(15, 200, 255, 255, 1, 33, 24)]
        [TestCase(16, 200, 255, 254, 1, 34, 25)]
        [TestCase(17, 200, 255, 128, 1, 36, 26)]
        [TestCase(33, 77, 77, 77, 1, 25, 27)]
        [TestCase(48, 33, 99, 11, 4, 508, 28)]
        [TestCase(55, 44, 88, 22, 1, 44, 29)]
        [TestCase(66, 11, 11, 11, 1, 8, 30)]
        [TestCase(77, 123, 200, 100, 2, 326, 31)]
        [TestCase(88, 200, 100, 50, 4, 1197, 32)]
        [TestCase(10, 200, 200, 200, 0, 0, 33)]
        [TestCase(75, 180, 255, 180, 0, 0, 34)]
        [TestCase(99, 255, 255, 0, 0, 0, 35)]
        [TestCase(25, 60, 40, 20, 0, 0, 36)]
        [TestCase(60, 200, 255, 128, 1, 40, 37)]
        [TestCase(80, 80, 90, 90, 1, 27, 38)]
        [TestCase(40, 80, 45, 90, 1, 17, 39)]
        [TestCase(99, 200, 150, 150, 1, 84, 40)]
        public void CalculoDaño_CasosDelaTabla(
            int level, int power, int atk, int def,
            double mod, double expected, int caseNum)
        {
            // alternar físico (impares) / especial (pares)
            var moveType = (caseNum % 2 == 0) ? MoveType.Special : MoveType.Physical;

            var attacker = new Pokemon("Test", level: level, atk: atk, spAtk: atk);
            var defender = new Pokemon("Dummy", def: def, spDef: def);
            var move = new Move("Golpe", moveType, PokemonType.Normal, power: power);

            double damage = BattleCalculator.CalculateDamage(attacker, defender, move, mod);

            Assert.AreEqual(expected, damage, 1.0,
                $"Fallo en caso {caseNum} con LV={level}, PWR={power}, ATK={atk}, DEF={def}, MOD={mod}");
        }
    }
}
