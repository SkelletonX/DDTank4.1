package cmodule.decry
{
   import avm2.intrinsics.memory.li32;
   import avm2.intrinsics.memory.si32;
   
   public final class FSM___Balloc_D2A extends Machine
   {
      
      public static const intRegCount:int = 6;
      
      public static const NumberRegCount:int = 0;
       
      
      public var i0:int;
      
      public var i1:int;
      
      public var i2:int;
      
      public var i3:int;
      
      public var i4:int;
      
      public var i5:int;
      
      public function FSM___Balloc_D2A()
      {
         super();
      }
      
      public static function start() : void
      {
         var _loc1_:FSM___Balloc_D2A = null;
         _loc1_ = new FSM___Balloc_D2A();
         FSM___Balloc_D2A.gworker = _loc1_;
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
               this.i0 = FSM___Balloc_D2A;
               this.i1 = li32(mstate.ebp + 8);
               this.i2 = this.i1 << 2;
               this.i0 = this.i0 + this.i2;
               this.i2 = li32(this.i0);
               if(this.i2 != 0)
               {
                  this.i1 = li32(this.i2);
                  si32(this.i1,this.i0);
                  this.i1 = this.i2;
                  break;
               }
               this.i0 = 1;
               this.i0 = this.i0 << this.i1;
               this.i2 = this.i0 << 2;
               this.i3 = li32(FSM___Balloc_D2A);
               this.i2 = this.i2 + 27;
               this.i4 = FSM___Balloc_D2A;
               this.i4 = this.i3 - this.i4;
               this.i5 = this.i2 >>> 3;
               this.i4 = this.i4 >> 3;
               this.i4 = this.i4 + this.i5;
               if(uint(this.i4) <= uint(288))
               {
                  this.i2 = this.i5 << 3;
                  this.i2 = this.i3 + this.i2;
                  si32(this.i2,FSM___Balloc_D2A);
                  si32(this.i1,this.i3 + 4);
                  si32(this.i0,this.i3 + 8);
                  this.i1 = this.i3;
                  break;
               }
               this.i3 = 0;
               mstate.esp = mstate.esp - 8;
               this.i2 = this.i2 & -8;
               si32(this.i3,mstate.esp);
               si32(this.i2,mstate.esp + 4);
               state = 1;
               mstate.esp = mstate.esp - 4;
               FSM___Balloc_D2A.start();
               return;
            case 1:
               this.i2 = mstate.eax;
               mstate.esp = mstate.esp + 8;
               si32(this.i1,this.i2 + 4);
               si32(this.i0,this.i2 + 8);
               this.i1 = this.i2;
         }
         this.i0 = this.i1;
         this.i1 = 0;
         si32(this.i1,this.i0 + 16);
         si32(this.i1,this.i0 + 12);
         mstate.eax = this.i0;
         mstate.esp = mstate.ebp;
         mstate.ebp = li32(mstate.esp);
         mstate.esp = mstate.esp + 4;
         mstate.esp = mstate.esp + 4;
         mstate.gworker = caller;
      }
   }
}
