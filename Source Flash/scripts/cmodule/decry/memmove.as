package cmodule.decry
{
   public function memmove(param1:int, param2:int, param3:int) : int
   {
      var _loc4_:int = 0;
      if(param2 > param1 || param2 + param3 < param1)
      {
         memcpy(param1,param2,param3);
      }
      else
      {
         _loc4_ = param1 + param3;
         param2 = param2 + param3;
         while(param3--)
         {
            gstate.ds[--_loc4_] = gstate.ds[--param2];
         }
      }
      return param1;
   }
}
