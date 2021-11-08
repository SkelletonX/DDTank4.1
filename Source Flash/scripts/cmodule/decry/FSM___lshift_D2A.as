package cmodule.decry
{
   import avm2.intrinsics.memory.li32;
   import avm2.intrinsics.memory.si32;
   
   public final class FSM___lshift_D2A extends Machine
   {
      
      public static const intRegCount:int = 16;
      
      public static const NumberRegCount:int = 0;
       
      
      public var i10:int;
      
      public var i11:int;
      
      public var i12:int;
      
      public var i13:int;
      
      public var i14:int;
      
      public var i15:int;
      
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
      
      public function FSM___lshift_D2A()
      {
         super();
      }
      
      public static function start() : void
      {
         var _loc1_:FSM___lshift_D2A = null;
         _loc1_ = new FSM___lshift_D2A();
         FSM___lshift_D2A.gworker = _loc1_;
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
               this.i1 = li32(mstate.ebp + 12);
               this.i2 = li32(this.i0 + 16);
               this.i3 = this.i1 >> 5;
               this.i2 = this.i2 + this.i3;
               this.i4 = li32(this.i0 + 4);
               this.i5 = li32(this.i0 + 8);
               this.i6 = this.i2 + 1;
               this.i7 = this.i0 + 16;
               this.i8 = this.i0 + 4;
               this.i9 = this.i0;
               if(this.i6 > this.i5)
               {
                  this.i10 = -1;
                  while(true)
                  {
                     this.i10 = this.i10 + 1;
                     this.i5 = this.i5 << 1;
                     if(this.i6 > this.i5)
                     {
                        continue;
                     }
                     break;
                  }
                  this.i4 = this.i10 + this.i4;
                  this.i4 = this.i4 + 1;
               }
               mstate.esp = mstate.esp - 4;
               si32(this.i4,mstate.esp);
               state = 1;
               mstate.esp = mstate.esp - 4;
               FSM___lshift_D2A.start();
               return;
            case 1:
               this.i4 = mstate.eax;
               mstate.esp = mstate.esp + 4;
               this.i5 = this.i4 + 20;
               this.i10 = this.i4;
               if(this.i3 <= 0)
               {
                  this.i3 = this.i5;
               }
               else
               {
                  this.i5 = 0;
                  this.i4 = this.i4 + 20;
                  while(true)
                  {
                     this.i11 = 0;
                     si32(this.i11,this.i4);
                     this.i4 = this.i4 + 4;
                     this.i5 = this.i5 + 1;
                     if(this.i5 != this.i3)
                     {
                        continue;
                     }
                     break;
                  }
                  this.i3 = this.i5 << 2;
                  this.i3 = this.i10 + this.i3;
                  this.i3 = this.i3 + 20;
               }
               this.i4 = li32(this.i7);
               this.i1 = this.i1 & 31;
               this.i5 = this.i3;
               if(this.i1 == 0)
               {
                  this.i1 = 0;
                  this.i3 = this.i1;
                  while(true)
                  {
                     this.i6 = this.i9 + this.i3;
                     this.i6 = li32(this.i6 + 20);
                     this.i7 = this.i5 + this.i3;
                     si32(this.i6,this.i7);
                     this.i3 = this.i3 + 4;
                     this.i1 = this.i1 + 1;
                     if(this.i1 < this.i4)
                     {
                        continue;
                     }
                     break;
                  }
               }
               else
               {
                  this.i7 = 0;
                  this.i11 = 32 - this.i1;
                  this.i12 = this.i7;
                  this.i13 = this.i7;
                  while(true)
                  {
                     this.i14 = this.i9 + this.i12;
                     this.i15 = li32(this.i14 + 20);
                     this.i15 = this.i15 << this.i1;
                     this.i7 = this.i15 | this.i7;
                     this.i15 = this.i5 + this.i12;
                     si32(this.i7,this.i15);
                     this.i7 = li32(this.i14 + 20);
                     this.i12 = this.i12 + 4;
                     this.i13 = this.i13 + 1;
                     this.i7 = this.i7 >>> this.i11;
                     if(this.i13 < this.i4)
                     {
                        continue;
                     }
                     break;
                  }
                  this.i1 = this.i13 << 2;
                  this.i1 = this.i3 + this.i1;
                  si32(this.i7,this.i1);
                  if(this.i7 != 0)
                  {
                     si32(this.i6,this.i10 + 16);
                     if(this.i0 != 0)
                     {
                        addr477:
                        this.i1 = FSM___lshift_D2A;
                        this.i2 = li32(this.i8);
                        this.i2 = this.i2 << 2;
                        this.i1 = this.i1 + this.i2;
                        this.i2 = li32(this.i1);
                        si32(this.i2,this.i0);
                        si32(this.i0,this.i1);
                     }
                  }
                  mstate.eax = this.i10;
                  mstate.esp = mstate.ebp;
                  mstate.ebp = li32(mstate.esp);
                  mstate.esp = mstate.esp + 4;
                  mstate.esp = mstate.esp + 4;
                  mstate.gworker = caller;
                  return;
               }
               si32(this.i2,this.i10 + 16);
               if(this.i0 != 0)
               {
                  §§goto(addr477);
               }
               §§goto(addr517);
         }
      }
   }
}
