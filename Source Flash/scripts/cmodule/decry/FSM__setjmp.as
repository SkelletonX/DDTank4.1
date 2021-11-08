package cmodule.decry
{
   public class FSM__setjmp extends Machine
   {
       
      
      public function FSM__setjmp()
      {
         super();
      }
      
      public static function start() : void
      {
         FSM__setjmp.gworker = new FSM__setjmp();
         throw new AlchemyDispatch();
      }
      
      override public function work() : void
      {
         mstate.pop();
         var _loc1_:int = _mr32(mstate.esp);
         _mw32(_loc1_ + 0,667788);
         _mw32(_loc1_ + 4,caller.state);
         _mw32(_loc1_ + 8,mstate.esp);
         _mw32(_loc1_ + 12,mstate.ebp);
         _mw32(_loc1_ + 16,887766);
         log(4,"setjmp: " + _loc1_);
         var _loc2_:Machine = findMachineForESP(mstate.esp);
         if(_loc2_)
         {
            delete FSM__setjmp[_loc2_];
         }
         FSM__setjmp[caller] = mstate.esp;
         mstate.gworker = caller;
         mstate.eax = 0;
      }
   }
}
