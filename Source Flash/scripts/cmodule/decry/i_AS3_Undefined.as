package cmodule.decry
{
   const i_AS3_Undefined:int = exportSym("_AS3_Undefined",new CProcTypemap(CTypemap.AS3ValType,[]).createC(function():*
   {
      return undefined;
   })[0]);
}
