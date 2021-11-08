package room.view.bigMapInfoPanel
{
   import com.pickgliss.ui.ComponentFactory;
   import com.pickgliss.ui.controls.SimpleBitmapButton;
   import ddt.events.RoomEvent;
   import ddt.manager.PathManager;
   import ddt.manager.SoundManager;
   import flash.events.MouseEvent;
   import room.RoomManager;
   import room.view.chooseMap.DungeonChooseMapFrame;
   
   public class DungeonBigMapInfoPanel extends MissionRoomBigMapInfoPanel
   {
       
      
      private var _chooseBtn:SimpleBitmapButton;
      
      public function DungeonBigMapInfoPanel()
      {
         super();
      }
      
      override protected function initEvents() : void
      {
         super.initEvents();
         this._chooseBtn.addEventListener(MouseEvent.MOUSE_OVER,this.__overHandler);
         this._chooseBtn.addEventListener(MouseEvent.MOUSE_OUT,this.__outHandler);
         this._chooseBtn.addEventListener(MouseEvent.CLICK,this.__clickHandler);
         _info.addEventListener(RoomEvent.STARTED_CHANGED,this.__onGameStarted);
         _info.addEventListener(RoomEvent.PLAYER_STATE_CHANGED,this.__playerStateChange);
      }
      
      private function __clickHandler(param1:MouseEvent) : void
      {
         SoundManager.instance.play("008");
         var _loc2_:DungeonChooseMapFrame = ComponentFactory.Instance.creatCustomObject("asset.room.dungeonChooseMapFrame");
         _loc2_.show();
         dispatchEvent(new RoomEvent(RoomEvent.OPEN_DUNGEON_CHOOSER));
      }
      
      private function __outHandler(param1:MouseEvent) : void
      {
         if(_info.mapId != 0 && _info.mapId != 10000)
         {
            this._chooseBtn.alpha = 0;
         }
         else
         {
            this._chooseBtn.alpha = 1;
         }
      }
      
      private function __overHandler(param1:MouseEvent) : void
      {
         this._chooseBtn.alpha = 1;
      }
      
      override protected function removeEvents() : void
      {
         super.removeEvents();
         this._chooseBtn.removeEventListener(MouseEvent.MOUSE_OVER,this.__overHandler);
         this._chooseBtn.removeEventListener(MouseEvent.MOUSE_OUT,this.__outHandler);
         this._chooseBtn.removeEventListener(MouseEvent.CLICK,this.__clickHandler);
         _info.removeEventListener(RoomEvent.STARTED_CHANGED,this.__onGameStarted);
         _info.removeEventListener(RoomEvent.PLAYER_STATE_CHANGED,this.__playerStateChange);
      }
      
      override protected function initView() : void
      {
         _bg = ComponentFactory.Instance.creatBitmap("asset.room.view.bigMapInfoPanel.mathRoomBigMapInfoPanelBgAsset");
         addChild(_bg);
         _mapShowContainer = ComponentFactory.Instance.creatCustomObject("asset.room.bigMapIconContainer");
         addChild(_mapShowContainer);
         _pos1 = ComponentFactory.Instance.creatCustomObject("room.dropListPos1");
         _pos2 = ComponentFactory.Instance.creatCustomObject("room.dropListPos2");
         this._chooseBtn = ComponentFactory.Instance.creatComponentByStylename("asset.room.selectDungeonButton");
         this._chooseBtn.transparentEnable = true;
         this._chooseBtn.displacement = false;
         this._chooseBtn.visible = false;
         addChild(this._chooseBtn);
         _dropList = new DropList();
         _dropList.x = _pos1.x;
         _dropList.y = _pos1.y;
         addChild(_dropList);
         _dropList.visible = true;
         _info = RoomManager.Instance.current;
         if(_info)
         {
            _info.addEventListener(RoomEvent.MAP_CHANGED,this.__onMapChanged);
            _info.addEventListener(RoomEvent.HARD_LEVEL_CHANGED,__updateHard);
            updateMap();
            updateDropList();
            if(_info.selfRoomPlayer)
            {
               this._chooseBtn.visible = _info.selfRoomPlayer.isHost;
            }
         }
      }
      
      private function __onGameStarted(param1:RoomEvent) : void
      {
         this._chooseBtn.enable = !_info.started;
      }
      
      override protected function __onMapChanged(param1:RoomEvent) : void
      {
         super.__onMapChanged(param1);
         if(_info.mapId != 0 && _info.mapId != 10000)
         {
            this._chooseBtn.alpha = 0;
         }
         else
         {
            this._chooseBtn.alpha = 1;
         }
      }
      
      private function __playerStateChange(param1:RoomEvent) : void
      {
         this._chooseBtn.visible = _info.selfRoomPlayer.isHost;
      }
      
      override protected function solvePath() : String
      {
         var _loc1_:String = PathManager.SITE_MAIN + "image/map/";
         if(_info && _info.mapId > 0)
         {
            _loc1_ = _loc1_ + (_info.mapId + "/show1.jpg");
         }
         else
         {
            _loc1_ = _loc1_ + "10000/show1.jpg";
         }
         return _loc1_;
      }
      
      override public function dispose() : void
      {
         super.dispose();
         this._chooseBtn.dispose();
         this._chooseBtn = null;
      }
   }
}
