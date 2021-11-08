package cmodule.decry
{
   const i_AS3_Function:int = exportSym("_AS3_Function",new CProcTypemap(CTypemap.AS3ValType,[CTypemap.PtrType,new CProcTypemap(CTypemap.AS3ValType,[CTypemap.PtrType,CTypemap.AS3ValType])]).createC(AS3_Function)[0]);
}
