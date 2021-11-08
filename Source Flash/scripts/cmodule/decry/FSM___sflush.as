package cmodule.decry
{
   import avm2.intrinsics.memory.li16;
   import avm2.intrinsics.memory.li32;
   import avm2.intrinsics.memory.si16;
   import avm2.intrinsics.memory.si32;
   import avm2.intrinsics.memory.sxi16;
   
   public final class FSM___sflush extends Machine
   {
      
      public static const intRegCount:int = 7;
      
      public static const NumberRegCount:int = 0;
       
      
      public var i0:int;
      
      public var i1:int;
      
      public var i2:int;
      
      public var i3:int;
      
      public var i4:int;
      
      public var i5:int;
      
      public var i6:int;
      
      public function FSM___sflush()
      {
         super();
      }
      
      public static function start() : void
      {
         var _loc1_:FSM___sflush = null;
         _loc1_ = new FSM___sflush();
         FSM___sflush.gworker = _loc1_;
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
               this.i1 = si16(li16(this.i0 + 12));
               this.i2 = this.i0 + 12;
               this.i3 = this.i1 & 8;
               if(this.i3 != 0)
               {
                  this.i3 = li32(this.i0 + 16);
                  if(this.i3 != 0)
                  {
                     this.i4 = li32(this.i0);
                     si32(this.i3,this.i0);
                     this.i5 = this.i0 + 8;
                     this.i6 = this.i3;
                     this.i1 = this.i1 & 3;
                     if(this.i1 == 0)
                     {
                        this.i1 = li32(this.i0 + 20);
                        si32(this.i1,this.i5);
                        this.i4 = this.i4 - this.i6;
                        if(this.i4 >= 1)
                        {
                           this.i5 = 0;
                        }
                        else
                        {
                           break;
                        }
                     }
                     else
                     {
                        this.i1 = 0;
                        si32(this.i1,this.i5);
                        this.i4 = this.i4 - this.i6;
                        if(this.i4 >= 1)
                        {
                           this.i5 = 0;
                        }
                        else
                        {
                           break;
                        }
                     }
                     addr165:
                     mstate.esp = mstate.esp - 12;
                     this.i1 = this.i3 + this.i5;
                     si32(this.i0,mstate.esp);
                     si32(this.i1,mstate.esp + 4);
                     si32(this.i4,mstate.esp + 8);
                     state = 1;
                     mstate.esp = mstate.esp - 4;
                     FSM___sflush.start();
                     return;
                  }
                  break;
               }
               break;
            case 1:
               this.i1 = mstate.eax;
               mstate.esp = mstate.esp + 12;
               if(this.i1 <= 0)
               {
                  this.i4 = -1;
                  this.i5 = li16(this.i2);
                  this.i5 = this.i5 | 64;
                  si16(this.i5,this.i2);
                  mstate.eax = this.i4;
                  addr289:
                  mstate.esp = mstate.ebp;
                  mstate.ebp = li32(mstate.esp);
                  mstate.esp = mstate.esp + 4;
                  mstate.esp = mstate.esp + 4;
                  mstate.gworker = caller;
                  return;
               }
               this.i4 = this.i4 - this.i1;
               this.i5 = this.i5 + this.i1;
               if(this.i4 >= 1)
               {
                  §§goto(addr165);
               }
               else
               {
                  break;
               }
         }
         this.i0 = 0;
         mstate.eax = this.i0;
         §§goto(addr289);
      }
   }
}
