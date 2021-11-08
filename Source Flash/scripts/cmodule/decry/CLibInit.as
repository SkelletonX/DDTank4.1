package cmodule.decry
{
   import flash.display.Sprite;
   import flash.utils.ByteArray;
   
   public class CLibInit
   {
       
      
      public function CLibInit()
      {
         super();
      }
      
      public function init() : *
      {
         var result:* = undefined;
         var regged:Boolean = false;
         var runner:CRunner = new CRunner(true);
         var saveState:MState = new MState(null);
         CLibInit.copyTo(saveState);
         try
         {
            runner.startSystem();
            while(true)
            {
               try
               {
                  while(true)
                  {
                     runner.work();
                  }
               }
               catch(e:AlchemyDispatch)
               {
                  continue;
               }
               catch(e:AlchemyYield)
               {
                  continue;
               }
            }
         }
         catch(e:AlchemyLibInit)
         {
            log(3,"Caught AlchemyLibInit " + e.rv);
            regged = true;
            result = CLibInit.AS3ValType.valueTracker.release(e.rv);
         }
         finally
         {
            saveState.copyTo(CLibInit);
            if(!regged)
            {
               log(1,"Lib didn\'t register");
            }
         }
         return result;
      }
      
      public function supplyFile(param1:String, param2:ByteArray) : void
      {
         CLibInit[param1] = param2;
      }
      
      public function putEnv(param1:String, param2:String) : void
      {
         CLibInit[param1] = param2;
      }
      
      public function setSprite(param1:Sprite) : void
      {
         var CLibInit:* = param1;
      }
   }
}
