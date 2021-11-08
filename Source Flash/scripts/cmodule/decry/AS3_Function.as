package cmodule.decry
{
   function AS3_Function(param1:int, param2:Function) : Function
   {
      var data:int = param1;
      var func:Function = param2;
      return function(... rest):*
      {
         return func(data,rest);
      };
   }
}
