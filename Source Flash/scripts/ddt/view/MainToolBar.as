package ddt.view
{
   import bagAndInfo.BagAndInfoManager;
   import bagAndInfo.bag.BagView;
   import com.pickgliss.effect.EffectManager;
   import com.pickgliss.effect.EffectTypes;
   import com.pickgliss.effect.IEffect;
   import com.pickgliss.events.FrameEvent;
   import com.pickgliss.ui.AlertManager;
   import com.pickgliss.ui.ComponentFactory;
   import com.pickgliss.ui.LayerManager;
   import com.pickgliss.ui.ShowTipManager;
   import com.pickgliss.ui.controls.BaseButton;
   import com.pickgliss.ui.controls.alert.BaseAlerFrame;
   import com.pickgliss.ui.controls.container.HBox;
   import com.pickgliss.utils.ObjectUtils;
   import ddt.manager.KeyboardShortcutsManager;
   import ddt.manager.LanguageMgr;
   import ddt.manager.LeavePageManager;
   import ddt.manager.MessageTipManager;
   import ddt.manager.PlayerManager;
   import ddt.manager.SocketManager;
   import ddt.manager.SoundManager;
   import ddt.manager.StateManager;
   import ddt.manager.TaskManager;
   import ddt.states.StateType;
   import email.manager.MailManager;
   import email.view.EmailEvent;
   import exitPrompt.ExitPromptManager;
   import flash.display.Bitmap;
   import flash.display.Sprite;
   import flash.events.Event;
   import flash.events.MouseEvent;
   import flash.geom.Point;
   import gotopage.view.GotoPageController;
   import im.IMController;
   import petsBag.PetsBagManager;
   import quest.QuestBubbleManager;
   import trainer.controller.WeakGuildManager;
   import trainer.data.ArrowType;
   import trainer.data.Step;
   import trainer.view.NewHandContainer;
   
   public class MainToolBar extends Sprite
   {
      
      public static const ENTER_HALL:int = 0;
      
      public static const LEAVE_HALL:int = 1;
      
      private static var _instance:MainToolBar;
       
      
      private var _toolBarBg:Bitmap;
      
      private var _bgGradient:Bitmap;
      
      private var _goSupplyBtn:BaseButton;
      
      private var _goShopBtn:BaseButton;
      
      private var _goBagBtn:BaseButton;
      
      private var _goEmailBtn:BaseButton;
      
      private var _goTaskBtn:BaseButton;
      
      private var _goFriendListBtn:BaseButton;
      
      private var _goSignBtn:BaseButton;
      
      private var _goChannelBtn:BaseButton;
      
      private var _goReturnBtn:BaseButton;
      
      private var _goExitBtn:BaseButton;
      
      private var _callBackFun:Function;
      
      private var _unReadEmail:Boolean;
      
      private var _unReadTask:Boolean;
      
      private var _enabled:Boolean;
      
      private var _unReadMovement:Boolean;
      
      private var _taskEffectEnable:Boolean;
      
      private var _signEffectEnable:Boolean = true;
      
      private var _boxContainer:HBox;
      
      private var allBtns:Array;
      
      private var _isEvent:Boolean;
      
      private var _emailShineEffect:IEffect;
      
      private var _taskShineEffect:IEffect;
      
      private var _friendShineEffec:IEffect;
      
      public function MainToolBar()
      {
         super();
         this.initView();
         this.initEvent();
      }
      
      public static function get Instance() : MainToolBar
      {
         if(_instance == null)
         {
            _instance = new MainToolBar();
         }
         return _instance;
      }
      
      private function initView() : void
      {
         this._toolBarBg = ComponentFactory.Instance.creatBitmap("asset.toolbar.toolBarBg");
         this._bgGradient = ComponentFactory.Instance.creatBitmap("asset.toolbar.bgGradient");
         this._goSupplyBtn = ComponentFactory.Instance.creat("toolbar.gochargebtn");
         this._goShopBtn = ComponentFactory.Instance.creat("toolbar.goshopbtn");
         this._goBagBtn = ComponentFactory.Instance.creat("toolbar.gobagbtn");
         this._goEmailBtn = ComponentFactory.Instance.creat("toolbar.goemailbtn");
         this._goTaskBtn = ComponentFactory.Instance.creat("toolbar.gotaskbtn");
         this._goFriendListBtn = ComponentFactory.Instance.creat("toolbar.goimbtn");
         this._goSignBtn = ComponentFactory.Instance.creat("toolbar.gosignbtn");
         this._goChannelBtn = ComponentFactory.Instance.creat("toolbar.turntobtn");
         this._goReturnBtn = ComponentFactory.Instance.creat("toolbar.gobackbtn");
         this._goExitBtn = ComponentFactory.Instance.creat("toolbar.goexitbtn");
         this._goSupplyBtn.tipData = LanguageMgr.GetTranslation("tank.view.common.BellowStripViewII.supply");
         this._goShopBtn.tipData = LanguageMgr.GetTranslation("tank.view.common.BellowStripViewII.shop");
         this._goBagBtn.tipData = LanguageMgr.GetTranslation("tank.view.common.BellowStripViewII.bag");
         this._goEmailBtn.tipData = LanguageMgr.GetTranslation("tank.view.common.BellowStripViewII.email");
         this._goTaskBtn.tipData = LanguageMgr.GetTranslation("tank.game.ToolStripView.task");
         this._goFriendListBtn.tipData = LanguageMgr.GetTranslation("tank.game.ToolStripView.friend");
         this._goSignBtn.tipData = LanguageMgr.GetTranslation("tank.game.ToolStripView.sign");
         this._goChannelBtn.tipData = LanguageMgr.GetTranslation("tank.view.common.BellowStripViewII.channel");
         this._goReturnBtn.tipData = LanguageMgr.GetTranslation("tank.view.common.BellowStripViewII.back");
         this._goExitBtn.tipData = LanguageMgr.GetTranslation("tank.view.common.BellowStripViewII.exit");
         this.allBtns = [];
         this.allBtns.push(this._goShopBtn);
         this.allBtns.push(this._goBagBtn);
         this.allBtns.push(this._goEmailBtn);
         this.allBtns.push(this._goTaskBtn);
         this.allBtns.push(this._goFriendListBtn);
         this.allBtns.push(this._goSignBtn);
         this.allBtns.push(this._goChannelBtn);
         this.allBtns.push(this._goReturnBtn);
         this.allBtns.push(this._goSupplyBtn);
         addChild(this._toolBarBg);
         addChild(this._goTaskBtn);
         addChild(this._goReturnBtn);
         var _loc1_:Point = ComponentFactory.Instance.creatCustomObject("toolbar.bagShineIconPos");
         var _loc2_:Point = ComponentFactory.Instance.creatCustomObject("toolbar.ShineAssetPos");
         this._emailShineEffect = EffectManager.Instance.creatEffect(EffectTypes.ADD_MOVIE_EFFECT,this._goEmailBtn,"asset.toolbar.EmailBtnGlow",_loc2_);
         this._friendShineEffec = EffectManager.Instance.creatEffect(EffectTypes.ADD_MOVIE_EFFECT,this._goFriendListBtn,"asset.toolbar.friendBtnGlow",_loc2_);
         if(MailManager.Instance.Model.hasUnReadEmail())
         {
            this.showEmailShineEffect(true);
            this._unReadEmail = true;
         }
         else
         {
            this.showEmailShineEffect(false);
            this._unReadEmail = false;
         }
         this._taskShineEffect = EffectManager.Instance.creatEffect(EffectTypes.ADD_MOVIE_EFFECT,this._goTaskBtn,"asset.toolbar.TaskBtnGlow",_loc2_);
      }
      
      public function set enabled(param1:Boolean) : void
      {
         this._enabled = param1;
         this.update();
      }
      
      public function enableAll() : void
      {
         this.enabled = true;
         this._goExitBtn.enable = true;
      }
      
      public function disableAll() : void
      {
         this.enabled = false;
         this._goExitBtn.enable = false;
      }
      
      private function initEvent() : void
      {
         this._isEvent = true;
         this._goSupplyBtn.addEventListener(MouseEvent.CLICK,this.__onSupplyClick);
         this._goShopBtn.addEventListener(MouseEvent.CLICK,this.__onShopClick);
         this._goBagBtn.addEventListener(MouseEvent.CLICK,this.__onBagClick);
         this._goEmailBtn.addEventListener(MouseEvent.CLICK,this.__onEmailClick);
         this._goTaskBtn.addEventListener(MouseEvent.MOUSE_OVER,this._overTaskBtn);
         this._goTaskBtn.addEventListener(MouseEvent.MOUSE_OUT,this._outTaskBtn);
         this._goTaskBtn.addEventListener(MouseEvent.CLICK,this.__onTaskClick);
         this._goFriendListBtn.addEventListener(MouseEvent.CLICK,this.__onImClick);
         this._goFriendListBtn.addEventListener(MouseEvent.MOUSE_OVER,this.__friendOverHandler);
         this._goFriendListBtn.addEventListener(MouseEvent.MOUSE_OUT,this.__friendOutHandler);
         this._goSignBtn.addEventListener(MouseEvent.CLICK,this.__onSignClick);
         this._goChannelBtn.addEventListener(MouseEvent.CLICK,this.__onChannelClick);
         this._goReturnBtn.addEventListener(MouseEvent.CLICK,this.__onReturnClick);
         this._goExitBtn.addEventListener(MouseEvent.CLICK,this.__onExitClick);
         MailManager.Instance.Model.addEventListener(EmailEvent.INIT_EMAIL,this.__updateEmail);
         IMController.Instance.addEventListener(IMController.HAS_NEW_MESSAGE,this.__hasNewHandler);
         IMController.Instance.addEventListener(IMController.NO_MESSAGE,this.__noMessageHandler);
         addEventListener(Event.ADDED_TO_STAGE,this.__addToStageHandler);
      }
      
      protected function __addToStageHandler(param1:Event) : void
      {
         if(IMController.Instance.hasUnreadMessage() && !IMController.Instance.cancelflashState)
         {
            this.showFriendShineEffec(true);
         }
         else
         {
            this.showFriendShineEffec(false);
         }
      }
      
      protected function __noMessageHandler(param1:Event) : void
      {
         this.showFriendShineEffec(false);
      }
      
      protected function __hasNewHandler(param1:Event) : void
      {
         this.showFriendShineEffec(true);
      }
      
      protected function __friendOverHandler(param1:MouseEvent) : void
      {
         IMController.Instance.showMessageBox(this._goFriendListBtn);
      }
      
      protected function __friendOutHandler(param1:MouseEvent) : void
      {
         IMController.Instance.hideMessageBox();
      }
      
      public function btnOpen() : void
      {
         if(PlayerManager.Instance.Self.IsWeakGuildFinish(Step.CHANNEL_OPEN) && this._goChannelBtn.parent == null)
         {
            addChild(this._goChannelBtn);
            if(this._goChannelBtn.enable)
            {
               KeyboardShortcutsManager.Instance.prohibitNewHandChannel(true);
            }
         }
         if(PlayerManager.Instance.Self.IsWeakGuildFinish(Step.BAG_OPEN) && this._goBagBtn.parent == null)
         {
            addChild(this._goBagBtn);
            if(this._goBagBtn.enable)
            {
               KeyboardShortcutsManager.Instance.prohibitNewHandBag(true);
            }
         }
         if(PlayerManager.Instance.Self.IsWeakGuildFinish(Step.IM_OPEN) && this._goFriendListBtn.parent == null)
         {
            addChild(this._goFriendListBtn);
            if(this._goFriendListBtn.enable)
            {
               KeyboardShortcutsManager.Instance.prohibitNewHandFriend(true);
            }
         }
         if(PlayerManager.Instance.Self.IsWeakGuildFinish(Step.SHOP_OPEN) && this._goShopBtn.parent == null)
         {
            addChild(this._goShopBtn);
         }
         if(PlayerManager.Instance.Self.IsWeakGuildFinish(Step.SHOP_OPEN) && this._goSupplyBtn.parent == null)
         {
            addChild(this._goSupplyBtn);
         }
         if(PlayerManager.Instance.Self.IsWeakGuildFinish(Step.MAIL_OPEN) && this._goEmailBtn.parent == null)
         {
            addChild(this._goEmailBtn);
            if(this._goEmailBtn.enable)
            {
               KeyboardShortcutsManager.Instance.prohibitNewHandMail(true);
            }
         }
         if(PlayerManager.Instance.Self.IsWeakGuildFinish(Step.SIGN_OPEN) && this._goSignBtn.parent == null)
         {
            addChild(this._goSignBtn);
            if(this._goSignBtn.enable)
            {
               KeyboardShortcutsManager.Instance.prohibitNewHandCalendar(true);
            }
         }
      }
      
      public function set backFunction(param1:Function) : void
      {
         this._callBackFun = param1;
      }
      
      private function removeEvent() : void
      {
         this._isEvent = false;
         this._goSupplyBtn.removeEventListener(MouseEvent.CLICK,this.__onSupplyClick);
         this._goShopBtn.removeEventListener(MouseEvent.CLICK,this.__onShopClick);
         this._goBagBtn.removeEventListener(MouseEvent.CLICK,this.__onBagClick);
         this._goEmailBtn.removeEventListener(MouseEvent.CLICK,this.__onEmailClick);
         this._goTaskBtn.removeEventListener(MouseEvent.MOUSE_OVER,this._overTaskBtn);
         this._goTaskBtn.removeEventListener(MouseEvent.MOUSE_OUT,this._outTaskBtn);
         this._goTaskBtn.removeEventListener(MouseEvent.CLICK,this.__onTaskClick);
         this._goFriendListBtn.removeEventListener(MouseEvent.CLICK,this.__onImClick);
         this._goFriendListBtn.removeEventListener(MouseEvent.MOUSE_OVER,this.__friendOverHandler);
         this._goFriendListBtn.removeEventListener(MouseEvent.MOUSE_OUT,this.__friendOutHandler);
         this._goSignBtn.removeEventListener(MouseEvent.CLICK,this.__onSignClick);
         this._goChannelBtn.removeEventListener(MouseEvent.CLICK,this.__onChannelClick);
         this._goReturnBtn.removeEventListener(MouseEvent.CLICK,this.__onReturnClick);
         this._goExitBtn.removeEventListener(MouseEvent.CLICK,this.__onExitClick);
         MailManager.Instance.Model.removeEventListener(EmailEvent.INIT_EMAIL,this.__updateEmail);
         IMController.Instance.removeEventListener(IMController.HAS_NEW_MESSAGE,this.__hasNewHandler);
         IMController.Instance.removeEventListener(IMController.NO_MESSAGE,this.__noMessageHandler);
         removeEventListener(Event.ADDED_TO_STAGE,this.__addToStageHandler);
      }
      
      public function show() : void
      {
         if(!this._isEvent)
         {
            this.initEvent();
         }
         this.enableAll();
         if(IMController.Instance.hasUnreadMessage() && !IMController.Instance.cancelflashState)
         {
            this.showFriendShineEffec(true);
         }
         else
         {
            this.showFriendShineEffec(false);
         }
         LayerManager.Instance.addToLayer(this,LayerManager.GAME_UI_LAYER,false,0,false);
         this.refresh();
      }
      
      public function hide() : void
      {
         this.dispose();
      }
      
      public function setRoomWaitState() : void
      {
         KeyboardShortcutsManager.Instance.forbiddenSection(1,true);
      }
      
      public function setRoomStartState() : void
      {
         KeyboardShortcutsManager.Instance.forbiddenSection(1,false);
         this._goReturnBtn.enable = this._goShopBtn.enable = this._goSupplyBtn.enable = false;
         this.setMailEnable(false);
         this.setSignEnable(false);
         this.setChannelEnable(false);
         this.setBagEnable(false);
         this.showEmailShineEffect(false);
      }
      
      public function setRoomStartState2(param1:Boolean) : void
      {
         KeyboardShortcutsManager.Instance.forbiddenSection(1,param1);
         this._goReturnBtn.enable = this._goShopBtn.enable = this._goSupplyBtn.enable = param1;
         this.setMailEnable(param1);
         this.setSignEnable(param1);
         this.setChannelEnable(param1);
         this.setBagEnable(param1);
         this.showEmailShineEffect(param1);
      }
      
      private function setSeverListStartState() : void
      {
         this._goReturnBtn.enable = this._goTaskBtn.enable = this._goShopBtn.enable = this._goSupplyBtn.enable = false;
         this.setMailEnable(false);
         this.setSignEnable(false);
         this.setChannelEnable(false);
         this.setFriendBtnEnable(false);
         this.setBagEnable(false);
         if(this._emailShineEffect)
         {
            this.showEmailShineEffect(false);
         }
         if(this._taskShineEffect)
         {
            this.showTaskShineEffect(false);
         }
      }
      
      public function setReturnEnable(param1:Boolean) : void
      {
         this._goReturnBtn.enable = param1;
      }
      
      public function updateReturnBtn(param1:int) : void
      {
         switch(param1)
         {
            case ENTER_HALL:
               addChildAt(this._bgGradient,0);
               addChild(this._goExitBtn);
               if(this._goReturnBtn.parent)
               {
                  this._goReturnBtn.parent.removeChild(this._goReturnBtn);
               }
               break;
            case LEAVE_HALL:
               if(this._bgGradient.parent)
               {
                  this._bgGradient.parent.removeChild(this._bgGradient);
               }
               addChild(this._goReturnBtn);
               if(this._goExitBtn.parent)
               {
                  this._goExitBtn.parent.removeChild(this._goExitBtn);
               }
         }
      }
      
      private function isBitMapAddGrayFilter(param1:Bitmap, param2:Boolean) : void
      {
         if(param1 == null)
         {
            return;
         }
         if(!param2)
         {
            param1.filters = ComponentFactory.Instance.creatFilters("grayFilter");
         }
         else
         {
            param1.filters = null;
         }
      }
      
      private function dispose() : void
      {
         this.removeEvent();
         QuestBubbleManager.Instance.dispose();
         if(parent)
         {
            parent.removeChild(this);
         }
      }
      
      private function __onReturnClick(param1:MouseEvent) : void
      {
         SoundManager.instance.play("015");
         NewHandContainer.Instance.clearArrowByID(ArrowType.BAG_ROOM);
         if(StateManager.currentStateType == StateType.MAIN)
         {
            KeyboardShortcutsManager.Instance.forbiddenFull();
         }
         if(this._callBackFun != null)
         {
            this._callBackFun();
         }
         else
         {
            StateManager.back();
         }
      }
      
      private function __onExitClick(param1:MouseEvent) : void
      {
         SoundManager.instance.play("008");
         ExitPromptManager.Instance.showView("1");
      }
      
      private function __onImClick(param1:MouseEvent) : void
      {
         SoundManager.instance.play("003");
         IMController.Instance.switchVisible();
      }
      
      private function __onChannelClick(param1:MouseEvent) : void
      {
         SoundManager.instance.play("003");
         GotoPageController.Instance.switchVisible();
      }
      
      private function __onSignClick(param1:MouseEvent) : void
      {
         SoundManager.instance.play("003");
         if(PlayerManager.Instance.Self.Grade < 25)
         {
            MessageTipManager.getInstance().show(LanguageMgr.GetTranslation("MainToolBar.PetLevelTip"));
            return;
         }
         this._signEffectEnable = false;
         PetsBagManager.instance.show();
      }
      
      public function set signEffectEnable(param1:Boolean) : void
      {
         this._signEffectEnable = param1;
      }
      
      private function __onEmailClick(param1:MouseEvent) : void
      {
         SoundManager.instance.play("003");
         this.showEmailShineEffect(false);
         this._unReadEmail = false;
         MailManager.Instance.switchVisible();
      }
      
      private function _overTaskBtn(param1:MouseEvent) : void
      {
         ShowTipManager.Instance.removeTip(this._goTaskBtn);
         QuestBubbleManager.Instance.addEventListener(QuestBubbleManager.Instance.SHOWTASKTIP,this._showTaskTip);
         QuestBubbleManager.Instance.show();
      }
      
      private function _outTaskBtn(param1:MouseEvent) : void
      {
         QuestBubbleManager.Instance.dispose();
      }
      
      private function _showTaskTip(param1:Event) : void
      {
         ShowTipManager.Instance.addTip(this._goTaskBtn);
         ShowTipManager.Instance.showTip(this._goTaskBtn);
         QuestBubbleManager.Instance.dispose();
      }
      
      private function __onTaskClick(param1:MouseEvent) : void
      {
         SoundManager.instance.play("003");
         if(this._taskShineEffect && this._taskEffectEnable)
         {
            this._taskEffectEnable = false;
            this.showTaskShineEffect(false);
         }
         QuestBubbleManager.Instance.dispose(true);
         TaskManager.switchVisible();
      }
      
      private function __onBagClick(param1:MouseEvent) : void
      {
         SoundManager.instance.play("003");
         BagAndInfoManager.Instance.showBagAndInfo();
         if(!PlayerManager.Instance.Self.IsWeakGuildFinish(Step.CLICK_BAG))
         {
            SocketManager.Instance.out.syncWeakStep(Step.CLICK_BAG);
            SocketManager.Instance.out.syncWeakStep(Step.BAG_OPEN_SHOW);
         }
      }
      
      private function __onShopClick(param1:MouseEvent) : void
      {
         var _loc2_:BaseAlerFrame = null;
         if(this.toShopNeedConfirm())
         {
            _loc2_ = AlertManager.Instance.simpleAlert(LanguageMgr.GetTranslation("AlertDialog.Info"),LanguageMgr.GetTranslation("tank.room.ToShopConfirm"),LanguageMgr.GetTranslation("ok"),LanguageMgr.GetTranslation("cancel"),false,false,false,LayerManager.ALPHA_BLOCKGOUND);
            _loc2_.addEventListener(FrameEvent.RESPONSE,this.__confirmToShopResponse);
         }
         else
         {
            StateManager.setState(StateType.SHOP);
         }
         SoundManager.instance.play("003");
      }
      
      private function toShopNeedConfirm() : Boolean
      {
         if(StateManager.currentStateType == StateType.MATCH_ROOM || StateManager.currentStateType == StateType.DUNGEON_ROOM || StateManager.currentStateType == StateType.MISSION_ROOM)
         {
            return true;
         }
         return false;
      }
      
      private function __confirmToShopResponse(param1:FrameEvent) : void
      {
         var _loc2_:BaseAlerFrame = param1.currentTarget as BaseAlerFrame;
         _loc2_.removeEventListener(FrameEvent.RESPONSE,this.__confirmToShopResponse);
         ObjectUtils.disposeObject(_loc2_);
         SoundManager.instance.play("008");
         if(param1.responseCode == FrameEvent.ENTER_CLICK || param1.responseCode == FrameEvent.SUBMIT_CLICK)
         {
            StateManager.setState(StateType.SHOP);
         }
      }
      
      private function __onSupplyClick(param1:MouseEvent) : void
      {
         LeavePageManager.leaveToFillPath();
      }
      
      public function set unReadEmail(param1:Boolean) : void
      {
         if(param1 == this._unReadEmail)
         {
            return;
         }
         this._unReadEmail = param1;
         if(this._enabled)
         {
            this.updateEmail();
         }
      }
      
      public function set unReadTask(param1:Boolean) : void
      {
         if(this._unReadTask == param1)
         {
            return;
         }
         this._unReadTask = param1;
         if(this._enabled)
         {
            this.updateTask();
         }
      }
      
      public function get unReadTask() : Boolean
      {
         return this._unReadTask;
      }
      
      public function set unReadMovement(param1:Boolean) : void
      {
      }
      
      public function get unReadMovement() : Boolean
      {
         return this._unReadMovement;
      }
      
      private function __updateEmail(param1:EmailEvent) : void
      {
         if(MailManager.Instance.Model.hasUnReadEmail())
         {
            this.unReadEmail = true;
         }
         else
         {
            this.unReadEmail = false;
         }
      }
      
      public function showTaskHightLight() : void
      {
         this.unReadTask = true;
      }
      
      public function hideTaskHightLight() : void
      {
         this.unReadTask = false;
      }
      
      public function showmovementHightLight() : void
      {
         this.unReadMovement = true;
      }
      
      public function setRoomState() : void
      {
         this.setChannelEnable(false);
      }
      
      public function setShopState() : void
      {
         this.setBagEnable(false);
      }
      
      public function setAuctionHouseState() : void
      {
         this.setBagEnable(false);
      }
      
      private function update() : void
      {
         var _loc1_:uint = 0;
         while(_loc1_ < this.allBtns.length)
         {
            if(this._enabled)
            {
               if(_loc1_ == 3)
               {
                  this.updateTask();
               }
               else if(_loc1_ == 2)
               {
                  this.updateEmail();
               }
            }
            this.setEnableByIndex(_loc1_,this._enabled);
            _loc1_++;
         }
         this.setRoomWaitState();
      }
      
      private function setEnableByIndex(param1:int, param2:Boolean) : void
      {
         if(param1 == 1)
         {
            this.setBagEnable(param2);
         }
         else if(param1 == 2)
         {
            this.setMailEnable(param2);
         }
         else if(param1 == 4)
         {
            this.setFriendBtnEnable(param2);
         }
         else if(param1 == 5)
         {
            this.setSignEnable(param2);
         }
         else if(param1 == 6)
         {
            this.setChannelEnable(param2);
         }
         else
         {
            this.allBtns[param1].enable = param2;
         }
      }
      
      private function updateTask() : void
      {
         if(this._unReadTask)
         {
            this.showTaskShineEffect(true);
         }
         else
         {
            this.showTaskShineEffect(false);
         }
         this._goTaskBtn.enable = true;
         this.tipTask();
      }
      
      private function updateEmail() : void
      {
         if(!this._goEmailBtn.enable)
         {
            return;
         }
         if(this._unReadEmail)
         {
            this.showEmailShineEffect(true);
         }
         else
         {
            this.showEmailShineEffect(false);
         }
         this._goEmailBtn.enable = true;
      }
      
      private function showEmailShineEffect(param1:Boolean) : void
      {
         if(param1)
         {
            this._emailShineEffect.play();
            this._goEmailBtn.alpha = 0;
         }
         else
         {
            this._emailShineEffect.stop();
            this._goEmailBtn.alpha = 1;
         }
      }
      
      private function showFriendShineEffec(param1:Boolean) : void
      {
         if(param1 && this._goFriendListBtn.parent)
         {
            this._friendShineEffec.play();
            this._goFriendListBtn.alpha = 0;
         }
         else
         {
            this._friendShineEffec.stop();
            this._goFriendListBtn.alpha = 1;
         }
      }
      
      private function showTaskShineEffect(param1:Boolean) : void
      {
         if(param1)
         {
            this._taskShineEffect.play();
            this._goTaskBtn.alpha = 0;
         }
         else
         {
            this._taskShineEffect.stop();
            this._goTaskBtn.alpha = 1;
         }
      }
      
      public function __player(param1:MouseEvent) : void
      {
         SoundManager.instance.play("008");
      }
      
      public function refresh() : void
      {
         if(!WeakGuildManager.Instance.switchUserGuide || PlayerManager.Instance.Self.IsWeakGuildFinish(Step.OLD_PLAYER))
         {
            return;
         }
         if(PlayerManager.Instance.Self.IsWeakGuildFinish(Step.BAG_OPEN))
         {
            if(!PlayerManager.Instance.Self.IsWeakGuildFinish(Step.BAG_OPEN_SHOW) && parent && this._goBagBtn.parent && this._goBagBtn.enable)
            {
               WeakGuildManager.Instance.showMainToolBarBtnOpen(Step.BAG_OPEN_SHOW,"trainer.posBarBag");
            }
         }
         if(PlayerManager.Instance.Self.IsWeakGuildFinish(Step.IM_OPEN))
         {
            if(!PlayerManager.Instance.Self.IsWeakGuildFinish(Step.IM_SHOW) && parent && this._goFriendListBtn.parent && this._goFriendListBtn.enable)
            {
               WeakGuildManager.Instance.showMainToolBarBtnOpen(Step.IM_SHOW,"trainer.posBarIM");
            }
         }
         if(PlayerManager.Instance.Self.IsWeakGuildFinish(Step.CHANNEL_OPEN))
         {
            if(!PlayerManager.Instance.Self.IsWeakGuildFinish(Step.CHANNEL_OPEN_SHOW) && parent && this._goChannelBtn.parent && this._goChannelBtn.enable)
            {
               WeakGuildManager.Instance.showMainToolBarBtnOpen(Step.CHANNEL_OPEN_SHOW,"trainer.posBarChannel");
            }
         }
         if(PlayerManager.Instance.Self.IsWeakGuildFinish(Step.SIGN_OPEN))
         {
            if(!PlayerManager.Instance.Self.IsWeakGuildFinish(Step.SIGN_OPEN_SHOW) && parent && this._goSignBtn.parent && this._goSignBtn.enable)
            {
               WeakGuildManager.Instance.showMainToolBarBtnOpen(Step.SIGN_OPEN_SHOW,"trainer.posBarSign");
            }
         }
      }
      
      private function setChannelEnable(param1:Boolean) : void
      {
         this._goChannelBtn.enable = param1;
         if(param1 && PlayerManager.Instance.Self.IsWeakGuildFinish(Step.CHANNEL_OPEN))
         {
            KeyboardShortcutsManager.Instance.prohibitNewHandChannel(true);
            KeyboardShortcutsManager.Instance.prohibitNewHandSeting(true);
         }
         else
         {
            KeyboardShortcutsManager.Instance.prohibitNewHandChannel(false);
            KeyboardShortcutsManager.Instance.prohibitNewHandSeting(false);
         }
      }
      
      private function setBagEnable(param1:Boolean) : void
      {
         this._goBagBtn.enable = param1;
         if(param1 && PlayerManager.Instance.Self.IsWeakGuildFinish(Step.BAG_OPEN))
         {
            KeyboardShortcutsManager.Instance.prohibitNewHandBag(true);
         }
         else
         {
            KeyboardShortcutsManager.Instance.prohibitNewHandBag(false);
         }
      }
      
      private function setFriendBtnEnable(param1:Boolean) : void
      {
         this._goFriendListBtn.enable = param1;
         if(param1 && PlayerManager.Instance.Self.IsWeakGuildFinish(Step.IM_OPEN))
         {
            KeyboardShortcutsManager.Instance.prohibitNewHandFriend(true);
         }
         else
         {
            KeyboardShortcutsManager.Instance.prohibitNewHandFriend(false);
         }
      }
      
      private function setMailEnable(param1:Boolean) : void
      {
         this._goEmailBtn.enable = param1;
         if(param1 && PlayerManager.Instance.Self.IsWeakGuildFinish(Step.MAIL_OPEN))
         {
            KeyboardShortcutsManager.Instance.prohibitNewHandMail(true);
         }
         else
         {
            KeyboardShortcutsManager.Instance.prohibitNewHandMail(false);
         }
      }
      
      private function setSignEnable(param1:Boolean) : void
      {
         this._goSignBtn.enable = param1;
         if(param1 && PlayerManager.Instance.Self.IsWeakGuildFinish(Step.SIGN_OPEN))
         {
            KeyboardShortcutsManager.Instance.prohibitNewHandCalendar(true);
         }
         else
         {
            KeyboardShortcutsManager.Instance.prohibitNewHandCalendar(false);
         }
      }
      
      public function tipTask() : void
      {
         if(!WeakGuildManager.Instance.switchUserGuide || PlayerManager.Instance.Self.IsWeakGuildFinish(Step.OLD_PLAYER))
         {
            return;
         }
         if(StateManager.currentStateType != StateType.MAIN && StateManager.currentStateType != StateType.MATCH_ROOM)
         {
            return;
         }
         if(NewHandContainer.Instance.hasArrow(ArrowType.HALL_BUILD) || NewHandContainer.Instance.hasArrow(ArrowType.START_GAME) || NewHandContainer.Instance.hasArrow(ArrowType.GET_ZXC_ITEM))
         {
            return;
         }
         NewHandContainer.Instance.clearArrowByID(-1);
         if(BagView.isFlag)
         {
            BagView.isFlag = false;
            dispatchEvent(new Event("SHOW_ARROW_BAGVIEW"));
         }
         if(PlayerManager.Instance.Self.Grade == 3 && !PlayerManager.Instance.Self.IsWeakGuildFinish(Step.CLICK_BAG) && this._goBagBtn.parent && this._goBagBtn.enable)
         {
            NewHandContainer.Instance.showArrow(ArrowType.CLICK_BAG,0,"trainer.posClickBag","asset.trainer.txtClickBag","trainer.posClickBagTxt",this);
         }
         else if(this._unReadTask && !PlayerManager.Instance.Self.IsWeakGuildFinish(Step.ACHIVED_THREE_QUEST))
         {
            NewHandContainer.Instance.showArrow(ArrowType.CLICK_TASK,0,"trainer.posClickTask","asset.trainer.txtGetReward","trainer.posClickTaskTxt",this);
         }
         else if(PlayerManager.Instance.Self.Grade < 3)
         {
            NewHandContainer.Instance.showArrow(ArrowType.CLICK_TASK,0,"trainer.posClickTask","asset.trainer.txtClickTask","trainer.posClickTaskTxt",this);
         }
         else if(TaskManager.isAchieved(TaskManager.getQuestByID(320)) && StateManager.currentStateType != StateType.MAIN && !TaskManager.getQuestByID(321).isCompleted && PlayerManager.Instance.Self.Grade == 3)
         {
            NewHandContainer.Instance.showArrow(ArrowType.BAG_ROOM,0,"trainer.RoomListViewArrowPos");
         }
      }
   }
}
