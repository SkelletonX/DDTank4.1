package trainer
{
   import ddt.manager.PlayerManager;
   import flash.utils.getDefinitionByName;
   
   public class TrainStep
   {
      
      private static const MAX_LEVEL:int = 15;
      
      public static var Step = getDefinitionByName("TrainerStep") as Class;
       
      
      public function TrainStep()
      {
         super();
      }
      
      public static function send(param1:int) : void
      {
         if(PlayerManager.Instance.Self.Grade >= MAX_LEVEL)
         {
            return;
         }
         if(Step)
         {
            Step.instance.send(param1);
         }
      }
      
      public static function get currentStep() : int
      {
         if(Step)
         {
            return Step.instance.currentStep;
         }
         return 0;
      }
   }
}
