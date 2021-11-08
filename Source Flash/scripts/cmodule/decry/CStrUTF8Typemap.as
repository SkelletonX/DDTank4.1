package cmodule.decry
{
   import flash.utils.ByteArray;
   
   class CStrUTF8Typemap extends CAllocedValueTypemap
   {
       
      
      function CStrUTF8Typemap(param1:ICAllocator = null)
      {
         if(!param1)
         {
            param1 = new CHeapAllocator();
         }
         super(param1);
      }
      
      protected function ByteArrayForString(param1:String) : ByteArray
      {
         var _loc2_:ByteArray = null;
         _loc2_ = new ByteArray();
         _loc2_.writeUTFBytes(param1);
         _loc2_.writeByte(0);
         _loc2_.position = 0;
         return _loc2_;
      }
      
      override public function readValue(param1:int) : *
      {
         var _loc2_:int = 0;
         CStrUTF8Typemap.ds.position = param1;
         _loc2_ = 0;
         while(CStrUTF8Typemap.ds.readByte() != 0)
         {
            _loc2_++;
         }
         CStrUTF8Typemap.ds.position = param1;
         return CStrUTF8Typemap.ds.readUTFBytes(_loc2_);
      }
      
      override public function getValueSize(param1:*) : int
      {
         return this.ByteArrayForString(String(param1)).length;
      }
      
      override public function get ptrLevel() : int
      {
         return 1;
      }
      
      override public function writeValue(param1:int, param2:*) : void
      {
         this.ByteArrayForString(String(param2)).readBytes(CStrUTF8Typemap.ds,param1);
      }
   }
}
