
using System;

namespace Game.Logic.Cmd
{
  public class GameCommandAttribute : Attribute
  {
    private int m_code;
    private string m_description;

    public GameCommandAttribute(int code, string description)
    {
      this.m_code = code;
      this.m_description = description;
    }

    public int Code
    {
      get
      {
        return this.m_code;
      }
    }

    public string Description
    {
      get
      {
        return this.m_description;
      }
    }
  }
}
