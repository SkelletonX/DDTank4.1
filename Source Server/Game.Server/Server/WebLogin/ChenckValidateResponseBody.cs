// Decompiled with JetBrains decompiler
// Type: Game.Server.WebLogin.ChenckValidateResponseBody
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Game.Server.WebLogin
{
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [DebuggerStepThrough]
  [DataContract(Namespace = "dandantang")]
  [EditorBrowsable(EditorBrowsableState.Advanced)]
  public class ChenckValidateResponseBody
  {
    [DataMember(EmitDefaultValue = false, Order = 0)]
    public string ChenckValidateResult;

    public ChenckValidateResponseBody()
    {
    }

    public ChenckValidateResponseBody(string ChenckValidateResult)
    {
      this.ChenckValidateResult = ChenckValidateResult;
    }
  }
}
