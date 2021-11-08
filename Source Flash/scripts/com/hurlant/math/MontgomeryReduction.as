package com.hurlant.math
{
   use namespace bi_internal;
   
   class MontgomeryReduction implements IReduction
   {
       
      
      private var um:int;
      
      private var mp:int;
      
      private var mph:int;
      
      private var mpl:int;
      
      private var mt2:int;
      
      private var m:BigInteger;
      
      function MontgomeryReduction(m:BigInteger)
      {
         super();
         this.m = m;
         mp = m.invDigit();
         mpl = mp & 32767;
         mph = mp >> 15;
         um = (1 << BigInteger.DB - 15) - 1;
         mt2 = 2 * m.t;
      }
      
      public function mulTo(x:BigInteger, y:BigInteger, r:BigInteger) : void
      {
         x.multiplyTo(y,r);
         reduce(r);
      }
      
      public function revert(x:BigInteger) : BigInteger
      {
         var r:BigInteger = null;
         r = new BigInteger();
         x.copyTo(r);
         reduce(r);
         return r;
      }
      
      public function convert(x:BigInteger) : BigInteger
      {
         var r:BigInteger = null;
         r = new BigInteger();
         x.abs().dlShiftTo(m.t,r);
         r.divRemTo(m,null,r);
         if(x.s < 0 && r.compareTo(BigInteger.ZERO) > 0)
         {
            m.subTo(r,r);
         }
         return r;
      }
      
      public function reduce(x:BigInteger) : void
      {
         var i:int = 0;
         var j:int = 0;
         var u0:int = 0;
         while(x.t <= mt2)
         {
            x.a[x.t++] = 0;
         }
         for(i = 0; i < m.t; i++)
         {
            j = x.a[i] & 32767;
            u0 = j * mpl + ((j * mph + (x.a[i] >> 15) * mpl & um) << 15) & BigInteger.DM;
            j = i + m.t;
            x.a[j] = x.a[j] + m.am(0,u0,x,i,0,m.t);
            while(x.a[j] >= BigInteger.DV)
            {
               x.a[j] = x.a[j] - BigInteger.DV;
               x.a[++j]++;
            }
         }
         x.clamp();
         x.drShiftTo(m.t,x);
         if(x.compareTo(m) >= 0)
         {
            x.subTo(m,x);
         }
      }
      
      public function sqrTo(x:BigInteger, r:BigInteger) : void
      {
         x.squareTo(r);
         reduce(r);
      }
   }
}
