// Decompiled with JetBrains decompiler
// Type: Fighting.Service.action.ConsoleStart
// Assembly: Fighting.Service, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A10CE59F-8EFA-4220-9FA2-1100E5246235
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Fight\Fighting.Service.exe

using Fighting.Server;
using Fighting.Server.Games;
using Fighting.Server.Rooms;
using log4net;
using System;
using System.Collections;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace Fighting.Service.action
{
  public class ConsoleStart : Fighting.Service.IAction
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public string HelpStr
    {
      get
      {
        return ConfigurationSettings.AppSettings[nameof (HelpStr)];
      }
    }

    public string Name
    {
      get
      {
        return "--start";
      }
    }

    public string Syntax
    {
      get
      {
        return "--start [-config=./config/serverconfig.xml]";
      }
    }

    public string Description
    {
      get
      {
        return "Starts the Fighting server in console mode";
      }
    }

    public void OnAction(Hashtable parameters)
    {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Title = "Fighting - DDtank True";
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("|| DDtank True                         ||");
            Console.WriteLine("|| Versão 4.1                          ||");
            Console.WriteLine("|| Data da Release:         {0} ||", File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location).ToString("dd-MM-yyyy"));
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("Iniciando Fighting Server... Aguarde!");
            FightServerConfig config = new FightServerConfig();
      try
      {
        config.Load();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        Console.ReadKey();
        return;
      }
      FightServer.CreateInstance(config);
      FightServer.Instance.Start();
      bool flag = true;
      while (flag)
      {
        try
        {
          Console.Write("> ");
          string[] strArray = Console.ReadLine().Split(' ');
          string lower = strArray[0].ToLower();
          if (!(lower == "clear"))
          {
            if (!(lower == "list"))
            {
              if (!(lower == "exit"))
              {
                if (lower == "cfg&reload")
                {
                  ConfigurationManager.RefreshSection("appSettings");
                  Console.WriteLine("Configuration file is Reload!");
                }
              }
              else
                flag = false;
            }
            else if (strArray.Length > 1)
            {
              string str = strArray[1];
              if (!(str == "-client"))
              {
                if (!(str == "-room"))
                {
                  if (str == "-game")
                  {
                    Console.WriteLine("game list:");
                    Console.WriteLine("-------------------------------");
                    foreach (object game in GameMgr.GetGames())
                      Console.WriteLine(game.ToString());
                    Console.WriteLine("-------------------------------");
                  }
                }
                else
                {
                  Console.WriteLine("room list:");
                  Console.WriteLine("-------------------------------");
                  foreach (object obj in ProxyRoomMgr.GetAllRoom())
                    Console.WriteLine(obj.ToString());
                  Console.WriteLine("-------------------------------");
                }
              }
              else
              {
                Console.WriteLine("server client list:");
                Console.WriteLine("--------------------");
                foreach (object allClient in FightServer.Instance.GetAllClients())
                  Console.WriteLine(allClient.ToString());
                Console.WriteLine("-------------------");
              }
            }
            else
            {
              Console.WriteLine("list [-client][-room][-game]");
              Console.WriteLine("     -client:列出所有服务器对象");
              Console.WriteLine("     -room:列出所有房间对象");
              Console.WriteLine("     -game:列出所有游戏对象");
            }
          }
          else
            Console.Clear();
        }
        catch (Exception ex)
        {
          Console.WriteLine("Error:" + ex.ToString());
        }
      }
      if (FightServer.Instance == null)
        return;
      FightServer.Instance.Stop();
    }
  }
}
