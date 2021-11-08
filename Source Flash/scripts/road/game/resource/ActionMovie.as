package road.game.resource
{
   import flash.display.FrameLabel;
   import flash.display.MovieClip;
   import flash.events.Event;
   import flash.geom.Point;
   import flash.media.SoundTransform;
   import flash.utils.Dictionary;
   import flash.utils.getDefinitionByName;
   
   public class ActionMovie extends MovieClip
   {
      
      public static var LEFT:String = "left";
      
      public static var RIGHT:String = "right";
      
      public static var DEFAULT_ACTION:String = "stand";
      
      public static var STAND_ACTION:String = "stand";
       
      
      private var _labelLastFrames:Array;
      
      private var _soundControl:SoundTransform;
      
      private var _labelLastFrame:Dictionary;
      
      private var _currentAction:String;
      
      private var lastAction:String = "";
      
      private var _callBacks:Dictionary;
      
      private var _argsDic:Dictionary;
      
      private var _actionEnded:Boolean = true;
      
      protected var _actionRelative:Object;
      
      public var labelMapping:Dictionary;
      
      private var _soundEffectInstance;
      
      private var _isMute:Boolean = false;
      
      public function ActionMovie()
      {
         var _loc1_:* = undefined;
         this._labelLastFrames = [];
         this._labelLastFrame = new Dictionary();
         this._argsDic = new Dictionary();
         this.labelMapping = new Dictionary();
         super();
         try
         {
            _loc1_ = getDefinitionByName("ddt.manager.SoundEffectManager");
            if(_loc1_)
            {
               this._soundEffectInstance = _loc1_.Instance;
            }
         }
         catch(e:Error)
         {
         }
         this._callBacks = new Dictionary();
         mouseEnabled = false;
         mouseChildren = false;
         scrollRect = null;
         this._soundControl = new SoundTransform();
         soundTransform = this._soundControl;
         this.initMovie();
         this.addEvent();
      }
      
      private function initMovie() : void
      {
         var _loc1_:Array = currentLabels;
         var _loc2_:int = 0;
         while(_loc2_ < _loc1_.length)
         {
            if(_loc2_ != 0)
            {
               this._labelLastFrame[_loc1_[_loc2_ - 1].name] = int(_loc1_[_loc2_].frame - 1);
            }
            _loc2_++;
         }
         this._labelLastFrame[_loc1_[_loc1_.length - 1].name] = int(totalFrames);
      }
      
      private function addEvent() : void
      {
         addEventListener(ActionMovieEvent.ACTION_END,this.__onActionEnd);
      }
      
      public function doAction(param1:String, param2:Function = null, param3:Array = null) : void
      {
         var _loc4_:String = null;
         if(this.labelMapping[param1])
         {
            _loc4_ = this.labelMapping[param1];
         }
         else
         {
            _loc4_ = param1;
         }
         if(!this.hasThisAction(_loc4_))
         {
            if(param2 != null)
            {
               this.callFun(param2,param3);
            }
            return;
         }
         if(!this._actionEnded)
         {
            if(this._callBacks && this._callBacks[this.currentAction] != null)
            {
               this.callCallBack(this.currentAction);
            }
            this._actionEnded = true;
            dispatchEvent(new ActionMovieEvent(ActionMovieEvent.ACTION_END));
         }
         this._actionEnded = false;
         if(param2 != null && this._callBacks != null && this._callBacks[_loc4_] != param2)
         {
            this._callBacks[_loc4_] = param2;
            this._argsDic[_loc4_] = param3;
         }
         this.lastAction = this.currentAction;
         this._currentAction = _loc4_;
         if(this._soundControl)
         {
            this._soundControl.volume = !!this._isMute?Number(0):Number(1);
         }
         if(soundTransform && this._soundControl)
         {
            soundTransform = this._soundControl;
         }
         addEventListener(Event.ENTER_FRAME,this.loop);
         this.MCGotoAndPlay(this.currentAction);
         dispatchEvent(new ActionMovieEvent(ActionMovieEvent.ACTION_START));
      }
      
      private function hasThisAction(param1:String) : Boolean
      {
         var _loc3_:FrameLabel = null;
         var _loc2_:Boolean = false;
         for each(_loc3_ in currentLabels)
         {
            if(_loc3_.name == param1)
            {
               _loc2_ = true;
               break;
            }
         }
         return _loc2_;
      }
      
      private function loop(param1:Event) : void
      {
         if(currentFrame == this._labelLastFrame[this.currentAction] || currentLabel != this.currentAction)
         {
            removeEventListener(Event.ENTER_FRAME,this.loop);
            this._actionEnded = true;
            if(this._callBacks && this._callBacks[this.currentAction] != null)
            {
               this.callCallBack(this.currentAction);
            }
            dispatchEvent(new ActionMovieEvent(ActionMovieEvent.ACTION_END));
         }
      }
      
      private function callCallBack(param1:String) : void
      {
         var _loc2_:Array = this._argsDic[param1];
         if(this._callBacks[param1] == null)
         {
            return;
         }
         this.callFun(this._callBacks[param1],_loc2_);
         this.deleteFun(param1);
      }
      
      private function deleteFun(param1:String) : void
      {
         if(this._callBacks)
         {
            this._callBacks[param1] = null;
            delete this._callBacks[param1];
         }
         if(this._argsDic)
         {
            this._argsDic[param1] = null;
            delete this._argsDic[param1];
         }
      }
      
      private function callFun(param1:Function, param2:Array) : void
      {
         if(param2 == null || param2.length == 0)
         {
            param1();
         }
         else if(param2.length == 1)
         {
            param1(param2[0]);
         }
         else if(param2.length == 2)
         {
            param1(param2[0],param2[1]);
         }
         else if(param2.length == 3)
         {
            param1(param2[0],param2[1],param2[2]);
         }
         else if(param2.length == 4)
         {
            param1(param2[0],param2[1],param2[2],param2[3]);
         }
      }
      
      public function get currentAction() : String
      {
         return this._currentAction;
      }
      
      public function setActionRelative(param1:Object) : void
      {
         this._actionRelative = param1;
      }
      
      public function get popupPos() : Point
      {
         if(this["_popPos"])
         {
            return new Point(this["_popPos"].x * scaleX,this["_popPos"].y);
         }
         return null;
      }
      
      public function get popupDir() : Point
      {
         if(this["_popDir"])
         {
            return new Point(this["_popDir"].x,this["_popDir"].y);
         }
         return null;
      }
      
      public function set direction(param1:String) : void
      {
         if(ActionMovie.LEFT == param1)
         {
            scaleX = 1;
         }
         else if(ActionMovie.RIGHT == param1)
         {
            scaleX = -1;
         }
      }
      
      public function get direction() : String
      {
         if(scaleX > 0)
         {
            return ActionMovie.LEFT;
         }
         return ActionMovie.RIGHT;
      }
      
      public function setActionMapping(param1:String, param2:String) : void
      {
         if(param1.length <= 0)
         {
            return;
         }
         this.labelMapping[param1] = param2;
      }
      
      private function stopMovieClip(param1:MovieClip) : void
      {
         var _loc2_:int = 0;
         if(param1)
         {
            param1.gotoAndStop(1);
            if(param1.numChildren > 0)
            {
               _loc2_ = 0;
               while(_loc2_ < param1.numChildren)
               {
                  this.stopMovieClip(param1.getChildAt(_loc2_) as MovieClip);
                  _loc2_++;
               }
            }
         }
      }
      
      override public function gotoAndStop(param1:Object, param2:String = null) : void
      {
         var _loc3_:FrameLabel = null;
         if(param1 is String)
         {
            for each(_loc3_ in currentLabels)
            {
               if(_loc3_.name == param1)
               {
                  super.gotoAndStop(param1);
                  return;
               }
            }
         }
         else
         {
            super.gotoAndStop(param1);
         }
      }
      
      protected function endAction() : void
      {
         dispatchEvent(new ActionMovieEvent("end"));
      }
      
      protected function startAction() : void
      {
         dispatchEvent(new ActionMovieEvent("start"));
      }
      
      protected function send(param1:String) : void
      {
         dispatchEvent(new ActionMovieEvent(param1));
      }
      
      protected function sendCommand(param1:String, param2:Object = null) : void
      {
         dispatchEvent(new ActionMovieEvent(param1,param2));
      }
      
      override public function gotoAndPlay(param1:Object, param2:String = null) : void
      {
         this.doAction(String(param1));
      }
      
      public function MCGotoAndPlay(param1:Object) : void
      {
         super.gotoAndPlay(param1);
      }
      
      private function __onActionEnd(param1:ActionMovieEvent) : void
      {
         if(!this._actionRelative)
         {
            return;
         }
         if(!this._actionRelative[this._currentAction])
         {
            this.doAction(DEFAULT_ACTION);
            return;
         }
         if(this._actionRelative[this._currentAction] is Function)
         {
            this._actionRelative[this._currentAction]();
         }
         else
         {
            this.doAction(this._actionRelative[this._currentAction]);
         }
      }
      
      public function get versionTag() : String
      {
         return "road.game.resource.ActionMovie version:1.02";
      }
      
      public function doSomethingSpecial() : void
      {
      }
      
      public function dispose() : void
      {
         this._soundControl.volume = 0;
         removeEventListener(Event.ENTER_FRAME,this.loop);
         this.stopMovieClip(this);
         stop();
         this._soundControl = null;
         this._labelLastFrames = null;
         if(parent)
         {
            parent.removeChild(this);
         }
         this._callBacks = null;
      }
      
      public function mute() : void
      {
         this._soundControl.volume = 0;
         this._isMute = true;
      }
   }
}
