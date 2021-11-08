package littleGame.character
{
   import character.ComplexBitmapCharacter;
   import character.action.ComplexBitmapAction;
   import com.pickgliss.ui.core.Disposeable;
   import com.pickgliss.utils.StringUtils;
   import ddt.data.player.PlayerInfo;
   import ddt.manager.PathManager;
   import ddt.manager.SoundManager;
   import flash.display.BitmapData;
   import flash.events.Event;
   import flash.net.URLLoader;
   import flash.net.URLRequest;
   import flash.utils.Dictionary;
   
   public class LittleGameCharacter extends ComplexBitmapCharacter implements Disposeable
   {
      
      public static const WIDTH:Number = 230;
      
      public static const HEIGHT:Number = 175;
      
      public static var DEFINE:XML;
       
      
      private var _playerInfo:PlayerInfo;
      
      private var _headBmd:BitmapData;
      
      private var _bodyBmd:BitmapData;
      
      private var headBmds:Vector.<BitmapData>;
      
      private var _loader:LittleGameCharaterLoader;
      
      private var hasFaceColor:Boolean = false;
      
      private var hasClothColor:Boolean = false;
      
      private var _isComplete:Boolean;
      
      private var _currentAct:String;
      
      private var _currentSoundPlayed:Boolean;
      
      public function LittleGameCharacter(param1:PlayerInfo, param2:int = 1)
      {
         this._playerInfo = param1;
         var _loc3_:Array = this._playerInfo.Colors.split(",");
         this.hasFaceColor = Boolean(_loc3_[5]);
         this.hasClothColor = Boolean(_loc3_[4]);
         this._loader = new LittleGameCharaterLoader(this._playerInfo,param2);
         this._loader.load(this.onComplete);
         super(null,null,"",WIDTH,HEIGHT);
      }
      
      public static function setup() : void
      {
         var loader:URLLoader = null;
         var onXmlComplete:Function = null;
         onXmlComplete = function(param1:Event):void
         {
            param1.currentTarget.removeEventListener(Event.COMPLETE,onXmlComplete);
            DEFINE = XML(loader.data);
         };
         loader = new URLLoader();
         loader.addEventListener(Event.COMPLETE,onXmlComplete);
         loader.load(new URLRequest(PathManager.SITE_MAIN + "flash/characterDefine.xml?rnd=" + Math.random()));
      }
      
      override public function doAction(param1:String) : void
      {
         var _loc3_:FrameByFrameItem = null;
         play();
         var _loc2_:ComplexBitmapAction = _actionSet.getAction(param1) as ComplexBitmapAction;
         if(_loc2_)
         {
            if(_currentAction == null)
            {
               this.currentAction = _loc2_;
            }
            else if(_loc2_.priority >= _currentAction.priority)
            {
               for each(_loc3_ in _currentAction.assets)
               {
                  _loc3_.stop();
                  removeItem(_loc3_);
               }
               _currentAction.reset();
               this.currentAction = _loc2_;
               if(param1.indexOf("back") != -1)
               {
                  _items.reverse();
               }
            }
         }
         this._currentAct = param1;
      }
      
      override protected function set currentAction(param1:ComplexBitmapAction) : void
      {
         var _loc2_:FrameByFrameItem = null;
         _currentAction = param1;
         _autoStop = _currentAction.endStop;
         for each(_loc2_ in _currentAction.assets)
         {
            _loc2_.play();
            addItem(_loc2_);
         }
         if(!StringUtils.isEmpty(_currentAction.sound) && _soundEnabled && (this._currentAct != param1.name || !this._currentSoundPlayed))
         {
            SoundManager.instance.play(_currentAction.sound);
            this._currentSoundPlayed = true;
         }
      }
      
      override protected function update() : void
      {
         super.update();
      }
      
      private function onComplete() : void
      {
         this._headBmd = this._loader.getContent()[0];
         this._bodyBmd = this._loader.getContent()[1];
         var _loc1_:Dictionary = new Dictionary();
         _loc1_["head"] = this._headBmd;
         _loc1_["body"] = this._bodyBmd;
         _loc1_["effect"] = this._loader.getContent()[2];
         _loc1_["specialHead"] = this._loader.getContent()[4];
         assets = _loc1_;
         this.headBmds = new Vector.<BitmapData>();
         this.headBmds.push(this._loader.getContent()[3],this._loader.getContent()[4],this._loader.getContent()[5]);
         super.description = DEFINE;
         dispatchEvent(new Event(Event.COMPLETE));
         if(this._currentAct != null)
         {
            this.doAction(this._currentAct);
         }
      }
      
      override public function get actions() : Array
      {
         return _actionSet.actions;
      }
      
      private function updateRenderSource(param1:String, param2:BitmapData) : void
      {
         var _loc3_:FrameByFrameItem = null;
         for each(_loc3_ in _bitmapRendItems)
         {
            if(_loc3_.sourceName == param1)
            {
               _loc3_.source = param2;
               if(_loc3_ is CrossFrameItem)
               {
                  CrossFrameItem(_loc3_).frames = CrossFrameItem(_loc3_).frames;
               }
            }
         }
      }
      
      public function setFunnyHead(param1:uint = 0) : void
      {
         if(this.headBmds == null || param1 > this.headBmds.length - 1)
         {
            return;
         }
         this.updateRenderSource("specialHead",this.headBmds[param1]);
      }
      
      override public function dispose() : void
      {
         var _loc1_:int = 0;
         if(assets)
         {
            assets["head"] = null;
            assets["body"] = null;
            assets["effect"] = null;
            assets["specialHead"] = null;
            assets = null;
         }
         super.dispose();
         if(this._headBmd)
         {
            this._headBmd.dispose();
            this._headBmd = null;
         }
         if(this._bodyBmd && this.hasClothColor)
         {
            this._bodyBmd.dispose();
         }
         this._bodyBmd = null;
         if(this._loader)
         {
            this._loader.dispose();
         }
         if(this.hasFaceColor && this.headBmds)
         {
            _loc1_ = 0;
            while(_loc1_ < this.headBmds.length)
            {
               if(this.headBmds[_loc1_])
               {
                  this.headBmds[_loc1_].dispose();
               }
               _loc1_++;
            }
         }
         this.headBmds = null;
      }
   }
}
