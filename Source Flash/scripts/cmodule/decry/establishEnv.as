package cmodule.decry
{
   public function establishEnv() : void
   {
      var ns:Namespace = null;
      try
      {
         ns = new Namespace("avmplus");
         var gdomainClass:* = ns::["Domain"];
         var gshell:* = true;
      }
      catch(e:*)
      {
      }
      if(!gdomainClass)
      {
         ns = new Namespace("flash.system");
         gdomainClass = ns::["ApplicationDomain"];
      }
   }
}
