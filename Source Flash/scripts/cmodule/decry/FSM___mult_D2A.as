package cmodule.decry
{
   import avm2.intrinsics.memory.li32;
   import avm2.intrinsics.memory.si32;
   
   public final class FSM___mult_D2A extends Machine
   {
      
      public static const intRegCount:int = 21;
      
      public static const NumberRegCount:int = 0;
       
      
      public var i10:int;
      
      public var i11:int;
      
      public var i12:int;
      
      public var i13:int;
      
      public var i14:int;
      
      public var i15:int;
      
      public var i17:int;
      
      public var i19:int;
      
      public var i16:int;
      
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
      
      public var i18:int;
      
      public var i20:int;
      
      public function FSM___mult_D2A()
      {
         super();
      }
      
      public static function start() : void
      {
         var _loc1_:FSM___mult_D2A = null;
         _loc1_ = new FSM___mult_D2A();
         FSM___mult_D2A.gworker = _loc1_;
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
               this.i3 = li32(this.i1 + 16);
               this.i4 = this.i2 < this.i3?int(this.i0):int(this.i1);
               this.i0 = this.i2 < this.i3?int(this.i1):int(this.i0);
               this.i1 = li32(this.i0 + 16);
               this.i2 = li32(this.i4 + 16);
               this.i3 = li32(this.i0 + 8);
               this.i5 = this.i2 + this.i1;
               this.i6 = li32(this.i0 + 4);
               this.i3 = this.i3 < this.i5?int(1):int(0);
               this.i3 = this.i3 & 1;
               mstate.esp = mstate.esp - 4;
               this.i3 = this.i3 + this.i6;
               si32(this.i3,mstate.esp);
               state = 1;
               mstate.esp = mstate.esp - 4;
               FSM___mult_D2A.start();
               return;
            case 1:
               this.i3 = mstate.eax;
               mstate.esp = mstate.esp + 4;
               this.i6 = this.i3;
               if(this.i5 >= 1)
               {
                  this.i7 = 0;
                  this.i8 = this.i6 + 20;
                  while(true)
                  {
                     this.i9 = 0;
                     si32(this.i9,this.i8);
                     this.i8 = this.i8 + 4;
                     this.i7 = this.i7 + 1;
                     if(this.i7 < this.i5)
                     {
                        continue;
                     }
                     break;
                  }
               }
               if(this.i2 >= 1)
               {
                  this.i7 = 0;
                  this.i8 = this.i7;
                  loop1:
                  while(true)
                  {
                     this.i9 = this.i4 + this.i8;
                     this.i9 = li32(this.i9 + 20);
                     if(this.i9 != 0)
                     {
                        this.i10 = 20;
                        this.i11 = 0;
                        this.i12 = this.i6 + this.i8;
                        this.i13 = this.i11;
                        this.i14 = this.i11;
                        this.i15 = this.i11;
                        addr303:
                        while(true)
                        {
                           this.i16 = 0;
                           this.i17 = this.i0 + this.i10;
                           mstate.esp = mstate.esp - 16;
                           this.i17 = li32(this.i17);
                           si32(this.i17,mstate.esp);
                           si32(this.i16,mstate.esp + 4);
                           si32(this.i9,mstate.esp + 8);
                           si32(this.i11,mstate.esp + 12);
                           this.i17 = this.i12 + this.i10;
                           this.i18 = li32(this.i17);
                           mstate.esp = mstate.esp - 4;
                           mstate.funcs[FSM___mult_D2A]();
                        }
                     }
                     addr498:
                     while(true)
                     {
                        this.i8 = this.i8 + 4;
                        this.i7 = this.i7 + 1;
                        if(this.i7 < this.i2)
                        {
                           continue loop1;
                        }
                        break loop1;
                     }
                     this.i0 = 0;
                     this.i1 = this.i2 + this.i1;
                     while(true)
                     {
                        this.i2 = this.i1;
                        this.i1 = this.i0;
                        this.i0 = this.i1 ^ -1;
                        this.i0 = this.i5 + this.i0;
                        this.i0 = this.i0 << 2;
                        this.i0 = this.i3 + this.i0;
                        this.i0 = li32(this.i0 + 20);
                        if(this.i0 != 0)
                        {
                           this.i1 = this.i2;
                           break;
                        }
                        this.i0 = this.i2 + -1;
                        this.i2 = this.i1 + 1;
                        if(this.i0 >= 1)
                        {
                           this.i1 = this.i0;
                           this.i0 = this.i2;
                           continue;
                        }
                        this.i1 = this.i0;
                        break;
                     }
                     this.i0 = this.i1;
                     si32(this.i0,this.i3 + 16);
                     mstate.eax = this.i3;
                     mstate.esp = mstate.ebp;
                     mstate.ebp = li32(mstate.esp);
                     mstate.esp = mstate.esp + 4;
                     mstate.esp = mstate.esp + 4;
                     mstate.gworker = caller;
                     return;
                  }
               }
               if(this.i5 <= 0)
               {
                  this.i1 = this.i5;
               }
               else
               {
                  §§goto(addr559);
               }
               §§goto(addr620);
            case 2:
               while(true)
               {
                  this.i19 = mstate.eax;
                  this.i20 = mstate.edx;
                  this.i14 = __addc(this.i18,this.i14);
                  this.i13 = __adde(this.i13,this.i16);
                  this.i14 = __addc(this.i14,this.i19);
                  this.i13 = __adde(this.i13,this.i20);
                  si32(this.i14,this.i17);
                  this.i10 = this.i10 + 4;
                  this.i14 = this.i15 + 1;
                  mstate.esp = mstate.esp + 16;
                  if(this.i14 < this.i1)
                  {
                     this.i15 = this.i14;
                     this.i14 = this.i13;
                     this.i13 = this.i16;
                  }
                  else
                  {
                     this.i9 = this.i7 + this.i14;
                     this.i9 = this.i9 << 2;
                     this.i9 = this.i3 + this.i9;
                     si32(this.i13,this.i9 + 20);
                     §§goto(addr498);
                  }
                  §§goto(addr303);
               }
         }
      }
   }
}
