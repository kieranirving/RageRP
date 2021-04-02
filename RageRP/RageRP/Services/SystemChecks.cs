using RageRP.Server.DAL;
using RageRP.Server.Helpers;
using System;
using System.Data;
using System.Data.SqlClient;

namespace RageRP.Server.Services
{
    public class SystemChecks
    {
        private string _connectionString;
        public SystemChecks()
        {
            _connectionString = DataAccess.ConnectionString;
        }

        public bool testConnection()
        {
            bool result = false;
            using (var conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        c.WriteLine("SystemCheck: Connection to database works!");
                        Console.ResetColor();
                        result = true;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        c.WriteLine("SystemCheck: Connection doesn't work. Shits fucked.");
                        Console.ResetColor();
                        result = false;
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    c.WriteLine("SystemCheck: Connection doesn't work. Shits fucked. Exception in the hoooooooouse");
                    c.WriteLine(ex.ToString());
                    Console.ResetColor();
                    result = false;
                }
                finally
                {
                    if (conn != null)
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
            return result;
            
        }
    }
}