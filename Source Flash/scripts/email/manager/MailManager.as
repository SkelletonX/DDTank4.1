package email.manager
{
   import bagAndInfo.BagAndGiftFrame;
   import bagAndInfo.BagAndInfoManager;
   import com.pickgliss.events.FrameEvent;
   import com.pickgliss.events.UIModuleEvent;
   import com.pickgliss.loader.BaseLoader;
   import com.pickgliss.loader.LoaderEvent;
   import com.pickgliss.loader.LoaderManager;
   import com.pickgliss.loader.UIModuleLoader;
   import com.pickgliss.toplevel.StageReferance;
   import com.pickgliss.ui.AlertManager;
   import com.pickgliss.ui.ComponentFactory;
   import com.pickgliss.ui.LayerManager;
   import com.pickgliss.ui.controls.alert.BaseAlerFrame;
   import com.pickgliss.utils.ObjectUtils;
   import consortion.ConsortionModelControl;
   import ddt.data.UIModuleTypes;
   import ddt.data.analyze.ShopItemAnalyzer;
   import ddt.events.CrazyTankSocketEvent;
   import ddt.manager.LanguageMgr;
   import ddt.manager.LeavePageManager;
   import ddt.manager.MessageTipManager;
   import ddt.manager.PathManager;
   import ddt.manager.PlayerManager;
   import ddt.manager.SelectListManager;
   import ddt.manager.SharedManager;
   import ddt.manager.ShopManager;
   import ddt.manager.SocketManager;
   import ddt.manager.TaskManager;
   import ddt.manager.TimeManager;
   import ddt.utils.PositionUtils;
   import ddt.utils.RequestVairableCreater;
   import ddt.view.MainToolBar;
   import ddt.view.UIModuleSmallLoading;
   import email.analyze.AllEmailAnalyzer;
   import email.analyze.SendedEmailAnalyze;
   import email.data.EmailInfo;
   import email.data.EmailState;
   import email.data.EmailType;
   import email.model.EmailModel;
   import email.view.EmailEvent;
   import email.view.EmailView;
   import email.view.WritingView;
   import flash.events.Event;
   import flash.net.URLVariables;
   import road7th.comm.PackageIn;
   import room.RoomManager;
   
   public class MailManager
   {
      
      private static var useFirst:Boolean = true;
      
      private static var loadComplete:Boolean = false;
      
      private static var _instance:MailManager;
       
      
      public const NUM_OF_WRITING_DIAMONDS:uint = 4;
      
      private var _model:EmailModel;
      
      private var _view:EmailView;
      
      private var _isShow:Boolean;
      
      private var args:URLVariables;
      
      public var isOpenFromBag:Boolean = false;
      
      private var _write:WritingView;
      
      private var _name:String;
      
      private var emailtype:int;
      
      public function MailManager()
      {
         super();
         this._model = new EmailModel();
         SocketManager.Instance.addEventListener(CrazyTankSocketEvent.SEND_EMAIL,this.__sendEmail);
         SocketManager.Instance.addEventListener(CrazyTankSocketEvent.DELETE_MAIL,this.__deleteMail);
         SocketManager.Instance.addEventListener(CrazyTankSocketEvent.GET_MAIL_ATTACHMENT,this.__getMailToBag);
         SocketManager.Instance.addEventListener(CrazyTankSocketEvent.MAIL_CANCEL,this.__mailCancel);
         SocketManager.Instance.addEventListener(CrazyTankSocketEvent.MAIL_RESPONSE,this.__responseMail);
      }
      
      public static function get Instance() : MailManager
      {
         if(_instance == null)
         {
            _instance = new MailManager();
         }
         return _instance;
      }
      
      public function get Model() : EmailModel
      {
         return this._model;
      }
      
      public function getAllEmailLoader() : BaseLoader
      {
         var _loc1_:URLVariables = RequestVairableCreater.creatWidthKey(true);
         if(ConsortionModelControl.Instance.quitConstrion)
         {
            _loc1_["chairmanID"] = 0;
         }
         else if(PlayerManager.Instance.Self.consortiaInfo.ChairmanID)
         {
            _loc1_["chairmanID"] = PlayerManager.Instance.Self.consortiaInfo.ChairmanID;
         }
         else
         {
            _loc1_["chairmanID"] = SelectListManager.Instance.currentLoginRole.ChairmanID;
         }
         var _loc2_:BaseLoader = LoaderManager.Instance.creatLoader(PathManager.solveRequestPath("LoadUserMail.ashx"),BaseLoader.COMPRESS_REQUEST_LOADER,_loc1_);
         _loc2_.loadErrorMessage = LanguageMgr.GetTranslation("tank.view.emailII.LoadMailAllInfoError");
         _loc2_.analyzer = new AllEmailAnalyzer(this.stepAllEmail);
         _loc2_.addEventListener(LoaderEvent.LOAD_ERROR,this.__onLoadError);
         return _loc2_;
      }
      
      public function getSendedEmailLoader() : BaseLoader
      {
         var _loc1_:URLVariables = RequestVairableCreater.creatWidthKey(true);
         var _loc2_:BaseLoader = LoaderManager.Instance.creatLoader(PathManager.solveRequestPath("MailSenderList.ashx"),BaseLoader.COMPRESS_REQUEST_LOADER,_loc1_);
         _loc2_.loadErrorMessage = LanguageMgr.GetTranslation("tank.view.emailII.LoadSendInfoError");
         _loc2_.analyzer = new SendedEmailAnalyze(this.stepSendedEmails);
         _loc2_.addEventListener(LoaderEvent.LOAD_ERROR,this.__onLoadError);
         return _loc2_;
      }
      
      private function __onLoadError(param1:LoaderEvent) : void
      {
         var _loc2_:String = param1.loader.loadErrorMessage;
         if(param1.loader.analyzer)
         {
            _loc2_ = param1.loader.loadErrorMessage + "\n" + param1.loader.analyzer.message;
         }
         var _loc3_:BaseAlerFrame = AlertManager.Instance.simpleAlert(LanguageMgr.GetTranslation("alert"),param1.loader.loadErrorMessage,LanguageMgr.GetTranslation("tank.view.bagII.baglocked.sure"));
         _loc3_.addEventListener(FrameEvent.RESPONSE,this.__onAlertResponse);
      }
      
      private function __onAlertResponse(param1:FrameEvent) : void
      {
         param1.currentTarget.addEventListener(FrameEvent.RESPONSE,this.__onAlertResponse);
         ObjectUtils.disposeObject(param1.currentTarget);
         LeavePageManager.leaveToLoginPath();
      }
      
      public function loadMail(param1:uint) : void
      {
         switch(param1)
         {
            case 1:
               LoaderManager.Instance.startLoad(this.getAllEmailLoader());
               break;
            case 2:
               LoaderManager.Instance.startLoad(this.getSendedEmailLoader());
               break;
            case 3:
               LoaderManager.Instance.startLoad(this.getAllEmailLoader());
               LoaderManager.Instance.startLoad(this.getSendedEmailLoader());
         }
      }
      
      public function get isShow() : Boolean
      {
         return this._isShow;
      }
      
      public function stepAllEmail(param1:AllEmailAnalyzer) : void
      {
         this._model.emails = param1.list;
         this.changeSelected(null);
         if(this._model.hasUnReadEmail() && (RoomManager.Instance.current != null && !RoomManager.Instance.current.started))
         {
            MainToolBar.Instance.unReadEmail = true;
         }
      }
      
      private function stepSendedEmails(param1:SendedEmailAnalyze) : void
      {
         this._model.sendedMails = param1.list;
      }
      
      public function show() : void
      {
         if(loadComplete)
         {
            this._view = null;
            this._view = ComponentFactory.Instance.creatCustomObject("emailView");
            this._view.setup(this,this._model);
            this._view.addEventListener(EmailEvent.ESCAPE_KEY,this.__escapeKeyDown);
            this._isShow = true;
            this._view.show();
         }
         else if(useFirst)
         {
            UIModuleSmallLoading.Instance.progress = 0;
            UIModuleSmallLoading.Instance.show();
            UIModuleSmallLoading.Instance.addEventListener(Event.CLOSE,this.__onClose);
            UIModuleLoader.Instance.addEventListener(UIModuleEvent.UI_MODULE_COMPLETE,this.__onUIModuleComplete);
            UIModuleLoader.Instance.addUIModuleImp(UIModuleTypes.EMAIL);
            useFirst = false;
         }
      }
      
      private function __onClose(param1:Event) : void
      {
         useFirst = true;
         UIModuleSmallLoading.Instance.hide();
         UIModuleSmallLoading.Instance.removeEventListener(Event.CLOSE,this.__onClose);
         UIModuleLoader.Instance.removeEventListener(UIModuleEvent.UI_MODULE_COMPLETE,this.__onUIModuleComplete);
      }
      
      protected function __onUIModuleComplete(param1:UIModuleEvent) : void
      {
         UIModuleSmallLoading.Instance.hide();
         UIModuleSmallLoading.Instance.removeEventListener(Event.CLOSE,this.__onClose);
         UIModuleLoader.Instance.removeEventListener(UIModuleEvent.UI_MODULE_COMPLETE,this.__onUIModuleComplete);
         loadComplete = true;
         if(param1.module == UIModuleTypes.EMAIL)
         {
            this.show();
         }
      }
      
      public function hide() : void
      {
         MainToolBar.Instance.unReadEmail = false;
         if(this._view)
         {
            this._model.selectEmail = null;
            this._model.mailType = EmailState.ALL;
            this._view.removeEventListener(EmailEvent.ESCAPE_KEY,this.__escapeKeyDown);
            this._view.dispose();
            this._view = null;
         }
         this._isShow = false;
         if(this.isOpenFromBag)
         {
            BagAndInfoManager.Instance.showBagAndInfo(BagAndGiftFrame.GIFTVIEW);
         }
         this.isOpenFromBag = false;
      }
      
      private function __escapeKeyDown(param1:EmailEvent) : void
      {
         if(this._write && this._write.parent)
         {
            this._write.removeEventListener(EmailEvent.ESCAPE_KEY,this.__escapeKeyDown);
            this._write.removeEventListener(EmailEvent.CLOSE_WRITING_FRAME,this.__closeWritingFrame);
            ObjectUtils.disposeObject(this._write);
            this._write = null;
         }
         if(this._view)
         {
            if(this._view.writeView && this._view.writeView.parent)
            {
               this._view.writeView.closeWin();
            }
            else
            {
               this.hide();
            }
         }
      }
      
      public function switchVisible() : void
      {
         if(this._view && this._view.parent)
         {
            this.hide();
         }
         else
         {
            this.show();
         }
      }
      
      public function changeState(param1:String) : void
      {
         this._model.state = param1;
      }
      
      public function showWriting(param1:String = null) : void
      {
         this._name = param1;
         if(loadComplete)
         {
            this.creatWriteView();
         }
         else
         {
            UIModuleSmallLoading.Instance.progress = 0;
            UIModuleSmallLoading.Instance.show();
            UIModuleSmallLoading.Instance.addEventListener(Event.CLOSE,this.__onClose);
            UIModuleLoader.Instance.addEventListener(UIModuleEvent.UI_MODULE_COMPLETE,this.__onWriteUIModuleComplete);
            UIModuleLoader.Instance.addUIModuleImp(UIModuleTypes.EMAIL);
         }
      }
      
      private function __onWriteUIModuleComplete(param1:UIModuleEvent) : void
      {
         UIModuleSmallLoading.Instance.hide();
         UIModuleSmallLoading.Instance.removeEventListener(Event.CLOSE,this.__onClose);
         UIModuleLoader.Instance.removeEventListener(UIModuleEvent.UI_MODULE_COMPLETE,this.__onUIModuleComplete);
         loadComplete = true;
         if(param1.module == UIModuleTypes.EMAIL)
         {
            this.creatWriteView();
         }
      }
      
      private function creatWriteView() : void
      {
         if(this._write != null)
         {
            this._write = null;
         }
         this._write = ComponentFactory.Instance.creat("email.writingView");
         this._write.type = 0;
         PositionUtils.setPos(this._write,"EmailView.Pos_2");
         this._write.selectInfo = this._model.selectEmail;
         LayerManager.Instance.addToLayer(this._write,LayerManager.GAME_DYNAMIC_LAYER,false,LayerManager.BLCAK_BLOCKGOUND);
         if(StageReferance.stage && StageReferance.stage.focus)
         {
            StageReferance.stage.focus == this._write;
         }
         this._write.reset();
         if(this._name != null)
         {
            this._write.setName(this._name);
         }
         this._write.addEventListener(EmailEvent.CLOSE_WRITING_FRAME,this.__closeWritingFrame);
         this._write.addEventListener(FrameEvent.RESPONSE,this.__closeWriting);
         this._write.addEventListener(EmailEvent.ESCAPE_KEY,this.__escapeKeyDown);
         this._write.addEventListener(EmailEvent.DISPOSED,this.__onDispose);
      }
      
      private function __closeWriting(param1:FrameEvent) : void
      {
         if(this._write)
         {
            this._write.closeWin();
         }
      }
      
      private function __closeWritingFrame(param1:EmailEvent) : void
      {
         if(this._write)
         {
            this._write.removeEventListener(FrameEvent.RESPONSE,this.__closeWriting);
            this._write.removeEventListener(EmailEvent.ESCAPE_KEY,this.__escapeKeyDown);
            this._write.removeEventListener(EmailEvent.CLOSE_WRITING_FRAME,this.__closeWritingFrame);
            ObjectUtils.disposeObject(this._write);
            this._write = null;
         }
      }
      
      private function __onDispose(param1:EmailEvent) : void
      {
         if(this._write)
         {
            try
            {
               this._write.removeEventListener(FrameEvent.RESPONSE,this.__closeWriting);
               this._write.removeEventListener(EmailEvent.ESCAPE_KEY,this.__escapeKeyDown);
               this._write.removeEventListener(EmailEvent.CLOSE_WRITING_FRAME,this.__closeWritingFrame);
               ObjectUtils.disposeObject(this._write);
            }
            catch(e:Error)
            {
            }
         }
         this._write = null;
      }
      
      public function changeType(param1:String) : void
      {
         if(this._model.mailType == param1)
         {
            return;
         }
         this.updateNoReadMails();
         this._model.mailType = param1;
      }
      
      public function changeSelected(param1:EmailInfo) : void
      {
         this._model.selectEmail = param1;
      }
      
      public function updateNoReadMails() : void
      {
         this._model.getNoReadMails();
      }
      
      public function getAnnexToBag(param1:EmailInfo, param2:int) : void
      {
         if(!this.HasAtLeastOneDiamond(param1))
         {
            return;
         }
         SocketManager.Instance.out.sendGetMail(param1.ID,param2);
      }
      
      private function HasAtLeastOneDiamond(param1:EmailInfo) : Boolean
      {
         if(param1.Gold > 0)
         {
            return true;
         }
         if(param1.Money > 0)
         {
            return true;
         }
         if(param1.GiftToken > 0)
         {
            return true;
         }
         if(param1.Medal > 0)
         {
            return true;
         }
         var _loc2_:uint = 1;
         while(_loc2_ <= 5)
         {
            if(param1["Annex" + _loc2_])
            {
               return true;
            }
            _loc2_++;
         }
         return false;
      }
      
      public function deleteEmail(param1:EmailInfo) : void
      {
         var _loc2_:Array = null;
         if(param1)
         {
            if(param1.Type == EmailType.CONSORTION_EMAIL)
            {
               if(SharedManager.Instance.deleteMail[PlayerManager.Instance.Self.ID])
               {
                  _loc2_ = SharedManager.Instance.deleteMail[PlayerManager.Instance.Self.ID] as Array;
                  if(_loc2_.indexOf(param1.ID) < 0)
                  {
                     _loc2_.push(param1.ID);
                  }
               }
               else
               {
                  SharedManager.Instance.deleteMail[PlayerManager.Instance.Self.ID] = [param1.ID];
               }
               SharedManager.Instance.save();
            }
            SocketManager.Instance.out.sendDeleteMail(param1.ID);
         }
      }
      
      public function readEmail(param1:EmailInfo) : void
      {
         if(this._model.mailType != EmailState.NOREAD)
         {
            this._model.removeFromNoRead(param1);
         }
         SocketManager.Instance.out.sendUpdateMail(param1.ID);
      }
      
      public function setPage(param1:Boolean, param2:Boolean = true) : void
      {
         if(!param1 && !param2)
         {
            this._model.currentPage = this._model.currentPage;
            return;
         }
         if(param1)
         {
            if(this._model.currentPage - 1 > 0)
            {
               this._model.currentPage = this._model.currentPage - 1;
            }
         }
         else if(this._model.currentPage + 1 <= this._model.totalPage)
         {
            this._model.currentPage = this._model.currentPage + 1;
         }
         else if(this._model.mailType == EmailState.NOREAD && this._model.totalPage == 1)
         {
            this._model.currentPage = 1;
         }
      }
      
      public function sendEmail(param1:Object) : void
      {
         SocketManager.Instance.out.sendEmail(param1);
      }
      
      public function onSendAnnex(param1:Array) : void
      {
         TaskManager.onSendAnnex(param1);
      }
      
      public function untreadEmail(param1:int) : void
      {
         SocketManager.Instance.out.untreadEmail(param1);
      }
      
      private function __getMailToBag(param1:CrazyTankSocketEvent) : void
      {
         var _loc7_:int = 0;
         var _loc2_:PackageIn = param1.pkg;
         var _loc3_:int = _loc2_.readInt();
         var _loc4_:int = _loc2_.readInt();
         var _loc5_:EmailInfo = this._model.getMailByID(_loc3_);
         if(!_loc5_)
         {
            return;
         }
         if(_loc5_.Type > 100 && _loc5_.Money > 0)
         {
            _loc5_.ValidDate = 72;
            _loc5_.Money = 0;
         }
         var _loc6_:uint = 0;
         while(_loc6_ < _loc4_)
         {
            _loc7_ = _loc2_.readInt();
            this.deleteMailDiamond(_loc5_,_loc7_);
            _loc6_++;
         }
         this._model.changeEmail(_loc5_);
      }
      
      private function deleteMailDiamond(param1:EmailInfo, param2:int) : void
      {
         var _loc3_:uint = 0;
         switch(param2)
         {
            case 6:
               param1.Gold = 0;
               break;
            case 7:
               param1.Money = 0;
               break;
            case 8:
               param1.GiftToken = 0;
               break;
            case 9:
               param1.Medal = 0;
               break;
            default:
               _loc3_ = 1;
               while(_loc3_ <= 5)
               {
                  if(param2 == _loc3_)
                  {
                     param1["Annex" + _loc3_] = null;
                     break;
                  }
                  _loc3_++;
               }
         }
      }
      
      private function __deleteMail(param1:CrazyTankSocketEvent) : void
      {
         var _loc2_:int = param1.pkg.readInt();
         var _loc3_:Boolean = param1.pkg.readBoolean();
         if(_loc3_)
         {
            this.removeMail(this._model.getMailByID(_loc2_));
            this.changeSelected(null);
            MessageTipManager.getInstance().show(LanguageMgr.GetTranslation("tank.manager.MailManager.delete"));
         }
         else
         {
            MessageTipManager.getInstance().show(LanguageMgr.GetTranslation("tank.manager.MailManager.false"));
         }
      }
      
      public function removeMail(param1:EmailInfo) : void
      {
         this._model.removeEmail(param1);
      }
      
      private function __sendEmail(param1:CrazyTankSocketEvent) : void
      {
         if(param1.pkg.readBoolean())
         {
            if(this._view)
            {
               this._view.resetWrite();
            }
            if(this._write)
            {
               this._write.reset();
            }
         }
      }
      
      private function __mailCancel(param1:CrazyTankSocketEvent) : void
      {
         var _loc2_:int = param1.pkg.readInt();
         if(param1.pkg.readBoolean())
         {
            this._model.removeEmail(this._model.selectEmail);
            this.changeSelected(null);
            MessageTipManager.getInstance().show(LanguageMgr.GetTranslation("tank.manager.MailManager.back"));
         }
         else
         {
            MessageTipManager.getInstance().show(LanguageMgr.GetTranslation("tank.manager.MailManager.return"));
         }
      }
      
      private function __responseMail(param1:CrazyTankSocketEvent) : void
      {
         var _loc3_:URLVariables = null;
         var _loc4_:BaseLoader = null;
         var _loc2_:int = param1.pkg.readInt();
         this.emailtype = param1.pkg.readInt();
         if(this.emailtype == 4)
         {
            SocketManager.Instance.out.sendReloadGift();
            this.emailtype = 1;
         }
         this.loadMail(this.emailtype);
         if(this.emailtype != 2)
         {
            MainToolBar.Instance.unReadEmail = true;
         }
         if(this.emailtype == 5)
         {
            _loc3_ = RequestVairableCreater.creatWidthKey(true);
            _loc3_["timeTick"] = Math.random();
            _loc4_ = LoaderManager.Instance.creatLoader(PathManager.solveRequestPath("ShopItemList.xml"),BaseLoader.COMPRESS_TEXT_LOADER,_loc3_);
            _loc4_.analyzer = new ShopItemAnalyzer(ShopManager.Instance.updateShopGoods);
            LoaderManager.Instance.startLoad(_loc4_);
         }
      }
      
      public function calculateRemainTime(param1:String, param2:Number) : Number
      {
         var _loc3_:String = param1;
         var _loc4_:Date = new Date(Number(_loc3_.substr(0,4)),Number(_loc3_.substr(5,2)) - 1,Number(_loc3_.substr(8,2)),Number(_loc3_.substr(11,2)),Number(_loc3_.substr(14,2)),Number(_loc3_.substr(17,2)));
         var _loc5_:Date = TimeManager.Instance.Now();
         var _loc6_:Number = param2 - (_loc5_.time - _loc4_.time) / (60 * 60 * 1000);
         if(_loc6_ < 0)
         {
            return -1;
         }
         return _loc6_;
      }
   }
}
