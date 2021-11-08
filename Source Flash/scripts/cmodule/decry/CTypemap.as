package cmodule.decry
{
   class CTypemap
   {
      
      public static var AS3ValType:CAS3ValTypemap = new CAS3ValTypemap();
      
      public static var DoubleType:CDoubleTypemap = new CDoubleTypemap();
      
      public static var VoidType:CVoidTypemap = new CVoidTypemap();
      
      public static var DoubleRefType:CRefTypemap = new CRefTypemap(CTypemap.DoubleType);
      
      public static var StrRefType:CRefTypemap = new CRefTypemap(CTypemap.StrType);
      
      public static var IntRefType:CRefTypemap = new CRefTypemap(CTypemap.IntType);
      
      public static var SizedStrType:CSizedStrUTF8Typemap = new CSizedStrUTF8Typemap();
      
      public static var IntType:CIntTypemap = new CIntTypemap();
      
      public static var StrType:CStrUTF8Typemap = new CStrUTF8Typemap();
      
      public static var PtrType:CPtrTypemap = new CPtrTypemap();
      
      public static var BufferType:CBufferTypemap = new CBufferTypemap();
       
      
      function CTypemap()
      {
         super();
      }
      
      public static function getTypeByName(param1:String) : CTypemap
      {
         return CTypemap[param1];
      }
      
      public static function getTypesByNames(param1:String) : Array
      {
         return CTypemap.getTypesByNameArray(param1.split(/\s*,\s*/));
      }
      
      public static function getTypesByNameArray(param1:Array) : Array
      {
         var _loc3_:* = undefined;
         var _loc2_:Array = [];
         if(param1)
         {
            for each(_loc3_ in param1)
            {
               _loc2_.push(CTypemap.getTypeByName(_loc3_));
            }
         }
         return _loc2_;
      }
      
      public function fromC(param1:Array) : *
      {
         return undefined;
      }
      
      public function writeValue(param1:int, param2:*) : void
      {
         var _loc3_:Array = this.createC(param2);
         CTypemap.ds.position = param1;
         var _loc4_:int = 0;
         while(_loc4_ < _loc3_.length)
         {
            CTypemap.ds.writeInt(_loc3_[_loc4_]);
            _loc4_++;
         }
      }
      
      public function readValue(param1:int) : *
      {
         var _loc2_:Array = [];
         CTypemap.ds.position = param1;
         var _loc3_:int = 0;
         while(_loc3_ < this.typeSize)
         {
            _loc2_.push(CTypemap.ds.readInt());
            _loc3_++;
         }
         return this.fromC(_loc2_);
      }
      
      public function get ptrLevel() : int
      {
         return 0;
      }
      
      public function createC(param1:*, param2:int = 0) : Array
      {
         return null;
      }
      
      public function fromReturnRegs(param1:Object) : *
      {
         var _loc2_:Array = [param1.eax];
         var _loc3_:* = this.fromC(_loc2_);
         this.destroyC(_loc2_);
         return _loc3_;
      }
      
      public function destroyC(param1:Array) : void
      {
      }
      
      public function toReturnRegs(param1:Object, param2:*, param3:int = 0) : void
      {
         param1.eax = this.createC(param2,param3)[0];
      }
      
      public function get typeSize() : int
      {
         return 4;
      }
      
      public function getValueSize(param1:*) : int
      {
         return this.typeSize;
      }
   }
}
