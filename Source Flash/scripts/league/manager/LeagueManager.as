package league.manager
{
   import com.pickgliss.ui.ComponentFactory;
   import ddt.bagStore.BagStore;
   import ddt.events.CrazyTankSocketEvent;
   import ddt.manager.SocketManager;
   import ddt.manager.StateManager;
   import ddt.states.StateType;
   import flash.events.Event;
   import flash.events.EventDispatcher;
   import flash.events.IEventDispatcher;
   import league.view.LeagueStartNoticeView;
   
   public class LeagueManager extends EventDispatcher
   {
      
      private static var _instance:LeagueManager;
       
      
      private var _lsnView:LeagueStartNoticeView;
      
      public function LeagueManager(param1:IEventDispatcher = null)
      {
         super(param1);
      }
      
      public static function get instance() : LeagueManager
      {
         if(_instance == null)
         {
            _instance = new LeagueManager();
         }
         return _instance;
      }
      
      public function initLeagueStartNoticeEvent() : void
      {
         SocketManager.Instance.addEventListener(CrazyTankSocketEvent.POPUP_LEAGUESTART_NOTICE,this.onLeagueNotice);
      }
      
      private function onLeagueNotice(param1:Event) : void
      {
         if(StateManager.currentStateType == StateType.MAIN || StateManager.currentStateType == StateType.ROOM_LIST || StateManager.currentStateType == StateType.DUNGEON_LIST || StateManager.currentStateType == StateType.DUNGEON_ROOM || StateManager.currentStateType == StateType.MATCH_ROOM || StateManager.currentStateType == StateType.CHALLENGE_ROOM || StateManager.currentStateType == StateType.LOGIN)
         {
            if(BagStore.instance.storeOpenAble)
            {
               return;
            }
            this._lsnView = ComponentFactory.Instance.creat("league.leagueStartNoticeView");
            this._lsnView.show();
         }
      }
   }
}
