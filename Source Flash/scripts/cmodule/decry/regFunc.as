package cmodule.decry
{
   public function regFunc(param1:Function) : int
   {
      return gstate.funcs.push(param1) - 1;
   }
}
