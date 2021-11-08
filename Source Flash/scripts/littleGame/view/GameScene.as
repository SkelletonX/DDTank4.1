package littleGame.view
{
   import com.pickgliss.toplevel.StageReferance;
   import com.pickgliss.ui.core.Disposeable;
   import com.pickgliss.utils.ClassUtils;
   import com.pickgliss.utils.ObjectUtils;
   import ddt.manager.ChatManager;
   import flash.display.Bitmap;
   import flash.display.BitmapData;
   import flash.display.DisplayObject;
   import flash.display.DisplayObjectContainer;
   import flash.display.Graphics;
   import flash.display.Sprite;
   import flash.events.MouseEvent;
   import flash.geom.Point;
   import flash.geom.Rectangle;
   import flash.utils.Dictionary;
   import flash.utils.setTimeout;
   import littleGame.LittleGameManager;
   import littleGame.actions.LittleLivingBornAction;
   import littleGame.data.Grid;
   import littleGame.data.Node;
   import littleGame.events.LittleGameEvent;
   import littleGame.events.LittleLivingEvent;
   import littleGame.model.LittleLiving;
   import littleGame.model.LittlePlayer;
   import littleGame.model.LittleSelf;
   import littleGame.model.Scenario;
   import road7th.utils.MovieClipWrapper;
   
   public class GameScene extends Sprite implements Disposeable
   {
       
      
      private var _game:Scenario;
      
      private var _background:DisplayObject;
      
      private var _foreground:DisplayObject;
      
      private var _backLivingLayer:Sprite;
      
      private var _foreLivingLayer:Sprite;
      
      private var _debug:Boolean = false;
      
      private var _cameraRect:Rectangle;
      
      private var _x:Number = 0;
      
      private var _y:Number = 0;
      
      private var _gameLivings:Dictionary;
      
      private var _left:int;
      
      private var _right:int;
      
      private var _top:int;
      
      private var _bottom:int;
      
      private var shouldRender:Boolean = true;
      
      private var selfGameLiving:GameLittleSelf;
      
      private var selfPos:Point;
      
      private var otherPos:Point;
      
      private var CONST_DISTANCE:Number;
      
      private var dt:Number = 0;
      
      public function GameScene(param1:Scenario)
      {
         this.selfPos = new Point();
         this.otherPos = new Point();
         super();
         this._game = param1;
         this._cameraRect = new Rectangle(300,250,300,150);
         this._left = StageReferance.stageWidth - this._game.grid.width;
         this._right = 0;
         this._top = StageReferance.stageHeight - this._game.grid.height;
         this._bottom = 0;
         this._gameLivings = new Dictionary();
         this.CONST_DISTANCE = Point.distance(new Point(),new Point(StageReferance.stageHeight,StageReferance.stageHeight));
         this.configUI();
         this.addEvent();
         this._backLivingLayer.mouseEnabled = false;
         this._foreLivingLayer.mouseEnabled = false;
      }
      
      private function configUI() : void
      {
         this.drawScene();
         this._backLivingLayer = new Sprite();
         addChildAt(this._backLivingLayer,getChildIndex(this._foreground));
         this._foreLivingLayer = new Sprite();
         addChild(this._foreLivingLayer);
         this.drawLivings();
         if(this._debug)
         {
            this.drawGrid();
         }
      }
      
      private function drawLivings() : void
      {
         var _loc3_:LittleLiving = null;
         var _loc1_:Dictionary = this._game.livings;
         var _loc2_:int = 1;
         for each(_loc3_ in _loc1_)
         {
            if(_loc3_.isSelf || !_loc3_.isPlayer)
            {
               this.drawLiving(_loc3_);
            }
            else
            {
               setTimeout(this.drawLiving,150 * _loc2_++,_loc3_);
            }
         }
      }
      
      private function drawLiving(param1:LittleLiving) : GameLittleLiving
      {
         var _loc2_:GameLittleLiving = null;
         if(this._game == null || this._game.livings == null || this._game.livings[param1.id] != param1)
         {
            return null;
         }
         if(param1.isSelf)
         {
            _loc2_ = new GameLittleSelf(param1 as LittleSelf);
            this.selfGameLiving = _loc2_ as GameLittleSelf;
            param1.addEventListener(LittleLivingEvent.PosChenged,this.__selfPosChanged);
            this.focusSelf(param1);
         }
         else if(param1.isPlayer)
         {
            _loc2_ = new GameLittlePlayer(param1 as LittlePlayer);
         }
         else
         {
            _loc2_ = new GameLittleLiving(param1);
         }
         this._backLivingLayer.addChild(_loc2_);
         if(this._gameLivings[param1.id])
         {
         }
         this._gameLivings[param1.id] = _loc2_;
         this.sortLiving(_loc2_);
         return _loc2_;
      }
      
      private function onLivingClicked(param1:MouseEvent) : void
      {
         param1.stopPropagation();
         var _loc2_:LittleSelf = this._game.selfPlayer;
         var _loc3_:GameLittleLiving = param1.currentTarget as GameLittleLiving;
         var _loc4_:int = _loc3_.living.pos.x * _loc3_.living.speed / this._game.grid.cellSize;
         var _loc5_:int = _loc3_.living.pos.y * _loc3_.living.speed / this._game.grid.cellSize;
         var _loc6_:Array = LittleGameManager.Instance.fillPath(_loc2_,this._game.grid,_loc2_.pos.x,_loc2_.pos.y,_loc4_,_loc5_);
         if(_loc6_)
         {
         }
      }
      
      private function drawGrid() : void
      {
         var _loc6_:int = 0;
         var _loc7_:int = 0;
         var _loc1_:Grid = this._game.grid;
         var _loc2_:BitmapData = new BitmapData(_loc1_.width,_loc1_.height,true,0);
         var _loc3_:Array = _loc1_.nodes;
         var _loc4_:int = _loc3_.length;
         var _loc5_:int = 0;
         while(_loc5_ < _loc4_)
         {
            _loc6_ = _loc3_[_loc5_].length;
            _loc7_ = 0;
            while(_loc7_ < _loc6_)
            {
               if(!_loc3_[_loc5_][_loc7_].walkable)
               {
                  _loc2_.fillRect(new Rectangle(_loc7_ * _loc1_.cellSize,_loc5_ * _loc1_.cellSize,_loc1_.cellSize,_loc1_.cellSize),2583625728);
               }
               _loc7_++;
            }
            _loc5_++;
         }
         addChild(new Bitmap(_loc2_));
      }
      
      private function drawScene() : void
      {
         this._background = ClassUtils.CreatInstance("asset.littleGame.back" + this._game.id);
         addChild(this._background);
         this._foreground = ClassUtils.CreatInstance("asset.littleGame.fore" + this._game.id);
         if(this._foreground is DisplayObjectContainer)
         {
            DisplayObjectContainer(this._foreground).mouseChildren = DisplayObjectContainer(this._foreground).mouseEnabled = false;
         }
         addChild(this._foreground);
      }
      
      private function addEvent() : void
      {
         this._game.addEventListener(LittleGameEvent.AddLiving,this.__addLiving);
         this._game.addEventListener(LittleGameEvent.Update,this.__update);
         this._game.addEventListener(LittleGameEvent.RemoveLiving,this.__removeLiving);
         addEventListener(MouseEvent.CLICK,this.__click);
      }
      
      private function __mouseDown(param1:MouseEvent) : void
      {
         startDrag();
         StageReferance.stage.addEventListener(MouseEvent.MOUSE_UP,this.__mouseUp);
      }
      
      private function __mouseUp(param1:MouseEvent) : void
      {
         StageReferance.stage.removeEventListener(MouseEvent.MOUSE_UP,this.__mouseUp);
         stopDrag();
      }
      
      private function __removeLiving(param1:LittleGameEvent) : void
      {
         var _loc2_:LittleLiving = param1.paras[0] as LittleLiving;
         var _loc3_:GameLittleLiving = this._gameLivings[_loc2_.id];
         if(_loc3_)
         {
            _loc3_.removeEventListener(MouseEvent.CLICK,this.onLivingClicked);
            ObjectUtils.disposeObject(_loc3_);
            delete this._gameLivings[_loc2_.id];
         }
      }
      
      private function __update(param1:LittleGameEvent) : void
      {
         this.updateLivingVisible();
         this.sortDepth();
      }
      
      private function sortDepth() : void
      {
         var _loc4_:DisplayObject = null;
         var _loc1_:Array = new Array();
         var _loc2_:int = 0;
         while(_loc2_ < this._backLivingLayer.numChildren)
         {
            _loc4_ = this._backLivingLayer.getChildAt(_loc2_);
            if(_loc4_ is GameLittleLiving && !GameLittleLiving(_loc4_).lock)
            {
               _loc1_.push(_loc4_);
            }
            else
            {
               _loc1_.push(_loc4_);
            }
            _loc2_++;
         }
         _loc1_.sortOn(["y"],[Array.NUMERIC]);
         var _loc3_:int = 0;
         while(_loc3_ < _loc1_.length)
         {
            this._backLivingLayer.setChildIndex(_loc1_[_loc3_],_loc3_);
            _loc3_++;
         }
      }
      
      private function updateLivingVisible() : void
      {
         var _loc1_:GameLittleLiving = null;
         if(this.selfGameLiving)
         {
            this.selfPos.x = this.selfGameLiving.x;
            this.selfPos.y = this.selfGameLiving.y;
            for each(_loc1_ in this._gameLivings)
            {
               this.otherPos.x = _loc1_.x;
               this.otherPos.y = _loc1_.y;
               this.shouldRender = Point.distance(this.selfPos,this.otherPos) < this.CONST_DISTANCE;
               if(this.shouldRender || _loc1_.lock)
               {
                  _loc1_.realRender = true;
                  if(!_loc1_.lock)
                  {
                     this._backLivingLayer.addChild(_loc1_);
                  }
               }
               else
               {
                  _loc1_.realRender = false;
                  if(this._backLivingLayer.contains(_loc1_))
                  {
                     this._backLivingLayer.removeChild(_loc1_);
                  }
               }
            }
         }
      }
      
      private function sortLiving(param1:GameLittleLiving) : void
      {
         var _loc4_:Rectangle = null;
         var _loc2_:Rectangle = param1.getBounds(this);
         var _loc3_:Vector.<Rectangle> = this._game.stones;
         for each(_loc4_ in _loc3_)
         {
            if(_loc4_.intersects(_loc2_))
            {
               if(_loc2_.bottom <= _loc4_.bottom)
               {
                  this._backLivingLayer.addChild(param1);
               }
               else
               {
                  this._foreLivingLayer.addChild(param1);
               }
            }
         }
      }
      
      private function __selfPosChanged(param1:LittleLivingEvent) : void
      {
         var _loc2_:LittleLiving = param1.currentTarget as LittleLiving;
         var _loc3_:Point = new Point(param1.paras[0].x * _loc2_.speed,param1.paras[0].y * _loc2_.speed);
         var _loc4_:Point = new Point(_loc2_.pos.x * _loc2_.speed,_loc2_.pos.y * _loc2_.speed);
         var _loc5_:Point = localToGlobal(_loc4_);
         if(_loc4_.y > _loc3_.y && _loc5_.y > this._cameraRect.bottom)
         {
            this.y = y + (_loc3_.y - _loc4_.y);
         }
         else if(_loc4_.y < _loc3_.y && _loc5_.y < this._cameraRect.top)
         {
            this.y = y + (_loc3_.y - _loc4_.y);
         }
         if(_loc4_.x > _loc3_.x && _loc5_.x > this._cameraRect.right)
         {
            this.x = x + (_loc3_.x - _loc4_.x);
         }
         else if(_loc4_.x < _loc3_.x && _loc5_.x < this._cameraRect.left)
         {
            this.x = x + (_loc3_.x - _loc4_.x);
         }
      }
      
      public function drawServPath(param1:LittleLiving) : void
      {
         var _loc2_:Array = param1.servPath;
         var _loc3_:Graphics = this._foreLivingLayer.graphics;
         _loc3_.clear();
         var _loc4_:Node = _loc2_[0];
         _loc3_.lineStyle(2,255);
         _loc3_.moveTo(_loc4_.x * this._game.grid.cellSize,_loc4_.y * this._game.grid.cellSize);
         var _loc5_:int = _loc2_.length;
         ChatManager.Instance.sysChatYellow("drawServPath:" + _loc5_);
         var _loc6_:int = 1;
         while(_loc6_ < _loc5_)
         {
            _loc3_.lineTo(_loc2_[_loc6_].x * this._game.grid.cellSize,_loc2_[_loc6_].y * this._game.grid.cellSize);
            _loc6_++;
         }
         _loc3_.endFill();
      }
      
      override public function set x(param1:Number) : void
      {
         if(param1 >= this._left && param1 <= this._right)
         {
            super.x = param1;
         }
      }
      
      override public function set y(param1:Number) : void
      {
         if(param1 >= this._top && param1 <= this._bottom)
         {
            super.y = param1;
         }
      }
      
      private function __click(param1:MouseEvent) : void
      {
         var _loc6_:MovieClipWrapper = null;
         var _loc2_:LittleSelf = this._game.selfPlayer;
         var _loc3_:int = mouseX / this._game.grid.cellSize;
         var _loc4_:int = mouseY / this._game.grid.cellSize;
         var _loc5_:Array = LittleGameManager.Instance.fillPath(_loc2_,this._game.grid,_loc2_.pos.x,_loc2_.pos.y,_loc3_,_loc4_);
         if(_loc5_)
         {
            LittleGameManager.Instance.selfMoveTo(this._game,_loc2_,_loc2_.pos.x,_loc2_.pos.y,_loc3_,_loc4_,this._game.clock.time,_loc5_);
            _loc6_ = new MovieClipWrapper(ClassUtils.CreatInstance("asset.hotSpring.MouseClickMovie"),true,true);
            _loc6_.movie.mouseChildren = _loc6_.movie.mouseEnabled = false;
            _loc6_.x = mouseX;
            _loc6_.y = mouseY;
            addChild(_loc6_.movie);
         }
      }
      
      public function findGameLiving(param1:int) : GameLittleLiving
      {
         return this._gameLivings[param1];
      }
      
      public function __addLiving(param1:LittleGameEvent) : void
      {
         var _loc2_:GameLittleLiving = this.drawLiving(param1.paras[0]);
         if(!_loc2_.living.isPlayer)
         {
            _loc2_.living.act(new LittleLivingBornAction(_loc2_.living));
         }
      }
      
      private function focusSelf(param1:LittleLiving) : void
      {
         var _loc2_:Point = localToGlobal(new Point(param1.pos.x * param1.speed,param1.pos.y * param1.speed));
         this.x = (StageReferance.stageWidth >> 1) - _loc2_.x;
         this.y = (StageReferance.stageWidth >> 1) - _loc2_.y;
      }
      
      private function removeEvent() : void
      {
         var _loc1_:GameLittleLiving = null;
         for each(_loc1_ in this._gameLivings)
         {
            _loc1_.removeEventListener(MouseEvent.CLICK,this.onLivingClicked);
         }
         this._game.removeEventListener(LittleGameEvent.AddLiving,this.__addLiving);
         this._game.removeEventListener(LittleGameEvent.Update,this.__update);
         this._game.removeEventListener(LittleGameEvent.RemoveLiving,this.__removeLiving);
         removeEventListener(MouseEvent.CLICK,this.__click);
      }
      
      public function addToLayer(param1:DisplayObject, param2:int) : void
      {
         var _loc3_:DisplayObjectContainer = this.getLayer(param2);
         if(_loc3_)
         {
            _loc3_.addChild(param1);
         }
      }
      
      public function getLayer(param1:int) : DisplayObjectContainer
      {
         switch(param1)
         {
            case LittleGameManager.GameBackLayer:
               return this._backLivingLayer;
            case LittleGameManager.GameForeLayer:
               return this._foreLivingLayer;
            default:
               return null;
         }
      }
      
      public function dispose() : void
      {
         var _loc1_:* = null;
         var _loc2_:GameLittleLiving = null;
         this.removeEvent();
         if(parent)
         {
            parent.removeChild(this);
         }
         for(_loc1_ in this._gameLivings)
         {
            _loc2_ = this._gameLivings[_loc1_];
            ObjectUtils.disposeObject(_loc2_);
            delete this._gameLivings[_loc1_];
         }
         this._game = null;
         ObjectUtils.disposeObject(this._background);
         ObjectUtils.disposeObject(this._foreground);
         ObjectUtils.disposeObject(this._backLivingLayer);
         ObjectUtils.disposeObject(this._foreLivingLayer);
         this.selfGameLiving = null;
         this._background = null;
         this._foreground = null;
         this._backLivingLayer = null;
         this._foreLivingLayer = null;
      }
   }
}
