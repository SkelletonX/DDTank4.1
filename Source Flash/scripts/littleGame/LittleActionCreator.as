package littleGame
{
   import littleGame.actions.AddObjectAction;
   import littleGame.actions.InhaleAction;
   import littleGame.actions.LittleAction;
   import littleGame.actions.LittleLivingDieAction;
   import littleGame.actions.RemoveObjectAction;
   import littleGame.actions.UnInhaleAction;
   import littleGame.data.LittleActType;
   import road7th.comm.PackageIn;
   
   public class LittleActionCreator
   {
       
      
      public function LittleActionCreator()
      {
         super();
      }
      
      public static function CreatAction(param1:String, param2:PackageIn = null, ... rest) : LittleAction
      {
         var _loc4_:LittleAction = null;
         switch(param1)
         {
            case LittleActType.LivingInhale:
               _loc4_ = new InhaleAction();
               break;
            case LittleActType.AddObject:
               _loc4_ = new AddObjectAction();
               break;
            case LittleActType.RemoveObject:
               _loc4_ = new RemoveObjectAction();
               break;
            case LittleActType.LivingUnInhale:
               _loc4_ = new UnInhaleAction();
               break;
            case LittleActType.LivingDie:
               _loc4_ = new LittleLivingDieAction();
         }
         return _loc4_;
      }
   }
}
