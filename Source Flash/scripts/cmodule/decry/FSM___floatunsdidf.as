package cmodule.decry
{
   import avm2.intrinsics.memory.li32;
   import avm2.intrinsics.memory.si32;
   
   public final class FSM___floatunsdidf extends Machine
   {
       
      
      public function FSM___floatunsdidf()
      {
         super();
      }
      
      public static function start() : void
      {
         var _loc1_:int = 0;
         var _loc2_:int = 0;
         var _loc3_:Number = NaN;
         var _loc4_:Number = NaN;
         FSM___floatunsdidf.esp = FSM___floatunsdidf.esp - 4;
         si32(FSM___floatunsdidf.ebp,FSM___floatunsdidf.esp);
         FSM___floatunsdidf.ebp = FSM___floatunsdidf.esp;
         FSM___floatunsdidf.esp = FSM___floatunsdidf.esp - 0;
         _loc1_ = li32(FSM___floatunsdidf.ebp + 12);
         _loc2_ = li32(FSM___floatunsdidf.ebp + 8);
         _loc3_ = Number(uint(_loc1_));
         _loc4_ = Number(uint(_loc2_));
         _loc3_ = _loc3_ * 4294970000;
         _loc3_ = _loc4_ + _loc3_;
         FSM___floatunsdidf.st0 = _loc3_;
         FSM___floatunsdidf.esp = FSM___floatunsdidf.ebp;
         FSM___floatunsdidf.ebp = li32(FSM___floatunsdidf.esp);
         FSM___floatunsdidf.esp = FSM___floatunsdidf.esp + 4;
         FSM___floatunsdidf.esp = FSM___floatunsdidf.esp + 4;
      }
   }
}
