using System;
using Parsers;
using Senders;
using Hangfire;
using Hangfire.MySql;
using Serilog;
using Serilog.Events;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            String ConnectionString = "server=localhost;uid=root;pwd=123qwe;database=hangfire";
            GlobalConfiguration.Configuration.UseStorage(new MySqlStorage(ConnectionString));
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

            Log.Logger.Information("Hello, Serilog!");

            var parser = new CSVParser(@"C:\zad1\mails.csv");
            var mails = parser.Parse();
            var sender = new MailSender();

            var cutList = sender.CutList(mails);
            sender.SendBatch(cutList, Log.Logger);
        }
    }
}
