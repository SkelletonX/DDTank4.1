package cmodule.decry
{
   function AS3_CallS(param1:String, param2:Object, param3:Array) : *
   {
      return param2[param1].apply(param2,param3);
   }
}
