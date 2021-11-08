package cmodule.decry
{
   import flash.text.TextField;
   import flash.text.TextFormat;
   
   class TextFieldO extends IO
   {
       
      
      private var m_trace:Boolean;
      
      private var m_tf:TextField;
      
      function TextFieldO(param1:TextField, param2:Boolean = false)
      {
         super();
         this.m_tf = param1;
         this.m_trace = param2;
      }
      
      override public function write(param1:int, param2:int) : int
      {
         var _loc3_:int = param2;
         var _loc4_:String = "";
         while(_loc3_--)
         {
            _loc4_ = _loc4_ + String.fromCharCode(TextFieldO._mru8(param1));
            param1++;
         }
         if(this.m_trace)
         {
            trace(_loc4_);
         }
         var _loc5_:int = this.m_tf.length;
         this.m_tf.replaceText(_loc5_,_loc5_,_loc4_);
         var _loc6_:int = this.m_tf.length;
         var _loc7_:TextFormat = this.m_tf.getTextFormat(_loc5_,_loc6_);
         _loc7_.bold = true;
         this.m_tf.setTextFormat(_loc7_,_loc5_,_loc6_);
         this.m_tf.setSelection(_loc6_,_loc6_);
         return param2;
      }
   }
}
