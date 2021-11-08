using Bussiness;
using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.Managers;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;

namespace Game.Server.Packets.Client
{
	[PacketHandler(74, "VisualizaItensdosplayes")]//EditByHeroDev
	public class UserEquipListHandler : IPacketHandler
	{
		public int HandlePacket(GameClient client, GSPacketIn packet)
		{
			int num = 0;
			string nickName = null;
			bool flag = packet.ReadBoolean();
			PlayerInfo playerInfo = null;
			List<ItemInfo> list = null;
			GamePlayer gamePlayer;
			if (!flag)
			{
				nickName = packet.ReadString();
				gamePlayer = WorldMgr.GetClientByPlayerNickName(nickName);
			}
			else
			{
				num = packet.ReadInt();
				gamePlayer = WorldMgr.GetPlayerById(num);
			}
			if (gamePlayer != null)
			{
				playerInfo = gamePlayer.PlayerCharacter;
				list = gamePlayer.EquipBag.GetItems(0, 31);
			}
			else
			{
				using (PlayerBussiness playerBussiness = new PlayerBussiness())
				{
					playerInfo = (flag ? playerBussiness.GetUserSingleByUserID(num) : playerBussiness.GetUserSingleByNickName(nickName));
					if (playerInfo != null)
					{
						playerInfo.Texp = playerBussiness.GetUserTexpInfoSingle(playerInfo.ID);
						list = playerBussiness.GetUserEuqip(playerInfo.ID);
					}
				}
			}
			if (playerInfo != null && list != null && playerInfo.Texp != null)
			{
				client.Out.SendUserEquip(playerInfo, list);
			}
			else
			{
				Console.WriteLine("Usuario Não Consta na Base de Dados.!");
			}
			return 0;
		}
	}
}
