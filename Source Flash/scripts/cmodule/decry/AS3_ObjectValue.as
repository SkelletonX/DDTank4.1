package cmodule.decry
{
   function AS3_ObjectValue(param1:Object, param2:String, param3:int) : void
   {
      var _loc6_:String = null;
      var _loc7_:CTypemap = null;
      var _loc8_:int = 0;
      var _loc9_:Array = null;
      var _loc10_:int = 0;
      if(!param2 || !param2.length)
      {
         return;
      }
      var _loc4_:Array = param2.split(/\s*[,\:]\s*/);
      var _loc5_:int = 0;
      while(_loc5_ < _loc4_.length)
      {
         _loc6_ = _loc4_[_loc5_];
         _loc7_ = CTypemap.getTypeByName(_loc4_[_loc5_ + 1]);
         mstate.ds.position = param3;
         _loc8_ = mstate.ds.readInt();
         param3 = param3 + 4;
         _loc9_ = _loc7_.createC(param1[_loc6_]);
         mstate.ds.position = _loc8_;
         _loc10_ = 0;
         while(_loc10_ < _loc9_.length)
         {
            mstate.ds.writeInt(_loc9_[_loc10_]);
            _loc10_++;
         }
         _loc5_ = _loc5_ + 2;
      }
   }
}
