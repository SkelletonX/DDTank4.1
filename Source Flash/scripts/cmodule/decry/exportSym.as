package cmodule.decry
{
   public function exportSym(param1:String, param2:int) : int
   {
      gstate.syms[param1] = param2;
      return param2;
   }
}
