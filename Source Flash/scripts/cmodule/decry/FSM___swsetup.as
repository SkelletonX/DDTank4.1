package cmodule.decry
{
   import avm2.intrinsics.memory.li16;
   import avm2.intrinsics.memory.li32;
   import avm2.intrinsics.memory.li8;
   import avm2.intrinsics.memory.si16;
   import avm2.intrinsics.memory.si32;
   import avm2.intrinsics.memory.si8;
   
   public final class FSM___swsetup extends Machine
   {
      
      public static const intRegCount:int = 5;
      
      public static const NumberRegCount:int = 0;
       
      
      public var i0:int;
      
      public var i1:int;
      
      public var i2:int;
      
      public var i3:int;
      
      public var i4:int;
      
      public function FSM___swsetup()
      {
         super();
      }
      
      public static function start() : void
      {
         var _loc1_:FSM___swsetup = null;
         _loc1_ = new FSM___swsetup();
         FSM___swsetup.gworker = _loc1_;
      }
      
      override public final function work() : void
      {
         switch(state)
         {
            case 0:
               mstate.esp = mstate.esp - 4;
               si32(mstate.ebp,mstate.esp);
               mstate.ebp = mstate.esp;
               mstate.esp = mstate.esp - 0;
               this.i0 = li32(mstate.ebp + 8);
               this.i1 = li8(FSM___swsetup);
               if(this.i1 == 0)
               {
                  this.i1 = FSM___swsetup;
                  this.i2 = FSM___swsetup;
                  this.i3 = 0;
                  this.i1 = this.i1 + 56;
                  while(true)
                  {
                     si32(this.i2,this.i1);
                     this.i2 = this.i2 + 152;
                     this.i1 = this.i1 + 88;
                     this.i3 = this.i3 + 1;
                     if(this.i3 != 17)
                     {
                        continue;
                     }
                     break;
                  }
                  this.i1 = 1;
                  si8(this.i1,FSM___swsetup);
                  si8(this.i1,FSM___swsetup);
               }
               this.i1 = li16(this.i0 + 12);
               this.i2 = this.i0 + 12;
               this.i3 = this.i1;
               this.i4 = this.i1 & 8;
               if(this.i4 == 0)
               {
                  this.i4 = this.i3 & 16;
                  if(this.i4 == 0)
                  {
                     this.i0 = 9;
                     si32(this.i0,FSM___swsetup);
                     this.i0 = -1;
                     addr471:
                     addr476:
                     mstate.eax = this.i0;
                     mstate.esp = mstate.ebp;
                     mstate.ebp = li32(mstate.esp);
                     mstate.esp = mstate.esp + 4;
                     mstate.esp = mstate.esp + 4;
                     mstate.gworker = caller;
                     return;
                  }
                  this.i3 = this.i3 & 4;
                  if(this.i3 != 0)
                  {
                     this.i1 = li32(this.i0 + 48);
                     this.i3 = this.i0 + 48;
                     if(this.i1 != 0)
                     {
                        this.i4 = this.i0 + 64;
                        if(this.i1 != this.i4)
                        {
                           this.i4 = 0;
                           mstate.esp = mstate.esp - 8;
                           si32(this.i1,mstate.esp);
                           si32(this.i4,mstate.esp + 4);
                           state = 1;
                           mstate.esp = mstate.esp - 4;
                           FSM___swsetup.start();
                           return;
                        }
                        addr265:
                        this.i1 = 0;
                        si32(this.i1,this.i3);
                     }
                     this.i1 = 0;
                     this.i3 = li16(this.i2);
                     this.i3 = this.i3 & -37;
                     si16(this.i3,this.i2);
                     si32(this.i1,this.i0 + 4);
                     this.i1 = li32(this.i0 + 16);
                     si32(this.i1,this.i0);
                     this.i1 = this.i3;
                  }
                  this.i1 = this.i1 | 8;
                  si16(this.i1,this.i2);
               }
               this.i1 = li32(this.i0 + 16);
               if(this.i1 == 0)
               {
                  mstate.esp = mstate.esp - 4;
                  si32(this.i0,mstate.esp);
                  state = 2;
                  mstate.esp = mstate.esp - 4;
                  FSM___swsetup.start();
                  return;
               }
               break;
            case 1:
               this.i1 = mstate.eax;
               mstate.esp = mstate.esp + 8;
               §§goto(addr265);
            case 2:
               mstate.esp = mstate.esp + 4;
         }
         this.i1 = li16(this.i2);
         this.i2 = this.i1 & 1;
         if(this.i2 != 0)
         {
            this.i1 = 0;
            si32(this.i1,this.i0 + 8);
            this.i2 = li32(this.i0 + 20);
            this.i2 = 0 - this.i2;
            si32(this.i2,this.i0 + 24);
            addr457:
            mstate.eax = this.i1;
         }
         else
         {
            this.i2 = this.i0 + 8;
            this.i1 = this.i1 & 2;
            if(this.i1 == 0)
            {
               this.i1 = 0;
               this.i0 = li32(this.i0 + 20);
               si32(this.i0,this.i2);
               §§goto(addr457);
            }
            else
            {
               this.i0 = 0;
               si32(this.i0,this.i2);
               §§goto(addr471);
            }
         }
         §§goto(addr476);
      }
   }
}
