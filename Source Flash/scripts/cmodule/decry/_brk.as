package cmodule.decry
{
   public function _brk(param1:int) : int
   {
      var _loc2_:int = param1;
      gstate.ds.length = _loc2_;
      return 0;
   }
}
