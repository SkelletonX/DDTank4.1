using Bussiness;
using Bussiness.Managers;
using Game.Base;
using Game.Base.Packets;
using Game.Server;
using Game.Server.GameObjects;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;

namespace Game.Server.Packets.Client
{
	[PacketHandler(238, "Biriken Giriş")]
	public class AccumulAtiveLoginAwardHandler : IPacketHandler
	{
		public AccumulAtiveLoginAwardHandler()
		{


		}

		public int HandlePacket(GameClient client, GSPacketIn packet)
		{
			int num = packet.ReadInt();
			GSPacketIn gSPacketIn = new GSPacketIn(238, client.Player.PlayerCharacter.ID);
			Console.WriteLine(num);
			Console.WriteLine("AccumulAtiveLoginAwardHandler");
			return 0;
			
		}
	}
}