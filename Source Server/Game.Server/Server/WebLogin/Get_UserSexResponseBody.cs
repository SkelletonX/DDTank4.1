// Decompiled with JetBrains decompiler
// Type: Game.Server.WebLogin.Get_UserSexResponseBody
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Game.Server.WebLogin
{
  [EditorBrowsable(EditorBrowsableState.Advanced)]
  [DataContract(Namespace = "dandantang")]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [DebuggerStepThrough]
  public class Get_UserSexResponseBody
  {
    [DataMember(Order = 0)]
    public bool? Get_UserSexResult;

    public Get_UserSexResponseBody()
    {
    }

    public Get_UserSexResponseBody(bool? Get_UserSexResult)
    {
      this.Get_UserSexResult = Get_UserSexResult;
    }
  }
}
