using Game.Server.GameObjects;
using Game.Server.Rooms;
using log4net;
using Lsj.Util.JSON;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Game.Server.Managers
{
    public class RobotManager
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static int CurRobotID = -1000000;

        private static Dictionary<int, RobotGamePlayer> RobotGamePlayers = new Dictionary<int, RobotGamePlayer>();

        public static bool Start()
        {
            try
            {
                var data = "";
                if (File.Exists(@".\robot.json"))
                {
                    data = File.ReadAllText(@".\robot.json");
                }

                var robots = JSONParser.Parse<List<Robot>>(data);
                if (robots == null)
                {
                    robots = new List<Robot>
                    {
                        new Robot()
                    };
                }
                //UserVIPInfo userVIPInfo = new UserVIPInfo(CurRobotID);
                PlayerInfo playerinfo = new PlayerInfo();
                foreach (var robot in robots)
                {
                    try
                    {
                        var robotGamePlayer = new RobotGamePlayer(CurRobotID, new PlayerInfo
                        {
                            IsAutoBot = true,
                            ID = CurRobotID,
                            GP = robot.GP,
                            Grade = robot.Level,
                            NickName = robot.Name,
                            Texp = new TexpInfo(),
                            Sex = robot.Sex,
                            State = robot.State,
                        });
                        if (robot.Level >= 20)
                        {
                            playerinfo.apprenticeshipState = robot.apprenticeshipState;
                        }
                        else
                        {
                            playerinfo.masterID = robot.masterID;
                        }
                        playerinfo.VIPLevel = robot.VIPLevel;
                        playerinfo.VIPExpireDay = DateTime.Parse(robot.VIPExpireDay);
                        playerinfo.typeVIP = robot.typeVIP;

                        foreach (var equip in robot.Equips)
                        {
                            robotGamePlayer.Equip(equip.ID, equip.Strength, equip.Compose);
                        }


                        WorldMgr.AddPlayer(CurRobotID, robotGamePlayer);
                        if (robot.UserType == 1)//挂大厅
                        {
                            RoomMgr.WaitingRoom.AddPlayer(robotGamePlayer);
                        }

                        RobotGamePlayers.Add(CurRobotID--, robotGamePlayer);
                    }
                    catch (Exception e)
                    {
                        RobotManager.log.Error("Error when load robot, robot name: " + robot.Name, e);
                    }
                }

                File.WriteAllText(@".\robot.json", JSONFormater.Format(JSONConverter.ConvertToJSONString(robots)));
                data = "";
                if (File.Exists(@".\robotRoom.json"))
                {
                    data = File.ReadAllText(@".\robotRoom.json");
                }
                var robotRooms = JSONParser.Parse<List<RobotRoom>>(data);
                if (robotRooms == null)
                {
                    robotRooms = new List<RobotRoom>
                    {
                        new RobotRoom()
                    };
                }
                foreach (var robotRoom in robotRooms)
                {
                    RoomMgr.FakeRoom(robotRoom.RoomName, robotRoom.PlayerCount, robotRoom.MaxPlayerCount, robotRoom.RoomType);
                }

                File.WriteAllText(@".\robotRoom.json", JSONFormater.Format(JSONConverter.ConvertToJSONString(robotRooms)));
            }
            catch (Exception e)
            {
                RobotManager.log.Error("Robot Init", e);
                return false;

            }
            return true;
        }


        public class Robot
        {
            public string Name { get; set; } = "Robot";
            public int Level { get; set; } = 20;
            public int GP { get; set; } = 0;
            public bool Sex { get; set; }
            public int UserType { get; set; } = 1;
            public int VIPLevel { get; set; }
            public byte typeVIP { get; set; } = 2;
            public byte State { get; set; } = 1;
            public byte masterID { get; set; } = 0;
            public byte apprenticeshipState { get; set; } = 1;
            public string VIPExpireDay { get; set; } = new DateTime(2018, 1, 1).ToString();
            public List<Equip> Equips { get; set; } = new List<Equip> { new Equip() };

        }
        public class Equip
        {
            public int ID { get; set; } = 7001;
            public int Strength { get; set; } = 0;
            public int Compose { get; set; } = 0;
        }

        public class RobotRoom
        {
            public int PlayerCount { get; set; }
            public int MaxPlayerCount { get; set; }
            public string RoomName { get; set; } = "Robot Room Name";
            public int RoomType { get; set; }
        }
    }
}
