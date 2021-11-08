package com.pickgliss.ui.controls
{
   import com.pickgliss.geom.InnerRectangle;
   import com.pickgliss.ui.ComponentFactory;
   import com.pickgliss.utils.ClassUtils;
   import com.pickgliss.utils.ObjectUtils;
   import flash.geom.Rectangle;
   import flash.text.TextField;
   
   public class SelectedTextButton extends SelectedButton
   {
      
      public static const P_backgoundInnerRect:String = "backgoundInnerRect";
      
      public static const P_text:String = "text";
      
      public static const P_textField:String = "textField";
       
      
      protected var _textField:TextField;
      
      protected var _text:String = "";
      
      protected var _textStyle:String;
      
      protected var _backgoundInnerRect:InnerRectangle;
      
      protected var _backgoundInnerRectString:String;
      
      public function SelectedTextButton()
      {
         this._backgoundInnerRect = new InnerRectangle(0,0,0,0,-1);
         super();
      }
      
      public function set backgoundInnerRectString(param1:String) : void
      {
         if(this._backgoundInnerRectString == param1)
         {
            return;
         }
         this._backgoundInnerRectString = param1;
         this._backgoundInnerRect = ClassUtils.CreatInstance(ClassUtils.INNERRECTANGLE,ComponentFactory.parasArgs(this._backgoundInnerRectString));
         onPropertiesChanged(P_backgoundInnerRect);
      }
      
      override public function dispose() : void
      {
         if(this._textField)
         {
            ObjectUtils.disposeObject(this._textField);
         }
         this._textField = null;
         super.dispose();
      }
      
      public function set text(param1:String) : void
      {
         if(this._text == param1)
         {
            return;
         }
         this._text = param1;
         onPropertiesChanged(P_text);
      }
      
      public function set textField(param1:TextField) : void
      {
         if(this._textField == param1)
         {
            return;
         }
         ObjectUtils.disposeObject(this._textField);
         this._textField = param1;
         onPropertiesChanged(P_textField);
      }
      
      public function set textStyle(param1:String) : void
      {
         if(this._textStyle == param1)
         {
            return;
         }
         this._textStyle = param1;
         this.textField = ComponentFactory.Instance.creat(this._textStyle);
      }
      
      override protected function addChildren() : void
      {
         super.addChildren();
         if(this._textField)
         {
            addChild(this._textField);
         }
      }
      
      override protected function onProppertiesUpdate() : void
      {
         var _loc1_:Rectangle = null;
         super.onProppertiesUpdate();
         if(this._textField == null)
         {
            return;
         }
         this._textField.text = this._text;
         if(_autoSizeAble)
         {
            _loc1_ = this._backgoundInnerRect.getInnerRect(this._textField.width,this._textField.height);
            _width = _selectedButton.width = _unSelectedButton.width = _loc1_.width;
            _height = _unSelectedButton.height = _unSelectedButton.height = _loc1_.height;
            this._textField.x = this._backgoundInnerRect.para1;
            this._textField.y = this._backgoundInnerRect.para3;
         }
      }
   }
}
