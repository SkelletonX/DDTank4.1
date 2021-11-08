using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server.GameUtils;
using SqlDataProvider.Data;
using System;
using System.Text.RegularExpressions;

namespace Game.Server.Packets.Client
{
  internal class AASInfoSetHandle : IPacketHandler
  {
    private static Regex _objRegex = new Regex("\\d{18}|\\d{15}");
    private static Regex _objRegex1 = new Regex("/^[1-9]\\d{7}((0\\d)|(1[0-2]))(([0|1|2]\\d)|3[0-1])\\d{3}$/");
    private static Regex _objRegex2 = new Regex("/^[1-9]\\d{5}[1-9]\\d{3}((0\\d)|(1[0-2]))(([0|1|2]\\d)|3[0-1])\\d{4}$/");
    private static string[] cities = new string[92]
    {
      null,
      null,
      null,
      null,
      null,
      null,
      null,
      null,
      null,
      null,
      null,
      "北京",
      "天津",
      "河北",
      "山西",
      "内蒙古",
      null,
      null,
      null,
      null,
      null,
      "辽宁",
      "吉林",
      "黑龙江",
      null,
      null,
      null,
      null,
      null,
      null,
      null,
      "上海",
      "江苏",
      "浙江",
      "安微",
      "福建",
      "江西",
      "山东",
      null,
      null,
      null,
      "河南",
      "湖北",
      "湖南",
      "广东",
      "广西",
      "海南",
      null,
      null,
      null,
      "重庆",
      "四川",
      "贵州",
      "云南",
      "西藏",
      null,
      null,
      null,
      null,
      null,
      null,
      "陕西",
      "甘肃",
      "青海",
      "宁夏",
      "新疆",
      null,
      null,
      null,
      null,
      null,
      "台湾",
      null,
      null,
      null,
      null,
      null,
      null,
      null,
      null,
      null,
      "香港",
      "澳门",
      null,
      null,
      null,
      null,
      null,
      null,
      null,
      null,
      "国外"
    };
    private static int[] WI = new int[17]
    {
      7,
      9,
      10,
      5,
      8,
      4,
      2,
      1,
      6,
      3,
      7,
      9,
      10,
      5,
      8,
      4,
      2
    };
    private static char[] checkCode = new char[11]
    {
      '1',
      '0',
      'X',
      '9',
      '8',
      '7',
      '6',
      '5',
      '4',
      '3',
      '2'
    };

    private bool CheckIDNumber(string IDNum)
    {
      bool flag = false;
      if (!AASInfoSetHandle._objRegex.IsMatch(IDNum))
        return false;
      int index1 = int.Parse(IDNum.Substring(0, 2));
      if (AASInfoSetHandle.cities[index1] == null)
        return false;
      if (IDNum.Length == 18)
      {
        int num = 0;
        for (int index2 = 0; index2 < 17; ++index2)
        {
          char ch = IDNum[index2];
          num += int.Parse(ch.ToString()) * AASInfoSetHandle.WI[index2];
        }
        int index3 = num % 11;
        if ((int) IDNum[17] == (int) AASInfoSetHandle.checkCode[index3])
          flag = true;
      }
      return flag;
    }

    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      AASInfo info = new AASInfo()
      {
        UserID = client.Player.PlayerCharacter.ID
      };
      bool result = false;
      bool flag;
      if (packet.ReadBoolean())
      {
        info.Name = "";
        info.IDNumber = "";
        info.State = 0;
        flag = true;
      }
      else
      {
        info.Name = packet.ReadString();
        info.IDNumber = packet.ReadString();
        flag = this.CheckIDNumber(info.IDNumber);
        if (info.IDNumber != "")
        {
          client.Player.IsAASInfo = true;
          int int32_1 = Convert.ToInt32(info.IDNumber.Substring(6, 4));
          int int32_2 = Convert.ToInt32(info.IDNumber.Substring(10, 2));
          int num = DateTime.Now.Year;
          if (num.CompareTo(int32_1 + 18) <= 0)
          {
            num = DateTime.Now.Year;
            if (num.CompareTo(int32_1 + 18) == 0)
            {
              num = DateTime.Now.Month;
              if (num.CompareTo(int32_2) < 0)
                goto label_7;
            }
            else
              goto label_7;
          }
          client.Player.IsMinor = false;
        }
label_7:
        info.State = !(info.Name != "" & flag) ? 0 : 1;
      }
      if (flag)
      {
        client.Out.SendAASState(false);
        using (PlayerBussiness playerBussiness = new PlayerBussiness())
        {
          result = playerBussiness.AddAASInfo(info);
          client.Out.SendAASInfoSet(result);
        }
      }
      if (result && info.State == 1)
      {
        ItemTemplateInfo itemTemplate = ItemMgr.FindItemTemplate(11019);
        if (itemTemplate != null)
        {
          SqlDataProvider.Data.ItemInfo fromTemplate = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(itemTemplate, 1, 107);
          if (fromTemplate != null)
          {
            fromTemplate.IsBinds = true;
            AbstractInventory itemInventory = (AbstractInventory) client.Player.GetItemInventory(fromTemplate.Template);
            if (itemInventory.AddItem(fromTemplate, itemInventory.BeginSlot))
              client.Out.SendMessage(eMessageType.ChatNormal, LanguageMgr.GetTranslation("ASSInfoSetHandle.Success", (object) fromTemplate.Template.Name));
            else
              client.Out.SendMessage(eMessageType.ChatNormal, LanguageMgr.GetTranslation("ASSInfoSetHandle.NoPlace"));
          }
        }
      }
      return 0;
    }
  }
}
