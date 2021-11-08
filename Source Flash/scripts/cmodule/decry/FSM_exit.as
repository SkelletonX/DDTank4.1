package cmodule.decry
{
   import avm2.intrinsics.memory.li32;
   import avm2.intrinsics.memory.li8;
   import avm2.intrinsics.memory.si32;
   
   public final class FSM_exit extends Machine
   {
      
      public static const intRegCount:int = 2;
      
      public static const NumberRegCount:int = 0;
       
      
      public var i0:int;
      
      public var i1:int;
      
      public function FSM_exit()
      {
         super();
      }
      
      public static function start() : void
      {
         var _loc1_:FSM_exit = null;
         _loc1_ = new FSM_exit();
         FSM_exit.gworker = _loc1_;
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
               this.i0 = li8(FSM_exit);
               this.i1 = li32(mstate.ebp + 8);
               this.i0 = this.i0 ^ 1;
               this.i0 = this.i0 & 1;
               if(this.i0 == 0)
               {
                  state = 1;
                  mstate.esp = mstate.esp - 4;
                  FSM_exit.start();
                  return;
               }
            case 1:
               mstate.esp = mstate.esp - 4;
               si32(this.i1,mstate.esp);
               mstate.esp = mstate.esp - 4;
               FSM_exit.start();
            case 2:
               mstate.esp = mstate.esp + 4;
         }
         throw "Invalid state in _exit";
      }
   }
}
