package cmodule.decry
{
   public function vgl_keych() : int
   {
      if(vglKeys.length)
      {
         return vglKeys.shift();
      }
      return 0;
   }
}
