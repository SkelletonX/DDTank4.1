package cmodule.decry
{
   import avm2.intrinsics.memory.li32;
   import avm2.intrinsics.memory.si16;
   import avm2.intrinsics.memory.si32;
   import avm2.intrinsics.memory.si8;
   
   public final class FSM_sprintf extends Machine
   {
      
      public static const intRegCount:int = 4;
      
      public static const NumberRegCount:int = 0;
       
      
      public var i0:int;
      
      public var i1:int;
      
      public var i2:int;
      
      public var i3:int;
      
      public function FSM_sprintf()
      {
         super();
      }
      
      public static function start() : void
      {
         var _loc1_:FSM_sprintf = null;
         _loc1_ = new FSM_sprintf();
         FSM_sprintf.gworker = _loc1_;
      }
      
      override public final function work() : void
      {
         switch(state)
         {
            case 0:
               mstate.esp = mstate.esp - 4;
               si32(mstate.ebp,mstate.esp);
               mstate.ebp = mstate.esp;
               mstate.esp = mstate.esp - 256;
               this.i0 = -1;
               si16(this.i0,mstate.ebp + -82);
               this.i0 = 520;
               si16(this.i0,mstate.ebp + -84);
               this.i0 = li32(mstate.ebp + 8);
               si32(this.i0,mstate.ebp + -96);
               si32(this.i0,mstate.ebp + -80);
               this.i0 = 2147483647;
               si32(this.i0,mstate.ebp + -88);
               this.i1 = mstate.ebp + -256;
               si32(this.i0,mstate.ebp + -76);
               si32(this.i1,mstate.ebp + -40);
               this.i0 = 0;
               si32(this.i0,mstate.ebp + -256);
               si32(this.i0,mstate.ebp + -252);
               si32(this.i0,mstate.ebp + -248);
               si32(this.i0,mstate.ebp + -244);
               si32(this.i0,mstate.ebp + -240);
               this.i1 = this.i1 + 24;
               this.i2 = 128;
               memset(this.i1,this.i0,this.i2);
               this.i1 = mstate.ebp + 16;
               si32(this.i1,mstate.ebp + -4);
               mstate.esp = mstate.esp - 12;
               this.i2 = li32(mstate.ebp + 12);
               this.i3 = mstate.ebp + -96;
               si32(this.i3,mstate.esp);
               si32(this.i2,mstate.esp + 4);
               si32(this.i1,mstate.esp + 8);
               state = 1;
               mstate.esp = mstate.esp - 4;
               FSM_sprintf.start();
               return;
            case 1:
               this.i1 = mstate.eax;
               mstate.esp = mstate.esp + 12;
               this.i1 = li32(mstate.ebp + -96);
               si8(this.i0,this.i1);
               mstate.esp = mstate.ebp;
               mstate.ebp = li32(mstate.esp);
               mstate.esp = mstate.esp + 4;
               mstate.esp = mstate.esp + 4;
               mstate.gworker = caller;
               return;
         }
      }
   }
}
