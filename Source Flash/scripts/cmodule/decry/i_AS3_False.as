package cmodule.decry
{
   const i_AS3_False:int = exportSym("_AS3_False",new CProcTypemap(CTypemap.AS3ValType,[]).createC(function():Boolean
   {
      return false;
   })[0]);
}
