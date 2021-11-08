package cmodule.decry
{
   const i_AS3_Release:int = exportSym("_AS3_Release",new CProcTypemap(CTypemap.VoidType,[CTypemap.PtrType]).createC(CTypemap.AS3ValType.valueTracker.release)[0]);
}
