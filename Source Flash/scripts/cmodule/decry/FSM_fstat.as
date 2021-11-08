package cmodule.decry
{
   import avm2.intrinsics.memory.li32;
   import avm2.intrinsics.memory.si32;
   
   public final class FSM_fstat extends Machine
   {
      
      public static const intRegCount:int = 5;
      
      public static const NumberRegCount:int = 0;
       
      
      public var i0:int;
      
      public var i1:int;
      
      public var i2:int;
      
      public var i3:int;
      
      public var i4:int;
      
      public function FSM_fstat()
      {
         super();
      }
      
      public static function start() : void
      {
         var _loc1_:FSM_fstat = null;
         _loc1_ = new FSM_fstat();
         FSM_fstat.gworker = _loc1_;
      }
      
      override public final function work() : void
      {
         switch(state)
         {
            case 0:
               mstate.esp = mstate.esp - 4;
               si32(mstate.ebp,mstate.esp);
               mstate.ebp = mstate.esp;
               mstate.esp = mstate.esp - 4096;
               this.i0 = li32(mstate.ebp + 8);
               this.i1 = li32(mstate.ebp + 12);
               if(this.i0 >= 2)
               {
                  state = 1;
               }
               else
               {
                  this.i0 = 0;
                  addr238:
                  mstate.eax = this.i0;
                  break;
               }
            case 1:
               this.i0 = mstate.system.fsize(this.i0);
               if(this.i0 <= -1)
               {
                  this.i0 = FSM_fstat;
                  mstate.esp = mstate.esp - 20;
                  this.i1 = FSM_fstat;
                  this.i2 = 59;
                  this.i3 = 2;
                  this.i4 = mstate.ebp + -4096;
                  si32(this.i4,mstate.esp);
                  si32(this.i0,mstate.esp + 4);
                  si32(this.i3,mstate.esp + 8);
                  si32(this.i1,mstate.esp + 12);
                  si32(this.i2,mstate.esp + 16);
                  state = 2;
                  mstate.esp = mstate.esp - 4;
                  FSM_fstat.start();
                  return;
               }
               this.i2 = 0;
               this.i3 = this.i1;
               this.i4 = 96;
               memset(this.i3,this.i2,this.i4);
               this.i3 = this.i0 >> 31;
               si32(this.i0,this.i1 + 48);
               si32(this.i3,this.i1 + 52);
               mstate.eax = this.i2;
               break;
            case 2:
               mstate.esp = mstate.esp + 20;
               this.i1 = 3;
               this.i0 = this.i4;
               log(this.i1,mstate.gworker.stringFromPtr(this.i0));
               si32(this.i3,FSM_fstat);
               this.i0 = -1;
               §§goto(addr238);
         }
         mstate.esp = mstate.ebp;
         mstate.ebp = li32(mstate.esp);
         mstate.esp = mstate.esp + 4;
         mstate.esp = mstate.esp + 4;
         mstate.gworker = caller;
      }
   }
}
