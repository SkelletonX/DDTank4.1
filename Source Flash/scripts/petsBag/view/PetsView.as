package petsBag.view
{
   import bagAndInfo.bag.BagView;
   import com.pickgliss.events.FrameEvent;
   import com.pickgliss.ui.ComponentFactory;
   import com.pickgliss.ui.LayerManager;
   import com.pickgliss.ui.controls.Frame;
   import com.pickgliss.utils.ObjectUtils;
   import ddt.data.EquipType;
   import ddt.data.goods.ItemTemplateInfo;
   import ddt.events.CellEvent;
   import ddt.manager.LanguageMgr;
   import ddt.manager.PlayerManager;
   import ddt.manager.SoundManager;
   import ddt.utils.PositionUtils;
   import flash.display.DisplayObject;
   import flash.events.Event;
   import flash.events.MouseEvent;
   import petsBag.PetsBagManager;
   import petsBag.controller.PetBagController;
   import petsBag.data.PetFarmGuildeTaskType;
   import store.HelpFrame;
   import trainer.data.ArrowType;
   
   public class PetsView extends Frame
   {
       
      
      private var _petsBagOutView:PetsBagOutView;
      
      private var _bagView:BagView;
      
      public function PetsView()
      {
         super();
         this.initView();
         this.initEvent();
      }
      
      private function initView() : void
      {
         titleText = LanguageMgr.GetTranslation("petBag.view.titleText");
         this._petsBagOutView = ComponentFactory.Instance.creatCustomObject("petsBagOutPnl");
         addToContent(this._petsBagOutView);
         this._petsBagOutView.infoPlayer = PlayerManager.Instance.Self;
         PetBagController.instance().view = this._petsBagOutView;
         this._bagView = new BagView(true);
         this._bagView.sortBagEnable = false;
         this._bagView.breakBtnEnable = false;
         this._bagView.sortBagFilter = ComponentFactory.Instance.creatFilters("grayFilter");
         this._bagView.breakBtnFilter = ComponentFactory.Instance.creatFilters("grayFilter");
         this._bagView.isScreenFood = true;
         this._bagView.setBagType(BagView.PET);
         this._bagView.info = PlayerManager.Instance.Self;
         this._bagView.enableOrdisableSB(true);
         this._bagView.setBtnY();
         this._bagView.enableDressSelectedBtn(false);
         this._bagView.deleteButtonForPet();
         PlayerManager.Instance.Self.PropBag.sortBag(5,PlayerManager.Instance.Self.getBag(5),0,48,false);
         PositionUtils.setPos(this._bagView,"petsBagView.bagView.pos");
         addToContent(this._bagView);
      }
      
      private function initEvent() : void
      {
         addEventListener(FrameEvent.RESPONSE,this.__frameEventHandler);
         this._bagView.addEventListener(CellEvent.DRAGSTART,this.__startShine);
         this._bagView.addEventListener(CellEvent.DRAGSTOP,this.__stopShine);
         this._bagView.addEventListener(BagView.TABCHANGE,this.__changeHandler);
         PetsBagManager.instance.addEventListener("equitClick",this._clickEquit);
      }
      
      private function _clickEquit(param1:Event) : void
      {
         this._bagView.setBagType(BagView.EQUIP);
      }
      
      private function __startShine(param1:CellEvent) : void
      {
         if(param1.data is ItemTemplateInfo)
         {
            if((param1.data as ItemTemplateInfo).CategoryID == EquipType.FOOD)
            {
               if(this._petsBagOutView)
               {
                  this._petsBagOutView.startShine();
               }
            }
            else if((param1.data as ItemTemplateInfo).CategoryID == EquipType.PET_EQUIP_ARM)
            {
               if(this._petsBagOutView)
               {
                  this._petsBagOutView.playShined(0);
               }
            }
            else if((param1.data as ItemTemplateInfo).CategoryID == EquipType.PET_EQUIP_CLOTH)
            {
               if(this._petsBagOutView)
               {
                  this._petsBagOutView.playShined(2);
               }
            }
            else if((param1.data as ItemTemplateInfo).CategoryID == EquipType.PET_EQUIP_HEAD)
            {
               if(this._petsBagOutView)
               {
                  this._petsBagOutView.playShined(1);
               }
            }
         }
      }
      
      private function __stopShine(param1:CellEvent) : void
      {
         if(this._petsBagOutView)
         {
            this._petsBagOutView.stopShine();
         }
         if(this._petsBagOutView)
         {
            this._petsBagOutView.stopShined(0);
         }
         if(this._petsBagOutView)
         {
            this._petsBagOutView.stopShined(1);
         }
         if(this._petsBagOutView)
         {
            this._petsBagOutView.stopShined(2);
         }
      }
      
      private function __changeHandler(param1:Event) : void
      {
      }
      
      protected function __onPetsHelp(param1:MouseEvent) : void
      {
         SoundManager.instance.play("008");
         var _loc2_:DisplayObject = ComponentFactory.Instance.creat("petsBag.HelpPrompt");
         var _loc3_:HelpFrame = ComponentFactory.Instance.creat("petsBag.HelpFrame");
         _loc3_.setView(_loc2_);
         _loc3_.titleText = LanguageMgr.GetTranslation("ddt.petsBag.readme");
         _loc3_.setButtonPos(158,446);
         LayerManager.Instance.addToLayer(_loc3_,LayerManager.STAGE_DYANMIC_LAYER,true,LayerManager.BLCAK_BLOCKGOUND);
      }
      
      public function show() : void
      {
         LayerManager.Instance.addToLayer(this,LayerManager.GAME_DYNAMIC_LAYER,true,LayerManager.ALPHA_BLOCKGOUND);
      }
      
      private function removeEvent() : void
      {
         removeEventListener(FrameEvent.RESPONSE,this.__frameEventHandler);
         this._bagView.removeEventListener(CellEvent.DRAGSTART,this.__startShine);
         this._bagView.removeEventListener(CellEvent.DRAGSTOP,this.__stopShine);
         this._bagView.removeEventListener(BagView.TABCHANGE,this.__changeHandler);
      }
      
      private function __frameEventHandler(param1:FrameEvent) : void
      {
         SoundManager.instance.play("008");
         switch(param1.responseCode)
         {
            case FrameEvent.ESC_CLICK:
            case FrameEvent.CLOSE_CLICK:
               PetsBagManager.instance.hide();
               if(!PetBagController.instance().haveTaskOrderByID(PetFarmGuildeTaskType.PET_TASK2))
               {
                  PetBagController.instance().clearCurrentPetFarmGuildeArrow(ArrowType.OPEN_PET_BAG);
               }
         }
      }
      
      override public function dispose() : void
      {
         this.removeEvent();
         if(this._petsBagOutView)
         {
            ObjectUtils.disposeObject(this._petsBagOutView);
         }
         this._petsBagOutView = null;
         PetBagController.instance().view = null;
         if(this._bagView)
         {
            this._bagView.dispose();
            this._bagView = null;
         }
         if(this.parent)
         {
            this.parent.removeChild(this);
         }
         super.dispose();
      }
   }
}
