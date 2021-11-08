
using System.Threading;

namespace Game.Server.RingStation
{
    public class RingStationConfiguration
    {
        public static int PlayerID = 3000;
        public static string[] RandomName = new string[19]
        {
      "rodriguin1991",
      "Alguemai",
      "Jorgeuchiha",
      "MiaKalista",
      "shockwave128",
      "Chamanozap",
      "khanacademy",
      "wallysson",
      "daniel1999",
      "robson171",
      "Luizãodd",
      "OmaTokita",
      "RafaelCalabouço",
      "MartinsDan",
      "AbSilvio",
      "SoulSister",
      "PitBull",
      "FaixaPreta",
      "Alehandro"
        };
        public static int roomID = 3000;
        public static int ServerID = 104;
        public static string ServerName = "AutoBot";

        public static int NextPlayerID()
        {
            return Interlocked.Increment(ref RingStationConfiguration.PlayerID);
        }

        public static int NextRoomId()
        {
            return Interlocked.Increment(ref RingStationConfiguration.roomID);
        }
    }
}
