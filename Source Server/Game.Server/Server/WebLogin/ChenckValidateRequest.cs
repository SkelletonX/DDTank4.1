﻿// Decompiled with JetBrains decompiler
// Type: Game.Server.WebLogin.ChenckValidateRequest
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
