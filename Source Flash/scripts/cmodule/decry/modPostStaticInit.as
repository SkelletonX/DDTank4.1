package cmodule.decry
{
   public function modPostStaticInit() : void
   {
      var _loc1_:int = 0;
      if(gpostStaticInits)
      {
         _loc1_ = 0;
         while(_loc1_ < gpostStaticInits.length)
         {
            gpostStaticInits[_loc1_]();
            _loc1_++;
         }
      }
   }
}
