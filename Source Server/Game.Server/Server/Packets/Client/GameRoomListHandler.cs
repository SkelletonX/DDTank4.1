using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Base.Packets;
using Game.Server.Managers;
using Game.Server.Rooms;

namespace Game.Server.Packets.Client
{
    [PacketHandler((int)ePackageType.GAME_ROOM_LIST,"房间列表")]
    //[Obsolete("是否已经没有使用")]
    public class GameRoomListHandler:IPacketHandler 
    {
        public const int numberRoomInPage = 100;
   
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            //BaseSceneGame[] list = GameMgr.GetAllGame();
            //foreach (BaseSceneGame g in list)
            //{
            //    if (g.Count > 0 && g.GameState != eGameState.FREE)
            //    {
            //        //client.Out.SendRoomInfo(g.Player, g);
            //        client.Out.SendTCP(client.Out.SendRoomInfo(g.Player,g));
            //    }
            //}
            //client.
            int hallType=packet.ReadInt();
            int updateType=packet.ReadInt();
            int page=packet.ReadInt();
            if(page<0)page=0;
            page++;
            BaseRoom[] list = RoomMgr.Rooms;
            List<BaseRoom> tempList = new List<BaseRoom>();
            //int maxRoomInList = 7;
            var count = 0;
            for (int i = 0; i < list.Length; i++)
            {
                if (!list[i].IsEmpty )
                {
                        // if(!list[i].IsPlaying&&cou) countPr
                    count++;
                    if (count < page * numberRoomInPage&&count>(page-1)*numberRoomInPage)
                        tempList.Add(list[i]);
                    else if(count > page * numberRoomInPage) { 
                        break; 
                    }
                    //m_player.Out.SendUpdateRoomList(list[i]);
                }
            }
            if(tempList.Count>0)  client.Out.SendUpdateRoomList(tempList);
            return 0;
        }
    }
}
