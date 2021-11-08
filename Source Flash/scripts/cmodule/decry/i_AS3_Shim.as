package cmodule.decry
{
   const i_AS3_Shim:int = exportSym("_AS3_Shim",new CProcTypemap(CTypemap.PtrType,[CTypemap.AS3ValType,CTypemap.AS3ValType,CTypemap.StrType,CTypemap.StrType,CTypemap.IntType]).createC(AS3_Shim)[0]);
}
