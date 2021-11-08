package cmodule.decry
{
   function AS3_Shim(param1:Function, param2:Object, param3:String, param4:String, param5:Boolean) : int
   {
      var func:Function = param1;
      var thiz:Object = param2;
      var rt:String = param3;
      var tt:String = param4;
      var varargs:Boolean = param5;
      var retType:CTypemap = CTypemap.getTypeByName(rt);
      var argTypes:Array = CTypemap.getTypesByNames(tt);
      var tm:CTypemap = new CProcTypemap(retType,argTypes,varargs);
      var id:int = tm.createC(function(... rest):*
      {
         return func.apply(thiz,rest);
      })[0];
      return id;
   }
}
