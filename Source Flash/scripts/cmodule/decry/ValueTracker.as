package cmodule.decry
{
   import flash.utils.Dictionary;
   
   class ValueTracker
   {
       
      
      private var snum:int = 1;
      
      private var val2rcv:Dictionary;
      
      private var id2key:Object;
      
      function ValueTracker()
      {
         this.val2rcv = new Dictionary();
         this.id2key = {};
         super();
      }
      
      public function acquireId(param1:int) : int
      {
         var _loc2_:Object = null;
         if(param1)
         {
            _loc2_ = this.id2key[param1];
            this.val2rcv[_loc2_].rc++;
         }
         return param1;
      }
      
      public function get(param1:int) : *
      {
         var _loc2_:Object = null;
         var _loc3_:RCValue = null;
         if(param1)
         {
            _loc2_ = this.id2key[param1];
            _loc3_ = this.val2rcv[_loc2_];
            return _loc3_.value;
         }
         return undefined;
      }
      
      public function release(param1:int) : *
      {
         var _loc2_:Object = null;
         var _loc3_:RCValue = null;
         if(param1)
         {
            _loc2_ = this.id2key[param1];
            _loc3_ = this.val2rcv[_loc2_];
            if(_loc3_)
            {
               if(!--_loc3_.rc)
               {
                  delete this.id2key[param1];
                  delete this.val2rcv[_loc2_];
               }
               return _loc3_.value;
            }
            log(1,"ValueTracker extra release!: " + param1);
         }
         return undefined;
      }
      
      public function acquire(param1:*) : int
      {
         var _loc4_:int = 0;
         if(typeof param1 == "undefined")
         {
            return 0;
         }
         var _loc2_:Object = Object(param1);
         if(_loc2_ instanceof QName)
         {
            _loc2_ = "*VT*QName*/" + _loc2_.toString();
         }
         var _loc3_:* = this.val2rcv[_loc2_];
         if(typeof _loc3_ == "undefined")
         {
            while(!this.snum || typeof this.id2key[this.snum] != "undefined")
            {
               this.snum++;
            }
            _loc4_ = this.snum;
            this.val2rcv[_loc2_] = new RCValue(param1,_loc4_);
            this.id2key[_loc4_] = _loc2_;
         }
         else
         {
            _loc4_ = _loc3_.id;
            this.val2rcv[_loc2_].rc++;
         }
         return _loc4_;
      }
   }
}
