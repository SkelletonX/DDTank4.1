package cmodule.decry
{
   const i_AS3_True:int = exportSym("_AS3_True",new CProcTypemap(CTypemap.AS3ValType,[]).createC(function():Boolean
   {
      return true;
   })[0]);
}
