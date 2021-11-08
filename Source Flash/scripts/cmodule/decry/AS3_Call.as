package cmodule.decry
{
   function AS3_Call(param1:*, param2:Object, param3:Array) : *
   {
      return param1.apply(param2,param3);
   }
}
