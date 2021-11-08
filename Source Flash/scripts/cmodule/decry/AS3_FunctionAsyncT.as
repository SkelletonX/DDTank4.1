package cmodule.decry
{
   function AS3_FunctionAsyncT(param1:int, param2:int, param3:String, param4:String, param5:Boolean) : Function
   {
      var _loc6_:CTypemap = new CProcTypemap(CTypemap.getTypeByName(param3),CTypemap.getTypesByNames(param4),param5,true);
      return AS3_FunctionAsync(param1,_loc6_.fromC([param2]));
   }
}
