// Decompiled with JetBrains decompiler
// Type: Bussiness.CenterService.ServerData
// Assembly: Bussiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C2537CFF-7BDB-4A06-BE9C-A8074B2C97E3
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Bussiness.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Bussiness.CenterService
{
  [DebuggerStepThrough]
  [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
  [DataContract(Name = "ServerData", Namespace = "http://schemas.datacontract.org/2004/07/Center.Server")]
  [Serializable]
  public class ServerData : IExtensibleDataObject, INotifyPropertyChanged
  {
    [NonSerialized]
    private ExtensionDataObject extensionDataField;
    [OptionalField]
    private int IdField;
    [OptionalField]
    private string IpField;
    [OptionalField]
    private int LowestLevelField;
    [OptionalField]
    private int MustLevelField;
    [OptionalField]
    private string NameField;
    [OptionalField]
    private int OnlineField;
    [OptionalField]
    private int PortField;
    [OptionalField]
    private int StateField;

    [Browsable(false)]
    public ExtensionDataObject ExtensionData
    {
      get
      {
        return this.extensionDataField;
      }
      set
      {
        this.extensionDataField = value;
      }
    }

    [DataMember]
    public int Id
    {
      get
      {
        return this.IdField;
      }
      set
      {
        if (this.IdField.Equals(value))
          return;
        this.IdField = value;
        this.RaisePropertyChanged(nameof (Id));
      }
    }

    [DataMember]
    public string Ip
    {
      get
      {
        return this.IpField;
      }
      set
      {
        if ((object) this.IpField == (object) value)
          return;
        this.IpField = value;
        this.RaisePropertyChanged(nameof (Ip));
      }
    }

    [DataMember]
    public int LowestLevel
    {
      get
      {
        return this.LowestLevelField;
      }
      set
      {
        if (this.LowestLevelField.Equals(value))
          return;
        this.LowestLevelField = value;
        this.RaisePropertyChanged(nameof (LowestLevel));
      }
    }

    [DataMember]
    public int MustLevel
    {
      get
      {
        return this.MustLevelField;
      }
      set
      {
        if (this.MustLevelField.Equals(value))
          return;
        this.MustLevelField = value;
        this.RaisePropertyChanged(nameof (MustLevel));
      }
    }

    [DataMember]
    public string Name
    {
      get
      {
        return this.NameField;
      }
      set
      {
        if ((object) this.NameField == (object) value)
          return;
        this.NameField = value;
        this.RaisePropertyChanged(nameof (Name));
      }
    }

    [DataMember]
    public int Online
    {
      get
      {
        return this.OnlineField;
      }
      set
      {
        if (this.OnlineField.Equals(value))
          return;
        this.OnlineField = value;
        this.RaisePropertyChanged(nameof (Online));
      }
    }

    [DataMember]
    public int Port
    {
      get
      {
        return this.PortField;
      }
      set
      {
        if (this.PortField.Equals(value))
          return;
        this.PortField = value;
        this.RaisePropertyChanged(nameof (Port));
      }
    }

    [DataMember]
    public int State
    {
      get
      {
        return this.StateField;
      }
      set
      {
        if (this.StateField.Equals(value))
          return;
        this.StateField = value;
        this.RaisePropertyChanged(nameof (State));
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void RaisePropertyChanged(string propertyName)
    {
      PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
      if (propertyChanged == null)
        return;
      propertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
