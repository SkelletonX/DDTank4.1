package cmodule.decry
{
   import avm2.intrinsics.memory.li32;
   import avm2.intrinsics.memory.si32;
   
   public final class FSM___floatdisf extends Machine
   {
       
      
      public function FSM___floatdisf()
      {
         super();
      }
      
      public static function start() : void
      {
         var _loc1_:int = 0;
         var _loc2_:int = 0;
         var _loc3_:int = 0;
         var _loc4_:int = 0;
         var _loc5_:Number = NaN;
         var _loc6_:Number = NaN;
         FSM___floatdisf.esp = FSM___floatdisf.esp - 4;
         si32(FSM___floatdisf.ebp,FSM___floatdisf.esp);
         FSM___floatdisf.ebp = FSM___floatdisf.esp;
         FSM___floatdisf.esp = FSM___floatdisf.esp - 0;
         _loc1_ = li32(FSM___floatdisf.ebp + 12);
         _loc2_ = li32(FSM___floatdisf.ebp + 8);
         _loc3_ = _loc1_ >> 31;
         _loc2_ = __addc(_loc2_,_loc3_);
         _loc4_ = __adde(_loc1_,_loc3_);
         _loc4_ = _loc4_ ^ _loc3_;
         _loc5_ = Number(uint(_loc4_));
         _loc2_ = _loc2_ ^ _loc3_;
         _loc5_ = _loc5_ * 4294970000;
         _loc6_ = Number(uint(_loc2_));
         _loc5_ = _loc5_;
         _loc5_ = _loc5_;
         _loc6_ = _loc6_;
         _loc5_ = _loc6_ + _loc5_;
         _loc5_ = _loc5_;
         if(_loc1_ <= -1)
         {
            _loc5_ = _loc5_;
            _loc5_ = -_loc5_;
            _loc5_ = _loc5_;
         }
         FSM___floatdisf.st0 = _loc5_;
         FSM___floatdisf.esp = FSM___floatdisf.ebp;
         FSM___floatdisf.ebp = li32(FSM___floatdisf.esp);
         FSM___floatdisf.esp = FSM___floatdisf.esp + 4;
         FSM___floatdisf.esp = FSM___floatdisf.esp + 4;
      }
   }
}
