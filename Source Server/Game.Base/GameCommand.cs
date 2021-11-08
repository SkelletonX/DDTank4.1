// Decompiled with JetBrains decompiler
// Type: Game.Base.GameCommand
// Assembly: Game.Base, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2C15C00-C3DB-415D-8006-692895AE7555
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Base.dll

namespace Game.Base
{
  public class GameCommand
  {
    public string[] m_usage;
    public string m_cmd;
    public uint m_lvl;
    public string m_desc;
    public ICommandHandler m_cmdHandler;
  }
}
