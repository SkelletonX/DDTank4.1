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
   
   public class PriceShape extends Bitmap implements Disposeable
   {
       
      
      public function PriceShape(param1:int)
      {
         super(null,"auto",true);
         this.drawPrice(param1);
      }
      
      private function drawPrice(param1:int) : void
      {
         var _loc2_:BitmapData = LittleGameManager.Instance.Current.priceBack;
         var _loc3_:int = _loc2_.width;
         var _loc4_:BitmapData = LittleGameManager.Instance.Current.priceNum;
         var _loc5_:String = param1.toString();
         var _loc6_:int = _loc4_.width / 10;
         var _loc7_:int = _loc4_.height;
         var _loc8_:BitmapData = new BitmapData(_loc3_ + _loc5_.length * _loc6_,_loc2_.height,true,0);
         _loc8_.copyPixels(_loc2_,_loc2_.rect,new Point(0,0));
         var _loc9_:Rectangle = new Rectangle(0,0,_loc6_,_loc7_);
         var _loc10_:int = _loc5_.length;
         var _loc11_:int = 0;
         while(_loc11_ < _loc10_)
         {
            _loc9_.x = int(_loc5_.substr(_loc11_,1)) * _loc6_;
            _loc8_.copyPixels(_loc4_,_loc9_,new Point(_loc3_ + _loc6_ * _loc11_,0));
            _loc11_++;
         }
         if(bitmapData)
         {
            bitmapData.dispose();
         }
         bitmapData = _loc8_;
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
