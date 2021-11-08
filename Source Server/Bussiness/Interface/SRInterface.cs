// Decompiled with JetBrains decompiler
// Type: Bussiness.Interface.SRInterface
// Assembly: Bussiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C2537CFF-7BDB-4A06-BE9C-A8074B2C97E3
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Bussiness.dll

using Bussiness.WebLogin;
using System;

namespace Bussiness.Interface
{
  public class SRInterface : BaseInterface
  {
    public override bool GetUserSex(string name)
    {
      try
      {
        return new PassPortSoapClient().Get_UserSex(string.Empty, name).Value;
      }
      catch (Exception ex)
      {
        BaseInterface.log.Error((object) "获取性别失败", ex);
        return true;
      }
    }
  }
}
