using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM
{
    class Account
    {
        private long AccountNumber { get; set; }
        private int Pin { get; set; }
        private double Balance { get; set; }
        public Account(long accountNumber, int pin, double balance)
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

        public override string ToString()
        {
            return "Account: " + AccountNumber.ToString() + "\r\nBalance: " + Balance.ToString();
        }
    }
}
