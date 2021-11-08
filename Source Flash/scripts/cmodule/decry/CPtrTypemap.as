package cmodule.decry
{
   class CPtrTypemap extends CTypemap
   {
       
      
      function CPtrTypemap()
      {
         super();
      }
      
      override public function fromC(param1:Array) : *
      {
         return int(param1[0]);
      }
      
      override public function createC(param1:*, param2:int = 0) : Array
      {
         return [int(param1)];
      }
   }
}
