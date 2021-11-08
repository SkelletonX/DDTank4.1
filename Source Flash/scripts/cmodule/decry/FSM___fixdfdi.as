package cmodule.decry
{
   import avm2.intrinsics.memory.lf64;
   import avm2.intrinsics.memory.li32;
   import avm2.intrinsics.memory.sf64;
   import avm2.intrinsics.memory.si32;
   
   public final class FSM___fixdfdi extends Machine
   {
       
      
      public function FSM___fixdfdi()
      {
         super();
      }
      
      public static function start() : void
      {
         var _loc1_:int = 0;
         var _loc2_:int = 0;
         var _loc3_:int = 0;
         var _loc4_:Number = NaN;
         var _loc5_:Number = NaN;
         FSM___fixdfdi.esp = FSM___fixdfdi.esp - 4;
         si32(FSM___fixdfdi.ebp,FSM___fixdfdi.esp);
         FSM___fixdfdi.ebp = FSM___fixdfdi.esp;
         FSM___fixdfdi.esp = FSM___fixdfdi.esp - 0;
         _loc4_ = 0;
         _loc5_ = lf64(FSM___fixdfdi.ebp + 8);
         if(_loc5_ < _loc4_)
         {
            _loc4_ = -9223370000000000000;
            if(_loc5_ <= _loc4_)
            {
               _loc1_ = -2147483648;
               _loc2_ = 0;
            }
            else
            {
               _loc1_ = 0;
               FSM___fixdfdi.esp = FSM___fixdfdi.esp - 8;
               _loc5_ = -_loc5_;
               sf64(_loc5_,FSM___fixdfdi.esp);
               FSM___fixdfdi.esp = FSM___fixdfdi.esp - 4;
               FSM___fixdfdi.funcs[FSM___fixdfdi]();
               _loc2_ = FSM___fixdfdi.eax;
               _loc3_ = FSM___fixdfdi.edx;
               FSM___fixdfdi.esp = FSM___fixdfdi.esp + 8;
               _loc2_ = __subc(_loc1_,_loc2_);
               _loc1_ = __sube(_loc1_,_loc3_);
            }
            addr210:
            FSM___fixdfdi.edx = _loc1_;
            FSM___fixdfdi.eax = _loc2_;
         }
         else
         {
            _loc4_ = 9223370000000000000;
            if(_loc5_ >= _loc4_)
            {
               _loc1_ = 2147483647;
               _loc2_ = -1;
               §§goto(addr210);
            }
            else
            {
               FSM___fixdfdi.esp = FSM___fixdfdi.esp - 8;
               sf64(_loc5_,FSM___fixdfdi.esp);
               FSM___fixdfdi.esp = FSM___fixdfdi.esp - 4;
               FSM___fixdfdi.funcs[FSM___fixdfdi]();
               _loc1_ = FSM___fixdfdi.eax;
               _loc2_ = FSM___fixdfdi.edx;
               FSM___fixdfdi.esp = FSM___fixdfdi.esp + 8;
               FSM___fixdfdi.edx = _loc2_;
               FSM___fixdfdi.eax = _loc1_;
            }
         }
         FSM___fixdfdi.esp = FSM___fixdfdi.ebp;
         FSM___fixdfdi.ebp = li32(FSM___fixdfdi.esp);
         FSM___fixdfdi.esp = FSM___fixdfdi.esp + 4;
         FSM___fixdfdi.esp = FSM___fixdfdi.esp + 4;
      }
   }
}
