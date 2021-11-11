// Decompiled with JetBrains decompiler
// Type: Center.Service.actions.ConsoleStart
// Assembly: Center.Service, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BC5694D-A9FA-4488-B8B6-DD6BBEB5CBAF
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Center\Center.Service.exe

using Bussiness.Protocol;
using Center.Server;
using Game.Base;
using Game.Service;
using log4net;
using System;
using System.Collections;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace Center.Service.actions
{
  public class ConsoleStart : IAction
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
        return "Starts the DOL server in console mode";
      }
    }

    private static bool StartServer()
    {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Title = "Center - DDtank Dev";
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("|| DDtank Dev                         ||");
            Console.WriteLine("|| Versão 4.1                          ||");
            Console.WriteLine("|| Data da Release:         {0} ||", File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location).ToString("dd-MM-yyyy"));
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("Iniciando Center Server... Aguarde!");
            return CenterServer.Instance.Start();
    }

    public void OnAction(Hashtable parameters)
    {
      CenterServer.CreateInstance(new CenterServerConfig());
      ConsoleStart.StartServer();
      ConsoleClient consoleClient = new ConsoleClient();
      bool flag = true;
      while (flag)
      {
        try
        {
          Console.Write("> ");
          string cmdLine = Console.ReadLine();
          string[] strArray = cmdLine.Split('&');
          string lower = strArray[0].ToLower();
          if (!(lower == "exit"))
          {
            if (!(lower == "notice"))
            {
              if (!(lower == "reload"))
              {
                if (!(lower == "shutdown"))
                {
                  if (!(lower == "help"))
                  {
                    if (lower == "AAS")
                    {
                      if (strArray.Length < 2)
                        Console.WriteLine("You need enter TRUE or FALSE in parameter!");
                      else
                        CenterServer.Instance.SendAAS(bool.Parse(strArray[1]));
                    }
                    else if (cmdLine.Length > 0)
                    {
                      if (cmdLine[0] == '/')
                        cmdLine = cmdLine.Remove(0, 1).Insert(0, "&");
                      try
                      {
                        if (!CommandMgr.HandleCommandNoPlvl((BaseClient) consoleClient, cmdLine))
                          Console.WriteLine("Unknown command: " + cmdLine);
                      }
                      catch (Exception ex)
                      {
                        Console.WriteLine(ex.ToString());
                      }
                    }
                  }
                  else
                    Console.WriteLine(this.HelpStr);
                }
                else
                  CenterServer.Instance.SendShutdown();
              }
              else if (strArray.Length < 2)
                Console.WriteLine("You need enter a valid parameter!");
              else
                CenterServer.Instance.SendReload(strArray[1]);
            }
            else if (strArray.Length < 2)
              Console.WriteLine("You need enter a valid parameter!");
            else
              CenterServer.Instance.SendSystemNotice(strArray[1]);
          }
          else
            flag = false;
        }
        catch (Exception ex)
        {
          Console.WriteLine("Error:" + ex.ToString());
        }
      }
      if (CenterServer.Instance == null)
        return;
      CenterServer.Instance.Stop();
    }

    public void Reload(eReloadType type)
    {
    }
  }
}
