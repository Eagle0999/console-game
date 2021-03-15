using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGame
{
    // Перечисление участников игры.
    enum Users
    {
        Танк_игрока=1,
        Танк_компьютера
    }
    // Перечисление возможных действий игрока(по кол-ву методов).
    enum Operations
    {
        Выстрел = 1,
        Починка,
        Купить_патроны
    }
    class Program
    {
                
            static string PrintCartridges(char symbolCartridge, int n)
            {
                string printCartridges = "";
                for (int i=0; i < n; i++)
                    printCartridges += symbolCartridge + " ";
                return printCartridges;
            }
            static void PrintTank(Tank tankUser, Tank tankComputer)
            {
                // (char)4 - символ патрона танка игрока.
                // (char)6 - символ патрона танка компьютера.
                Console.WriteLine("Здоровье танка игрока: {0}, Кол-во патронов танка игрока: {1}", tankUser.healthTank, PrintCartridges((char)4, tankUser.cartridgesTank));
                Console.WriteLine("Здоровье танка компьютера: {0}, Кол-во патронов танка компьютера: {1}", tankComputer.healthTank, PrintCartridges((char)6, tankComputer.cartridgesTank));
            }
             
            static Users ChoiceStepUserTank(Tank tankUser, Tank tankComputer)
            {
                Console.WriteLine("\nХод танка игрока\n");
                PrintTank(tankUser, tankComputer);
                Console.WriteLine("Выберите требуемое действие:\n 1. {0}\n 2. {1}\n 3. {2} ", Operations.Выстрел, Operations.Починка, Operations.Купить_патроны);
                return Users.Танк_игрока;
            }
           
            static void Main(string[] args)
            {
                double startArmor = 0.00;
                double startHealth = 100.00;
                double startDamage = 25.00;
                int startCartridgeUser = 1;
                int startCartridgeComputer = 0;
                // Данный массив нужен для реализации метода "ShotTank()".
                Tank[] tempTank = new Tank[2];
                Tank tankUser = new Tank(startArmor, startHealth, startDamage, startCartridgeUser);
                Tank tankComputer = new Tank(startArmor, startHealth, startDamage, startCartridgeComputer);
                var rand = new Random();
                Users choiceStep = Users.Танк_игрока;
                Operations choiceAction = Operations.Выстрел; 
                int counterStep = 0;
                while ((tankUser.healthTank >= 0) || (tankComputer.healthTank >= 0))
                {
                    if (tankUser.healthTank <= 0)
                    { 
                        Console.WriteLine("\nВражеский танк победил. Танк игрока уничтожен.\n");
                        break;
                    }
                    if(tankComputer.healthTank <= 0)
                    { 
                        Console.WriteLine("\nТанк игрока победил. Вражеский танк уничтожен.\n");
                        break;
                    }
                    // Условие по которому ходит игрок.
                    if (((counterStep % 2) == 0))
                    {
                        choiceStep = ChoiceStepUserTank(tankUser, tankComputer);
                        choiceAction = (Operations)int.Parse(Console.ReadLine());
                    }
                    // Условие по которому ходит компьютер.
                    if (!((counterStep % 2) == 0))
                    {
                        choiceStep = Users.Танк_компьютера;
                        choiceAction = tankComputer.ChoiceActionComputerTank(tankComputer, startHealth, rand);
                        
                    }
                        
                    if ( (choiceAction >= Operations.Выстрел) && (choiceAction <= Operations.Купить_патроны) )
                    {
                        switch (choiceAction)
                        {
                            case Operations.Выстрел:
                                tempTank = tankComputer.ShotTank(tankUser, tankComputer, choiceStep);
                                tankUser = tempTank[0];
                                tankComputer = tempTank[1];
                                PrintTank(tankUser, tankComputer);
                                break;
                            case Operations.Починка:
                                if (choiceStep == Users.Танк_игрока)
                                    tankUser.healthTank = tankUser.RepairTank(tankUser.healthTank, startHealth);
                                if((choiceStep == Users.Танк_компьютера) && (tankComputer.healthTank != startHealth))
                                    tankComputer.healthTank = tankComputer.RepairTank(tankComputer.healthTank, startHealth);
                                PrintTank(tankUser, tankComputer);
                                break;
                            case Operations.Купить_патроны:
                                if (choiceStep == Users.Танк_игрока)
                                    tankUser.cartridgesTank = tankUser.BuyCartridges(tankUser.cartridgesTank);
                                if (choiceStep == Users.Танк_компьютера)
                                    tankComputer.cartridgesTank = tankComputer.BuyCartridges(tankComputer.cartridgesTank);
                                PrintTank(tankUser, tankComputer);
                                break;
                        }
                    }
                    else Console.WriteLine("Выбранно неверное действие");
                counterStep += 1;
                }
                Console.Read();
            }
    }
}
