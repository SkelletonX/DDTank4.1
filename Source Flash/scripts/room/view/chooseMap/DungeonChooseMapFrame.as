package room.view.chooseMap
{
   import com.pickgliss.events.FrameEvent;
   import com.pickgliss.ui.ComponentFactory;
   import com.pickgliss.ui.LayerManager;
   import com.pickgliss.ui.controls.Frame;
   import com.pickgliss.ui.controls.TextButton;
   import ddt.data.map.DungeonInfo;
   import ddt.manager.GameInSocketOut;
   import ddt.manager.LanguageMgr;
   import ddt.manager.MapManager;
   import ddt.manager.SoundManager;
   import ddt.utils.PositionUtils;
   import flash.events.MouseEvent;
   import room.RoomManager;
   import room.model.RoomInfo;
   
   public class DungeonChooseMapFrame extends Frame
   {
       
      
      private var _view:DungeonChooseMapView;
      
      private var _okBtn:TextButton;
      
      public function DungeonChooseMapFrame()
      {
         super();
         escEnable = true;
         this._view = ComponentFactory.Instance.creatCustomObject("room.dungeonChooseMapView");
         addToContent(this._view);
         this._okBtn = ComponentFactory.Instance.creatComponentByStylename("asset.room.dungeonChooseMapButton");
         this._okBtn.text = LanguageMgr.GetTranslation("ok");
         addToContent(this._okBtn);
         PositionUtils.setPos(this._okBtn,"asset.DungeonRoom.OkbtnPos");
         this._okBtn.addEventListener(MouseEvent.CLICK,this.__okClick);
         titleText = LanguageMgr.GetTranslation("tank.room.RoomIIMapSetPanel.room");
         addEventListener(FrameEvent.RESPONSE,this.__responeHandler);
      }
      
      public function show() : void
      {
         LayerManager.Instance.addToLayer(this,LayerManager.GAME_DYNAMIC_LAYER,true,LayerManager.BLCAK_BLOCKGOUND);
      }
      
      private function __okClick(param1:MouseEvent) : void
      {
         var _loc2_:DungeonInfo = null;
         SoundManager.instance.play("008");
         if(this._view.checkState())
         {
            _loc2_ = MapManager.getDungeonInfo(this._view.selectedMapID);
            if(_loc2_.Type == MapManager.PVE_ACADEMY_MAP)
            {
               GameInSocketOut.sendGameRoomSetUp(this._view.selectedMapID,RoomInfo.ACADEMY_DUNGEON_ROOM,this._view._roomPass,this._view._roomName,1,this._view.selectedLevel);
            }
            else if(_loc2_.Type == MapManager.PVE_ACTIVITY_MAP)
            {
               GameInSocketOut.sendGameRoomSetUp(this._view.selectedMapID,RoomInfo.ACTIVITY_DUNGEON_ROOM,this._view._roomPass,this._view._roomName,1,this._view.selectedLevel);
            }
            else
            {
               GameInSocketOut.sendGameRoomSetUp(this._view.selectedMapID,RoomInfo.DUNGEON_ROOM,this._view._roomPass,this._view._roomName,1,this._view.selectedLevel);
            }
            RoomManager.Instance.current.roomName = this._view._roomName;
            RoomManager.Instance.current.roomPass = this._view._roomPass;
            RoomManager.Instance.current.dungeonType = this._view.selectedDungeonType;
            this.dispose();
         }
      }
      
      private function __responeHandler(param1:FrameEvent) : void
      {
         if(param1.responseCode == FrameEvent.CLOSE_CLICK || param1.responseCode == FrameEvent.ESC_CLICK)
         {
            SoundManager.instance.play("008");
            this.dispose();
         }
      }
      
      override public function dispose() : void
      {
         this._okBtn.removeEventListener(MouseEvent.CLICK,this.__okClick);
         removeEventListener(FrameEvent.RESPONSE,this.__responeHandler);
         this._view.dispose();
         this._view = null;
         super.dispose();
      }
   }
}
