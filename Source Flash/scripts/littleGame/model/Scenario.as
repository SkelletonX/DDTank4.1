package littleGame.model
{
   import com.pickgliss.utils.ClassUtils;
   import com.pickgliss.utils.ObjectUtils;
   import ddt.ddt_internal;
   import ddt.interfaces.IProcessObject;
   import ddt.manager.ProcessManager;
   import flash.display.BitmapData;
   import flash.events.EventDispatcher;
   import flash.events.TimerEvent;
   import flash.geom.Matrix;
   import flash.geom.Rectangle;
   import flash.utils.Dictionary;
   import flash.utils.Timer;
   import littleGame.LittleGameLoader;
   import littleGame.LittleGameManager;
   import littleGame.clock.Clock;
   import littleGame.data.Grid;
   import littleGame.data.Node;
   import littleGame.events.LittleGameEvent;
   import littleGame.events.LittleLivingEvent;
   import littleGame.interfaces.ILittleObject;
   import littleGame.view.ScoreShape;
   
   [Event(name="update",type="littleGame.events.LittleGameEvent")]
   [Event(name="removeLiving",type="littleGame.events.LittleGameEvent")]
   [Event(name="addLiving",type="littleGame.events.LittleGameEvent")]
   [Event(name="soundEnabledChanged",type="littleGame.events.LittleGameEvent")]
   [Event(name="selfInhaledChanged",type="littleGame.events.LittleGameEvent")]
   public class Scenario extends EventDispatcher implements IProcessObject
   {
       
      
      public var localStartTime:int;
      
      public var startTimestamp:int;
      
      public var grid:Grid;
      
      public var id:int;
      
      public var worldID:int;
      
      public var monsters:String;
      
      public var objects:String = "object/other.swf";
      
      private var _objects:Dictionary;
      
      private var _livings:Dictionary;
      
      private var _livingCount:int = 0;
      
      private var _onProcess:Boolean;
      
      private var _pause:Boolean = false;
      
      private var _stones:Vector.<Rectangle>;
      
      private var _selfPlayer:LittleSelf;
      
      private var _numDic:Dictionary;
      
      public var clock:Clock;
      
      public var delay:int;
      
      ddt_internal var bigNum:BitmapData;
      
      ddt_internal var normalNum:BitmapData;
      
      ddt_internal var markBack:BitmapData;
      
      ddt_internal var priceBack:BitmapData;
      
      ddt_internal var priceNum:BitmapData;
      
      ddt_internal var inhaleNeed:BitmapData;
      
      public var serverClock:int;
      
      public var gameLoader:LittleGameLoader;
      
      public var music:String;
      
      private var _timer:Timer;
      
      public var virtualTime:int = 0;
      
      private var _last:int = 0;
      
      private var _soundEnabled:Boolean;
      
      private var _selfInhaled:Boolean;
      
      public function Scenario()
      {
         this._objects = new Dictionary();
         this._livings = new Dictionary();
         this._stones = new Vector.<Rectangle>();
         this._numDic = new Dictionary();
         this.clock = new Clock();
         this._timer = new Timer(1000);
         this._timer.addEventListener(TimerEvent.TIMER,this.onTimer);
         super();
      }
      
      private function __clock(param1:TimerEvent) : void
      {
      }
      
      ddt_internal function drawNum() : void
      {
         ddt_internal::inhaleNeed = ClassUtils.CreatInstance("asset.littleGame.InhaleNeed");
         ddt_internal::priceBack = ClassUtils.CreatInstance("asset.littleGame.price");
         ddt_internal::priceNum = ClassUtils.CreatInstance("asset.littleGame.numprice");
         ddt_internal::markBack = ClassUtils.CreatInstance("asset.littleGame.Mark");
         ddt_internal::bigNum = ClassUtils.CreatInstance("asset.littleGame.num");
         var _loc1_:Number = ScoreShape.ddt_internal::size / ddt_internal::bigNum.height;
         var _loc2_:Matrix = new Matrix();
         _loc2_.scale(_loc1_,_loc1_);
         ddt_internal::normalNum = new BitmapData(ddt_internal::bigNum.width * _loc1_,ddt_internal::bigNum.height * _loc1_,true,0);
         ddt_internal::normalNum.draw(ddt_internal::bigNum,_loc2_,null,null,null,true);
      }
      
      public function get stones() : Vector.<Rectangle>
      {
         return this._stones;
      }
      
      public function addObject(param1:ILittleObject) : ILittleObject
      {
         this._objects[param1.id] = param1;
         return param1;
      }
      
      public function removeObject(param1:ILittleObject) : ILittleObject
      {
         if(param1 == null)
         {
            return null;
         }
         delete this._objects[param1.id];
         ObjectUtils.disposeObject(param1);
         return param1;
      }
      
      public function addLiving(param1:LittleLiving) : LittleLiving
      {
         var _loc2_:Node = null;
         if(this._livings[param1.id] == null)
         {
            this._livings[param1.id] = param1;
            param1.inGame = true;
            if(param1.isSelf)
            {
               this.setSelfPlayer(param1 as LittleSelf);
            }
            param1.speed = this.grid.cellSize;
            _loc2_ = this.grid.getNode(param1.pos.x,param1.pos.y);
            this._livingCount++;
            if(this.running)
            {
               dispatchEvent(new LittleGameEvent(LittleGameEvent.AddLiving,param1));
            }
         }
         return param1;
      }
      
      public function removeLiving(param1:LittleLiving) : LittleLiving
      {
         var _loc2_:Node = null;
         if(param1 && !param1.dieing && this._livings[param1.id] != null)
         {
            delete this._livings[param1.id];
            param1.inGame = false;
            _loc2_ = this.grid.getNode(param1.pos.x,param1.pos.y);
            this._livingCount--;
            dispatchEvent(new LittleGameEvent(LittleGameEvent.RemoveLiving,param1));
         }
         return param1;
      }
      
      public function get livings() : Dictionary
      {
         return this._livings;
      }
      
      public function findObject(param1:int) : ILittleObject
      {
         return this._objects[param1] as ILittleObject;
      }
      
      public function get littleObjects() : Dictionary
      {
         return this._objects;
      }
      
      public function findLiving(param1:int) : LittleLiving
      {
         return this._livings[param1] as LittleLiving;
      }
      
      public function get running() : Boolean
      {
         return this._onProcess;
      }
      
      private function creat() : void
      {
         this._stones.push(new Rectangle(654,20,80,200));
      }
      
      public function setSelfPlayer(param1:LittleSelf) : void
      {
         this._selfPlayer = param1;
      }
      
      private function __selfCollided(param1:LittleLivingEvent) : void
      {
      }
      
      public function get selfPlayer() : LittleSelf
      {
         return this._selfPlayer;
      }
      
      public function startup() : void
      {
         ProcessManager.Instance.addObject(this);
      }
      
      public function shutdown() : void
      {
         ProcessManager.Instance.removeObject(this);
      }
      
      public function pause() : void
      {
         this._pause = true;
      }
      
      public function resume() : void
      {
         this._pause = false;
      }
      
      private function onTimer(param1:TimerEvent) : void
      {
         LittleGameManager.Instance.synchronousLivingPos(this._selfPlayer.pos.x,this._selfPlayer.pos.y);
      }
      
      public function startSysnPos() : void
      {
         this._timer.start();
      }
      
      public function stopSysnPos() : void
      {
         this._timer.stop();
      }
      
      public function dispose() : void
      {
         var _loc1_:* = null;
         var _loc2_:* = null;
         var _loc3_:LittleLiving = null;
         var _loc4_:ILittleObject = null;
         ObjectUtils.disposeObject(ddt_internal::inhaleNeed);
         ddt_internal::inhaleNeed = null;
         ObjectUtils.disposeObject(ddt_internal::priceNum);
         ddt_internal::priceNum = null;
         ObjectUtils.disposeObject(ddt_internal::priceBack);
         ddt_internal::priceBack = null;
         ObjectUtils.disposeObject(ddt_internal::markBack);
         ddt_internal::markBack = null;
         ObjectUtils.disposeObject(ddt_internal::bigNum);
         ddt_internal::bigNum = null;
         ObjectUtils.disposeObject(ddt_internal::normalNum);
         ddt_internal::normalNum = null;
         ObjectUtils.disposeObject(this.grid);
         this.grid = null;
         ProcessManager.Instance.removeObject(this);
         this._timer.stop();
         this._timer.removeEventListener(TimerEvent.TIMER,this.onTimer);
         this._timer = null;
         this.gameLoader.unload();
         this.gameLoader.dispose();
         for(_loc1_ in this._livings)
         {
            _loc3_ = this._livings[_loc1_];
            ObjectUtils.disposeObject(_loc3_);
            delete this._livings[_loc1_];
         }
         for(_loc2_ in this._objects)
         {
            _loc4_ = this._objects[_loc2_];
            ObjectUtils.disposeObject(_loc4_);
            delete this._objects[_loc2_];
         }
         this._livings = null;
         this._objects = null;
         this._selfPlayer = null;
         this.gameLoader = null;
      }
      
      public function get onProcess() : Boolean
      {
         return this._onProcess;
      }
      
      public function set onProcess(param1:Boolean) : void
      {
         this._onProcess = param1;
      }
      
      public function process(param1:Number) : void
      {
         var _loc2_:LittleLiving = null;
         if(this._pause)
         {
            return;
         }
         dispatchEvent(new LittleGameEvent(LittleGameEvent.Update));
         for each(_loc2_ in this._livings)
         {
            _loc2_.update();
         }
      }
      
      public function get soundEnabled() : Boolean
      {
         return this._soundEnabled;
      }
      
      public function set soundEnabled(param1:Boolean) : void
      {
         if(this._soundEnabled == param1)
         {
            return;
         }
         this._soundEnabled = param1;
         dispatchEvent(new LittleGameEvent(LittleGameEvent.SoundEnabledChanged));
      }
      
      public function get selfInhaled() : Boolean
      {
         return this._selfInhaled;
      }
      
      public function set selfInhaled(param1:Boolean) : void
      {
         if(this._selfInhaled == param1)
         {
            return;
         }
         this._selfInhaled = param1;
         dispatchEvent(new LittleGameEvent(LittleGameEvent.SelfInhaleChanged));
      }
   }
}
