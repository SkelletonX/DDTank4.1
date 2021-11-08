package cmodule.decry
{
   class CAS3ValTypemap extends CTypemap
   {
       
      
      private var values:ValueTracker;
      
      function CAS3ValTypemap()
      {
         this.values = new ValueTracker();
         super();
      }
      
      override public function fromC(param1:Array) : *
      {
         return this.values.get(param1[0]);
      }
      
      override public function createC(param1:*, param2:int = 0) : Array
      {
         return [this.values.acquire(param1)];
      }
      
      override public function destroyC(param1:Array) : void
      {
         this.values.release(param1[0]);
      }
      
      public function get valueTracker() : ValueTracker
      {
         return this.values;
      }
   }
}
