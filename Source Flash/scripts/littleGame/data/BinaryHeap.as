package littleGame.data
{
   public class BinaryHeap
   {
       
      
      public var a:Array;
      
      private var _justMinFunc:Function;
      
      public function BinaryHeap(param1:Function)
      {
         this.a = new Array();
         super();
         this.a.push(-1);
         this._justMinFunc = param1;
      }
      
      public function ins(param1:Node) : void
      {
         var _loc4_:Object = null;
         var _loc2_:int = this.a.length;
         this.a[_loc2_] = param1;
         var _loc3_:int = _loc2_ >> 1;
         while(_loc2_ > 1 && this._justMinFunc(this.a[_loc2_],this.a[_loc3_]))
         {
            _loc4_ = this.a[_loc2_];
            this.a[_loc2_] = this.a[_loc3_];
            this.a[_loc3_] = _loc4_;
            _loc2_ = _loc3_;
            _loc3_ = _loc2_ >> 1;
         }
      }
      
      public function pop() : Node
      {
         var _loc6_:int = 0;
         var _loc7_:Object = null;
         var _loc1_:Object = this.a[1];
         this.a[1] = this.a[this.a.length - 1];
         this.a.pop();
         var _loc2_:int = 1;
         var _loc3_:int = this.a.length;
         var _loc4_:int = _loc2_ << 1;
         var _loc5_:int = _loc4_ + 1;
         while(_loc4_ < _loc3_)
         {
            if(_loc5_ < _loc3_)
            {
               _loc6_ = Boolean(this._justMinFunc(this.a[_loc5_],this.a[_loc4_]))?int(_loc5_):int(_loc4_);
            }
            else
            {
               _loc6_ = _loc4_;
            }
            if(this._justMinFunc(this.a[_loc6_],this.a[_loc2_]))
            {
               _loc7_ = this.a[_loc2_];
               this.a[_loc2_] = this.a[_loc6_];
               this.a[_loc6_] = _loc7_;
               _loc2_ = _loc6_;
               _loc4_ = _loc2_ << 1;
               _loc5_ = _loc4_ + 1;
               continue;
            }
            break;
         }
         return _loc1_ as Node;
      }
   }
}
