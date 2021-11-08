package roomList.pvpRoomList
{
   import LimitAward.LimitAwardButton;
   import calendar.CalendarManager;
   import com.pickgliss.events.ListItemEvent;
   import com.pickgliss.ui.ComponentFactory;
   import com.pickgliss.ui.UICreatShortcut;
   import com.pickgliss.ui.controls.ComboBox;
   import com.pickgliss.ui.controls.SimpleBitmapButton;
   import com.pickgliss.ui.controls.container.SimpleTileList;
   import com.pickgliss.ui.controls.list.VectorListModel;
   import com.pickgliss.ui.core.Disposeable;
   import com.pickgliss.ui.image.Scale9CornerImage;
   import com.pickgliss.utils.ObjectUtils;
   import ddt.manager.BossBoxManager;
   import ddt.manager.GameInSocketOut;
   import ddt.manager.LanguageMgr;
   import ddt.manager.MessageTipManager;
   import ddt.manager.PlayerManager;
   import ddt.manager.SocketManager;
   import ddt.manager.SoundManager;
   import ddt.manager.TaskManager;
   import ddt.utils.PositionUtils;
   import ddt.view.bossbox.SmallBoxButton;
   import flash.display.Bitmap;
   import flash.display.Sprite;
   import flash.events.Event;
   import flash.events.MouseEvent;
   import flash.utils.getTimer;
   import road7th.data.DictionaryEvent;
   import room.model.RoomInfo;
   import roomList.LookupEnumerate;
   import serverlist.view.RoomListServerDropList;
   import trainer.controller.NewHandQueue;
   import trainer.data.ArrowType;
   import trainer.data.Step;
   import trainer.view.NewHandContainer;
   
   public class RoomListBGView extends Sprite implements Disposeable
   {
      
      public static var PREWORD:Array = [LanguageMgr.GetTranslation("tank.roomlist.RoomListIICreatePveRoomView.tank"),LanguageMgr.GetTranslation("tank.roomlist.RoomListIICreatePveRoomView.go"),LanguageMgr.GetTranslation("tank.roomlist.RoomListIICreatePveRoomView.fire")];
      
      public static const FULL_MODE:int = 0;
      
      public static const ATHLETICS_MODE:int = 1;
      
      public static const CHALLENGE_MODE:int = 2;
       
      
      private var _roomListBG:Scale9CornerImage;
      
      private var _roomListBGII:Scale9CornerImage;
      
      private var _titleBg:Bitmap;
      
      private var _titleBgII:Bitmap;
      
      private var _btnBG:Bitmap;
      
      private var _nextBtn:SimpleBitmapButton;
      
      private var _preBtn:SimpleBitmapButton;
      
      private var _createBtn:SimpleBitmapButton;
      
      private var _rivalshipBtn:SimpleBitmapButton;
      
      private var _lookUpBtn:SimpleBitmapButton;
      
      private var _itemList:SimpleTileList;
      
      private var _itemArray:Array;
      
      private var _model:RoomListModel;
      
      private var _controller:RoomListController;
      
      private var _boxButton:SmallBoxButton;
      
      private var _limitAwardButton:LimitAwardButton;
      
      private var _serverlist:RoomListServerDropList;
      
      private var _tempDataList:Array;
      
      private var _modeMenu:ComboBox;
      
      private var _currentMode:int;
      
      private var _isPermissionEnter:Boolean;
      
      private var _modeArray:Array;
      
      private var _selectItemPos:int;
      
      private var _selectItemID:int;
      
      private var _lastCreatTime:int = 0;
      
      public function RoomListBGView(param1:RoomListController, param2:RoomListModel)
      {
         this._modeArray = ["ddt.roomList.roomListBG.full","ddt.roomList.roomListBG.Athletics","ddt.roomList.roomListBG.challenge"];
         this._model = param2;
         this._controller = param1;
         super();
         this.init();
         this.initEvent();
      }
      
      private function init() : void
      {
         this._roomListBG = ComponentFactory.Instance.creat("roomList.pvpRoomList.roomListBG");
         addChild(this._roomListBG);
         this._roomListBGII = ComponentFactory.Instance.creat("roomList.pvpRoomList.roomListBGII");
         addChild(this._roomListBGII);
         this._titleBg = ComponentFactory.Instance.creat("asset.roomList.titleBg_01");
         addChild(this._titleBg);
         this._titleBgII = ComponentFactory.Instance.creat("asset.roomList.titleBg_02");
         addChild(this._titleBgII);
         this._btnBG = UICreatShortcut.creatAndAdd("asset.roomList.commonBG_03",this);
         this._modeMenu = UICreatShortcut.creatAndAdd("roomList.pvpRoomList.modeMenu",this);
         this._modeMenu.textField.text = LanguageMgr.GetTranslation(this._modeArray[FULL_MODE]);
         this._currentMode = FULL_MODE;
         this._itemList = ComponentFactory.Instance.creat("roomList.itemContainer",[2]);
         addChild(this._itemList);
         this.updateList();
         this.initButton();
         if(BossBoxManager.instance.isShowBoxButton())
         {
            this._boxButton = new SmallBoxButton(SmallBoxButton.PVR_ROOMLIST_POINT);
            PositionUtils.setPos(this._boxButton,"roomListItem.SmallBoxButtonPos");
            addChild(this._boxButton);
         }
         if(CalendarManager.getInstance().checkEventInfo() && PlayerManager.Instance.Self.Grade >= 8)
         {
            if(!this._limitAwardButton)
            {
               this._limitAwardButton = new LimitAwardButton(LimitAwardButton.PVR_ROOMLIST_POINT);
               addChild(this._limitAwardButton);
            }
         }
         this._isPermissionEnter = true;
      }
      
      private function initButton() : void
      {
         this._nextBtn = ComponentFactory.Instance.creat("asset.roomList.nextBtn");
         addChild(this._nextBtn);
         this._preBtn = ComponentFactory.Instance.creat("asset.roomList.preBtn");
         addChild(this._preBtn);
         this._createBtn = ComponentFactory.Instance.creat("asset.roomList.createBtn");
         this._createBtn.tipData = LanguageMgr.GetTranslation("tank.roomlist.RoomListIIRoomBtnPanel.createRoom");
         addChild(this._createBtn);
         this._rivalshipBtn = ComponentFactory.Instance.creat("asset.roomList.rivalshipBtn");
         this._rivalshipBtn.tipData = LanguageMgr.GetTranslation("tank.roomlist.joinBattleQuickly");
         addChild(this._rivalshipBtn);
         this._lookUpBtn = ComponentFactory.Instance.creat("asset.roomList.lookupBtn");
         this._lookUpBtn.tipData = LanguageMgr.GetTranslation("tank.roomlist.RoomListIIRoomBtnPanel.findRoom");
         addChild(this._lookUpBtn);
         this._serverlist = ComponentFactory.Instance.creat("serverlist.room.ServerDropList");
         addChild(this._serverlist);
         this.addTipPanel();
         this._itemArray = [];
         this.userGuide();
      }
      
      private function userGuide() : void
      {
         if(TaskManager.isAchieved(TaskManager.getQuestByID(320)) && !TaskManager.isAchieved(TaskManager.getQuestByID(321)))
         {
            NewHandQueue.Instance.push(new Step(1,this.expWeaponTip,this.preWeaponTip,this.finStoneTip));
         }
      }
      
      private function expWeaponTip() : void
      {
      }
      
      private function preWeaponTip() : void
      {
         NewHandContainer.Instance.showArrow(ArrowType.BAG_ROOM,0,"trainer.RoomListBGViewArrowPos");
      }
      
      private function finStoneTip() : void
      {
         this.disposeUserGuide();
      }
      
      private function disposeUserGuide() : void
      {
         NewHandContainer.Instance.clearArrowByID(ArrowType.BAG_ROOM);
         NewHandQueue.Instance.dispose();
      }
      
      private function initEvent() : void
      {
         this._createBtn.addEventListener(MouseEvent.CLICK,this.__createBtnClick);
         this._rivalshipBtn.addEventListener(MouseEvent.CLICK,this._rivalshipClick);
         this._lookUpBtn.addEventListener(MouseEvent.CLICK,this.__lookupClick);
         this._nextBtn.addEventListener(MouseEvent.CLICK,this.__updateClick);
         this._preBtn.addEventListener(MouseEvent.CLICK,this.__updateClick);
         this._model.addEventListener(RoomListModel.ROOM_ITEM_UPDATE,this.__updateItem);
         this._model.getRoomList().addEventListener(DictionaryEvent.CLEAR,this.__clearRoom);
         this._modeMenu.listPanel.list.addEventListener(ListItemEvent.LIST_ITEM_CLICK,this.__onListClick);
      }
      
      private function removeEvent() : void
      {
         this._createBtn.removeEventListener(MouseEvent.CLICK,this.__createBtnClick);
         this._rivalshipBtn.removeEventListener(MouseEvent.CLICK,this._rivalshipClick);
         this._lookUpBtn.removeEventListener(MouseEvent.CLICK,this.__lookupClick);
         this._nextBtn.removeEventListener(MouseEvent.CLICK,this.__updateClick);
         this._preBtn.removeEventListener(MouseEvent.CLICK,this.__updateClick);
         this._model.removeEventListener(RoomListModel.ROOM_ITEM_UPDATE,this.__updateItem);
         if(this._model.getRoomList())
         {
            this._model.getRoomList().removeEventListener(DictionaryEvent.CLEAR,this.__clearRoom);
         }
      }
      
      private function __updateItem(param1:Event) : void
      {
         this.upadteItemPos();
         this._isPermissionEnter = true;
      }
      
      private function __onListClick(param1:ListItemEvent) : void
      {
         SoundManager.instance.play("008");
         this._currentMode = this.getCurrentMode(param1.cellValue);
         this.addTipPanel();
      }
      
      private function getCurrentMode(param1:String) : int
      {
         var _loc2_:int = 0;
         while(_loc2_ < this._modeArray.length)
         {
            if(LanguageMgr.GetTranslation(this._modeArray[_loc2_]) == param1)
            {
               return _loc2_;
            }
            _loc2_++;
         }
         return -1;
      }
      
      private function addTipPanel() : void
      {
         var _loc1_:VectorListModel = this._modeMenu.listPanel.vectorListModel;
         _loc1_.clear();
         switch(this._currentMode)
         {
            case FULL_MODE:
               _loc1_.append(LanguageMgr.GetTranslation(this._modeArray[ATHLETICS_MODE]));
               _loc1_.append(LanguageMgr.GetTranslation(this._modeArray[CHALLENGE_MODE]));
               SocketManager.Instance.out.sendUpdateRoomList(LookupEnumerate.ROOM_LIST,LookupEnumerate.ROOMLIST_DEFAULT);
               break;
            case ATHLETICS_MODE:
               _loc1_.append(LanguageMgr.GetTranslation(this._modeArray[FULL_MODE]));
               _loc1_.append(LanguageMgr.GetTranslation(this._modeArray[CHALLENGE_MODE]));
               SocketManager.Instance.out.sendUpdateRoomList(LookupEnumerate.ROOM_LIST,LookupEnumerate.ROOMLIST_ATHLETICTICS);
               break;
            case CHALLENGE_MODE:
               _loc1_.append(LanguageMgr.GetTranslation(this._modeArray[FULL_MODE]));
               _loc1_.append(LanguageMgr.GetTranslation(this._modeArray[ATHLETICS_MODE]));
               SocketManager.Instance.out.sendUpdateRoomList(LookupEnumerate.ROOM_LIST,LookupEnumerate.ROOMLIST_DEFY);
         }
      }
      
      private function __clearRoom(param1:DictionaryEvent) : void
      {
         this.cleanItem();
         this._isPermissionEnter = true;
      }
      
      private function updateList() : void
      {
         var _loc2_:RoomInfo = null;
         var _loc3_:RoomListItemView = null;
         var _loc1_:int = 0;
         while(_loc1_ < this._model.getRoomList().length)
         {
            _loc2_ = this._model.getRoomList().list[_loc1_];
            _loc3_ = new RoomListItemView(_loc2_);
            _loc3_.addEventListener(MouseEvent.CLICK,this.__itemClick);
            this._itemList.addChild(_loc3_);
            this._itemArray.push(_loc3_);
            _loc1_++;
         }
      }
      
      private function cleanItem() : void
      {
         var _loc1_:int = 0;
         while(_loc1_ < this._itemArray.length)
         {
            (this._itemArray[_loc1_] as RoomListItemView).removeEventListener(MouseEvent.CLICK,this.__itemClick);
            (this._itemArray[_loc1_] as RoomListItemView).dispose();
            _loc1_++;
         }
         this._itemList.disposeAllChildren();
         this._itemArray = [];
      }
      
      private function __itemClick(param1:MouseEvent) : void
      {
         if(!this._isPermissionEnter)
         {
            return;
         }
         this.gotoIntoRoom((param1.currentTarget as RoomListItemView).info);
         this.getSelectItemPos((param1.currentTarget as RoomListItemView).id);
      }
      
      private function getSelectItemPos(param1:int) : int
      {
         if(!this._itemList)
         {
            return 0;
         }
         var _loc2_:int = 0;
         while(_loc2_ < this._itemArray.length)
         {
            if(!(this._itemArray[_loc2_] as RoomListItemView))
            {
               return 0;
            }
            if((this._itemArray[_loc2_] as RoomListItemView).id == param1)
            {
               this._selectItemPos = _loc2_;
               this._selectItemID = (this._itemArray[_loc2_] as RoomListItemView).id;
               return _loc2_;
            }
            _loc2_++;
         }
         return 0;
      }
      
      public function get currentDataList() : Array
      {
         if(this._model.roomShowMode == 1)
         {
            return this._model.getRoomList().filter("isPlaying",false).concat(this._model.getRoomList().filter("isPlaying",true));
         }
         return this._model.getRoomList().list;
      }
      
      private function getInfosPos(param1:int) : int
      {
         this._tempDataList = this.currentDataList;
         if(!this._tempDataList)
         {
            return 0;
         }
         var _loc2_:int = 0;
         while(_loc2_ < this._tempDataList.length)
         {
            if((this._tempDataList[_loc2_] as RoomInfo).ID == param1)
            {
               return _loc2_;
            }
            _loc2_++;
         }
         return 0;
      }
      
      private function upadteItemPos() : void
      {
         var _loc1_:RoomInfo = null;
         var _loc2_:int = 0;
         var _loc3_:RoomInfo = null;
         var _loc4_:RoomListItemView = null;
         this._tempDataList = this.currentDataList;
         if(this._tempDataList)
         {
            _loc1_ = this._tempDataList[this._selectItemPos];
            _loc2_ = this.getInfosPos(this._selectItemID);
            this._tempDataList[this._selectItemPos] = this._tempDataList[_loc2_];
            this._tempDataList[_loc2_] = _loc1_;
            this._tempDataList = this.sortRooInfo(this._tempDataList);
            this.cleanItem();
            for each(_loc3_ in this._tempDataList)
            {
               if(!_loc3_)
               {
                  return;
               }
               _loc4_ = new RoomListItemView(_loc3_);
               _loc4_.addEventListener(MouseEvent.CLICK,this.__itemClick,false,0,true);
               this._itemList.addChild(_loc4_);
               this._itemArray.push(_loc4_);
            }
         }
      }
      
      private function sortRooInfo(param1:Array) : Array
      {
         var _loc3_:int = 0;
         var _loc4_:RoomInfo = null;
         var _loc2_:Array = new Array();
         switch(this._currentMode)
         {
            case ATHLETICS_MODE:
               _loc3_ = 0;
               break;
            case CHALLENGE_MODE:
               _loc3_ = 1;
         }
         for each(_loc4_ in param1)
         {
            if(_loc4_)
            {
               if(_loc4_.type == _loc3_ && !_loc4_.isPlaying)
               {
                  _loc2_.unshift(_loc4_);
               }
               else
               {
                  _loc2_.push(_loc4_);
               }
            }
         }
         return _loc2_;
      }
      
      private function gotoTip(param1:int) : Boolean
      {
         if(param1 == 0)
         {
            if(PlayerManager.Instance.Self.Grade < 6)
            {
               MessageTipManager.getInstance().show(LanguageMgr.GetTranslation("tank.roomlist.notGotoIntoRoom",6,LanguageMgr.GetTranslation("tank.view.chat.ChannelListSelectView.ream")));
               return true;
            }
         }
         else if(param1 == 1)
         {
            if(PlayerManager.Instance.Self.Grade < 12)
            {
               MessageTipManager.getInstance().show(LanguageMgr.GetTranslation("tank.roomlist.notGotoIntoRoom",12,LanguageMgr.GetTranslation("tank.roomlist.challenge")));
               return true;
            }
         }
         return false;
      }
      
      public function gotoIntoRoom(param1:RoomInfo) : void
      {
         SoundManager.instance.play("008");
         if(this.gotoTip(param1.type))
         {
            return;
         }
         SocketManager.Instance.out.sendGameLogin(1,-1,param1.ID,"");
         this._isPermissionEnter = false;
      }
      
      private function __lookupClick(param1:MouseEvent) : void
      {
         SoundManager.instance.play("008");
         this._controller.showFindRoom();
      }
      
      private function _rivalshipClick(param1:MouseEvent) : void
      {
         SoundManager.instance.play("008");
         if(!this._isPermissionEnter)
         {
            return;
         }
         if(this.gotoTip(0))
         {
            return;
         }
         SocketManager.Instance.out.sendGameLogin(1,0);
         this._isPermissionEnter = false;
      }
      
      private function __updateClick(param1:MouseEvent) : void
      {
         SoundManager.instance.play("008");
         this.sendUpdate();
      }
      
      private function __placeCountClick(param1:MouseEvent) : void
      {
         SoundManager.instance.play("008");
         this.sendUpdate();
      }
      
      private function __hardLevelClick(param1:MouseEvent) : void
      {
         SoundManager.instance.play("008");
         this.sendUpdate();
      }
      
      private function __roomModeClick(param1:MouseEvent) : void
      {
         SoundManager.instance.play("008");
         this.sendUpdate();
      }
      
      private function __roomNameClick(param1:MouseEvent) : void
      {
         SoundManager.instance.play("008");
         this.sendUpdate();
      }
      
      private function __idBtnClick(param1:MouseEvent) : void
      {
         SoundManager.instance.play("008");
         this.sendUpdate();
      }
      
      private function sendUpdate() : void
      {
         switch(this._currentMode)
         {
            case FULL_MODE:
               SocketManager.Instance.out.sendUpdateRoomList(LookupEnumerate.ROOM_LIST,LookupEnumerate.ROOMLIST_DEFAULT);
               break;
            case ATHLETICS_MODE:
               SocketManager.Instance.out.sendUpdateRoomList(LookupEnumerate.ROOM_LIST,LookupEnumerate.ROOMLIST_ATHLETICTICS);
               break;
            case CHALLENGE_MODE:
               SocketManager.Instance.out.sendUpdateRoomList(LookupEnumerate.ROOM_LIST,LookupEnumerate.ROOMLIST_DEFY);
         }
      }
      
      private function __createBtnClick(param1:MouseEvent) : void
      {
         SoundManager.instance.play("008");
         if(getTimer() - this._lastCreatTime > 2000)
         {
            this._lastCreatTime = getTimer();
            GameInSocketOut.sendCreateRoom(PREWORD[int(Math.random() * PREWORD.length)],0,3);
         }
      }
      
      public function dispose() : void
      {
         this.removeEvent();
         this.cleanItem();
         this._roomListBG.dispose();
         this._roomListBG = null;
         this._roomListBGII.dispose();
         this._roomListBGII = null;
         ObjectUtils.disposeObject(this._titleBg);
         this._titleBg = null;
         ObjectUtils.disposeObject(this._titleBgII);
         this._titleBgII = null;
         this._nextBtn.dispose();
         this._nextBtn = null;
         this._preBtn.dispose();
         this._preBtn = null;
         this._createBtn.dispose();
         this._createBtn = null;
         this._rivalshipBtn.dispose();
         this._rivalshipBtn = null;
         this._lookUpBtn.dispose();
         this._lookUpBtn = null;
         if(this._itemList)
         {
            this._itemList.disposeAllChildren();
         }
         ObjectUtils.disposeObject(this._itemList);
         this._itemList = null;
         this._itemArray = null;
         if(this._boxButton && this._boxButton.parent)
         {
            this._boxButton.parent.removeChild(this._boxButton);
            this._boxButton.dispose();
            this._boxButton = null;
         }
         if(this._btnBG)
         {
            ObjectUtils.disposeObject(this._btnBG);
            this._btnBG = null;
         }
         if(this._modeMenu)
         {
            ObjectUtils.disposeObject(this._modeMenu);
            this._modeMenu = null;
         }
         if(this._serverlist)
         {
            this._serverlist.dispose();
            this._serverlist = null;
         }
         if(this.parent)
         {
            this.parent.removeChild(this);
         }
         if(this._limitAwardButton)
         {
            ObjectUtils.disposeObject(this._limitAwardButton);
         }
         this._limitAwardButton = null;
      }
   }
}
