package littleGame.actions
{
   import ddt.ddt_internal;
   import flash.geom.Point;
   import littleGame.data.Grid;
   import littleGame.data.Node;
   import littleGame.model.LittleLiving;
   import littleGame.model.Scenario;
   
   use namespace ddt_internal;
   
   public class LittleLivingMoveAction extends LittleAction
   {
       
      
      protected var _path:Array;
      
      protected var _grid:Grid;
      
      protected var _idx:int = 0;
      
      protected var _scene:Scenario;
      
      protected var _len:int;
      
      protected var _totalTime:int;
      
      protected var _elapsed:int;
      
      public function LittleLivingMoveAction(param1:LittleLiving, param2:Array, param3:Scenario)
      {
         this._scene = param3;
         _living = param1;
         this._path = param2;
         this._grid = this._scene == null?null:this._scene.grid;
         super();
      }
      
      override public function connect(param1:LittleAction) : Boolean
      {
         var _loc2_:LittleLivingMoveAction = null;
         if(param1 is InhaleAction)
         {
            this.cancel();
            return false;
         }
         if(param1 is LittleLivingMoveAction)
         {
            _loc2_ = param1 as LittleLivingMoveAction;
            this._scene = _loc2_._scene;
            _living = _loc2_._living;
            this._path = _loc2_._path;
            this._grid = _loc2_._grid;
            this._len = _loc2_._len;
            this._idx = 0;
            return true;
         }
         return false;
      }
      
      override public function prepare() : void
      {
         var _loc1_:Node = null;
         var _loc2_:Point = null;
         if(this._path)
         {
            _loc1_ = this._path[0];
            _loc2_ = new Point(_loc1_.x,_loc1_.y);
            if(_living)
            {
               _living.setNextDirection(_loc2_);
               _living.pos = _loc2_;
            }
         }
         super.prepare();
      }
      
      override public function execute() : void
      {
         var _loc1_:Node = null;
         var _loc2_:Point = null;
         if(this._path && this._idx < this._path.length)
         {
            _loc1_ = this._path[this._idx++];
            if(_loc1_)
            {
               _loc2_ = new Point(_loc1_.x,_loc1_.y);
               _living.setNextDirection(_loc2_);
               _living.pos = _loc2_;
            }
         }
         else
         {
            this.finish();
         }
      }
      
      override protected function finish() : void
      {
         var _loc2_:Point = null;
         _living.doAction("stand");
         var _loc1_:Node = this._path[this._path.length - 1];
         if(_loc1_)
         {
            _loc2_ = new Point(_loc1_.x,_loc1_.y);
            _living.pos = _loc2_;
         }
         _living = null;
         this._grid = null;
         this._scene = null;
         this._path = null;
         super.finish();
      }
      
      override public function cancel() : void
      {
         _isFinished = true;
         _living = null;
         this._grid = null;
         this._scene = null;
         this._path = null;
      }
   }
}
