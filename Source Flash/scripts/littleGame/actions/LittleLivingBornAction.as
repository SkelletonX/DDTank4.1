package littleGame.actions
{
   import littleGame.model.LittleLiving;
   
   public class LittleLivingBornAction extends LittleAction
   {
       
      
      private var _life:int;
      
      private var _lifeTime:int;
      
      public function LittleLivingBornAction(param1:LittleLiving, param2:int = 10)
      {
         _living = param1;
         super();
      }
      
      override public function prepare() : void
      {
         _living.borning = true;
         this._life = _living.bornLife;
         _living.doAction("born");
         super.prepare();
      }
      
      override public function execute() : void
      {
         this._lifeTime++;
         if(this._lifeTime >= this._life)
         {
            this.finish();
         }
      }
      
      override protected function finish() : void
      {
         _living.borning = false;
         _living.stand();
         super.finish();
      }
   }
}
