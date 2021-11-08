package cmodule.decry
{
   public function importSym(param1:String) : int
   {
      var s:String = param1;
      var res:int = gstate.syms[s];
      if(!res)
      {
         log(3,"Undefined sym: " + s);
         return exportSym(s,regFunc(function():*
         {
            throw "Undefined sym: " + s;
         }));
      }
      return res;
   }
}
