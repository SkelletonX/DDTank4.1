// Decompiled with JetBrains decompiler
// Type: Game.Server.WebLogin.Get_UserSexRequest
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace Game.Server.WebLogin
{
  [EditorBrowsable(EditorBrowsableState.Advanced)]
  [MessageContract(IsWrapped = false)]
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
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
