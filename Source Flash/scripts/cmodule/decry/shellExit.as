package cmodule.decry
{
   function shellExit(param1:int) : void
   {
      var ns:Namespace = null;
      var nativeApp:Object = null;
      var nativeAppClass:Object = null;
      var res:int = param1;
      ns = new Namespace("flash.desktop");
      try
      {
         nativeAppClass = ns::["NativeApplication"];
         nativeApp = nativeAppClass.nativeApplication;
      }
      catch(e:*)
      {
         log(3,"No nativeApplication: " + e);
      }
      if(nativeApp)
      {
         nativeApp.exit(res);
         return;
      }
      throw new AlchemyExit(res);
   }
}
