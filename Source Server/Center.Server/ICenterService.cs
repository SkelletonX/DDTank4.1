// Decompiled with JetBrains decompiler
// Type: Center.Server.ICenterService
// Assembly: Center.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4391F35A-9CAD-44A9-AFE0-208E0547A0DD
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Center\Center.Server.dll

using System.Collections.Generic;
using System.ServiceModel;

namespace Center.Server
{
  [ServiceContract]
  public interface ICenterService
  {
    [OperationContract]
    int AASGetState();

    [OperationContract]
    bool AASUpdateState(bool state);

    [OperationContract]
    bool ActivePlayer(bool isActive);

    [OperationContract]
    bool CreatePlayer(int id, string name, string password, bool isFirst);

    [OperationContract]
    bool ChargeMoney(int userID, string chargeID);

    [OperationContract]
    int ExperienceRateUpdate(int serverId);

    [OperationContract]
    int GetConfigState(int type);

    [OperationContract]
    List<ServerData> GetServerList();

    [OperationContract]
    bool KitoffUser(int playerID, string msg);

    [OperationContract]
    bool MailNotice(int playerID);

    [OperationContract]
    int NoticeServerUpdate(int serverId, int type);

    [OperationContract]
    bool Reload(string type);

    [OperationContract]
    bool ReLoadServerList();

    [OperationContract]
    bool SystemNotice(string msg);

    [OperationContract]
    bool UpdateConfigState(int type, bool state);

    [OperationContract]
    bool ValidateLoginAndGetID(string name, string password, ref int userID, ref bool isFirst);
  }
}
