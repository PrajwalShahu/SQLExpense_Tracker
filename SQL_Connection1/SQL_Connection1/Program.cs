//using System;
//using System.Collections.Generic;
//using System.Transactions;
using System.Data.SqlClient;

class Program
{
    static void Main(string[] args)
    {
        SqlConnection conn = new SqlConnection("Data source = IN-B33K9S3; Initial Catalog = Expenses_Tracker;Integrated Security=true");
        conn.Open();

        while (true)
        {
            Console.WriteLine("Welcome to Expenses Tracker application");
            Console.WriteLine("1. AddTransaction");
            Console.WriteLine("2. View Expenses");
            Console.WriteLine("3. View Income");
            Console.WriteLine("4. Check Available Balance");


            Console.Write("Enter your choice (1-4): ");
            int choice = Convert.ToInt32(Console.ReadLine());


            switch (choice)
            {
                case 1:
                    {
                        string title = "";
                        string description = "";
                        decimal amount = 0;
                        try
                        {
                            Console.Write("Enter Title: ");
                            title = Console.ReadLine();

                            Console.Write("Enter Description: ");
                            description = Console.ReadLine();

                            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(description))
                            {
                                throw new EmptyException();
                            }
                        }
                        catch (EmptyException)
                        {
                            Console.WriteLine("Fields should not be empty");
                            return;
                        }

                        try
                        {
                            Console.Write("Enter Amount: ");
                            amount = Convert.ToDecimal(Console.ReadLine());
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("Enter only integer value");
                            return;
                        }

                        Console.Write("Enter Date (dd-MM-yyyy): ");
                        DateTime date = Convert.ToDateTime(Console.ReadLine());

                        string query = "INSERT INTO Transactions (Title, Description, Amount, Date) VALUES (@Title, @Description, @Amount, @Date)";
                        SqlCommand cmd = new SqlCommand(query, conn);

                        cmd.Parameters.AddWithValue("@Title", title);
                        cmd.Parameters.AddWithValue("@Description", description);
                        cmd.Parameters.AddWithValue("@Amount", amount);
                        cmd.Parameters.AddWithValue("@Date", date);
                        cmd.ExecuteNonQuery();

                        Console.WriteLine("Record Added Successfully!");

                        break;
                    }
                case 2:
                    {
                        string query = "SELECT * FROM Transactions WHERE Amount < 0";
                        SqlCommand command = new SqlCommand(query, conn);

                        SqlDataReader reader = command.ExecuteReader();

                        Console.WriteLine("Expenses Transactions:");
                        Console.WriteLine("--------------------");
                        Console.WriteLine("{0,-15} {1,-20} {2,-20} {3,-10}", "Title", "Description", "Amount", "Date");
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader[0],-15} {reader[1],-20} {reader[2],-20} {reader[4],-10}");
                        }


                        break;
                    }


                case 3:
                    {
                        string query = "SELECT * FROM Transactions WHERE Amount > 0";
                        SqlCommand command = new SqlCommand(query, conn);

                        SqlDataReader reader = command.ExecuteReader();

                        Console.WriteLine("Income Transactions:");
                        Console.WriteLine("--------------------");
                        Console.WriteLine("{0,-15} {1,-20} {2,-20} {3,-10}", "Title", "Description", "Amount", "Date");
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader[0],-15} {reader[1],-20} {reader[2],-20} {reader[4],-10}");
                        }


                        break;
                    }

                case 4:
                    {
                        SqlCommand command = new SqlCommand("SELECT SUM(Amount) as Available_Balance FROM Transactions", conn);
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            Console.WriteLine(reader["Available_Balance"]);
                        }
                        break;
                    }

                default:
                    Console.WriteLine("Wrong Choice Entered!");
                    break;
            }

            Console.WriteLine();
        }
        conn.Close();
    }

}
