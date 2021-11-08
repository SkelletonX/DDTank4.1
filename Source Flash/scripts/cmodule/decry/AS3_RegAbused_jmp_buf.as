package cmodule.decry
{
   function AS3_RegAbused_jmp_buf(param1:int) : void
   {
      log(4,"regAbused: " + param1);
      log(1,"Can\'t RegAbused -- abuse support disabled");
   }
}
