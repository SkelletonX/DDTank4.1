package cmodule.decry
{
   class CBuffer
   {
      
      private static var ptr2Buffer:Object = {};
       
      
      private var sizeVal:int;
      
      private var valCache;
      
      private var allocator:ICAllocator;
      
      private var ptrVal:int;
      
      function CBuffer(param1:int, param2:ICAllocator = null)
      {
         super();
         if(!param2)
         {
            param2 = new CHeapAllocator();
         }
         this.allocator = param2;
         this.sizeVal = param1;
         this.alloc();
      }
      
      public static function free(param1:int) : void
      {
         ptr2Buffer[param1].free();
      }
      
      public function get size() : int
      {
         return this.sizeVal;
      }
      
      public function set value(param1:*) : void
      {
         if(this.ptrVal)
         {
            this.setValue(param1);
         }
         else
         {
            this.valCache = param1;
         }
      }
      
      public function free() : void
      {
         if(this.ptrVal)
         {
            this.valCache = this.computeValue();
            this.allocator.free(this.ptrVal);
            delete ptr2Buffer[this.ptrVal];
            this.ptrVal = 0;
         }
      }
      
      public function get ptr() : int
      {
         return this.ptrVal;
      }
      
      protected function setValue(param1:*) : void
      {
      }
      
      public function get value() : *
      {
         return Boolean(this.ptrVal)?this.computeValue():this.valCache;
      }
      
      protected function computeValue() : *
      {
         return undefined;
      }
      
      private function alloc() : void
      {
         if(!this.ptrVal)
         {
            this.ptrVal = this.allocator.alloc(this.sizeVal);
            ptr2Buffer[this.ptrVal] = this;
         }
      }
      
      public function reset() : void
      {
         if(!this.ptrVal)
         {
            this.alloc();
            this.setValue(this.valCache);
         }
      }
   }
}
