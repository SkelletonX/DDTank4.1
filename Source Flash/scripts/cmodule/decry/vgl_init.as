package cmodule.decry
{
   import flash.display.Bitmap;
   import flash.display.BitmapData;
   import flash.display.Stage;
   import flash.events.KeyboardEvent;
   
   public function vgl_init(param1:int, param2:int, param3:int) : int
   {
      var stage:Stage = null;
      var vk2scan:Array = null;
      var width:int = param1;
      var height:int = param2;
      var pixels:int = param3;
      stage = gsprite.stage;
      trace("vgl_init: " + width + " / " + height + " : " + pixels);
      if(vglKeyFirst)
      {
         vk2scan = [0,0,0,70,0,0,0,0,14,15,0,0,76,28,0,0,42,29,56,0,58,0,0,0,0,0,0,1,0,0,0,0,57,73,81,79,71,75,72,77,80,0,0,0,84,82,83,99,11,2,3,4,5,6,7,8,9,10,0,0,0,0,0,0,0,30,48,46,32,18,33,34,35,23,36,37,38,50,49,24,25,16,19,31,20,22,47,17,45,21,44,91,92,93,0,95,82,79,80,81,75,76,77,71,72,73,55,78,0,74,83,53,59,60,61,62,63,64,65,66,67,68,87,88,100,101,102,103,104,105,106,107,108,109,110,118,0,0,0,0,0,0,0,0,69,70,0,0,0,0,0,0,0,0,0,0,0,0,0,0,42,54,29,29,56,56,106,105,103,104,101,102,50,32,46,48,25,16,36,34,108,109,107,33,0,0,39,13,51,12,52,53,41,115,126,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,26,43,27,40,0,0,0,86,0,0,0,0,0,0,113,92,123,0,111,90,0,0,91,0,95,0,94,0,0,0,93,0,98,0,0,0,0];
         stage.addEventListener(KeyboardEvent.KEY_DOWN,function(param1:KeyboardEvent):*
         {
            var _loc2_:int = vglKeyMode == 2?int(vk2scan[param1.keyCode & 127]):int(param1.charCode);
            vglKeys.push(_loc2_);
         });
         stage.addEventListener(KeyboardEvent.KEY_UP,function(param1:KeyboardEvent):*
         {
            var _loc2_:int = vglKeyMode == 2?int(vk2scan[param1.keyCode & 127]):int(param1.charCode);
            if(vglKeyMode == 2)
            {
               vglKeys.push(_loc2_ | 128);
            }
         });
         vglKeys.push(69);
         stage.focus = stage;
         var vglKeyFirst:* = false;
      }
      var gvglpixels:* = pixels;
      var gvglbmd:* = new BitmapData(Math.abs(width),Math.abs(height),false);
      if(!gvglbm)
      {
         var gvglbm:* = new Bitmap();
         gsprite.addChild(gvglbm);
      }
      gvglbm.bitmapData = gvglbmd;
      gvglbm.scaleX = gsprite.stage.stageWidth / width;
      gvglbm.scaleY = gsprite.stage.stageHeight / height;
      trace("vgl_init done");
      return 0;
   }
}
