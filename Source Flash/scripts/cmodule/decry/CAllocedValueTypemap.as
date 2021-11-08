package cmodule.decry
{
   class CAllocedValueTypemap extends CTypemap
   {
       
      
      private var allocator:ICAllocator;
      
      function CAllocedValueTypemap(param1:ICAllocator)
      {
         super();
         this.allocator = param1;
      }
      
      override public function fromC(param1:Array) : *
      {
         return readValue(param1[0]);
      }
      
      protected function alloc(param1:*) : int
      {
         return this.allocator.alloc(getValueSize(param1));
      }
      
      override public function createC(param1:*, param2:int = 0) : Array
      {
         if(!param2)
         {
            param2 = this.alloc(param1);
         }
         writeValue(param2,param1);
         return [param2];
      }
      
      override public function destroyC(param1:Array) : void
      {
         this.free(param1[0]);
      }
      
      protected function free(param1:int) : void
      {
         return this.allocator.free(param1);
      }
   }
}
