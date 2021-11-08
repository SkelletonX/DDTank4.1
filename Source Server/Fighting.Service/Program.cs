// Decompiled with JetBrains decompiler
// Type: Fighting.Service.Program
// Assembly: Fighting.Service, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A10CE59F-8EFA-4220-9FA2-1100E5246235
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Fight\Fighting.Service.exe

using Fighting.Service.action;
using System;
using System.Collections;
using System.IO;
using System.Threading;

namespace Fighting.Service
{
  internal class Program
  {
    private static ArrayList _actions = new ArrayList();

    [MTAThread]
    private static void Main(string[] args)
    {
      AppDomain.CurrentDomain.SetupInformation.PrivateBinPath = "." + Path.DirectorySeparatorChar.ToString() + "lib";
      Thread.CurrentThread.Name = "MAIN";
      Program.RegisterActions();
      if (args.Length == 0)
        args = new string[1]{ "--start" };
      string actionName;
      Hashtable parameters;
      try
      {
        Program.ParseParameters(args, out actionName, out parameters);
      }
      catch (ArgumentException ex)
      {
        Console.WriteLine(ex.Message);
        return;
      }
      IAction action = Program.GetAction(actionName);
      if (action != null)
        action.OnAction(parameters);
      else
        Program.ShowSyntax();
    }

    private static void RegisterActions()
    {
      Program.RegisterAction((IAction) new ConsoleStart());
    }

    private static void RegisterAction(IAction action)
    {
      if (action == null)
        throw new ArgumentException("Action can't be bull", "actioni");
      Program._actions.Add((object) action);
    }

    public static void ShowSyntax()
    {
      Console.WriteLine("Syntax: RoadServer.exe {action} [param1=value1] [param2=value2] ...");
      Console.WriteLine("Possible actions:");
      foreach (IAction action in Program._actions)
      {
        if (action.Syntax != null && action.Description != null)
          Console.WriteLine(string.Format("{0,-20}\t{1}", (object) action.Syntax, (object) action.Description));
      }
    }

    private static IAction GetAction(string name)
    {
      foreach (IAction action in Program._actions)
      {
        if (action.Name.Equals(name))
          return action;
      }
      return (IAction) null;
    }

    private static void ParseParameters(
      string[] args,
      out string actionName,
      out Hashtable parameters)
    {
      parameters = new Hashtable();
      actionName = (string) null;
      if (!args[0].StartsWith("--"))
        throw new ArgumentException("First argument must be the action");
      actionName = args[0];
      if (args.Length == 1)
        return;
      for (int index = 1; index < args.Length; ++index)
      {
        string str1 = args[index];
        if (str1.StartsWith("--"))
          throw new ArgumentException("At least two actions given and only one action allowed!");
        if (str1.StartsWith("-"))
        {
          int length = str1.IndexOf('=');
          if (length == -1)
          {
            parameters.Add((object) str1, (object) "");
          }
          else
          {
            string str2 = str1.Substring(0, length);
            string str3 = "";
            if (length + 1 < str1.Length)
              str3 = str1.Substring(length + 1);
            parameters.Add((object) str2, (object) str3);
          }
        }
      }
    }
  }
}
