package cmodule.decry
{
   import flash.utils.ByteArray;
   
   class GLEByteArrayProvider
   {
       
      
      function GLEByteArrayProvider()
      {
         super();
      }
      
      public static function get() : ByteArray
      {
         var result:ByteArray = null;
         try
         {
            result = GLEByteArrayProvider.currentDomain.domainMemory;
         }
         catch(e:*)
         {
         }
         if(!result)
         {
            result = new LEByteArray();
            try
            {
               result.length = GLEByteArrayProvider.MIN_DOMAIN_MEMORY_LENGTH;
               GLEByteArrayProvider.currentDomain.domainMemory = result;
            }
            catch(e:*)
            {
               log(3,"Not using domain memory");
            }
         }
         return result;
      }
   }
}
