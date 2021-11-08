// Decompiled with JetBrains decompiler
// Type: Game.Server.Managers.LoginMgr
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Server.GameObjects;
using System.Collections.Generic;

namespace Game.Server.Managers
{
  public class LoginMgr
  {
    private static object _locker = new object();
    private static Dictionary<int, GameClient> _players = new Dictionary<int, GameClient>();

    public static void Add(int player, GameClient server)
    {
      GameClient gameClient = (GameClient) null;
      lock (LoginMgr._locker)
      {
        if (LoginMgr._players.ContainsKey(player))
        {
          GameClient player1 = LoginMgr._players[player];
          if (player1 != null)
            gameClient = player1;
          LoginMgr._players[player] = server;
        }
        else
          LoginMgr._players.Add(player, server);
      }
      if (gameClient == null)
        return;
      gameClient.Out.SendKitoff(LanguageMgr.GetTranslation("Game.Server.LoginNext"));
      gameClient.Disconnect();
    }

    public static void ClearLoginPlayer(int playerId)
    {
      GameClient gameClient = (GameClient) null;
      lock (LoginMgr._locker)
      {
        if (LoginMgr._players.ContainsKey(playerId))
        {
          gameClient = LoginMgr._players[playerId];
          LoginMgr._players.Remove(playerId);
        }
      }
      if (gameClient == null)
        return;
      gameClient.Out.SendKitoff(LanguageMgr.GetTranslation("Game.Server.LoginNext"));
      gameClient.Disconnect();
    }

    public static void ClearLoginPlayer(int playerId, GameClient client)
    {
      lock (LoginMgr._locker)
      {
        if (!LoginMgr._players.ContainsKey(playerId) || LoginMgr._players[playerId] != client)
          return;
        LoginMgr._players.Remove(playerId);
      }
    }

    public static bool ContainsUser(int playerId)
    {
      lock (LoginMgr._locker)
        return LoginMgr._players.ContainsKey(playerId) && LoginMgr._players[playerId].IsConnected;
    }

    public static bool ContainsUser(string account)
    {
      lock (LoginMgr._locker)
      {
        foreach (GameClient gameClient in LoginMgr._players.Values)
        {
          if (gameClient != null && gameClient.Player != null && gameClient.Player.Account == account)
            return true;
        }
        return false;
      }
    }

    public static GamePlayer LoginClient(int playerId)
    {
      GameClient gameClient = (GameClient) null;
      lock (LoginMgr._locker)
      {
        if (LoginMgr._players.ContainsKey(playerId))
        {
          gameClient = LoginMgr._players[playerId];
          LoginMgr._players.Remove(playerId);
        }
      }
      return gameClient?.Player;
    }

    public static void Remove(int player)
    {
      lock (LoginMgr._locker)
      {
        if (!LoginMgr._players.ContainsKey(player))
          return;
        LoginMgr._players.Remove(player);
      }
    }
  }
}
