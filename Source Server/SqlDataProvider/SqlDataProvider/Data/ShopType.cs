﻿// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.ShopType
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

namespace SqlDataProvider.Data
{
  public enum ShopType
  {
    MONEY_M_HOTSALE = 1,
    MONEY_F_HOTSALE = 2,
    MONEY_M_RECOMMEND = 3,
    MONEY_F_RECOMMEND = 4,
    MONEY_M_CONCESSIONS = 5,
    MONEY_F_CONCESSIONS = 6,
    MONEY_M_WEAPON = 7,
    MONEY_F_WEAPON = 8,
    MONEY_M_CLOTH = 9,
    MONEY_F_CLOTH = 10, // 0x0000000A
    MONEY_M_HAT = 11, // 0x0000000B
    MONEY_F_HAT = 12, // 0x0000000C
    MONEY_M_GLASS = 13, // 0x0000000D
    MONEY_F_GLASS = 14, // 0x0000000E
    MONEY_M_SHIPIN = 15, // 0x0000000F
    MONEY_F_SHIPIN = 16, // 0x00000010
    MONEY_M_HAIR = 17, // 0x00000011
    MONEY_F_HAIR = 18, // 0x00000012
    MONEY_M_EYES = 19, // 0x00000013
    MONEY_F_EYES = 20, // 0x00000014
    MONEY_M_LIANSHI = 21, // 0x00000015
    MONEY_F_LIANSHI = 22, // 0x00000016
    MONEY_M_SUIT = 23, // 0x00000017
    MONEY_F_SUIT = 24, // 0x00000018
    MONEY_M_WING = 25, // 0x00000019
    MONEY_F_WING = 26, // 0x0000001A
    MONEY_M_FUNCTIONPROP = 27, // 0x0000001B
    MONEY_F_FUNCTIONPROP = 28, // 0x0000001C
    MONEY_M_PAOPAO = 29, // 0x0000001D
    MONEY_F_PAOPAO = 30, // 0x0000001E
    MONEY_HOT_GOODS = 33, // 0x00000021
    MONEY_FLOWER = 34, // 0x00000022
    MONEY_DESSERT = 35, // 0x00000023
    MONEY_TOYS = 36, // 0x00000024
    MONEY_RARE = 37, // 0x00000025
    MONEY_FESTIVAL = 38, // 0x00000026
    MONEY_WEDDING = 39, // 0x00000027
    DDTMONEY_M_HOTSALE = 41, // 0x00000029
    DDTMONEY_F_HOTSALE = 42, // 0x0000002A
    DDTMONEY_M_RECOMMEND = 43, // 0x0000002B
    DDTMONEY_F_RECOMMEND = 44, // 0x0000002C
    DDTMONEY_M_CONCESSIONS = 45, // 0x0000002D
    DDTMONEY_F_CONCESSIONS = 46, // 0x0000002E
    DDTMONEY_M_WEAPON = 47, // 0x0000002F
    DDTMONEY_F_WEAPON = 48, // 0x00000030
    DDTMONEY_M_CLOTH = 49, // 0x00000031
    DDTMONEY_F_CLOTH = 50, // 0x00000032
    DDTMONEY_M_HAT = 51, // 0x00000033
    DDTMONEY_F_HAT = 52, // 0x00000034
    DDTMONEY_M_GLASS = 53, // 0x00000035
    DDTMONEY_F_GLASS = 54, // 0x00000036
    DDTMONEY_M_SHIPIN = 55, // 0x00000037
    DDTMONEY_F_SHIPIN = 56, // 0x00000038
    DDTMONEY_M_HAIR = 57, // 0x00000039
    DDTMONEY_F_HAIR = 58, // 0x0000003A
    DDTMONEY_M_EYES = 59, // 0x0000003B
    DDTMONEY_F_EYES = 60, // 0x0000003C
    DDTMONEY_M_LIANSHI = 61, // 0x0000003D
    DDTMONEY_F_LIANSHI = 62, // 0x0000003E
    DDTMONEY_M_SUIT = 63, // 0x0000003F
    DDTMONEY_F_SUIT = 64, // 0x00000040
    DDTMONEY_M_WING = 65, // 0x00000041
    DDTMONEY_F_WING = 66, // 0x00000042
    DDTMONEY_M_FUNCTIONPROP = 67, // 0x00000043
    DDTMONEY_F_FUNCTIONPROP = 68, // 0x00000044
    DDTMONEY_M_PAOPAO = 69, // 0x00000045
    DDTMONEY_F_PAOPAO = 70, // 0x00000046
    DDTMONEY_HOT_GOODS = 73, // 0x00000049
    DDTMONEY_FLOWER = 74, // 0x0000004A
    DDTMONEY_DESSERT = 75, // 0x0000004B
    DDTMONEY_TOYS = 76, // 0x0000004C
    DDTMONEY_RARE = 77, // 0x0000004D
    DDTMONEY_FESTIVAL = 78, // 0x0000004E
    DDTMONEY_WEDDING = 79, // 0x0000004F
    GUILD_SHOP_1 = 80, // 0x00000050
    GUILD_SHOP_2 = 81, // 0x00000051
    GUILD_SHOP_3 = 82, // 0x00000052
    GUILD_SHOP_4 = 83, // 0x00000053
    GUILD_SHOP_5 = 84, // 0x00000054
    M_POPULARITY_RANKING = 85, // 0x00000055
    F_POPULARITY_RANKING = 86, // 0x00000056
    LITTLEGAME_AWARD_TYPE = 87, // 0x00000057
    FARM_SEED_TYPE = 88, // 0x00000058
    FARM_MANURE_TYPE = 89, // 0x00000059
    ROOM_PROP = 90, // 0x0000005A
    WORLDBOSS_AWARD_TYPE = 91, // 0x0000005B
    FARM_PETEGG_TYPE = 92, // 0x0000005C
    LEAGUE_SHOP_TYPE = 93, // 0x0000005D
    DRAGON_BOAT_TYPE = 95, // 0x0000005F
    KING_STATUE_SHOP = 100, // 0x00000064
    SALE_SHOP = 110, // 0x0000006E
    REGRESS_SHOP = 199, // 0x000000C7
    TRANSNATIONAL_AWARD_TYPE = 200, // 0x000000C8
    SHOP_PAST_PRICE = 280, // 0x00000118
  }
}
