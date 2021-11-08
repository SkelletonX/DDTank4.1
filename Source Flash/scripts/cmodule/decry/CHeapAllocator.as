package cmodule.decry
{
   class CHeapAllocator implements ICAllocator
   {
       
      
      private var pmalloc:Function;
      
      private var pfree:Function;
      
      function CHeapAllocator()
      {
         super();
      }
      
      public function free(param1:int) : void
      {
         if(this.pfree == null)
         {
            this.pfree = new CProcTypemap(CHeapAllocator.VoidType,[CHeapAllocator.PtrType]).fromC([CHeapAllocator]);
         }
         this.pfree(param1);
      }
      
      public function alloc(param1:int) : int
      {
         if(this.pmalloc == null)
         {
            this.pmalloc = new CProcTypemap(CHeapAllocator.PtrType,[CHeapAllocator.IntType]).fromC([CHeapAllocator]);
         }
         var _loc2_:int = this.pmalloc(param1);
         return _loc2_;
      }
   }
}
