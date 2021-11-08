package cmodule.decry
{
   function AS3_CallT(param1:*, param2:Object, param3:String, param4:int) : *
   {
      return param1.apply(param2,AS3_Array(param3,param4));
   }
}
