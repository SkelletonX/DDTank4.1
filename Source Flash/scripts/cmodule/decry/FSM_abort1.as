package cmodule.decry
{
   import avm2.intrinsics.memory.li8;
   import avm2.intrinsics.memory.si32;
   
   public final class FSM_abort1 extends Machine
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
      
      public function FSM_abort1()
      {
         super();
      }
      
      public static function start() : void
      {
         var _loc1_:FSM_abort1 = null;
         _loc1_ = new FSM_abort1();
         FSM_abort1.gworker = _loc1_;
      }
      
      override public final function work() : void
      {
         switch(state)
         {
            case 0:
               mstate.esp = mstate.esp - 4;
               si32(mstate.ebp,mstate.esp);
               mstate.ebp = mstate.esp;
               mstate.esp = mstate.esp - 20480;
               this.i0 = li8(FSM_abort1);
               this.i0 = this.i0 ^ 1;
               this.i0 = this.i0 & 1;
               if(this.i0 == 0)
               {
                  state = 1;
                  mstate.esp = mstate.esp - 4;
                  FSM_abort1.start();
                  return;
               }
            case 1:
               this.i2 = FSM_abort1;
               this.i3 = 4;
               this.i0 = this.i2;
               this.i1 = this.i3;
               log(this.i1,mstate.gworker.stringFromPtr(this.i0));
               mstate.esp = mstate.esp - 20;
               this.i4 = FSM_abort1;
               this.i5 = FSM_abort1;
               this.i6 = 34;
               this.i7 = 78;
               this.i0 = mstate.ebp + -20480;
               si32(this.i0,mstate.esp);
               si32(this.i4,mstate.esp + 4);
               si32(this.i7,mstate.esp + 8);
               si32(this.i5,mstate.esp + 12);
               si32(this.i6,mstate.esp + 16);
               state = 2;
               mstate.esp = mstate.esp - 4;
               FSM_abort1.start();
               return;
            case 2:
               mstate.esp = mstate.esp + 20;
               this.i8 = 3;
               this.i1 = this.i8;
               log(this.i1,mstate.gworker.stringFromPtr(this.i0));
               si32(this.i7,FSM_abort1);
               this.i9 = FSM_abort1;
               this.i10 = FSM_abort1;
               this.i0 = this.i9;
               this.i1 = this.i3;
               log(this.i1,mstate.gworker.stringFromPtr(this.i0));
               this.i0 = this.i10;
               this.i1 = this.i3;
               log(this.i1,mstate.gworker.stringFromPtr(this.i0));
               mstate.esp = mstate.esp - 20;
               this.i11 = 50;
               this.i0 = mstate.ebp + -16384;
               si32(this.i0,mstate.esp);
               si32(this.i4,mstate.esp + 4);
               si32(this.i7,mstate.esp + 8);
               si32(this.i5,mstate.esp + 12);
               si32(this.i11,mstate.esp + 16);
               state = 3;
               mstate.esp = mstate.esp - 4;
               FSM_abort1.start();
               return;
            case 3:
               mstate.esp = mstate.esp + 20;
               this.i1 = this.i8;
               log(this.i1,mstate.gworker.stringFromPtr(this.i0));
               si32(this.i7,FSM_abort1);
               this.i0 = FSM_abort1;
               this.i1 = this.i3;
               log(this.i1,mstate.gworker.stringFromPtr(this.i0));
               mstate.esp = mstate.esp - 20;
               this.i0 = 10;
               this.i1 = mstate.ebp + -12288;
               si32(this.i1,mstate.esp);
               si32(this.i4,mstate.esp + 4);
               si32(this.i7,mstate.esp + 8);
               si32(this.i5,mstate.esp + 12);
               si32(this.i0,mstate.esp + 16);
               state = 4;
               mstate.esp = mstate.esp - 4;
               FSM_abort1.start();
               return;
            case 4:
               mstate.esp = mstate.esp + 20;
               this.i0 = this.i1;
               this.i1 = this.i8;
               log(this.i1,mstate.gworker.stringFromPtr(this.i0));
               si32(this.i7,FSM_abort1);
               this.i0 = this.i2;
               this.i1 = this.i3;
               log(this.i1,mstate.gworker.stringFromPtr(this.i0));
               mstate.esp = mstate.esp - 20;
               this.i0 = mstate.ebp + -8192;
               si32(this.i0,mstate.esp);
               si32(this.i4,mstate.esp + 4);
               si32(this.i7,mstate.esp + 8);
               si32(this.i5,mstate.esp + 12);
               si32(this.i6,mstate.esp + 16);
               state = 5;
               mstate.esp = mstate.esp - 4;
               FSM_abort1.start();
               return;
            case 5:
               mstate.esp = mstate.esp + 20;
               this.i1 = this.i8;
               log(this.i1,mstate.gworker.stringFromPtr(this.i0));
               si32(this.i7,FSM_abort1);
               this.i0 = this.i9;
               this.i1 = this.i3;
               log(this.i1,mstate.gworker.stringFromPtr(this.i0));
               this.i0 = this.i10;
               this.i1 = this.i3;
               log(this.i1,mstate.gworker.stringFromPtr(this.i0));
               mstate.esp = mstate.esp - 20;
               this.i0 = mstate.ebp + -4096;
               si32(this.i0,mstate.esp);
               si32(this.i4,mstate.esp + 4);
               si32(this.i7,mstate.esp + 8);
               si32(this.i5,mstate.esp + 12);
               si32(this.i11,mstate.esp + 16);
               state = 6;
               mstate.esp = mstate.esp - 4;
               FSM_abort1.start();
               return;
            case 6:
               mstate.esp = mstate.esp + 20;
               this.i1 = this.i8;
               log(this.i1,mstate.gworker.stringFromPtr(this.i0));
               si32(this.i7,FSM_abort1);
               mstate.esp = mstate.esp - 4;
               this.i0 = 1;
               si32(this.i0,mstate.esp);
               state = 7;
               mstate.esp = mstate.esp - 4;
               FSM_abort1.start();
               return;
            case 7:
               mstate.esp = mstate.esp + 4;
         }
      }
   }
}
