package cmodule.decry
{
   import avm2.intrinsics.memory.li32;
   import avm2.intrinsics.memory.si32;
   
   public final class FSM___trailz_D2A extends Machine
   {
       
      
      public function FSM___trailz_D2A()
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
         FSM___trailz_D2A.esp = FSM___trailz_D2A.esp - 4;
         si32(FSM___trailz_D2A.ebp,FSM___trailz_D2A.esp);
         FSM___trailz_D2A.ebp = FSM___trailz_D2A.esp;
         FSM___trailz_D2A.esp = FSM___trailz_D2A.esp - 4;
         _loc1_ = li32(FSM___trailz_D2A.ebp + 8);
         _loc2_ = li32(_loc1_ + 16);
         _loc3_ = _loc1_ + 20;
         _loc4_ = _loc2_ << 2;
         _loc4_ = _loc3_ + _loc4_;
         if(_loc2_ <= 0)
         {
            _loc1_ = 0;
            _loc2_ = _loc3_;
         }
         else
         {
            _loc3_ = 0;
            _loc1_ = _loc1_ + 20;
            _loc5_ = _loc3_;
            while(true)
            {
               _loc6_ = li32(_loc1_);
               _loc7_ = _loc1_;
               if(_loc6_ != 0)
               {
                  _loc2_ = _loc7_;
                  _loc1_ = _loc5_;
                  break;
               }
               _loc5_ = _loc5_ + 32;
               _loc1_ = _loc1_ + 4;
               _loc3_ = _loc3_ + 1;
               _loc6_ = _loc1_;
               if(_loc3_ >= _loc2_)
               {
                  _loc2_ = _loc6_;
                  _loc1_ = _loc5_;
                  break;
               }
            }
         }
         if(uint(_loc2_) < uint(_loc4_))
         {
            _loc3_ = FSM___trailz_D2A.ebp + -4;
            _loc2_ = li32(_loc2_);
            si32(_loc2_,FSM___trailz_D2A.ebp + -4);
            FSM___trailz_D2A.esp = FSM___trailz_D2A.esp - 4;
            si32(_loc3_,FSM___trailz_D2A.esp);
            FSM___trailz_D2A.esp = FSM___trailz_D2A.esp - 4;
            FSM___trailz_D2A.start();
            _loc2_ = FSM___trailz_D2A.eax;
            FSM___trailz_D2A.esp = FSM___trailz_D2A.esp + 4;
            _loc1_ = _loc2_ + _loc1_;
         }
         FSM___trailz_D2A.eax = _loc1_;
         FSM___trailz_D2A.esp = FSM___trailz_D2A.ebp;
         FSM___trailz_D2A.ebp = li32(FSM___trailz_D2A.esp);
         FSM___trailz_D2A.esp = FSM___trailz_D2A.esp + 4;
         FSM___trailz_D2A.esp = FSM___trailz_D2A.esp + 4;
      }
   }
}
