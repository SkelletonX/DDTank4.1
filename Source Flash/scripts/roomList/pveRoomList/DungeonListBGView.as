package roomList.pveRoomList
{
   import LimitAward.LimitAwardButton;
   import calendar.CalendarManager;
   import com.pickgliss.toplevel.StageReferance;
   import com.pickgliss.ui.ComponentFactory;
   import com.pickgliss.ui.controls.BaseButton;
   import com.pickgliss.ui.controls.Scrollbar;
   import com.pickgliss.ui.controls.SimpleBitmapButton;
   import com.pickgliss.ui.controls.container.SimpleTileList;
   import com.pickgliss.ui.core.Disposeable;
   import com.pickgliss.ui.image.ScaleBitmapImage;
   import com.pickgliss.ui.text.FilterFrameText;
   import com.pickgliss.utils.DisplayUtils;
   import com.pickgliss.utils.ObjectUtils;
   import ddt.manager.BossBoxManager;
   import ddt.manager.GameInSocketOut;
   import ddt.manager.LanguageMgr;
   import ddt.manager.MapManager;
   import ddt.manager.PlayerManager;
   import ddt.manager.ServerManager;
   import ddt.manager.SocketManager;
   import ddt.manager.SoundManager;
   import ddt.view.bossbox.SmallBoxButton;
   import flash.display.Bitmap;
   import flash.display.DisplayObject;
   import flash.display.Sprite;
   import flash.events.Event;
   import flash.events.MouseEvent;
   import flash.geom.Point;
   import flash.utils.getTimer;
   import road7th.data.DictionaryEvent;
   import room.RoomManager;
   import room.model.RoomInfo;
   import room.view.chooseMap.DungeonChooseMapView;
   import roomList.LookupEnumerate;
   import roomList.RoomListMapTipPanel;
   import roomList.RoomListTipPanel;
   import serverlist.view.RoomListServerDropList;
   
   public class DungeonListBGView extends Sprite implements Disposeable
   {
      
      public static var PREWORD:Array = [LanguageMgr.GetTranslation("tank.roomlist.RoomListIICreatePveRoomView.tank"),LanguageMgr.GetTranslation("tank.roomlist.RoomListIICreatePveRoomView.go"),LanguageMgr.GetTranslation("tank.roomlist.RoomListIICreatePveRoomView.fire")];
       
      
      private var _dungeonListBG:Bitmap;
      
      private var _model:DungeonListModel;
      
      private var _bmpSiftBg:Bitmap;
      
      private var _bmpSiftFb:Bitmap;
      
      private var _bmpSiftHardLv:Bitmap;
      
      private var _btnSiftReset:SimpleBitmapButton;
      
      private var _bmpCbFb:BaseButton;
      
      private var _bmpCbHardLv:BaseButton;
      
      private var _txtCbFb:FilterFrameText;
      
      private var _txtCbHardLv:FilterFrameText;
      
      private var _iconBtnII:SimpleBitmapButton;
      
      private var _iconBtnIII:SimpleBitmapButton;
      
      private var _nextBtn:SimpleBitmapButton;
      
      private var _preBtn:SimpleBitmapButton;
      
      private var _createBtn:SimpleBitmapButton;
      
      private var _rivalshipBtn:SimpleBitmapButton;
      
      private var _lookUpBtn:SimpleBitmapButton;
      
      private var _itemList:SimpleTileList;
      
      private var _itemArray:Array;
      
      private var _pveHardLeveRoomListTipPanel:RoomListTipPanel;
      
      private var _pveMapRoomListTipPanel:RoomListMapTipPanel;
      
      private var _controlle:DungeonListController;
      
      private var _boxButton:SmallBoxButton;
      
      private var _limitAwardButton:LimitAwardButton;
      
      private var _tempDataList:Array;
      
      private var _serverlist:RoomListServerDropList;
      
      private var _isPermissionEnter:Boolean;
      
      private var _selectItemPos:int;
      
      private var _selectItemID:int;
      
      private var _last_creat:uint;
      
      public function DungeonListBGView(param1:DungeonListController, param2:DungeonListModel)
      {
         this._controlle = param1;
         this._model = param2;
         super();
         this.init();
         this.initEvent();
      }
      
      private function init() : void
      {
         this._itemArray = [];
         this._dungeonListBG = ComponentFactory.Instance.creat("asset.DungeonList.DungeonListBG");
         addChild(this._dungeonListBG);
         this._bmpSiftBg = ComponentFactory.Instance.creatBitmap("asset.roomList.siftBg");
         addChild(this._bmpSiftBg);
         this._bmpSiftFb = ComponentFactory.Instance.creatBitmap("asset.roomList.siftFb");
         addChild(this._bmpSiftFb);
         this._bmpSiftHardLv = ComponentFactory.Instance.creatBitmap("asset.roomList.siftHardLv");
         addChild(this._bmpSiftHardLv);
         this._btnSiftReset = ComponentFactory.Instance.creat("asset.roomList.btnSiftReset");
         addChild(this._btnSiftReset);
         this._bmpCbFb = ComponentFactory.Instance.creat("asset.roomList.bmpCbFb");
         addChild(this._bmpCbFb);
         this._bmpCbHardLv = ComponentFactory.Instance.creat("asset.roomList.bmpCbHardLv");
         this._bmpCbHardLv.width = 107;
         addChild(this._bmpCbHardLv);
         this._txtCbFb = ComponentFactory.Instance.creat("asset.roomList.txtFb");
         this._txtCbFb.mouseEnabled = false;
         addChild(this._txtCbFb);
         this._txtCbHardLv = ComponentFactory.Instance.creat("asset.roomList.txtHardLv");
         this._txtCbHardLv.mouseEnabled = false;
         addChild(this._txtCbHardLv);
         this._nextBtn = ComponentFactory.Instance.creat("asset.DungeonList.nextBtn");
         addChild(this._nextBtn);
         this._preBtn = ComponentFactory.Instance.creat("asset.DungeonList.preBtn");
         addChild(this._preBtn);
         this._createBtn = ComponentFactory.Instance.creat("asset.DungeonList.createBtn");
         this._createBtn.tipData = LanguageMgr.GetTranslation("tank.roomlist.RoomListIIRoomBtnPanel.createRoom");
         addChild(this._createBtn);
         this._rivalshipBtn = ComponentFactory.Instance.creat("asset.DungeonList.rivalshipBtn");
         this._rivalshipBtn.tipData = LanguageMgr.GetTranslation("tank.roomlist.joinDuplicateQuickly");
         addChild(this._rivalshipBtn);
         this._lookUpBtn = ComponentFactory.Instance.creat("asset.DungeonList.lookupBtn");
         this._lookUpBtn.tipData = LanguageMgr.GetTranslation("tank.roomlist.RoomListIIRoomBtnPanel.findRoom");
         addChild(this._lookUpBtn);
         this._iconBtnII = ComponentFactory.Instance.creat("asset.DungeonList.iconButton_02");
         addChild(this._iconBtnII);
         this._iconBtnIII = ComponentFactory.Instance.creat("asset.DungeonList.iconButton_03");
         addChild(this._iconBtnIII);
         var _loc1_:String = String(ServerManager.Instance.current.Name);
         var _loc2_:int = _loc1_.indexOf("(");
         _loc2_ = _loc2_ == -1?int(_loc1_.length):int(_loc2_);
         this._itemList = ComponentFactory.Instance.creat("roomList.DungeonList.ItemList",[2]);
         addChild(this._itemList);
         this._serverlist = ComponentFactory.Instance.creat("serverlist.room.ServerDropList");
         addChild(this._serverlist);
         this.addTipPanel();
         this.resetSift();
         if(BossBoxManager.instance.isShowBoxButton())
         {
            this._boxButton = new SmallBoxButton(SmallBoxButton.PVE_ROOMLIST_POINT);
            addChild(this._boxButton);
         }
         if(CalendarManager.getInstance().checkEventInfo() && PlayerManager.Instance.Self.Grade >= 8)
         {
            if(!this._limitAwardButton)
            {
               this._limitAwardButton = new LimitAwardButton(LimitAwardButton.PVE_ROOMLIST_POINT);
               addChild(this._limitAwardButton);
            }
         }
         this._isPermissionEnter = true;
      }
      
      private function initEvent() : void
      {
         this._createBtn.addEventListener(MouseEvent.CLICK,this.__createClick);
         this._rivalshipBtn.addEventListener(MouseEvent.CLICK,this.__rivalshipBtnClick);
         this._iconBtnII.addEventListener(MouseEvent.CLICK,this.__iconBtnIIClick);
         this._iconBtnIII.addEventListener(MouseEvent.CLICK,this.__iconBtnIIIClick);
         this._bmpCbFb.addEventListener(MouseEvent.CLICK,this.__iconBtnIIClick);
         this._bmpCbHardLv.addEventListener(MouseEvent.CLICK,this.__iconBtnIIIClick);
         this._btnSiftReset.addEventListener(MouseEvent.CLICK,this.__siftReset);
         this._pveMapRoomListTipPanel.addEventListener(RoomListMapTipPanel.FB_CHANGE,this.__fbChange);
         this._pveHardLeveRoomListTipPanel.addEventListener(RoomListTipPanel.HARD_LV_CHANGE,this.__hardLvChange);
         this._nextBtn.addEventListener(MouseEvent.CLICK,this.__updateClick);
         this._preBtn.addEventListener(MouseEvent.CLICK,this.__updateClick);
         this._lookUpBtn.addEventListener(MouseEvent.CLICK,this.__lookupClick);
         this._model.addEventListener(DungeonListModel.DUNGEON_LIST_UPDATE,this.__addRoom);
         this._model.getRoomList().addEventListener(DictionaryEvent.CLEAR,this.__clearRoom);
         StageReferance.stage.addEventListener(MouseEvent.CLICK,this.__stageClick);
         RoomManager.Instance.addEventListener(RoomManager.LOGIN_ROOM_RESULT,this.__loginRoomRes);
      }
      
      private function removeEvent() : void
      {
         this._createBtn.removeEventListener(MouseEvent.CLICK,this.__createClick);
         this._rivalshipBtn.removeEventListener(MouseEvent.CLICK,this.__rivalshipBtnClick);
         this._iconBtnII.removeEventListener(MouseEvent.CLICK,this.__iconBtnIIClick);
         this._iconBtnIII.removeEventListener(MouseEvent.CLICK,this.__iconBtnIIIClick);
         this._bmpCbFb.removeEventListener(MouseEvent.CLICK,this.__iconBtnIIClick);
         this._bmpCbHardLv.removeEventListener(MouseEvent.CLICK,this.__iconBtnIIIClick);
         this._btnSiftReset.removeEventListener(MouseEvent.CLICK,this.__siftReset);
         this._pveMapRoomListTipPanel.removeEventListener(RoomListMapTipPanel.FB_CHANGE,this.__fbChange);
         this._pveHardLeveRoomListTipPanel.removeEventListener(RoomListTipPanel.HARD_LV_CHANGE,this.__hardLvChange);
         this._nextBtn.removeEventListener(MouseEvent.CLICK,this.__updateClick);
         this._preBtn.removeEventListener(MouseEvent.CLICK,this.__updateClick);
         this._lookUpBtn.removeEventListener(MouseEvent.CLICK,this.__lookupClick);
         this._model.removeEventListener(DungeonListModel.DUNGEON_LIST_UPDATE,this.__addRoom);
         this._model.getRoomList().removeEventListener(DictionaryEvent.CLEAR,this.__clearRoom);
         StageReferance.stage.removeEventListener(MouseEvent.CLICK,this.__stageClick);
         RoomManager.Instance.removeEventListener(RoomManager.LOGIN_ROOM_RESULT,this.__loginRoomRes);
      }
      
      private function __loginRoomRes(param1:Event) : void
      {
         this._isPermissionEnter = true;
      }
      
      private function __rivalshipBtnClick(param1:MouseEvent) : void
      {
         SoundManager.instance.play("008");
         if(!this._isPermissionEnter)
         {
            return;
         }
         SocketManager.Instance.out.sendGameLogin(LookupEnumerate.DUNGEON_LIST,4);
         this._isPermissionEnter = false;
      }
      
      private function __stageClick(param1:MouseEvent) : void
      {
         if(!DisplayUtils.isTargetOrContain(param1.target as DisplayObject,this._iconBtnII) && !DisplayUtils.isTargetOrContain(param1.target as DisplayObject,this._iconBtnIII) && !DisplayUtils.isTargetOrContain(param1.target as DisplayObject,this._bmpCbFb) && !DisplayUtils.isTargetOrContain(param1.target as DisplayObject,this._bmpCbHardLv) && !(param1.target is BaseButton) && !(param1.target is ScaleBitmapImage && (param1.target as DisplayObject).parent is Scrollbar))
         {
            this._pveMapRoomListTipPanel.visible = false;
            this._pveHardLeveRoomListTipPanel.visible = false;
         }
      }
      
      private function __lookupClick(param1:MouseEvent) : void
      {
         SoundManager.instance.play("008");
         this._controlle.showFindRoom();
      }
      
      private function __fbChange(param1:Event) : void
      {
         this.sendSift();
         if(this._pveMapRoomListTipPanel.value == 10000)
         {
            this.setTxtCbFb(LanguageMgr.GetTranslation("tank.roomlist.siftAllFb"));
         }
         else
         {
            this.setTxtCbFb(MapManager.getMapName(this._pveMapRoomListTipPanel.value));
         }
      }
      
      private function __hardLvChange(param1:Event) : void
      {
         this.sendSift();
         this.setTxtCbHardLv(this.getHardLvTxt(this._pveHardLeveRoomListTipPanel.value));
      }
      
      private function __siftReset(param1:MouseEvent) : void
      {
         SoundManager.instance.playButtonSound();
         this.resetSift();
         this.sendSift();
      }
      
      private function sendSift() : void
      {
         SocketManager.Instance.out.sendUpdateRoomList(LookupEnumerate.DUNGEON_LIST,-2,this._pveMapRoomListTipPanel.value,this._pveHardLeveRoomListTipPanel.value);
      }
      
      private function resetSift() : void
      {
         this._pveMapRoomListTipPanel.resetValue();
         this._pveHardLeveRoomListTipPanel.resetValue();
         this.setTxtCbFb(LanguageMgr.GetTranslation("tank.roomlist.siftAllFb"));
         this.setTxtCbHardLv("tank.room.difficulty.all");
      }
      
      private function setTxtCbFb(param1:String) : void
      {
         this._txtCbFb.text = param1;
         this._txtCbFb.x = this._bmpCbFb.x + (this._bmpCbFb.width - this._iconBtnII.width - this._txtCbFb.width) / 2;
      }
      
      private function setTxtCbHardLv(param1:String) : void
      {
         this._txtCbHardLv.text = LanguageMgr.GetTranslation(param1);
         this._txtCbHardLv.x = this._bmpCbHardLv.x + (this._bmpCbHardLv.width - this._iconBtnIII.width - this._txtCbHardLv.width) / 2;
      }
      
      private function getHardLvTxt(param1:int) : String
      {
         switch(param1)
         {
            case LookupEnumerate.DUNGEON_LIST_SIMPLE:
               return "tank.room.difficulty.simple";
            case LookupEnumerate.DUNGEON_LIST_COMMON:
               return "tank.room.difficulty.normal";
            case LookupEnumerate.DUNGEON_LIST_STRAIT:
               return "tank.room.difficulty.hard";
            case LookupEnumerate.DUNGEON_LIST_HERO:
               return "tank.room.difficulty.hero";
            default:
               return "tank.room.difficulty.all";
         }
      }
      
      private function addTipPanel() : void
      {
         var _loc1_:Bitmap = ComponentFactory.Instance.creatBitmap("asset.roomList.hardLevel_01");
         var _loc2_:Bitmap = ComponentFactory.Instance.creatBitmap("asset.roomList.hardLevel_02");
         var _loc3_:Bitmap = ComponentFactory.Instance.creatBitmap("asset.roomList.hardLevel_03");
         var _loc4_:Bitmap = ComponentFactory.Instance.creatBitmap("asset.roomList.hardLevel_04");
         var _loc5_:Bitmap = ComponentFactory.Instance.creatBitmap("asset.roomList.hardLevel_05");
         var _loc6_:Point = ComponentFactory.Instance.creatCustomObject("roomList.DungeonList.DungeonListTipPanelSizeII");
         this._pveHardLeveRoomListTipPanel = new RoomListTipPanel(_loc6_.x,_loc6_.y);
         this._pveHardLeveRoomListTipPanel.addItem(_loc5_,LookupEnumerate.DUNGEON_LIST_ALL);
         this._pveHardLeveRoomListTipPanel.addItem(_loc1_,LookupEnumerate.DUNGEON_LIST_SIMPLE);
         this._pveHardLeveRoomListTipPanel.addItem(_loc2_,LookupEnumerate.DUNGEON_LIST_COMMON);
         this._pveHardLeveRoomListTipPanel.addItem(_loc3_,LookupEnumerate.DUNGEON_LIST_STRAIT);
         this._pveHardLeveRoomListTipPanel.addItem(_loc4_,LookupEnumerate.DUNGEON_LIST_HERO);
         var _loc7_:Point = ComponentFactory.Instance.creatCustomObject("roomList.DungeonList.pveHardLeveRoomListTipPanelPos");
         this._pveHardLeveRoomListTipPanel.x = _loc7_.x;
         this._pveHardLeveRoomListTipPanel.y = _loc7_.y;
         this._pveHardLeveRoomListTipPanel.visible = false;
         addChild(this._pveHardLeveRoomListTipPanel);
         var _loc8_:Point = ComponentFactory.Instance.creatCustomObject("roomList.DungeonList.pveMapPanelPos");
         var _loc9_:Point = ComponentFactory.Instance.creatCustomObject("roomList.DungeonList.DungeonListTipPanelSizeIII");
         this._pveMapRoomListTipPanel = new RoomListMapTipPanel(_loc9_.x,_loc9_.y);
         this._pveMapRoomListTipPanel.x = _loc8_.x;
         this._pveMapRoomListTipPanel.y = _loc8_.y;
         this._pveMapRoomListTipPanel.addItem(10000);
         var _loc10_:int = 1;
         while(_loc10_ < DungeonChooseMapView.DUNGEON_NO)
         {
            if(MapManager.getByOrderingDungeonInfo(_loc10_))
            {
               this._pveMapRoomListTipPanel.addItem(MapManager.getByOrderingDungeonInfo(_loc10_).ID);
            }
            _loc10_++;
         }
         var _loc11_:int = 1;
         while(_loc11_ < DungeonChooseMapView.DUNGEON_NO)
         {
            if(MapManager.getByOrderingSpecialDungeonInfo(_loc11_))
            {
               this._pveMapRoomListTipPanel.addItem(MapManager.getByOrderingSpecialDungeonInfo(_loc11_).ID);
            }
            _loc11_++;
         }
         this._pveMapRoomListTipPanel.visible = false;
         addChild(this._pveMapRoomListTipPanel);
      }
      
      private function __clearRoom(param1:DictionaryEvent) : void
      {
         this.cleanItem();
         this._isPermissionEnter = true;
      }
      
      private function __addRoom(param1:Event) : void
      {
         this.upadteItemPos();
         this._isPermissionEnter = true;
      }
      
      private function upadteItemPos() : void
      {
         var _loc1_:RoomInfo = null;
         var _loc2_:int = 0;
         var _loc3_:RoomInfo = null;
         var _loc4_:DungeonListItemView = null;
         this._tempDataList = this.currentDataList;
         if(this._tempDataList)
         {
            _loc1_ = this._tempDataList[this._selectItemPos];
            _loc2_ = this.getInfosPos(this._selectItemID);
            this._tempDataList[this._selectItemPos] = this._tempDataList[_loc2_];
            this._tempDataList[_loc2_] = _loc1_;
            this.cleanItem();
            for each(_loc3_ in this._tempDataList)
            {
               if(_loc3_)
               {
                  _loc4_ = new DungeonListItemView(_loc3_);
                  _loc4_.addEventListener(MouseEvent.CLICK,this.__itemClick,false,0,true);
                  this._itemList.addChild(_loc4_);
                  this._itemArray.push(_loc4_);
               }
            }
         }
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
            if(!(this._itemArray[_loc2_] as DungeonListItemView))
            {
               return 0;
            }
            if((this._itemArray[_loc2_] as DungeonListItemView).id == param1)
            {
               this._selectItemPos = _loc2_;
               this._selectItemID = (this._itemArray[_loc2_] as DungeonListItemView).id;
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
      
      private function __iconBtnIIClick(param1:MouseEvent) : void
      {
         SoundManager.instance.play("008");
         this._pveMapRoomListTipPanel.visible = !this._pveMapRoomListTipPanel.visible;
         this._pveHardLeveRoomListTipPanel.visible = false;
      }
      
      private function __iconBtnIIIClick(param1:MouseEvent) : void
      {
         SoundManager.instance.play("008");
         this._pveHardLeveRoomListTipPanel.visible = !this._pveHardLeveRoomListTipPanel.visible;
         this._pveMapRoomListTipPanel.visible = false;
      }
      
      private function __updateClick(param1:MouseEvent) : void
      {
         SoundManager.instance.play("008");
         this.sendSift();
      }
      
      private function __itemClick(param1:MouseEvent) : void
      {
         if(!this._isPermissionEnter)
         {
            return;
         }
         this.gotoIntoRoom((param1.currentTarget as DungeonListItemView).info);
         this.getSelectItemPos((param1.currentTarget as DungeonListItemView).id);
      }
      
      public function gotoIntoRoom(param1:RoomInfo) : void
      {
         SoundManager.instance.play("008");
         SocketManager.Instance.out.sendGameLogin(2,-1,param1.ID,"");
         this._isPermissionEnter = false;
      }
      
      private function __createClick(param1:MouseEvent) : void
      {
         SoundManager.instance.play("008");
         if(getTimer() - this._last_creat >= 2000)
         {
            this._last_creat = getTimer();
            GameInSocketOut.sendCreateRoom(PREWORD[int(Math.random() * PREWORD.length)],4,3);
         }
      }
      
      private function cleanItem() : void
      {
         var _loc1_:int = 0;
         while(_loc1_ < this._itemArray.length)
         {
            (this._itemArray[_loc1_] as DungeonListItemView).removeEventListener(MouseEvent.CLICK,this.__itemClick);
            (this._itemArray[_loc1_] as DungeonListItemView).dispose();
            _loc1_++;
         }
         this._itemList.disposeAllChildren();
         this._itemArray = [];
      }
      
      public function dispose() : void
      {
         this.removeEvent();
         this.cleanItem();
         this._itemList.dispose();
         this._itemList = null;
         this._iconBtnII.dispose();
         this._iconBtnII = null;
         this._iconBtnIII.dispose();
         this._iconBtnIII = null;
         ObjectUtils.disposeObject(this._bmpSiftBg);
         this._bmpSiftBg = null;
         ObjectUtils.disposeObject(this._bmpSiftFb);
         this._bmpSiftFb = null;
         ObjectUtils.disposeObject(this._bmpSiftHardLv);
         this._bmpSiftHardLv = null;
         ObjectUtils.disposeObject(this._bmpCbFb);
         this._bmpCbFb = null;
         ObjectUtils.disposeObject(this._bmpCbHardLv);
         this._bmpCbHardLv = null;
         ObjectUtils.disposeObject(this._txtCbFb);
         this._txtCbFb = null;
         ObjectUtils.disposeObject(this._txtCbHardLv);
         this._txtCbHardLv = null;
         ObjectUtils.disposeObject(this._btnSiftReset);
         this._btnSiftReset = null;
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
         if(this._limitAwardButton)
         {
            ObjectUtils.disposeObject(this._limitAwardButton);
         }
         this._limitAwardButton = null;
         if(this._pveHardLeveRoomListTipPanel && this._pveHardLeveRoomListTipPanel.parent)
         {
            this._pveHardLeveRoomListTipPanel.parent.removeChild(this._pveHardLeveRoomListTipPanel);
         }
         this._pveHardLeveRoomListTipPanel.dispose();
         this._pveHardLeveRoomListTipPanel = null;
         if(this._pveMapRoomListTipPanel && this._pveMapRoomListTipPanel.parent)
         {
            this._pveMapRoomListTipPanel.parent.removeChild(this._pveMapRoomListTipPanel);
         }
         this._pveMapRoomListTipPanel.dispose();
         this._pveMapRoomListTipPanel = null;
         if(this._boxButton && this._boxButton.parent)
         {
            this._boxButton.parent.removeChild(this._boxButton);
            this._boxButton.dispose();
            this._boxButton = null;
         }
         if(this._serverlist)
         {
            ObjectUtils.disposeObject(this._serverlist);
            this._serverlist = null;
         }
         if(this._dungeonListBG)
         {
            ObjectUtils.disposeObject(this._dungeonListBG);
            this._dungeonListBG = null;
         }
         if(this.parent)
         {
            this.parent.removeChild(this);
         }
      }
   }
}
