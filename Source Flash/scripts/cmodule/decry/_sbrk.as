package cmodule.decry
{
   public function _sbrk(param1:int) : int
   {
      var _loc2_:int = gstate.ds.length;
      var _loc3_:int = _loc2_ + param1;
      gstate.ds.length = _loc3_;
      return _loc2_;
   }
}
