package cmodule.decry
{
   function findMachineForESP(param1:int) : Machine
   {
      var _loc2_:* = null;
      for(_loc2_ in gsetjmpMachine2ESPMap)
      {
         if(gsetjmpMachine2ESPMap[_loc2_] == param1)
         {
            return Machine(_loc2_);
         }
      }
      return null;
   }
}
