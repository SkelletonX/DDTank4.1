package cmodule.decry
{
   class CSizedStrUTF8Typemap extends CTypemap
   {
       
      
      function CSizedStrUTF8Typemap()
      {
         super();
      }
      
      override public function fromC(param1:Array) : *
      {
         CSizedStrUTF8Typemap.ds.position = param1[0];
         return CSizedStrUTF8Typemap.ds.readUTFBytes(param1[1]);
      }
      
      override public function get typeSize() : int
      {
         return 8;
      }
   }
}
