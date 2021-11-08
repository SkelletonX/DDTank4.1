package littleGame.object
{
   import com.greensock.TweenLite;
   import com.greensock.easing.Bounce;
   import com.pickgliss.toplevel.StageReferance;
   import com.pickgliss.utils.ClassUtils;
   import com.pickgliss.utils.ObjectUtils;
   import ddt.manager.ChatManager;
   import ddt.manager.SoundManager;
   import flash.display.DisplayObject;
   import flash.display.Graphics;
   import flash.display.MovieClip;
   import flash.events.Event;
   import flash.events.MouseEvent;
   import flash.events.TimerEvent;
   import flash.media.SoundChannel;
   import flash.utils.Dictionary;
   import flash.utils.Timer;
   import littleGame.LittleGameManager;
   import littleGame.data.LittleObjectType;
   import littleGame.interfaces.ILittleObject;
   import littleGame.view.PriceShape;
   import littleGame.view.ScoreShape;
   
   public class BigBoguInhaled extends NormalBoguInhaled
   {
       
      
      private var _clickSoundChannel:SoundChannel;
      
      private var _soundPlaying:Boolean = false;
      
      private var _scoreShape:ScoreShape;
      
      private var _soundPlayVer:int;
      
      private var _scoreTween:Boolean = false;
      
      public function BigBoguInhaled()
      {
         super();
      }
      
      override public function get type() : String
      {
         return LittleObjectType.BigBoguInhaled;
      }
      
      override protected function removeEvent() : void
      {
         super.removeEvent();
         if(this._clickSoundChannel)
         {
            this._clickSoundChannel.removeEventListener(Event.SOUND_COMPLETE,this.__soundComplete);
         }
      }
      
      override protected function drawInhaleAsset() : void
      {
         _inhaleAsset = ClassUtils.CreatInstance("asset.littleGame.BigInhale");
         _inhaleAsset.x = 526;
         _inhaleAsset.y = 324;
         addChild(_inhaleAsset);
         _inhaleAsset.mouseChildren = _inhaleAsset.mouseEnabled = false;
         _inhaleAsset.addEventListener(Event.ENTER_FRAME,this.__inhaleOnFrame);
         _inhaleAsset.gotoAndPlay("born");
         SoundManager.instance.play("163");
      }
      
      private function __inhaleOnFrame(param1:Event) : void
      {
         var _loc2_:MovieClip = param1.currentTarget as MovieClip;
         if(_loc2_.currentFrameLabel == "bornEnd")
         {
            this.start();
         }
         else if(_loc2_.currentFrame >= _loc2_.totalFrames)
         {
            ObjectUtils.disposeObject(_loc2_);
            this.complete();
         }
      }
      
      override protected function drawBackground() : void
      {
         var _loc1_:Graphics = graphics;
         _loc1_.beginFill(0,0.8);
         _loc1_.drawRect(0,0,StageReferance.stageWidth,StageReferance.stageHeight);
         _loc1_.endFill();
      }
      
      override public function execute() : void
      {
         var _loc2_:* = null;
         var _loc3_:ILittleObject = null;
         this.drawBackground();
         drawMark();
         lockLivings();
         _scene.selfInhaled = true;
         ChatManager.Instance.focusFuncEnabled = false;
         var _loc1_:Dictionary = _scene.littleObjects;
         for(_loc2_ in _loc1_)
         {
            _loc3_ = _loc1_[_loc2_];
            if(_loc3_.type == LittleObjectType.BoguGiveup)
            {
               _scene.removeObject(_loc3_);
            }
         }
         LittleGameManager.Instance.mainStage.addChild(this);
         _timer = new Timer(1000,_time);
         _timer.addEventListener(TimerEvent.TIMER,__mark);
         _timer.addEventListener(TimerEvent.TIMER_COMPLETE,this.__markComplete);
         _timer.start();
      }
      
      private function scoreTweenComplete() : void
      {
         this._scoreTween = false;
      }
      
      private function priceTweenIn(param1:DisplayObject) : void
      {
         TweenLite.to(param1,0.2,{
            "delay":2,
            "alpha":1,
            "y":param1.y - param1.height * 2,
            "ease":Bounce.easeOut,
            "onComplete":ObjectUtils.disposeObject,
            "onCompleteParams":[param1]
         });
      }
      
      private function __soundComplete(param1:Event) : void
      {
         this._soundPlaying = false;
         param1.currentTarget.removeEventListener(Event.SOUND_COMPLETE,this.__soundComplete);
         if(this._soundPlayVer < _clickCount && _running)
         {
            this._clickSoundChannel = SoundManager.instance.play("164");
            if(this._clickSoundChannel)
            {
               this._clickSoundChannel.addEventListener(Event.SOUND_COMPLETE,this.__soundComplete);
            }
            this._soundPlaying = true;
            this._soundPlayVer = _clickCount;
         }
      }
      
      override protected function __click(param1:MouseEvent) : void
      {
         var _loc3_:PriceShape = null;
         _clickCount++;
         if(_inhaleAsset)
         {
            _inhaleAsset["admit"]["water"].gotoAndStop(int(_inhaleAsset["admit"]["water"].totalFrames * _clickCount / _totalClick));
            _inhaleAsset["admit"].play();
         }
         _score = _totalScore * _clickCount / _totalClick;
         var _loc2_:int = 0;
         if(this._scoreShape)
         {
            this._scoreShape.setScore(_score);
         }
         else
         {
            this._scoreShape = new ScoreShape(1);
            this._scoreShape.y = StageReferance.stageHeight - this._scoreShape.height >> 1;
            this._scoreShape.setScore(_score);
            addChild(this._scoreShape);
         }
         if(!this._scoreTween)
         {
            this._scoreShape.alpha = 0;
            this._scoreShape.x = 200;
            this._scoreTween = true;
            TweenLite.to(this._scoreShape,0.3,{
               "alpha":1,
               "x":300,
               "ease":Bounce.easeOut,
               "onComplete":this.scoreTweenComplete
            });
            SoundManager.instance.stop("164");
            SoundManager.instance.play("164");
         }
         if(_clickCount >= _totalClick)
         {
            removeEventListener(MouseEvent.CLICK,this.__click);
            _loc2_ = _totalScore * 0.2;
            _loc3_ = new PriceShape(_loc2_);
            _loc3_.alpha = 0;
            _loc3_.x = 300;
            _loc3_.y = this._scoreShape.y;
            addChild(_loc3_);
            TweenLite.to(_loc3_,0.2,{
               "delay":0.2,
               "alpha":1,
               "y":_loc3_.y - _loc3_.height - 20,
               "onComplete":this.priceTweenIn,
               "onCompleteParams":[_loc3_]
            });
            _target.dieing = true;
         }
         _score = _score + _loc2_;
      }
      
      override protected function complete() : void
      {
         LittleGameManager.Instance.sendScore(_score,_target.id);
         if(_inhaleAsset)
         {
            _inhaleAsset.removeEventListener(Event.ENTER_FRAME,this.__inhaleOnFrame);
         }
         _running = false;
         this.dispose();
      }
      
      private function start() : void
      {
         _inhaleAsset.gotoAndPlay("stand");
         if(_timer)
         {
            _timer.start();
         }
         addEvent();
      }
      
      override protected function __markComplete(param1:TimerEvent) : void
      {
         var _loc2_:Timer = param1.currentTarget as Timer;
         _loc2_.removeEventListener(TimerEvent.TIMER,__mark);
         _loc2_.removeEventListener(TimerEvent.TIMER_COMPLETE,this.__markComplete);
         removeEventListener(MouseEvent.CLICK,this.__click);
         _inhaleAsset.gotoAndPlay("out");
      }
      
      override public function dispose() : void
      {
         _removed = true;
         if(_running)
         {
            return;
         }
         if(_self)
         {
            _self.doAction("stand");
            _self.MotionState = 2;
            _self.inhaled = false;
         }
         releaseLivings();
         super.dispose();
      }
   }
}
