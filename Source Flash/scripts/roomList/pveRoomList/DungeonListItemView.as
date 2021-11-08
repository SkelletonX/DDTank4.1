package roomList.pveRoomList
{
   import com.pickgliss.loader.BaseLoader;
   import com.pickgliss.loader.DisplayLoader;
   import com.pickgliss.loader.LoaderEvent;
   import com.pickgliss.loader.LoaderManager;
   import com.pickgliss.ui.ComponentFactory;
   import com.pickgliss.ui.UICreatShortcut;
   import com.pickgliss.ui.core.Disposeable;
   import com.pickgliss.ui.image.ScaleFrameImage;
   import com.pickgliss.ui.text.FilterFrameText;
   import com.pickgliss.utils.ObjectUtils;
   import ddt.manager.LanguageMgr;
   import ddt.manager.PathManager;
   import ddt.utils.PositionUtils;
   import flash.display.Bitmap;
   import flash.display.Sprite;
   import flash.geom.Rectangle;
   import room.model.RoomInfo;
   
   public class DungeonListItemView extends Sprite implements Disposeable
   {
      
      public static const NAN_MAP:int = 10000;
       
      
      private var _info:RoomInfo;
      
      private var _mode:ScaleFrameImage;
      
      private var _itemBg:ScaleFrameImage;
      
      private var _lock:Bitmap;
      
      private var _nameText:FilterFrameText;
      
      private var _placeCountText:FilterFrameText;
      
      private var _hard:FilterFrameText;
      
      private var _simpMapLoader:DisplayLoader;
      
      private var _loader:DisplayLoader;
      
      private var _mapShowContainer:Sprite;
      
      private var _mapShow:Bitmap;
      
      private var _simpMapShow:Bitmap;
      
      private var _defaultMask:Bitmap;
      
      private var _mask:Sprite;
      
      private var _hardLevel:Array;
      
      public function DungeonListItemView(param1:RoomInfo = null)
      {
         this._hardLevel = ["tank.room.difficulty.simple","tank.room.difficulty.normal","tank.room.difficulty.hard","tank.room.difficulty.hero","tank.room.difficulty.none"];
         this._info = param1;
         super();
         this.init();
      }
      
      private function init() : void
      {
         this.buttonMode = true;
         this._mapShowContainer = new Sprite();
         addChild(this._mapShowContainer);
         this._itemBg = ComponentFactory.Instance.creat("roomList.DungeonList.DungeonListItem");
         this._itemBg.setFrame(1);
         addChild(this._itemBg);
         this._defaultMask = UICreatShortcut.creatAndAdd("asset.DungeonList.itemMask",this);
         this._hard = UICreatShortcut.creatTextAndAdd("asset.DungeonList.itemHardTextStyle","XXOO",this);
         this._nameText = ComponentFactory.Instance.creat("roomList.DungeonList.nameText");
         addChild(this._nameText);
         this._placeCountText = ComponentFactory.Instance.creat("roomList.DungeonList.placeCountText");
         addChild(this._placeCountText);
         this._lock = ComponentFactory.Instance.creatBitmap("asset.roomList.lock");
         PositionUtils.setPos(this._lock,"dungeonList.lockPos");
         this._lock.visible = false;
         addChild(this._lock);
         var _loc1_:Rectangle = ComponentFactory.Instance.creatCustomObject("dungeonList.maskRectangle");
         this._mask = new Sprite();
         this._mask.visible = false;
         this._mask.graphics.beginFill(0,0);
         this._mask.graphics.drawRoundRect(0,0,_loc1_.width,_loc1_.height,_loc1_.y);
         this._mask.graphics.endFill();
         PositionUtils.setPos(this._mask,"roomListItem.maskPos");
         addChild(this._mask);
         this.update();
      }
      
      public function get info() : RoomInfo
      {
         return this._info;
      }
      
      public function set info(param1:RoomInfo) : void
      {
         this._info = param1;
         this.update();
      }
      
      public function get id() : int
      {
         return this._info.ID;
      }
      
      private function update() : void
      {
         this._defaultMask.visible = this._info.mapId == 0 || this._info.mapId == 10000?Boolean(true):Boolean(false);
         this._lock.visible = this._info.IsLocked;
         this._nameText.text = this._info.Name;
         if(this._info.mapId == 0 || this._info.mapId == 10000)
         {
            this._hard.visible = false;
         }
         else
         {
            this._hard.visible = true;
            this._hard.text = "(" + LanguageMgr.GetTranslation(this._hardLevel[this._info.hardLevel]) + ")";
         }
         var _loc1_:String = this._info.maxViewerCnt == 0?"-":String(this._info.viewerCnt);
         this._placeCountText.text = String(this._info.totalPlayer) + "/" + String(this._info.placeCount) + " (" + _loc1_ + ")";
         if(this._info.isPlaying)
         {
            this._itemBg.setFrame(2);
            this._nameText.setFrame(2);
            this._hard.setFrame(2);
            this._placeCountText.setFrame(2);
         }
         else
         {
            this._itemBg.setFrame(1);
            this._nameText.setFrame(1);
            this._hard.setFrame(1);
            this._placeCountText.setFrame(1);
         }
         this.loadIcon();
      }
      
      private function loadIcon() : void
      {
         var _loc1_:int = this._info.mapId == 0?int(NAN_MAP):int(this._info.mapId);
         if(this._loader)
         {
            this._loader.removeEventListener(LoaderEvent.COMPLETE,this.__showMap);
         }
         this._loader = LoaderManager.Instance.creatLoader(PathManager.solveMapIconPath(_loc1_,1),BaseLoader.BITMAP_LOADER);
         this._loader.addEventListener(LoaderEvent.COMPLETE,this.__showMap);
         LoaderManager.Instance.startLoad(this._loader);
         if(this._simpMapLoader)
         {
            this._simpMapLoader.removeEventListener(LoaderEvent.COMPLETE,this.__showSimpMap);
         }
         this._simpMapLoader = LoaderManager.Instance.creatLoader(PathManager.solveMapIconPath(_loc1_,0),BaseLoader.BITMAP_LOADER);
         this._simpMapLoader.addEventListener(LoaderEvent.COMPLETE,this.__showSimpMap);
         LoaderManager.Instance.startLoad(this._simpMapLoader);
      }
      
      private function __showMap(param1:LoaderEvent) : void
      {
         if(param1.loader.isSuccess)
         {
            ObjectUtils.disposeAllChildren(this._mapShowContainer);
            param1.loader.removeEventListener(LoaderEvent.COMPLETE,this.__showMap);
            if(this._mapShow)
            {
               ObjectUtils.disposeObject(this._mapShow);
            }
            this._mapShow = null;
            this._mapShow = param1.loader.content as Bitmap;
            this._mapShow.scaleX = 69 / this._mapShow.height;
            this._mapShow.scaleY = 69 / this._mapShow.height;
            this._mapShow.smoothing = true;
            PositionUtils.setPos(this._mapShow,"dungeonList.MapShowPos");
            if(!this._mapShowContainer)
            {
               this._mapShowContainer = new Sprite();
            }
            this._mapShowContainer.addChild(this._mapShow);
            this._mapShowContainer.mask = this._mask;
         }
      }
      
      private function __showSimpMap(param1:LoaderEvent) : void
      {
         if(param1.loader.isSuccess)
         {
            param1.loader.removeEventListener(LoaderEvent.COMPLETE,this.__showSimpMap);
            if(this._simpMapShow)
            {
               ObjectUtils.disposeObject(this._simpMapShow);
               this._simpMapShow = null;
            }
            this._simpMapShow = param1.loader.content as Bitmap;
            PositionUtils.setPos(this._simpMapShow,"dungeonList.simpMapPos");
            addChild(this._simpMapShow);
         }
      }
      
      public function dispose() : void
      {
         if(this._loader)
         {
            this._loader.removeEventListener(LoaderEvent.COMPLETE,this.__showMap);
            this._loader = null;
         }
         if(this._simpMapLoader)
         {
            this._simpMapLoader.removeEventListener(LoaderEvent.COMPLETE,this.__showSimpMap);
            this._simpMapLoader = null;
         }
         if(this._simpMapShow)
         {
            ObjectUtils.disposeObject(this._simpMapShow);
            this._simpMapShow = null;
         }
         if(this._defaultMask)
         {
            ObjectUtils.disposeObject(this._defaultMask);
            this._defaultMask = null;
         }
         if(this._mapShowContainer && this._mapShowContainer.parent)
         {
            ObjectUtils.disposeAllChildren(this._mapShowContainer);
            this._mapShowContainer.parent.removeChild(this._mapShowContainer);
            this._mapShowContainer = null;
         }
         if(this._mask && this._mask.parent)
         {
            ObjectUtils.disposeAllChildren(this._mask);
            this._mask.parent.removeChild(this._mask);
            this._mask = null;
         }
         if(this._mapShow)
         {
            ObjectUtils.disposeObject(this._mapShow);
            this._mapShow = null;
         }
         if(this._hard)
         {
            ObjectUtils.disposeObject(this._hard);
            this._hard = null;
         }
         ObjectUtils.disposeObject(this._lock);
         this._lock = null;
         this._itemBg.dispose();
         this._nameText.dispose();
         this._placeCountText.dispose();
         this._itemBg = null;
         this._nameText = null;
         this._placeCountText = null;
         if(this.parent)
         {
            this.parent.removeChild(this);
         }
      }
   }
}
