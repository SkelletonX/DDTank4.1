package littleGame.model
{
   import ddt.data.player.PlayerInfo;
   import littleGame.events.LittleLivingEvent;
   
   public class LittlePlayer extends LittleLiving
   {
       
      
      private var _playerInfo:PlayerInfo;
      
      public function LittlePlayer(param1:PlayerInfo, param2:int, param3:int, param4:int, param5:int)
      {
         this._playerInfo = param1;
         super(param2,param3,param4,param5);
      }
      
      public function get playerInfo() : PlayerInfo
      {
         return this._playerInfo;
      }
      
      override public function get isPlayer() : Boolean
      {
         return true;
      }
      
      override public function toString() : String
      {
         return "LittlePlayer_" + this._playerInfo.NickName;
      }
      
      public function set headType(param1:int) : void
      {
         dispatchEvent(new LittleLivingEvent(LittleLivingEvent.HeadChanged,param1));
      }
   }
}
