package littleGame
{
   import ddt.interfaces.IProcessObject;
   import ddt.manager.ProcessManager;
   import ddt.manager.SocketManager;
   import flash.events.Event;
   import littleGame.events.LittleGameSocketEvent;
   
   public class LittleGamePacketQueue implements IProcessObject
   {
      
      private static var _ins:LittleGamePacketQueue;
       
      
      private var _executable:Array;
      
      public var _waitlist:Array;
      
      private var _lifeTime:int;
      
      private var _onProcess:Boolean = false;
      
      public function LittleGamePacketQueue()
      {
         super();
      }
      
      public static function get Instance() : LittleGamePacketQueue
      {
         return _ins = _ins || new LittleGamePacketQueue();
      }
      
      public function addQueue(param1:LittleGameSocketEvent) : void
      {
         this._waitlist.push(param1);
      }
      
      public function startup() : void
      {
         ProcessManager.Instance.addObject(this);
      }
      
      public function shutdown() : void
      {
         ProcessManager.Instance.removeObject(this);
      }
      
      public function setLifeTime(param1:int) : void
      {
         this._lifeTime = param1;
      }
      
      public function reset() : void
      {
         this._executable = [];
         this._waitlist = [];
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
         var _loc2_:int = 0;
         var _loc3_:int = 0;
         var _loc4_:int = 0;
         var _loc5_:LittleGameSocketEvent = null;
         this._lifeTime++;
         if(this.running)
         {
            _loc2_ = 0;
            _loc3_ = 0;
            while(_loc3_ < this._waitlist.length)
            {
               _loc5_ = this._waitlist[_loc3_];
               if(_loc5_.pkg.extend2 <= this._lifeTime)
               {
                  this._executable.push(_loc5_);
                  _loc2_++;
                  _loc3_++;
                  continue;
               }
               break;
            }
            this._waitlist.splice(0,_loc2_);
            _loc2_ = 0;
            _loc4_ = 0;
            while(_loc4_ < this._executable.length)
            {
               if(this.running)
               {
                  this.dispatchEvent(this._executable[_loc4_]);
                  _loc2_++;
               }
               _loc4_++;
            }
            this._executable.splice(0,_loc2_);
         }
      }
      
      private function dispatchEvent(param1:Event) : void
      {
         var event:Event = param1;
         try
         {
            SocketManager.Instance.dispatchEvent(event);
            return;
         }
         catch(err:Error)
         {
            SocketManager.Instance.out.sendErrorMsg("type:" + event.type + "msg:" + err.message + "\r\n" + err.getStackTrace());
            return;
         }
      }
   }
}
