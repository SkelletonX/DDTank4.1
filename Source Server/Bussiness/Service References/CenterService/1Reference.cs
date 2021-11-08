// Decompiled with JetBrains decompiler
// Type: Bussiness.CenterService.CenterServiceClient
// Assembly: Bussiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C2537CFF-7BDB-4A06-BE9C-A8074B2C97E3
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Bussiness.dll

using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Bussiness.CenterService
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  public class CenterServiceClient : ClientBase<ICenterService>, ICenterService
    {
    public CenterServiceClient()
    {
    }

    public CenterServiceClient(string endpointConfigurationName)
      : base(endpointConfigurationName)
    {
    }

    public CenterServiceClient(string endpointConfigurationName, string remoteAddress)
      : base(endpointConfigurationName, remoteAddress)
    {
    }

    public CenterServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress)
      : base(endpointConfigurationName, remoteAddress)
    {
    }

    public CenterServiceClient(Binding binding, EndpointAddress remoteAddress)
      : base(binding, remoteAddress)
    {
    }

    public ServerData[] GetServerList()
    {
      return this.Channel.GetServerList();
    }

    public bool ChargeMoney(int userID, string chargeID)
    {
      return this.Channel.ChargeMoney(userID, chargeID);
    }

    public bool SystemNotice(string msg)
    {
      return this.Channel.SystemNotice(msg);
    }

    public bool KitoffUser(int playerID, string msg)
    {
      return this.Channel.KitoffUser(playerID, msg);
    }

    public bool ReLoadServerList()
    {
      return this.Channel.ReLoadServerList();
    }

    public bool MailNotice(int playerID)
    {
      return this.Channel.MailNotice(playerID);
    }

    public bool ActivePlayer(bool isActive)
    {
      return this.Channel.ActivePlayer(isActive);
    }

    public bool CreatePlayer(int id, string name, string password, bool isFirst)
    {
      return this.Channel.CreatePlayer(id, name, password, isFirst);
    }

    public bool ValidateLoginAndGetID(
      string name,
      string password,
      ref int userID,
      ref bool isFirst)
    {
      return this.Channel.ValidateLoginAndGetID(name, password, ref userID, ref isFirst);
    }

    public bool AASUpdateState(bool state)
    {
      return this.Channel.AASUpdateState(state);
    }

    public int AASGetState()
    {
      return this.Channel.AASGetState();
    }

    public int ExperienceRateUpdate(int serverId)
    {
      return this.Channel.ExperienceRateUpdate(serverId);
    }

    public int NoticeServerUpdate(int serverId, int type)
    {
      return this.Channel.NoticeServerUpdate(serverId, type);
    }

    public bool UpdateConfigState(int type, bool state)
    {
      return this.Channel.UpdateConfigState(type, state);
    }

    public int GetConfigState(int type)
    {
      return this.Channel.GetConfigState(type);
    }

    public bool Reload(string type)
    {
      return this.Channel.Reload(type);
    }
  }
}
