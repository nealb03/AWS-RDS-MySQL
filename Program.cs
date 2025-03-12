// Mohammad Abu Shattal. Ph.D., 
// abushattal@ieee.org
//This Code is written based on the seed code developed by Tapas Bal: Using SQLite in a C# Application
// https://www.codeguru.com/dotnet/using-sqlite-in-a-c-application/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace AWSRDSDemo
{
   class Program
   {

      static void Main(string[] args)
      {         
         MySqlConnection mysql_conn;
         mysql_conn = CreateConnection();         
         int option = 0;
         string inputOptionstring;

         while(true){
         Console.WriteLine("Please choose the option from the list below:\n"+
         "1) Create Tables and fill data.\n"+
         "2) Read Data from tables and tabulate data.\n"+
         "3) Drop Tables.\n"+
         "4) Search Employee by SSN.\n"+
         "5) Search Department by Dnumer\n"+
         "6) Add Department\n"+
         "7) Add Department Location\n"+
         "10)Exit Program!" 
          );
         inputOptionstring = Console.ReadLine() ?? "1" ;
         option = Int32.Parse(inputOptionstring);        
         switch(option){
            case 1: CreateTables(mysql_conn);                   
                    break;
            case 2: ReadData(mysql_conn);
                    break;
            case 3: dropTables(mysql_conn);
                  break;
            case 4: 
                  searchEmployeeBySSN(mysql_conn);
                  break;
            case 5:
                  searchDepartmentByDNumber(mysql_conn);
                  break;
            case 6: 
                  InsertDepartment( mysql_conn);
                  break;
            case 7: 
                  AddDepartmentLocation(mysql_conn);
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
   
   static MySqlConnection CreateConnection(){
      MySqlConnection conn;
      var constring1 = "Server=clouddb2.c944gfronmhi.us-east-1.rds.amazonaws.com;Database=COMPANY;User ID=admin;Password=clouddb2password;SslMode=Preferred;";
      var constring2 = "Server=clouddb2.c944gfronmhi.us-east-1.rds.amazonaws.com;User ID=admin;Password=clouddb2password;SslMode=Preferred;";
         // Create a new database connection:
         conn = new MySqlConnection(constring1);
         // Open the connection:
         try
         {
            conn.Open();           
         }
         catch (Exception ex)
         {
            try
            {               
               conn = new MySqlConnection(constring2);
               conn.Open();
               var cmd = conn.CreateCommand();
               cmd.CommandText = "CREATE DATABASE IF NOT EXISTS `COMPANY`;";
               cmd.ExecuteNonQuery();
               return conn;
            }
            catch(Exception ex2){
               Console.WriteLine(ex.Message);
               Console.WriteLine(ex2.Message);               
            }           
         }
         return conn;
   }


  static void dropTables(MySqlConnection conn)
{
    Console.WriteLine("Are you sure you want to drop the tables? (yes, no) ");
    string Y = Console.ReadLine() ?? "no";

    if (Y.ToLower() == "yes")
    {
        string[] dropStatements = new string[]
        {
            "DROP TABLE IF EXISTS DEPENDENT;",
            "DROP TABLE IF EXISTS WORKS_ON;",
            "DROP TABLE IF EXISTS DEPT_LOCATIONS;",
            "DROP TABLE IF EXISTS PROJECT;",
            "DROP TABLE IF EXISTS DEPARTMENT;",
            "DROP TABLE IF EXISTS EMPLOYEE;"
        };

        try
        {
            conn.Open();
            foreach (string stmt in dropStatements)
            {
                using var cmd = new MySqlCommand(stmt, conn);
                cmd.ExecuteNonQuery();
            }
            Console.WriteLine("Tables dropped!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
        finally
        {
            conn.Close();
        }
    }
    else
    {
        Console.WriteLine("No Tables dropped!");
    }
}

   static void CreateTables(MySqlConnection conn)
      {
         MySqlCommand mydql_cmd;
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
         mydql_cmd = conn.CreateCommand();
         mydql_cmd.CommandText = Createsql;
         mydql_cmd.ExecuteNonQuery();       
          Console.WriteLine("Tables created!");  
          conn.Close();
      }

   static void ReadData(MySqlConnection conn)
      {
         try
         {
            conn.Open();
         }
         catch (Exception ex)
         {
            Console.WriteLine(ex.Message);
         }
         MySqlDataReader mysqldatareader;
         
         MySqlCommand mydql_cmd;
         mydql_cmd = conn.CreateCommand();
         //mydql_cmd.CommandText = "SELECT * FROM EMPLOYEE, DEPENDENT Where Ssn= Essn " ;
         mydql_cmd.CommandText = "SELECT * FROM EMPLOYEE" ;


         int rowCounter = 0;
         mysqldatareader = mydql_cmd.ExecuteReader();     

         while (mysqldatareader.Read())
         {
         try{
            object[] values = new object[mysqldatareader.FieldCount];
            for (int i = 0; i < mysqldatareader.FieldCount; i++)
            {
               values[i] = mysqldatareader.GetValue(i);
               Console.Write(values[i] + "\t".PadRight(5));
            }
         }catch(Exception e){
           // Console.WriteLine(e.Message);
           string s = e.Message;
           s= "";
         }
         Console.WriteLine();
         rowCounter++;
         }
         Console.WriteLine("----------------------------------------- ");
         Console.WriteLine("Tables data retrieved!"); 
         Console.WriteLine("No. of Attributes = "+ mysqldatareader.FieldCount);
         Console.WriteLine("No. of Rows = "+ rowCounter);
         conn.Close();         
      }
    
   static void searchEmployeeBySSN(MySqlConnection conn)
      {
         try
         {
            conn.Open();
         }
         catch (Exception ex)
         {
           string s = ex.Message;  
           s =""   ;
         }

         MySqlDataReader mysqldatareader;
         MySqlCommand mydql_cmd;
         Console.WriteLine("\nPlease input SSN: ");
         string ssn = Console.ReadLine() ?? "123456789";
         if (ssn == ""){
            Console.WriteLine("Not a valid SSN! No record retrieved!");
            return;
         }
         mydql_cmd = conn.CreateCommand();
         mydql_cmd.Parameters.Add("@SSN", MySqlDbType.Int32).Value = ssn;

         mydql_cmd.Parameters["@SSN"].Value = ssn;
         
         mydql_cmd.CommandText = "SELECT * FROM EMPLOYEE Where Ssn= @SSN" ;

         int rowCounter = 0;
         mysqldatareader = mydql_cmd.ExecuteReader();     

         while (mysqldatareader.Read())
         {

         try{
           object[] values = new object[mysqldatareader.FieldCount];
            for (int i = 0; i < mysqldatareader.FieldCount; i++)
            {
               values[i] = mysqldatareader.GetValue(i);
               Console.Write(values[i] + "\t".PadRight(5));
            }  

         }catch(Exception e){
            string s = e.Message;            
         }
         Console.WriteLine();
         rowCounter++;
         }
         Console.WriteLine("----------------------------------------- ");
         Console.WriteLine("No. of Attributes = "+ mysqldatareader.FieldCount);
         Console.WriteLine("No. of Rows = "+ rowCounter);
         conn.Close();
      }

       static void searchDepartmentByDNumber(MySqlConnection conn)
      {
         try
         {
            conn.Open();
         }
         catch (Exception ex)
         {
            string s = ex.Message;  
           s =""   ;
         }

         MySqlDataReader mysqldatareader;
         MySqlCommand mydql_cmd;
         Console.WriteLine("\nPlease input Department Number: ");
         String DNOString = Console.ReadLine() ?? "1";
         int DNO = Convert.ToInt32(DNOString);
         if (DNO < -1){
            Console.WriteLine("Not a valid Dno! No record retrieved!");
            return;
         }
         mydql_cmd = conn.CreateCommand();

         mydql_cmd.Parameters.Add("@Dno", MySqlDbType.Int32).Value = DNO;

         mydql_cmd.Parameters["@Dno"].Value = DNO;

         mydql_cmd.CommandText = "SELECT * FROM DEPARTMENT Where Dnumber= @Dno" ;

         int rowCounter = 0;
         mysqldatareader = mydql_cmd.ExecuteReader();     

         while (mysqldatareader.Read())
         {
         int counter = 0;
         try{
            while(counter<mysqldatareader.FieldCount ){
             string myreader  = mysqldatareader.GetValue(counter).ToString() ?? "        ";
               Console.Write(myreader.PadRight(8) + "\t");
             counter++;            
            }
         }catch(Exception e){
           string s = e.Message;  
           s =""   ;
         }
         Console.WriteLine();
         rowCounter++;
         }
         Console.WriteLine("----------------------------------------- ");
         Console.WriteLine("No. of Attributes = "+ mysqldatareader.FieldCount);
         Console.WriteLine("No. of Rows = "+ rowCounter);
         conn.Close();
      }

      static void AddDepartmentLocation(MySqlConnection conn)
      {
         Console.WriteLine("Location Added!");
      }

      static void InsertDepartment(MySqlConnection conn)
      {
         MySqlCommand mydql_cmd;
         try
         {
            conn.Open();
         }
         catch (Exception ex)
         {
            string s = ex.Message;  
           s =""   ;
         }
 
         mydql_cmd = conn.CreateCommand();
         Console.WriteLine("Enter the department data one by one:\n For the date values input in the following foramt: Jan 15, 2024\n");
         //string input = Console.ReadLine() ?? " ";
         //string[] split = input.Split(',');  

         string DnameStr= Console.ReadLine() ?? "Department";
         string DNAME = DnameStr;
         mydql_cmd.Parameters.Add("@D_NAME", MySqlDbType.VarChar).Value = DNAME;
         mydql_cmd.Parameters["@D_NAME"].Value = DNAME;
        
         string DnoStr= Console.ReadLine() ?? "";
         int DNUM = Int32.Parse(DnoStr); 
         mydql_cmd.Parameters.Add("@DNUMBER", MySqlDbType.Int32).Value = DNUM;
         mydql_cmd.Parameters["@DNUMBER"].Value = DNUM;

         string mgrssnStr= Console.ReadLine() ?? "9999";
         int MGRSSN = Int32.Parse(mgrssnStr);
         mydql_cmd.Parameters.Add("@MGR_SSN", MySqlDbType.Int32).Value = MGRSSN;
         mydql_cmd.Parameters["@MGR_SSN"].Value = MGRSSN;

         string sdateStr= Console.ReadLine() ?? "9999";
         string startdate = sdateStr;
         var parsedDate = DateTime.Parse(startdate);

         mydql_cmd.Parameters.Add("@STARTDATE", MySqlDbType.Date).Value = parsedDate;
         mydql_cmd.Parameters["@STARTDATE"].Value =parsedDate;

         string queryText = "Insert into DEPARTMENT values (@D_NAME, @DNUMBER, @MGR_SSN, @STARTDATE )";
         mydql_cmd.CommandText = queryText;
         mydql_cmd.ExecuteNonQuery();       
          Console.WriteLine("Department created!");  
      }
   }
}


     
