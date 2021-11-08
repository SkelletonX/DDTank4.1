// Decompiled with JetBrains decompiler
// Type: Center.Server.LoginMgr
// Assembly: Center.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4391F35A-9CAD-44A9-AFE0-208E0547A0DD
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Center\Center.Server.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace Center.Server
{
  public class LoginMgr
  {
    private static Dictionary<int, Player> m_players = new Dictionary<int, Player>();
    private static object syc_obj = new object();

    public static void CreatePlayer(Player player)
    {
      Player player1 = (Player) null;
      lock (LoginMgr.syc_obj)
      {
        player.LastTime = DateTime.Now.Ticks;
        if (LoginMgr.m_players.ContainsKey(player.Id))
        {
          player1 = LoginMgr.m_players[player.Id];
          player.State = player1.State;
          player.CurrentServer = player1.CurrentServer;
          LoginMgr.m_players[player.Id] = player;
        }
        else
        {
          player1 = LoginMgr.GetPlayerByName(player.Name);
          if (player1 != null && LoginMgr.m_players.ContainsKey(player1.Id))
            LoginMgr.m_players.Remove(player1.Id);
          player.State = ePlayerState.NotLogin;
          LoginMgr.m_players.Add(player.Id, player);
        }
      }
      if (player1 == null || player1.CurrentServer == null)
        return;
      player1.CurrentServer.SendKitoffUser(player1.Id);
    }

    public static Player[] GetAllPlayer()
    {
      lock (LoginMgr.syc_obj)
        return LoginMgr.m_players.Values.ToArray<Player>();
    }

    public static int GetOnlineCount()
    {
      Player[] allPlayer = LoginMgr.GetAllPlayer();
      int num = 0;
      foreach (Player player in allPlayer)
      {
        if ((uint) player.State > 0U)
          ++num;
      }
      return num;
    }

    public static Dictionary<int, int> GetOnlineForLine()
    {
      Dictionary<int, int> dictionary1 = new Dictionary<int, int>();
      foreach (Player player in LoginMgr.GetAllPlayer())
      {
        if (player.CurrentServer != null)
        {
          if (dictionary1.ContainsKey(player.CurrentServer.Info.ID))
          {
            Dictionary<int, int> dictionary2;
            int id;
            (dictionary2 = dictionary1)[id = player.CurrentServer.Info.ID] = dictionary2[id] + 1;
          }
          else
            dictionary1.Add(player.CurrentServer.Info.ID, 1);
        }
      }
      return dictionary1;
    }

    public static Player GetPlayer(int playerId)
    {
      lock (LoginMgr.syc_obj)
      {
        if (LoginMgr.m_players.ContainsKey(playerId))
          return LoginMgr.m_players[playerId];
      }
      return (Player) null;
    }

    public static Player GetPlayerByName(string name)
    {
      Player[] allPlayer = LoginMgr.GetAllPlayer();
      if (allPlayer != null)
      {
        foreach (Player player in allPlayer)
        {
          if (player.Name == name)
            return player;
        }
      }
      return (Player) null;
    }

    public static ServerClient GetServerClient(int playerId)
    {
      Player player = LoginMgr.GetPlayer(playerId);
      if (player != null)
        return player.CurrentServer;
      return (ServerClient) null;
    }

    public static List<Player> GetServerPlayers(ServerClient server)
    {
      List<Player> playerList = new List<Player>();
      foreach (Player player in LoginMgr.GetAllPlayer())
      {
        if (player.CurrentServer == server)
          playerList.Add(player);
      }
      return playerList;
    }

    public static void PlayerLogined(int id, ServerClient server)
    {
      lock (LoginMgr.syc_obj)
      {
        if (!LoginMgr.m_players.ContainsKey(id))
          return;
        Player player = LoginMgr.m_players[id];
        if (player != null)
        {
          player.CurrentServer = server;
          player.State = ePlayerState.Play;
        }
      }
    }

    public static void PlayerLoginOut(int id, ServerClient server)
    {
      lock (LoginMgr.syc_obj)
      {
        if (!LoginMgr.m_players.ContainsKey(id))
          return;
        Player player = LoginMgr.m_players[id];
        if (player != null && player.CurrentServer == server)
        {
          player.CurrentServer = (ServerClient) null;
          player.State = ePlayerState.NotLogin;
        }
      }
    }

    public static void RemovePlayer(int playerId)
    {
      lock (LoginMgr.syc_obj)
      {
        if (!LoginMgr.m_players.ContainsKey(playerId))
          return;
        LoginMgr.m_players.Remove(playerId);
      }
    }

    public static void RemovePlayer(List<Player> players)
    {
      lock (LoginMgr.syc_obj)
      {
        foreach (Player player in players)
          LoginMgr.m_players.Remove(player.Id);
      }
    }

    public static bool TryLoginPlayer(int id, ServerClient server)
    {
      lock (LoginMgr.syc_obj)
      {
        if (!LoginMgr.m_players.ContainsKey(id))
          return false;
        Player player = LoginMgr.m_players[id];
        if (player.CurrentServer == null)
        {
          player.CurrentServer = server;
          player.State = ePlayerState.Logining;
          return true;
        }
        if (player.State == ePlayerState.Play)
          player.CurrentServer.SendKitoffUser(id);
        return false;
      }
    }
  }
}
