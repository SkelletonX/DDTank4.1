package email.view
{
   import bagAndInfo.BagAndGiftFrame;
   import bagAndInfo.BagAndInfoManager;
   import baglocked.BaglockedManager;
   import com.pickgliss.events.FrameEvent;
   import com.pickgliss.toplevel.StageReferance;
   import com.pickgliss.ui.AlertManager;
   import com.pickgliss.ui.ComponentFactory;
   import com.pickgliss.ui.LayerManager;
   import com.pickgliss.ui.controls.BaseButton;
   import com.pickgliss.ui.controls.Frame;
   import com.pickgliss.ui.controls.SelectedButton;
   import com.pickgliss.ui.controls.SelectedButtonGroup;
   import com.pickgliss.ui.controls.TextButton;
   import com.pickgliss.ui.controls.alert.BaseAlerFrame;
   import com.pickgliss.ui.controls.container.HBox;
   import com.pickgliss.ui.image.MovieImage;
   import com.pickgliss.ui.image.ScaleFrameImage;
   import com.pickgliss.ui.text.FilterFrameText;
   import com.pickgliss.ui.text.TextArea;
   import com.pickgliss.utils.ClassUtils;
   import com.pickgliss.utils.ObjectUtils;
   import ddt.data.EquipType;
   import ddt.data.player.PlayerInfo;
   import ddt.events.PlayerPropertyEvent;
   import ddt.manager.KeyboardShortcutsManager;
   import ddt.manager.LanguageMgr;
   import ddt.manager.MessageTipManager;
   import ddt.manager.PathManager;
   import ddt.manager.PlayerManager;
   import ddt.manager.SocketManager;
   import ddt.manager.SoundManager;
   import ddt.manager.TimeManager;
   import ddt.utils.PositionUtils;
   import ddt.view.character.CharactoryFactory;
   import ddt.view.character.RoomCharacter;
   import ddt.view.common.LevelIcon;
   import email.data.EmailInfo;
   import email.data.EmailInfoOfSended;
   import email.data.EmailState;
   import email.data.EmailType;
   import email.manager.MailManager;
   import feedback.FeedbackManager;
   import feedback.data.FeedbackInfo;
   import flash.display.Bitmap;
   import flash.display.MovieClip;
   import flash.display.Sprite;
   import flash.events.MouseEvent;
   import flash.geom.Point;
   import im.IMController;
   import road7th.data.DictionaryData;
   import road7th.utils.DateUtils;
   import socialContact.friendBirthday.FriendBirthdayManager;
   
   public class ReadingView extends Frame
   {
       
      
      private var _info:EmailInfo;
      
      private var _readOnly:Boolean;
      
      private var _isCanReply:Boolean;
      
      private var _readViewBg:MovieClip;
      
      private var _prompt:Bitmap;
      
      private var _sender:FilterFrameText;
      
      private var _topic:FilterFrameText;
      
      private var _content:TextArea;
      
      private var _personalImgBg:MovieImage;
      
      private var _leftTopBtnGroup:SelectedButtonGroup;
      
      private var _emailListButton:SelectedButton;
      
      private var _noReadButton:SelectedButton;
      
      private var _sendedButton:SelectedButton;
      
      private var _leftPageBtn:BaseButton;
      
      private var _rightPageBtn:BaseButton;
      
      private var _pageTxt:FilterFrameText;
      
      private var _selectAllBtn:BaseButton;
      
      private var _deleteBtn:BaseButton;
      
      private var _reciveMailBtn:BaseButton;
      
      private var _reBack_btn:TextButton;
      
      private var _reply_btn:TextButton;
      
      private var _close_btn:TextButton;
      
      private var _write_btn:TextButton;
      
      private var _help_btn:BaseButton;
      
      private var _helpPage:Frame;
      
      private var _helpPageCloseBtn:TextButton;
      
      private var _diamonds:Array;
      
      private var _list:EmailListView;
      
      private var _diamondHBox:HBox;
      
      private var _addFriend:BaseButton;
      
      private var _titleTop:ScaleFrameImage;
      
      private var _rebackGiftBtn:BaseButton;
      
      private var _presentGiftBtn:BaseButton;
      
      private var _playerview:RoomCharacter;
      
      private var _levelIcon:LevelIcon;
      
      private var _tempInfo:PlayerInfo;
      
      private var _PromptTxt:FilterFrameText;
      
      private const _PRESENTGIFT:int = 16;
      
      private var _complainBtn:BaseButton;
      
      private var _complainAlert:BaseAlerFrame;
      
      public function ReadingView()
      {
         super();
         this.initView();
         this.addEvent();
      }
      
      private function initView() : void
      {
         this._readViewBg = ClassUtils.CreatInstance("asset.email.readViewBg");
         PositionUtils.setPos(this._readViewBg,"readingViewBG.pos");
         addToContent(this._readViewBg);
         this.addLeftTopBtnGroup();
         this._leftPageBtn = ComponentFactory.Instance.creat("email.leftPageBtn");
         addToContent(this._leftPageBtn);
         this._leftPageBtn.enable = false;
         this._rightPageBtn = ComponentFactory.Instance.creat("email.rightPageBtn");
         addToContent(this._rightPageBtn);
         this._rightPageBtn.enable = false;
         this._pageTxt = ComponentFactory.Instance.creat("email.pageTxt");
         this._pageTxt.text = "1/1";
         addToContent(this._pageTxt);
         this._selectAllBtn = ComponentFactory.Instance.creat("email.selectAllBtn");
         addToContent(this._selectAllBtn);
         this._deleteBtn = ComponentFactory.Instance.creat("email.deleteBtn");
         addToContent(this._deleteBtn);
         this._reciveMailBtn = ComponentFactory.Instance.creat("email.reciveMailBtn");
         addToContent(this._reciveMailBtn);
         this._titleTop = ComponentFactory.Instance.creatComponentByStylename("email.titleTopAsset");
         addToContent(this._titleTop);
         this._titleTop.setFrame(1);
         this._prompt = ComponentFactory.Instance.creatBitmap("asset.email.prompt");
         addToContent(this._prompt);
         this._prompt.visible = false;
         this._sender = ComponentFactory.Instance.creat("email.senderTxt");
         this._sender.maxChars = 36;
         addToContent(this._sender);
         this._topic = ComponentFactory.Instance.creat("email.topicTxt");
         this._topic.maxChars = 22;
         addToContent(this._topic);
         this._content = ComponentFactory.Instance.creatComponentByStylename("email.content");
         addToContent(this._content);
         this._diamondHBox = ComponentFactory.Instance.creat("emial.diamondHbox");
         addToContent(this._diamondHBox);
         this._diamonds = new Array();
         var _loc1_:uint = 0;
         while(_loc1_ < 5)
         {
            this._diamonds[_loc1_] = new DiamondOfReading();
            this._diamonds[_loc1_].index = _loc1_;
            this._diamondHBox.addChild(this._diamonds[_loc1_]);
            _loc1_++;
         }
         this._diamondHBox.refreshChildPos();
         this._reBack_btn = ComponentFactory.Instance.creat("email.reBackBtn");
         this._reBack_btn.text = LanguageMgr.GetTranslation("reBack_btn.label");
         addToContent(this._reBack_btn);
         this._reply_btn = ComponentFactory.Instance.creat("email.replyBtn");
         this._reply_btn.text = LanguageMgr.GetTranslation("reply_btn.label");
         addToContent(this._reply_btn);
         this._write_btn = ComponentFactory.Instance.creat("email.writeBtn");
         this._write_btn.text = LanguageMgr.GetTranslation("write_btn.label");
         addToContent(this._write_btn);
         this._close_btn = ComponentFactory.Instance.creat("email.closeBtn");
         addToContent(this._close_btn);
         this._close_btn.text = LanguageMgr.GetTranslation("cancel");
         this._help_btn = ComponentFactory.Instance.creat("email.helpPageBtn");
         addToContent(this._help_btn);
         this._list = ComponentFactory.Instance.creat("email.emailListView");
         addToContent(this._list);
         this.isCanReply = false;
         this._personalImgBg = ComponentFactory.Instance.creat("emial.personalImgBg");
         addToContent(this._personalImgBg);
         this._personalImgBg.visible = false;
         this._addFriend = ComponentFactory.Instance.creatComponentByStylename("email.addFriendBtn");
         this._addFriend.enable = false;
         addToContent(this._addFriend);
         this._rebackGiftBtn = ComponentFactory.Instance.creatComponentByStylename("email.rebackGiftBtn");
         addToContent(this._rebackGiftBtn);
         this._rebackGiftBtn.visible = false;
         this._presentGiftBtn = ComponentFactory.Instance.creatComponentByStylename("email.giveGiftBtn");
         addToContent(this._presentGiftBtn);
         this._presentGiftBtn.visible = false;
         if(PathManager.solveFeedbackEnable())
         {
            this._complainBtn = ComponentFactory.Instance.creatComponentByStylename("email.complainbtn");
            addToContent(this._complainBtn);
            this._complainBtn.visible = false;
         }
      }
      
      private function addLeftTopBtnGroup() : void
      {
         this._leftTopBtnGroup = new SelectedButtonGroup();
         this._emailListButton = ComponentFactory.Instance.creat("emailListBtn");
         this._leftTopBtnGroup.addSelectItem(this._emailListButton);
         addToContent(this._emailListButton);
         this._noReadButton = ComponentFactory.Instance.creat("noReadBtn");
         this._leftTopBtnGroup.addSelectItem(this._noReadButton);
         addToContent(this._noReadButton);
         this._sendedButton = ComponentFactory.Instance.creat("sendedBtn");
         this._leftTopBtnGroup.addSelectItem(this._sendedButton);
         addToContent(this._sendedButton);
         this._leftTopBtnGroup.selectIndex = 0;
      }
      
      private function addEvent() : void
      {
         addEventListener(FrameEvent.RESPONSE,this.__responseHandler);
         this._emailListButton.addEventListener(MouseEvent.CLICK,this.__selectMailTypeListener);
         this._noReadButton.addEventListener(MouseEvent.CLICK,this.__selectMailTypeListener);
         this._sendedButton.addEventListener(MouseEvent.CLICK,this.__selectMailTypeListener);
         this._leftPageBtn.addEventListener(MouseEvent.CLICK,this.__lastPage);
         this._rightPageBtn.addEventListener(MouseEvent.CLICK,this.__nextPage);
         this._selectAllBtn.addEventListener(MouseEvent.CLICK,this.__selectAllListener);
         this._deleteBtn.addEventListener(MouseEvent.CLICK,this.__deleteSelectListener);
         this._reciveMailBtn.addEventListener(MouseEvent.CLICK,this.__receiveExListener);
         this._reBack_btn.addEventListener(MouseEvent.CLICK,this.__backEmail);
         this._reply_btn.addEventListener(MouseEvent.CLICK,this.__reply);
         this._close_btn.addEventListener(MouseEvent.CLICK,this.__close);
         this._write_btn.addEventListener(MouseEvent.CLICK,this.__write);
         this._help_btn.addEventListener(MouseEvent.CLICK,this.__help);
         this._addFriend.addEventListener(MouseEvent.CLICK,this.__addFriend);
         this._rebackGiftBtn.addEventListener(MouseEvent.CLICK,this.__rebackGift);
         this._presentGiftBtn.addEventListener(MouseEvent.CLICK,this._clickPresent);
         if(PathManager.solveFeedbackEnable())
         {
            this._complainBtn.addEventListener(MouseEvent.CLICK,this.__complainhandler);
         }
      }
      
      private function removeEvent() : void
      {
         removeEventListener(FrameEvent.RESPONSE,this.__responseHandler);
         this._emailListButton.removeEventListener(MouseEvent.CLICK,this.__selectMailTypeListener);
         this._noReadButton.removeEventListener(MouseEvent.CLICK,this.__selectMailTypeListener);
         this._sendedButton.removeEventListener(MouseEvent.CLICK,this.__selectMailTypeListener);
         this._leftPageBtn.removeEventListener(MouseEvent.CLICK,this.__lastPage);
         this._rightPageBtn.removeEventListener(MouseEvent.MOUSE_DOWN,this.__nextPage);
         this._selectAllBtn.removeEventListener(MouseEvent.CLICK,this.__selectAllListener);
         this._deleteBtn.removeEventListener(MouseEvent.CLICK,this.__deleteSelectListener);
         this._reciveMailBtn.removeEventListener(MouseEvent.CLICK,this.__receiveExListener);
         this._reBack_btn.removeEventListener(MouseEvent.CLICK,this.__backEmail);
         this._reply_btn.removeEventListener(MouseEvent.CLICK,this.__reply);
         this._close_btn.removeEventListener(MouseEvent.CLICK,this.__close);
         this._write_btn.removeEventListener(MouseEvent.CLICK,this.__write);
         this._help_btn.removeEventListener(MouseEvent.CLICK,this.__help);
         this._addFriend.removeEventListener(MouseEvent.CLICK,this.__addFriend);
         this._rebackGiftBtn.removeEventListener(MouseEvent.CLICK,this.__rebackGift);
         this._presentGiftBtn.removeEventListener(MouseEvent.CLICK,this._clickPresent);
         if(this._helpPageCloseBtn)
         {
            this._helpPageCloseBtn.removeEventListener(MouseEvent.CLICK,this.__helpPageClose);
            this._helpPage.removeEventListener(FrameEvent.RESPONSE,this.__helpResponseHandler);
         }
         if(PathManager.solveFeedbackEnable())
         {
            this._complainBtn.addEventListener(MouseEvent.CLICK,this.__complainhandler);
         }
      }
      
      private function __complainhandler(param1:MouseEvent) : void
      {
         SoundManager.instance.play("008");
         this._complainAlert = AlertManager.Instance.simpleAlert(LanguageMgr.GetTranslation("AlertDialog.Info"),LanguageMgr.GetTranslation("email.complain.confim"),LanguageMgr.GetTranslation("ok"),LanguageMgr.GetTranslation("cancel"),false,false,false,LayerManager.BLCAK_BLOCKGOUND);
         this._complainAlert.addEventListener(FrameEvent.RESPONSE,this.__frameResponse);
      }
      
      protected function __frameResponse(param1:FrameEvent) : void
      {
         var _loc2_:FeedbackInfo = null;
         switch(param1.responseCode)
         {
            case FrameEvent.ENTER_CLICK:
            case FrameEvent.SUBMIT_CLICK:
               if(FeedbackManager.instance.examineTime())
               {
                  _loc2_ = new FeedbackInfo();
                  _loc2_.question_title = LanguageMgr.GetTranslation("email.complain.lan");
                  _loc2_.question_content = this._info.Content;
                  _loc2_.occurrence_date = DateUtils.dateFormat(new Date());
                  _loc2_.question_type = 8;
                  _loc2_.report_user_name = this._info.Sender;
                  FeedbackManager.instance.submitFeedbackInfo(_loc2_);
               }
               else
               {
                  MessageTipManager.getInstance().show(LanguageMgr.GetTranslation("feedback.view.SystemsAnalysis"));
               }
         }
         this._complainAlert.removeEventListener(FrameEvent.RESPONSE,this.__frameResponse);
         this._complainAlert.dispose();
         this._complainAlert = null;
      }
      
      public function set info(param1:EmailInfo) : void
      {
         this._info = param1;
         if(this._info is EmailInfoOfSended)
         {
            this.updateSended();
            return;
         }
         this.update();
         if(this._info && (this._info.Type == 1 || this._info.Type == 101 || this._info.Type == 10))
         {
            IMController.Instance.saveRecentContactsID(this._info.SenderID);
         }
      }
      
      private function updateSended() : void
      {
         this._prompt.visible = false;
         var _loc1_:EmailInfoOfSended = this._info as EmailInfoOfSended;
         if(_loc1_.Type == EmailType.CONSORTION_EMAIL)
         {
            this._sender.text = LanguageMgr.GetTranslation("tank.view.common.ConsortiaIcon.self");
         }
         else
         {
            this._sender.text = Boolean(_loc1_)?_loc1_.Receiver:"";
         }
         this._topic.text = Boolean(_loc1_)?_loc1_.Title:"";
         this._content.text = Boolean(_loc1_)?_loc1_.Content:"";
         this._content.textField.text = this._content.textField.text + ("\n" + _loc1_.AnnexRemark);
         this._list.updateInfo(this._info);
      }
      
      private function update() : void
      {
         var _loc1_:DiamondOfReading = null;
         if(this._info && (this._info.Type == 0 || this._info.Type == 6 || this._info.Type == 1 || this._info.Type == 7 || this._info.Type == 10 || this._info.Type > 100 || this._info.Type == 59))
         {
            if(this._info.Sender != PlayerManager.Instance.Self.NickName)
            {
               this._addFriend.enable = true;
            }
            this._prompt.visible = true;
         }
         else
         {
            this._prompt.visible = false;
            this._addFriend.enable = false;
         }
         if(this._info && (this._info.ReceiverID != this._info.SenderID && this._info.Type == 1 || this._info.Type == 59 || this._info.Type == 101))
         {
            if(PathManager.solveFeedbackEnable())
            {
               this._complainBtn.visible = true;
            }
         }
         else if(PathManager.solveFeedbackEnable())
         {
            this._complainBtn.visible = false;
         }
         this._sender.text = Boolean(this._info)?this._info.Sender:"";
         this._topic.text = Boolean(this._info)?this._info.Title:"";
         this._content.text = Boolean(this._info)?this._info.Content:"";
         this._personalImgBg.visible = false;
         this.clearPersonalImage();
         if(this._info)
         {
            this.prepareShow();
         }
         for each(_loc1_ in this._diamonds)
         {
            _loc1_.info = this._info;
         }
         this._list.updateInfo(this._info);
         this.upRebackGift();
         this._upPresentGift();
      }
      
      private function upRebackGift() : void
      {
         if(this._info)
         {
            if(this._info.MailType == 1 && this._info.Type != EmailType.GIFT_GUIDE && this._info.Type != EmailType.MYSELF_BRITHDAY)
            {
               this._rebackGiftBtn.visible = true;
               if(PlayerManager.Instance.Self.Grade >= 16)
               {
                  this._rebackGiftBtn.enable = true;
               }
               else
               {
                  this._rebackGiftBtn.enable = false;
               }
            }
            else
            {
               this._rebackGiftBtn.visible = false;
            }
         }
         else
         {
            this._rebackGiftBtn.visible = false;
         }
      }
      
      private function _upPresentGift() : void
      {
         if(this._info && this._info.MailType == 0 && this._info.Type == EmailType.FRIEND_BRITHDAY)
         {
            this._presentGiftBtn.visible = true;
         }
         else
         {
            this._presentGiftBtn.visible = false;
         }
         if(PlayerManager.Instance.Self.Grade >= this._PRESENTGIFT)
         {
            this._presentGiftBtn.enable = true;
         }
         else
         {
            this._presentGiftBtn.enable = false;
         }
      }
      
      private function clearPersonalImage() : void
      {
         this._tempInfo = null;
         if(this._playerview)
         {
            this._playerview.dispose();
            this._playerview = null;
         }
         if(this._levelIcon)
         {
            this._levelIcon.dispose();
            this._levelIcon = null;
         }
      }
      
      private function prepareShow() : void
      {
         this._tempInfo = PlayerManager.Instance.findPlayer(this._info.UserID,PlayerManager.Instance.Self.ZoneID);
         if(this._info.Money > 0 && this._info.UserID != PlayerManager.Instance.Self.ID && this._info.UserID != 0)
         {
            this._personalImgBg.visible = true;
            if(!PlayerManager.Instance.hasInFriendList(this._info.UserID) && !PlayerManager.Instance.hasInClubPlays(this._info.UserID) && !PlayerManager.Instance.hasInMailTempList(this._info.UserID))
            {
               SocketManager.Instance.out.sendItemEquip(this._info.UserID);
               this._tempInfo.addEventListener(PlayerPropertyEvent.PROPERTY_CHANGE,this.showPersonal);
               return;
            }
            this.showBegain();
         }
         else
         {
            this._personalImgBg.visible = false;
         }
      }
      
      private function showPersonal(param1:PlayerPropertyEvent) : void
      {
         var _loc2_:DictionaryData = new DictionaryData();
         this._tempInfo.removeEventListener(PlayerPropertyEvent.PROPERTY_CHANGE,this.showPersonal);
         _loc2_[this._info.UserID] = this._tempInfo;
         PlayerManager.Instance.mailTempList = _loc2_;
         this.showBegain();
      }
      
      private function showBegain() : void
      {
         this._tempInfo.WeaponID = int(this._tempInfo.Style.split(",")[EquipType.ARM - 1].split("|")[0]);
         this._playerview = CharactoryFactory.createCharacter(this._tempInfo,"room") as RoomCharacter;
         this._playerview.showGun = true;
         this._playerview.setShowLight(true,null);
         this._playerview.show(true,-1);
         this._playerview.stopAnimation();
         this.showComplete();
      }
      
      private function showComplete() : void
      {
         PositionUtils.setPos(this._playerview,"email.playerviewPos");
         var _loc1_:Sprite = new Sprite();
         _loc1_.graphics.beginFill(0);
         _loc1_.graphics.drawRect(0,0,124,140);
         _loc1_.graphics.endFill();
         var _loc2_:Point = ComponentFactory.Instance.creatCustomObject("email.playerviewMaskPos");
         _loc1_.x = _loc2_.x;
         _loc1_.y = _loc2_.y;
         this._playerview.mask = _loc1_;
         addToContent(_loc1_);
         addToContent(this._playerview);
         this._levelIcon = ComponentFactory.Instance.creatCustomObject("email.levelIcon");
         this._levelIcon.setSize(LevelIcon.SIZE_BIG);
         this._levelIcon.setInfo(this._tempInfo.Grade,this._tempInfo.Repute,this._tempInfo.WinCount,this._tempInfo.TotalCount,this._tempInfo.FightPower,this._tempInfo.Offer,false);
         this._levelIcon.mouseEnabled = false;
         this._levelIcon.mouseChildren = false;
         this._levelIcon.buttonMode = false;
         addToContent(this._levelIcon);
      }
      
      public function setListView(param1:Array, param2:int, param3:int, param4:Boolean = false) : void
      {
         this._list.update(param1,param4);
         this._pageTxt.text = param3.toString() + "/" + param2.toString();
         this._leftPageBtn.enable = param3 == 0 || param3 == 1?Boolean(false):Boolean(true);
         this._rightPageBtn.enable = param3 == param2?Boolean(false):Boolean(true);
      }
      
      public function switchBtnsVisible(param1:Boolean) : void
      {
         this._selectAllBtn.visible = param1;
         this._deleteBtn.visible = param1;
         this._reciveMailBtn.visible = param1;
      }
      
      private function btnSound() : void
      {
         SoundManager.instance.play("043");
      }
      
      public function set readOnly(param1:Boolean) : void
      {
         var _loc2_:uint = 0;
         while(_loc2_ < 5)
         {
            (this._diamonds[_loc2_] as DiamondOfReading).readOnly = param1;
            (this._diamonds[_loc2_] as DiamondOfReading).visible = !param1;
            _loc2_++;
         }
      }
      
      function set isCanReply(param1:Boolean) : void
      {
         if(this._info is EmailInfoOfSended)
         {
            return;
         }
         this._reply_btn.enable = param1;
         if(this._info)
         {
            if(this._info.Type > 100 && this._info.Money > 0)
            {
               this._reBack_btn.enable = true;
            }
            else
            {
               this._reBack_btn.enable = false;
            }
         }
         else
         {
            this._reBack_btn.enable = false;
         }
      }
      
      private function closeWin() : void
      {
         MailManager.Instance.hide();
      }
      
      public function personalHide() : void
      {
      }
      
      private function createHelpPage() : void
      {
         this._helpPage = ComponentFactory.Instance.creat("email.helpPageFrame");
         this._helpPage.escEnable = true;
         this._helpPage.titleText = LanguageMgr.GetTranslation("tank.view.emailII.ReadingView.useHelp");
         LayerManager.Instance.addToLayer(this._helpPage,LayerManager.GAME_TOP_LAYER,true);
         this._helpPageCloseBtn = ComponentFactory.Instance.creat("email.helpPageCloseBtn");
         this._helpPageCloseBtn.text = LanguageMgr.GetTranslation("close");
         this._helpPage.addToContent(this._helpPageCloseBtn);
         this._helpPageCloseBtn.addEventListener(MouseEvent.CLICK,this.__helpPageClose);
         var _loc1_:Bitmap = ComponentFactory.Instance.creatBitmap("asset.email.helpPageWord");
         this._helpPage.addToContent(_loc1_);
         this._helpPage.visible = false;
         this._helpPage.addEventListener(FrameEvent.RESPONSE,this.__helpResponseHandler);
      }
      
      private function __helpResponseHandler(param1:FrameEvent) : void
      {
         SoundManager.instance.play("008");
         if(param1.responseCode == FrameEvent.CLOSE_CLICK || param1.responseCode == FrameEvent.ESC_CLICK)
         {
            this._helpPage.visible = false;
         }
      }
      
      override public function dispose() : void
      {
         super.dispose();
         this.removeEvent();
         if(this._complainAlert)
         {
            this._complainAlert.removeEventListener(FrameEvent.RESPONSE,this.__frameResponse);
            this._complainAlert.dispose();
         }
         this._complainAlert = null;
         if(this._readViewBg)
         {
            ObjectUtils.disposeObject(this._readViewBg);
         }
         this._readViewBg = null;
         if(this._prompt)
         {
            ObjectUtils.disposeObject(this._prompt);
         }
         this._prompt = null;
         if(this._sender)
         {
            ObjectUtils.disposeObject(this._sender);
         }
         this._sender = null;
         if(this._topic)
         {
            ObjectUtils.disposeObject(this._topic);
         }
         this._topic = null;
         if(this._personalImgBg)
         {
            ObjectUtils.disposeObject(this._personalImgBg);
         }
         this._personalImgBg = null;
         if(this._leftTopBtnGroup)
         {
            ObjectUtils.disposeObject(this._leftTopBtnGroup);
         }
         this._leftTopBtnGroup = null;
         if(this._emailListButton)
         {
            ObjectUtils.disposeObject(this._emailListButton);
         }
         this._emailListButton = null;
         if(this._noReadButton)
         {
            ObjectUtils.disposeObject(this._noReadButton);
         }
         this._noReadButton = null;
         if(this._sendedButton)
         {
            ObjectUtils.disposeObject(this._sendedButton);
         }
         this._sendedButton = null;
         if(this._leftPageBtn)
         {
            ObjectUtils.disposeObject(this._leftPageBtn);
         }
         this._leftPageBtn = null;
         if(this._rightPageBtn)
         {
            ObjectUtils.disposeObject(this._rightPageBtn);
         }
         this._rightPageBtn = null;
         if(this._pageTxt)
         {
            ObjectUtils.disposeObject(this._pageTxt);
         }
         this._pageTxt = null;
         if(this._selectAllBtn)
         {
            ObjectUtils.disposeObject(this._selectAllBtn);
         }
         this._selectAllBtn = null;
         if(this._deleteBtn)
         {
            ObjectUtils.disposeObject(this._deleteBtn);
         }
         this._deleteBtn = null;
         if(this._reciveMailBtn)
         {
            ObjectUtils.disposeObject(this._reciveMailBtn);
         }
         this._reciveMailBtn = null;
         if(this._reBack_btn)
         {
            ObjectUtils.disposeObject(this._reBack_btn);
         }
         this._reBack_btn = null;
         if(this._reply_btn)
         {
            ObjectUtils.disposeObject(this._reply_btn);
         }
         this._reply_btn = null;
         if(this._close_btn)
         {
            ObjectUtils.disposeObject(this._close_btn);
         }
         this._close_btn = null;
         if(this._write_btn)
         {
            ObjectUtils.disposeObject(this._write_btn);
         }
         this._write_btn = null;
         if(this._help_btn)
         {
            ObjectUtils.disposeObject(this._help_btn);
         }
         this._help_btn = null;
         if(this._list)
         {
            ObjectUtils.disposeObject(this._list);
         }
         this._list = null;
         if(this._diamondHBox)
         {
            ObjectUtils.disposeObject(this._diamondHBox);
         }
         this._diamondHBox = null;
         if(this._titleTop)
         {
            ObjectUtils.disposeObject(this._titleTop);
         }
         this._titleTop = null;
         if(this._rebackGiftBtn)
         {
            ObjectUtils.disposeObject(this._rebackGiftBtn);
         }
         this._rebackGiftBtn = null;
         if(this._presentGiftBtn)
         {
            ObjectUtils.disposeObject(this._presentGiftBtn);
         }
         this._presentGiftBtn = null;
         if(this._addFriend)
         {
            ObjectUtils.disposeObject(this._addFriend);
         }
         this._addFriend = null;
         if(this._complainBtn)
         {
            ObjectUtils.disposeObject(this._complainBtn);
         }
         this._complainBtn = null;
         this._info = null;
         this._diamonds = null;
         this.helpPageDispose();
         if(parent)
         {
            parent.removeChild(this);
         }
      }
      
      private function helpPageDispose() : void
      {
         if(this._helpPage)
         {
            if(this._helpPageCloseBtn)
            {
               ObjectUtils.disposeObject(this._helpPageCloseBtn);
            }
            this._helpPageCloseBtn = null;
            this._helpPage.dispose();
            if(this._helpPage && this._helpPage.parent)
            {
               this._helpPage.parent.removeChild(this._helpPage);
            }
            this._helpPage = null;
         }
      }
      
      private function __selectMailTypeListener(param1:MouseEvent) : void
      {
         this._personalImgBg.visible = false;
         this.btnSound();
         if(param1.currentTarget == this._emailListButton)
         {
            this._titleTop.setFrame(1);
            MailManager.Instance.changeType(EmailState.ALL);
         }
         else if(param1.currentTarget == this._noReadButton)
         {
            this._titleTop.setFrame(1);
            MailManager.Instance.changeType(EmailState.NOREAD);
         }
         else
         {
            this._titleTop.setFrame(2);
            MailManager.Instance.changeType(EmailState.SENDED);
         }
      }
      
      private function __lastPage(param1:MouseEvent) : void
      {
         SoundManager.instance.play("045");
         MailManager.Instance.setPage(true,this._list.canChangePage());
         MailManager.Instance.changeSelected(null);
      }
      
      private function __nextPage(param1:MouseEvent) : void
      {
         SoundManager.instance.play("045");
         MailManager.Instance.setPage(false,this._list.canChangePage());
         MailManager.Instance.changeSelected(null);
      }
      
      private function __selectAllListener(param1:MouseEvent) : void
      {
         this.btnSound();
         this._list.switchSeleted();
      }
      
      private function __deleteSelectListener(param1:MouseEvent) : void
      {
         var _loc3_:int = 0;
         var _loc4_:BaseAlerFrame = null;
         this.btnSound();
         if(PlayerManager.Instance.Self.bagLocked)
         {
            BaglockedManager.Instance.show();
            return;
         }
         var _loc2_:Array = this._list.getSelectedMails();
         if(_loc2_.length > 0)
         {
            _loc3_ = 0;
            while(_loc3_ < _loc2_.length)
            {
               if(_loc2_[_loc3_].hasAnnexs() || _loc2_[_loc3_].Money != 0 || _loc2_[_loc3_].Medal != 0 || _loc2_[_loc3_].GiftToken != 0 || _loc2_[_loc3_].Gold != 0)
               {
                  _loc4_ = AlertManager.Instance.simpleAlert(LanguageMgr.GetTranslation("tank.view.task.TaskCatalogContentView.tip"),LanguageMgr.GetTranslation("tank.view.emailII.EmailIIStripView.delectEmail"),LanguageMgr.GetTranslation("ok"),LanguageMgr.GetTranslation("cancel"),false,true,true,LayerManager.ALPHA_BLOCKGOUND);
                  _loc4_.addEventListener(FrameEvent.RESPONSE,this.__simpleAlertResponse);
                  KeyboardShortcutsManager.Instance.prohibitNewHandMail(false);
                  break;
               }
               if(_loc3_ == _loc2_.length - 1)
               {
                  this.ok();
               }
               _loc3_++;
            }
         }
         else
         {
            MessageTipManager.getInstance().show(LanguageMgr.GetTranslation("tank.view.emailII.ReadingView.deleteSelectListener"));
         }
      }
      
      private function __simpleAlertResponse(param1:FrameEvent) : void
      {
         SoundManager.instance.play("008");
         var _loc2_:BaseAlerFrame = param1.currentTarget as BaseAlerFrame;
         _loc2_.removeEventListener(FrameEvent.RESPONSE,this.__simpleAlertResponse);
         ObjectUtils.disposeObject(_loc2_);
         if(_loc2_.parent)
         {
            _loc2_.parent.removeChild(_loc2_);
         }
         if(param1.responseCode == FrameEvent.CANCEL_CLICK || param1.responseCode == FrameEvent.CLOSE_CLICK)
         {
            this.cancel();
         }
         else if(param1.responseCode == FrameEvent.SUBMIT_CLICK || param1.responseCode == FrameEvent.ENTER_CLICK)
         {
            this.ok();
         }
         KeyboardShortcutsManager.Instance.prohibitNewHandMail(true);
      }
      
      private function cancel() : void
      {
         this.btnSound();
      }
      
      private function ok() : void
      {
         this.btnSound();
         this._personalImgBg.visible = false;
         var _loc1_:Array = this._list.getSelectedMails();
         var _loc2_:uint = 0;
         while(_loc2_ < _loc1_.length)
         {
            MailManager.Instance.deleteEmail(_loc1_[_loc2_]);
            MailManager.Instance.removeMail(_loc1_[_loc2_]);
            MailManager.Instance.changeSelected(null);
            _loc2_++;
         }
      }
      
      private function __receiveExListener(param1:MouseEvent) : void
      {
         var _loc3_:uint = 0;
         var _loc4_:EmailInfo = null;
         var _loc5_:String = null;
         var _loc6_:Date = null;
         var _loc7_:String = null;
         var _loc8_:Date = null;
         this.btnSound();
         if(PlayerManager.Instance.Self.bagLocked)
         {
            BaglockedManager.Instance.show();
            return;
         }
         var _loc2_:Array = this._list.getSelectedMails();
         if(_loc2_.length > 0 || this._info)
         {
            if(_loc2_.length > 0)
            {
               _loc3_ = 0;
               while(_loc3_ < _loc2_.length)
               {
                  if(!((_loc2_[_loc3_] as EmailInfo).Type > 100 && (_loc2_[_loc3_] as EmailInfo).Money > 0))
                  {
                     _loc4_ = _loc2_[_loc3_] as EmailInfo;
                     if(!_loc4_.IsRead)
                     {
                        _loc5_ = _loc4_.SendTime;
                        _loc6_ = new Date(Number(_loc5_.substr(0,4)),Number(_loc5_.substr(5,2)) - 1,Number(_loc5_.substr(8,2)),Number(_loc5_.substr(11,2)),Number(_loc5_.substr(14,2)),Number(_loc5_.substr(17,2)));
                        _loc4_.ValidDate = 72 + (TimeManager.Instance.Now().time - _loc6_.time) / (60 * 60 * 1000);
                        _loc4_.IsRead = true;
                        this._list.updateInfo(_loc4_);
                     }
                     MailManager.Instance.getAnnexToBag(_loc2_[_loc3_],0);
                  }
                  _loc3_++;
               }
            }
            if(this._info)
            {
               if(this._info.Type > 100 && this._info.Money > 0)
               {
                  if(this._info.Money > 0)
                  {
                     MessageTipManager.getInstance().show(LanguageMgr.GetTranslation("ddt.email.readingEmail.payEmail"));
                  }
                  return;
               }
               if(!this._info.IsRead)
               {
                  _loc7_ = this._info.SendTime;
                  _loc8_ = new Date(Number(_loc7_.substr(0,4)),Number(_loc7_.substr(5,2)) - 1,Number(_loc7_.substr(8,2)),Number(_loc7_.substr(11,2)),Number(_loc7_.substr(14,2)),Number(_loc7_.substr(17,2)));
                  this._info.ValidDate = 72 + (TimeManager.Instance.Now().time - _loc8_.time) / (60 * 60 * 1000);
               }
               MailManager.Instance.getAnnexToBag(this._info,0);
            }
         }
         else
         {
            MessageTipManager.getInstance().show(LanguageMgr.GetTranslation("tank.view.emailII.ReadingView.deleteSelectListener"));
         }
      }
      
      private function __backEmail(param1:MouseEvent) : void
      {
         this.btnSound();
         MailManager.Instance.untreadEmail(this._info.ID);
      }
      
      private function __reply(param1:MouseEvent) : void
      {
         this.btnSound();
         MailManager.Instance.changeState(EmailState.REPLY);
      }
      
      private function __close(param1:MouseEvent) : void
      {
         this.btnSound();
         this.closeWin();
      }
      
      private function __write(param1:MouseEvent) : void
      {
         this.btnSound();
         if(this._helpPage)
         {
            this._helpPage.visible = false;
         }
         MailManager.Instance.changeState(EmailState.WRITE);
      }
      
      private function __addFriend(param1:MouseEvent) : void
      {
         if(this._info)
         {
            IMController.Instance.addFriend(this._info.Sender);
         }
         SoundManager.instance.play("008");
      }
      
      private function __help(param1:MouseEvent) : void
      {
         SoundManager.instance.play("008");
         param1.stopImmediatePropagation();
         if(!this._helpPage)
         {
            this.createHelpPage();
         }
         StageReferance.stage.focus = this._helpPage;
         this._helpPage.visible = !!this._helpPage.visible?Boolean(false):Boolean(true);
      }
      
      private function __helpPageClose(param1:MouseEvent) : void
      {
         SoundManager.instance.play("008");
         this._helpPage.visible = false;
      }
      
      private function __responseHandler(param1:FrameEvent) : void
      {
         if(param1.responseCode == FrameEvent.CLOSE_CLICK)
         {
            this.btnSound();
            this.closeWin();
         }
      }
      
      protected function __rebackGift(param1:MouseEvent) : void
      {
         param1.stopImmediatePropagation();
         SoundManager.instance.play("008");
         BagAndInfoManager.Instance.showBagAndInfo(BagAndGiftFrame.GIFTVIEW,this._sender.text);
         MailManager.Instance.hide();
      }
      
      private function _clickPresent(param1:MouseEvent) : void
      {
         var _loc2_:String = this._info.Content;
         _loc2_ = _loc2_.substring(_loc2_.search(/\[/) + 1,_loc2_.search("]"));
         FriendBirthdayManager.Instance.friendName = _loc2_;
         param1.stopImmediatePropagation();
         SoundManager.instance.play("008");
         BagAndInfoManager.Instance.showBagAndInfo(BagAndGiftFrame.GIFTVIEW,_loc2_);
         MailManager.Instance.hide();
      }
   }
}
