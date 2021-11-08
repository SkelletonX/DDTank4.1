package cmodule.decry
{
   function AS3_New(param1:*, param2:Array) : *
   {
      switch(param2.length)
      {
         case 0:
            return new param1();
         case 1:
            return new param1(param2[0]);
         case 2:
            return new param1(param2[0],param2[1]);
         case 3:
            return new param1(param2[0],param2[1],param2[2]);
         case 4:
            return new param1(param2[0],param2[1],param2[2],param2[3]);
         case 5:
            return new param1(param2[0],param2[1],param2[2],param2[3],param2[4]);
         default:
            log(1,"New with too many params! (" + param2.length + ")");
            return undefined;
      }
   }
}
