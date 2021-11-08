package cmodule.decry
{
   public function __addc(param1:uint, param2:uint) : uint
   {
      var _loc3_:uint = param1 + param2;
      gstate.cf = uint(_loc3_ < param1);
      return _loc3_;
   }
}
