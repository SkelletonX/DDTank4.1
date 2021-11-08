// Decompiled with JetBrains decompiler
// Type: Bussiness.WebLogin.ChenckValidateRequest
// Assembly: Bussiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C2537CFF-7BDB-4A06-BE9C-A8074B2C97E3
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Bussiness.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace Bussiness.WebLogin
{
  [EditorBrowsable(EditorBrowsableState.Advanced)]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [DebuggerStepThrough]
  [MessageContract(IsWrapped = false)]
  public class ChenckValidateRequest
  {
    [MessageBodyMember(Name = "ChenckValidate", Namespace = "dandantang", Order = 0)]
    public ChenckValidateRequestBody Body;

    public ChenckValidateRequest()
    {
    }

    public ChenckValidateRequest(ChenckValidateRequestBody Body)
    {
      this.Body = Body;
    }
  }
}
