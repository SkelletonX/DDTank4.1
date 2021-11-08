// Decompiled with JetBrains decompiler
// Type: Game.Server.Managers.FusionCombined
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using SqlDataProvider.Data;

namespace Game.Server.Managers
{
  public class FusionCombined
  {
    public static Items_Fusion_List_Info[] m_itemsfusionlist;
    public static FusionCombinedInfo[] m_listFusionCombined;

    public static FusionCombinedInfo[] ListCombinedFusion()
    {
      using (ProduceBussiness produceBussiness = new ProduceBussiness())
        FusionCombined.m_itemsfusionlist = produceBussiness.GetAllFusionList();
      return FusionCombined.m_listFusionCombined;
    }
  }
}
