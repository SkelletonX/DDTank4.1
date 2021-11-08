package cmodule.decry
{
   import avm2.intrinsics.memory.li32;
   import avm2.intrinsics.memory.si32;
   
   public final class FSM___one_cmpldi2 extends Machine
   {
       
      
      public function FSM___one_cmpldi2()
      {
         super();
      }
      
      public static function start() : void
      {
         var _loc1_:int = 0;
         var _loc2_:int = 0;
         FSM___one_cmpldi2.esp = FSM___one_cmpldi2.esp - 4;
         si32(FSM___one_cmpldi2.ebp,FSM___one_cmpldi2.esp);
         FSM___one_cmpldi2.ebp = FSM___one_cmpldi2.esp;
         FSM___one_cmpldi2.esp = FSM___one_cmpldi2.esp - 0;
         _loc1_ = li32(FSM___one_cmpldi2.ebp + 8);
         _loc2_ = li32(FSM___one_cmpldi2.ebp + 12);
         _loc2_ = _loc2_ ^ -1;
         _loc1_ = _loc1_ ^ -1;
         FSM___one_cmpldi2.edx = _loc2_;
         FSM___one_cmpldi2.eax = _loc1_;
         FSM___one_cmpldi2.esp = FSM___one_cmpldi2.ebp;
         FSM___one_cmpldi2.ebp = li32(FSM___one_cmpldi2.esp);
         FSM___one_cmpldi2.esp = FSM___one_cmpldi2.esp + 4;
         FSM___one_cmpldi2.esp = FSM___one_cmpldi2.esp + 4;
      }
   }
}
