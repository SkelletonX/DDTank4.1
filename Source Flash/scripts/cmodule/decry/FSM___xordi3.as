package cmodule.decry
{
   import avm2.intrinsics.memory.li32;
   import avm2.intrinsics.memory.si32;
   
   public final class FSM___xordi3 extends Machine
   {
       
      
      public function FSM___xordi3()
      {
         super();
      }
      
      public static function start() : void
      {
         var _loc1_:int = 0;
         var _loc2_:int = 0;
         var _loc3_:int = 0;
         var _loc4_:int = 0;
         FSM___xordi3.esp = FSM___xordi3.esp - 4;
         si32(FSM___xordi3.ebp,FSM___xordi3.esp);
         FSM___xordi3.ebp = FSM___xordi3.esp;
         FSM___xordi3.esp = FSM___xordi3.esp - 0;
         _loc1_ = li32(FSM___xordi3.ebp + 16);
         _loc2_ = li32(FSM___xordi3.ebp + 8);
         _loc3_ = li32(FSM___xordi3.ebp + 12);
         _loc4_ = li32(FSM___xordi3.ebp + 20);
         _loc3_ = _loc3_ ^ _loc4_;
         _loc1_ = _loc1_ ^ _loc2_;
         FSM___xordi3.edx = _loc3_;
         FSM___xordi3.eax = _loc1_;
         FSM___xordi3.esp = FSM___xordi3.ebp;
         FSM___xordi3.ebp = li32(FSM___xordi3.esp);
         FSM___xordi3.esp = FSM___xordi3.esp + 4;
         FSM___xordi3.esp = FSM___xordi3.esp + 4;
      }
   }
}
