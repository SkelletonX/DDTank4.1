package cmodule.decry
{
   public function modPreStaticInit() : void
   {
      var _loc1_:int = 0;
      if(gpreStaticInits)
      {
         _loc1_ = 0;
         while(_loc1_ < gpreStaticInits.length)
         {
            gpreStaticInits[_loc1_]();
            _loc1_++;
         }
      }
   }
}
