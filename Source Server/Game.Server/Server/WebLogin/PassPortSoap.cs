﻿// Decompiled with JetBrains decompiler
// Type: Game.Server.WebLogin.PassPortSoap
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using System.CodeDom.Compiler;
using System.ServiceModel;

namespace Game.Server.WebLogin
{
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [ServiceContract(ConfigurationName = "WebLogin.PassPortSoap", Namespace = "dandantang")]
  public interface PassPortSoap
  {
    [OperationContract(Action = "dandantang/ChenckValidate", ReplyAction = "*")]
    ChenckValidateResponse ChenckValidate(ChenckValidateRequest request);

    [OperationContract(Action = "dandantang/Get_UserSex", ReplyAction = "*")]
    Get_UserSexResponse Get_UserSex(Get_UserSexRequest request);
  }
}
