using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGame
{
    class Tank : ITankUser, ITankComputer
    {

        // Свойство танка «Броня».
        double armor;
        public double armorTank { get => armor; set => armor = value; }
        // Свойство танка «Жизнь».
        double health;
        public double healthTank { get => health; set => health = value; }
        // Свойство танка «Урон».
        double damage;
        public double damageTank { get => damage; set => damage = value; }
        // Свойство танка «Количество патронов».
        int cartridges;
        public int cartridgesTank { get => cartridges; set => cartridges = value; }

        // Конструктор по умолчанию для присвоения нулевых значений свойствам класса танка.
        public Tank()
        {
            armorTank = 0.0;
            healthTank = 0.0;
            damageTank = 0.0;
            cartridgesTank = 0;
        }

        // Конструктор класса 'Tank' с 3-мя параметрами по умолчанию для свойств: «Броня», «Жизнь», «Урон».
        public Tank(double armor, double health, double damage, int cartridges)
        {
            this.armorTank = armor;
            this.healthTank = health;
            this.damageTank = damage;
            this.cartridgesTank = cartridges;
        }
        // Метод «Выстрел» для класса 'Tank'.
        //
        public Tank[] ShotTank(Tank tankUser, Tank tankComputer, Users choiceStep)
        {
            Tank[] tempTank = new Tank[2];
            // Первый элемент массива "tempTank" хранит в себе объект танка игрока и всего его свойства.
            tempTank[0] = tankUser;
            // Второй элемент массива "tempTank" хранит в себе объект танка компьютера и всего его свойства.
            tempTank[1] = tankComputer;
            Console.WriteLine("Выбранно действие: {0}", Enum.GetName(typeof(Operations), 1));
            if ((tankUser.cartridgesTank < 1) && (choiceStep == Users.Танк_игрока))
            {
                Console.WriteLine("Кончились патроны. Необходимо купить патроны для танка игрока ");
                return tempTank;
            }
            if (tankComputer.cartridgesTank < 1 && (choiceStep == Users.Танк_компьютера))
            {
                Console.WriteLine("Кончились патроны. Танк компьютера покупает патроны");
                tankComputer.cartridgesTank = tankComputer.BuyCartridges(tankComputer.cartridgesTank);
                return tempTank;
            }
            
            var rand = new Random();
            // Переменная 'probabilityCriticalShot' - вероятность критического выстрела.
            int probabilityCriticalShot = rand.Next(0, 101);
            // Переменная 'probabilityMissShot' - вероятность промаха.
            int probabilityMissShot = rand.Next(0, 101);
            if (choiceStep == Users.Танк_игрока)
            {
                if (probabilityMissShot <= 20)
                {
                    Console.WriteLine("Танк игрока промазал");
                    tempTank[0].cartridgesTank--;
                    return tempTank;
                }
                if (probabilityCriticalShot <= 10)
                {
                    Console.WriteLine("Танк игрока нанёс критический урон");
                    tankUser.damageTank = tankUser.damageTank + ((tankUser.damageTank * 20) / 100);
                }
                tankComputer.healthTank = (tankComputer.healthTank + tankComputer.armorTank) - tankUser.damageTank;
                tankUser.cartridgesTank--;
            }

            if (choiceStep == Users.Танк_компьютера)
            {
                if (probabilityMissShot <= 20)
                {
                    Console.WriteLine("Танк компьютера промазал");
                    tempTank[1].cartridgesTank--;
                    return tempTank;
                }
                if (probabilityCriticalShot <= 10)
                {
                    Console.WriteLine("Танк компьютера нанёс критический урон");
                    tankComputer.damageTank = tankComputer.damageTank + ((tankComputer.damageTank * 20) / 100);
                }
                tankUser.healthTank = (tankUser.healthTank + tankUser.armorTank) - tankComputer.damageTank;
                tankComputer.cartridgesTank--;
            }
            tempTank[0] = tankUser;
            tempTank[1] = tankComputer;
            return tempTank;
        }
        // Метод «Починка» для класса 'Tank'.
        public double RepairTank(double currentHealth, double startHealth)
        {
            Console.WriteLine("Выбранно действие: {0}", Enum.GetName(typeof(Operations), 2));
            var rand = new Random();
            double addHealth = rand.Next(1, 25);
            if (((currentHealth + addHealth) <= startHealth))
            {
                currentHealth = currentHealth + addHealth;
                Console.WriteLine("Добавлено очков здоровья: {0}", addHealth);
            }
            else
                Console.WriteLine("Добавляемое количество здоровья превышает максимальное значение.");
            
            return currentHealth;
        }
        // Метод «Купить патроны» для класса 'Tank'.
        public int BuyCartridges(int currentCartridges)
        {
            Console.WriteLine("Выбранно действие: {0}",  Enum.GetName(typeof(Operations), 3));
            var rand = new Random();
            int addCartridges = rand.Next(1, 4);
            if (((currentCartridges + addCartridges) <= 7))
            {
                currentCartridges = currentCartridges + addCartridges;
                Console.WriteLine("Добавлено число патронов: {0}", addCartridges);
            }
            else
                Console.WriteLine("Добавляемое число патронов превышает максимальное значение.");
            return currentCartridges;
        }
        // Метод «Ход компьютера» для класса 'Tank'.
        public Operations ChoiceActionComputerTank(Tank tankComputer, double startHealthTankComputer, Random rand)
        {
            Console.WriteLine("\n\nХод танка компьютера\n");
            Operations choiceAction;
            choiceAction = (Operations)rand.Next((int)Operations.Выстрел, (int)Operations.Купить_патроны + 1);
            // Если у танка компьютера максимальное кол-во жизней, не выполнять метод «Починка».
            while ((tankComputer.healthTank == startHealthTankComputer) && (choiceAction == Operations.Починка))
            {
                if (choiceAction != Operations.Починка)
                    break;
                choiceAction = (Operations)rand.Next((int)Operations.Выстрел, (int)Operations.Купить_патроны + 1);
            }
            if(tankComputer.healthTank < 30)
                choiceAction = Operations.Починка;
            return choiceAction;
        }
    }
}
