using Bussiness;
using Game.Base.Packets;
using SqlDataProvider.Data;
using System;

namespace Game.Server.Packets.Client
{
    [PacketHandler(114, "修改邮件的已读未读标志")]
    public class UserUpdateMailHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(114, client.Player.PlayerCharacter.ID);
            int num = packet.ReadInt();
            using (PlayerBussiness playerBussiness = new PlayerBussiness())
            {
                MailInfo mailSingle = playerBussiness.GetMailSingle(client.Player.PlayerCharacter.ID, num);
                bool flag = mailSingle != null && !mailSingle.IsRead;
                if (flag)
                {
                    mailSingle.IsRead = (true);
                    bool flag2 = mailSingle.Type < 100;
                    if (flag2)
                    {
                        mailSingle.ValidDate = (72);
                        mailSingle.SendTime = (DateTime.Now);
                    }
                    playerBussiness.UpdateMail(mailSingle, mailSingle.Money);
                    gSPacketIn.WriteBoolean(true);
                }
                else
                {
                    gSPacketIn.WriteBoolean(false);
                }
            }
            client.Out.SendTCP(gSPacketIn);
            return 0;
        }
    }
}
