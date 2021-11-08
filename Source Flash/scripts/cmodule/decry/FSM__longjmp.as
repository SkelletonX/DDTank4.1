package cmodule.decry
{
   public class FSM__longjmp extends Machine
   {
       
      
      public function FSM__longjmp()
      {
         super();
      }
      
      public static function start() : void
      {
         FSM__longjmp.gworker = new FSM__longjmp();
         throw new AlchemyDispatch();
      }
      
      override public function work() : void
      {
         mstate.pop();
         var _loc1_:int = _mr32(mstate.esp);
         var _loc2_:int = _mr32(mstate.esp + 4);
         log(4,"longjmp: " + _loc1_);
         var _loc3_:int = _mr32(_loc1_ + 4);
         var _loc4_:int = _mr32(_loc1_ + 8);
         var _loc5_:int = _mr32(_loc1_ + 12);
         log(3,"longjmp -- buf: " + _loc1_ + " state: " + _loc3_ + " esp: " + _loc4_ + " ebp: " + _loc5_);
         if(!_loc1_ || !_loc4_ || !_loc5_)
         {
            throw "longjmp -- bad jmp_buf";
         }
         var _loc6_:Machine = findMachineForESP(_loc4_);
         if(!_loc6_)
         {
            debugTraceMem(_loc1_ - 24,_loc1_ + 24);
            throw "longjmp -- bad esp";
         }
         delete FSM__longjmp[_loc6_];
         mstate.gworker = _loc6_;
         _loc6_.state = _loc3_;
         mstate.esp = _loc4_;
         mstate.ebp = _loc5_;
         mstate.eax = _loc2_;
         throw new AlchemyDispatch();
      }
   }
}
