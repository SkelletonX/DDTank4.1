// Decompiled with JetBrains decompiler
// Type: Game.Logic.Spells.SpellMgr
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Base.Events;
using Game.Logic.Phy.Object;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Game.Logic.Spells
{
  public class SpellMgr
  {
    private static Dictionary<int, ISpellHandler> handles = new Dictionary<int, ISpellHandler>();
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public static void ExecuteSpell(BaseGame game, Player player, ItemTemplateInfo item)
    {
      try
      {
        SpellMgr.LoadSpellHandler(item.Property1).Execute(game, player, item);
      }
      catch (Exception ex)
      {
        SpellMgr.log.Error((object) "Execute Spell Error:", ex);
      }
    }

    public static ISpellHandler LoadSpellHandler(int code)
    {
      return SpellMgr.handles[code];
    }

    [ScriptLoadedEvent]
    public static void OnScriptCompiled(RoadEvent ev, object sender, EventArgs args)
    {
      SpellMgr.handles.Clear();
      int num = SpellMgr.SearchSpellHandlers(Assembly.GetAssembly(typeof (BaseGame)));
      if (!SpellMgr.log.IsInfoEnabled)
        return;
      SpellMgr.log.Info((object) ("SpellMgr: Loaded " + (object) num + " spell handlers from GameServer Assembly!"));
    }

    protected static void RegisterSpellHandler(int type, ISpellHandler handle)
    {
      SpellMgr.handles.Add(type, handle);
    }

    protected static int SearchSpellHandlers(Assembly assembly)
    {
      int num = 0;
      foreach (Type type in assembly.GetTypes())
      {
        if (type.IsClass && type.GetInterface("Game.Logic.Spells.ISpellHandler") != (Type) null)
        {
          SpellAttibute[] customAttributes = (SpellAttibute[]) type.GetCustomAttributes(typeof (SpellAttibute), true);
          if ((uint) customAttributes.Length > 0U)
          {
            ++num;
            SpellMgr.RegisterSpellHandler(customAttributes[0].Type, Activator.CreateInstance(type) as ISpellHandler);
          }
        }
      }
      return num;
    }
  }
}
