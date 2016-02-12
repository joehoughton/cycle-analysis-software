namespace cycle_analysis.Database
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Reflection;
    using DbUp;
    using DbUp.Engine;

    class Program
    {
    /// <summary>
    /// To run the database scripts, right click the project > Debug > Start new instance
    /// When adding a new script, right click the file > Properties > Build Action > Embedded Resource
    /// </summary>
    static int Main(string[] args)
    {
        var connectionString = ConfigurationManager.ConnectionStrings["CycleAnalysis"].ConnectionString;

        var results = new List<DatabaseUpgradeResult>();

        var migrationResult = ExecuteScriptsInNamespace(connectionString, "cycle_analysis.Database.Scripts");
        var seedResult = ExecuteScriptsInNamespace(connectionString, "cycle_analysis.Database.Seeds");

        results.Add(seedResult);
        results.Add(migrationResult);

        if (results.Any(x => !x.Successful))
        {
            Console.ForegroundColor = ConsoleColor.Red;

            foreach (var result in results)
            {
                Console.WriteLine(result.Error);
            }

            Console.ResetColor();
#if DEBUG
            Console.ReadKey();
#endif
            return -1;
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Success!");
        Console.ResetColor();
#if DEBUG
        Console.ReadKey();
#endif
        return 0;
        }

        private static DatabaseUpgradeResult ExecuteScriptsInNamespace(string connectionString, string scriptNamespace)
        {
            return DeployChanges.To.SqlDatabase(connectionString).WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), x => x.StartsWith(scriptNamespace)).LogToConsole().Build().PerformUpgrade();
        }
    }
}
