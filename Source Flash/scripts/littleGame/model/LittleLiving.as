package littleGame.model
{
   import ddt.ddt_internal;
   import flash.events.EventDispatcher;
   import flash.geom.Point;
   import littleGame.actions.LittleAction;
   import littleGame.actions.LittleActionManager;
   import littleGame.actions.LittleLivingMoveAction;
   import littleGame.data.DirectionType;
   import littleGame.data.Node;
   import littleGame.events.LittleLivingEvent;
   import road7th.comm.PackageIn;
   
   use namespace ddt_internal;
   
   [Event(name="doAction",type="littleGame.events.LittleLivingEvent")]
   [Event(name="directionChanged",type="littleGame.events.LittleLivingEvent")]
   [Event(name="posChanged",type="littleGame.events.LittleLivingEvent")]
   [Event(name="die",type="littleGame.events.LittleLivingEvent")]
   public class LittleLiving extends EventDispatcher
   {
      
      public static var count:int = 0;
       
      
      public var bornLife:int;
      
      public var dieLife:int;
      
      public var dieing:Boolean = false;
      
      public var borning:Boolean = false;
      
      public var speed:int = 10;
      
      public var collideable:Boolean = false;
      
      public var name:String;
      
      public var size:int;
      
      public var MotionState:int = 2;
      
      public var inGame:Boolean = false;
      
      public var dx:int;
      
      public var dy:int;
      
      public var lock:Boolean;
      
      private var _id:int;
      
      ddt_internal var _modelID:String;
      
      ddt_internal var gridIdx:int;
      
      private var _pos:Point;
      
      private var _path:Array;
      
      private var _type:int;
      
      private var _inhaled:Boolean = false;
      
      private var _onProcess:Boolean = false;
      
      private var _idx:int = 0;
      
      private var _direction:String = "leftDown";
      
      protected var _actionMgr:LittleActionManager;
      
      protected var _currentAction;
      
      public var servPath:Array;
      
      public function LittleLiving(param1:int, param2:int, param3:int, param4:int, param5:String = null)
      {
         this._pos = new Point(400,400);
         this._id = param1;
         this._pos = new Point(param2,param3);
         this._modelID = param5;
         this._type = param4;
         this._actionMgr = new LittleActionManager();
         super();
      }
      
      public function get id() : int
      {
         return this._id;
      }
      
      public function get type() : int
      {
         return this._type;
      }
      
      public function dispose() : void
      {
         this._actionMgr = null;
      }
      
      public final function update() : void
      {
         this._actionMgr.execute();
      }
      
      public function act(param1:LittleAction) : void
      {
         this._actionMgr.act(param1);
      }
      
      public function get currentAction() : *
      {
         return this._currentAction;
      }
      
      public function doAction(param1:*) : void
      {
         if(this._currentAction != param1)
         {
            this._currentAction = param1;
            dispatchEvent(new LittleLivingEvent(LittleLivingEvent.DoAction,param1));
         }
      }
      
      public function stand() : void
      {
         var _loc1_:LittleAction = null;
         for each(_loc1_ in this._actionMgr._queue)
         {
            if(_loc1_ is LittleLivingMoveAction)
            {
               LittleLivingMoveAction(_loc1_).cancel();
            }
         }
      }
      
      public function get pos() : Point
      {
         return this._pos;
      }
      
      public function set pos(param1:Point) : void
      {
         var _loc2_:Point = this._pos;
         this._pos = param1;
         dispatchEvent(new LittleLivingEvent(LittleLivingEvent.PosChenged,_loc2_));
      }
      
      public function get isPlayer() : Boolean
      {
         return false;
      }
      
      public function get isSelf() : Boolean
      {
         return false;
      }
      
      public function set direction(param1:String) : void
      {
         if(this._direction != param1)
         {
            this._direction = param1;
            dispatchEvent(new LittleLivingEvent(LittleLivingEvent.DirectionChanged));
         }
      }
      
      public function get direction() : String
      {
         return this._direction;
      }
      
      public function get isBack() : Boolean
      {
         return this._direction == DirectionType.RIGHT_UP || this._direction == DirectionType.LEFT_UP;
      }
      
      public function get isLeft() : Boolean
      {
         return this._direction == DirectionType.LEFT_DOWN || this._direction == DirectionType.LEFT_UP;
      }
      
      ddt_internal function setNextDirection(param1:Point) : void
      {
         if(param1.x > this.pos.x && param1.y >= this.pos.y)
         {
            this.direction = DirectionType.RIGHT_DOWN;
            this.doAction("walk");
         }
         else if(param1.x > this.pos.x && param1.y < this.pos.y)
         {
            this.direction = DirectionType.RIGHT_UP;
            this.doAction("backWalk");
         }
         else if(param1.x < this.pos.x && param1.y >= this.pos.y)
         {
            this.direction = DirectionType.LEFT_DOWN;
            this.doAction("walk");
         }
         else if(param1.x < this.pos.x && param1.y < this.pos.y)
         {
            this.direction = DirectionType.LEFT_UP;
            this.doAction("backWalk");
         }
         else if(param1.y > this.pos.y)
         {
            this.doAction("walk");
         }
         else if(param1.y < this.pos.y)
         {
            this.doAction("backWalk");
         }
         else if(this.isBack)
         {
            this.doAction("backWalk");
         }
         else
         {
            this.doAction("walk");
         }
      }
      
      override public function toString() : String
      {
         return "LittleLiving_" + this._id;
      }
      
      public function readServPaht(param1:PackageIn) : void
      {
         this.servPath = [];
         var _loc2_:int = param1.readInt();
         var _loc3_:int = 0;
         while(_loc3_ < _loc2_)
         {
            this.servPath.push(new Node(param1.readInt(),param1.readInt()));
            _loc3_++;
         }
      }
   }
}
