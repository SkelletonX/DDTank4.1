package cmodule.decry
{
   public function memcpy(param1:int, param2:int, param3:int) : int
   {
      if(param3)
      {
         gstate.ds.position = param1;
         gstate.ds.writeBytes(gstate.ds,param2,param3);
      }
      return param1;
   }
}
