package cmodule.decry
{
   class CRefTypemap extends CTypemap
   {
       
      
      private var subtype:CTypemap;
      
      function CRefTypemap(param1:CTypemap)
      {
         super();
         this.subtype = param1;
      }
      
      override public function fromC(param1:Array) : *
      {
         var _loc2_:int = param1[0];
         var _loc3_:int = 0;
         while(_loc3_ < this.subtype.ptrLevel)
         {
            CRefTypemap.ds.position = _loc2_;
            _loc2_ = CRefTypemap.ds.readInt();
            _loc3_++;
         }
         return this.subtype.readValue(_loc2_);
      }
      
      override public function createC(param1:*, param2:int = 0) : Array
      {
         return null;
      }
   }
}
