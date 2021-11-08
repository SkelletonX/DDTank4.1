package cmodule.decry
{
   import flash.utils.ByteArray;
   
   class LEByteArray extends ByteArray
   {
       
      
      function LEByteArray()
      {
         super();
         super.endian = "littleEndian";
      }
      
      override public function set endian(param1:String) : void
      {
         throw "LEByteArray endian set attempted";
      }
   }
}
