package ddt.manager
{
   import ddt.data.ServerConfigInfo;
   import ddt.data.analyze.ServerConfigAnalyz;
   import road7th.data.DictionaryData;
   
   public class ServerConfigManager
   {
      
      private static var _instance:ServerConfigManager;
      
      public static const MARRT_ROOM_CREATE_MONET:String = "MarryRoomCreateMoney";
      
      public static const MISSION_RICHES:String = "MissionRiches";
      
      public static const VIP_EXP_NEEDEDFOREACHLV:String = "VIPExpNeededForEachLv";
      
      public static const HOT_SPRING_EXP:String = "HotSpringExp";
      
      public static const FIRSTRECHARGE_RETURN:String = "FirstChargeReturn";
      
      public static const PLAYER_MIN_LEVEL:String = "PlayerMinLevel";
      
      public static const WARRIORFAMRAIDPRICEPERMIN:String = "WarriorFamRaidPricePerMin";
       
      
      private var _serverConfigInfoList:DictionaryData;
      
      public function ServerConfigManager()
      {
         super();
      }
      
      public static function get instance() : ServerConfigManager
      {
         if(_instance == null)
         {
            _instance = new ServerConfigManager();
         }
         return _instance;
      }
      
      public function getserverConfigInfo(param1:ServerConfigAnalyz) : void
      {
         this._serverConfigInfoList = param1.serverConfigInfoList;
      }
      
      public function get serverConfigInfo() : DictionaryData
      {
         return this._serverConfigInfoList;
      }
      
      public function get weddingMoney() : Array
      {
         return this.findInfoByName(ServerConfigManager.MARRT_ROOM_CREATE_MONET).Value.split(",");
      }
      
      public function get MissionRiches() : Array
      {
         return this.findInfoByName(ServerConfigManager.MISSION_RICHES).Value.split("|");
      }
      
      public function get VIPExpNeededForEachLv() : Array
      {
         return this.findInfoByName(ServerConfigManager.VIP_EXP_NEEDEDFOREACHLV).Value.split("|");
      }
      
      public function get HotSpringExp() : Array
      {
         return this.findInfoByName(ServerConfigManager.HOT_SPRING_EXP).Value.split(",");
      }
      
      public function findInfoByName(param1:String) : ServerConfigInfo
      {
         return this._serverConfigInfoList[param1];
      }
      
      public function getFirstRechargeRebateAndValue() : Array
      {
         var _loc1_:Object = this.findInfoByName(FIRSTRECHARGE_RETURN);
         if(_loc1_)
         {
            return this.findInfoByName(FIRSTRECHARGE_RETURN).Value.split("|");
         }
         return [1,998];
      }
      
      public function get minOpenPetSystemLevel() : int
      {
         var _loc1_:Object = this.findInfoByName(PLAYER_MIN_LEVEL);
         return int(_loc1_.Value);
      }
      
      public function get WarriorFamRaidPricePerMin() : Number
      {
         return Number(this.findInfoByName(WARRIORFAMRAIDPRICEPERMIN).Value);
      }
   }
}
