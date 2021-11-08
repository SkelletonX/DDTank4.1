package email.view
{
   import com.pickgliss.ui.controls.container.VBox;
   import ddt.manager.LanguageMgr;
   import email.data.EmailInfo;
   import flash.events.Event;
   
   public class EmailListView extends VBox
   {
       
      
      private var _strips:Array;
      
      public function EmailListView()
      {
         super();
         this._strips = new Array();
      }
      
      private function addEvent() : void
      {
      }
      
      private function removeEvent() : void
      {
      }
      
      public function update(param1:Array, param2:Boolean = false) : void
      {
         var _loc4_:EmailStrip = null;
         this.clearElements();
         var _loc3_:uint = 0;
         while(_loc3_ < param1.length)
         {
            if(param2)
            {
               _loc4_ = new EmailStripSended();
            }
            else
            {
               _loc4_ = new EmailStrip();
            }
            _loc4_.addEventListener(EmailStrip.SELECT,this.__select);
            _loc4_.info = param1[_loc3_] as EmailInfo;
            if(_loc4_.info.Title == LanguageMgr.GetTranslation("tank.auctionHouse.view.AuctionCellView.Object") && _loc4_.info.Type == 9)
            {
               if(_loc4_.info.Annex1)
               {
                  _loc4_.info.Annex1.ValidDate = -1;
               }
            }
            addChild(_loc4_);
            this._strips.push(_loc4_);
            _loc3_++;
         }
         refreshChildPos();
      }
      
      public function switchSeleted() : void
      {
         if(this.allHasSelected())
         {
            this.changeAll(false);
            return;
         }
         this.changeAll(true);
      }
      
      private function allHasSelected() : Boolean
      {
         var _loc1_:uint = 0;
         while(_loc1_ < this._strips.length)
         {
            if(!(EmailStrip(this._strips[_loc1_]).info.Type == 66 || EmailStrip(this._strips[_loc1_]).info.Type == 65 || EmailStrip(this._strips[_loc1_]).info.Type == 66))
            {
               if(!EmailStrip(this._strips[_loc1_]).selected)
               {
                  return false;
               }
            }
            _loc1_++;
         }
         return true;
      }
      
      private function changeAll(param1:Boolean) : void
      {
         var _loc2_:uint = 0;
         while(_loc2_ < this._strips.length)
         {
            if(EmailStrip(this._strips[_loc2_]).delecte)
            {
               EmailStrip(this._strips[_loc2_]).selected = param1;
            }
            _loc2_++;
         }
      }
      
      public function getSelectedMails() : Array
      {
         var _loc1_:Array = [];
         var _loc2_:uint = 0;
         while(_loc2_ < this._strips.length)
         {
            if(EmailStrip(this._strips[_loc2_]).selected)
            {
               _loc1_.push(EmailStrip(this._strips[_loc2_]).info);
            }
            _loc2_++;
         }
         return _loc1_;
      }
      
      public function updateInfo(param1:EmailInfo) : void
      {
         var _loc2_:EmailStrip = null;
         if(param1 == null)
         {
            return;
         }
         for each(_loc2_ in this._strips)
         {
            if(param1 == _loc2_.info)
            {
               _loc2_.info = param1;
               break;
            }
         }
      }
      
      private function clearElements() : void
      {
         var _loc1_:int = 0;
         while(_loc1_ < this._strips.length)
         {
            this._strips[_loc1_].removeEventListener(EmailStrip.SELECT,this.__select);
            this._strips[_loc1_].dispose();
            this._strips[_loc1_] = null;
            _loc1_++;
         }
         this._strips = new Array();
      }
      
      private function __select(param1:Event) : void
      {
         var _loc3_:EmailStrip = null;
         var _loc2_:EmailStrip = param1.target as EmailStrip;
         for each(_loc3_ in this._strips)
         {
            if(_loc3_ != _loc2_)
            {
               _loc3_.isReading = false;
            }
         }
      }
      
      function canChangePage() : Boolean
      {
         var _loc1_:EmailStrip = null;
         for each(_loc1_ in this._strips)
         {
            if(_loc1_.emptyItem)
            {
               return false;
            }
         }
         return true;
      }
   }
}
