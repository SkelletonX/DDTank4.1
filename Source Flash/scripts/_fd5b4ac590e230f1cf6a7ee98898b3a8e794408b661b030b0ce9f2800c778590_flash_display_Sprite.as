package
{
   import flash.display.Sprite;
   import flash.system.Security;
   
   [ExcludeClass]
   public class _fd5b4ac590e230f1cf6a7ee98898b3a8e794408b661b030b0ce9f2800c778590_flash_display_Sprite extends Sprite
   {
       
      
      public function _fd5b4ac590e230f1cf6a7ee98898b3a8e794408b661b030b0ce9f2800c778590_flash_display_Sprite()
      {
         super();
      }
      
      public function allowDomainInRSL(... rest) : void
      {
         Security.allowDomain.apply(null,rest);
      }
      
      public function allowInsecureDomainInRSL(... rest) : void
      {
         Security.allowInsecureDomain.apply(null,rest);
      }
   }
}
