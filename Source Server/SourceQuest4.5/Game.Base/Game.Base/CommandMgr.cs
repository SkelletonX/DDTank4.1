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

		private static Hashtable m_cmds = new Hashtable(StringComparer.InvariantCultureIgnoreCase);

		private static string[] m_disabledarray = new string[0];

		public static string[] DisableCommands
		{
			get
			{
				return m_disabledarray;
			}
			set
			{
				m_disabledarray = ((value == null) ? new string[0] : value);
			}
		}

		public static GameCommand GetCommand(string cmd)
		{
			return m_cmds[cmd] as GameCommand;
		}

		public static GameCommand GuessCommand(string cmd)
		{
			GameCommand gameCommand = GetCommand(cmd);
			if (gameCommand != null)
			{
				return gameCommand;
			}
			string value = cmd.ToLower();
			IDictionaryEnumerator enumerator = m_cmds.GetEnumerator();
			while (enumerator.MoveNext())
			{
				GameCommand gameCommand2 = enumerator.Value as GameCommand;
				string text = enumerator.Key as string;
				if (gameCommand2 != null && text.ToLower().StartsWith(value))
				{
					gameCommand = gameCommand2;
					break;
				}
			}
			return gameCommand;
		}

		public static string[] GetCommandList(ePrivLevel plvl, bool addDesc)
		{
			IDictionaryEnumerator enumerator = m_cmds.GetEnumerator();
			ArrayList arrayList = new ArrayList();
			while (enumerator.MoveNext())
			{
				GameCommand gameCommand = enumerator.Value as GameCommand;
				string text = enumerator.Key as string;
				if (gameCommand == null || text == null)
				{
					continue;
				}
				if (text[0] == '&')
				{
					text = "/" + text.Remove(0, 1);
				}
				if ((uint)plvl >= gameCommand.m_lvl)
				{
					if (addDesc)
					{
						arrayList.Add(text + " - " + gameCommand.m_desc);
					}
					else
					{
						arrayList.Add(gameCommand.m_cmd);
					}
				}
			}
			return (string[])arrayList.ToArray(typeof(string));
		}

		[ScriptLoadedEvent]
		public static void OnScriptCompiled(RoadEvent ev, object sender, EventArgs args)
		{
			LoadCommands();
		}

		public static bool LoadCommands()
		{
			m_cmds.Clear();
			foreach (Assembly item in new ArrayList(ScriptMgr.Scripts))
			{
				Type[] types = item.GetTypes();
				foreach (Type type in types)
				{
					if (type.IsClass && !(type.GetInterface("Game.Base.ICommandHandler") == null))
					{
						try
						{
							object[] customAttributes = type.GetCustomAttributes(typeof(CmdAttribute), inherit: false);
							for (int j = 0; j < customAttributes.Length; j++)
							{
								CmdAttribute cmdAttribute = (CmdAttribute)customAttributes[j];
								bool flag = false;
								string[] disabledarray = m_disabledarray;
								foreach (string b in disabledarray)
								{
									if (cmdAttribute.Cmd.Replace('&', '/') == b)
									{
										flag = true;
										break;
									}
								}
								if (!flag && !m_cmds.ContainsKey(cmdAttribute.Cmd))
								{
									GameCommand gameCommand = new GameCommand();
									gameCommand.m_usage = cmdAttribute.Usage;
									gameCommand.m_cmd = cmdAttribute.Cmd;
									gameCommand.m_lvl = cmdAttribute.Level;
									gameCommand.m_desc = cmdAttribute.Description;
									gameCommand.m_cmdHandler = (ICommandHandler)Activator.CreateInstance(type);
									m_cmds.Add(cmdAttribute.Cmd, gameCommand);
									if (cmdAttribute.Aliases != null)
									{
										disabledarray = cmdAttribute.Aliases;
										foreach (string key in disabledarray)
										{
											m_cmds.Add(key, gameCommand);
										}
									}
								}
							}
						}
						catch (Exception exception)
						{
							if (log.IsErrorEnabled)
							{
								log.Error("LoadCommands", exception);
							}
						}
					}
				}
			}
			return true;
		}

		public static void DisplaySyntax(BaseClient client)
		{
			client.DisplayMessage("Commands list:");
			string[] commandList = GetCommandList(ePrivLevel.Admin, addDesc: true);
			foreach (string str in commandList)
			{
				client.DisplayMessage("         " + str);
			}
		}

		public static bool HandleCommandNoPlvl(BaseClient client, string cmdLine)
		{
			try
			{
				string[] array = ParseCmdLine(cmdLine);
				GameCommand gameCommand = GuessCommand(array[0]);
				if (gameCommand == null)
				{
					return false;
				}
				ExecuteCommand(client, gameCommand, array);
			}
			catch (Exception exception)
			{
				if (log.IsErrorEnabled)
				{
					log.Error("HandleCommandNoPlvl", exception);
				}
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
			{
				throw new ArgumentNullException("cmdLine");
			}
			List<string> list = new List<string>();
			int num = 0;
			StringBuilder stringBuilder = new StringBuilder(cmdLine.Length >> 1);
			for (int i = 0; i < cmdLine.Length; i++)
			{
				char c = cmdLine[i];
				switch (num)
				{
				case 0:
					if (c != ' ')
					{
						stringBuilder.Length = 0;
						if (c == '"')
						{
							num = 2;
							break;
						}
						num = 1;
						i--;
					}
					break;
				case 1:
					if (c == ' ')
					{
						list.Add(stringBuilder.ToString());
						num = 0;
					}
					stringBuilder.Append(c);
					break;
				case 2:
					if (c == '"')
					{
						list.Add(stringBuilder.ToString());
						num = 0;
					}
					stringBuilder.Append(c);
					break;
				}
			}
			if (num != 0)
			{
				list.Add(stringBuilder.ToString());
			}
			string[] array = new string[list.Count];
			list.CopyTo(array);
			return array;
		}
	}
}
