package cmodule.decry
{
   import flash.utils.ByteArray;
   
   function AS3_ByteArray_writeBytes(param1:ByteArray, param2:int, param3:int) : int
   {
      log(5,"--- wrteBytes: ba length = " + param1.length + " / " + param3);
      if(param3 > 0)
      {
         param1.writeBytes(gstate.ds,param2,param3);
         return param3;
      }
      return 0;
   }
}
