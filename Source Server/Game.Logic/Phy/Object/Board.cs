// Decompiled with JetBrains decompiler
// Type: Game.Logic.Phy.Object.Board
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using System.Drawing;

namespace Game.Logic.Phy.Object
{
  public class Board : Physics
  {
    private int _type;
    private int _templateId;
    private int _valueType;
    private int _value;
    private int _rotate;
    private int _state;

    public int Type
    {
      get
      {
        return this._type;
      }
      set
      {
        this._type = value;
      }
    }

    public int TemplateId
    {
      get
      {
        return this._templateId;
      }
      set
      {
        this._templateId = value;
      }
    }

    public int ValueType
    {
      get
      {
        return this._valueType;
      }
      set
      {
        this._valueType = value;
      }
    }

    public int Value
    {
      get
      {
        return this._value;
      }
      set
      {
        this._value = value;
      }
    }

    public int Rotate
    {
      get
      {
        return this._rotate;
      }
      set
      {
        this._rotate = value;
      }
    }

    public int State
    {
      get
      {
        return this._state;
      }
      set
      {
        this._state = value;
      }
    }

    public Board(
      int id,
      int type,
      int templateId,
      int valueType,
      int value,
      int rotate,
      int state)
      : base(id)
    {
      this._type = type;
      this._templateId = templateId;
      this._valueType = valueType;
      this._value = value;
      this._rotate = rotate;
      this._state = state;
      this.m_rect = new Rectangle(-52, -9, 104, 20);
    }

    public Board(int id, int templateId, int valueType, int value)
      : base(id)
    {
      this._templateId = templateId;
      this._valueType = valueType;
      this._value = value;
      this.m_rect = new Rectangle(-52, -9, 104, 20);
    }
  }
}
