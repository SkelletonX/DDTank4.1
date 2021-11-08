// Decompiled with JetBrains decompiler
// Type: Bussiness.WebLogin.PassPortSoap
// Assembly: Bussiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C2537CFF-7BDB-4A06-BE9C-A8074B2C97E3
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Bussiness.dll

using System.CodeDom.Compiler;
using System.ServiceModel;

namespace Bussiness.WebLogin
{
  [ServiceContract(ConfigurationName = "WebLogin.PassPortSoap", Namespace = "dandantang")]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  public interface PassPortSoap
  {
    [OperationContract(Action = "dandantang/ChenckValidate", ReplyAction = "*")]
    ChenckValidateResponse ChenckValidate(ChenckValidateRequest request);

    [OperationContract(Action = "dandantang/Get_UserSex", ReplyAction = "*")]
    Get_UserSexResponse Get_UserSex(Get_UserSexRequest request);
  }
}
