package cmodule.decry
{
   import avm2.intrinsics.memory.li32;
   import avm2.intrinsics.memory.si32;
   
   public final class FSM__fini extends Machine
   {
       
      
      public function FSM__fini()
      {
         super();
      }
      
      public static function start() : void
      {
         var _loc1_:int = 0;
         var _loc2_:int = 0;
         FSM__fini.esp = FSM__fini.esp - 4;
         si32(FSM__fini.ebp,FSM__fini.esp);
         FSM__fini.ebp = FSM__fini.esp;
         FSM__fini.esp = FSM__fini.esp - 0;
         _loc1_ = FSM__fini;
         _loc2_ = 4;
         log(_loc2_,FSM__fini.gworker.stringFromPtr(_loc1_));
         FSM__fini.esp = FSM__fini.ebp;
         FSM__fini.ebp = li32(FSM__fini.esp);
         FSM__fini.esp = FSM__fini.esp + 4;
         FSM__fini.esp = FSM__fini.esp + 4;
      }
   }
}
