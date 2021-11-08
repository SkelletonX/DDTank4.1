package cmodule.decry
{
   import avm2.intrinsics.memory.li32;
   import avm2.intrinsics.memory.si32;
   
   public final class FSM___error extends Machine
   {
       
      
      public function FSM___error()
      {
         super();
      }
      
      public static function start() : void
      {
         var _loc1_:int = 0;
         FSM___error.esp = FSM___error.esp - 4;
         si32(FSM___error.ebp,FSM___error.esp);
         FSM___error.ebp = FSM___error.esp;
         FSM___error.esp = FSM___error.esp - 0;
         _loc1_ = FSM___error;
         FSM___error.eax = _loc1_;
         FSM___error.esp = FSM___error.ebp;
         FSM___error.ebp = li32(FSM___error.esp);
         FSM___error.esp = FSM___error.esp + 4;
         FSM___error.esp = FSM___error.esp + 4;
      }
   }
}
