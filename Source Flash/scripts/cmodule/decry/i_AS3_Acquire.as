package cmodule.decry
{
   const i_AS3_Acquire:int = exportSym("_AS3_Acquire",new CProcTypemap(CTypemap.VoidType,[CTypemap.PtrType]).createC(CTypemap.AS3ValType.valueTracker.acquireId)[0]);
}
