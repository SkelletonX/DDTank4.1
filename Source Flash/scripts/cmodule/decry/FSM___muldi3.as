package cmodule.decry
{
   import avm2.intrinsics.memory.li32;
   import avm2.intrinsics.memory.si32;
   
   public final class FSM___muldi3 extends Machine
   {
       
      
      public function FSM___muldi3()
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
         var _loc10_:int = 0;
         var _loc11_:int = 0;
         var _loc12_:int = 0;
         FSM___muldi3.esp = FSM___muldi3.esp - 4;
         si32(FSM___muldi3.ebp,FSM___muldi3.esp);
         FSM___muldi3.ebp = FSM___muldi3.esp;
         FSM___muldi3.esp = FSM___muldi3.esp - 0;
         _loc1_ = 0;
         _loc1_ = li32(FSM___muldi3.ebp + 12);
         _loc2_ = li32(FSM___muldi3.ebp + 20);
         _loc3_ = li32(FSM___muldi3.ebp + 8);
         _loc4_ = _loc1_ >> 31;
         _loc5_ = li32(FSM___muldi3.ebp + 16);
         _loc6_ = _loc2_ >> 31;
         _loc3_ = __addc(_loc3_,_loc4_);
         _loc7_ = __adde(_loc1_,_loc4_);
         _loc5_ = __addc(_loc5_,_loc6_);
         _loc8_ = __adde(_loc2_,_loc6_);
         _loc1_ = _loc1_ >>> 31;
         _loc7_ = _loc7_ ^ _loc4_;
         _loc3_ = _loc3_ ^ _loc4_;
         _loc2_ = _loc2_ >>> 31;
         _loc4_ = _loc8_ ^ _loc6_;
         _loc5_ = _loc5_ ^ _loc6_;
         _loc6_ = _loc7_;
         _loc8_ = _loc7_;
         if(_loc7_ == 0)
         {
            if(_loc4_ == 0)
            {
               FSM___muldi3.esp = FSM___muldi3.esp - 8;
               si32(_loc3_,FSM___muldi3.esp);
               si32(_loc5_,FSM___muldi3.esp + 4);
               FSM___muldi3.esp = FSM___muldi3.esp - 4;
               FSM___muldi3.start();
               _loc3_ = FSM___muldi3.eax;
               _loc4_ = FSM___muldi3.edx;
               FSM___muldi3.esp = FSM___muldi3.esp + 8;
               if(_loc1_ != _loc2_)
               {
                  addr428:
                  _loc1_ = 0;
                  _loc3_ = __subc(_loc1_,_loc3_);
                  _loc4_ = __sube(_loc1_,_loc4_);
                  FSM___muldi3.edx = _loc4_;
                  FSM___muldi3.eax = _loc3_;
               }
               addr469:
               FSM___muldi3.esp = FSM___muldi3.ebp;
               FSM___muldi3.ebp = li32(FSM___muldi3.esp);
               FSM___muldi3.esp = FSM___muldi3.esp + 4;
               FSM___muldi3.esp = FSM___muldi3.esp + 4;
               return;
            }
            addr455:
            _loc1_ = _loc3_;
            _loc2_ = _loc4_;
            FSM___muldi3.edx = _loc2_;
            FSM___muldi3.eax = _loc1_;
            §§goto(addr469);
         }
         _loc7_ = uint(_loc5_) < uint(_loc4_)?int(_loc5_):int(_loc4_);
         _loc9_ = uint(_loc5_) < uint(_loc4_)?int(_loc4_):int(_loc5_);
         _loc10_ = uint(_loc6_) < uint(_loc3_)?int(_loc8_):int(_loc3_);
         _loc8_ = uint(_loc6_) < uint(_loc3_)?int(_loc3_):int(_loc8_);
         _loc11_ = uint(_loc5_) < uint(_loc4_)?int(1):int(0);
         _loc12_ = uint(_loc6_) < uint(_loc3_)?int(1):int(0);
         _loc7_ = _loc9_ - _loc7_;
         _loc8_ = _loc8_ - _loc10_;
         FSM___muldi3.esp = FSM___muldi3.esp - 8;
         _loc9_ = _loc11_ ^ _loc12_;
         _loc7_ = _loc7_ * _loc8_;
         si32(_loc3_,FSM___muldi3.esp);
         si32(_loc5_,FSM___muldi3.esp + 4);
         _loc3_ = _loc9_ & 1;
         _loc5_ = 0 - _loc7_;
         _loc3_ = _loc3_ != 0?int(_loc5_):int(_loc7_);
         _loc4_ = _loc4_ * _loc6_;
         FSM___muldi3.esp = FSM___muldi3.esp - 4;
         FSM___muldi3.start();
         _loc5_ = FSM___muldi3.eax;
         _loc6_ = FSM___muldi3.edx;
         _loc3_ = _loc3_ + _loc4_;
         _loc3_ = _loc3_ + _loc5_;
         FSM___muldi3.esp = FSM___muldi3.esp + 8;
         _loc4_ = _loc3_ + _loc6_;
         if(_loc1_ != _loc2_)
         {
            _loc3_ = _loc5_;
            §§goto(addr428);
         }
         else
         {
            _loc3_ = _loc5_;
            §§goto(addr455);
         }
         §§goto(addr469);
      }
   }
}
