using ATM.AtmDataSetTableAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ATM
{
    class Program
    {
        private static String Connection = "Data Source = (LocalDB)\\MSSQLLocalDB; AttachDbFilename = C:\\Users\\Paul\\Documents\\GitHub\\CSharp-Simple-AtM\\Atm.mdf; Integrated Security = True";
        private static Boolean Success = false;
        private static double balance;
        static void Main(string[] args)
        {
            int UserId;
            int Pin;
            int Counter = 0;
            double amount;
            Console.WriteLine("Welcome");
            do
            {
                Console.Write("Enter Account Number: ");
                UserId = Convert.ToInt32(Console.ReadLine());
                Console.Write("Enter Pin: ");
                Pin = Convert.ToInt32(Console.ReadLine());
                if (CheckCredentials(UserId, Pin)){
                    Success = true;
                    break;
                }
                else
                {
                    Counter++;
                }
                
            } while (Counter <= 3 );
            
            if (Success)
            {
                Account acc = new Account(UserId, Pin, balance);
                Console.WriteLine("Login Successful");
                Boolean exit = false;
                do
                {
                    Console.WriteLine(acc.ToString());
                    Console.WriteLine("Choose one of the following options: D-Depoist, W-Withdraw, U-Update Pin, T- Transactions or E-Exit");
                    char option = Convert.ToChar(Console.ReadLine());

                    switch (char.ToUpper(option))
                    {
                        case 'E':
                                 exit = true;
                                 break;
                        case 'W':Console.WriteLine("Enter amount to withdraw: ");
                                amount = Convert.ToDouble(Console.ReadLine());
                                acc.Withdraw(amount);
                                WriteTransaction('W', amount, acc.AccountNumber);
                                break;
                        case 'D':
                                Console.WriteLine("Enter amount to deposit: ");
                                amount = Convert.ToDouble(Console.ReadLine());
                                acc.Deposit(amount);
                                WriteTransaction('D', amount, acc.AccountNumber);
                                break;
                        case 'U':
                                Console.WriteLine("Enter new pin:");
                                acc.UpdatePin(Convert.ToInt32(Console.ReadLine()));
                                break;
                        case 'T':
                                PrintTransactions(acc.AccountNumber);
                                break;
                        default: 
                                Console.WriteLine("Invalid option, try again!");
                                break;

                    }

                } while (!exit);
                UpdateServer(acc);
                Console.WriteLine("Server updated\r\nSession Closed");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Login Unsuccessful");
            }
        }

        private static void WriteTransaction(char v, double amount, int user)
        {
            String type = "";
            if (v.Equals('D'))
            {
                type = "Deposit";
            }
            else if(v.Equals("W"))
            {
                type = "Withdraw";
                amount = amount * -1;
            }

            try
            {
                SqlConnection Conn = new SqlConnection(Connection);
                SqlCommand insertCMD = new SqlCommand("Insert into Transactions Values (" + user + "," + amount + ",'" + type + "'," + DateTime.Now + ")", Conn);
                Conn.Open();
                insertCMD.ExecuteNonQuery();
                Conn.Close();
            }
            
            catch (SqlException  e)
            {
                Console.WriteLine("Insert into Transactions Values (" + user + "," + amount + "," + type + "," + DateTime.Now + ")");
            }
        }

        private static void PrintTransactions(int user)
        {
            SqlConnection Conn = new SqlConnection(Connection);
            SqlCommand selectCMD = new SqlCommand("SELECT * FROM Transactions where id =" + user + " ", Conn);
            selectCMD.CommandTimeout = 30;
            SqlDataAdapter userDA = new SqlDataAdapter();
            userDA.SelectCommand = selectCMD;
            Conn.Open();
            DataTable DT = new DataTable();
            userDA.Fill(DT);
            Conn.Close();

            if (DT.Rows.Count > 0)
            {
                foreach (DataRow rows in DT.Rows)
                {
                    Console.WriteLine(String.Format("Type: , Amount: , Date: ", rows["type"].ToString(), Convert.ToDouble(rows["amount"]), Convert.ToDateTime(rows["transaction_date"])));
                }    
                
            }
            else
            {
                Console.WriteLine("No transactions");
            }

        }



        private static void UpdateServer(Account acc)
        {
            SqlConnection Conn = new SqlConnection(Connection);
            SqlCommand updateCMD = new SqlCommand("Update [dbo].[Table] set balance = " + acc.Balance + ", pin = " +acc.Pin +" where account_id =" + acc.AccountNumber + " AND pin = " + acc.Pin + " ", Conn);
            Conn.Open();
            updateCMD.ExecuteNonQuery();
            Conn.Close();
        }

        public static Boolean CheckCredentials(int user, int pin)
        {
            SqlConnection Conn = new SqlConnection(Connection);
            SqlCommand selectCMD = new SqlCommand("SELECT * FROM [dbo].[Table] where account_id =" + user + " AND pin = " + pin +" ", Conn);
            selectCMD.CommandTimeout = 30;
            SqlDataAdapter userDA = new SqlDataAdapter();
            userDA.SelectCommand = selectCMD;
            Conn.Open();
            DataTable DT = new DataTable();
            userDA.Fill(DT);
            Conn.Close();
            if (DT.Rows.Count > 0)
            {
                balance = Convert.ToDouble(DT.Rows[0]["balance"]);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
  
}
