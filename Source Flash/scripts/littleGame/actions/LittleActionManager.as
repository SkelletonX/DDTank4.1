package littleGame.actions
{
   import ddt.ddt_internal;
   
   use namespace ddt_internal;
   
   public class LittleActionManager
   {
       
      
      ddt_internal var _queue:Array;
      
      public function LittleActionManager()
      {
         super();
         this._queue = new Array();
      }
      
      public function act(param1:LittleAction) : void
      {
         var _loc3_:LittleAction = null;
         var _loc2_:int = 0;
         while(_loc2_ < this._queue.length)
         {
            _loc3_ = this._queue[_loc2_];
            if(_loc3_.connect(param1))
            {
               return;
            }
            if(_loc3_.canReplace(param1))
            {
               param1.prepare();
               this._queue[_loc2_] = param1;
               return;
            }
            _loc2_++;
         }
         this._queue.push(param1);
         if(this._queue.length == 1)
         {
            param1.prepare();
         }
      }
      
      public function execute() : void
      {
         var _loc1_:LittleAction = null;
         if(this._queue.length > 0)
         {
            _loc1_ = this._queue[0];
            if(!_loc1_.isFinished)
            {
               _loc1_.execute();
            }
            else
            {
               this._queue.shift();
               if(this._queue.length > 0)
               {
                  this._queue[0].prepare();
               }
            }
         }
      }
      
      public function dispose() : void
      {
         var _loc1_:LittleAction = null;
         for each(_loc1_ in this._queue)
         {
            _loc1_.cancel();
         }
         this._queue = null;
      }
   }
}
