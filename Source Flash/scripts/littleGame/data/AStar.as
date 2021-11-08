package littleGame.data
{
   import com.pickgliss.ui.core.Disposeable;
   import flash.geom.Point;
   import flash.utils.getTimer;
   
   public class AStar implements Disposeable
   {
       
      
      public var heuristic:Function;
      
      private var _straightCost:Number = 1;
      
      private var _diagCost:Number = 1.41421;
      
      private var nowversion:int = 0;
      
      private var TwoOneTwoZero:Number;
      
      private var _endNode:Node;
      
      private var _startNode:Node;
      
      private var _grid:Grid;
      
      private var _open:BinaryHeap;
      
      private var _path:Array;
      
      private var _floydPath:Array;
      
      public function AStar(param1:Grid)
      {
         this.TwoOneTwoZero = 2 * Math.cos(Math.PI / 3);
         super();
         this._grid = param1;
         this.heuristic = this.euclidian2;
      }
      
      public function dispose() : void
      {
         this._open = null;
         this.heuristic = null;
         this._endNode = this._startNode = null;
         this._grid = null;
         this._path = null;
      }
      
      private function justMin(param1:Node, param2:Node) : Boolean
      {
         return param1.f < param2.f;
      }
      
      public function manhattan(param1:Node) : Number
      {
         return Math.abs(param1.x - this._endNode.x) + Math.abs(param1.y - this._endNode.y);
      }
      
      public function manhattan2(param1:Node) : Number
      {
         var _loc2_:Number = Math.abs(param1.x - this._endNode.x);
         var _loc3_:Number = Math.abs(param1.y - this._endNode.y);
         return _loc2_ + _loc3_ + Math.abs(_loc2_ - _loc3_) / 1000;
      }
      
      public function euclidian(param1:Node) : Number
      {
         var _loc2_:Number = param1.x - this._endNode.x;
         var _loc3_:Number = param1.y - this._endNode.y;
         return Math.sqrt(_loc2_ * _loc2_ + _loc3_ * _loc3_);
      }
      
      public function chineseCheckersEuclidian2(param1:Node) : Number
      {
         var _loc2_:Number = param1.y / this.TwoOneTwoZero;
         var _loc3_:Number = param1.x + param1.y / 2;
         var _loc4_:Number = _loc3_ - this._endNode.x - this._endNode.y / 2;
         var _loc5_:Number = _loc2_ - this._endNode.y / this.TwoOneTwoZero;
         return this.sqrt(_loc4_ * _loc4_ + _loc5_ * _loc5_);
      }
      
      private function sqrt(param1:Number) : Number
      {
         return Math.sqrt(param1);
      }
      
      public function euclidian2(param1:Node) : Number
      {
         var _loc2_:Number = param1.x - this._endNode.x;
         var _loc3_:Number = param1.y - this._endNode.y;
         return _loc2_ * _loc2_ + _loc3_ * _loc3_;
      }
      
      public function fillPath() : Boolean
      {
         this._endNode = this._grid.endNode;
         this._startNode = this._grid.startNode;
         this.nowversion++;
         this._open = new BinaryHeap(this.justMin);
         this._startNode.g = 0;
         var _loc1_:int = getTimer();
         var _loc2_:Boolean = this.search();
         return _loc2_;
      }
      
      public function search() : Boolean
      {
         var _loc2_:int = 0;
         var _loc3_:int = 0;
         var _loc4_:Node = null;
         var _loc5_:Number = NaN;
         var _loc6_:Number = NaN;
         var _loc7_:Number = NaN;
         var _loc8_:Number = NaN;
         var _loc1_:Node = this._startNode;
         _loc1_.version = this.nowversion;
         while(_loc1_ != this._endNode)
         {
            _loc2_ = _loc1_.links.length;
            _loc3_ = 0;
            while(_loc3_ < _loc2_)
            {
               _loc4_ = _loc1_.links[_loc3_];
               if(!(_loc1_.x == _loc4_.x || _loc1_.y == _loc4_.y))
               {
                  _loc5_ = Grid.diagCost;
               }
               else
               {
                  _loc5_ = Grid.straightCost;
               }
               _loc6_ = _loc1_.g + _loc5_;
               _loc7_ = this.heuristic(_loc4_);
               _loc8_ = _loc6_ + _loc7_;
               if(_loc4_.version == this.nowversion)
               {
                  if(_loc4_.f > _loc8_)
                  {
                     _loc4_.f = _loc8_;
                     _loc4_.g = _loc6_;
                     _loc4_.h = _loc7_;
                     _loc4_.parent = _loc1_;
                  }
               }
               else
               {
                  _loc4_.f = _loc8_;
                  _loc4_.g = _loc6_;
                  _loc4_.h = _loc7_;
                  _loc4_.parent = _loc1_;
                  this._open.ins(_loc4_);
                  _loc4_.version = this.nowversion;
               }
               _loc3_++;
            }
            if(this._open.a.length == 1)
            {
               return false;
            }
            _loc1_ = this._open.pop() as Node;
         }
         this.buildPath();
         return true;
      }
      
      private function buildPath() : void
      {
         this._path = [];
         var _loc1_:Node = this._endNode;
         this._path.push(_loc1_);
         while(_loc1_ != this._startNode)
         {
            _loc1_ = _loc1_.parent;
            this._path.unshift(_loc1_);
         }
      }
      
      public function get path() : Array
      {
         return this._path;
      }
      
      public function floyd() : void
      {
         var _loc2_:Node = null;
         var _loc3_:Node = null;
         var _loc4_:int = 0;
         var _loc5_:int = 0;
         var _loc6_:int = 0;
         if(this.path == null)
         {
            return;
         }
         this._floydPath = this.path.concat();
         var _loc1_:int = this._floydPath.length;
         if(_loc1_ > 2)
         {
            _loc2_ = new Node(0,0);
            _loc3_ = new Node(0,0);
            this.floydVector(_loc2_,this._floydPath[_loc1_ - 1],this._floydPath[_loc1_ - 2]);
            _loc4_ = this._floydPath.length - 3;
            while(_loc4_ >= 0)
            {
               this.floydVector(_loc3_,this._floydPath[_loc4_ + 1],this._floydPath[_loc4_]);
               if(_loc2_.x == _loc3_.x && _loc2_.y == _loc3_.y)
               {
                  this._floydPath.splice(_loc4_ + 1,1);
               }
               else
               {
                  _loc2_.x = _loc3_.x;
                  _loc2_.y = _loc3_.y;
               }
               _loc4_--;
            }
         }
         _loc1_ = this._floydPath.length;
         _loc4_ = _loc1_ - 1;
         while(_loc4_ >= 0)
         {
            _loc5_ = 0;
            while(_loc5_ <= _loc4_ - 2)
            {
               if(this.floydCrossAble(this._floydPath[_loc4_],this._floydPath[_loc5_]))
               {
                  _loc6_ = _loc4_ - 1;
                  while(_loc6_ > _loc5_)
                  {
                     this._floydPath.splice(_loc6_,1);
                     _loc6_--;
                  }
                  _loc4_ = _loc5_;
                  _loc1_ = this._floydPath.length;
                  break;
               }
               _loc5_++;
            }
            _loc4_--;
         }
      }
      
      private function floydCrossAble(param1:Node, param2:Node) : Boolean
      {
         var _loc3_:Array = this.bresenhamNodes(new Point(param1.x,param1.y),new Point(param2.x,param2.y));
         var _loc4_:int = _loc3_.length - 2;
         while(_loc4_ > 0)
         {
            if(!this._grid.getNode(_loc3_[_loc4_].x,_loc3_[_loc4_].y).walkable)
            {
               return false;
            }
            _loc4_--;
         }
         return true;
      }
      
      private function floydVector(param1:Node, param2:Node, param3:Node) : void
      {
         param1.x = param2.x - param3.x;
         param1.y = param2.y - param3.y;
      }
      
      private function bresenhamNodes(param1:Point, param2:Point) : Array
      {
         var _loc10_:int = 0;
         var _loc11_:int = 0;
         var _loc12_:int = 0;
         var _loc3_:Boolean = Math.abs(param2.y - param1.y) > Math.abs(param2.x - param1.x);
         if(_loc3_)
         {
            _loc10_ = param1.x;
            param1.x = param1.y;
            param1.y = _loc10_;
            _loc10_ = param2.x;
            param2.x = param2.y;
            param2.y = _loc10_;
         }
         var _loc4_:int = param2.x > param1.x?int(1):param2.x < param1.x?int(-1):int(0);
         var _loc5_:int = param2.y > param1.y?int(1):param2.y < param1.y?int(-1):int(0);
         var _loc6_:Number = (param2.y - param1.y) / Math.abs(param2.x - param1.x);
         var _loc7_:Array = [];
         var _loc8_:Number = param1.x + _loc4_;
         var _loc9_:Number = param1.y + _loc6_;
         if(_loc3_)
         {
            _loc7_.push(new Point(param1.y,param1.x));
         }
         else
         {
            _loc7_.push(new Point(param1.x,param1.y));
         }
         while(_loc8_ != param2.x)
         {
            _loc11_ = Math.floor(_loc9_);
            _loc12_ = Math.ceil(_loc9_);
            if(_loc3_)
            {
               _loc7_.push(new Point(_loc11_,_loc8_));
            }
            else
            {
               _loc7_.push(new Point(_loc8_,_loc11_));
            }
            if(_loc11_ != _loc12_)
            {
               if(_loc3_)
               {
                  _loc7_.push(new Point(_loc12_,_loc8_));
               }
               else
               {
                  _loc7_.push(new Point(_loc8_,_loc12_));
               }
            }
            _loc8_ = _loc8_ + _loc4_;
            _loc9_ = _loc9_ + _loc6_;
         }
         if(_loc3_)
         {
            _loc7_.push(new Point(param2.y,param2.x));
         }
         else
         {
            _loc7_.push(new Point(param2.x,param2.y));
         }
         return _loc7_;
      }
      
      public function get floydPath() : Array
      {
         return this._floydPath;
      }
   }
}
