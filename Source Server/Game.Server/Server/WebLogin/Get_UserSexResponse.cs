// Decompiled with JetBrains decompiler
// Type: Game.Server.WebLogin.Get_UserSexResponse
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace Game.Server.WebLogin
{
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [DebuggerStepThrough]
  [EditorBrowsable(EditorBrowsableState.Advanced)]
  [MessageContract(IsWrapped = false)]
  public class Get_UserSexResponse
  {
    [MessageBodyMember(Name = "Get_UserSexResponse", Namespace = "dandantang", Order = 0)]
    public Get_UserSexResponseBody Body;

    public Get_UserSexResponse()
    {
    }

    public Get_UserSexResponse(Get_UserSexResponseBody Body)
    {
      this.Body = Body;
    }
  }
}
