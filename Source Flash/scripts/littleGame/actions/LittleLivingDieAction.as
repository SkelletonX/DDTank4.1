package littleGame.actions
{
   import littleGame.model.LittleLiving;
   import littleGame.model.Scenario;
   import road7th.comm.PackageIn;
   
   public class LittleLivingDieAction extends LittleAction
   {
       
      
      private var _lifeTime:int;
      
      private var _life:int;
      
      private var _scene:Scenario;
      
      public function LittleLivingDieAction(param1:Scenario = null, param2:LittleLiving = null, param3:int = 6)
      {
         _living = param2;
         if(_living)
         {
            _living.dieing = true;
         }
         this._life = param3;
         this._scene = param1;
         super();
      }
      
      override public function connect(param1:LittleAction) : Boolean
      {
         return true;
      }
      
      override public function parsePackege(param1:Scenario, param2:PackageIn = null) : void
      {
         this._scene = param1;
         var _loc3_:int = param2.readInt();
         var _loc4_:LittleLiving = this._scene.findLiving(_loc3_);
         if(_loc4_ && !_loc4_.dieing)
         {
            _living = _loc4_;
            _living.stand();
            _living.dieing = true;
            _living.act(this);
         }
      }
      
      override public function prepare() : void
      {
         _living.dieing = true;
         this._life = _living.dieLife;
         _living.doAction("die");
      }
      
      override public function execute() : void
      {
         if(this._lifeTime >= this._life)
         {
            _living.dieing = false;
            this._scene.removeLiving(_living);
            finish();
         }
         this._lifeTime++;
      }
   }
}
