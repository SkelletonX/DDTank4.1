package email.model
{
   import ddt.manager.PlayerManager;
   import ddt.manager.SharedManager;
   import ddt.manager.TimeManager;
   import email.data.EmailInfo;
   import email.data.EmailInfoOfSended;
   import email.data.EmailState;
   import email.manager.MailManager;
   import email.view.EmailEvent;
   import flash.events.EventDispatcher;
   import flash.events.IEventDispatcher;
   
   public class EmailModel extends EventDispatcher
   {
       
      
      public var isLoaded:Boolean = false;
      
      private var _sendedMails:Array;
      
      private var _noReadMails:Array;
      
      private var _emails:Array;
      
      private var _mailType:String = "all mails";
      
      private var _currentDate:Array;
      
      private var _state:String = "read";
      
      private var _currentPage:int = 1;
      
      private var _selectEmail:EmailInfo;
      
      public function EmailModel(param1:IEventDispatcher = null)
      {
         this._sendedMails = [];
         this._emails = [];
         super(param1);
      }
      
      public function set sendedMails(param1:Array) : void
      {
         this._sendedMails = param1;
         if(this._mailType == EmailState.SENDED)
         {
            dispatchEvent(new EmailEvent(EmailEvent.INIT_EMAIL));
         }
      }
      
      public function get sendedMails() : Array
      {
         return this._sendedMails;
      }
      
      public function get noReadMails() : Array
      {
         return this._noReadMails;
      }
      
      public function get emails() : Array
      {
         return this._emails.slice(0);
      }
      
      public function set emails(param1:Array) : void
      {
         var _loc3_:Number = NaN;
         this._emails = [];
         var _loc2_:int = 0;
         while(_loc2_ < param1.length)
         {
            _loc3_ = this.calculateRemainTime(param1[_loc2_].SendTime,param1[_loc2_].ValidDate);
            if(_loc3_ > -1)
            {
               this._emails.push(param1[_loc2_]);
            }
            _loc2_++;
         }
         this.getNoReadMails();
         this.isLoaded = true;
         dispatchEvent(new EmailEvent(EmailEvent.INIT_EMAIL));
      }
      
      public function getValidateMails(param1:Array) : Array
      {
         var _loc4_:EmailInfo = null;
         var _loc2_:Array = [];
         var _loc3_:int = 0;
         while(_loc3_ < param1.length)
         {
            _loc4_ = param1[_loc3_] as EmailInfo;
            if(_loc4_)
            {
               if(MailManager.Instance.calculateRemainTime(_loc4_.SendTime,_loc4_.ValidDate) > 0)
               {
                  _loc2_.push(_loc4_);
               }
            }
            _loc3_++;
         }
         return _loc2_;
      }
      
      public function set mailType(param1:String) : void
      {
         this._mailType = param1;
         this.resetModel();
         dispatchEvent(new EmailEvent(EmailEvent.CHANGE_TYPE));
      }
      
      public function get mailType() : String
      {
         return this._mailType;
      }
      
      public function get currentDate() : Array
      {
         switch(this._mailType)
         {
            case EmailState.ALL:
               this._currentDate = this._emails;
               break;
            case EmailState.NOREAD:
               this._currentDate = this._noReadMails;
               break;
            case EmailState.SENDED:
               this._currentDate = this._sendedMails;
               break;
            default:
               this._currentDate = this._emails;
         }
         return this._currentDate;
      }
      
      public function set state(param1:String) : void
      {
         this._state = param1;
         dispatchEvent(new EmailEvent(EmailEvent.CHANE_STATE));
      }
      
      public function get state() : String
      {
         return this._state;
      }
      
      private function resetModel() : void
      {
         this._currentPage = 1;
         this.selectEmail = null;
      }
      
      public function get totalPage() : int
      {
         if(this.currentDate)
         {
            if(this.currentDate.length == 0)
            {
               return 1;
            }
            return Math.ceil(this.currentDate.length / 7);
         }
         return 1;
      }
      
      public function get currentPage() : int
      {
         if(this._currentPage > this.totalPage)
         {
            this._currentPage = this.totalPage;
         }
         return this._currentPage;
      }
      
      public function set currentPage(param1:int) : void
      {
         this._currentPage = param1;
         dispatchEvent(new EmailEvent(EmailEvent.CHANE_PAGE));
      }
      
      public function getNoReadMails() : void
      {
         var _loc1_:EmailInfo = null;
         this._noReadMails = [];
         for each(_loc1_ in this._emails)
         {
            if(SharedManager.Instance.spacialReadedMail[PlayerManager.Instance.Self.ID] && SharedManager.Instance.spacialReadedMail[PlayerManager.Instance.Self.ID].indexOf(_loc1_.ID) > -1)
            {
               _loc1_.IsRead = true;
            }
            if(!_loc1_.IsRead)
            {
               this._noReadMails.push(_loc1_);
            }
         }
      }
      
      public function getMailByID(param1:int) : EmailInfo
      {
         var _loc2_:int = this._emails.length;
         var _loc3_:uint = 0;
         while(_loc3_ < _loc2_)
         {
            if((this._emails[_loc3_] as EmailInfo).ID == param1)
            {
               return this._emails[_loc3_] as EmailInfo;
            }
            _loc3_++;
         }
         return null;
      }
      
      public function getViewData() : Array
      {
         var _loc2_:int = 0;
         var _loc3_:int = 0;
         if(this._mailType == EmailState.NOREAD)
         {
            this.getNoReadMails();
         }
         var _loc1_:Array = new Array();
         if(this.currentDate)
         {
            _loc2_ = this.currentPage * 7 - 7;
            _loc3_ = _loc2_ + 7 > this.currentDate.length?int(this.currentDate.length):int(_loc2_ + 7);
            _loc1_ = this.currentDate.slice(_loc2_,_loc3_);
         }
         return _loc1_;
      }
      
      private function calculateRemainTime(param1:String, param2:Number) : Number
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
      
      public function get selectEmail() : EmailInfo
      {
         return this._selectEmail;
      }
      
      public function set selectEmail(param1:EmailInfo) : void
      {
         if(param1)
         {
            if(this._emails.indexOf(param1) <= -1 && this._sendedMails.indexOf(param1) <= -1)
            {
               this._selectEmail = null;
            }
            else
            {
               this._selectEmail = param1;
            }
         }
         else
         {
            this._selectEmail = null;
         }
         dispatchEvent(new EmailEvent(EmailEvent.SELECT_EMAIL,this._selectEmail));
      }
      
      public function addEmail(param1:EmailInfo) : void
      {
         this._emails.push(param1);
         dispatchEvent(new EmailEvent(EmailEvent.ADD_EMAIL,param1));
      }
      
      public function addEmailToSended(param1:EmailInfoOfSended) : void
      {
         this._sendedMails.unshift(param1);
         if(this._sendedMails.length > 21)
         {
            this._sendedMails.pop();
         }
      }
      
      public function removeFromNoRead(param1:EmailInfo) : void
      {
         var _loc2_:int = this._noReadMails.indexOf(param1);
         if(_loc2_ > -1)
         {
            this._noReadMails.splice(_loc2_,1);
         }
      }
      
      public function removeEmail(param1:EmailInfo) : void
      {
         var _loc2_:int = this._emails.indexOf(param1);
         if(_loc2_ > -1)
         {
            this._emails.splice(_loc2_,1);
            this.getNoReadMails();
            dispatchEvent(new EmailEvent(EmailEvent.REMOVE_EMAIL,param1));
         }
      }
      
      public function changeEmail(param1:EmailInfo) : void
      {
         var _loc2_:int = this._emails.indexOf(param1);
         param1.IsRead = true;
         if(_loc2_ > -1)
         {
            dispatchEvent(new EmailEvent(EmailEvent.SELECT_EMAIL,param1));
         }
      }
      
      public function clearEmail() : void
      {
         this._emails = new Array();
         dispatchEvent(new EmailEvent(EmailEvent.CLEAR_EMAIL));
      }
      
      public function dispose() : void
      {
         this._emails = new Array();
      }
      
      public function hasUnReadEmail() : Boolean
      {
         var _loc1_:EmailInfo = null;
         for each(_loc1_ in this._emails)
         {
            if(!_loc1_.IsRead)
            {
               return true;
            }
         }
         return false;
      }
      
      public function hasUnReadGiftEmail() : Boolean
      {
         var _loc1_:EmailInfo = null;
         for each(_loc1_ in this._emails)
         {
            if(!_loc1_.IsRead && _loc1_.MailType == 1)
            {
               return true;
            }
         }
         return false;
      }
   }
}
