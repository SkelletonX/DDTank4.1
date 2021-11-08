using Bussiness;
using Bussiness.Managers;
using Game.Base;
using Game.Logic;
using Game.Server;
using Game.Server.GameObjects;
using Game.Server.Managers;
using Game.Server.Packets;
using Game.Server.Rooms;
using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using SqlDataProvider.Data;
using System.IO;

namespace Game.Service.actions
{
    public class ConsoleStart : Game.Service.IAction
    {
        private delegate int ConsoleCtrlDelegate(ConsoleStart.ConsoleEvent ctrlType);
        private enum ConsoleEvent
        {
            Ctrl_C,
            Ctrl_Break,
            Close,
            Logoff,
            Shutdown
        }
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static Timer _timer;
        private static int _count;
        private static ConsoleStart.ConsoleCtrlDelegate handler;
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
        double num6 = (double)GC.GetTotalMemory(false);
        public void OnAction(Hashtable parameters)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Title = "Road Server - DDtank True";
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("|| DDtank True                         ||");
            Console.WriteLine("|| Versão 4.1                          ||");
            Console.WriteLine("|| Data da Release:         {0} ||", File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location).ToString("dd-MM-yyyy"));
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("Iniciando Road Server... Aguarde!");
            GameServer.CreateInstance(new GameServerConfig());
            GameServer.Instance.Start();
            GameServer.KeepRunning = true;
            Console.WriteLine("Servidor iniciado!");
            ConsoleClient client = new ConsoleClient();
            while (GameServer.KeepRunning)
            {
                try
                {
                    ConsoleStart.handler = new ConsoleStart.ConsoleCtrlDelegate(ConsoleStart.ConsoleCtrHandler);
                    ConsoleStart.SetConsoleCtrlHandler(ConsoleStart.handler, true);
                    Console.Write("> ");
                    string text = Console.ReadLine();
                    string[] array = text.Split(new char[]
                    {
                        ' '
                    });
                    string key;
                    switch (key = array[0])
                    {
                        case "exit":
                            GameServer.KeepRunning = false;
                            continue;

                        case "cp":
                            {
                                GameClient[] allClients = GameServer.Instance.GetAllClients();
                                int num2 = (allClients == null) ? 0 : allClients.Length;
                                GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
                                int num3 = (allPlayers == null) ? 0 : allPlayers.Length;
                                List<BaseRoom> allUsingRoom = RoomMgr.GetAllUsingRoom();
                                int num4 = 0;
                                int num5 = 0;
                                foreach (BaseRoom current in allUsingRoom)
                                {
                                    if (!current.IsEmpty)
                                    {
                                        num4++;
                                        if (current.IsPlaying)
                                        {
                                            num5++;
                                        }
                                    }
                                }
                                Console.WriteLine(string.Format("Número atual de jogadores: {0} / {1}", num2, num3));
                                Console.WriteLine(string.Format("Número de salas de jogos:{0} / {1}", num4, num5));
                                Console.WriteLine(string.Format("Memória em uso:{0} MB", num6 / 1024.0 / 1024.0));
                                continue;
                            }

                        case "shutdown":
                            ConsoleStart._count = 6;
                            ConsoleStart._timer = new Timer(new TimerCallback(ConsoleStart.ShutDownCallBack), null, 0, 60000);
                            continue;

                        case "savemap":
                            continue;

                        case "clear":
                            Console.Clear();
                            continue;

                        case "clrscr":
                            Console.Clear();
                            continue;

                        case "ball&reload":
                            if (BallMgr.ReLoad())
                            {
                                Console.WriteLine("Informação da bola re-runed!");
                                continue;
                            }
                            Console.WriteLine("Informação da bola Não é possível executar novamente!");
                            continue;

                        case "xu&reload":
                            if (BallMgr.ReLoad())
                            {
                                Console.WriteLine("Ball info Da Duoc Chay Lai!");
                                continue;
                            }
                            Console.WriteLine("Ball info Khong The Chay Lai!");
                            continue;

                        case "map&reload":
                            if (MapMgr.ReLoadMap())
                            {
                                Console.WriteLine("Map info Da Duoc Chay Lai!");
                                continue;
                            }
                            Console.WriteLine("Map info Khong The Chay Lai!");
                            continue;

                        case "mapserver&reload":
                            if (MapMgr.ReLoadMapServer())
                            {
                                Console.WriteLine("mapserver info Da Duoc Chay Lai!");
                                continue;
                            }
                            Console.WriteLine("mapserver Khong The Chay Lai!");
                            continue;

                        case "prop&reload":
                            if (PropItemMgr.Reload())
                            {
                                Console.WriteLine("prop info Da Duoc Chay Lai!");
                                continue;
                            }
                            Console.WriteLine("prop info Khong The Chay Lai!");
                            continue;

                        case "item":
                            if (ItemMgr.ReLoad())
                            {
                                Console.WriteLine("item info Da Duoc Chay Lai!");
                                continue;
                            }
                            Console.WriteLine("item info Khong The Chay Lai!");
                            continue;
                        case "reload&drop":
                            if(DropMgr.ReLoad())
                            {
                                Console.WriteLine("Drops Recarregados Com Sucesso!");
                                continue;
                            }
                            Console.WriteLine("Erro Ao Recarregar Os Drops");
                            continue;
                        case "shop&reload":
                            if (ShopMgr.ReLoad())
                            {
                                Console.WriteLine("shop info Da Duoc Chay Lai!");
                                continue;
                            }
                            Console.WriteLine("shop info Khong The Chay Lai!");
                            continue;

                        case "quest&reload":
                            if (QuestMgr.ReLoad())
                            {
                                Console.WriteLine("quest info Da Duoc Chay Lai!");
                                continue;
                            }
                            Console.WriteLine("quest info Khong The Chay Lai!");
                            continue;

                        case "fusion&reload":
                            if (FusionMgr.ReLoad())
                            {
                                Console.WriteLine("fusion info Da Duoc Chay Lai!");
                                continue;
                            }
                            Console.WriteLine("fusion info Khong The Chay Lai!");
                            continue;

                        case "consortia&reload":
                            if (ConsortiaMgr.ReLoad())
                            {
                                Console.WriteLine("consortiaMgr info Da Duoc Chay Lai!");
                                continue;
                            }
                            Console.WriteLine("consortiaMgr info Khong The Chay Lai!");
                            continue;

                        case "rate&reload":
                            if (RateMgr.ReLoad())
                            {
                                Console.WriteLine("Rate Rate Da Duoc Chay Lai!");
                                continue;
                            }
                            Console.WriteLine("Rate Rate Khong The Chay Lai!");
                            continue;

                        case "npc":
                            if (NPCInfoMgr.ReLoad())
                            {
                                Console.WriteLine("NPCInfo Da Duoc Chay Lai!");
                                continue;
                            }
                            Console.WriteLine("NPCInfo Khong The Chay Lai!");
                            continue;

                        case "fight&reload":
                            if (FightRateMgr.ReLoad())
                            {
                                Console.WriteLine("FightRateMgr Da Duoc Chay Lai!");
                                continue;
                            }
                            Console.WriteLine("FightRateMgr Khong The Chay Lai!");
                            continue;

                        case "dailyaward&reload":
                            if (AwardMgr.ReLoad())
                            {
                                Console.WriteLine("dailyaward Da Duoc Chay Lai!");
                                continue;
                            }
                            Console.WriteLine("dailyaward Khong The Chay Lai!");
                            continue;
                      
                        case "reload":
                            BallMgr.ReLoad();
                            DropMgr.ReLoad();
                            ShopMgr.ReLoad();
                            MapMgr.ReLoadMap();
                            MapMgr.ReLoadMapServer();
                            PropItemMgr.Reload();
                            ItemMgr.ReLoad();
                            QuestMgr.ReLoad();
                            FusionMgr.ReLoad();
                            ConsortiaMgr.ReLoad();
                            NPCInfoMgr.ReLoad();
                            RateMgr.ReLoad();
                            FightRateMgr.ReLoad();
                            AwardMgr.ReLoad();
                            LanguageMgr.Reload("");
                            Console.WriteLine("Recarregado");
                            continue;

                        case "language&reload":
                            if (LanguageMgr.Reload(""))
                            {
                                Console.WriteLine("Reprise da linguagem!");
                                continue;
                            }
                            Console.WriteLine("Idioma não pode ser reproduzido novamente!");
                            continue;
                        case "mensagem":
                            string msg = Console.ReadLine();
                            WorldMgr.SendSysNotice(msg);
                            continue;

                        case "nickname":
                            {
                                Console.WriteLine("Digite seu nome");
                                string nickName = Console.ReadLine();
                                string playerStringByPlayerNickName = WorldMgr.GetPlayerStringByPlayerNickName(nickName);
                                Console.WriteLine(playerStringByPlayerNickName);
                                continue;
                            }
                        case "ban":
                            {
                                
                                Console.WriteLine("Digite o UserId");
                                try
                                {
                                    int userid = Convert.ToInt32(Console.ReadLine());
                                    Console.WriteLine("Escreva o tempo De Ban em Dias");
                                    int BanTime = Convert.ToInt32(Console.ReadLine());
                                    GamePlayer jogador =  WorldMgr.GetPlayerById(userid);
                                    DateTime dt2 = DateTime.Now;
                                    dt2 = dt2.AddDays(BanTime);
                                    if(jogador != null)
                                    {
                                        jogador.SendMessage(string.Format("Você Foi Banido por {0} Dias", BanTime));
                                    }

                                    using (ManageBussiness managebussiness = new ManageBussiness())
                                    {
                                        managebussiness.ForbidPlayerByUserID(userid, dt2, false);
                                    }
                                    Console.WriteLine("O UserID: " + userid +" "+ jogador.PlayerCharacter.UserName + " Foi Banido do servidor.");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Tem Que Ser um Numero");
                                }
                                continue;
                            }
                        case "kick":
                            {
                                
                                Console.WriteLine("Digite o UserId");
                                try
                                {
                                    int userid = Convert.ToInt32(Console.ReadLine());
                                    GamePlayer jogador = WorldMgr.GetPlayerById(userid);
                                    if (jogador != null)
                                    {
                                        jogador.SendMessage(string.Format("Você Foi kikado do server pelo Gm"));
                                        jogador.Disconnect();
                                    }
                                    else
                                    {
                                        Console.WriteLine("Jogador Encontra-se offline");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Tem Que Ser um Numero");
                                }
                                continue;
                            }

                    }
                    if (text.Length > 0)
                    {
                        if (text[0] == '/')
                        {
                            text = text.Remove(0, 1);
                            text = text.Insert(0, "&");
                        }
                        try
                        {
                            if (!CommandMgr.HandleCommandNoPlvl(client, text))
                            {
                                Console.WriteLine("Pedido inválido: " + text);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }
                }
                catch (Exception value)
                {
                    Console.WriteLine(value);
                }
            }
            if (GameServer.Instance != null)
            {
                GameServer.Instance.Stop();
            }
            LogManager.Shutdown();
        }
        private static void ShutDownCallBack(object state)
        {
            ConsoleStart._count--;
            Console.WriteLine(string.Format("O servidor será perdido após {0} minutos!", ConsoleStart._count));
            GameClient[] allClients = GameServer.Instance.GetAllClients();
            GameClient[] array = allClients;
            for (int i = 0; i < array.Length; i++)
            {
                GameClient gameClient = array[i];
                if (gameClient.Out != null)
                {
                    gameClient.Out.SendMessage(eMessageType.Normal, string.Format("{0}{1}{2}", LanguageMgr.GetTranslation("Game.Service.actions.ShutDown1", new object[0]), ConsoleStart._count, LanguageMgr.GetTranslation("Game.Service.actions.ShutDown2", new object[0])));
                }
            }
            if (ConsoleStart._count == 0)
            {
                ConsoleStart._timer.Dispose();
                ConsoleStart._timer = null;
                GameServer.Instance.Stop();
                Console.WriteLine("O servidor retornou!");
            }
        }
        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int SetConsoleCtrlHandler(ConsoleStart.ConsoleCtrlDelegate HandlerRoutine, bool add);
        private static int ConsoleCtrHandler(ConsoleStart.ConsoleEvent e)
        {
            ConsoleStart.SetConsoleCtrlHandler(ConsoleStart.handler, false);
            if (GameServer.Instance != null)
            {
                GameServer.Instance.Stop();
            }
            return 0;
        }
    }
}
