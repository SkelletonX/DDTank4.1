package littleGame.actions
{
   import ddt.ddt_internal;
   import flash.geom.Point;
   import flash.utils.getTimer;
   import littleGame.LittleGameManager;
   import littleGame.data.Grid;
   import littleGame.data.Node;
   import littleGame.model.LittleSelf;
   import littleGame.model.Scenario;
   
   use namespace ddt_internal;
   
   public class LittleSelfMoveAction extends LittleAction
   {
       
      
      private var _self:LittleSelf;
      
      private var _path:Array;
      
      private var _grid:Grid;
      
      private var _idx:int = 0;
      
      private var _elapsed:int = 0;
      
      private var _last:int;
      
      private var _startTime:int;
      
      private var _endTime:int;
      
      private var _scene:Scenario;
      
      private var _len:int;
      
      private var _reset:Boolean;
      
      public function LittleSelfMoveAction(param1:LittleSelf, param2:Array, param3:Scenario, param4:int, param5:int, param6:Boolean = false)
      {
         this._scene = param3;
         _living = this._self = param1;
         this._path = param2;
         this._grid = this._scene.grid;
         this._startTime = param4;
         this._endTime = param5;
         this._len = this._path.length;
         this._reset = param6;
         super();
      }
      
      override public function connect(param1:LittleAction) : Boolean
      {
         var _loc2_:LittleSelfMoveAction = null;
         if(param1 is InhaleAction)
         {
            this.cancel();
            return false;
         }
         if(param1 is LittleSelfMoveAction)
         {
            _loc2_ = param1 as LittleSelfMoveAction;
            this._scene = _loc2_._scene;
            this._self = _loc2_._self;
            this._path = _loc2_._path;
            this._grid = _loc2_._grid;
            this._startTime = _loc2_._startTime;
            this._len = _loc2_._len;
            this._idx = 0;
            return true;
         }
         return false;
      }
      
      override public function prepare() : void
      {
         LittleGameManager.Instance.Current.startSysnPos();
         var _loc1_:Node = this._path[0];
         var _loc2_:Point = new Point(_loc1_.x,_loc1_.y);
         this._self.setNextDirection(_loc2_);
         this._self.pos = _loc2_;
         super.prepare();
         this._last = getTimer();
      }
      
      override public function execute() : void
      {
         var _loc1_:Node = null;
         var _loc2_:Point = null;
         if(this._idx < this._path.length)
         {
            _loc1_ = this._path[this._idx++];
            if(_loc1_)
            {
               _loc2_ = new Point(_loc1_.x,_loc1_.y);
               this._self.setNextDirection(_loc2_);
               this._self.pos = _loc2_;
            }
         }
         else
         {
            this.finish();
         }
      }
      
      override protected function finish() : void
      {
         if(this._self.isBack)
         {
            this._self.doAction("backStand");
         }
         else
         {
            this._self.doAction("stand");
         }
         this.synchronous();
         LittleGameManager.Instance.Current.stopSysnPos();
         super.finish();
      }
      
      private function synchronous() : void
      {
         LittleGameManager.Instance.synchronousLivingPos(this._self.pos.x,this._self.pos.y);
      }
      
      override public function cancel() : void
      {
         _isFinished = true;
         _living = null;
      }
   }
}
