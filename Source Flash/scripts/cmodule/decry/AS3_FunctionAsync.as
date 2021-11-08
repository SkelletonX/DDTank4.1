package cmodule.decry
{
   function AS3_FunctionAsync(param1:int, param2:Function) : Function
   {
      var data:int = param1;
      var func:Function = param2;
      return function(... rest):*
      {
         var _loc2_:* = rest.shift();
         return func(_loc2_,data,rest);
      };
   }
}
