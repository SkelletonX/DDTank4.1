package cmodule.decry
{
   import flash.events.KeyboardEvent;
   import flash.events.TextEvent;
   import flash.text.TextField;
   import flash.text.TextFormat;
   import flash.utils.setTimeout;
   
   class TextFieldI extends IO
   {
       
      
      private var m_buf:String = "";
      
      private var m_tf:TextField;
      
      private var m_start:int = -1;
      
      private var m_closed:Boolean = false;
      
      function TextFieldI(param1:TextField)
      {
         var tf:TextField = param1;
         super();
         this.m_tf = tf;
         this.m_tf.addEventListener(KeyboardEvent.KEY_DOWN,function(param1:KeyboardEvent):*
         {
            var event:KeyboardEvent = param1;
            if(String.fromCharCode(event.charCode).toLowerCase() == "d" && event.ctrlKey)
            {
               m_closed = true;
            }
            if(String.fromCharCode(event.charCode).toLowerCase() == "t" && event.ctrlKey)
            {
               setTimeout(function():void
               {
                  m_start = -1;
                  m_tf.text = "";
               },1);
            }
         });
         this.m_tf.addEventListener(TextEvent.TEXT_INPUT,function(param1:TextEvent):*
         {
            var _loc2_:int = 0;
            var _loc5_:TextFormat = null;
            var _loc6_:String = null;
            var _loc7_:String = null;
            var _loc8_:int = 0;
            var _loc9_:int = 0;
            var _loc10_:int = 0;
            var _loc11_:Boolean = false;
            _loc2_ = m_tf.length;
            var _loc3_:int = m_tf.selectionBeginIndex;
            if(m_start < 0 || m_start > _loc3_)
            {
               m_start = _loc3_;
            }
            param1.preventDefault();
            m_tf.replaceSelectedText(param1.text);
            var _loc4_:int = m_tf.selectionEndIndex;
            _loc5_ = m_tf.getTextFormat(_loc3_,_loc4_);
            _loc5_.bold = false;
            m_tf.setTextFormat(_loc5_,_loc3_,_loc4_);
            if(param1.text.indexOf("\n") >= 0)
            {
               _loc6_ = m_tf.text;
               _loc7_ = "";
               _loc2_ = m_tf.length;
               _loc8_ = m_start;
               while(_loc8_ < _loc2_)
               {
                  _loc5_ = m_tf.getTextFormat(_loc8_,_loc8_ + 1);
                  _loc11_ = _loc5_.bold;
                  if(_loc11_ != null && !_loc11_.valueOf())
                  {
                     _loc7_ = _loc7_ + _loc6_.charAt(_loc8_);
                  }
                  _loc8_++;
               }
               _loc7_ = _loc7_.replace(/\r/g,"\n");
               _loc9_ = _loc7_.lastIndexOf("\n");
               _loc10_ = _loc2_ - (_loc7_.length - _loc9_ - 1);
               m_tf.setSelection(_loc10_,_loc10_);
               _loc7_ = _loc7_.substr(0,_loc9_ + 1);
               if(!m_closed)
               {
                  m_buf = m_buf + _loc7_;
               }
               m_start = _loc10_;
            }
         });
      }
      
      override public function read(param1:int, param2:int) : int
      {
         if(!this.m_buf)
         {
            if(this.m_closed)
            {
               return 0;
            }
            throw new AlchemyBlock();
         }
         var _loc3_:int = 0;
         while(this.m_buf && param2--)
         {
            _loc3_++;
            TextFieldI._mw8(param1++,this.m_buf.charCodeAt(0));
            this.m_buf = this.m_buf.substr(1);
         }
         return _loc3_;
      }
   }
}
