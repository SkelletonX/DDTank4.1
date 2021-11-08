package littleGame.events
{
   import flash.events.Event;
   
   public class LittleGameEvent extends Event
   {
      
      public static const AddLiving:String = "addLiving";
      
      public static const RemoveLiving:String = "removeLiving";
      
      public static const Update:String = "update";
      
      public static const Mark:String = "mark";
      
      public static const Startup:String = "startup";
      
      public static const Shutdown:String = "shutdown";
      
      public static const SoundEnabledChanged:String = "soundEnabledChanged";
      
      public static const ActivedChanged:String = "activedChanged";
      
      public static const SelfInhaleChanged:String = "selfInhaledChanged";
       
      
      private var _paras:Array;
      
      public function LittleGameEvent(param1:String, ... rest)
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
