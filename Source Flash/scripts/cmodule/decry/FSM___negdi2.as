package cmodule.decry
{
   import avm2.intrinsics.memory.li32;
   import avm2.intrinsics.memory.si32;
   
   public final class FSM___negdi2 extends Machine
   {
       
      
      public function FSM___negdi2()
      {
         super();
      }
      
      public static function start() : void
      {
         var _loc1_:int = 0;
         var _loc2_:int = 0;
         var _loc3_:int = 0;
         var _loc4_:int = 0;
         FSM___negdi2.esp = FSM___negdi2.esp - 4;
         si32(FSM___negdi2.ebp,FSM___negdi2.esp);
         FSM___negdi2.ebp = FSM___negdi2.esp;
         FSM___negdi2.esp = FSM___negdi2.esp - 0;
         _loc1_ = 0;
         _loc2_ = li32(FSM___negdi2.ebp + 8);
         _loc3_ = li32(FSM___negdi2.ebp + 12);
         _loc4_ = _loc2_ != 0?int(1):int(0);
         _loc3_ = __subc(_loc1_,_loc3_);
         _loc4_ = _loc4_ & 1;
         _loc1_ = __subc(_loc1_,_loc2_);
         _loc2_ = __subc(_loc3_,_loc4_);
         FSM___negdi2.edx = _loc2_;
         FSM___negdi2.eax = _loc1_;
         FSM___negdi2.esp = FSM___negdi2.ebp;
         FSM___negdi2.ebp = li32(FSM___negdi2.esp);
         FSM___negdi2.esp = FSM___negdi2.esp + 4;
         FSM___negdi2.esp = FSM___negdi2.esp + 4;
      }
   }
}
