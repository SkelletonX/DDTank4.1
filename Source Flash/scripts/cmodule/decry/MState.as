package cmodule.decry
{
   import flash.utils.ByteArray;
   
   public class MState extends MemUser
   {
       
      
      public var esp:int;
      
      public const syms:Object = MState == null?{}:MState.syms;
      
      public const ds:ByteArray = MState == null || MState.ds == null?MState.get():MState.ds;
      
      public var eax:int;
      
      public var cf:uint;
      
      public var gworker:Machine;
      
      public var st0:Number;
      
      public var ebp:int;
      
      public var funcs:Vector.<Object>;
      
      public var edx:int;
      
      public var system:CSystem;
      
      public function MState(param1:Machine)
      {
         this.system = MState == null?null:MState.system;
         this.funcs = MState == null?new Vector.<Object>(1):MState.funcs;
         super();
         if(param1)
         {
            this.gworker = param1;
            this.gworker.mstate = this;
         }
         if(MState == null)
         {
            this.ds.length = this.ds.length + MState;
            this.esp = this.ds.length;
         }
      }
      
      public function copyTo(param1:MState) : void
      {
         param1.esp = this.esp;
         param1.ebp = this.ebp;
         param1.eax = this.eax;
         param1.edx = this.edx;
         param1.st0 = this.st0;
         param1.cf = this.cf;
         param1.gworker = this.gworker;
      }
      
      public function pop() : int
      {
         var _loc1_:int = _mr32(this.esp);
         this.esp = this.esp + 4;
         return _loc1_;
      }
      
      public function push(param1:int) : void
      {
         this.esp = this.esp - 4;
         _mw32(this.esp,param1);
      }
   }
}
