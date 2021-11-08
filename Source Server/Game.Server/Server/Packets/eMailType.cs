// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.eMailType
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

namespace Game.Server.Packets
{
  public enum eMailType
  {
    Default = 0,
    Common = 1,
    AuctionSuccess = 2,
    AuctionFail = 3,
    BidSuccess = 4,
    BidFail = 5,
    ReturnPayment = 6,
    PaymentCancel = 7,
    BuyItem = 8,
    ItemOverdue = 9,
    PresentItem = 10, // 0x0000000A
    PaymentFinish = 11, // 0x0000000B
    OpenUpArk = 12, // 0x0000000C
    StoreCanel = 13, // 0x0000000D
    Marry = 14, // 0x0000000E
    DailyAward = 15, // 0x0000000F
    Manage = 51, // 0x00000033
    Active = 52, // 0x00000034
    GiftGuide = 55, // 0x00000037
    AdvertMail = 58, // 0x0000003A
    ConsortionEmail = 59, // 0x0000003B
    FriendBrithday = 60, // 0x0000003C
    MyseftBrithday = 61, // 0x0000003D
    ConsortionSkillMail = 62, // 0x0000003E
    Payment = 101, // 0x00000065
  }
}
