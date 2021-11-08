package cmodule.decry
{
   public function isinf(param1:Number) : int
   {
      return int(param1 === Number.POSITIVE_INFINITY || param1 === Number.NEGATIVE_INFINITY);
   }
}
