using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM
{
    class Program
    {
        static void Main(string[] args)
        {
            Account acc = new Account(8674536, 1234, 100.00);
            Console.WriteLine(acc.ToString());
            Console.ReadKey();
            acc.Deposit(1000);
            Console.WriteLine(acc.ToString());
            Console.ReadKey();
            acc.Withdraw(200);
            Console.WriteLine(acc.ToString());
            Console.ReadKey();
        }
    }
}
