package cmodule.decry
{
   class CVoidTypemap extends CTypemap
   {
       
      
      function CVoidTypemap()
      {
         super();
      }
      
      override public function fromReturnRegs(param1:Object) : *
      {
         return undefined;
      }
      
      override public function toReturnRegs(param1:Object, param2:*, param3:int = 0) : void
      {
      }
      
      override public function get typeSize() : int
      {
         return 0;
      }
   }
}
