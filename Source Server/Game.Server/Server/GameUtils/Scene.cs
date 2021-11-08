// Decompiled with JetBrains decompiler
// Type: Game.Server.GameUtils.Scene
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base.Packets;
using Game.Server.GameObjects;
using log4net;
using SqlDataProvider.Data;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Game.Server.GameUtils
{
  public class Scene
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    protected ReaderWriterLock _locker = new ReaderWriterLock();
    protected Dictionary<int, GamePlayer> _players = new Dictionary<int, GamePlayer>();

    public Scene(ServerInfo info)
    {
    }

    public bool AddPlayer(GamePlayer player)
    {
      this._locker.AcquireWriterLock(-1);
      try
      {
        if (this._players.ContainsKey(player.PlayerCharacter.ID))
        {
          this._players[player.PlayerCharacter.ID] = player;
          return true;
        }
        this._players.Add(player.PlayerCharacter.ID, player);
        return true;
      }
      finally
      {
        this._locker.ReleaseWriterLock();
      }
    }

    public GamePlayer[] GetAllPlayer()
    {
      GamePlayer[] gamePlayerArray = (GamePlayer[]) null;
      this._locker.AcquireReaderLock(10000);
      try
      {
        gamePlayerArray = this._players.Values.ToArray<GamePlayer>();
      }
      finally
      {
        this._locker.ReleaseReaderLock();
      }
      return gamePlayerArray ?? new GamePlayer[0];
    }

    public GamePlayer GetClientFromID(int id)
    {
      if (this._players.Keys.Contains<int>(id))
        return this._players[id];
      return (GamePlayer) null;
    }

    public void RemovePlayer(GamePlayer player)
    {
      this._locker.AcquireWriterLock(-1);
      try
      {
        if (this._players.ContainsKey(player.PlayerCharacter.ID))
          this._players.Remove(player.PlayerCharacter.ID);
      }
      finally
      {
        this._locker.ReleaseWriterLock();
      }
      GamePlayer[] allPlayer = this.GetAllPlayer();
      GSPacketIn packet = (GSPacketIn) null;
      foreach (GamePlayer gamePlayer in allPlayer)
      {
        if (packet == null)
          packet = gamePlayer.Out.SendSceneRemovePlayer(player);
        else
          gamePlayer.Out.SendTCP(packet);
      }
    }

    public void SendToALL(GSPacketIn pkg)
    {
      this.SendToALL(pkg, (GamePlayer) null);
    }

    public void SendToALL(GSPacketIn pkg, GamePlayer except)
    {
      foreach (GamePlayer gamePlayer in this.GetAllPlayer())
      {
        if (gamePlayer != except)
          gamePlayer.Out.SendTCP(pkg);
      }
    }
  }
}
