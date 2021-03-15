using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGame
{
    interface ITankUser
    {
        double armorTank { get; set; }
        double healthTank { get; set; }
        double damageTank { get; set; }
        int cartridgesTank { get; set; }
        Tank[] ShotTank(Tank tankUser, Tank tankComputer, Users choiceStep);
        double RepairTank(double currentHealth, double startHealth);
        int BuyCartridges(int cartridges);
    }
}
