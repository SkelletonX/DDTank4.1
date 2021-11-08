// Decompiled with JetBrains decompiler
// Type: Bussiness.WebLogin.ChenckValidateResponse
// Assembly: Bussiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C2537CFF-7BDB-4A06-BE9C-A8074B2C97E3
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Bussiness.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace Bussiness.WebLogin
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
