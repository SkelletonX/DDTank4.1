package littleGame.view
{
   import com.pickgliss.ui.ComponentFactory;
   import com.pickgliss.utils.ObjectUtils;
   import ddt.manager.ChatManager;
   import ddt.view.FaceContainer;
   import ddt.view.chat.ChatEvent;
   import flash.events.Event;
   import littleGame.character.LittleGameCharacter;
   import littleGame.events.LittleLivingEvent;
   import littleGame.model.LittlePlayer;
   
   public class GameLittlePlayer extends GameLittleLiving
   {
       
      
      private var _facecontainer:FaceContainer;
      
      protected var _nameField:PlayerNameField;
      
      public function GameLittlePlayer(param1:LittlePlayer)
      {
         super(param1);
         mouseEnabled = mouseChildren = false;
      }
      
      override protected function addEvent() : void
      {
         super.addEvent();
         ChatManager.Instance.addEventListener(ChatEvent.SHOW_FACE,this.__getFace);
         _living.addEventListener(LittleLivingEvent.HeadChanged,this.__headChanged);
      }
      
      override protected function removeEvent() : void
      {
         super.removeEvent();
         ChatManager.Instance.removeEventListener(ChatEvent.SHOW_FACE,this.__getFace);
         if(this._facecontainer)
         {
            this._facecontainer.removeEventListener(Event.COMPLETE,this.onFaceComplete);
         }
      }
      
      override public function dispose() : void
      {
         super.dispose();
         ObjectUtils.disposeObject(this._nameField);
         this._nameField = null;
         ObjectUtils.disposeObject(this._facecontainer);
         this._facecontainer = null;
      }
      
      private function __getFace(param1:ChatEvent) : void
      {
         var _loc3_:int = 0;
         var _loc4_:int = 0;
         var _loc2_:Object = param1.data;
         if(_loc2_["playerid"] == this.player.playerInfo.ID)
         {
            _loc3_ = _loc2_["faceid"];
            _loc4_ = _loc2_["delay"];
            this.showFace(_loc3_);
         }
      }
      
      private function onFaceComplete(param1:Event) : void
      {
         if(this._facecontainer && contains(this._facecontainer))
         {
            removeChild(this._facecontainer);
         }
      }
      
      private function showFace(param1:int) : void
      {
         if(this._facecontainer == null)
         {
            this._facecontainer = new FaceContainer();
            this._facecontainer.addEventListener(Event.COMPLETE,this.onFaceComplete);
            this._facecontainer.y = -100;
         }
         addChild(this._facecontainer);
         this._facecontainer.scaleX = 1;
         this._facecontainer.setFace(param1);
      }
      
      override protected function configUI() : void
      {
         super.configUI();
         this._nameField = ComponentFactory.Instance.creatCustomObject("littleGame.PlayerName",[this.player.playerInfo]);
         this._nameField.x = -this._nameField.width >> 1;
         addChild(this._nameField);
      }
      
      override protected function createBody() : void
      {
         var _loc1_:LittleGameCharacter = new LittleGameCharacter(this.player.playerInfo);
         _loc1_.soundEnabled = false;
         _loc1_.addEventListener(Event.COMPLETE,this.onComplete);
         _body = addChild(_loc1_);
         __directionChanged(null);
         if(_living.currentAction)
         {
            LittleGameCharacter(_body).doAction(_living.currentAction);
         }
         else
         {
            LittleGameCharacter(_body).doAction("stand");
         }
      }
      
      private function onComplete(param1:Event) : void
      {
         var _loc2_:LittleGameCharacter = null;
         _loc2_ = param1.currentTarget as LittleGameCharacter;
         _loc2_.removeEventListener(Event.COMPLETE,this.onComplete);
         _loc2_.x = -_loc2_.registerPoint.x;
         _loc2_.y = -_loc2_.registerPoint.y;
         __directionChanged(null);
      }
      
      override protected function centerBody() : void
      {
         var _loc1_:LittleGameCharacter = _body as LittleGameCharacter;
         if(_body && _loc1_)
         {
            _body.x = _body.scaleX == 1?Number(-_loc1_.registerPoint.x):Number(_loc1_.registerPoint.x);
         }
      }
      
      override protected function __doAction(param1:LittleLivingEvent) : void
      {
         if(_body)
         {
            LittleGameCharacter(_body).doAction(_living.currentAction);
         }
      }
      
      protected function __headChanged(param1:LittleLivingEvent) : void
      {
         LittleGameCharacter(_body).setFunnyHead(int(param1.paras[0]));
      }
      
      protected function get player() : LittlePlayer
      {
         return _living as LittlePlayer;
      }
   }
}
