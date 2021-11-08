package cmodule.decry
{
   public class Machine extends MemUser
   {
      
      public static var dbgFrameBreakLow:int = 0;
      
      public static const dbgFileNames:Array = [];
      
      public static const dbgFuncs:Array = [];
      
      public static const dbgGlobals:Array = [];
      
      public static const dbgScopes:Array = [];
      
      public static const dbgLabels:Array = [];
      
      public static var sMS:uint;
      
      public static const dbgBreakpoints:Object = {};
      
      public static const dbgFuncNames:Array = [];
      
      public static const dbgLocs:Array = [];
      
      public static var dbgFrameBreakHigh:int = -1;
       
      
      public var dbgFileId:int = 0;
      
      public var mstate:MState;
      
      public var dbgLabel:int = 0;
      
      public var caller:Machine;
      
      public var state:int = 0;
      
      public var dbgLineNo:int = 0;
      
      public function Machine()
      {
         this.caller = Boolean(Machine)?Machine.gworker:null;
         this.mstate = Boolean(this.caller)?this.caller.mstate:null;
         super();
      }
      
      public static function debugTraverseScope(param1:Object, param2:int, param3:Function) : void
      {
         var _loc4_:Array = null;
         var _loc5_:int = 0;
         if(param1 && param2 >= param1.startLabelId && param2 < param1.endLabelId)
         {
            param3(param1);
            _loc4_ = param1.scopes;
            _loc5_ = 0;
            while(_loc5_ < _loc4_.length)
            {
               debugTraverseScope(_loc4_[_loc5_],param2,param3);
               _loc5_++;
            }
         }
      }
      
      public function debugTraceMem(param1:int, param2:int) : void
      {
         trace("");
         trace("*****");
         while(param1 <= param2)
         {
            trace("* " + param1 + " : " + this.mstate._mr32(param1));
            param1 = param1 + 4;
         }
         trace("");
      }
      
      public function get dbgFuncId() : int
      {
         return -1;
      }
      
      public function work() : void
      {
         throw new AlchemyYield();
      }
      
      public function stringFromPtr(param1:int) : String
      {
         var _loc3_:int = 0;
         var _loc2_:String = "";
         while(true)
         {
            _loc3_ = this.mstate._mru8(param1++);
            if(!_loc3_)
            {
               break;
            }
            _loc2_ = _loc2_ + String.fromCharCode(_loc3_);
         }
         return _loc2_;
      }
      
      public function get dbgLoc() : Object
      {
         return {
            "fileId":this.dbgFileId,
            "lineNo":this.dbgLineNo
         };
      }
      
      public function get dbgDepth() : int
      {
         var _loc2_:int = 0;
         var _loc1_:Machine = this;
         while(_loc1_)
         {
            _loc2_++;
            _loc1_ = _loc1_.caller;
         }
         return _loc2_;
      }
      
      public function get dbgTrace() : String
      {
         return this.dbgFuncName + "(" + (this as Object).constructor + ") - " + this.dbgFileName + " : " + this.dbgLineNo + "(" + this.state + ")";
      }
      
      public function debugTraverseCurrentScope(param1:Function) : void
      {
         debugTraverseScope(dbgScopes[this.dbgFuncId],this.dbgLabel,param1);
      }
      
      public function debugLabel(param1:int) : void
      {
         this.dbgLabel = param1;
      }
      
      public function stringToPtr(param1:int, param2:int, param3:String) : int
      {
         var _loc4_:int = param3.length;
         if(param2 >= 0 && param2 < _loc4_)
         {
            _loc4_ = param2;
         }
         var _loc5_:int = 0;
         while(_loc5_ < _loc4_)
         {
            this.mstate._mw8(param1++,param3.charCodeAt(_loc5_));
            _loc5_++;
         }
         return _loc4_;
      }
      
      public function debugBreak(param1:Object) : void
      {
         throw new AlchemyBreakpoint(param1);
      }
      
      public function debugLoc(param1:int, param2:int) : void
      {
         var _loc5_:int = 0;
         if(this.dbgFileId == param1 && this.dbgLineNo == param2)
         {
            return;
         }
         this.dbgFileId = param1;
         this.dbgLineNo = param2;
         var _loc3_:String = param1 + ":" + param2;
         var _loc4_:Object = dbgBreakpoints[_loc3_];
         if(_loc4_ && _loc4_.enabled)
         {
            if(_loc4_.temp)
            {
               delete dbgBreakpoints[_loc3_];
            }
            this.debugBreak(_loc4_);
         }
         else if(dbgFrameBreakHigh >= dbgFrameBreakLow)
         {
            _loc5_ = this.dbgDepth;
            if(_loc5_ >= dbgFrameBreakLow && _loc5_ <= dbgFrameBreakHigh)
            {
               this.debugBreak(null);
            }
         }
      }
      
      public function get dbgFileName() : String
      {
         return dbgFileNames[this.dbgFileId];
      }
      
      public function getSecsSetMS() : uint
      {
         var _loc1_:Number = new Date().time;
         Machine.sMS = _loc1_ % 1000;
         return _loc1_ / 1000;
      }
      
      public function get dbgFuncName() : String
      {
         return dbgFuncNames[this.dbgFuncId];
      }
      
      public function backtrace() : void
      {
         var framePtr:int = 0;
         var cur:Machine = this;
         trace("");
         trace("*** backtrace");
         framePtr = this.mstate.ebp;
         while(cur)
         {
            trace(cur.dbgTrace);
            cur.debugTraverseCurrentScope(function(param1:Object):void
            {
               var _loc4_:int = 0;
               var _loc5_:int = 0;
               var _loc6_:String = null;
               var _loc7_:int = 0;
               trace("{{{");
               var _loc2_:Array = param1.vars;
               var _loc3_:int = 0;
               while(_loc3_ < _loc2_.length)
               {
                  _loc4_ = _loc2_[_loc3_ + 0];
                  _loc5_ = mstate._mr32(_loc4_ + 8);
                  _loc6_ = stringFromPtr(_loc5_);
                  _loc7_ = _loc2_[_loc3_ + 1];
                  trace("--- " + _loc6_ + " (" + (_loc7_ + framePtr) + ")");
                  _loc3_ = _loc3_ + 2;
               }
            });
            framePtr = this.mstate._mr32(framePtr);
            cur = cur.caller;
         }
         trace("");
      }
   }
}
