package littleGame.data
{
   class Link
   {
       
      
      public var node:Node;
      
      public var cost:Number;
      
      function Link(param1:Node, param2:Number)
      {
         super();
         this.node = param1;
         this.cost = param2;
      }
   }
}
