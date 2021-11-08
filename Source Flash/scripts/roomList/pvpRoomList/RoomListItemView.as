package roomList.pvpRoomList
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
   import ddt.manager.PathManager;
   import ddt.utils.PositionUtils;
   import flash.display.Bitmap;
   import flash.display.Sprite;
   import flash.filters.ColorMatrixFilter;
   import flash.geom.Rectangle;
   import room.model.RoomInfo;
   
   public class RoomListItemView extends Sprite implements Disposeable
   {
       
      
      private var _info:RoomInfo;
      
      private var _mode:ScaleFrameImage;
      
      private var _itemBg:ScaleFrameImage;
      
      private var _lock:Bitmap;
      
      private var _nameText:FilterFrameText;
      
      private var _placeCountText:FilterFrameText;
      
      private var _watchPlaceCountText:FilterFrameText;
      
      private var _mapShowLoader:DisplayLoader;
      
      private var _simpMapLoader:DisplayLoader;
      
      private var _mapShowContainer:Sprite;
      
      private var _simpMapShow:Bitmap;
      
      private var _mapShow:Bitmap;
      
      private var _mask:Sprite;
      
      private var _myMatrixFilter:ColorMatrixFilter;
      
      public function RoomListItemView(param1:RoomInfo)
      {
         this._myMatrixFilter = new ColorMatrixFilter([0.58516,0.36563,0.0492,0,0,0.18516,0.76564,0.0492,0,0,0.18516,0.36563,0.4492,0,0,0,0,0,1,0]);
         this._info = param1;
         super();
         this.init();
      }
      
      private function init() : void
      {
         this.buttonMode = true;
         this._mapShowContainer = new Sprite();
         addChild(this._mapShowContainer);
         this._itemBg = ComponentFactory.Instance.creat("roomList.pvpRoomList.roomListItem");
         this._itemBg.setFrame(1);
         addChild(this._itemBg);
         this._mode = ComponentFactory.Instance.creat("roomList.pvpRoomList.mode");
         this._mode.setFrame(1);
         addChild(this._mode);
         this._nameText = ComponentFactory.Instance.creat("roomList.pvpRoomList.nameText");
         addChild(this._nameText);
         this._placeCountText = ComponentFactory.Instance.creat("roomList.pvpRoomList.placeCountText");
         addChild(this._placeCountText);
         this._lock = ComponentFactory.Instance.creatBitmap("asset.roomList.lock");
         addChild(this._lock);
         var _loc1_:Rectangle = ComponentFactory.Instance.creatCustomObject("roomList.maskRectangle");
         this._mask = new Sprite();
         this._mask.graphics.beginFill(0,0);
         this._mask.graphics.drawRoundRect(0,0,_loc1_.width,_loc1_.height,_loc1_.y);
         this._mask.graphics.endFill();
         PositionUtils.setPos(this._mask,"roomListItem.maskPos");
         addChild(this._mask);
         this._watchPlaceCountText = UICreatShortcut.creatAndAdd("roomList.pvpRoomList.watchPlaceCountText",this);
         this.upadte();
      }
      
      private function upadte() : void
      {
         if(this._info.isPlaying)
         {
            this._mode.filters = [this._myMatrixFilter];
            this._itemBg.setFrame(2);
            this._nameText.setFrame(2);
            this._placeCountText.setFrame(2);
            this._watchPlaceCountText.setFrame(2);
         }
         else
         {
            this._mode.filters = null;
            this._itemBg.setFrame(1);
            this._nameText.setFrame(1);
            this._placeCountText.setFrame(1);
            this._watchPlaceCountText.setFrame(1);
         }
         this._mode.setFrame(this._info.type + 1);
         this._nameText.text = this._info.Name;
         this._lock.visible = this._info.IsLocked;
         var _loc1_:String = this._info.maxViewerCnt == 0?"-":String(this._info.viewerCnt);
         this._placeCountText.text = String(this._info.totalPlayer) + "/" + String(this._info.placeCount);
         this._watchPlaceCountText.text = "(" + _loc1_ + ")";
         this.loadIcon();
      }
      
      private function loadIcon() : void
      {
         if(this._mapShowLoader)
         {
            this._mapShowLoader.removeEventListener(LoaderEvent.COMPLETE,this.__showMap);
         }
         this._mapShowLoader = LoaderManager.Instance.creatLoader(PathManager.solveMapIconPath(this._info.mapId,1),BaseLoader.BITMAP_LOADER);
         this._mapShowLoader.addEventListener(LoaderEvent.COMPLETE,this.__showMap);
         LoaderManager.Instance.startLoad(this._mapShowLoader);
         if(this._simpMapLoader)
         {
            this._simpMapLoader.removeEventListener(LoaderEvent.COMPLETE,this.__showSimpMap);
         }
         this._simpMapLoader = LoaderManager.Instance.creatLoader(PathManager.solveMapIconPath(this._info.mapId,0),BaseLoader.BITMAP_LOADER);
         this._simpMapLoader.addEventListener(LoaderEvent.COMPLETE,this.__showSimpMap);
         LoaderManager.Instance.startLoad(this._simpMapLoader);
      }
      
      private function __showMap(param1:LoaderEvent) : void
      {
         if(param1.loader.isSuccess)
         {
            ObjectUtils.disposeAllChildren(this._mapShowContainer);
            if(this._mapShow)
            {
               ObjectUtils.disposeObject(this._mapShow);
            }
            param1.loader.removeEventListener(LoaderEvent.COMPLETE,this.__showMap);
            this._mapShow = param1.loader.content as Bitmap;
            this._mapShow.scaleX = 69 / this._mapShow.height;
            this._mapShow.scaleY = 69 / this._mapShow.height;
            this._mapShow.smoothing = true;
            PositionUtils.setPos(this._mapShow,"roomList.MapShowPos");
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
            PositionUtils.setPos(this._simpMapShow,"roomList.simpMapPos");
            addChild(this._simpMapShow);
         }
      }
      
      public function get info() : RoomInfo
      {
         return this._info;
      }
      
      public function get id() : int
      {
         return this._info.ID;
      }
      
      public function dispose() : void
      {
         this._info = null;
         ObjectUtils.disposeObject(this._itemBg);
         this._itemBg = null;
         ObjectUtils.disposeObject(this._mode);
         this._mode = null;
         ObjectUtils.disposeObject(this._mapShow);
         this._mapShow = null;
         this._nameText.dispose();
         this._nameText = null;
         this._placeCountText.dispose();
         this._placeCountText = null;
         ObjectUtils.disposeObject(this._lock);
         ObjectUtils.disposeObject(this._simpMapShow);
         this._simpMapShow = null;
         this._lock = null;
         ObjectUtils.disposeObject(this._watchPlaceCountText);
         this._watchPlaceCountText = null;
         if(this._mapShowContainer)
         {
            if(this._mapShowContainer.parent)
            {
               this._mapShowContainer.parent.removeChild(this._mapShowContainer);
            }
            ObjectUtils.disposeAllChildren(this._mapShowContainer);
            this._mapShowContainer = null;
         }
         if(this._mapShowLoader != null)
         {
            this._mapShowLoader.removeEventListener(LoaderEvent.COMPLETE,this.__showMap);
            this._mapShowLoader = null;
         }
         if(this._simpMapLoader)
         {
            this._simpMapLoader.addEventListener(LoaderEvent.COMPLETE,this.__showSimpMap);
            this._simpMapLoader = null;
         }
         if(this._mask && this._mask.parent)
         {
            ObjectUtils.disposeAllChildren(this._mask);
            this._mask.parent.removeChild(this._mask);
            this._mask = null;
         }
         if(parent)
         {
            parent.removeChild(this);
         }
      }
   }
}
