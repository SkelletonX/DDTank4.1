package cmodule.decry
{
   function AS3_ArrayValue(param1:Array, param2:String, param3:int) : void
   {
      var _loc6_:CTypemap = null;
      var _loc7_:int = 0;
      var _loc8_:Array = null;
      var _loc9_:int = 0;
      if(!param2 || !param2.length)
      {
         return;
      }
      var _loc4_:Array = param2.split(/\s*,\s*/);
      var _loc5_:int = 0;
      while(_loc5_ < _loc4_.length && _loc5_ < param1.length)
      {
         _loc6_ = CTypemap.getTypeByName(_loc4_[_loc5_]);
         mstate.ds.position = param3;
         _loc7_ = mstate.ds.readInt();
         param3 = param3 + 4;
         _loc8_ = _loc6_.createC(param1[_loc5_]);
         mstate.ds.position = _loc7_;
         _loc9_ = 0;
         while(_loc9_ < _loc8_.length)
         {
            mstate.ds.writeInt(_loc8_[_loc9_]);
            _loc9_++;
         }
         _loc5_++;
      }
   }
}
