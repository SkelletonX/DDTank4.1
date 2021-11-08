package hotSpring.controller
{
   import ddt.data.HotSpringRoomInfo;
   import ddt.events.CrazyTankSocketEvent;
   import ddt.manager.SocketManager;
   import ddt.states.BaseStateView;
   import ddt.states.StateType;
   import ddt.view.MainToolBar;
   import hotSpring.model.HotSpringRoomListModel;
   import hotSpring.view.HotSpringMainView;
   import road7th.comm.PackageIn;
   
   public class HotSpringRoomListController extends BaseStateView
   {
       
      
      private var _view:HotSpringMainView;
      
      private var _model:HotSpringRoomListModel;
      
      public function HotSpringRoomListController()
      {
         super();
      }
      
      override public function prepare() : void
      {
         super.prepare();
      }
      
      override public function enter(param1:BaseStateView, param2:Object = null) : void
      {
         super.enter(param1,param2);
         this._model = HotSpringRoomListModel.Instance;
         if(this._view)
         {
            this._view.hide();
            this._view.dispose();
         }
         this._view = null;
         this._view = new HotSpringMainView(this,this._model);
         this.setEvent();
         this._view.show();
         MainToolBar.Instance.show();
      }
      
      private function setEvent() : void
      {
         SocketManager.Instance.addEventListener(CrazyTankSocketEvent.HOTSPRING_ROOM_ADD_OR_UPDATE,this.roomAddOrUpdate);
         SocketManager.Instance.addEventListener(CrazyTankSocketEvent.HOTSPRING_ROOM_CREATE,this.roomCreateSucceed);
         SocketManager.Instance.addEventListener(CrazyTankSocketEvent.HOTSPRING_ROOM_LIST_GET,this.roomListGet);
         SocketManager.Instance.addEventListener(CrazyTankSocketEvent.HOTSPRING_ROOM_REMOVE,this.roomRemove);
      }
      
      private function removeEvent() : void
      {
         SocketManager.Instance.removeEventListener(CrazyTankSocketEvent.HOTSPRING_ROOM_ADD_OR_UPDATE,this.roomAddOrUpdate);
         SocketManager.Instance.removeEventListener(CrazyTankSocketEvent.HOTSPRING_ROOM_CREATE,this.roomCreateSucceed);
         SocketManager.Instance.removeEventListener(CrazyTankSocketEvent.HOTSPRING_ROOM_LIST_GET,this.roomListGet);
         SocketManager.Instance.removeEventListener(CrazyTankSocketEvent.HOTSPRING_ROOM_REMOVE,this.roomRemove);
      }
      
      private function roomAddOrUpdate(param1:CrazyTankSocketEvent) : void
      {
         var _loc2_:PackageIn = param1.pkg;
         var _loc3_:HotSpringRoomInfo = new HotSpringRoomInfo();
         _loc3_.roomNumber = _loc2_.readInt();
         _loc3_.roomID = _loc2_.readInt();
         _loc3_.roomName = _loc2_.readUTF();
         _loc3_.roomPassword = _loc2_.readUTF();
         _loc3_.effectiveTime = _loc2_.readInt();
         _loc3_.curCount = _loc2_.readInt();
         _loc3_.playerID = _loc2_.readInt();
         _loc3_.playerName = _loc2_.readUTF();
         _loc3_.startTime = _loc2_.readDate();
         _loc3_.roomIntroduction = _loc2_.readUTF();
         _loc3_.roomType = _loc2_.readInt();
         _loc3_.maxCount = _loc2_.readInt();
         _loc3_.roomIsPassword = _loc3_.roomPassword != "" && _loc3_.roomPassword.length > 0;
         this._model.roomAddOrUpdate(_loc3_);
      }
      
      public function hotSpringEnter() : void
      {
         SocketManager.Instance.out.sendHotSpringEnter();
      }
      
      private function roomCreateSucceed(param1:CrazyTankSocketEvent) : void
      {
         var _loc2_:PackageIn = param1.pkg;
         var _loc3_:Boolean = _loc2_.readBoolean();
         if(!_loc3_)
         {
            return;
         }
         var _loc4_:HotSpringRoomInfo = new HotSpringRoomInfo();
         _loc4_.roomID = _loc2_.readInt();
         _loc4_.roomName = _loc2_.readUTF();
         _loc4_.roomPassword = _loc2_.readUTF();
         _loc4_.effectiveTime = _loc2_.readInt();
         _loc4_.curCount = _loc2_.readInt();
         _loc4_.playerID = _loc2_.readInt();
         _loc4_.playerName = _loc2_.readUTF();
         _loc4_.startTime = _loc2_.readDate();
         _loc4_.roomIntroduction = _loc2_.readUTF();
         _loc4_.roomType = _loc2_.readInt();
         _loc4_.maxCount = _loc2_.readInt();
         if(_loc4_.roomPassword && _loc4_.roomPassword != "" && _loc4_.roomPassword.length > 0)
         {
            _loc4_.roomIsPassword = true;
         }
         else
         {
            _loc4_.roomIsPassword = false;
         }
         this._model.roomSelf = _loc4_;
      }
      
      private function roomListGet(param1:CrazyTankSocketEvent) : void
      {
         var _loc2_:HotSpringRoomInfo = null;
         var _loc3_:PackageIn = param1.pkg;
         var _loc4_:int = _loc3_.readInt();
         var _loc5_:int = 0;
         while(_loc5_ < _loc4_)
         {
            _loc2_ = new HotSpringRoomInfo();
            _loc2_.roomNumber = _loc3_.readInt();
            _loc2_.roomID = _loc3_.readInt();
            _loc2_.roomName = _loc3_.readUTF();
            _loc2_.roomPassword = _loc3_.readUTF();
            _loc2_.effectiveTime = _loc3_.readInt();
            _loc2_.curCount = _loc3_.readInt();
            _loc2_.playerID = _loc3_.readInt();
            _loc2_.playerName = _loc3_.readUTF();
            _loc2_.startTime = _loc3_.readDate();
            _loc2_.roomIntroduction = _loc3_.readUTF();
            _loc2_.roomType = _loc3_.readInt();
            _loc2_.maxCount = _loc3_.readInt();
            this._model.roomAddOrUpdate(_loc2_);
            _loc5_++;
         }
      }
      
      private function roomRemove(param1:CrazyTankSocketEvent) : void
      {
         var _loc2_:PackageIn = param1.pkg;
         var _loc3_:int = _loc2_.readInt();
         this._model.roomRemove(_loc3_);
      }
      
      public function roomEnterConfirm(param1:int) : void
      {
         SocketManager.Instance.out.sendHotSpringRoomEnterConfirm(param1);
      }
      
      public function roomEnter(param1:int, param2:String) : void
      {
         SocketManager.Instance.out.sendHotSpringRoomEnter(param1,param2);
      }
      
      public function quickEnterRoom() : void
      {
         SocketManager.Instance.out.sendHotSpringRoomQuickEnter();
      }
      
      public function roomCreate(param1:HotSpringRoomInfo) : void
      {
         SocketManager.Instance.out.sendHotSpringRoomCreate(param1);
      }
      
      override public function leaving(param1:BaseStateView) : void
      {
         super.leaving(param1);
         MainToolBar.Instance.hide();
         this.dispose();
      }
      
      override public function getBackType() : String
      {
         return StateType.MAIN;
      }
      
      override public function getType() : String
      {
         return StateType.HOT_SPRING_ROOM_LIST;
      }
      
      override public function dispose() : void
      {
         super.dispose();
         this.removeEvent();
         if(this._view)
         {
            this._view.hide();
            this._view.dispose();
         }
         this._view = null;
         if(this._model)
         {
            this._model.dispose();
         }
         this._model = null;
      }
   }
}
