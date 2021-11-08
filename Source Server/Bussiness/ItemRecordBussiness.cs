// Decompiled with JetBrains decompiler
// Type: Bussiness.ItemRecordBussiness
// Assembly: Bussiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C2537CFF-7BDB-4A06-BE9C-A8074B2C97E3
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Bussiness.dll

using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Bussiness
{
  public class ItemRecordBussiness : BaseBussiness
  {
    public void FusionItem(SqlDataProvider.Data.ItemInfo item, ref string Property)
    {
      if (item == null)
        return;
      Property = Property + string.Format("{0}:{1},{2}", (object) item.ItemID, (object) item.Template.Name, (object) Convert.ToInt32(item.IsBinds)) + "|";
    }

    public bool LogDropItemDb(DataTable dt)
    {
      bool flag = false;
      if (dt != null)
      {
        SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(ConfigurationManager.AppSettings["countDb"], SqlBulkCopyOptions.UseInternalTransaction);
        try
        {
          sqlBulkCopy.NotifyAfter = dt.Rows.Count;
          sqlBulkCopy.DestinationTableName = "Log_DropItem";
          sqlBulkCopy.ColumnMappings.Add(0, "ApplicationId");
          sqlBulkCopy.ColumnMappings.Add(1, "SubId");
          sqlBulkCopy.ColumnMappings.Add(2, "LineId");
          sqlBulkCopy.ColumnMappings.Add(3, "UserId");
          sqlBulkCopy.ColumnMappings.Add(4, "ItemId");
          sqlBulkCopy.ColumnMappings.Add(5, "TemplateID");
          sqlBulkCopy.ColumnMappings.Add(6, "DropId");
          sqlBulkCopy.ColumnMappings.Add(7, "DropData");
          sqlBulkCopy.ColumnMappings.Add(8, "EnterTime");
          sqlBulkCopy.WriteToServer(dt);
          flag = true;
        }
        catch (Exception ex)
        {
          if (BaseBussiness.log.IsErrorEnabled)
            BaseBussiness.log.Error((object) ("DropItem Log Error:" + ex.ToString()));
        }
        finally
        {
          sqlBulkCopy.Close();
          dt.Clear();
        }
      }
      return flag;
    }

    public bool LogFightDb(DataTable dt)
    {
      bool flag = false;
      if (dt != null)
      {
        SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(ConfigurationManager.AppSettings["countDb"], SqlBulkCopyOptions.UseInternalTransaction);
        try
        {
          sqlBulkCopy.NotifyAfter = dt.Rows.Count;
          sqlBulkCopy.DestinationTableName = "Log_Fight";
          sqlBulkCopy.ColumnMappings.Add(0, "ApplicationId");
          sqlBulkCopy.ColumnMappings.Add(1, "SubId");
          sqlBulkCopy.ColumnMappings.Add(2, "LineId");
          sqlBulkCopy.ColumnMappings.Add(3, "RoomId");
          sqlBulkCopy.ColumnMappings.Add(4, "RoomType");
          sqlBulkCopy.ColumnMappings.Add(5, "FightType");
          sqlBulkCopy.ColumnMappings.Add(6, "ChangeTeam");
          sqlBulkCopy.ColumnMappings.Add(7, "PlayBegin");
          sqlBulkCopy.ColumnMappings.Add(8, "PlayEnd");
          sqlBulkCopy.ColumnMappings.Add(9, "UserCount");
          sqlBulkCopy.ColumnMappings.Add(10, "MapId");
          sqlBulkCopy.ColumnMappings.Add(11, "TeamA");
          sqlBulkCopy.ColumnMappings.Add(12, "TeamB");
          sqlBulkCopy.ColumnMappings.Add(13, "PlayResult");
          sqlBulkCopy.ColumnMappings.Add(14, "WinTeam");
          sqlBulkCopy.ColumnMappings.Add(15, "Detail");
          sqlBulkCopy.WriteToServer(dt);
          flag = true;
        }
        catch (Exception ex)
        {
          if (BaseBussiness.log.IsErrorEnabled)
            BaseBussiness.log.Error((object) ("Fight Log Error:" + ex.ToString()));
        }
        finally
        {
          sqlBulkCopy.Close();
          dt.Clear();
        }
      }
      return flag;
    }

    public bool LogItemDb(DataTable dt)
    {
      bool flag = false;
      if (dt != null)
      {
        SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(ConfigurationManager.AppSettings["countDb"], SqlBulkCopyOptions.UseInternalTransaction);
        try
        {
          sqlBulkCopy.NotifyAfter = dt.Rows.Count;
          sqlBulkCopy.DestinationTableName = "Log_Item";
          sqlBulkCopy.ColumnMappings.Add(0, "ApplicationId");
          sqlBulkCopy.ColumnMappings.Add(1, "SubId");
          sqlBulkCopy.ColumnMappings.Add(2, "LineId");
          sqlBulkCopy.ColumnMappings.Add(3, "EnterTime");
          sqlBulkCopy.ColumnMappings.Add(4, "UserId");
          sqlBulkCopy.ColumnMappings.Add(5, "Operation");
          sqlBulkCopy.ColumnMappings.Add(6, "ItemName");
          sqlBulkCopy.ColumnMappings.Add(7, "ItemID");
          sqlBulkCopy.ColumnMappings.Add(8, "AddItem");
          sqlBulkCopy.ColumnMappings.Add(9, "BeginProperty");
          sqlBulkCopy.ColumnMappings.Add(10, "EndProperty");
          sqlBulkCopy.ColumnMappings.Add(11, "Result");
          sqlBulkCopy.WriteToServer(dt);
          flag = true;
          dt.Clear();
        }
        catch (Exception ex)
        {
          if (BaseBussiness.log.IsErrorEnabled)
            BaseBussiness.log.Error((object) ("Smith Log Error:" + ex.ToString()));
        }
        finally
        {
          sqlBulkCopy.Close();
        }
      }
      return flag;
    }

    public bool LogMoneyDb(DataTable dt)
    {
      bool flag = false;
      if (dt != null)
      {
        SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(ConfigurationManager.AppSettings["countDb"], SqlBulkCopyOptions.UseInternalTransaction);
        try
        {
          sqlBulkCopy.NotifyAfter = dt.Rows.Count;
          sqlBulkCopy.DestinationTableName = "Log_Money";
          sqlBulkCopy.ColumnMappings.Add(0, "ApplicationId");
          sqlBulkCopy.ColumnMappings.Add(1, "SubId");
          sqlBulkCopy.ColumnMappings.Add(2, "LineId");
          sqlBulkCopy.ColumnMappings.Add(3, "MastType");
          sqlBulkCopy.ColumnMappings.Add(4, "SonType");
          sqlBulkCopy.ColumnMappings.Add(5, "UserId");
          sqlBulkCopy.ColumnMappings.Add(6, "EnterTime");
          sqlBulkCopy.ColumnMappings.Add(7, "Moneys");
          sqlBulkCopy.ColumnMappings.Add(8, "Gold");
          sqlBulkCopy.ColumnMappings.Add(9, "GiftToken");
          sqlBulkCopy.ColumnMappings.Add(10, "Offer");
          sqlBulkCopy.ColumnMappings.Add(11, "OtherPay");
          sqlBulkCopy.ColumnMappings.Add(12, "GoodId");
          sqlBulkCopy.ColumnMappings.Add(13, "ShopId");
          sqlBulkCopy.ColumnMappings.Add(14, "Datas");
          sqlBulkCopy.WriteToServer(dt);
          flag = true;
        }
        catch
        {
        }
        finally
        {
          sqlBulkCopy.Close();
          dt.Clear();
        }
      }
      return flag;
    }

    public bool LogServerDb(DataTable dt)
    {
      bool flag = false;
      if (dt != null)
      {
        SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(ConfigurationManager.AppSettings["countDb"], SqlBulkCopyOptions.UseInternalTransaction);
        try
        {
          sqlBulkCopy.NotifyAfter = dt.Rows.Count;
          sqlBulkCopy.DestinationTableName = "Log_Server";
          sqlBulkCopy.ColumnMappings.Add(0, "ApplicationId");
          sqlBulkCopy.ColumnMappings.Add(1, "SubId");
          sqlBulkCopy.ColumnMappings.Add(2, "EnterTime");
          sqlBulkCopy.ColumnMappings.Add(3, "Online");
          sqlBulkCopy.ColumnMappings.Add(4, "Reg");
          sqlBulkCopy.WriteToServer(dt);
          flag = true;
        }
        catch (Exception ex)
        {
          if (BaseBussiness.log.IsErrorEnabled)
            BaseBussiness.log.Error((object) ("Server Log Error:" + ex.ToString()));
        }
        finally
        {
          sqlBulkCopy.Close();
          dt.Clear();
        }
      }
      return flag;
    }

    public void PropertyString(SqlDataProvider.Data.ItemInfo item, ref string Property)
    {
      if (item == null)
        return;
      Property = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", (object) item.StrengthenLevel, (object) item.Attack, (object) item.Defence, (object) item.Agility, (object) item.Luck, (object) item.AttackCompose, (object) item.DefendCompose, (object) item.AgilityCompose, (object) item.LuckCompose);
    }
  }
}
