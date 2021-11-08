package cmodule.decry
{
   import avm2.intrinsics.memory.li32;
   import avm2.intrinsics.memory.si32;
   
   public final class FSM___multadd_D2A extends Machine
   {
      
      public static const intRegCount:int = 12;
      
      public static const NumberRegCount:int = 0;
       
      
      public var i10:int;
      
      public var i11:int;
      
      public var i0:int;
      
      public var i1:int;
      
      public var i2:int;
      
      public var i3:int;
      
      public var i4:int;
      
      public var i5:int;
      
      public var i6:int;
      
      public var i7:int;
      
      public var i8:int;
      
      public var i9:int;
      
      public function FSM___multadd_D2A()
      {
         super();
      }
      
      public static function start() : void
      {
         var _loc1_:FSM___multadd_D2A = null;
         _loc1_ = new FSM___multadd_D2A();
         FSM___multadd_D2A.gworker = _loc1_;
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
               this.i0 = 0;
               this.i1 = li32(mstate.ebp + 8);
               this.i2 = li32(mstate.ebp + 12);
               this.i3 = li32(this.i1 + 16);
               this.i4 = this.i2 >> 31;
               this.i5 = this.i1 + 20;
               this.i6 = this.i1 + 16;
               this.i7 = this.i0;
               this.i8 = this.i0;
               addr89:
               while(true)
               {
                  this.i9 = 0;
                  mstate.esp = mstate.esp - 16;
                  this.i10 = li32(this.i5);
                  si32(this.i10,mstate.esp);
                  si32(this.i9,mstate.esp + 4);
                  si32(this.i2,mstate.esp + 8);
                  si32(this.i4,mstate.esp + 12);
                  mstate.esp = mstate.esp - 4;
                  mstate.funcs[FSM___multadd_D2A]();
               }
            case 1:
               while(true)
               {
                  this.i10 = mstate.eax;
                  this.i11 = mstate.edx;
                  this.i7 = __addc(this.i10,this.i7);
                  this.i0 = __adde(this.i11,this.i0);
                  si32(this.i7,this.i5);
                  this.i5 = this.i5 + 4;
                  this.i7 = this.i8 + 1;
                  mstate.esp = mstate.esp + 16;
                  this.i8 = this.i0;
                  if(this.i7 < this.i3)
                  {
                     this.i8 = this.i7;
                     this.i7 = this.i0;
                     this.i0 = this.i9;
                     §§goto(addr89);
                  }
                  else
                  {
                     break;
                  }
               }
               this.i2 = this.i8 == 0?int(1):int(0);
               if(this.i2 == 0)
               {
                  this.i2 = li32(this.i1 + 8);
                  if(this.i2 > this.i3)
                  {
                     break;
                  }
                  this.i2 = li32(this.i1 + 4);
                  mstate.esp = mstate.esp - 4;
                  this.i2 = this.i2 + 1;
                  si32(this.i2,mstate.esp);
                  state = 2;
                  mstate.esp = mstate.esp - 4;
                  FSM___multadd_D2A.start();
                  return;
               }
               mstate.eax = this.i1;
               mstate.esp = mstate.ebp;
               mstate.ebp = li32(mstate.esp);
               mstate.esp = mstate.esp + 4;
               mstate.esp = mstate.esp + 4;
               mstate.gworker = caller;
               return;
            case 2:
               this.i2 = mstate.eax;
               mstate.esp = mstate.esp + 4;
               this.i4 = li32(this.i6);
               this.i5 = this.i2 + 12;
               this.i4 = this.i4 << 2;
               this.i6 = this.i1 + 12;
               this.i4 = this.i4 + 8;
               memcpy(this.i5,this.i6,this.i4);
               this.i4 = this.i1 + 4;
               if(this.i1 == 0)
               {
                  this.i1 = this.i2;
                  break;
               }
               this.i5 = FSM___multadd_D2A;
               this.i4 = li32(this.i4);
               this.i4 = this.i4 << 2;
               this.i4 = this.i5 + this.i4;
               this.i5 = li32(this.i4);
               si32(this.i5,this.i1);
               si32(this.i1,this.i4);
               this.i1 = this.i2;
               break;
         }
         this.i2 = this.i3 << 2;
         this.i2 = this.i1 + this.i2;
         si32(this.i0,this.i2 + 20);
         this.i0 = this.i3 + 1;
         si32(this.i0,this.i1 + 16);
         §§goto(addr442);
      }
   }
}
