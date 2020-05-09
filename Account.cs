using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM
{
    class Account
    {
        public int AccountNumber { get; }
        public int Pin { get; set; }
        public double Balance { get; set; }
        public Account(int accountNumber, int pin, double balance)
        {
            AccountNumber = accountNumber;
            Pin = pin;
            Balance = balance;
        }

        public void Withdraw(double amount)
        {
             Balance -= amount;
        }

        public void Deposit(double amount)
        {
             Balance += amount;
        }
        public void UpdatePin(int pin)
        {
            if(pin < 4 || pin > 4)
            {
                Console.WriteLine("Invalid pin. Please enter a 4 digit pin!");
            }
            else
            {
                Pin = pin;
                Console.WriteLine("Pin updated successfully!");
            }
           
        }
        public override string ToString()
        {
            return "Account: " + AccountNumber.ToString() + "\r\nBalance: " + Balance.ToString();
        }
    }
}
