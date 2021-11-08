package cmodule.decry
{
   function AS3_CallTS(param1:String, param2:Object, param3:String, param4:int) : *
   {
      return param2[param1].apply(param2,AS3_Array(param3,param4));
   }
}
