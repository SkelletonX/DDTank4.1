package email.analyze
{
   import com.pickgliss.loader.DataAnalyzer;
   import com.pickgliss.utils.ObjectUtils;
   import ddt.data.goods.InventoryItemInfo;
   import ddt.manager.ItemManager;
   import ddt.manager.PlayerManager;
   import ddt.manager.SharedManager;
   import email.data.EmailInfo;
   import flash.utils.describeType;
   
   public class AllEmailAnalyzer extends DataAnalyzer
   {
       
      
      private var _list:Array;
      
      public function AllEmailAnalyzer(param1:Function)
      {
         super(param1);
      }
      
      override public function analyze(param1:*) : void
      {
         var _loc3_:XMLList = null;
         var _loc4_:XML = null;
         var _loc5_:XML = null;
         var _loc6_:int = 0;
         var _loc7_:EmailInfo = null;
         var _loc8_:XMLList = null;
         var _loc9_:int = 0;
         var _loc10_:InventoryItemInfo = null;
         var _loc11_:InventoryItemInfo = null;
         this._list = new Array();
         var _loc2_:XML = new XML(param1);
         if(_loc2_.@value == "true")
         {
            _loc3_ = _loc2_.Item;
            _loc4_ = describeType(new EmailInfo());
            _loc5_ = describeType(new InventoryItemInfo());
            _loc6_ = 0;
            while(_loc6_ < _loc3_.length())
            {
               _loc7_ = new EmailInfo();
               ObjectUtils.copyPorpertiesByXML(_loc7_,_loc3_[_loc6_]);
               _loc8_ = _loc3_[_loc6_].Item;
               _loc9_ = 0;
               while(_loc9_ < _loc8_.length())
               {
                  _loc10_ = new InventoryItemInfo();
                  ObjectUtils.copyPorpertiesByXML(_loc10_,_loc8_[_loc9_]);
                  _loc10_.isGold = _loc8_[_loc9_].@IsGold == "true"?Boolean(true):Boolean(false);
                  _loc10_.goldBeginTime = String(_loc8_[_loc9_].@GoldBeginTime);
                  _loc10_.goldValidDate = int(_loc8_[_loc9_].@GoldVaild);
                  _loc11_ = ItemManager.fill(_loc10_);
                  _loc7_["Annex" + this.getAnnexPos(_loc7_,_loc10_)] = _loc10_;
                  _loc7_.UserID = _loc11_.UserID;
                  _loc10_.IsVisleBound = false;
                  _loc9_++;
               }
               if(!SharedManager.Instance.deleteMail[PlayerManager.Instance.Self.ID] || SharedManager.Instance.deleteMail[PlayerManager.Instance.Self.ID].indexOf(_loc7_.ID) < 0)
               {
                  this._list.push(_loc7_);
               }
               _loc6_++;
            }
            onAnalyzeComplete();
         }
         else
         {
            message = _loc2_.@message;
            onAnalyzeError();
            onAnalyzeComplete();
         }
      }
      
      public function get list() : Array
      {
         this._list.reverse();
         return this._list;
      }
      
      private function getAnnexPos(param1:EmailInfo, param2:InventoryItemInfo) : int
      {
         var _loc3_:uint = 1;
         while(_loc3_ <= 5)
         {
            if(param1["Annex" + _loc3_ + "ID"] == param2.ItemID)
            {
               return _loc3_;
            }
            _loc3_++;
         }
         return 1;
      }
   }
}
