package littleGame.actions
{
   import littleGame.LittleGameManager;
   import littleGame.model.LittleLiving;
   import littleGame.model.Scenario;
   import road7th.comm.PackageIn;
   
   public class RemoveObjectAction extends LittleAction
   {
       
      
      private var _scene:Scenario;
      
      private var _pkg:PackageIn;
      
      public function RemoveObjectAction()
      {
         super();
      }
      
      override public function parsePackege(param1:Scenario, param2:PackageIn = null) : void
      {
         this._scene = param1;
         this._pkg = param2;
         var _loc3_:int = this._pkg.readInt();
         param2.readUTF();
         var _loc4_:LittleLiving = this._scene.findLiving(_loc3_);
         if(_loc4_)
         {
            _loc4_.act(this);
         }
      }
      
      override public function execute() : void
      {
         LittleGameManager.Instance.removeObject(this._scene,this._pkg);
         finish();
      }
   }
}
