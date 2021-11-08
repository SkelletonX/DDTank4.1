// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.PassWordTwoHandle
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;

namespace Game.Server.Packets.Client
{
  [PacketHandler(25, "二级密码")]
  public class PassWordTwoHandle : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      string translateId = "二级密码";
      bool val1 = false;
      int val2 = 0;
      bool val3 = false;
      int Count = 0;
      string passwordTwo1 = packet.ReadString();
      string passwordTwo2 = packet.ReadString();
      int num = packet.ReadInt();
      string PasswordQuestion1 = packet.ReadString();
      string PasswordAnswer1_1 = packet.ReadString();
      string PasswordQuestion2 = packet.ReadString();
      string PasswordAnswer2_1 = packet.ReadString();
      switch (num)
      {
        case 1:
          val2 = 1;
          if (string.IsNullOrEmpty(client.Player.PlayerCharacter.PasswordTwo))
          {
            using (PlayerBussiness playerBussiness = new PlayerBussiness())
            {
              if (passwordTwo1 != "" && playerBussiness.UpdatePasswordTwo(client.Player.PlayerCharacter.ID, passwordTwo1))
              {
                client.Player.PlayerCharacter.PasswordTwo = passwordTwo1;
                client.Player.PlayerCharacter.IsLocked = false;
                translateId = "SetPassword.success";
              }
              if (PasswordQuestion1 != "" && PasswordAnswer1_1 != "" && (PasswordQuestion2 != "" && PasswordAnswer2_1 != ""))
              {
                if (playerBussiness.UpdatePasswordInfo(client.Player.PlayerCharacter.ID, PasswordQuestion1, PasswordAnswer1_1, PasswordQuestion2, PasswordAnswer2_1, 5))
                {
                  client.Player.PlayerCharacter.PasswordQuest1 = PasswordQuestion1;
                  client.Player.PlayerCharacter.PasswordQuest2 = PasswordAnswer1_1;
                  client.Player.PlayerCharacter.FailedPasswordAttemptCount = 5;
                  val1 = true;
                  val3 = false;
                  translateId = "UpdatePasswordInfo.Success";
                  break;
                }
                val1 = false;
                break;
              }
              val1 = true;
              val3 = true;
              break;
            }
          }
          else
          {
            translateId = "SetPassword.Fail";
            val1 = false;
            val3 = false;
            break;
          }
        case 2:
          val2 = 2;
          if (!(passwordTwo1 == client.Player.PlayerCharacter.PasswordTwo))
          {
            translateId = "PasswordTwo.error";
            val1 = false;
            val3 = false;
            break;
          }
          client.Player.PlayerCharacter.IsLocked = false;
          translateId = "BagUnlock.success";
          val1 = true;
          break;
        case 3:
          val2 = 3;
          using (PlayerBussiness playerBussiness = new PlayerBussiness())
          {
            playerBussiness.GetPasswordInfo(client.Player.PlayerCharacter.ID, ref PasswordQuestion1, ref PasswordAnswer1_1, ref PasswordQuestion2, ref PasswordAnswer2_1, ref Count);
            --Count;
            playerBussiness.UpdatePasswordInfo(client.Player.PlayerCharacter.ID, PasswordQuestion1, PasswordAnswer1_1, PasswordQuestion2, PasswordAnswer2_1, Count);
            if (passwordTwo1 == client.Player.PlayerCharacter.PasswordTwo)
            {
              if (playerBussiness.UpdatePasswordTwo(client.Player.PlayerCharacter.ID, passwordTwo2))
              {
                client.Player.PlayerCharacter.IsLocked = false;
                client.Player.PlayerCharacter.PasswordTwo = passwordTwo2;
                translateId = "UpdatePasswordTwo.Success";
                val1 = true;
                val3 = false;
                break;
              }
              translateId = "UpdatePasswordTwo.Fail";
              val1 = false;
              val3 = false;
              break;
            }
            translateId = "PasswordTwo.error";
            val1 = false;
            val3 = false;
            break;
          }
        case 4:
          val2 = 4;
          string PasswordAnswer1_2 = "";
          string passwordTwo3 = "";
          string PasswordAnswer2_2 = "";
          using (PlayerBussiness playerBussiness = new PlayerBussiness())
          {
            playerBussiness.GetPasswordInfo(client.Player.PlayerCharacter.ID, ref PasswordQuestion1, ref PasswordAnswer1_2, ref PasswordQuestion2, ref PasswordAnswer2_2, ref Count);
            --Count;
            playerBussiness.UpdatePasswordInfo(client.Player.PlayerCharacter.ID, PasswordQuestion1, PasswordAnswer1_1, PasswordQuestion2, PasswordAnswer2_1, Count);
            if (PasswordAnswer1_2 == PasswordAnswer1_1 && PasswordAnswer2_2 == PasswordAnswer2_1 && (PasswordAnswer1_2 != "" && PasswordAnswer2_2 != ""))
            {
              if (playerBussiness.UpdatePasswordTwo(client.Player.PlayerCharacter.ID, passwordTwo3))
              {
                client.Player.PlayerCharacter.PasswordTwo = passwordTwo3;
                client.Player.PlayerCharacter.IsLocked = false;
                translateId = "DeletePassword.success";
                val1 = true;
                val3 = false;
                break;
              }
              translateId = "DeletePassword.Fail";
              val1 = false;
              break;
            }
            if (passwordTwo1 == client.Player.PlayerCharacter.PasswordTwo)
            {
              if (playerBussiness.UpdatePasswordTwo(client.Player.PlayerCharacter.ID, passwordTwo3))
              {
                client.Player.PlayerCharacter.PasswordTwo = passwordTwo3;
                client.Player.PlayerCharacter.IsLocked = false;
                translateId = "DeletePassword.success";
                val1 = true;
                val3 = false;
                break;
              }
              break;
            }
            translateId = "DeletePassword.Fail";
            val1 = false;
            break;
          }
        case 5:
          val2 = 5;
          if (client.Player.PlayerCharacter.PasswordTwo != null && PasswordQuestion1 != "" && (PasswordAnswer1_1 != "" && PasswordQuestion2 != "") && PasswordAnswer2_1 != "")
          {
            using (PlayerBussiness playerBussiness = new PlayerBussiness())
            {
              if (playerBussiness.UpdatePasswordInfo(client.Player.PlayerCharacter.ID, PasswordQuestion1, PasswordAnswer1_1, PasswordQuestion2, PasswordAnswer2_1, 5))
              {
                val1 = true;
                val3 = false;
                translateId = "UpdatePasswordInfo.Success";
                break;
              }
              val1 = false;
              break;
            }
          }
          else
            break;
      }
      GSPacketIn packet1 = new GSPacketIn((short) 25, client.Player.PlayerCharacter.ID);
      packet1.WriteInt(client.Player.PlayerCharacter.ID);
      packet1.WriteInt(val2);
      packet1.WriteBoolean(val1);
      packet1.WriteBoolean(val3);
      packet1.WriteString(LanguageMgr.GetTranslation(translateId));
      packet1.WriteInt(Count);
      packet1.WriteString(PasswordQuestion1);
      packet1.WriteString(PasswordQuestion2);
      client.Out.SendTCP(packet1);
      return 0;
    }
  }
}
