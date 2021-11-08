using Game.Base.Packets;
using Game.Server.GameObjects;

namespace Game.Server.Packets.Client
{
	[PacketHandler(17, "Client scene ready1")]
	public class UserSceneReadyHandler : IPacketHandler
	{
		public int HandlePacket(GameClient client, GSPacketIn packet)
		{
			if (client.Player.CurrentRoom != null)
			{
				GSPacketIn gSPacketIn = null;
				foreach (GamePlayer player in client.Player.CurrentRoom.GetPlayers())
				{
					if (player != client.Player)
					{
						if (gSPacketIn == null)
						{
							gSPacketIn = player.Out.SendSceneAddPlayer(client.Player);
						}
						else
						{
							player.Out.SendTCP(gSPacketIn);
						}
						client.Out.SendSceneRemovePlayer(player);
					}
				}
			}
			return 1;
		}

		public UserSceneReadyHandler()
		{
			
			
		}
	}
}
