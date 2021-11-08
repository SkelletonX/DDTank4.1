package bagAndInfo.bag
{
   import bagAndInfo.cell.BagCell;
   import bagAndInfo.cell.BaseCell;
   import bagAndInfo.cell.CellFactory;
   import com.pickgliss.events.InteractiveEvent;
   import com.pickgliss.ui.ComponentFactory;
   import com.pickgliss.ui.controls.container.SimpleTileList;
   import com.pickgliss.utils.DoubleClickManager;
   import ddt.data.BagInfo;
   import ddt.data.goods.InventoryItemInfo;
   import ddt.events.BagEvent;
   import ddt.events.CellEvent;
   import ddt.manager.SoundManager;
   import flash.display.Bitmap;
   import flash.events.Event;
   import flash.events.MouseEvent;
   import flash.utils.Dictionary;
   
   public class BagListView extends SimpleTileList
   {
      
      public static const BAG_CAPABILITY:int = 49;
       
      
      protected var _bagdata:BagInfo;
      
      protected var _cellNum:int;
      
      protected var _bagType:int;
      
      protected var _cells:Dictionary;
      
      protected var _cellMouseOverBg:Bitmap;
      
      protected var _cellVec:Array;
      
      private var _isSetFoodData:Boolean;
      
      private var _currentBagType:int;
      
      public function BagListView(param1:int, param2:int = 7, param3:int = 49)
      {
         this._cellNum = param3;
         this._bagType = param1;
         super(param2);
         _hSpace = _vSpace = 0;
         this._cellVec = new Array();
         this.createCells();
      }
      
      protected function createCells() : void
      {
         var _loc2_:BagCell = null;
         this._cells = new Dictionary();
         this._cellMouseOverBg = ComponentFactory.Instance.creatBitmap("bagAndInfo.cell.bagCellOverBgAsset");
         var _loc1_:int = 0;
         while(_loc1_ < 49)
         {
            _loc2_ = BagCell(CellFactory.instance.createBagCell(_loc1_));
            _loc2_.mouseOverEffBoolean = false;
            addChild(_loc2_);
            _loc2_.bagType = this._bagType;
            _loc2_.addEventListener(InteractiveEvent.CLICK,this.__clickHandler);
            _loc2_.addEventListener(MouseEvent.MOUSE_OVER,this._cellOverEff);
            _loc2_.addEventListener(MouseEvent.MOUSE_OUT,this._cellOutEff);
            _loc2_.addEventListener(InteractiveEvent.DOUBLE_CLICK,this.__doubleClickHandler);
            DoubleClickManager.Instance.enableDoubleClick(_loc2_);
            _loc2_.addEventListener(CellEvent.LOCK_CHANGED,this.__cellChanged);
            this._cells[_loc2_.place] = _loc2_;
            this._cellVec.push(_loc2_);
            _loc1_++;
         }
      }
      
      protected function __doubleClickHandler(param1:InteractiveEvent) : void
      {
         if((param1.currentTarget as BagCell).info != null)
         {
            SoundManager.instance.play("008");
            dispatchEvent(new CellEvent(CellEvent.DOUBLE_CLICK,param1.currentTarget));
         }
      }
      
      protected function __cellChanged(param1:Event) : void
      {
         dispatchEvent(new Event(Event.CHANGE));
      }
      
      protected function __clickHandler(param1:InteractiveEvent) : void
      {
         if((param1.currentTarget as BagCell).info != null)
         {
            dispatchEvent(new CellEvent(CellEvent.ITEM_CLICK,param1.currentTarget,false,false,param1.ctrlKey));
         }
      }
      
      protected function _cellOverEff(param1:MouseEvent) : void
      {
         BagCell(param1.currentTarget).onParentMouseOver(this._cellMouseOverBg);
      }
      
      protected function _cellOutEff(param1:MouseEvent) : void
      {
         BagCell(param1.currentTarget).onParentMouseOut();
      }
      
      public function setCellInfo(param1:int, param2:InventoryItemInfo) : void
      {
         if(param2 == null)
         {
            this._cells[String(param1)].info = null;
            return;
         }
         if(param2.Count == 0)
         {
            this._cells[String(param1)].info = null;
         }
         else
         {
            this._cells[String(param1)].info = param2;
         }
      }
      
      protected function clearDataCells() : void
      {
         var _loc1_:BagCell = null;
         for each(_loc1_ in this._cells)
         {
            _loc1_.info = null;
         }
      }
      
      public function set currentBagType(param1:int) : void
      {
         this._currentBagType = param1;
      }
      
      public function setData(param1:BagInfo) : void
      {
         var _loc3_:* = null;
         this._isSetFoodData = false;
         if(this._bagdata == param1)
         {
            return;
         }
         if(this._bagdata != null)
         {
            this._bagdata.removeEventListener(BagEvent.UPDATE,this.__updateGoods);
         }
         this.clearDataCells();
         this._bagdata = param1;
         var _loc2_:Array = new Array();
         for(_loc3_ in this._bagdata.items)
         {
            if(this._cells[_loc3_] != null)
            {
               if(this._currentBagType == BagView.PET)
               {
                  if(this._bagdata.items[_loc3_].CategoryID == 50 || this._bagdata.items[_loc3_].CategoryID == 51 || this._bagdata.items[_loc3_].CategoryID == 52)
                  {
                     this._bagdata.items[_loc3_].isMoveSpace = true;
                     this._cells[_loc3_].info = this._bagdata.items[_loc3_];
                     _loc2_.push(this._cells[_loc3_]);
                  }
               }
               else
               {
                  this._bagdata.items[_loc3_].isMoveSpace = true;
                  this._cells[_loc3_].info = this._bagdata.items[_loc3_];
               }
            }
         }
         this._bagdata.addEventListener(BagEvent.UPDATE,this.__updateGoods);
         if(this._currentBagType == BagView.PET)
         {
            this._cellsSort(_loc2_);
         }
      }
      
      private function sortItems() : void
      {
         var _loc2_:* = null;
         var _loc3_:InventoryItemInfo = null;
         var _loc1_:Array = new Array();
         for(_loc2_ in this._bagdata.items)
         {
            _loc3_ = this._bagdata.items[_loc2_];
            if(this._cells[_loc2_] != null && _loc3_)
            {
               if(_loc3_.CategoryID == 50 || _loc3_.CategoryID == 51 || _loc3_.CategoryID == 52)
               {
                  BaseCell(this._cells[_loc2_]).info = _loc3_;
                  _loc1_.push(this._cells[_loc2_]);
               }
            }
         }
         this._cellsSort(_loc1_);
      }
      
      private function _cellsSort(param1:Array) : void
      {
         var _loc2_:int = 0;
         var _loc3_:Number = NaN;
         var _loc4_:Number = NaN;
         var _loc5_:int = 0;
         var _loc6_:BagCell = null;
         if(param1.length <= 0)
         {
            return;
         }
         _loc2_ = 0;
         while(_loc2_ < param1.length)
         {
            _loc3_ = param1[_loc2_].x;
            _loc4_ = param1[_loc2_].y;
            _loc5_ = this._cellVec.indexOf(param1[_loc2_]);
            _loc6_ = this._cellVec[_loc2_];
            param1[_loc2_].x = _loc6_.x;
            param1[_loc2_].y = _loc6_.y;
            _loc6_.x = _loc3_;
            _loc6_.y = _loc4_;
            this._cellVec[_loc2_] = param1[_loc2_];
            this._cellVec[_loc5_] = _loc6_;
            _loc2_++;
         }
      }
      
      protected function __updateFoodGoods(param1:BagEvent) : void
      {
         var _loc3_:InventoryItemInfo = null;
         var _loc4_:int = 0;
         var _loc5_:InventoryItemInfo = null;
         var _loc6_:* = null;
         var _loc7_:InventoryItemInfo = null;
         var _loc8_:InventoryItemInfo = null;
         if(!this._bagdata)
         {
            return;
         }
         var _loc2_:Dictionary = param1.changedSlots;
         for each(_loc3_ in _loc2_)
         {
            _loc4_ = -1;
            _loc5_ = null;
            for(_loc6_ in this._bagdata.items)
            {
               _loc7_ = this._bagdata.items[_loc6_] as InventoryItemInfo;
               if(_loc3_.ItemID == _loc7_.ItemID)
               {
                  _loc5_ = _loc3_;
                  _loc4_ = int(_loc6_);
                  break;
               }
            }
            if(_loc4_ != -1)
            {
               _loc8_ = this._bagdata.getItemAt(_loc4_);
               if(_loc8_)
               {
                  _loc8_.Count = _loc5_.Count;
                  if(this._cells[String(_loc4_)].info)
                  {
                     this.setCellInfo(_loc4_,null);
                  }
                  else
                  {
                     this.setCellInfo(_loc4_,_loc8_);
                  }
               }
               else
               {
                  this.setCellInfo(_loc4_,null);
               }
               dispatchEvent(new Event(Event.CHANGE));
            }
         }
      }
      
      protected function __updateGoods(param1:BagEvent) : void
      {
         var _loc2_:Dictionary = null;
         var _loc3_:InventoryItemInfo = null;
         var _loc4_:InventoryItemInfo = null;
         if(this._isSetFoodData)
         {
            this.__updateFoodGoods(param1);
         }
         else
         {
            _loc2_ = param1.changedSlots;
            for each(_loc3_ in _loc2_)
            {
               _loc4_ = this._bagdata.getItemAt(_loc3_.Place);
               if(_loc4_)
               {
                  if(this._currentBagType == BagView.PET)
                  {
                     if(_loc4_.CategoryID != 50 && _loc4_.CategoryID != 51 && _loc4_.CategoryID != 52)
                     {
                        this.setCellInfo(_loc3_.Place,null);
                        continue;
                     }
                  }
                  this.setCellInfo(_loc4_.Place,_loc4_);
               }
               else
               {
                  this.setCellInfo(_loc3_.Place,null);
               }
               dispatchEvent(new Event(Event.CHANGE));
            }
         }
         if(this._currentBagType == BagView.PET)
         {
            this.sortItems();
         }
      }
      
      override public function dispose() : void
      {
         var _loc1_:BagCell = null;
         if(this._bagdata != null)
         {
            this._bagdata.removeEventListener(BagEvent.UPDATE,this.__updateGoods);
            this._bagdata = null;
         }
         for each(_loc1_ in this._cells)
         {
            _loc1_.removeEventListener(InteractiveEvent.CLICK,this.__clickHandler);
            _loc1_.removeEventListener(CellEvent.LOCK_CHANGED,this.__cellChanged);
            _loc1_.removeEventListener(MouseEvent.MOUSE_OVER,this._cellOverEff);
            _loc1_.removeEventListener(MouseEvent.MOUSE_OUT,this._cellOutEff);
            _loc1_.removeEventListener(InteractiveEvent.DOUBLE_CLICK,this.__doubleClickHandler);
            DoubleClickManager.Instance.disableDoubleClick(_loc1_);
            _loc1_.dispose();
         }
         this._cells = null;
         this._cellVec = null;
         if(this._cellMouseOverBg)
         {
            if(this._cellMouseOverBg.parent)
            {
               this._cellMouseOverBg.parent.removeChild(this._cellMouseOverBg);
            }
            this._cellMouseOverBg.bitmapData.dispose();
         }
         this._cellMouseOverBg = null;
         super.dispose();
      }
      
      public function get cells() : Dictionary
      {
         return this._cells;
      }
   }
}
