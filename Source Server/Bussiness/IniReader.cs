// Decompiled with JetBrains decompiler
// Type: Bussiness.IniReader
// Assembly: Bussiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C2537CFF-7BDB-4A06-BE9C-A8074B2C97E3
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Bussiness.dll

using System.Runtime.InteropServices;
using System.Text;

namespace Bussiness
{
  public class IniReader
  {
    private string FilePath;

    public IniReader(string _FilePath)
    {
      this.FilePath = _FilePath;
    }

    public string GetIniString(string Section, string Key)
    {
      StringBuilder retVal = new StringBuilder(2550);
      IniReader.GetPrivateProfileString(Section, Key, "", retVal, 2550, this.FilePath);
      return retVal.ToString();
    }

    [DllImport("kernel32")]
    private static extern int GetPrivateProfileString(
      string section,
      string key,
      string def,
      StringBuilder retVal,
      int size,
      string filePath);
  }
}
