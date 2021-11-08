package cmodule.decry
{
   import avm2.intrinsics.memory.li32;
   import avm2.intrinsics.memory.li8;
   import avm2.intrinsics.memory.si32;
   
   public final class FSM_malloc_pages extends Machine
   {
       
      
      public function FSM_malloc_pages()
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
         FSM_malloc_pages.esp = FSM_malloc_pages.esp - 4;
         si32(FSM_malloc_pages.ebp,FSM_malloc_pages.esp);
         FSM_malloc_pages.ebp = FSM_malloc_pages.esp;
         FSM_malloc_pages.esp = FSM_malloc_pages.esp - 0;
         _loc1_ = li32(FSM_malloc_pages.ebp + 8);
         _loc1_ = _loc1_ + 4095;
         _loc2_ = li32(FSM_malloc_pages);
         _loc3_ = _loc1_ & -4096;
         if(_loc2_ != 0)
         {
            _loc1_ = _loc2_;
            while(true)
            {
               _loc2_ = li32(_loc1_ + 16);
               _loc4_ = _loc1_ + 16;
               if(uint(_loc2_) >= uint(_loc3_))
               {
                  _loc5_ = li32(_loc1_ + 8);
                  _loc6_ = _loc1_ + 8;
                  if(_loc2_ == _loc3_)
                  {
                     _loc2_ = li32(_loc1_);
                     _loc4_ = _loc1_;
                     if(_loc2_ != 0)
                     {
                        _loc6_ = li32(_loc1_ + 4);
                        si32(_loc6_,_loc2_ + 4);
                     }
                     _loc2_ = li32(_loc1_ + 4);
                     _loc4_ = li32(_loc4_);
                     si32(_loc4_,_loc2_);
                     _loc2_ = _loc3_ >>> 12;
                     if(_loc5_ != 0)
                     {
                        _loc4_ = _loc5_;
                     }
                     else
                     {
                        break;
                     }
                  }
                  else
                  {
                     _loc1_ = _loc5_ + _loc3_;
                     si32(_loc1_,_loc6_);
                     _loc1_ = _loc2_ - _loc3_;
                     si32(_loc1_,_loc4_);
                     _loc2_ = _loc3_ >>> 12;
                     if(_loc5_ != 0)
                     {
                        _loc1_ = 0;
                        _loc4_ = _loc5_;
                     }
                     else
                     {
                        _loc1_ = 0;
                        break;
                     }
                  }
                  addr485:
                  if(_loc4_ != 0)
                  {
                     _loc5_ = 2;
                     _loc6_ = li32(FSM_malloc_pages);
                     _loc7_ = _loc4_ >>> 12;
                     _loc8_ = _loc7_ - _loc6_;
                     _loc9_ = li32(FSM_malloc_pages);
                     _loc8_ = _loc8_ << 2;
                     _loc8_ = _loc9_ + _loc8_;
                     si32(_loc5_,_loc8_);
                     if(uint(_loc2_) >= uint(2))
                     {
                        _loc5_ = 0;
                        _loc6_ = _loc7_ - _loc6_;
                        _loc6_ = _loc6_ << 2;
                        _loc6_ = _loc6_ + _loc9_;
                        _loc6_ = _loc6_ + 4;
                        _loc2_ = _loc2_ + -1;
                        while(true)
                        {
                           _loc7_ = 3;
                           si32(_loc7_,_loc6_);
                           _loc6_ = _loc6_ + 4;
                           _loc5_ = _loc5_ + 1;
                           if(_loc5_ != _loc2_)
                           {
                              continue;
                           }
                           break;
                        }
                     }
                     _loc2_ = li8(FSM_malloc_pages);
                     _loc2_ = _loc2_ ^ 1;
                     _loc2_ = _loc2_ & 1;
                     if(_loc2_ == 0)
                     {
                        _loc2_ = -48;
                        _loc5_ = _loc4_;
                        memset(_loc5_,_loc2_,_loc3_);
                     }
                  }
                  if(_loc1_ != 0)
                  {
                     _loc2_ = li32(FSM_malloc_pages);
                     if(_loc2_ == 0)
                     {
                        si32(_loc1_,FSM_malloc_pages);
                     }
                     else
                     {
                        FSM_malloc_pages.esp = FSM_malloc_pages.esp - 4;
                        si32(_loc1_,FSM_malloc_pages.esp);
                        FSM_malloc_pages.esp = FSM_malloc_pages.esp - 4;
                        FSM_malloc_pages.start();
                        FSM_malloc_pages.esp = FSM_malloc_pages.esp + 4;
                     }
                  }
                  FSM_malloc_pages.eax = _loc4_;
                  FSM_malloc_pages.esp = FSM_malloc_pages.ebp;
                  FSM_malloc_pages.ebp = li32(FSM_malloc_pages.esp);
                  FSM_malloc_pages.esp = FSM_malloc_pages.esp + 4;
                  FSM_malloc_pages.esp = FSM_malloc_pages.esp + 4;
                  return;
               }
               _loc1_ = li32(_loc1_);
               if(_loc1_ != 0)
               {
                  continue;
               }
            }
            _loc4_ = _loc1_;
            _loc5_ = _loc2_;
            _loc1_ = 0;
            _loc1_ = _sbrk(_loc1_);
            _loc1_ = _loc1_ + 4095;
            _loc6_ = _loc1_ & -4096;
            _loc1_ = _loc6_ + _loc3_;
            if(uint(_loc1_) >= uint(_loc6_))
            {
               _loc2_ = _loc1_;
               _loc2_ = _brk(_loc2_);
               if(_loc2_ == 0)
               {
                  _loc2_ = _loc1_ >>> 12;
                  _loc7_ = li32(FSM_malloc_pages);
                  _loc2_ = _loc2_ + -1;
                  _loc7_ = _loc2_ - _loc7_;
                  si32(_loc7_,FSM_malloc_pages);
                  si32(_loc1_,FSM_malloc_pages);
                  _loc1_ = li32(FSM_malloc_pages);
                  _loc2_ = _loc7_ + 1;
                  if(uint(_loc2_) < uint(_loc1_))
                  {
                     _loc1_ = _loc4_;
                     _loc2_ = _loc5_;
                     _loc4_ = _loc6_;
                  }
                  else
                  {
                     _loc1_ = FSM_malloc_pages;
                     _loc2_ = 4;
                     _loc8_ = 0;
                     log(_loc2_,FSM_malloc_pages.gworker.stringFromPtr(_loc1_));
                     _loc1_ = _sbrk(_loc8_);
                     _loc1_ = _loc1_ & 4095;
                     _loc1_ = 4096 - _loc1_;
                     _loc2_ = _loc7_ >>> 9;
                     _loc1_ = _loc1_ & 4095;
                     _loc2_ = _loc2_ & 1048575;
                     _loc2_ = _loc2_ + 2;
                     _loc1_ = _sbrk(_loc1_);
                     _loc1_ = _loc2_ << 12;
                     _loc1_ = _sbrk(_loc1_);
                     _loc7_ = _loc1_;
                     if(_loc1_ != -1)
                     {
                        _loc8_ = FSM_malloc_pages;
                        _loc9_ = li32(FSM_malloc_pages);
                        _loc10_ = li32(FSM_malloc_pages);
                        _loc9_ = _loc9_ << 2;
                        _loc2_ = _loc2_ << 10;
                        memcpy(_loc1_,_loc10_,_loc9_);
                        _loc1_ = _loc2_ & 1073740800;
                        si32(_loc1_,FSM_malloc_pages);
                        si32(_loc7_,FSM_malloc_pages);
                        _loc2_ = 4;
                        _loc1_ = _loc8_;
                        log(_loc2_,FSM_malloc_pages.gworker.stringFromPtr(_loc1_));
                        _loc1_ = _loc4_;
                        _loc2_ = _loc5_;
                        _loc4_ = _loc6_;
                     }
                  }
                  §§goto(addr485);
               }
            }
            _loc6_ = 0;
            _loc1_ = _loc4_;
            _loc2_ = _loc5_;
            _loc4_ = _loc6_;
            §§goto(addr485);
         }
         _loc1_ = 0;
         _loc2_ = _loc3_ >>> 12;
         §§goto(addr221);
      }
   }
}
