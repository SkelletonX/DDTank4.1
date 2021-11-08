package cmodule.decry
{
   import flash.display.Stage;
   import flash.events.MouseEvent;
   
   function vgl_mouse_buttons() : int
   {
      var stage:Stage = null;
      if(vglMouseFirst)
      {
         stage = gsprite.stage;
         stage.addEventListener(MouseEvent.MOUSE_DOWN,function(param1:MouseEvent):*
         {
            var vglMouseButtons:* = 1;
         });
         stage.addEventListener(MouseEvent.MOUSE_UP,function(param1:MouseEvent):*
         {
            var vglMouseButtons:* = 0;
         });
         var vglMouseFirst:* = false;
      }
      return vglMouseButtons;
   }
}
