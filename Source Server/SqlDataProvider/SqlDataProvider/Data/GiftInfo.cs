// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.GiftInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

using System;
using System.Collections.Generic;

namespace SqlDataProvider.Data
{
  public class GiftInfo : DataObject
  {
    private Dictionary<string, object> _tempInfo = new Dictionary<string, object>();
    private DateTime _addDate;
    private int _count;
    private int _itemID;
    private ItemTemplateInfo _template;
    private int _templateId;
    private int _userID;

    internal GiftInfo(ItemTemplateInfo template)
    {
      this._template = template;
      if (this._template != null)
        this._templateId = this._template.TemplateID;
      if (this._tempInfo != null)
        return;
      this._tempInfo = new Dictionary<string, object>();
    }

    public bool CanStackedTo(GiftInfo to)
    {
      if (this._templateId == to.TemplateID)
        return this.Template.MaxCount > 1;
      return false;
    }

    public static GiftInfo CreateFromTemplate(ItemTemplateInfo template, int count)
    {
      if (template == null)
        throw new ArgumentNullException(nameof (template));
      GiftInfo giftInfo = new GiftInfo(template);
      giftInfo.TemplateID = template.TemplateID;
      giftInfo.IsDirty = false;
      giftInfo.AddDate = DateTime.Now;
      giftInfo.Count = count;
      return giftInfo;
    }

    public static GiftInfo CreateWithoutInit(ItemTemplateInfo template)
    {
      return new GiftInfo(template);
    }

    public DateTime AddDate
    {
      get
      {
        return this._addDate;
      }
      set
      {
        if (this._addDate == value)
          return;
        this._addDate = value;
        this._isDirty = true;
      }
    }

    public int Count
    {
      get
      {
        return this._count;
      }
      set
      {
        if (this._count == value)
          return;
        this._count = value;
        this._isDirty = true;
      }
    }

    public int ItemID
    {
      get
      {
        return this._itemID;
      }
      set
      {
        this._itemID = value;
        this._isDirty = true;
      }
    }

    public Dictionary<string, object> TempInfo
    {
      get
      {
        return this._tempInfo;
      }
    }

    public ItemTemplateInfo Template
    {
      get
      {
        return this._template;
      }
    }

    public int TemplateID
    {
      get
      {
        return this._templateId;
      }
      set
      {
        if (this._templateId == value)
          return;
        this._templateId = value;
        this._isDirty = true;
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
        if (this._userID == value)
          return;
        this._userID = value;
        this._isDirty = true;
      }
    }
  }
}
