// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.PlayerInfoHistory
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

using System;
using System.Collections.Generic;

namespace SqlDataProvider.Data
{
  public class PlayerInfoHistory : DataObject
  {
    private static readonly int m_exist = 15;
    private object m_composeStateLocker = new object();
    private DateTime _lastQuestsTime;
    private DateTime _lastTreasureTime;
    private int _userID;
    private Dictionary<int, int> m_composeState;
    private string m_composeStateString;

    private void ClearExpireComposeState()
    {
      int[] array = new int[this.m_composeState.Keys.Count];
      this.m_composeState.Keys.CopyTo(array, 0);
      Array.Sort<int>(array);
      int exist = PlayerInfoHistory.m_exist;
      for (int index = array.Length - 1; index >= 0; --index)
      {
        --exist;
        if (exist < 0)
          this.m_composeState.Remove(array[index]);
      }
    }

    public int ComposeStateLockIncrement(int key)
    {
      lock (this.m_composeStateLocker)
      {
        this._isDirty = true;
        if (!this.m_composeState.ContainsKey(key))
          this.m_composeState.Add(key, 0);
        Dictionary<int, int> composeState;
        (composeState = this.m_composeState)[key] = composeState[key] + 1;
        return this.m_composeState[key];
      }
    }

    private string JointStateString(Dictionary<int, int> stateDict)
    {
      List<string> stringList = new List<string>();
      Dictionary<int, int>.Enumerator enumerator = this.m_composeState.GetEnumerator();
      while (enumerator.MoveNext())
      {
        KeyValuePair<int, int> current1 = enumerator.Current;
        KeyValuePair<int, int> current2 = enumerator.Current;
        stringList.Add(string.Format("{0}-{1}", (object) current2.Key, (object) current2.Value));
      }
      return string.Join(",", stringList.ToArray());
    }

    private Dictionary<int, int> SplitStateString(string stateString)
    {
      Dictionary<int, int> dictionary = new Dictionary<int, int>();
      string str1 = stateString;
      char[] chArray1 = new char[1]{ ',' };
      foreach (string str2 in str1.Split(chArray1))
      {
        char[] chArray2 = new char[1]{ '-' };
        string[] strArray = str2.Split(chArray2);
        if (strArray.Length == 2)
        {
          int key = int.Parse(strArray[0]);
          if (!dictionary.ContainsKey(key))
            dictionary.Add(key, int.Parse(strArray[1]));
        }
      }
      return dictionary;
    }

    public string ComposeStateString
    {
      get
      {
        this.m_composeStateString = this.JointStateString(this.m_composeState);
        if (this.m_composeStateString.Length > 200)
        {
          this.ClearExpireComposeState();
          this.m_composeStateString = this.JointStateString(this.m_composeState);
          if (this.m_composeStateString.Length > 200)
            throw new ArgumentOutOfRangeException("the compose state string is too long, to fix the error you should clean the column Sys_Users_History.ComposeState in DB");
        }
        return this.m_composeStateString;
      }
      set
      {
        this.m_composeStateString = value;
        this.m_composeState = this.SplitStateString(this.m_composeStateString);
      }
    }

    public DateTime LastQuestsTime
    {
      get
      {
        return this._lastQuestsTime;
      }
      set
      {
        this._lastQuestsTime = value;
      }
    }

    public DateTime LastTreasureTime
    {
      get
      {
        return this._lastTreasureTime;
      }
      set
      {
        this._lastTreasureTime = value;
      }
    }

    public int UserID
    {
      get
      {
        return this._userID;
      }
      set
      {
        this._userID = value;
      }
    }
  }
}
