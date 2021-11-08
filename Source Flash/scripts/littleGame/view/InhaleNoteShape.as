package littleGame.view
{
   import com.pickgliss.ui.core.Disposeable;
   import ddt.ddt_internal;
   import flash.display.Bitmap;
   import flash.display.BitmapData;
   import flash.geom.Point;
   import flash.geom.Rectangle;
   import littleGame.LittleGameManager;
   
   public class InhaleNoteShape extends Bitmap implements Disposeable
   {
       
      
      public function InhaleNoteShape(param1:int, param2:int)
      {
         super(null,"auto",true);
         this.setNote(param1,param2);
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
      
      public function setNote(param1:int, param2:int) : void
      {
         var _loc3_:BitmapData = LittleGameManager.Instance.Current.ddt_internal::inhaleNeed.clone();
         var _loc4_:BitmapData = LittleGameManager.Instance.Current.ddt_internal::bigNum;
         var _loc5_:int = _loc4_.width / 11;
         var _loc6_:Rectangle = new Rectangle(0,0,_loc5_,_loc4_.height);
         _loc6_.x = param2 * _loc5_;
         _loc3_.copyPixels(_loc4_,_loc6_,new Point(240,0));
         if(bitmapData)
         {
            bitmapData.dispose();
         }
         bitmapData = _loc3_;
      }
   }
}
