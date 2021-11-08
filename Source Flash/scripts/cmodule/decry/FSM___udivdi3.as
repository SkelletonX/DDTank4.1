package cmodule.decry
{
   import avm2.intrinsics.memory.li32;
   import avm2.intrinsics.memory.si32;
   
   public final class FSM___udivdi3 extends Machine
   {
       
      
      public function FSM___udivdi3()
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
         FSM___udivdi3.esp = FSM___udivdi3.esp - 4;
         si32(FSM___udivdi3.ebp,FSM___udivdi3.esp);
         FSM___udivdi3.ebp = FSM___udivdi3.esp;
         FSM___udivdi3.esp = FSM___udivdi3.esp - 0;
         _loc1_ = 0;
         FSM___udivdi3.esp = FSM___udivdi3.esp - 20;
         _loc2_ = li32(FSM___udivdi3.ebp + 8);
         _loc3_ = li32(FSM___udivdi3.ebp + 12);
         _loc4_ = li32(FSM___udivdi3.ebp + 16);
         _loc5_ = li32(FSM___udivdi3.ebp + 20);
         si32(_loc2_,FSM___udivdi3.esp);
         si32(_loc3_,FSM___udivdi3.esp + 4);
         si32(_loc4_,FSM___udivdi3.esp + 8);
         si32(_loc5_,FSM___udivdi3.esp + 12);
         si32(_loc1_,FSM___udivdi3.esp + 16);
         FSM___udivdi3.esp = FSM___udivdi3.esp - 4;
         FSM___udivdi3.start();
         _loc1_ = FSM___udivdi3.eax;
         _loc2_ = FSM___udivdi3.edx;
         FSM___udivdi3.esp = FSM___udivdi3.esp + 20;
         FSM___udivdi3.edx = _loc2_;
         FSM___udivdi3.eax = _loc1_;
         FSM___udivdi3.esp = FSM___udivdi3.ebp;
         FSM___udivdi3.ebp = li32(FSM___udivdi3.esp);
         FSM___udivdi3.esp = FSM___udivdi3.esp + 4;
         FSM___udivdi3.esp = FSM___udivdi3.esp + 4;
      }
   }
}
