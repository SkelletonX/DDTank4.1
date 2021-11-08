package littleGame.view
{
   import character.ICharacter;
   import com.pickgliss.ui.core.Disposeable;
   import com.pickgliss.utils.ObjectUtils;
   import ddt.ddt_internal;
   import flash.display.DisplayObject;
   import flash.display.Sprite;
   import flash.events.MouseEvent;
   import flash.filters.GlowFilter;
   import flash.geom.Point;
   import littleGame.LittleGameManager;
   import littleGame.data.DirectionType;
   import littleGame.events.LittleLivingEvent;
   import littleGame.model.LittleLiving;
   
   use namespace ddt_internal;
   
   public class GameLittleLiving extends Sprite implements Disposeable
   {
       
      
      public var isMove:Boolean = false;
      
      protected var _living:LittleLiving;
      
      protected var _body:DisplayObject;
      
      protected var _hitArea:Sprite;
      
      protected var _realRender:Boolean;
      
      public function GameLittleLiving(param1:LittleLiving)
      {
         this._living = param1;
         super();
         buttonMode = true;
         this.configUI();
         this.addEvent();
         mouseEnabled = false;
      }
      
      public function get realRender() : Boolean
      {
         return this._realRender;
      }
      
      public function set realRender(param1:Boolean) : void
      {
         if(this._realRender == param1)
         {
            return;
         }
         this._realRender = param1;
         ICharacter(this._body).realRender = param1;
      }
      
      public function setInhaled(param1:Boolean) : void
      {
      }
      
      public function get lock() : Boolean
      {
         return this._living && this._living.lock;
      }
      
      public function set lock(param1:Boolean) : void
      {
         if(this._living)
         {
            this._living.lock = param1;
         }
      }
      
      public function get inGame() : Boolean
      {
         return this._living && this._living.inGame;
      }
      
      override public function toString() : String
      {
         return this._living.toString();
      }
      
      protected function configUI() : void
      {
         x = this._living.pos.x * this._living.speed;
         y = this._living.pos.y * this._living.speed;
         this.createBody();
      }
      
      protected function createBody() : void
      {
         var _loc1_:ICharacter = null;
         _loc1_ = CharacterFactory.Instance.creatChacrater(this._living._modelID);
         _loc1_.soundEnabled = false;
         this._body = addChild(_loc1_ as DisplayObject);
         this._body.x = -_loc1_.registerPoint.x;
         this._body.y = -_loc1_.registerPoint.y;
         this._hitArea = new Sprite();
         this._hitArea.buttonMode = true;
         addChild(this._hitArea);
         this._hitArea.graphics.clear();
         this._hitArea.graphics.beginFill(11141120,0);
         this._hitArea.graphics.drawRect(_loc1_.rect.x,_loc1_.rect.y,_loc1_.rect.width,_loc1_.rect.height);
         this._hitArea.graphics.endFill();
         this.__directionChanged(null);
         this._living.bornLife = _loc1_.getActionFrames("born");
         this._living.dieLife = _loc1_.getActionFrames("die");
         if(this._living.currentAction)
         {
            _loc1_.doAction(this._living.currentAction);
         }
         else
         {
            _loc1_.doAction("stand");
         }
      }
      
      protected function addEvent() : void
      {
         if(this._hitArea != null)
         {
            this._hitArea.addEventListener(MouseEvent.MOUSE_OVER,this.onOver);
            this._hitArea.addEventListener(MouseEvent.MOUSE_OUT,this.onOut);
            this._hitArea.addEventListener(MouseEvent.CLICK,this.__click);
         }
         this._living.addEventListener(LittleLivingEvent.PosChenged,this.__posChanged);
         this._living.addEventListener(LittleLivingEvent.DirectionChanged,this.__directionChanged);
         this._living.addEventListener(LittleLivingEvent.DoAction,this.__doAction);
      }
      
      private function __click(param1:MouseEvent) : void
      {
         if(parent)
         {
            if(Point.distance(LittleGameManager.Instance.Current.selfPlayer.pos,this._living.pos) <= 20)
            {
               LittleGameManager.Instance.livingClick(LittleGameManager.Instance.Current,this._living,parent.mouseX,parent.mouseY);
               param1.stopPropagation();
            }
         }
      }
      
      protected function onOver(param1:MouseEvent) : void
      {
         this.filters = [new GlowFilter(16711680,1,24,24,2)];
      }
      
      protected function onOut(param1:MouseEvent) : void
      {
         this.filters = [];
      }
      
      protected function __doAction(param1:LittleLivingEvent) : void
      {
         if(!this.lock && this._body)
         {
            ICharacter(this._body).doAction(this._living.currentAction);
         }
      }
      
      protected function __directionChanged(param1:LittleLivingEvent) : void
      {
         if(!this.lock && this._living && this._body)
         {
            if(this._living.direction == DirectionType.LEFT_DOWN || this._living.direction == DirectionType.RIGHT_UP)
            {
               this._body.scaleX = 1;
            }
            else
            {
               this._body.scaleX = -1;
            }
            this.centerBody();
         }
      }
      
      protected function centerBody() : void
      {
         var _loc1_:ICharacter = this._body as ICharacter;
         if(this._body && _loc1_)
         {
            this._body.x = this._body.scaleX == 1?Number(-_loc1_.registerPoint.x):Number(_loc1_.registerPoint.x);
         }
      }
      
      private function __posChanged(param1:LittleLivingEvent) : void
      {
         if(!this.lock)
         {
            x = this._living.pos.x * this._living.speed;
            y = this._living.pos.y * this._living.speed + this._living.gridIdx;
            this.isMove = true;
         }
      }
      
      public function get living() : LittleLiving
      {
         return this._living;
      }
      
      protected function removeEvent() : void
      {
         if(this._hitArea != null)
         {
            this._hitArea.removeEventListener(MouseEvent.MOUSE_OVER,this.onOver);
            this._hitArea.removeEventListener(MouseEvent.MOUSE_OUT,this.onOut);
            this._hitArea.removeEventListener(MouseEvent.CLICK,this.__click);
         }
         this._living.removeEventListener(LittleLivingEvent.PosChenged,this.__posChanged);
         this._living.removeEventListener(LittleLivingEvent.DirectionChanged,this.__directionChanged);
         this._living.removeEventListener(LittleLivingEvent.DoAction,this.__doAction);
      }
      
      public function dispose() : void
      {
         this.removeEvent();
         if(parent)
         {
            parent.removeChild(this);
         }
         this._living = null;
         ICharacter(this._body).dispose();
         this._body = null;
         ObjectUtils.disposeObject(this._hitArea);
         this._hitArea = null;
      }
   }
}
