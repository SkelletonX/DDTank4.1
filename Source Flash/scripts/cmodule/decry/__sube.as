package cmodule.decry
{
   public function __sube(param1:uint, param2:uint) : uint
   {
      var _loc3_:uint = param1 - param2 - gstate.cf;
      gstate.cf = uint(_loc3_ > param1);
      return _loc3_;
   }
}
