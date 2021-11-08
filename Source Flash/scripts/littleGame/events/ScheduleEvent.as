package littleGame.events
{
   import flash.events.Event;
   
   public class ScheduleEvent extends Event
   {
      
      public static const Complete:String = "complete";
       
      
      private var _paras:Array;
      
      public function ScheduleEvent(param1:String, ... rest)
      {
         this._paras = rest;
         super(param1);
      }
      
      public function get paras() : Array
      {
         return this._paras;
      }
   }
}
