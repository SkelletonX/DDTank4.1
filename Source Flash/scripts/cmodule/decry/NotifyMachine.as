package cmodule.decry
{
   class NotifyMachine extends Machine
   {
       
      
      private var proc:Function;
      
      function NotifyMachine(param1:Function)
      {
         super();
         this.proc = param1;
         mstate.push(0);
         mstate.push(mstate.ebp);
         mstate.ebp = mstate.esp;
      }
      
      override public function work() : void
      {
         var noClean:Boolean = false;
         try
         {
            noClean = Boolean(this.proc())?Boolean(true):Boolean(false);
         }
         catch(e:*)
         {
            log(1,"NotifyMachine: " + e);
         }
         if(!noClean)
         {
            mstate.gworker = caller;
            mstate.ebp = mstate.pop();
            mstate.pop();
         }
      }
   }
}
