// Decompiled with JetBrains decompiler
// Type: Bussiness.WebLogin.Get_UserSexRequest
// Assembly: Bussiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C2537CFF-7BDB-4A06-BE9C-A8074B2C97E3
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Bussiness.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace Bussiness.WebLogin
{
  [DebuggerStepThrough]
  [EditorBrowsable(EditorBrowsableState.Advanced)]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(IsWrapped = false)]
  public class Get_UserSexRequest
  {
    [MessageBodyMember(Name = "Get_UserSex", Namespace = "dandantang", Order = 0)]
    public Get_UserSexRequestBody Body;

    public Get_UserSexRequest()
    {
    }

    public Get_UserSexRequest(Get_UserSexRequestBody Body)
    {
      this.Body = Body;
    }
  }
}
