package littleGame.view
{
   import com.pickgliss.toplevel.StageReferance;
   import com.pickgliss.ui.core.Disposeable;
   import flash.display.Graphics;
   import flash.display.MovieClip;
   import flash.display.Sprite;
   import flash.events.Event;
   import flash.events.MouseEvent;
   import flash.text.TextField;
   import flash.text.TextFormat;
   import flash.utils.getDefinitionByName;
   import flash.utils.getTimer;
   import littleGame.model.LittleSelf;
   
   public class ClickGame extends Sprite implements Disposeable
   {
       
      
      private var _clickTextField:TextField;
      
      private var _startTime:int;
      
      private var _clickCount:int = 0;
      
      private var _self:LittleSelf;
      
      private var _asset:MovieClip;
      
      public function ClickGame()
      {
         super();
         this.configUI();
         this.addEvent();
      }
      
      private function configUI() : void
      {
         var _loc2_:TextField = null;
         var _loc1_:Graphics = graphics;
         _loc1_.beginFill(0,0);
         _loc1_.drawRect(0,0,StageReferance.stageWidth,StageReferance.stageHeight);
         _loc1_.endFill();
         _loc2_ = new TextField();
         _loc2_.defaultTextFormat = new TextFormat("Arial",20,65280,true);
         _loc2_.autoSize = "left";
         _loc2_.text = "Click Screen!!!";
         _loc2_.mouseEnabled = false;
         _loc2_.x = StageReferance.stageWidth - _loc2_.width >> 1;
         addChild(_loc2_);
         this._clickTextField = new TextField();
         this._clickTextField.defaultTextFormat = new TextFormat("Arial",20,16711680,true);
         this._clickTextField.autoSize = "left";
         this._clickTextField.mouseEnabled = false;
         addChild(this._clickTextField);
         var _loc3_:Class = getDefinitionByName("littlegame.object.normalBoguInhaled") as Class;
         this._asset = new _loc3_() as MovieClip;
         addChild(this._asset);
      }
      
      private function addEvent() : void
      {
      }
      
      private function __shutdown(param1:Event) : void
      {
         dispatchEvent(new Event(Event.COMPLETE));
         this.dispose();
      }
      
      private function __startup(param1:Event) : void
      {
         this._startTime = getTimer();
         addEventListener(MouseEvent.CLICK,this.__clicked);
      }
      
      private function removeEvent() : void
      {
         removeEventListener(MouseEvent.CLICK,this.__clicked);
      }
      
      private function __clicked(param1:MouseEvent) : void
      {
         var _loc2_:int = getTimer();
         this._clickCount++;
         this._clickTextField.text = "click:" + this._clickCount;
         this._clickTextField.x = StageReferance.stageWidth - this._clickTextField.width >> 1;
         this._clickTextField.y = StageReferance.stageHeight - this._clickTextField.height >> 1;
         if(this._asset["water"].totalFrames >= this._asset["water"].currentFrame + 2)
         {
            this._asset["water"].gotoAndStop(this._asset["water"].currentFrame + 4);
         }
         else
         {
            this._asset["water"].gotoAndStop(this._asset["water"].totalFrames);
         }
         this._asset.play();
         this.__shutdown(null);
      }
      
      public function dispose() : void
      {
         this.removeEvent();
         if(parent)
         {
            parent.removeChild(this);
         }
      }
   }
}
