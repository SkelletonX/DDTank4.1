package cmodule.decry
{
   import avm2.intrinsics.memory.li32;
   import avm2.intrinsics.memory.si32;
   
   public final class FSM___pow5mult_D2A extends Machine
   {
      
      public static const intRegCount:int = 6;
      
      public static const NumberRegCount:int = 0;
       
      
      public var i0:int;
      
      public var i1:int;
      
      public var i2:int;
      
      public var i3:int;
      
      public var i4:int;
      
      public var i5:int;
      
      public function FSM___pow5mult_D2A()
      {
         super();
      }
      
      public static function start() : void
      {
         var _loc1_:FSM___pow5mult_D2A = null;
         _loc1_ = new FSM___pow5mult_D2A();
         FSM___pow5mult_D2A.gworker = _loc1_;
      }
      
      override public final function work() : void
      {
         loop3:
         switch(state)
         {
            case 0:
               mstate.esp = mstate.esp - 4;
               si32(mstate.ebp,mstate.esp);
               mstate.ebp = mstate.esp;
               mstate.esp = mstate.esp - 0;
               this.i0 = li32(mstate.ebp + 12);
               this.i1 = li32(mstate.ebp + 8);
               this.i2 = this.i0 & 3;
               if(this.i2 == 0)
               {
                  addr139:
                  this.i2 = this.i0 >> 2;
                  if(uint(this.i0) <= uint(3))
                  {
                     addr157:
                     this.i0 = this.i1;
                     mstate.eax = this.i0;
                     mstate.esp = mstate.ebp;
                     mstate.ebp = li32(mstate.esp);
                     mstate.esp = mstate.esp + 4;
                     mstate.esp = mstate.esp + 4;
                     mstate.gworker = caller;
                     return;
                  }
                  this.i0 = li32(FSM___pow5mult_D2A);
                  if(this.i0 == 0)
                  {
                     this.i0 = li32(FSM___pow5mult_D2A + 4);
                     if(this.i0 != 0)
                     {
                        this.i3 = li32(this.i0);
                        si32(this.i3,FSM___pow5mult_D2A + 4);
                     }
                     else
                     {
                        this.i0 = FSM___pow5mult_D2A;
                        this.i3 = li32(FSM___pow5mult_D2A);
                        this.i0 = this.i3 - this.i0;
                        this.i0 = this.i0 >> 3;
                        this.i0 = this.i0 + 4;
                        if(uint(this.i0) <= uint(288))
                        {
                           this.i0 = 1;
                           this.i4 = this.i3 + 32;
                           si32(this.i4,FSM___pow5mult_D2A);
                           si32(this.i0,this.i3 + 4);
                           this.i0 = 2;
                           si32(this.i0,this.i3 + 8);
                           this.i0 = this.i3;
                        }
                        else
                        {
                           this.i0 = 32;
                           mstate.esp = mstate.esp - 4;
                           si32(this.i0,mstate.esp);
                           state = 2;
                           mstate.esp = mstate.esp - 4;
                           FSM___pow5mult_D2A.start();
                           return;
                        }
                     }
                     addr376:
                     this.i3 = 0;
                     si32(this.i3,this.i0 + 12);
                     this.i4 = 625;
                     si32(this.i4,this.i0 + 20);
                     this.i4 = 1;
                     si32(this.i4,this.i0 + 16);
                     si32(this.i0,FSM___pow5mult_D2A);
                     si32(this.i3,this.i0);
                  }
                  addr418:
                  while(true)
                  {
                     this.i3 = this.i2 & 1;
                     if(this.i3 == 0)
                     {
                        break loop3;
                     }
                     break;
                  }
                  mstate.esp = mstate.esp - 8;
                  si32(this.i1,mstate.esp);
                  si32(this.i0,mstate.esp + 4);
                  state = 3;
                  mstate.esp = mstate.esp - 4;
                  FSM___pow5mult_D2A.start();
                  return;
               }
               this.i3 = FSM___pow5mult_D2A;
               this.i2 = this.i2 << 2;
               this.i2 = this.i2 + this.i3;
               this.i2 = li32(this.i2 + -4);
               mstate.esp = mstate.esp - 8;
               si32(this.i1,mstate.esp);
               si32(this.i2,mstate.esp + 4);
               state = 1;
               mstate.esp = mstate.esp - 4;
               FSM___pow5mult_D2A.start();
               return;
            case 1:
               this.i1 = mstate.eax;
               mstate.esp = mstate.esp + 8;
               §§goto(addr139);
            case 2:
               this.i0 = mstate.eax;
               mstate.esp = mstate.esp + 4;
               this.i3 = 1;
               si32(this.i3,this.i0 + 4);
               this.i3 = 2;
               si32(this.i3,this.i0 + 8);
               §§goto(addr376);
            case 3:
               this.i3 = mstate.eax;
               mstate.esp = mstate.esp + 8;
               if(this.i1 == 0)
               {
                  this.i1 = this.i3;
                  break;
               }
               this.i4 = FSM___pow5mult_D2A;
               this.i5 = li32(this.i1 + 4);
               this.i5 = this.i5 << 2;
               this.i4 = this.i4 + this.i5;
               this.i5 = li32(this.i4);
               si32(this.i5,this.i1);
               si32(this.i1,this.i4);
               this.i1 = this.i3;
               break;
            case 4:
               this.i0 = mstate.eax;
               mstate.esp = mstate.esp + 8;
               si32(this.i0,this.i4);
               si32(this.i2,this.i0);
               this.i2 = this.i3;
               addr417:
               while(true)
               {
                  §§goto(addr418);
               }
         }
         while(true)
         {
            this.i3 = this.i2 >> 1;
            if(uint(this.i2) <= uint(1))
            {
               §§goto(addr157);
            }
            else
            {
               this.i2 = li32(this.i0);
               this.i4 = this.i0;
               if(this.i2 != 0)
               {
                  this.i0 = this.i2;
                  this.i2 = this.i3;
                  §§goto(addr417);
               }
               else
               {
                  this.i2 = 0;
                  mstate.esp = mstate.esp - 8;
                  si32(this.i0,mstate.esp);
                  si32(this.i0,mstate.esp + 4);
                  state = 4;
                  mstate.esp = mstate.esp - 4;
                  FSM___pow5mult_D2A.start();
                  return;
               }
            }
         }
      }
   }
}
