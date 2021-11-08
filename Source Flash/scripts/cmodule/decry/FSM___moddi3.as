package cmodule.decry
{
   import avm2.intrinsics.memory.li32;
   import avm2.intrinsics.memory.si32;
   
   public final class FSM___moddi3 extends Machine
   {
       
      
      public function FSM___moddi3()
      {
         super();
      }
      
      public static function start() : void
      {
         var _loc1_:int = 0;
         var _loc2_:int = 0;
         var _loc3_:int = 0;
         var _loc4_:int = 0;
         var _loc5_:int = 0;
         var _loc6_:int = 0;
         var _loc7_:int = 0;
         var _loc8_:int = 0;
         FSM___moddi3.esp = FSM___moddi3.esp - 4;
         si32(FSM___moddi3.ebp,FSM___moddi3.esp);
         FSM___moddi3.ebp = FSM___moddi3.esp;
         FSM___moddi3.esp = FSM___moddi3.esp - 8;
         _loc1_ = FSM___moddi3.ebp + -8;
         _loc2_ = li32(FSM___moddi3.ebp + 12);
         _loc3_ = li32(FSM___moddi3.ebp + 20);
         _loc4_ = li32(FSM___moddi3.ebp + 8);
         _loc5_ = _loc2_ >> 31;
         _loc6_ = li32(FSM___moddi3.ebp + 16);
         _loc7_ = _loc3_ >> 31;
         _loc4_ = __addc(_loc4_,_loc5_);
         _loc8_ = __adde(_loc2_,_loc5_);
         _loc6_ = __addc(_loc6_,_loc7_);
         _loc3_ = __adde(_loc3_,_loc7_);
         FSM___moddi3.esp = FSM___moddi3.esp - 20;
         _loc3_ = _loc3_ ^ _loc7_;
         _loc6_ = _loc6_ ^ _loc7_;
         _loc7_ = _loc8_ ^ _loc5_;
         _loc4_ = _loc4_ ^ _loc5_;
         si32(_loc4_,FSM___moddi3.esp);
         si32(_loc7_,FSM___moddi3.esp + 4);
         si32(_loc6_,FSM___moddi3.esp + 8);
         si32(_loc3_,FSM___moddi3.esp + 12);
         si32(_loc1_,FSM___moddi3.esp + 16);
         FSM___moddi3.esp = FSM___moddi3.esp - 4;
         FSM___moddi3.start();
         _loc1_ = FSM___moddi3.eax;
         _loc1_ = FSM___moddi3.edx;
         FSM___moddi3.esp = FSM___moddi3.esp + 20;
         _loc1_ = li32(FSM___moddi3.ebp + -8);
         _loc3_ = li32(FSM___moddi3.ebp + -4);
         if(_loc2_ <= -1)
         {
            _loc2_ = 0;
            _loc1_ = __subc(_loc2_,_loc1_);
            _loc3_ = __sube(_loc2_,_loc3_);
         }
         FSM___moddi3.edx = _loc3_;
         FSM___moddi3.eax = _loc1_;
         FSM___moddi3.esp = FSM___moddi3.ebp;
         FSM___moddi3.ebp = li32(FSM___moddi3.esp);
         FSM___moddi3.esp = FSM___moddi3.esp + 4;
         FSM___moddi3.esp = FSM___moddi3.esp + 4;
      }
   }
}
