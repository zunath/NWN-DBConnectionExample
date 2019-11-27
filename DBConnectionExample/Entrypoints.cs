using System;
using System.Data.SqlClient;

namespace NWN
{
    public class Entrypoints
    {
        public const int SCRIPT_HANDLED = 0;
        public const int SCRIPT_NOT_HANDLED = -1;

        //
        // This is called once every main loop frame, outside of object context
        //
        public static void OnMainLoop(ulong frame)
        {

        }

        //
        // This is called every time a named script is scheduled to run.
        // oidSelf is the object running the script (OBJECT_SELF), and script
        // is the name given to the event handler (e.g. via SetEventScript).
        // If the script is not handled in the managed code, and needs to be
        // forwarded to the original NWScript VM, return SCRIPT_NOT_HANDLED.
        // Otherwise, return either 0/SCRIPT_HANDLED for void main() scripts,
        // or an int (0 or 1) for StartingConditionals
        //
        public static int OnRunScript(string script, uint oidSelf)
        {
            return SCRIPT_NOT_HANDLED; // passthrough
        }

        //
        // This is called once when the internal structures have been initialized
        // The module is not yet loaded, so most NWScript functions will fail if
        // called here.
        //
        public static void OnStart()
        {
            const string IpAddress = "172.22.0.100";
            const string Username = "sa";
            const string Password = "StRoNgP@sSwOrD!";
            const string Database = "swlor";

            Console.WriteLine("Building connection string.");
            var connectionString = new SqlConnectionStringBuilder()
            {
                DataSource = IpAddress,
                InitialCatalog = "MASTER",
                UserID = Username,
                Password = Password,
                ConnectTimeout = 300, // 5 minutes
                ApplicationName = "DBConnectionExample",
                
            }.ConnectionString;

            Console.WriteLine("Connection string: " + connectionString);
            bool result;

            using (var connection = new SqlConnection(connectionString))
            {
                Console.WriteLine("Opening connection");
                connection.Open();

                Console.WriteLine("Running select command");
                using (var command = new SqlCommand($"SELECT db_id('{Database}')", connection))
                {
                    Console.WriteLine("Executing scalar");
                    result = command.ExecuteScalar() != DBNull.Value;
                }
            }

            Console.WriteLine("DB Exists: " + result);
        }
    }
}