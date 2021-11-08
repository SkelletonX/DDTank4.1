// Decompiled with JetBrains decompiler
// Type: Game.Base.CommandMgr
// Assembly: Game.Base, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2C15C00-C3DB-415D-8006-692895AE7555
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Base.dll

using Game.Base.Events;
using Game.Server.Managers;
using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Game.Base
{
  public class CommandMgr
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static Hashtable m_cmds = new Hashtable((IEqualityComparer) StringComparer.InvariantCultureIgnoreCase);
    private static string[] m_disabledarray = new string[0];

    public static string[] DisableCommands
    {
      get
      {
        return CommandMgr.m_disabledarray;
      }
      set
      {
        CommandMgr.m_disabledarray = value == null ? new string[0] : value;
      }
    }

    public static GameCommand GetCommand(string cmd)
    {
      return CommandMgr.m_cmds[(object) cmd] as GameCommand;
    }

    public static GameCommand GuessCommand(string cmd)
    {
      GameCommand gameCommand1 = CommandMgr.GetCommand(cmd);
      if (gameCommand1 != null)
        return gameCommand1;
      string lower = cmd.ToLower();
      IDictionaryEnumerator enumerator = CommandMgr.m_cmds.GetEnumerator();
      while (enumerator.MoveNext())
      {
        GameCommand gameCommand2 = enumerator.Value as GameCommand;
        string key = enumerator.Key as string;
        if (gameCommand2 != null && key.ToLower().StartsWith(lower))
        {
          gameCommand1 = gameCommand2;
          break;
        }
      }
      return gameCommand1;
    }

    public static string[] GetCommandList(ePrivLevel plvl, bool addDesc)
    {
      IDictionaryEnumerator enumerator = CommandMgr.m_cmds.GetEnumerator();
      ArrayList arrayList = new ArrayList();
      while (enumerator.MoveNext())
      {
        GameCommand gameCommand = enumerator.Value as GameCommand;
        string str = enumerator.Key as string;
        if (gameCommand != null && str != null)
        {
          if (str[0] == '&')
            str = "/" + str.Remove(0, 1);
          if (plvl >= (ePrivLevel) gameCommand.m_lvl)
          {
            if (addDesc)
              arrayList.Add((object) (str + " - " + gameCommand.m_desc));
            else
              arrayList.Add((object) gameCommand.m_cmd);
          }
        }
      }
      return (string[]) arrayList.ToArray(typeof (string));
    }

    [ScriptLoadedEvent]
    public static void OnScriptCompiled(RoadEvent ev, object sender, EventArgs args)
    {
      CommandMgr.LoadCommands();
    }

    public static bool LoadCommands()
    {
      CommandMgr.m_cmds.Clear();
      foreach (Assembly assembly in new ArrayList((ICollection) ScriptMgr.Scripts))
      {
        if (CommandMgr.log.IsDebugEnabled)
          CommandMgr.log.Debug((object) ("ScriptMgr: Searching for commands in " + (object) assembly.GetName()));
        foreach (Type type in assembly.GetTypes())
        {
          if (type.IsClass)
          {
            if (!(type.GetInterface("Game.Base.ICommandHandler") == (Type) null))
            {
              try
              {
                foreach (CmdAttribute customAttribute in type.GetCustomAttributes(typeof (CmdAttribute), false))
                {
                  bool flag = false;
                  foreach (string str in CommandMgr.m_disabledarray)
                  {
                    if (customAttribute.Cmd.Replace('&', '/') == str)
                    {
                      flag = true;
                      CommandMgr.log.Info((object) ("Will not load command " + customAttribute.Cmd + " as it is disabled in game properties"));
                      break;
                    }
                  }
                  if (!flag)
                  {
                    if (CommandMgr.m_cmds.ContainsKey((object) customAttribute.Cmd))
                    {
                      CommandMgr.log.Info((object) (customAttribute.Cmd + " from " + (object) assembly.GetName() + " has been suppressed, a command of that type already exists!"));
                    }
                    else
                    {
                      if (CommandMgr.log.IsDebugEnabled)
                        CommandMgr.log.Debug((object) ("Load: " + customAttribute.Cmd + "," + customAttribute.Description));
                      GameCommand gameCommand = new GameCommand();
                      gameCommand.m_usage = customAttribute.Usage;
                      gameCommand.m_cmd = customAttribute.Cmd;
                      gameCommand.m_lvl = customAttribute.Level;
                      gameCommand.m_desc = customAttribute.Description;
                      gameCommand.m_cmdHandler = (ICommandHandler) Activator.CreateInstance(type);
                      CommandMgr.m_cmds.Add((object) customAttribute.Cmd, (object) gameCommand);
                      if (customAttribute.Aliases != null)
                      {
                        foreach (string aliase in customAttribute.Aliases)
                          CommandMgr.m_cmds.Add((object) aliase, (object) gameCommand);
                      }
                    }
                  }
                }
              }
              catch (Exception ex)
              {
                if (CommandMgr.log.IsErrorEnabled)
                  CommandMgr.log.Error((object) nameof (LoadCommands), ex);
              }
            }
          }
        }
      }
      CommandMgr.log.Info((object) ("Loaded " + (object) CommandMgr.m_cmds.Count + " commands!"));
      return true;
    }

    public static void DisplaySyntax(BaseClient client)
    {
      client.DisplayMessage("Commands list:");
      foreach (string command in CommandMgr.GetCommandList(ePrivLevel.Admin, true))
        client.DisplayMessage("         " + command);
    }

    public static bool HandleCommandNoPlvl(BaseClient client, string cmdLine)
    {
      try
      {
        string[] cmdLine1 = CommandMgr.ParseCmdLine(cmdLine);
        GameCommand myCommand = CommandMgr.GuessCommand(cmdLine1[0]);
        if (myCommand == null)
          return false;
        CommandMgr.ExecuteCommand(client, myCommand, cmdLine1);
      }
      catch (Exception ex)
      {
        if (CommandMgr.log.IsErrorEnabled)
          CommandMgr.log.Error((object) nameof (HandleCommandNoPlvl), ex);
      }
      return true;
    }

    private static bool ExecuteCommand(BaseClient client, GameCommand myCommand, string[] pars)
    {
      pars[0] = myCommand.m_cmd;
      return myCommand.m_cmdHandler.OnCommand(client, pars);
    }

    private static string[] ParseCmdLine(string cmdLine)
    {
      if (cmdLine == null)
        throw new ArgumentNullException(nameof (cmdLine));
      List<string> stringList = new List<string>();
      int num = 0;
      StringBuilder stringBuilder = new StringBuilder(cmdLine.Length >> 1);
      for (int index = 0; index < cmdLine.Length; ++index)
      {
        char ch = cmdLine[index];
        switch (num)
        {
          case 0:
            if (ch != ' ')
            {
              stringBuilder.Length = 0;
              if (ch == '"')
              {
                num = 2;
                break;
              }
              num = 1;
              --index;
              break;
            }
            break;
          case 1:
            if (ch == ' ')
            {
              stringList.Add(stringBuilder.ToString());
              num = 0;
            }
            stringBuilder.Append(ch);
            break;
          case 2:
            if (ch == '"')
            {
              stringList.Add(stringBuilder.ToString());
              num = 0;
            }
            stringBuilder.Append(ch);
            break;
        }
      }
      if ((uint) num > 0U)
        stringList.Add(stringBuilder.ToString());
      string[] array = new string[stringList.Count];
      stringList.CopyTo(array);
      return array;
    }
  }
}
