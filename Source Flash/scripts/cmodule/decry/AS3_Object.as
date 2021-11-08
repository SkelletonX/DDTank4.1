package cmodule.decry
{
   function AS3_Object(param1:String, param2:int) : *
   {
      var _loc6_:String = null;
      var _loc7_:CTypemap = null;
      var _loc8_:int = 0;
      var _loc9_:Array = null;
      var _loc3_:Object = {};
      if(!param1 || !param1.length)
      {
         return _loc3_;
      }
      var _loc4_:Array = param1.split(/\s*[,\:]\s*/);
      var _loc5_:int = 0;
      while(_loc5_ < _loc4_.length)
      {
         _loc6_ = _loc4_[_loc5_];
         _loc7_ = CTypemap.getTypeByName(_loc4_[_loc5_ + 1]);
         _loc8_ = _loc7_.typeSize;
         _loc9_ = [];
         mstate.ds.position = param2;
         param2 = param2 + _loc8_;
         while(_loc8_)
         {
            _loc9_.push(mstate.ds.readInt());
            _loc8_ = _loc8_ - 4;
         }
         _loc3_[_loc6_] = _loc7_.fromC(_loc9_);
         _loc5_ = _loc5_ + 2;
      }
      return _loc3_;
   }
}
