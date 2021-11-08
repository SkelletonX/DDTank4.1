// Decompiled with JetBrains decompiler
// Type: Game.Server.WebLogin.ChenckValidateResponse
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace Game.Server.WebLogin
{
  [MessageContract(IsWrapped = false)]
  [EditorBrowsable(EditorBrowsableState.Advanced)]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [DebuggerStepThrough]
  public class ChenckValidateResponse
  {
    [MessageBodyMember(Name = "ChenckValidateResponse", Namespace = "dandantang", Order = 0)]
    public ChenckValidateResponseBody Body;

    public ChenckValidateResponse()
    {
    }

    public ChenckValidateResponse(ChenckValidateResponseBody Body)
    {
      this.Body = Body;
    }
  }
}
