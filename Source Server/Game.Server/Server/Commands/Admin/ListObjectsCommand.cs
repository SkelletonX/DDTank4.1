// Decompiled with JetBrains decompiler
// Type: Game.Server.Commands.Admin.ListObjectsCommand
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base;
using Game.Logic;
using Game.Server.Battle;
using Game.Server.GameObjects;
using Game.Server.Games;
using Game.Server.Managers;
using Game.Server.Rooms;
using System;
using System.Collections.Generic;

namespace Game.Server.Commands.Admin
{
  [Cmd("&list", ePrivLevel.Player, "List the objects info in game", new string[] {"   /list [Option1][Option2] ...", "eg:    /list -g :list all game objects", "       /list -c :list all client objects", "       /list -p :list all gameplaye objects", "       /list -r :list all room objects", "       /list -b :list all battle servers"})]
  public class ListObjectsCommand : AbstractCommandHandler, ICommandHandler
  {
    public bool OnCommand(BaseClient client, string[] args)
    {
      if (args.Length > 1)
      {
        string str = args[1];
        if (!(str == "-c"))
        {
          if (!(str == "-p"))
          {
            if (!(str == "-r"))
            {
              if (!(str == "-g"))
              {
                if (str == "-b")
                {
                  Console.WriteLine("battle list:");
                  Console.WriteLine("-------------------------------");
                  List<BattleServer> allBattles = BattleMgr.GetAllBattles();
                  foreach (object obj in allBattles)
                    Console.WriteLine(obj.ToString());
                  Console.WriteLine("-------------------------------");
                  Console.WriteLine(string.Format("total:{0}", (object) allBattles.Count));
                  return true;
                }
                this.DisplaySyntax(client);
              }
              else
              {
                Console.WriteLine("game list:");
                Console.WriteLine("-------------------------------");
                List<BaseGame> allGame = GameMgr.GetAllGame();
                foreach (object obj in allGame)
                  Console.WriteLine(obj.ToString());
                Console.WriteLine("-------------------------------");
                Console.WriteLine(string.Format("total:{0}", (object) allGame.Count));
                return true;
              }
            }
            else
            {
              Console.WriteLine("room list:");
              Console.WriteLine("-------------------------------");
              List<BaseRoom> allUsingRoom = RoomMgr.GetAllUsingRoom();
              foreach (object obj in allUsingRoom)
                Console.WriteLine(obj.ToString());
              Console.WriteLine("-------------------------------");
              Console.WriteLine(string.Format("total:{0}", (object) allUsingRoom.Count));
              return true;
            }
          }
          else
          {
            Console.WriteLine("player list:");
            Console.WriteLine("-------------------------------");
            GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
            foreach (object obj in allPlayers)
              Console.WriteLine(obj.ToString());
            Console.WriteLine("-------------------------------");
            Console.WriteLine(string.Format("total:{0}", (object) allPlayers.Length));
            return true;
          }
        }
        else
        {
          Console.WriteLine("client list:");
          Console.WriteLine("-------------------------------");
          GameClient[] allClients = GameServer.Instance.GetAllClients();
          foreach (object obj in allClients)
            Console.WriteLine(obj.ToString());
          Console.WriteLine("-------------------------------");
          Console.WriteLine(string.Format("total:{0}", (object) allClients.Length));
          return true;
        }
      }
      else
        this.DisplaySyntax(client);
      return true;
    }
  }
}
