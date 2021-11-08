package cmodule.decry
{
   import avm2.intrinsics.memory.li16;
   import avm2.intrinsics.memory.li32;
   import avm2.intrinsics.memory.si16;
   import avm2.intrinsics.memory.si32;
   
   public final class FSM__swrite extends Machine
   {
      
      public static const intRegCount:int = 11;
      
      public static const NumberRegCount:int = 0;
       
      
      public var i10:int;
      
      public var i0:int;
      
      public var i1:int;
      
      public var i2:int;
      
      public var i3:int;
      
      public var i4:int;
      
      public var i5:int;
      
      public var i6:int;
      
      public var i8:int;
      
      public var i7:int;
      
      public var i9:int;
      
      public function FSM__swrite()
      {
         super();
      }
      
      public static function start() : void
      {
         var _loc1_:FSM__swrite = null;
         _loc1_ = new FSM__swrite();
         FSM__swrite.gworker = _loc1_;
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
               this.i1 = li16(this.i0 + 12);
               this.i2 = this.i0 + 12;
               this.i3 = li32(mstate.ebp + 12);
               this.i4 = li32(mstate.ebp + 16);
               this.i1 = this.i1 & 256;
               if(this.i1 != 0)
               {
                  this.i1 = 0;
                  this.i5 = li32(FSM__swrite);
                  mstate.esp = mstate.esp - 16;
                  this.i6 = 2;
                  si32(this.i0,mstate.esp);
                  si32(this.i1,mstate.esp + 4);
                  si32(this.i1,mstate.esp + 8);
                  si32(this.i6,mstate.esp + 12);
                  state = 1;
                  mstate.esp = mstate.esp - 4;
                  FSM__swrite.start();
                  return;
               }
               break;
            case 1:
               this.i1 = mstate.eax;
               this.i6 = mstate.edx;
               mstate.esp = mstate.esp + 16;
               this.i1 = this.i1 & this.i6;
               if(this.i1 == -1)
               {
                  this.i1 = li16(this.i2);
                  this.i1 = this.i1 & 1024;
                  if(this.i1 != 0)
                  {
                     this.i0 = -1;
                     addr473:
                     addr478:
                     mstate.eax = this.i0;
                     addr478:
                     mstate.esp = mstate.ebp;
                     mstate.ebp = li32(mstate.esp);
                     mstate.esp = mstate.esp + 4;
                     mstate.esp = mstate.esp + 4;
                     mstate.gworker = caller;
                     return;
                  }
               }
               si32(this.i5,FSM__swrite);
               break;
            case 2:
               this.i1 = mstate.eax;
               mstate.esp = mstate.esp + 12;
               if(this.i1 >= 0)
               {
                  this.i3 = li16(this.i2);
                  this.i4 = this.i3 & 4352;
                  if(this.i4 == 4352)
                  {
                     this.i4 = 2147483647;
                     this.i5 = li32(this.i0 + 80);
                     this.i6 = li32(this.i0 + 84);
                     this.i7 = this.i1 >> 31;
                     this.i8 = -1;
                     this.i8 = __subc(this.i8,this.i1);
                     this.i4 = __sube(this.i4,this.i7);
                     this.i0 = this.i0 + 80;
                     this.i9 = this.i6 > this.i4?int(1):int(0);
                     this.i8 = uint(this.i5) > uint(this.i8)?int(1):int(0);
                     this.i4 = this.i6 == this.i4?int(1):int(0);
                     this.i10 = this.i1;
                     this.i4 = this.i4 != 0?int(this.i8):int(this.i9);
                     if(this.i4 == 0)
                     {
                        this.i2 = __addc(this.i5,this.i10);
                        this.i3 = __adde(this.i6,this.i7);
                        si32(this.i2,this.i0);
                        si32(this.i3,this.i0 + 4);
                     }
                     addr457:
                     mstate.eax = this.i1;
                     §§goto(addr478);
                  }
                  this.i0 = this.i3 & -4097;
               }
               else
               {
                  if(this.i1 >= 0)
                  {
                     this.i0 = this.i1;
                     §§goto(addr473);
                  }
                  else
                  {
                     this.i0 = li16(this.i2);
                     this.i0 = this.i0 & -4097;
                  }
                  §§goto(addr478);
               }
               si16(this.i0,this.i2);
               §§goto(addr457);
         }
         this.i1 = li32(this.i0 + 44);
         this.i5 = li32(this.i0 + 28);
         mstate.esp = mstate.esp - 12;
         si32(this.i5,mstate.esp);
         si32(this.i3,mstate.esp + 4);
         si32(this.i4,mstate.esp + 8);
         state = 2;
         mstate.esp = mstate.esp - 4;
         mstate.funcs[this.i1]();
      }
   }
}
