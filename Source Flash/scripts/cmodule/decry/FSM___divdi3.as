package cmodule.decry
{
   import avm2.intrinsics.memory.li32;
   import avm2.intrinsics.memory.si32;
   
   public final class FSM___divdi3 extends Machine
   {
       
      
      public function FSM___divdi3()
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
         var _loc9_:int = 0;
         FSM___divdi3.esp = FSM___divdi3.esp - 4;
         si32(FSM___divdi3.ebp,FSM___divdi3.esp);
         FSM___divdi3.ebp = FSM___divdi3.esp;
         FSM___divdi3.esp = FSM___divdi3.esp - 0;
         _loc1_ = 0;
         _loc2_ = li32(FSM___divdi3.ebp + 12);
         _loc3_ = li32(FSM___divdi3.ebp + 20);
         _loc4_ = li32(FSM___divdi3.ebp + 8);
         _loc5_ = _loc2_ >> 31;
         _loc6_ = li32(FSM___divdi3.ebp + 16);
         _loc7_ = _loc3_ >> 31;
         _loc4_ = __addc(_loc4_,_loc5_);
         _loc8_ = __adde(_loc2_,_loc5_);
         _loc6_ = __addc(_loc6_,_loc7_);
         _loc9_ = __adde(_loc3_,_loc7_);
         FSM___divdi3.esp = FSM___divdi3.esp - 20;
         _loc9_ = _loc9_ ^ _loc7_;
         _loc6_ = _loc6_ ^ _loc7_;
         _loc7_ = _loc8_ ^ _loc5_;
         _loc4_ = _loc4_ ^ _loc5_;
         si32(_loc4_,FSM___divdi3.esp);
         si32(_loc7_,FSM___divdi3.esp + 4);
         si32(_loc6_,FSM___divdi3.esp + 8);
         si32(_loc9_,FSM___divdi3.esp + 12);
         si32(_loc1_,FSM___divdi3.esp + 16);
         FSM___divdi3.esp = FSM___divdi3.esp - 4;
         FSM___divdi3.start();
         _loc1_ = FSM___divdi3.eax;
         _loc4_ = FSM___divdi3.edx;
         FSM___divdi3.esp = FSM___divdi3.esp + 20;
         _loc2_ = _loc2_ >>> 31;
         _loc3_ = _loc3_ >>> 31;
         if(_loc2_ != _loc3_)
         {
            _loc2_ = 0;
            _loc1_ = __subc(_loc2_,_loc1_);
            _loc4_ = __sube(_loc2_,_loc4_);
         }
         FSM___divdi3.edx = _loc4_;
         FSM___divdi3.eax = _loc1_;
         FSM___divdi3.esp = FSM___divdi3.ebp;
         FSM___divdi3.ebp = li32(FSM___divdi3.esp);
         FSM___divdi3.esp = FSM___divdi3.esp + 4;
         FSM___divdi3.esp = FSM___divdi3.esp + 4;
      }
   }
}
