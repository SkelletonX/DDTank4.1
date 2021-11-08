package com.hurlant.math
{
   import com.hurlant.crypto.prng.Random;
   import com.hurlant.util.Hex;
   import com.hurlant.util.Memory;
   import flash.utils.ByteArray;
   
   use namespace bi_internal;
   
   public class BigInteger
   {
      
      public static const ONE:BigInteger = nbv(1);
      
      public static const ZERO:BigInteger = nbv(0);
      
      public static const DM:int = DV - 1;
      
      public static const F1:int = BI_FP - DB;
      
      public static const F2:int = 2 * DB - BI_FP;
      
      public static const lplim:int = (1 << 26) / lowprimes[lowprimes.length - 1];
      
      public static const lowprimes:Array = [2,3,5,7,11,13,17,19,23,29,31,37,41,43,47,53,59,61,67,71,73,79,83,89,97,101,103,107,109,113,127,131,137,139,149,151,157,163,167,173,179,181,191,193,197,199,211,223,227,229,233,239,241,251,257,263,269,271,277,281,283,293,307,311,313,317,331,337,347,349,353,359,367,373,379,383,389,397,401,409,419,421,431,433,439,443,449,457,461,463,467,479,487,491,499,503,509];
      
      public static const FV:Number = Math.pow(2,BI_FP);
      
      public static const BI_FP:int = 52;
      
      public static const DV:int = 1 << DB;
      
      public static const DB:int = 30;
       
      
      bi_internal var a:Array;
      
      bi_internal var s:int;
      
      public var t:int;
      
      public function BigInteger(value:* = null, radix:int = 0)
      {
         var array:ByteArray = null;
         var length:int = 0;
         super();
         a = new Array();
         if(value is String)
         {
            value = Hex.toArray(value);
            radix = 0;
         }
         if(value is ByteArray)
         {
            array = value as ByteArray;
            length = int(radix) || int(array.length - array.position);
            fromArray(array,length);
         }
      }
      
      public static function nbv(value:int) : BigInteger
      {
         var bn:BigInteger = null;
         bn = new BigInteger();
         bn.fromInt(value);
         return bn;
      }
      
      public function clearBit(n:int) : BigInteger
      {
         return changeBit(n,op_andnot);
      }
      
      public function negate() : BigInteger
      {
         var r:BigInteger = null;
         r = nbi();
         ZERO.subTo(this,r);
         return r;
      }
      
      public function andNot(a:BigInteger) : BigInteger
      {
         var r:BigInteger = null;
         r = new BigInteger();
         bitwiseTo(a,op_andnot,r);
         return r;
      }
      
      public function modPow(e:BigInteger, m:BigInteger) : BigInteger
      {
         var i:int = 0;
         var k:int = 0;
         var r:BigInteger = null;
         var z:IReduction = null;
         var g:Array = null;
         var n:int = 0;
         var k1:int = 0;
         var km:int = 0;
         var j:int = 0;
         var w:int = 0;
         var is1:Boolean = false;
         var r2:BigInteger = null;
         var t:BigInteger = null;
         var g2:BigInteger = null;
         i = e.bitLength();
         r = nbv(1);
         if(i <= 0)
         {
            return r;
         }
         if(i < 18)
         {
            k = 1;
         }
         else if(i < 48)
         {
            k = 3;
         }
         else if(i < 144)
         {
            k = 4;
         }
         else if(i < 768)
         {
            k = 5;
         }
         else
         {
            k = 6;
         }
         if(i < 8)
         {
            z = new ClassicReduction(m);
         }
         else if(m.isEven())
         {
            z = new BarrettReduction(m);
         }
         else
         {
            z = new MontgomeryReduction(m);
         }
         g = [];
         n = 3;
         k1 = k - 1;
         km = (1 << k) - 1;
         g[1] = z.convert(this);
         if(k > 1)
         {
            g2 = new BigInteger();
            z.sqrTo(g[1],g2);
            while(n <= km)
            {
               g[n] = new BigInteger();
               z.mulTo(g2,g[n - 2],g[n]);
               n = n + 2;
            }
         }
         j = e.t - 1;
         is1 = true;
         r2 = new BigInteger();
         i = nbits(e.a[j]) - 1;
         while(j >= 0)
         {
            if(i >= k1)
            {
               w = e.a[j] >> i - k1 & km;
            }
            else
            {
               w = (e.a[j] & (1 << i + 1) - 1) << k1 - i;
               if(j > 0)
               {
                  w = w | e.a[j - 1] >> DB + i - k1;
               }
            }
            n = k;
            while((w & 1) == 0)
            {
               w = w >> 1;
               n--;
            }
            if((i = i - n) < 0)
            {
               i = i + DB;
               j--;
            }
            if(is1)
            {
               g[w].copyTo(r);
               is1 = false;
            }
            else
            {
               while(n > 1)
               {
                  z.sqrTo(r,r2);
                  z.sqrTo(r2,r);
                  n = n - 2;
               }
               if(n > 0)
               {
                  z.sqrTo(r,r2);
               }
               else
               {
                  t = r;
                  r = r2;
                  r2 = t;
               }
               z.mulTo(r2,g[w],r);
            }
            while(j >= 0 && (e.a[j] & 1 << i) == 0)
            {
               z.sqrTo(r,r2);
               t = r;
               r = r2;
               r2 = t;
               if(--i < 0)
               {
                  i = DB - 1;
                  j--;
               }
            }
         }
         return z.revert(r);
      }
      
      public function isProbablePrime(t:int) : Boolean
      {
         var i:int = 0;
         var x:BigInteger = null;
         var m:int = 0;
         var j:int = 0;
         x = abs();
         if(x.t == 1 && x.a[0] <= lowprimes[lowprimes.length - 1])
         {
            for(i = 0; i < lowprimes.length; i++)
            {
               if(x[0] == lowprimes[i])
               {
                  return true;
               }
            }
            return false;
         }
         if(x.isEven())
         {
            return false;
         }
         i = 1;
         while(i < lowprimes.length)
         {
            m = lowprimes[i];
            j = i + 1;
            while(j < lowprimes.length && m < lplim)
            {
               m = m * lowprimes[j++];
            }
            m = x.modInt(m);
            while(i < j)
            {
               if(m % lowprimes[i++] == 0)
               {
                  return false;
               }
            }
         }
         return x.millerRabin(t);
      }
      
      private function op_or(x:int, y:int) : int
      {
         return x | y;
      }
      
      public function mod(v:BigInteger) : BigInteger
      {
         var r:BigInteger = null;
         r = nbi();
         abs().divRemTo(v,null,r);
         if(s < 0 && r.compareTo(ZERO) > 0)
         {
            v.subTo(r,r);
         }
         return r;
      }
      
      protected function addTo(a:BigInteger, r:BigInteger) : void
      {
         var i:int = 0;
         var c:int = 0;
         var m:int = 0;
         i = 0;
         c = 0;
         m = Math.min(a.t,t);
         while(i < m)
         {
            c = c + (this.a[i] + a.a[i]);
            r.a[i++] = c & DM;
            c = c >> DB;
         }
         if(a.t < t)
         {
            c = c + a.s;
            while(i < t)
            {
               c = c + this.a[i];
               r.a[i++] = c & DM;
               c = c >> DB;
            }
            c = c + s;
         }
         else
         {
            c = c + s;
            while(i < a.t)
            {
               c = c + a.a[i];
               r.a[i++] = c & DM;
               c = c >> DB;
            }
            c = c + a.s;
         }
         r.s = c < 0?int(-1):int(0);
         if(c > 0)
         {
            r.a[i++] = c;
         }
         else if(c < -1)
         {
            r.a[i++] = DV + c;
         }
         r.t = i;
         r.clamp();
      }
      
      protected function bitwiseTo(a:BigInteger, op:Function, r:BigInteger) : void
      {
         var i:int = 0;
         var f:int = 0;
         var m:int = 0;
         m = Math.min(a.t,t);
         for(i = 0; i < m; i++)
         {
            r.a[i] = op(this.a[i],a.a[i]);
         }
         if(a.t < t)
         {
            f = a.s & DM;
            for(i = m; i < t; i++)
            {
               r.a[i] = op(this.a[i],f);
            }
            r.t = t;
         }
         else
         {
            f = s & DM;
            for(i = m; i < a.t; i++)
            {
               r.a[i] = op(f,a.a[i]);
            }
            r.t = a.t;
         }
         r.s = op(s,a.s);
         r.clamp();
      }
      
      protected function modInt(n:int) : int
      {
         var d:int = 0;
         var r:int = 0;
         var i:int = 0;
         if(n <= 0)
         {
            return 0;
         }
         d = DV % n;
         r = s < 0?int(n - 1):int(0);
         if(t > 0)
         {
            if(d == 0)
            {
               r = a[0] % n;
            }
            else
            {
               for(i = t - 1; i >= 0; i--)
               {
                  r = (d * r + a[i]) % n;
               }
            }
         }
         return r;
      }
      
      protected function chunkSize(r:Number) : int
      {
         return Math.floor(Math.LN2 * DB / Math.log(r));
      }
      
      bi_internal function dAddOffset(n:int, w:int) : void
      {
         while(t <= w)
         {
            a[t++] = 0;
         }
         a[w] = a[w] + n;
         while(a[w] >= DV)
         {
            a[w] = a[w] - DV;
            if(++w >= t)
            {
               a[t++] = 0;
            }
            a[w]++;
         }
      }
      
      bi_internal function lShiftTo(n:int, r:BigInteger) : void
      {
         var bs:int = 0;
         var cbs:int = 0;
         var bm:int = 0;
         var ds:int = 0;
         var c:int = 0;
         var i:int = 0;
         bs = n % DB;
         cbs = DB - bs;
         bm = (1 << cbs) - 1;
         ds = n / DB;
         c = s << bs & DM;
         for(i = t - 1; i >= 0; i--)
         {
            r.a[i + ds + 1] = a[i] >> cbs | c;
            c = (a[i] & bm) << bs;
         }
         for(i = ds - 1; i >= 0; i--)
         {
            r.a[i] = 0;
         }
         r.a[ds] = c;
         r.t = t + ds + 1;
         r.s = s;
         r.clamp();
      }
      
      public function getLowestSetBit() : int
      {
         var i:int = 0;
         for(i = 0; i < t; i++)
         {
            if(a[i] != 0)
            {
               return i * DB + lbit(a[i]);
            }
         }
         if(s < 0)
         {
            return t * DB;
         }
         return -1;
      }
      
      public function subtract(a:BigInteger) : BigInteger
      {
         var r:BigInteger = null;
         r = new BigInteger();
         subTo(a,r);
         return r;
      }
      
      public function primify(bits:int, t:int) : void
      {
         if(!testBit(bits - 1))
         {
            bitwiseTo(BigInteger.ONE.shiftLeft(bits - 1),op_or,this);
         }
         if(isEven())
         {
            dAddOffset(1,0);
         }
         while(!isProbablePrime(t))
         {
            for(dAddOffset(2,0); bitLength() > bits; )
            {
               subTo(BigInteger.ONE.shiftLeft(bits - 1),this);
            }
         }
      }
      
      public function gcd(a:BigInteger) : BigInteger
      {
         var x:BigInteger = null;
         var y:BigInteger = null;
         var i:int = 0;
         var g:int = 0;
         var t:BigInteger = null;
         x = s < 0?negate():clone();
         y = a.s < 0?a.negate():a.clone();
         if(x.compareTo(y) < 0)
         {
            t = x;
            x = y;
            y = t;
         }
         i = x.getLowestSetBit();
         g = y.getLowestSetBit();
         if(g < 0)
         {
            return x;
         }
         if(i < g)
         {
            g = i;
         }
         if(g > 0)
         {
            x.rShiftTo(g,x);
            y.rShiftTo(g,y);
         }
         while(x.sigNum() > 0)
         {
            if((i = x.getLowestSetBit()) > 0)
            {
               x.rShiftTo(i,x);
            }
            if((i = y.getLowestSetBit()) > 0)
            {
               y.rShiftTo(i,y);
            }
            if(x.compareTo(y) >= 0)
            {
               x.subTo(y,x);
               x.rShiftTo(1,x);
            }
            else
            {
               y.subTo(x,y);
               y.rShiftTo(1,y);
            }
         }
         if(g > 0)
         {
            y.lShiftTo(g,y);
         }
         return y;
      }
      
      bi_internal function multiplyLowerTo(a:BigInteger, n:int, r:BigInteger) : void
      {
         var i:int = 0;
         var j:int = 0;
         i = Math.min(t + a.t,n);
         r.s = 0;
         r.t = i;
         while(i > 0)
         {
            r.a[--i] = 0;
         }
         for(j = r.t - t; i < j; i++)
         {
            r.a[i + t] = am(0,a.a[i],r,i,0,t);
         }
         for(j = Math.min(a.t,n); i < j; i++)
         {
            am(0,a.a[i],r,i,0,n - i);
         }
         r.clamp();
      }
      
      public function modPowInt(e:int, m:BigInteger) : BigInteger
      {
         var z:IReduction = null;
         if(e < 256 || m.isEven())
         {
            z = new ClassicReduction(m);
         }
         else
         {
            z = new MontgomeryReduction(m);
         }
         return exp(e,z);
      }
      
      bi_internal function intAt(str:String, index:int) : int
      {
         return parseInt(str.charAt(index),36);
      }
      
      public function testBit(n:int) : Boolean
      {
         var j:int = 0;
         j = Math.floor(n / DB);
         if(j >= t)
         {
            return s != 0;
         }
         return (a[j] & 1 << n % DB) != 0;
      }
      
      bi_internal function exp(e:int, z:IReduction) : BigInteger
      {
         var r:BigInteger = null;
         var r2:BigInteger = null;
         var g:BigInteger = null;
         var i:int = 0;
         var t:BigInteger = null;
         if(e > 4294967295 || e < 1)
         {
            return ONE;
         }
         r = nbi();
         r2 = nbi();
         g = z.convert(this);
         i = nbits(e) - 1;
         g.copyTo(r);
         while(--i >= 0)
         {
            z.sqrTo(r,r2);
            if((e & 1 << i) > 0)
            {
               z.mulTo(r2,g,r);
            }
            else
            {
               t = r;
               r = r2;
               r2 = t;
            }
         }
         return z.revert(r);
      }
      
      public function toArray(array:ByteArray) : uint
      {
         var k:int = 0;
         var km:int = 0;
         var d:int = 0;
         var i:int = 0;
         var p:int = 0;
         var m:Boolean = false;
         var c:int = 0;
         k = 8;
         km = (1 << 8) - 1;
         d = 0;
         i = t;
         p = DB - i * DB % k;
         m = false;
         c = 0;
         if(i-- > 0)
         {
            if(p < DB && (d = a[i] >> p) > 0)
            {
               m = true;
               array.writeByte(d);
               c++;
            }
            while(i >= 0)
            {
               if(p < k)
               {
                  d = (a[i] & (1 << p) - 1) << k - p;
                  d = d | a[--i] >> (p = p + (DB - k));
               }
               else
               {
                  d = a[i] >> (p = p - k) & km;
                  if(p <= 0)
                  {
                     p = p + DB;
                     i--;
                  }
               }
               if(d > 0)
               {
                  m = true;
               }
               if(m)
               {
                  array.writeByte(d);
                  c++;
               }
            }
         }
         return c;
      }
      
      public function dispose() : void
      {
         var r:Random = null;
         var i:uint = 0;
         r = new Random();
         for(i = 0; i < a.length; i++)
         {
            a[i] = r.nextByte();
            delete a[i];
         }
         a = null;
         t = 0;
         s = 0;
         Memory.gc();
      }
      
      private function lbit(x:int) : int
      {
         var r:int = 0;
         if(x == 0)
         {
            return -1;
         }
         r = 0;
         if((x & 65535) == 0)
         {
            x = x >> 16;
            r = r + 16;
         }
         if((x & 255) == 0)
         {
            x = x >> 8;
            r = r + 8;
         }
         if((x & 15) == 0)
         {
            x = x >> 4;
            r = r + 4;
         }
         if((x & 3) == 0)
         {
            x = x >> 2;
            r = r + 2;
         }
         if((x & 1) == 0)
         {
            r++;
         }
         return r;
      }
      
      bi_internal function divRemTo(m:BigInteger, q:BigInteger = null, r:BigInteger = null) : void
      {
         var pm:BigInteger = null;
         var pt:BigInteger = null;
         var y:BigInteger = null;
         var ts:int = 0;
         var ms:int = 0;
         var nsh:int = 0;
         var ys:int = 0;
         var y0:int = 0;
         var yt:Number = NaN;
         var d1:Number = NaN;
         var d2:Number = NaN;
         var e:Number = NaN;
         var i:int = 0;
         var j:int = 0;
         var t:BigInteger = null;
         var qd:int = 0;
         pm = m.abs();
         if(pm.t <= 0)
         {
            return;
         }
         pt = abs();
         if(pt.t < pm.t)
         {
            if(q != null)
            {
               q.fromInt(0);
            }
            if(r != null)
            {
               copyTo(r);
            }
            return;
         }
         if(r == null)
         {
            var r:BigInteger = nbi();
         }
         y = nbi();
         ts = s;
         ms = m.s;
         nsh = DB - nbits(pm.a[pm.t - 1]);
         if(nsh > 0)
         {
            pm.lShiftTo(nsh,y);
            pt.lShiftTo(nsh,r);
         }
         else
         {
            pm.copyTo(y);
            pt.copyTo(r);
         }
         ys = y.t;
         y0 = y.a[ys - 1];
         if(y0 == 0)
         {
            return;
         }
         yt = y0 * (1 << F1) + (ys > 1?y.a[ys - 2] >> F2:0);
         d1 = FV / yt;
         d2 = (1 << F1) / yt;
         e = 1 << F2;
         i = r.t;
         j = i - ys;
         t = q == null?nbi():q;
         y.dlShiftTo(j,t);
         if(r.compareTo(t) >= 0)
         {
            r.a[r.t++] = 1;
            r.subTo(t,r);
         }
         ONE.dlShiftTo(ys,t);
         for(t.subTo(y,y); y.t < ys; )
         {
            for(; §§hasnext(y,_loc6_); })
         {
            with(_loc9_)
            {
               
               y.t++;
               if(0)
               {
                  _loc5_[_loc6_] = _loc8_;
               }
            }
         }
         while(--j >= 0)
         {
            qd = r.a[--i] == y0?int(DM):int(Number(r.a[i]) * d1 + (Number(r.a[i - 1]) + e) * d2);
            if((r.a[i] = r.a[i] + y.am(0,qd,r,j,0,ys)) < qd)
            {
               y.dlShiftTo(j,t);
               r.subTo(t,r);
               while(r.a[i] < --qd)
               {
                  r.subTo(t,r);
               }
               continue;
            }
         }
         if(q != null)
         {
            r.drShiftTo(ys,q);
            if(ts != ms)
            {
               ZERO.subTo(q,q);
            }
         }
         r.t = ys;
         r.clamp();
         if(nsh > 0)
         {
            r.rShiftTo(nsh,r);
         }
         if(ts < 0)
         {
            ZERO.subTo(r,r);
         }
      }
      
      public function remainder(a:BigInteger) : BigInteger
      {
         var r:BigInteger = null;
         r = new BigInteger();
         divRemTo(a,null,r);
         return r;
      }
      
      public function divide(a:BigInteger) : BigInteger
      {
         var r:BigInteger = null;
         r = new BigInteger();
         divRemTo(a,r,null);
         return r;
      }
      
      public function divideAndRemainder(a:BigInteger) : Array
      {
         var q:BigInteger = null;
         var r:BigInteger = null;
         q = new BigInteger();
         r = new BigInteger();
         divRemTo(a,q,r);
         return [q,r];
      }
      
      public function valueOf() : Number
      {
         var coef:Number = NaN;
         var value:Number = NaN;
         var i:uint = 0;
         coef = 1;
         value = 0;
         for(i = 0; i < t; i++)
         {
            value = value + a[i] * coef;
            coef = coef * DV;
         }
         return value;
      }
      
      public function shiftLeft(n:int) : BigInteger
      {
         var r:BigInteger = null;
         r = new BigInteger();
         if(n < 0)
         {
            rShiftTo(-n,r);
         }
         else
         {
            lShiftTo(n,r);
         }
         return r;
      }
      
      public function multiply(a:BigInteger) : BigInteger
      {
         var r:BigInteger = null;
         r = new BigInteger();
         multiplyTo(a,r);
         return r;
      }
      
      bi_internal function am(i:int, x:int, w:BigInteger, j:int, c:int, n:int) : int
      {
         var xl:int = 0;
         var xh:int = 0;
         var l:int = 0;
         var h:int = 0;
         var m:int = 0;
         xl = x & 32767;
         xh = x >> 15;
         while(--n >= 0)
         {
            l = a[i] & 32767;
            h = a[i++] >> 15;
            m = xh * l + h * xl;
            l = xl * l + ((m & 32767) << 15) + w.a[j] + (c & 1073741823);
            c = (l >>> 30) + (m >>> 15) + xh * h + (c >>> 30);
            w.a[j++] = l & 1073741823;
         }
         return c;
      }
      
      bi_internal function drShiftTo(n:int, r:BigInteger) : void
      {
         var i:int = 0;
         for(i = n; i < t; i++)
         {
            r.a[i - n] = a[i];
         }
         r.t = Math.max(t - n,0);
         r.s = s;
      }
      
      public function add(a:BigInteger) : BigInteger
      {
         var r:BigInteger = null;
         r = new BigInteger();
         addTo(a,r);
         return r;
      }
      
      bi_internal function multiplyUpperTo(a:BigInteger, n:int, r:BigInteger) : void
      {
         var i:int = 0;
         n--;
         i = r.t = t + a.t - n;
         r.s = 0;
         while(--i >= 0)
         {
            r.a[i] = 0;
         }
         for(i = Math.max(n - t,0); i < a.t; i++)
         {
            r.a[t + i - n] = am(n - i,a.a[i],r,0,0,t + i - n);
         }
         r.clamp();
         r.drShiftTo(1,r);
      }
      
      protected function nbi() : *
      {
         return new BigInteger();
      }
      
      protected function millerRabin(t:int) : Boolean
      {
         var n1:BigInteger = null;
         var k:int = 0;
         var r:BigInteger = null;
         var a:BigInteger = null;
         var i:int = 0;
         var y:BigInteger = null;
         var j:int = 0;
         n1 = subtract(BigInteger.ONE);
         k = n1.getLowestSetBit();
         if(k <= 0)
         {
            return false;
         }
         r = n1.shiftRight(k);
         t = t + 1 >> 1;
         if(t > lowprimes.length)
         {
            t = lowprimes.length;
         }
         a = new BigInteger();
         for(i = 0; i < t; i++)
         {
            a.fromInt(lowprimes[i]);
            y = a.modPow(r,this);
            if(y.compareTo(BigInteger.ONE) != 0 && y.compareTo(n1) != 0)
            {
               j = 1;
               while(j++ < k && y.compareTo(n1) != 0)
               {
                  y = y.modPowInt(2,this);
                  if(y.compareTo(BigInteger.ONE) == 0)
                  {
                     return false;
                  }
               }
               if(y.compareTo(n1) != 0)
               {
                  return false;
               }
            }
         }
         return true;
      }
      
      bi_internal function dMultiply(n:int) : void
      {
         a[t] = am(0,n - 1,this,0,0,t);
         t++;
         clamp();
      }
      
      private function op_andnot(x:int, y:int) : int
      {
         return x & ~y;
      }
      
      bi_internal function clamp() : void
      {
         var c:int = 0;
         c = s & DM;
         while(t > 0 && a[t - 1] == c)
         {
            t--;
         }
      }
      
      bi_internal function invDigit() : int
      {
         var x:int = 0;
         var y:int = 0;
         if(t < 1)
         {
            return 0;
         }
         x = a[0];
         if((x & 1) == 0)
         {
            return 0;
         }
         y = x & 3;
         y = y * (2 - (x & 15) * y) & 15;
         y = y * (2 - (x & 255) * y) & 255;
         y = y * (2 - ((x & 65535) * y & 65535)) & 65535;
         y = y * (2 - x * y % DV) % DV;
         return y > 0?int(DV - y):int(-y);
      }
      
      protected function changeBit(n:int, op:Function) : BigInteger
      {
         var r:BigInteger = null;
         r = BigInteger.ONE.shiftLeft(n);
         bitwiseTo(r,op,r);
         return r;
      }
      
      public function equals(a:BigInteger) : Boolean
      {
         return compareTo(a) == 0;
      }
      
      public function compareTo(v:BigInteger) : int
      {
         var r:int = 0;
         var i:int = 0;
         r = s - v.s;
         if(r != 0)
         {
            return r;
         }
         i = t;
         r = i - v.t;
         if(r != 0)
         {
            return r;
         }
         while(--i >= 0)
         {
            r = a[i] - v.a[i];
            if(r != 0)
            {
               return r;
            }
         }
         return 0;
      }
      
      public function shiftRight(n:int) : BigInteger
      {
         var r:BigInteger = null;
         r = new BigInteger();
         if(n < 0)
         {
            lShiftTo(-n,r);
         }
         else
         {
            rShiftTo(n,r);
         }
         return r;
      }
      
      bi_internal function multiplyTo(v:BigInteger, r:BigInteger) : void
      {
         var x:BigInteger = null;
         var y:BigInteger = null;
         var i:int = 0;
         x = abs();
         y = v.abs();
         i = x.t;
         r.t = i + y.t;
         while(--i >= 0)
         {
            r.a[i] = 0;
         }
         for(i = 0; i < y.t; i++)
         {
            r.a[i + x.t] = x.am(0,y.a[i],r,i,0,x.t);
         }
         r.s = 0;
         r.clamp();
         if(s != v.s)
         {
            ZERO.subTo(r,r);
         }
      }
      
      public function bitCount() : int
      {
         var r:int = 0;
         var x:int = 0;
         var i:int = 0;
         r = 0;
         x = s & DM;
         for(i = 0; i < t; i++)
         {
            r = r + cbit(a[i] ^ x);
         }
         return r;
      }
      
      public function byteValue() : int
      {
         return t == 0?int(s):int(a[0] << 24 >> 24);
      }
      
      private function cbit(x:int) : int
      {
         var r:uint = 0;
         for(r = 0; x != 0; )
         {
            x = x & x - 1;
            r++;
         }
         return r;
      }
      
      bi_internal function rShiftTo(n:int, r:BigInteger) : void
      {
         var ds:int = 0;
         var bs:int = 0;
         var cbs:int = 0;
         var bm:int = 0;
         var i:int = 0;
         r.s = s;
         ds = n / DB;
         if(ds >= t)
         {
            r.t = 0;
            return;
         }
         bs = n % DB;
         cbs = DB - bs;
         bm = (1 << bs) - 1;
         r.a[0] = a[ds] >> bs;
         for(i = ds + 1; i < t; i++)
         {
            r.a[i - ds - 1] = r.a[i - ds - 1] | (a[i] & bm) << cbs;
            r.a[i - ds] = a[i] >> bs;
         }
         if(bs > 0)
         {
            r.a[t - ds - 1] = r.a[t - ds - 1] | (s & bm) << cbs;
         }
         r.t = t - ds;
         r.clamp();
      }
      
      public function modInverse(m:BigInteger) : BigInteger
      {
         var ac:Boolean = false;
         var u:BigInteger = null;
         var v:BigInteger = null;
         var a:BigInteger = null;
         var b:BigInteger = null;
         var c:BigInteger = null;
         var d:BigInteger = null;
         ac = m.isEven();
         if(isEven() && ac || m.sigNum() == 0)
         {
            return BigInteger.ZERO;
         }
         u = m.clone();
         v = clone();
         a = nbv(1);
         b = nbv(0);
         c = nbv(0);
         d = nbv(1);
         while(u.sigNum() != 0)
         {
            while(u.isEven())
            {
               u.rShiftTo(1,u);
               if(ac)
               {
                  if(!a.isEven() || !b.isEven())
                  {
                     a.addTo(this,a);
                     b.subTo(m,b);
                  }
                  a.rShiftTo(1,a);
               }
               else if(!b.isEven())
               {
                  b.subTo(m,b);
               }
               b.rShiftTo(1,b);
            }
            while(v.isEven())
            {
               v.rShiftTo(1,v);
               if(ac)
               {
                  if(!c.isEven() || !d.isEven())
                  {
                     c.addTo(this,c);
                     d.subTo(m,d);
                  }
                  c.rShiftTo(1,c);
               }
               else if(!d.isEven())
               {
                  d.subTo(m,d);
               }
               d.rShiftTo(1,d);
            }
            if(u.compareTo(v) >= 0)
            {
               u.subTo(v,u);
               if(ac)
               {
                  a.subTo(c,a);
               }
               b.subTo(d,b);
            }
            else
            {
               v.subTo(u,v);
               if(ac)
               {
                  c.subTo(a,c);
               }
               d.subTo(b,d);
            }
         }
         if(v.compareTo(BigInteger.ONE) != 0)
         {
            return BigInteger.ZERO;
         }
         if(d.compareTo(m) >= 0)
         {
            return d.subtract(m);
         }
         if(d.sigNum() < 0)
         {
            d.addTo(m,d);
            if(d.sigNum() < 0)
            {
               return d.add(m);
            }
            return d;
         }
         return d;
      }
      
      bi_internal function fromArray(value:ByteArray, length:int) : void
      {
         var p:int = 0;
         var i:int = 0;
         var sh:int = 0;
         var k:int = 0;
         var x:int = 0;
         p = value.position;
         i = p + length;
         sh = 0;
         k = 8;
         t = 0;
         s = 0;
         while(--i >= p)
         {
            x = i < value.length?int(value[i]):int(0);
            if(sh == 0)
            {
               a[t++] = x;
            }
            else if(sh + k > DB)
            {
               a[t - 1] = a[t - 1] | (x & (1 << DB - sh) - 1) << sh;
               a[t++] = x >> DB - sh;
            }
            else
            {
               a[t - 1] = a[t - 1] | x << sh;
            }
            sh = sh + k;
            if(sh >= DB)
            {
               sh = sh - DB;
            }
         }
         clamp();
         value.position = Math.min(p + length,value.length);
      }
      
      bi_internal function copyTo(r:BigInteger) : void
      {
         var i:int = 0;
         for(i = t - 1; i >= 0; i--)
         {
            r.a[i] = a[i];
         }
         r.t = t;
         r.s = s;
      }
      
      public function intValue() : int
      {
         if(s < 0)
         {
            if(t == 1)
            {
               return a[0] - DV;
            }
            if(t == 0)
            {
               return -1;
            }
         }
         else
         {
            if(t == 1)
            {
               return a[0];
            }
            if(t == 0)
            {
               return 0;
            }
         }
         return (a[1] & (1 << 32 - DB) - 1) << DB | a[0];
      }
      
      public function min(a:BigInteger) : BigInteger
      {
         return compareTo(a) < 0?this:a;
      }
      
      public function bitLength() : int
      {
         if(t <= 0)
         {
            return 0;
         }
         return DB * (t - 1) + nbits(a[t - 1] ^ s & DM);
      }
      
      public function shortValue() : int
      {
         return t == 0?int(s):int(a[0] << 16 >> 16);
      }
      
      public function and(a:BigInteger) : BigInteger
      {
         var r:BigInteger = null;
         r = new BigInteger();
         bitwiseTo(a,op_and,r);
         return r;
      }
      
      protected function toRadix(b:uint = 10) : String
      {
         var cs:int = 0;
         var a:Number = NaN;
         var d:BigInteger = null;
         var y:BigInteger = null;
         var z:BigInteger = null;
         var r:String = null;
         if(sigNum() == 0 || b < 2 || b > 32)
         {
            return "0";
         }
         cs = chunkSize(b);
         a = Math.pow(b,cs);
         d = nbv(a);
         y = nbi();
         z = nbi();
         r = "";
         divRemTo(d,y,z);
         while(y.sigNum() > 0)
         {
            r = (a + z.intValue()).toString(b).substr(1) + r;
            y.divRemTo(d,y,z);
         }
         return z.intValue().toString(b) + r;
      }
      
      public function not() : BigInteger
      {
         var r:BigInteger = null;
         var i:int = 0;
         r = new BigInteger();
         for(i = 0; i < t; i++)
         {
            r[i] = DM & ~a[i];
         }
         r.t = t;
         r.s = ~s;
         return r;
      }
      
      bi_internal function subTo(v:BigInteger, r:BigInteger) : void
      {
         var i:int = 0;
         var c:int = 0;
         var m:int = 0;
         i = 0;
         c = 0;
         m = Math.min(v.t,t);
         while(i < m)
         {
            c = c + (a[i] - v.a[i]);
            r.a[i++] = c & DM;
            c = c >> DB;
         }
         if(v.t < t)
         {
            c = c - v.s;
            while(i < t)
            {
               c = c + a[i];
               r.a[i++] = c & DM;
               c = c >> DB;
            }
            c = c + s;
         }
         else
         {
            c = c + s;
            while(i < v.t)
            {
               c = c - v.a[i];
               r.a[i++] = c & DM;
               c = c >> DB;
            }
            c = c - v.s;
         }
         r.s = c < 0?int(-1):int(0);
         if(c < -1)
         {
            r.a[i++] = DV + c;
         }
         else if(c > 0)
         {
            r.a[i++] = c;
         }
         r.t = i;
         r.clamp();
      }
      
      public function clone() : BigInteger
      {
         var r:BigInteger = null;
         r = new BigInteger();
         this.copyTo(r);
         return r;
      }
      
      public function pow(e:int) : BigInteger
      {
         return exp(e,new NullReduction());
      }
      
      public function flipBit(n:int) : BigInteger
      {
         return changeBit(n,op_xor);
      }
      
      public function xor(a:BigInteger) : BigInteger
      {
         var r:BigInteger = null;
         r = new BigInteger();
         bitwiseTo(a,op_xor,r);
         return r;
      }
      
      public function or(a:BigInteger) : BigInteger
      {
         var r:BigInteger = null;
         r = new BigInteger();
         bitwiseTo(a,op_or,r);
         return r;
      }
      
      public function max(a:BigInteger) : BigInteger
      {
         return compareTo(a) > 0?this:a;
      }
      
      bi_internal function fromInt(value:int) : void
      {
         t = 1;
         s = value < 0?int(-1):int(0);
         if(value > 0)
         {
            a[0] = value;
         }
         else if(value < -1)
         {
            a[0] = value + DV;
         }
         else
         {
            t = 0;
         }
      }
      
      bi_internal function isEven() : Boolean
      {
         return (t > 0?a[0] & 1:s) == 0;
      }
      
      public function toString(radix:Number = 16) : String
      {
         var k:int = 0;
         var km:int = 0;
         var d:int = 0;
         var m:Boolean = false;
         var r:String = null;
         var i:int = 0;
         var p:int = 0;
         if(s < 0)
         {
            return "-" + negate().toString(radix);
         }
         switch(radix)
         {
            case 2:
               k = 1;
               break;
            case 4:
               k = 2;
               break;
            case 8:
               k = 3;
               break;
            case 16:
               k = 4;
               break;
            case 32:
               k = 5;
         }
         km = (1 << k) - 1;
         d = 0;
         m = false;
         r = "";
         i = t;
         p = DB - i * DB % k;
         if(i-- > 0)
         {
            if(p < DB && (d = a[i] >> p) > 0)
            {
               m = true;
               r = d.toString(36);
            }
            while(i >= 0)
            {
               if(p < k)
               {
                  d = (a[i] & (1 << p) - 1) << k - p;
                  d = d | a[--i] >> (p = p + (DB - k));
               }
               else
               {
                  d = a[i] >> (p = p - k) & km;
                  if(p <= 0)
                  {
                     p = p + DB;
                     i--;
                  }
               }
               if(d > 0)
               {
                  m = true;
               }
               if(m)
               {
                  r = r + d.toString(36);
               }
            }
         }
         return !!m?r:"0";
      }
      
      public function setBit(n:int) : BigInteger
      {
         return changeBit(n,op_or);
      }
      
      public function abs() : BigInteger
      {
         return s < 0?negate():this;
      }
      
      bi_internal function nbits(x:int) : int
      {
         var r:int = 0;
         var t:int = 0;
         r = 1;
         if((t = x >>> 16) != 0)
         {
            x = t;
            r = r + 16;
         }
         if((t = x >> 8) != 0)
         {
            x = t;
            r = r + 8;
         }
         if((t = x >> 4) != 0)
         {
            x = t;
            r = r + 4;
         }
         if((t = x >> 2) != 0)
         {
            x = t;
            r = r + 2;
         }
         if((t = x >> 1) != 0)
         {
            x = t;
            r = r + 1;
         }
         return r;
      }
      
      public function sigNum() : int
      {
         if(s < 0)
         {
            return -1;
         }
         if(t <= 0 || t == 1 && a[0] <= 0)
         {
            return 0;
         }
         return 1;
      }
      
      public function toByteArray() : ByteArray
      {
         var i:int = 0;
         var r:ByteArray = null;
         var p:int = 0;
         var d:int = 0;
         var k:int = 0;
         i = t;
         r = new ByteArray();
         r[0] = s;
         p = DB - i * DB % 8;
         k = 0;
         if(i-- > 0)
         {
            if(p < DB && (d = a[i] >> p) != (s & DM) >> p)
            {
               r[k++] = d | s << DB - p;
            }
            while(i >= 0)
            {
               if(p < 8)
               {
                  d = (a[i] & (1 << p) - 1) << 8 - p;
                  d = d | a[--i] >> (p = p + (DB - 8));
               }
               else
               {
                  d = a[i] >> (p = p - 8) & 255;
                  if(p <= 0)
                  {
                     p = p + DB;
                     i--;
                  }
               }
               if((d & 128) != 0)
               {
                  d = d | -256;
               }
               if(k == 0 && (s & 128) != (d & 128))
               {
                  k++;
               }
               if(k > 0 || d != s)
               {
                  r[k++] = d;
               }
            }
         }
         return r;
      }
      
      bi_internal function squareTo(r:BigInteger) : void
      {
         var x:BigInteger = null;
         var i:int = 0;
         var c:int = 0;
         x = abs();
         for(i = r.t = 2 * x.t; --i >= 0; )
         {
            r.a[i] = 0;
         }
         for(i = 0; i < x.t - 1; i++)
         {
            c = x.am(i,x.a[i],r,2 * i,0,1);
            if((r.a[i + x.t] = r.a[i + x.t] + x.am(i + 1,2 * x.a[i],r,2 * i + 1,c,x.t - i - 1)) >= DV)
            {
               r.a[i + x.t] = r.a[i + x.t] - DV;
               r.a[i + x.t + 1] = 1;
            }
         }
         if(r.t > 0)
         {
            r.a[r.t - 1] = r.a[r.t - 1] + x.am(i,x.a[i],r,2 * i,0,1);
         }
         r.s = 0;
         r.clamp();
      }
      
      private function op_and(x:int, y:int) : int
      {
         return x & y;
      }
      
      protected function fromRadix(s:String, b:int = 10) : void
      {
         var cs:int = 0;
         var d:Number = NaN;
         var mi:Boolean = false;
         var j:int = 0;
         var w:int = 0;
         var i:int = 0;
         var x:int = 0;
         fromInt(0);
         cs = chunkSize(b);
         d = Math.pow(b,cs);
         mi = false;
         j = 0;
         w = 0;
         for(i = 0; i < s.length; i++)
         {
            x = intAt(s,i);
            if(x < 0)
            {
               if(s.charAt(i) == "-" && sigNum() == 0)
               {
                  mi = true;
               }
            }
            else
            {
               w = b * w + x;
               if(++j >= cs)
               {
                  dMultiply(d);
                  dAddOffset(w,0);
                  j = 0;
                  w = 0;
               }
            }
         }
         if(j > 0)
         {
            dMultiply(Math.pow(b,j));
            dAddOffset(w,0);
         }
         if(mi)
         {
            BigInteger.ZERO.subTo(this,this);
         }
      }
      
      bi_internal function dlShiftTo(n:int, r:BigInteger) : void
      {
         var i:int = 0;
         for(i = t - 1; i >= 0; i--)
         {
            r.a[i + n] = a[i];
         }
         for(i = n - 1; i >= 0; i--)
         {
            r.a[i] = 0;
         }
         r.t = t + n;
         r.s = s;
      }
      
      private function op_xor(x:int, y:int) : int
      {
         return x ^ y;
      }
   }
}
