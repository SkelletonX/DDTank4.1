package littleGame.clock
{
   public class TimeDelta
   {
       
      
      private var _latency:int;
      
      private var _timeSyncDelta:int;
      
      public function TimeDelta(param1:int, param2:int)
      {
         super();
         this._latency = param1;
         this._timeSyncDelta = param2;
      }
      
      public function get latency() : int
      {
         return this._latency;
      }
      
      public function get timeSyncDelta() : int
      {
         return this._timeSyncDelta;
      }
   }
}
