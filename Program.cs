// Mohammad Abu Shattal. Ph.D., 
// abushattal@ieee.org
//This Code is written based on the seed code developed by Tapas Bal: Using SQLite in a C# Application
// https://www.codeguru.com/dotnet/using-sqlite-in-a-c-application/

using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteDemo
{
   class Program
   {

      static void Main(string[] args)
      {
         
         SQLiteConnection sqlite_conn;
         sqlite_conn = CreateConnection();         
         //ReadData(sqlite_conn);
         //CreateTables(sqlite_conn);
         //dropTables(sqlite_conn);

         int option = 0;
         string inputOptionstring;

         while(true){
         Console.WriteLine("Please choose the option from the list below:\n"+
         "1) Create Tables and fill data.\n"+
         "2) Read Data from tables and tabulate data.\n"+
         "3) Drop Tables.\n"+
         "10)Exit Program!" );
         inputOptionstring = Console.ReadLine() ?? "1" ;
         option = Int32.Parse(inputOptionstring);        
         switch(option){
            case 1: CreateTables(sqlite_conn);
                    Console.WriteLine("Tables created!"); 
                    break;
            case 2: ReadData(sqlite_conn);
                    Console.WriteLine("Tables data retrieved!"); 
                    break;
            case 3: dropTables(sqlite_conn);
            Console.WriteLine("Tables dropped!");
                  break;
            case 10: 
                  Console.WriteLine("Exit program!");
                  System.Environment.Exit(1);  break; 
            default: 
               Console.WriteLine("Option not provided from the list. \nHere is a list of acceptable options:\n");   
               break;
            }
         }
      }
   static SQLiteConnection CreateConnection()
      {

         SQLiteConnection sqlite_conn;
         // Create a new database connection:
            sqlite_conn = new SQLiteConnection("Data Source=company.db;Version=3");
         // Open the connection:
         try
         {
            sqlite_conn.Open();
         }
         catch (Exception ex)
         {
            Console.WriteLine(ex.Message);

         }
         return sqlite_conn;
      }


   static void dropTables(SQLiteConnection conn){
     SQLiteCommand sqlite_cmd;
         string dropSqlStatement = 
          "drop table EMPLOYEE;"+
          "drop table DEPARTMENT;"+
          "drop table PROJECT;"+
          "drop table DEPT_LOCATIONS;"+
          "drop table WORKS_ON;"+
          "drop table DEPENDENT;";  
          try
         {
            conn.Open();
         }
         catch (Exception ex)
         {
            Console.WriteLine(ex.Message);
         }   
         sqlite_cmd = conn.CreateCommand();
         sqlite_cmd.CommandText = dropSqlStatement;
         sqlite_cmd.ExecuteNonQuery();          
}

   static void CreateTables(SQLiteConnection conn)
      {
         SQLiteCommand sqlite_cmd;
         string Createsql = File.ReadAllText("createTables.txt");
         try
         {
            conn.Open();
         }
         catch (Exception ex)
         {
            Console.WriteLine(ex.Message);
         }
         Console.WriteLine(Createsql);    
         sqlite_cmd = conn.CreateCommand();
         sqlite_cmd.CommandText = Createsql;
         sqlite_cmd.ExecuteNonQuery();        
      }

   static void ReadData(SQLiteConnection conn)
      {
         SQLiteDataReader sqlite_datareader;
         SQLiteCommand sqlite_cmd;
         sqlite_cmd = conn.CreateCommand();
         sqlite_cmd.CommandText = "SELECT * FROM EMPLOYEE, DEPENDENT Where Ssn= Essn " ;

         int rowCounter = 0;
         sqlite_datareader = sqlite_cmd.ExecuteReader();     

         while (sqlite_datareader.Read())
         {
         int counter = 0;
         try{
            while(counter<sqlite_datareader.GetValues().Count ){
             string myreader  = sqlite_datareader.GetValue(counter).ToString() ?? "";
               Console.Write(myreader.PadRight(8) + "\t");
             counter++;            
            }
         }catch(Exception e){
            Console.WriteLine(e.Message);
         }
         Console.WriteLine();
         rowCounter++;
         }
         Console.WriteLine("----------------------------------------- ");
         Console.WriteLine("No. of Attributes = "+ sqlite_datareader.GetValues().Count);
         Console.WriteLine("No. of Rows = "+ rowCounter);
         conn.Close();
      }

   }
}