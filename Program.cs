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
         
         //CreateTable(sqlite_conn);
         //InsertData(sqlite_conn);
         ReadData(sqlite_conn);
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

    static void ReadData(SQLiteConnection conn)
      {
         SQLiteDataReader sqlite_datareader;
         SQLiteCommand sqlite_cmd;
         sqlite_cmd = conn.CreateCommand();
         //int SSN = 123456789;
         //sqlite_cmd.CommandText = "SELECT Fname, Lname FROM EMPLOYEE WHERE Ssn = " + SSN.ToString() ;
         sqlite_cmd.CommandText = "SELECT * FROM EMPLOYEE " ;

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