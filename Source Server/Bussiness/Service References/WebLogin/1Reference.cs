// Decompiled with JetBrains decompiler
// Type: Bussiness.WebLogin.ChenckValidateRequestBody
// Assembly: Bussiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C2537CFF-7BDB-4A06-BE9C-A8074B2C97E3
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Bussiness.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Bussiness.WebLogin
{
  [EditorBrowsable(EditorBrowsableState.Advanced)]
  [DataContract(Namespace = "dandantang")]
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  public class ChenckValidateRequestBody
  {
    [DataMember(EmitDefaultValue = false, Order = 0)]
    public string applicationname;
    [DataMember(EmitDefaultValue = false, Order = 1)]
    public string username;
    [DataMember(EmitDefaultValue = false, Order = 2)]
    public string password;

    public ChenckValidateRequestBody()
    {
    }

    public ChenckValidateRequestBody(string applicationname, string username, string password)
    {
      this.applicationname = applicationname;
      this.username = username;
      this.password = password;
    }
  }
}
