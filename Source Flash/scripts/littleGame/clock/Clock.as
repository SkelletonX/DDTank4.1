package littleGame.clock
{
   import ddt.interfaces.IProcessObject;
   import ddt.manager.ProcessManager;
   import ddt.manager.SocketManager;
   import flash.events.EventDispatcher;
   import flash.events.TimerEvent;
   import flash.utils.Timer;
   import littleGame.LittleGameManager;
   import littleGame.events.LittleGameSocketEvent;
   
   public class Clock extends EventDispatcher implements IProcessObject
   {
       
      
      private var _deltas:Vector.<TimeDelta>;
      
      private var _maxDeltas:Number;
      
      private var _syncTimeDelta:int;
      
      private var _responsePending:Boolean;
      
      private var _timeRequestSent:Number;
      
      private var _latency:int;
      
      private var _latencyError:int;
      
      private var _backgroundWaitTime:int;
      
      private var _backgroundTimer:Timer;
      
      private var _bursting:Boolean;
      
      private var _lockedInServerTime:Boolean;
      
      private var _onProcess:Boolean;
      
      private var _internalClock:int;
      
      public function Clock()
      {
         super();
         this._maxDeltas = 10;
      }
      
      public function start(param1:int, param2:int = -1, param3:Boolean = true) : void
      {
         if(this.running)
         {
            return;
         }
         if(param2 != -1)
         {
            this._backgroundTimer = new Timer(param2);
            this._backgroundTimer.addEventListener(TimerEvent.TIMER,this.__onTimer);
            this._backgroundTimer.start();
         }
         this._internalClock = param1;
         this._deltas = new Vector.<TimeDelta>();
         this._lockedInServerTime = false;
         this._responsePending = false;
         this._bursting = param3;
         this.addEvent();
         ProcessManager.Instance.addObject(this);
         this.ping();
      }
      
      public function ping() : void
      {
         this._timeRequestSent = this._internalClock;
         LittleGameManager.Instance.ping(this._timeRequestSent);
      }
      
      private function addEvent() : void
      {
         SocketManager.Instance.addEventListener(LittleGameSocketEvent.PONG,this.__pong);
      }
      
      private function __pong(param1:LittleGameSocketEvent) : void
      {
         var _loc2_:int = param1.pkg.readInt();
         this.addTimeDelta(this._timeRequestSent,this._internalClock,_loc2_);
         this._responsePending = false;
         if(this._bursting)
         {
            if(this._deltas.length >= this._maxDeltas)
            {
               this._bursting = false;
            }
            this.ping();
         }
      }
      
      private function addTimeDelta(param1:int, param2:int, param3:int) : void
      {
         var _loc4_:Number = (param2 - param1) / 2;
         var _loc5_:int = param3 - param2;
         var _loc6_:int = _loc5_ + _loc4_;
         var _loc7_:TimeDelta = new TimeDelta(_loc4_,_loc6_);
         this._deltas.push(_loc7_);
         if(this._deltas.length > this._maxDeltas)
         {
            this._deltas.shift();
         }
         this.recalculate();
      }
      
      private function recalculate() : void
      {
         var _loc1_:Vector.<TimeDelta> = this._deltas.slice(0);
         _loc1_.sort(this.compare);
         var _loc2_:int = this.determineMedian(_loc1_);
         this.pruneOutliers(_loc1_,_loc2_,1.5);
         this._latency = this.determineAverageLatency(_loc1_);
         if(!this._lockedInServerTime)
         {
            this._syncTimeDelta = this.determineAverage(_loc1_);
            this._lockedInServerTime = this._deltas.length == this._maxDeltas;
         }
      }
      
      private function determineAverage(param1:Vector.<TimeDelta>) : int
      {
         var _loc4_:TimeDelta = null;
         var _loc2_:Number = 0;
         var _loc3_:Number = 0;
         while(_loc3_ < param1.length)
         {
            _loc4_ = param1[_loc3_];
            _loc2_ = _loc2_ + _loc4_.timeSyncDelta;
            _loc3_++;
         }
         return _loc2_ / param1.length;
      }
      
      private function determineAverageLatency(param1:Vector.<TimeDelta>) : int
      {
         var _loc5_:TimeDelta = null;
         var _loc2_:int = 0;
         var _loc3_:int = 0;
         while(_loc3_ < param1.length)
         {
            _loc5_ = param1[_loc3_];
            _loc2_ = _loc2_ + _loc5_.latency;
            _loc3_++;
         }
         var _loc4_:int = _loc2_ / param1.length;
         this._latencyError = Math.abs(TimeDelta(param1[param1.length - 1]).latency - _loc4_);
         return _loc4_;
      }
      
      private function pruneOutliers(param1:Vector.<TimeDelta>, param2:int, param3:Number) : void
      {
         var _loc6_:TimeDelta = null;
         var _loc4_:Number = param2 * param3;
         var _loc5_:Number = param1.length - 1;
         while(_loc5_ >= 0)
         {
            _loc6_ = param1[_loc5_];
            if(_loc6_.latency > _loc4_)
            {
               param1.splice(_loc5_,1);
               _loc5_--;
               continue;
            }
            break;
         }
      }
      
      private function determineMedian(param1:Vector.<TimeDelta>) : int
      {
         var _loc2_:Number = NaN;
         if(param1.length % 2 == 0)
         {
            _loc2_ = param1.length / 2 - 1;
            return (param1[_loc2_].latency + param1[_loc2_ + 1].latency) / 2;
         }
         _loc2_ = Math.floor(param1.length / 2);
         return param1[_loc2_].latency;
      }
      
      private function compare(param1:TimeDelta, param2:TimeDelta) : Number
      {
         if(param1.latency < param2.latency)
         {
            return -1;
         }
         if(param1.latency > param2.latency)
         {
            return 1;
         }
         return 0;
      }
      
      private function removeEvent() : void
      {
      }
      
      public function dispose() : void
      {
      }
      
      public function get onProcess() : Boolean
      {
         return this._onProcess;
      }
      
      public function set onProcess(param1:Boolean) : void
      {
         this._onProcess = param1;
      }
      
      public function get running() : Boolean
      {
         return this._onProcess;
      }
      
      public function process(param1:Number) : void
      {
         this._internalClock = this._internalClock + param1;
      }
      
      private function __onTimer(param1:TimerEvent) : void
      {
         if(!this._responsePending && !this._bursting)
         {
            this.ping();
         }
      }
      
      public function get time() : Number
      {
         return this._internalClock + this._syncTimeDelta;
      }
      
      public function get latency() : int
      {
         return this._latency;
      }
      
      public function get latencyError() : int
      {
         return this._latencyError;
      }
      
      public function get maxDeltas() : Number
      {
         return this._maxDeltas;
      }
      
      public function set maxDeltas(param1:Number) : void
      {
         this._maxDeltas = param1;
      }
   }
}
