package cmodule.decry
{
   import flash.utils.ByteArray;
   
   class CStrUTF8Buffer extends CBuffer
   {
       
      
      private var nullTerm:Boolean;
      
      function CStrUTF8Buffer(param1:int, param2:Boolean = true, param3:ICAllocator = null)
      {
         super(param1,param3);
         this.nullTerm = param2;
      }
      
      override protected function computeValue() : *
      {
         var _loc1_:int = 0;
         var _loc2_:int = this.size;
         CStrUTF8Buffer.ds.position = this.ptr;
         while(_loc2_-- && CStrUTF8Buffer.ds.readByte() != 0)
         {
            _loc1_++;
         }
         CStrUTF8Buffer.ds.position = this.ptr;
         return CStrUTF8Buffer.ds.readUTFBytes(_loc1_);
      }
      
      override protected function setValue(param1:*) : void
      {
         var _loc2_:ByteArray = new ByteArray();
         var _loc3_:int = !!this.nullTerm?int(this.size - 1):int(this.size);
         _loc2_.writeUTFBytes(param1);
         if(_loc2_.length > _loc3_)
         {
            _loc2_.length = _loc3_;
         }
         if(_loc2_.length < this.size)
         {
            _loc2_.writeByte(0);
         }
         _loc2_.position = 0;
         _loc2_.readBytes(CStrUTF8Buffer.ds,this.ptr);
      }
   }
}
