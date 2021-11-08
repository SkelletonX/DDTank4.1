package cmodule.decry
{
   import avm2.intrinsics.memory.li32;
   import avm2.intrinsics.memory.si32;
   
   public final class FSM__exit extends Machine
   {
       
      
      public function FSM__exit()
      {
         super();
      }
      
      public static function start() : void
      {
         var _loc1_:int = 0;
         FSM__exit.esp = FSM__exit.esp - 4;
         si32(FSM__exit.ebp,FSM__exit.esp);
         FSM__exit.ebp = FSM__exit.esp;
         FSM__exit.esp = FSM__exit.esp - 0;
         _loc1_ = li32(FSM__exit.ebp + 8);
         throw new AlchemyExit(_loc1_);
      }
   }
}
