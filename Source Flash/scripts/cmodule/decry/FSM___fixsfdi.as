package cmodule.decry
{
   import avm2.intrinsics.memory.lf32;
   import avm2.intrinsics.memory.li32;
   import avm2.intrinsics.memory.sf32;
   import avm2.intrinsics.memory.si32;
   
   public final class FSM___fixsfdi extends Machine
   {
       
      
      public function FSM___fixsfdi()
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
         var _loc6_:Number = NaN;
         FSM___fixsfdi.esp = FSM___fixsfdi.esp - 4;
         si32(FSM___fixsfdi.ebp,FSM___fixsfdi.esp);
         FSM___fixsfdi.ebp = FSM___fixsfdi.esp;
         FSM___fixsfdi.esp = FSM___fixsfdi.esp - 0;
         _loc4_ = 0;
         _loc5_ = lf32(FSM___fixsfdi.ebp + 8);
         _loc4_ = _loc4_;
         _loc4_ = _loc4_;
         _loc6_ = _loc5_;
         if(_loc6_ < _loc4_)
         {
            _loc4_ = -9223370000000000000;
            _loc4_ = _loc4_;
            _loc4_ = _loc4_;
            _loc6_ = _loc5_;
            if(_loc6_ <= _loc4_)
            {
               _loc1_ = -2147483648;
               _loc2_ = 0;
            }
            else
            {
               _loc1_ = 0;
               _loc4_ = _loc5_;
               _loc4_ = -_loc4_;
               FSM___fixsfdi.esp = FSM___fixsfdi.esp - 4;
               _loc5_ = _loc4_;
               sf32(_loc5_,FSM___fixsfdi.esp);
               FSM___fixsfdi.esp = FSM___fixsfdi.esp - 4;
               FSM___fixsfdi.funcs[FSM___fixsfdi]();
               _loc2_ = FSM___fixsfdi.eax;
               _loc3_ = FSM___fixsfdi.edx;
               FSM___fixsfdi.esp = FSM___fixsfdi.esp + 4;
               _loc2_ = __subc(_loc1_,_loc2_);
               _loc1_ = __sube(_loc1_,_loc3_);
            }
            addr245:
            FSM___fixsfdi.edx = _loc1_;
            FSM___fixsfdi.eax = _loc2_;
         }
         else
         {
            _loc4_ = 9223370000000000000;
            _loc4_ = _loc4_;
            _loc4_ = _loc4_;
            _loc6_ = _loc5_;
            if(_loc6_ >= _loc4_)
            {
               _loc1_ = 2147483647;
               _loc2_ = -1;
               §§goto(addr245);
            }
            else
            {
               FSM___fixsfdi.esp = FSM___fixsfdi.esp - 4;
               sf32(_loc5_,FSM___fixsfdi.esp);
               FSM___fixsfdi.esp = FSM___fixsfdi.esp - 4;
               FSM___fixsfdi.funcs[FSM___fixsfdi]();
               _loc1_ = FSM___fixsfdi.eax;
               _loc2_ = FSM___fixsfdi.edx;
               FSM___fixsfdi.esp = FSM___fixsfdi.esp + 4;
               FSM___fixsfdi.edx = _loc2_;
               FSM___fixsfdi.eax = _loc1_;
            }
         }
         FSM___fixsfdi.esp = FSM___fixsfdi.ebp;
         FSM___fixsfdi.ebp = li32(FSM___fixsfdi.esp);
         FSM___fixsfdi.esp = FSM___fixsfdi.esp + 4;
         FSM___fixsfdi.esp = FSM___fixsfdi.esp + 4;
      }
   }
}
