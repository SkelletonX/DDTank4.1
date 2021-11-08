package cmodule.decry
{
   import flash.utils.ByteArray;
   
   class ByteArrayIO extends IO
   {
       
      
      public var byteArray:ByteArray;
      
      function ByteArrayIO()
      {
         super();
      }
      
      override public function set size(param1:int) : void
      {
         if(!this.byteArray)
         {
            throw new AlchemyBlock();
         }
         this.byteArray.length = param1;
      }
      
      override public function read(param1:int, param2:int) : int
      {
         if(!this.byteArray)
         {
            throw new AlchemyBlock();
         }
         var _loc3_:int = Math.min(param2,this.byteArray.bytesAvailable);
         if(_loc3_)
         {
            this.byteArray.readBytes(ByteArrayIO.ds,param1,_loc3_);
         }
         return _loc3_;
      }
      
      override public function get size() : int
      {
         if(!this.byteArray)
         {
            throw new AlchemyBlock();
         }
         return this.byteArray.length;
      }
      
      override public function get position() : int
      {
         if(!this.byteArray)
         {
            throw new AlchemyBlock();
         }
         return this.byteArray.position;
      }
      
      override public function set position(param1:int) : void
      {
         if(!this.byteArray)
         {
            throw new AlchemyBlock();
         }
         this.byteArray.position = param1;
      }
      
      override public function write(param1:int, param2:int) : int
      {
         if(!this.byteArray)
         {
            throw new AlchemyBlock();
         }
         if(param2)
         {
            this.byteArray.writeBytes(ByteArrayIO.ds,param1,param2);
         }
         return param2;
      }
   }
}
