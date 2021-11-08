// Decompiled with JetBrains decompiler
// Type: Game.Server.Commands.Admin.ReloadCommand
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using Game.Base;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Server.Commands.Admin
{
  [Cmd("&load", ePrivLevel.Player, "Load the metedata.", new string[] {"   /load  [option]...  ", "Option:    /config     :Application config file.", "           /shop       :ShopMgr.ReLoad().", "           /item       :ItemMgr.Reload().", "           /property   :Game properties."})]
  public class ReloadCommand : AbstractCommandHandler, ICommandHandler
  {
    public bool OnCommand(BaseClient client, string[] args)
    {
      if (args.Length > 1)
      {
        StringBuilder stringBuilder1 = new StringBuilder();
        StringBuilder stringBuilder2 = new StringBuilder();
        if (((IEnumerable<string>) args).Contains<string>("/cmd"))
        {
          CommandMgr.LoadCommands();
          this.DisplayMessage(client, "Command load success!");
          stringBuilder1.Append("/cmd,");
        }
        if (((IEnumerable<string>) args).Contains<string>("/config"))
        {
          GameServer.Instance.Configuration.Refresh();
          this.DisplayMessage(client, "Application config file load success!");
          stringBuilder1.Append("/config,");
        }
        if (((IEnumerable<string>) args).Contains<string>("/property"))
        {
          GameProperties.Refresh();
          this.DisplayMessage(client, "Game properties load success!");
          stringBuilder1.Append("/property,");
        }
        if (((IEnumerable<string>) args).Contains<string>("/item"))
        {
          if (ItemMgr.ReLoad())
          {
            this.DisplayMessage(client, "Items load success!");
            stringBuilder1.Append("/item,");
          }
          else
          {
            this.DisplayMessage(client, "Items load failed!");
            stringBuilder2.Append("/item,");
          }
        }
        if (((IEnumerable<string>) args).Contains<string>("/shop"))
        {
          if (ItemMgr.ReLoad())
          {
            this.DisplayMessage(client, "Shops load success!");
            stringBuilder1.Append("/shop,");
          }
          else
          {
            this.DisplayMessage(client, "Shops load failed!");
            stringBuilder2.Append("/shop,");
          }
        }
        if (stringBuilder1.Length == 0 && stringBuilder2.Length == 0)
        {
          this.DisplayMessage(client, "Nothing executed!");
          this.DisplaySyntax(client);
        }
        else
        {
          this.DisplayMessage(client, "Success Options:    " + stringBuilder1.ToString());
          if (stringBuilder2.Length > 0)
          {
            this.DisplayMessage(client, "Faile Options:      " + stringBuilder2.ToString());
            return false;
          }
        }
        return true;
      }
      this.DisplaySyntax(client);
      return true;
    }
  }
}
