package cmodule.decry
{
   import avm2.intrinsics.memory.li16;
   import avm2.intrinsics.memory.li32;
   import avm2.intrinsics.memory.li8;
   import avm2.intrinsics.memory.si32;
   import avm2.intrinsics.memory.si8;
   
   public final class FSM_pubrealloc extends Machine
   {
      
      public static const intRegCount:int = 13;
      
      public static const NumberRegCount:int = 0;
       
      
      public var i10:int;
      
      public var i11:int;
      
      public var i12:int;
      
      public var i0:int;
      
      public var i1:int;
      
      public var i2:int;
      
      public var i3:int;
      
      public var i4:int;
      
      public var i5:int;
      
      public var i6:int;
      
      public var i7:int;
      
      public var i8:int;
      
      public var i9:int;
      
      public function FSM_pubrealloc()
      {
         super();
      }
      
      public static function start() : void
      {
         var _loc1_:FSM_pubrealloc = null;
         _loc1_ = new FSM_pubrealloc();
         FSM_pubrealloc.gworker = _loc1_;
      }
      
      override public final function work() : void
      {
         switch(state)
         {
            case 0:
               mstate.esp = mstate.esp - 4;
               si32(mstate.ebp,mstate.esp);
               mstate.ebp = mstate.esp;
               mstate.esp = mstate.esp - 4096;
               this.i0 = li32(FSM_pubrealloc);
               this.i2 = li32(mstate.ebp + 8);
               this.i3 = li32(mstate.ebp + 12);
               if(this.i0 >= 1)
               {
                  if(this.i0 == 1)
                  {
                     this.i2 = 2;
                     si32(this.i2,FSM_pubrealloc);
                  }
                  this.i2 = 88;
                  si32(this.i2,FSM_pubrealloc);
                  this.i2 = 0;
               }
               else
               {
                  this.i0 = 1;
                  si32(this.i0,FSM_pubrealloc);
                  this.i0 = li8(FSM_pubrealloc);
                  if(this.i0 == 0)
                  {
                     if(this.i2 != 0)
                     {
                        this.i2 = 0;
                        si32(this.i2,FSM_pubrealloc);
                        this.i3 = 88;
                        si32(this.i3,FSM_pubrealloc);
                     }
                     else
                     {
                        this.i0 = 0;
                        this.i4 = li32(FSM_pubrealloc);
                        this.i5 = mstate.ebp + -4096;
                        loop0:
                        while(true)
                        {
                           this.i6 = this.i0;
                           if(this.i6 != 1)
                           {
                              if(this.i6 == 0)
                              {
                                 break;
                              }
                           }
                           else
                           {
                              this.i0 = FSM_pubrealloc;
                              this.i1 = 4;
                              log(this.i1,mstate.gworker.stringFromPtr(this.i0));
                              mstate.esp = mstate.esp - 4;
                              this.i0 = FSM_pubrealloc;
                              si32(this.i0,mstate.esp);
                              mstate.esp = mstate.esp - 4;
                              FSM_pubrealloc.start();
                           }
                           addr938:
                           while(true)
                           {
                              this.i0 = this.i6 + 1;
                              if(this.i0 != 3)
                              {
                                 continue loop0;
                              }
                              this.i0 = li8(FSM_pubrealloc);
                              this.i0 = this.i0 ^ 1;
                              this.i0 = this.i0 & 1;
                              if(this.i0 == 0)
                              {
                                 this.i0 = 1;
                                 si8(this.i0,FSM_pubrealloc);
                              }
                              this.i0 = FSM_pubrealloc;
                              this.i1 = 4;
                              this.i5 = 0;
                              log(this.i1,mstate.gworker.stringFromPtr(this.i0));
                              this.i0 = _sbrk(this.i5);
                              this.i0 = this.i0 & 4095;
                              this.i0 = 4096 - this.i0;
                              this.i0 = this.i0 & 4095;
                              this.i0 = _sbrk(this.i0);
                              this.i0 = 4096;
                              this.i0 = _sbrk(this.i0);
                              si32(this.i0,FSM_pubrealloc);
                              this.i0 = this.i5;
                              this.i0 = _sbrk(this.i0);
                              this.i0 = this.i0 + 4095;
                              this.i0 = this.i0 >>> 12;
                              this.i0 = this.i0 + -12;
                              si32(this.i0,FSM_pubrealloc);
                              this.i0 = 1024;
                              si32(this.i0,FSM_pubrealloc);
                              this.i0 = li32(FSM_pubrealloc);
                              if(this.i0 == 0)
                              {
                                 this.i0 = this.i0 + 1;
                                 si32(this.i0,FSM_pubrealloc);
                              }
                              this.i1 = 20;
                              this.i0 = this.i0 << 12;
                              si32(this.i0,FSM_pubrealloc);
                              mstate.esp = mstate.esp - 4;
                              si32(this.i1,mstate.esp);
                              mstate.esp = mstate.esp - 4;
                              FSM_pubrealloc.start();
                           }
                        }
                        this.i0 = FSM_pubrealloc;
                        mstate.esp = mstate.esp - 20;
                        this.i1 = FSM_pubrealloc;
                        this.i7 = 99;
                        this.i8 = 22;
                        si32(this.i5,mstate.esp);
                        si32(this.i0,mstate.esp + 4);
                        si32(this.i8,mstate.esp + 8);
                        si32(this.i1,mstate.esp + 12);
                        si32(this.i7,mstate.esp + 16);
                        state = 1;
                        mstate.esp = mstate.esp - 4;
                        FSM_pubrealloc.start();
                        return;
                     }
                  }
                  addr1180:
                  this.i0 = li8(FSM_pubrealloc);
                  this.i1 = this.i2 == 2048?int(0):int(this.i2);
                  this.i0 = this.i0 ^ 1;
                  this.i0 = this.i0 & 1;
                  if(this.i0 == 0)
                  {
                     if(this.i3 == 0)
                     {
                        if(this.i1 == 0)
                        {
                           this.i1 = 0;
                           this.i3 = this.i1;
                        }
                        else
                        {
                           this.i3 = 0;
                           mstate.esp = mstate.esp - 4;
                           si32(this.i1,mstate.esp);
                           mstate.esp = mstate.esp - 4;
                           FSM_pubrealloc.start();
                        }
                     }
                     addr2244:
                     this.i0 = this.i1;
                     this.i1 = this.i3;
                     this.i2 = 0;
                     si32(this.i2,FSM_pubrealloc);
                     break;
                  }
                  if(this.i3 == 0)
                  {
                     if(this.i1 == 0)
                     {
                        this.i3 = 2048;
                        this.i1 = 0;
                     }
                     else
                     {
                        this.i3 = 0;
                        mstate.esp = mstate.esp - 4;
                        si32(this.i1,mstate.esp);
                        mstate.esp = mstate.esp - 4;
                        FSM_pubrealloc.start();
                     }
                  }
                  else if(this.i1 == 0)
                  {
                     this.i1 = 0;
                     mstate.esp = mstate.esp - 4;
                     si32(this.i3,mstate.esp);
                     mstate.esp = mstate.esp - 4;
                     FSM_pubrealloc.start();
                  }
                  else
                  {
                     this.i0 = li32(FSM_pubrealloc);
                     this.i2 = this.i1 >>> 12;
                     this.i4 = this.i2 - this.i0;
                     this.i5 = this.i1;
                     if(uint(this.i4) > uint(11))
                     {
                        this.i6 = li32(FSM_pubrealloc);
                        if(uint(this.i4) <= uint(this.i6))
                        {
                           this.i6 = li32(FSM_pubrealloc);
                           this.i7 = this.i4 << 2;
                           this.i7 = this.i6 + this.i7;
                           this.i7 = li32(this.i7);
                           this.i8 = this.i6;
                           if(this.i7 == 2)
                           {
                              this.i5 = this.i5 & 4095;
                              if(this.i5 == 0)
                              {
                                 this.i5 = this.i4 << 2;
                                 this.i5 = this.i5 + this.i8;
                                 this.i5 = li32(this.i5 + 4);
                                 if(this.i5 != 3)
                                 {
                                    this.i0 = 4096;
                                 }
                                 else
                                 {
                                    this.i5 = -1;
                                    this.i0 = this.i2 - this.i0;
                                    this.i0 = this.i0 << 2;
                                    this.i0 = this.i0 + this.i6;
                                    this.i0 = this.i0 + 8;
                                    while(true)
                                    {
                                       this.i7 = li32(this.i0);
                                       this.i0 = this.i0 + 4;
                                       this.i5 = this.i5 + 1;
                                       if(this.i7 == 3)
                                       {
                                          continue;
                                       }
                                       break;
                                    }
                                    this.i0 = this.i5 << 12;
                                    this.i0 = this.i0 + 8192;
                                 }
                                 this.i5 = li8(FSM_pubrealloc);
                                 this.i5 = this.i5 ^ 1;
                                 if(uint(this.i0) >= uint(this.i3))
                                 {
                                    this.i5 = this.i5 & 1;
                                    if(this.i5 != 0)
                                    {
                                       this.i5 = this.i0 + -4096;
                                       if(uint(this.i5) < uint(this.i3))
                                       {
                                          this.i5 = li8(FSM_pubrealloc);
                                          if(this.i5 != 0)
                                          {
                                             this.i5 = -48;
                                             this.i7 = this.i1 + this.i3;
                                             this.i0 = this.i0 - this.i3;
                                             this.i3 = this.i1 == 0?int(1):int(0);
                                             memset(this.i7,this.i5,this.i0);
                                             this.i0 = 0;
                                             si32(this.i0,FSM_pubrealloc);
                                             this.i0 = this.i3 & 1;
                                             break;
                                          }
                                          addr1749:
                                       }
                                    }
                                 }
                                 addr1669:
                                 mstate.esp = mstate.esp - 4;
                                 si32(this.i3,mstate.esp);
                                 mstate.esp = mstate.esp - 4;
                                 FSM_pubrealloc.start();
                              }
                              addr2219:
                              this.i0 = this.i1;
                              this.i1 = this.i0 == 0?int(1):int(0);
                              this.i1 = this.i1 & 1;
                              this.i3 = this.i0;
                           }
                           else if(uint(this.i7) >= uint(4))
                           {
                              this.i0 = li16(this.i7 + 8);
                              this.i2 = this.i0;
                              this.i4 = this.i0 + -1;
                              this.i4 = this.i4 & this.i5;
                              if(this.i4 == 0)
                              {
                                 this.i4 = 1;
                                 this.i6 = li16(this.i7 + 10);
                                 this.i5 = this.i5 & 4095;
                                 this.i5 = this.i5 >>> this.i6;
                                 this.i6 = this.i5 & -32;
                                 this.i6 = this.i6 >>> 3;
                                 this.i5 = this.i5 & 31;
                                 this.i6 = this.i7 + this.i6;
                                 this.i6 = li32(this.i6 + 16);
                                 this.i4 = this.i4 << this.i5;
                                 this.i4 = this.i4 & this.i6;
                                 if(this.i4 == 0)
                                 {
                                    this.i4 = li8(FSM_pubrealloc);
                                    this.i4 = this.i4 ^ 1;
                                    if(uint(this.i2) >= uint(this.i3))
                                    {
                                       this.i4 = this.i4 & 1;
                                       if(this.i4 != 0)
                                       {
                                          this.i4 = this.i2 >>> 1;
                                          if(uint(this.i4) >= uint(this.i3))
                                          {
                                             this.i0 = this.i0 & 65535;
                                             if(this.i0 == 16)
                                             {
                                             }
                                          }
                                          this.i0 = li8(FSM_pubrealloc);
                                          this.i0 = this.i0 ^ 1;
                                          this.i0 = this.i0 & 1;
                                          if(this.i0 == 0)
                                          {
                                             this.i0 = -48;
                                             this.i4 = this.i1 + this.i3;
                                             this.i3 = this.i2 - this.i3;
                                             this.i2 = this.i1 == 0?int(1):int(0);
                                             memset(this.i4,this.i0,this.i3);
                                             this.i0 = 0;
                                             si32(this.i0,FSM_pubrealloc);
                                             this.i0 = this.i2 & 1;
                                             break;
                                          }
                                          §§goto(addr1749);
                                       }
                                    }
                                    this.i0 = this.i2;
                                    §§goto(addr1669);
                                 }
                                 §§goto(addr2219);
                              }
                           }
                        }
                     }
                     this.i1 = 0;
                     §§goto(addr2219);
                  }
                  §§goto(addr2244);
               }
               mstate.eax = this.i2;
               addr2278:
               mstate.esp = mstate.ebp;
               mstate.ebp = li32(mstate.esp);
               mstate.esp = mstate.esp + 4;
               mstate.esp = mstate.esp + 4;
               mstate.gworker = caller;
               return;
            case 1:
               mstate.esp = mstate.esp + 20;
               this.i1 = 3;
               this.i0 = this.i5;
               log(this.i1,mstate.gworker.stringFromPtr(this.i0));
               si32(this.i8,FSM_pubrealloc);
               §§goto(addr938);
            case 2:
               while(true)
               {
                  this.i0 = mstate.eax;
                  mstate.esp = mstate.esp + 4;
                  if(this.i0 != 0)
                  {
                     this.i1 = li32(FSM_pubrealloc);
                     this.i7 = li8(FSM_pubrealloc);
                     this.i8 = li8(FSM_pubrealloc);
                     this.i9 = li8(FSM_pubrealloc);
                     this.i10 = li8(FSM_pubrealloc);
                     this.i11 = li8(FSM_pubrealloc);
                     while(true)
                     {
                        this.i12 = this.i1;
                        this.i1 = li8(this.i0);
                        if(this.i1 == 0)
                        {
                           this.i0 = this.i11;
                           this.i1 = this.i12;
                           break;
                        }
                        this.i1 = this.i1 << 24;
                        this.i1 = this.i1 >> 24;
                        if(this.i1 <= 89)
                        {
                           if(this.i1 <= 73)
                           {
                              if(this.i1 != 60)
                              {
                                 if(this.i1 != 62)
                                 {
                                    if(this.i1 == 72)
                                    {
                                       this.i0 = this.i0 + 1;
                                       if(this.i0 != 0)
                                       {
                                          this.i1 = 1;
                                          this.i7 = this.i1;
                                          this.i1 = this.i12;
                                          continue;
                                       }
                                       this.i1 = 1;
                                       this.i0 = this.i11;
                                       this.i7 = this.i1;
                                       this.i1 = this.i12;
                                       break;
                                    }
                                 }
                                 else
                                 {
                                    this.i0 = this.i0 + 1;
                                    this.i1 = this.i12 << 1;
                                    if(this.i0 != 0)
                                    {
                                       continue;
                                    }
                                    this.i0 = this.i11;
                                    break;
                                 }
                              }
                              else
                              {
                                 this.i0 = this.i0 + 1;
                                 this.i1 = this.i12 >>> 1;
                                 if(this.i0 != 0)
                                 {
                                    continue;
                                 }
                                 this.i0 = this.i11;
                                 break;
                              }
                           }
                           else if(this.i1 != 74)
                           {
                              if(this.i1 != 82)
                              {
                                 if(this.i1 == 86)
                                 {
                                    this.i0 = this.i0 + 1;
                                    if(this.i0 != 0)
                                    {
                                       this.i1 = 1;
                                       this.i10 = this.i1;
                                       this.i1 = this.i12;
                                       continue;
                                    }
                                    this.i1 = 1;
                                    this.i0 = this.i11;
                                    this.i10 = this.i1;
                                    this.i1 = this.i12;
                                    break;
                                 }
                              }
                              else
                              {
                                 this.i0 = this.i0 + 1;
                                 if(this.i0 != 0)
                                 {
                                    this.i1 = 1;
                                    this.i8 = this.i1;
                                    this.i1 = this.i12;
                                    continue;
                                 }
                                 this.i1 = 1;
                                 this.i0 = this.i11;
                                 this.i8 = this.i1;
                                 this.i1 = this.i12;
                                 break;
                              }
                           }
                           else
                           {
                              this.i0 = this.i0 + 1;
                              if(this.i0 != 0)
                              {
                                 this.i1 = 1;
                                 this.i9 = this.i1;
                                 this.i1 = this.i12;
                                 continue;
                              }
                              this.i1 = 1;
                              this.i0 = this.i11;
                              this.i9 = this.i1;
                              this.i1 = this.i12;
                              break;
                           }
                        }
                        else
                        {
                           if(this.i1 <= 113)
                           {
                              if(this.i1 != 90)
                              {
                                 if(this.i1 != 104)
                                 {
                                    if(this.i1 == 106)
                                    {
                                       this.i0 = this.i0 + 1;
                                       if(this.i0 != 0)
                                       {
                                          this.i1 = 0;
                                          this.i9 = this.i1;
                                          this.i1 = this.i12;
                                          continue;
                                       }
                                       this.i1 = 0;
                                       this.i0 = this.i11;
                                       this.i9 = this.i1;
                                       this.i1 = this.i12;
                                       break;
                                    }
                                 }
                                 else
                                 {
                                    this.i0 = this.i0 + 1;
                                    if(this.i0 != 0)
                                    {
                                       this.i1 = 0;
                                       this.i7 = this.i1;
                                       this.i1 = this.i12;
                                       continue;
                                    }
                                    this.i1 = 0;
                                    this.i0 = this.i11;
                                    this.i7 = this.i1;
                                    this.i1 = this.i12;
                                    break;
                                 }
                              }
                              else
                              {
                                 this.i1 = 1;
                              }
                              addr863:
                              this.i0 = this.i0 + 1;
                              if(this.i0 != 0)
                              {
                                 this.i11 = this.i1;
                                 this.i1 = this.i12;
                                 continue;
                              }
                              this.i0 = this.i1;
                              this.i1 = this.i12;
                              break;
                           }
                           if(this.i1 != 114)
                           {
                              if(this.i1 != 118)
                              {
                                 if(this.i1 == 122)
                                 {
                                    this.i0 = this.i0 + 1;
                                    if(this.i0 != 0)
                                    {
                                       this.i1 = 0;
                                       this.i11 = this.i1;
                                       this.i1 = this.i12;
                                       continue;
                                    }
                                    this.i0 = 0;
                                    this.i1 = this.i12;
                                    break;
                                 }
                              }
                              else
                              {
                                 this.i0 = this.i0 + 1;
                                 if(this.i0 != 0)
                                 {
                                    this.i1 = 0;
                                    this.i10 = this.i1;
                                    this.i1 = this.i12;
                                    continue;
                                 }
                                 this.i1 = 0;
                                 this.i0 = this.i11;
                                 this.i10 = this.i1;
                                 this.i1 = this.i12;
                                 break;
                              }
                           }
                           else
                           {
                              this.i0 = this.i0 + 1;
                              if(this.i0 != 0)
                              {
                                 this.i1 = 0;
                                 this.i8 = this.i1;
                                 this.i1 = this.i12;
                                 continue;
                              }
                              this.i1 = 0;
                              this.i0 = this.i11;
                              this.i8 = this.i1;
                              this.i1 = this.i12;
                              break;
                           }
                        }
                        this.i1 = this.i11;
                        §§goto(addr863);
                     }
                     si32(this.i1,FSM_pubrealloc);
                     si8(this.i7,FSM_pubrealloc);
                     si8(this.i8,FSM_pubrealloc);
                     si8(this.i9,FSM_pubrealloc);
                     si8(this.i10,FSM_pubrealloc);
                     si8(this.i0,FSM_pubrealloc);
                  }
                  §§goto(addr938);
               }
            case 3:
               this.i0 = mstate.eax;
               mstate.esp = mstate.esp + 4;
               si32(this.i0,FSM_pubrealloc);
               si32(this.i4,FSM_pubrealloc);
               this.i0 = 1;
               si8(this.i0,FSM_pubrealloc);
               §§goto(addr1180);
            case 4:
               mstate.esp = mstate.esp + 4;
               si32(this.i3,FSM_pubrealloc);
               this.i1 = this.i3;
               this.i0 = this.i1;
               this.i1 = this.i3;
               break;
            case 5:
               mstate.esp = mstate.esp + 4;
               si32(this.i3,FSM_pubrealloc);
               this.i1 = 2048;
               this.i0 = this.i3;
               break;
            case 6:
               this.i3 = mstate.eax;
               mstate.esp = mstate.esp + 4;
               this.i0 = this.i3 == 0?int(1):int(0);
               si32(this.i1,FSM_pubrealloc);
               this.i1 = this.i0 & 1;
               this.i0 = this.i1;
               this.i1 = this.i3;
               break;
            case 7:
               this.i2 = mstate.eax;
               mstate.esp = mstate.esp + 4;
               if(this.i2 == 0)
               {
                  this.i1 = this.i2;
               }
               else
               {
                  if(this.i0 != 0)
                  {
                     if(this.i3 != 0)
                     {
                        if(uint(this.i0) < uint(this.i3))
                        {
                           this.i3 = 0;
                           this.i4 = this.i2;
                           this.i5 = this.i1;
                           memcpy(this.i4,this.i5,this.i0);
                           mstate.esp = mstate.esp - 4;
                           si32(this.i1,mstate.esp);
                           mstate.esp = mstate.esp - 4;
                           FSM_pubrealloc.start();
                        }
                        else
                        {
                           this.i0 = this.i2;
                           this.i4 = this.i1;
                           memcpy(this.i0,this.i4,this.i3);
                        }
                     }
                  }
                  mstate.esp = mstate.esp - 4;
                  si32(this.i1,mstate.esp);
                  mstate.esp = mstate.esp - 4;
                  FSM_pubrealloc.start();
               }
               §§goto(addr2219);
            case 8:
               mstate.esp = mstate.esp + 4;
               this.i1 = this.i2 == 0?int(1):int(0);
               si32(this.i3,FSM_pubrealloc);
               this.i1 = this.i1 & 1;
               this.i0 = this.i1;
               this.i1 = this.i2;
               break;
            case 9:
               mstate.esp = mstate.esp + 4;
               this.i1 = this.i2;
               §§goto(addr2219);
         }
         if(this.i0 != 0)
         {
            this.i0 = 12;
            si32(this.i0,FSM_pubrealloc);
         }
         mstate.eax = this.i1;
         §§goto(addr2278);
      }
   }
}
