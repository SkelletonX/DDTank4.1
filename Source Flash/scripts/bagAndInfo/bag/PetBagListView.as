package bagAndInfo.bag
{
   import bagAndInfo.cell.BagCell;
   import bagAndInfo.cell.BaseCell;
   import ddt.data.BagInfo;
   import ddt.data.goods.InventoryItemInfo;
   import ddt.events.BagEvent;
   import flash.events.Event;
   import flash.utils.Dictionary;
   import road7th.data.DictionaryData;
   
   public class PetBagListView extends BagListView
   {
      
      public static const PET_BAG_CAPABILITY:int = 49;
       
      
      private var _allBagData:BagInfo;
      
      public function PetBagListView(param1:int, param2:int = 7)
      {
         super(param1,param2,PET_BAG_CAPABILITY);
      }
      
      override public function setData(param1:BagInfo) : void
      {
         if(_bagdata == param1)
         {
            return;
         }
         if(_bagdata != null)
         {
            _bagdata.removeEventListener(BagEvent.UPDATE,this.__updateGoods);
         }
         _bagdata = param1;
         this._allBagData = param1;
         _bagdata.addEventListener(BagEvent.UPDATE,this.__updateGoods);
         this.sortItems();
      }
      
      private function sortItems() : void
      {
         var _loc1_:Array = null;
         var _loc2_:* = null;
         var _loc3_:InventoryItemInfo = null;
         _loc1_ = new Array();
         for(_loc2_ in _bagdata.items)
         {
            _loc3_ = _bagdata.items[_loc2_];
            if(_cells[_loc2_] != null && _loc3_)
            {
               if(_loc3_.CategoryID == BagInfo.FOOD || _loc3_.CategoryID == BagInfo.FOOD_OLD)
               {
                  BaseCell(_cells[_loc2_]).info = _loc3_;
                  _loc1_.push(_cells[_loc2_]);
               }
            }
         }
         this._cellsSort(_loc1_);
      }
      
      override protected function __updateGoods(param1:BagEvent) : void
      {
         var _loc3_:InventoryItemInfo = null;
         var _loc4_:InventoryItemInfo = null;
         if(!_bagdata)
         {
            return;
         }
         var _loc2_:Dictionary = param1.changedSlots;
         for each(_loc3_ in _loc2_)
         {
            _loc4_ = _bagdata.getItemAt(_loc3_.Place);
            if(_loc4_ && _loc4_.CategoryID == BagInfo.FOOD)
            {
               setCellInfo(_loc3_.Place,_loc4_);
            }
            else
            {
               setCellInfo(_loc3_.Place,null);
            }
         }
         this.sortItems();
         dispatchEvent(new Event(Event.CHANGE));
      }
      
      private function updateFoodBagList() : void
      {
         var _loc5_:InventoryItemInfo = null;
         var _loc1_:BagInfo = new BagInfo(BagInfo.PROPBAG,PET_BAG_CAPABILITY);
         var _loc2_:DictionaryData = new DictionaryData();
         var _loc3_:int = 0;
         var _loc4_:int = 0;
         while(_loc4_ < PET_BAG_CAPABILITY)
         {
            _loc5_ = this._allBagData.items[_loc4_.toString()];
            if(_cells[_loc4_] != null)
            {
               if(_loc5_ && _loc5_.CategoryID == BagInfo.FOOD)
               {
                  _loc5_.isMoveSpace = false;
                  _cells[_loc3_].info = _loc5_;
                  _loc2_.add(_loc3_,_loc5_);
                  _loc3_++;
               }
            }
            _loc4_++;
         }
         _loc1_.items = _loc2_;
         _bagdata = _loc1_;
      }
      
      private function getItemIndex(param1:InventoryItemInfo) : int
      {
         var _loc3_:* = null;
         var _loc4_:InventoryItemInfo = null;
         var _loc2_:int = -1;
         for(_loc3_ in _bagdata.items)
         {
            _loc4_ = _bagdata.items[_loc3_] as InventoryItemInfo;
            if(param1.Place == _loc4_.Place)
            {
               _loc2_ = int(_loc3_);
               break;
            }
         }
         return _loc2_;
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
            _loc5_ = _cellVec.indexOf(param1[_loc2_]);
            _loc6_ = _cellVec[_loc2_];
            param1[_loc2_].x = _loc6_.x;
            param1[_loc2_].y = _loc6_.y;
            _loc6_.x = _loc3_;
            _loc6_.y = _loc4_;
            _cellVec[_loc2_] = param1[_loc2_];
            _cellVec[_loc5_] = _loc6_;
            _loc2_++;
         }
      }
   }
}
