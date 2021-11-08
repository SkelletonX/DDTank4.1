package littleGame.view
{
   import com.pickgliss.ui.core.Disposeable;
   import ddt.ddt_internal;
   import flash.display.Bitmap;
   import flash.display.BitmapData;
   import flash.geom.Point;
   import flash.geom.Rectangle;
   import littleGame.LittleGameManager;
   
   use namespace ddt_internal;
   
   public class MarkShape extends Bitmap implements Disposeable
   {
       
      
      public function MarkShape(param1:int)
      {
         super(null,"auto",true);
         this.setTime(param1);
      }
      
      public function setTime(param1:int) : void
      {
         if(param1 < 0)
         {
            return;
         }
         var _loc2_:BitmapData = LittleGameManager.Instance.Current.markBack.clone();
         var _loc3_:String = param1.toString();
         var _loc4_:BitmapData = LittleGameManager.Instance.Current.bigNum;
         var _loc5_:int = _loc4_.width / 11;
         var _loc6_:int = _loc4_.height;
         var _loc7_:int = param1 < 10?int(145):int(118);
         var _loc8_:Rectangle = new Rectangle(_loc4_.width - _loc5_,0,_loc5_,_loc6_);
         var _loc9_:int = _loc3_.length;
         var _loc10_:int = 0;
         while(_loc10_ < _loc9_)
         {
            _loc8_.x = int(_loc3_.substr(_loc10_,1)) * _loc5_;
            _loc2_.copyPixels(_loc4_,_loc8_,new Point(_loc7_ + _loc5_ * _loc10_,0));
            _loc10_++;
         }
         if(bitmapData)
         {
            bitmapData.dispose();
         }
         bitmapData = _loc2_;
      }
      
      public function dispose() : void
      {
         if(bitmapData)
         {
            bitmapData.dispose();
         }
         if(parent)
         {
            parent.removeChild(this);
         }
      }
   }
}
