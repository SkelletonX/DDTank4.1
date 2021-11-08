package littleGame.object
{
   import com.greensock.TweenLite;
   import com.greensock.easing.Bounce;
   import com.pickgliss.toplevel.StageReferance;
   import com.pickgliss.ui.ComponentFactory;
   import com.pickgliss.utils.ObjectUtils;
   import ddt.manager.ChatManager;
   import flash.display.DisplayObject;
   import flash.display.DisplayObjectContainer;
   import flash.display.Graphics;
   import flash.display.MovieClip;
   import flash.display.Sprite;
   import flash.events.MouseEvent;
   import flash.events.TimerEvent;
   import flash.geom.Point;
   import flash.utils.Dictionary;
   import flash.utils.Timer;
   import littleGame.LittleGameManager;
   import littleGame.actions.LittleLivingDieAction;
   import littleGame.data.LittleObjectType;
   import littleGame.interfaces.ILittleObject;
   import littleGame.model.LittleLiving;
   import littleGame.model.LittleSelf;
   import littleGame.model.Scenario;
   import littleGame.view.GameLittleLiving;
   import littleGame.view.GameScene;
   import littleGame.view.MarkShape;
   import road7th.comm.PackageIn;
   
   public class NormalBoguInhaled extends Sprite implements ILittleObject
   {
      
      public static var NoteCount:int;
      
      private static const MaxNoteCount:int = 3;
      
      private static var littleObjectCount:int = 0;
       
      
      private var _id:int;
      
      protected var _giveup:MovieClip;
      
      protected var _giveupAni:MovieClip;
      
      protected var _scene:Scenario;
      
      protected var _target:LittleLiving;
      
      protected var _self:LittleSelf;
      
      protected var _totalClick:int = 20;
      
      protected var _totalScore:int = 1000;
      
      protected var _clickScore:int;
      
      protected var _clickCount:int = 0;
      
      protected var _time:int;
      
      protected var _score:int;
      
      protected var _timer:Timer;
      
      protected var _inhaleAsset:MovieClip;
      
      protected var _gameLivings:Dictionary;
      
      protected var _markBar:MarkShape;
      
      protected var _running:Boolean = true;
      
      protected var _removed:Boolean = false;
      
      private var _mouseNote:DisplayObject;
      
      public function NormalBoguInhaled()
      {
         super();
         this._id = littleObjectCount++;
         mouseChildren = false;
      }
      
      public function get id() : int
      {
         return this._id;
      }
      
      public function get type() : String
      {
         return LittleObjectType.NormalBoguInhaled;
      }
      
      override public function toString() : String
      {
         var _loc1_:String = "[NormalBoguInhaled:(";
         return _loc1_ + ")]";
      }
      
      public function initialize(param1:Scenario, param2:PackageIn) : void
      {
         var _loc6_:int = 0;
         var _loc7_:GameLittleLiving = null;
         this._scene = param1;
         this._id = param2.readInt();
         this._self = this._scene.findLiving(param2.readInt()) as LittleSelf;
         this._target = this._scene.findLiving(param2.readInt());
         this._totalClick = param2.readInt();
         this._totalScore = param2.readInt();
         this._clickScore = param2.readInt();
         this._time = param2.readInt();
         var _loc3_:int = param2.readInt();
         var _loc4_:GameScene = LittleGameManager.Instance.gameScene;
         this._gameLivings = new Dictionary();
         this._gameLivings[this._target.id] = _loc4_.findGameLiving(this._target.id);
         var _loc5_:int = 0;
         while(_loc5_ < _loc3_)
         {
            _loc6_ = param2.readInt();
            _loc7_ = _loc4_.findGameLiving(_loc6_);
            if(_loc7_)
            {
               this._gameLivings[_loc6_] = _loc7_;
            }
            _loc5_++;
         }
         this.drawInhaleAsset();
         this.execute();
      }
      
      protected function drawInhaleAsset() : void
      {
      }
      
      protected function lockLivings() : void
      {
         var _loc1_:Array = null;
         var _loc2_:GameLittleLiving = null;
         var _loc3_:DisplayObjectContainer = null;
         var _loc4_:* = null;
         var _loc5_:int = 0;
         var _loc6_:Point = null;
         _loc1_ = new Array();
         for(_loc4_ in this._gameLivings)
         {
            _loc2_ = this._gameLivings[_loc4_];
            if(_loc2_ && _loc2_.parent && _loc2_.inGame)
            {
               _loc2_.lock = true;
               _loc6_ = _loc2_.parent.localToGlobal(new Point(_loc2_.living.dx * _loc2_.living.speed,_loc2_.living.dy * _loc2_.living.speed));
               _loc6_ = globalToLocal(_loc6_);
               _loc2_.x = _loc6_.x;
               _loc2_.y = _loc6_.y;
               _loc1_.push(_loc2_);
            }
         }
         _loc1_.sortOn("y",Array.NUMERIC);
         _loc5_ = _loc1_.length;
         while(_loc5_ > 0)
         {
            addChildAt(_loc1_[_loc5_ - 1],0);
            _loc5_--;
         }
      }
      
      protected function releaseLivings() : void
      {
         var _loc1_:Array = null;
         var _loc2_:GameLittleLiving = null;
         var _loc3_:LittleLiving = null;
         var _loc4_:DisplayObjectContainer = null;
         var _loc5_:* = null;
         var _loc6_:GameScene = null;
         var _loc7_:int = 0;
         _loc1_ = new Array();
         for(_loc5_ in this._gameLivings)
         {
            _loc2_ = this._gameLivings[_loc5_];
            if(_loc2_)
            {
               _loc2_.setInhaled(false);
            }
            if(_loc2_.inGame)
            {
               _loc2_.lock = false;
               _loc2_.living.stand();
               _loc2_.living.doAction("stand");
               _loc2_.x = _loc2_.living.pos.x * _loc2_.living.speed;
               _loc2_.y = _loc2_.living.pos.y * _loc2_.living.speed;
               _loc1_.push(_loc2_);
            }
         }
         _loc1_.sortOn("y",Array.NUMERIC);
         _loc6_ = LittleGameManager.Instance.gameScene;
         _loc7_ = 0;
         while(_loc7_ < _loc1_.length)
         {
            _loc6_.addToLayer(_loc1_[_loc7_] as DisplayObject,LittleGameManager.GameBackLayer);
            _loc7_++;
         }
      }
      
      protected function drawBackground() : void
      {
         var _loc1_:Graphics = graphics;
         _loc1_.beginFill(0,0);
         _loc1_.drawRect(0,0,StageReferance.stageWidth,StageReferance.stageHeight);
         _loc1_.endFill();
      }
      
      protected function drawMark() : void
      {
         this._markBar = new MarkShape(this._time);
         this._markBar.y = 450;
         this._markBar.x = StageReferance.stageWidth;
         this._markBar.alpha = 0;
         TweenLite.to(this._markBar,0.3,{
            "alpha":1,
            "x":StageReferance.stageWidth - this._markBar.width - 20,
            "ease":Bounce.easeOut
         });
         addChild(this._markBar);
      }
      
      public function invoke(param1:PackageIn) : void
      {
      }
      
      public function execute() : void
      {
         this.drawBackground();
         this._scene.selfInhaled = true;
         LittleGameManager.Instance.mainStage.addChild(this);
         ChatManager.Instance.focusFuncEnabled = false;
         this.addEvent();
         if(NoteCount < MaxNoteCount)
         {
            this._mouseNote = ComponentFactory.Instance.creat("LittleMouseNote");
            addChild(this._mouseNote);
            NoteCount++;
         }
      }
      
      protected function __mark(param1:TimerEvent) : void
      {
         if(this._markBar)
         {
            this._markBar.setTime(this._time - this._timer.currentCount);
         }
      }
      
      protected function __markComplete(param1:TimerEvent) : void
      {
         var _loc2_:Timer = param1.currentTarget as Timer;
         _loc2_.removeEventListener(TimerEvent.TIMER_COMPLETE,this.__markComplete);
         removeEventListener(MouseEvent.CLICK,this.__click);
         this.complete();
      }
      
      protected function addEvent() : void
      {
         addEventListener(MouseEvent.CLICK,this.__click);
      }
      
      protected function __click(param1:MouseEvent) : void
      {
         this._clickCount++;
         if(this._clickCount >= this._totalClick)
         {
            removeEventListener(MouseEvent.CLICK,this.__click);
            this._score = this._totalScore * this._clickCount / this._totalClick;
            if(this._timer)
            {
               this._timer.stop();
               this._timer.removeEventListener(TimerEvent.TIMER_COMPLETE,this.__markComplete);
               this._timer.removeEventListener(TimerEvent.TIMER,this.__mark);
            }
            this.complete();
         }
      }
      
      protected function complete() : void
      {
         LittleGameManager.Instance.sendScore(this._score,this._target.id);
         if(this._self)
         {
            this._self.doAction("stand");
            this._self.MotionState = 2;
            if(this._score > 0)
            {
               this._self.getScore(this._score);
            }
         }
         this._running = false;
         this._scene.removeObject(this);
      }
      
      protected function removeEvent() : void
      {
         removeEventListener(MouseEvent.CLICK,this.__click);
      }
      
      public function dispose() : void
      {
         var _loc1_:* = null;
         this._removed = true;
         if(this._running)
         {
            return;
         }
         this.removeEvent();
         ChatManager.Instance.focusFuncEnabled = true;
         ObjectUtils.disposeObject(this._mouseNote);
         this._mouseNote = null;
         ObjectUtils.disposeObject(this._markBar);
         this._markBar = null;
         ObjectUtils.disposeObject(this._inhaleAsset);
         this._inhaleAsset = null;
         if(parent)
         {
            parent.removeChild(this);
         }
         if(this._target)
         {
            this._target.act(new LittleLivingDieAction(this._scene,this._target));
         }
         this._target = this._self = null;
         for(_loc1_ in this._gameLivings)
         {
            delete this._gameLivings[_loc1_];
         }
         this._gameLivings = null;
      }
   }
}
