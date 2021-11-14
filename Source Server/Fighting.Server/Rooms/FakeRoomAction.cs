using Fighting.Server.Rooms;
using System;
namespace Game.Server.Rooms
{
    public class FakeRoomAction : IAction
    {
        private string roomName;
        private int playerCount;
        private int maxPlayerCount;
        private int roomType;

        private Random rand;

        public FakeRoomAction(string roomName, int playerCount, int maxPlayerCount, int roomType)
        {
            this.roomName = roomName;
            this.playerCount = playerCount;
            this.maxPlayerCount = maxPlayerCount;
            this.roomType = roomType;
            this.rand = new Random();
        }

        public void Execute()
        {
            throw new NotImplementedException();
        }
        /*
public void Execute()
{

 BaseRoom[] rooms = RoomMgr.Rooms;
   FakeRoom room = null;

   int roomId = rand.Next(rooms.Length);

   for (int i = 0; i < rooms.Length; i++)
   {
       if (!rooms[roomId].IsUsing)
       {
           room = new FakeRoom(rooms[roomId].RoomId, roomName, playerCount, maxPlayerCount, roomType);
           rooms[roomId] = room;
           break;
       }

       roomId = rand.Next(rooms.Length);
   }
   if (room != null)
   {
       room.Start();
       RoomMgr.WaitingRoom.SendUpdateRoom(room);
   } */
    }
}