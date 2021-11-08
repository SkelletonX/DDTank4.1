package email.view
{
   import baglocked.BaglockedManager;
   import com.pickgliss.events.FrameEvent;
   import com.pickgliss.ui.AlertManager;
   import com.pickgliss.ui.LayerManager;
   import com.pickgliss.ui.controls.alert.BaseAlerFrame;
   import com.pickgliss.utils.ObjectUtils;
   import ddt.data.goods.InventoryItemInfo;
   import ddt.manager.LanguageMgr;
   import ddt.manager.LeavePageManager;
   import ddt.manager.PlayerManager;
   import ddt.manager.SoundManager;
   import email.manager.MailManager;
   import flash.events.MouseEvent;
   
   public class DiamondOfReading extends DiamondBase
   {
       
      
      private var type:int;
      
      private var payAlertFrame:BaseAlerFrame;
      
      public function DiamondOfReading()
      {
         super();
      }
      
      public function set readOnly(param1:Boolean) : void
      {
         if(param1)
         {
            this.removeEvent();
         }
         else
         {
            this.addEvent();
         }
      }
      
      override protected function addEvent() : void
      {
         addEventListener(MouseEvent.CLICK,this.__onClick);
      }
      
      override protected function removeEvent() : void
      {
         removeEventListener(MouseEvent.CLICK,this.__onClick);
      }
      
      override protected function update() : void
      {
         var _loc1_:* = undefined;
         _loc1_ = _info.getAnnexByIndex(index);
         chargedImg.visible = false;
         if(_loc1_ && _loc1_ is String)
         {
            this.buttonMode = true;
            _cell.visible = false;
            centerMC.visible = true;
            countTxt.text = "";
            mouseEnabled = true;
            mouseChildren = true;
            if(_loc1_ == "gold")
            {
               centerMC.setFrame(3);
               countTxt.text = String(_info.Gold);
               mouseChildren = false;
            }
            else if(_loc1_ == "money")
            {
               if(_info.Type > 100)
               {
                  centerMC.visible = false;
                  mouseEnabled = false;
                  mouseChildren = false;
               }
               else
               {
                  centerMC.setFrame(2);
                  countTxt.text = String(_info.Money);
                  mouseChildren = false;
               }
            }
            else if(_loc1_ == "gift")
            {
               centerMC.setFrame(6);
               countTxt.text = String(_info.GiftToken);
               mouseChildren = false;
            }
            else if(_loc1_ == "medal")
            {
               centerMC.setFrame(7);
               countTxt.text = String(_info.Medal);
               mouseChildren = false;
            }
         }
         else if(_loc1_)
         {
            _cell.visible = true;
            _cell.info = _loc1_ as InventoryItemInfo;
            mouseEnabled = true;
            mouseChildren = true;
            countTxt.text = "";
            if(_info.Type > 100 && _info.Money > 0)
            {
               centerMC.visible = false;
               chargedImg.visible = true;
            }
            else
            {
               centerMC.visible = false;
            }
         }
         else
         {
            mouseEnabled = false;
            mouseChildren = false;
            centerMC.visible = false;
            _cell.visible = false;
            countTxt.text = "";
         }
      }
      
      private function __onClick(param1:MouseEvent) : void
      {
         this.__distill();
      }
      
      private function _responseI(param1:FrameEvent) : void
      {
         this._checkResponse(param1.responseCode,this.__distill);
         var _loc2_:BaseAlerFrame = BaseAlerFrame(param1.currentTarget);
         _loc2_.removeEventListener(FrameEvent.RESPONSE,this._responseI);
         ObjectUtils.disposeObject(param1.target);
      }
      
      private function _checkResponse(param1:int, param2:Function = null) : void
      {
         SoundManager.instance.play("008");
         switch(param1)
         {
            case FrameEvent.SUBMIT_CLICK:
            case FrameEvent.ENTER_CLICK:
               param2();
         }
      }
      
      private function __distill() : void
      {
         var _loc2_:uint = 0;
         SoundManager.instance.play("008");
         if(PlayerManager.Instance.Self.bagLocked)
         {
            BaglockedManager.Instance.show();
            return;
         }
         mouseEnabled = false;
         mouseChildren = false;
         if(_info == null)
         {
            return;
         }
         var _loc1_:* = _info.getAnnexByIndex(index);
         if(_loc1_)
         {
            _loc2_ = 1;
            while(_loc2_ <= 5)
            {
               if(_loc1_ == _info["Annex" + _loc2_])
               {
                  this.type = _loc2_;
                  break;
               }
               _loc2_++;
            }
            if(_loc1_ == "gold")
            {
               this.type = 6;
            }
            else if(_loc1_ == "money")
            {
               this.type = 7;
            }
            else if(_loc1_ == "gift")
            {
               this.type = 8;
            }
            else if(_loc1_ == "medal")
            {
               this.type = 9;
            }
         }
         if(this.type > -1)
         {
            if(_info.Type > 100 && (this.type >= 1 && this.type <= 5) && _info.Money > 0)
            {
               this.payAlertFrame = AlertManager.Instance.simpleAlert(LanguageMgr.GetTranslation("tank.view.emailII.EmailIIDiamondView.emailTip"),LanguageMgr.GetTranslation("tank.view.emailII.EmailIIDiamondView.deleteTip") + " " + _info.Money + LanguageMgr.GetTranslation("tank.view.emailII.EmailIIDiamondView.money"),LanguageMgr.GetTranslation("ok"),LanguageMgr.GetTranslation("cancel"),false,false,false,LayerManager.ALPHA_BLOCKGOUND);
               this.payAlertFrame.addEventListener(FrameEvent.RESPONSE,this.__payFrameResponse);
               return;
            }
            MailManager.Instance.getAnnexToBag(_info,this.type);
         }
      }
      
      private function __payFrameResponse(param1:FrameEvent) : void
      {
         SoundManager.instance.play("008");
         this.payAlertFrame.removeEventListener(FrameEvent.RESPONSE,this.__payFrameResponse);
         this.payAlertFrame.dispose();
         this.payAlertFrame = null;
         if(param1.responseCode == FrameEvent.SUBMIT_CLICK)
         {
            this.confirmPay();
         }
         else if(param1.responseCode == FrameEvent.CANCEL_CLICK || param1.responseCode == FrameEvent.CLOSE_CLICK || param1.responseCode == FrameEvent.ESC_CLICK)
         {
            this.canclePay();
         }
      }
      
      private function confirmPay() : void
      {
         var _loc1_:BaseAlerFrame = null;
         if(PlayerManager.Instance.Self.Money >= _info.Money)
         {
            MailManager.Instance.getAnnexToBag(_info,this.type);
            mouseEnabled = false;
            mouseChildren = false;
         }
         else
         {
            _loc1_ = AlertManager.Instance.simpleAlert(LanguageMgr.GetTranslation("AlertDialog.Info"),LanguageMgr.GetTranslation("tank.view.comon.lack"),LanguageMgr.GetTranslation("ok"),LanguageMgr.GetTranslation("cancel"),false,false,false,LayerManager.ALPHA_BLOCKGOUND);
            _loc1_.addEventListener(FrameEvent.RESPONSE,this.__confirmResponse);
            mouseEnabled = true;
            mouseChildren = true;
         }
      }
      
      private function __confirmResponse(param1:FrameEvent) : void
      {
         SoundManager.instance.play("008");
         var _loc2_:BaseAlerFrame = param1.currentTarget as BaseAlerFrame;
         _loc2_.removeEventListener(FrameEvent.RESPONSE,this.__confirmResponse);
         ObjectUtils.disposeObject(_loc2_);
         if(_loc2_.parent)
         {
            _loc2_.parent.removeChild(_loc2_);
         }
         if(param1.responseCode == FrameEvent.SUBMIT_CLICK || param1.responseCode == FrameEvent.ENTER_CLICK)
         {
            LeavePageManager.leaveToFillPath();
         }
      }
      
      private function canclePay() : void
      {
         mouseEnabled = true;
         mouseChildren = true;
      }
      
      override public function dispose() : void
      {
         if(this.payAlertFrame)
         {
            this.payAlertFrame.removeEventListener(FrameEvent.RESPONSE,this.__payFrameResponse);
            this.payAlertFrame.dispose();
         }
         this.payAlertFrame = null;
         super.dispose();
      }
   }
}
