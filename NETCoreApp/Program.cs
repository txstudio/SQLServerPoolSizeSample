using Microsoft.Data.SqlClient;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace NETCoreApp
{
    class Program
    {
        const string connectionString = @"Data Source=127.0.0.1;
                                            Initial Catalog=master;
                                            User ID=sa;
                                            Password=Pa$$w0rd;
                                            Persist Security Info=True;";

        const string extendsetting = "Max Pool Size=10;Connection Timeout=2";

        const int command_timeout = 5;

        static void Main(string[] args)
        {
            string _sqlcmd;

            //_sqlcmd = @"SELECT GETDATE()";
            _sqlcmd = @"WAITFOR DELAY '00:00:30'";

            Enumerable.Range(1, 20).Select(x =>
                Task.Run(() =>
                {
                    Console.WriteLine($"{x} start");

                    SqlConnection _conn = new SqlConnection();
                    _conn.ConnectionString = (connectionString + extendsetting);

                    SqlCommand _cmd = _conn.CreateCommand();
                    _cmd.CommandText = _sqlcmd;
                    _cmd.CommandTimeout = command_timeout;

                    Stopwatch _stopwatch = new Stopwatch();

                    _stopwatch.Start();

                    try
                    {
                        _conn.Open();
                        var _effectRows = _cmd.ExecuteNonQuery();
                        _conn.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"{x} Exception {ex.Message} ");
                    }
                    finally
                    {
                        _stopwatch.Stop();
                        Console.WriteLine($"{x} elapsed {_stopwatch.ElapsedMilliseconds} ms");
                    }
                }
            )).ToList();

            Console.ReadKey();
        }
    }
}
