using System;
using System.Configuration;
using System.Net;
using System.Timers;

namespace AttRank
{
    /* APP CONFIG
     * 
          <?xml version="1.0" encoding="utf-8" ?>
        <configuration>
	        <appSettings>
		        <add key="request" value="http://127.0.0.1/request/" />
	        </appSettings>
        </configuration>
     * 
     */
    internal class Program
    {
        private static string link = ConfigurationManager.AppSettings["request"];
        private static string req = Program.link + "CelebList/CreateAllCeleb.ashx?pass=snapepro";
        private static Timer timer;

        private static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Title = "Atualizador de Rankings";
            Console.WriteLine("Tools Desenvolvido Para TrueTank");
            Console.WriteLine("Tempo de Atualização: 5 Minutos");
            Console.WriteLine("\n========================================\n");
            timer = new Timer();
            timer.Interval = 300000.0;
            timer.Elapsed += new ElapsedEventHandler(tt);
            timer.Start();
            ttw();
            Console.Read();
        }

        private static void ttw()
        {
            try
            {
                new WebClient().DownloadString(Program.req);
                Console.WriteLine("Rankings atualizados: " + DateTime.Now.ToString());
                Console.ReadLine();
            }
            catch
            {
                Console.WriteLine("A atualização falhou, por favor, verifique a configuração");
                Console.ReadLine();
            }
        }

        private static void tt(object sender, ElapsedEventArgs e)
        {
            try
            {
                new WebClient().DownloadString(Program.req);
                Console.WriteLine("Rankings atualizados:  " + DateTime.Now.ToString());
                Console.ReadLine();
            }
            catch
            {
                Console.WriteLine("A atualização falhou, por favor, verifique a configuração");
                Console.ReadLine();
            }
        }
    }
}