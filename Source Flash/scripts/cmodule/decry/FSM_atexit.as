package cmodule.decry
{
   import avm2.intrinsics.memory.li32;
   import avm2.intrinsics.memory.si32;
   
   public final class FSM_atexit extends Machine
   {
      
      public static const intRegCount:int = 5;
      
      public static const NumberRegCount:int = 0;
       
      
      public var i0:int;
      
      public var i1:int;
      
      public var i2:int;
      
      public var i3:int;
      
      public var i4:int;
      
      public function FSM_atexit()
      {
         super();
      }
      
      public static function start() : void
      {
         var _loc1_:FSM_atexit = null;
         _loc1_ = new FSM_atexit();
         FSM_atexit.gworker = _loc1_;
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
               this.i0 = li32(FSM_atexit);
               this.i1 = li32(mstate.ebp + 8);
               if(this.i0 != 0)
               {
                  this.i2 = this.i0;
                  addr235:
                  this.i3 = li32(this.i0 + 4);
                  if(this.i3 <= 31)
                  {
                     break;
                  }
                  this.i0 = this.i2;
                  addr62:
                  this.i2 = 520;
                  mstate.esp = mstate.esp - 8;
                  this.i3 = 0;
                  si32(this.i3,mstate.esp);
                  si32(this.i2,mstate.esp + 4);
                  state = 1;
                  mstate.esp = mstate.esp - 4;
                  FSM_atexit.start();
                  return;
               }
               this.i0 = FSM_atexit;
               si32(this.i0,FSM_atexit);
               break;
            case 1:
               this.i2 = mstate.eax;
               mstate.esp = mstate.esp + 8;
               if(this.i2 != 0)
               {
                  this.i3 = li32(FSM_atexit);
                  if(this.i0 != this.i3)
                  {
                     this.i0 = 0;
                     mstate.esp = mstate.esp - 8;
                     si32(this.i2,mstate.esp);
                     si32(this.i0,mstate.esp + 4);
                     state = 2;
                     mstate.esp = mstate.esp - 4;
                     FSM_atexit.start();
                     return;
                  }
                  this.i0 = 0;
                  si32(this.i0,this.i2 + 4);
                  si32(this.i3,this.i2);
                  si32(this.i2,FSM_atexit);
                  this.i0 = this.i2;
                  this.i2 = this.i0;
                  §§goto(addr235);
               }
               addr330:
               mstate.esp = mstate.ebp;
               mstate.ebp = li32(mstate.esp);
               mstate.esp = mstate.esp + 4;
               mstate.esp = mstate.esp + 4;
               mstate.gworker = caller;
               return;
            case 2:
               this.i0 = mstate.eax;
               mstate.esp = mstate.esp + 8;
               this.i0 = li32(FSM_atexit);
               this.i2 = li32(this.i0 + 4);
               if(this.i2 <= 31)
               {
                  break;
               }
               §§goto(addr62);
         }
         this.i2 = 1;
         this.i3 = li32(this.i0 + 4);
         this.i4 = this.i3 << 4;
         this.i4 = this.i0 + this.i4;
         si32(this.i2,this.i4 + 8);
         si32(this.i1,this.i4 + 12);
         this.i1 = 0;
         si32(this.i1,this.i4 + 16);
         si32(this.i1,this.i4 + 20);
         this.i1 = this.i3 + 1;
         si32(this.i1,this.i0 + 4);
         §§goto(addr330);
      }
   }
}
