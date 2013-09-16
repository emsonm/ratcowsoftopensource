using System;

using sharpallegro;

namespace exfixed
{
  unsafe class extimer : Allegro
  {
    static int Main()
    {   /* declare three 32 bit (16.16) fixed point variables */
      int x, y, z;

      if (allegro_init() != 0)
        return 1;

      /* convert integers to fixed point like this */
      x = itofix(10);

      /* convert floating point to fixed point like this */
      y = ftofix(3.14);

      /* fixed point variables can be assigned, added, subtracted, negated,
       * and compared just like integers, eg: 
       */
      z = x + y;
      allegro_message(fixtof(x) + " + " + fixtof(y) + " = " + fixtof(z));

      /* you can't add integers or floating point to fixed point, though:
       *    z = x + 3;
       * would give the wrong result.
       */

      /* fixed point variables can be multiplied or divided by integers or
       * floating point numbers, eg:
       */
      z = y * 2;
      allegro_message(fixtof(y) + " * 2 = " + fixtof(z));

      /* you can't multiply or divide two fixed point numbers, though:
       *    z = x * y;
       * would give the wrong result. Use fixmul() and fixdiv() instead, eg:
       */
      z = fixmul(x, y);
      allegro_message(fixtof(x) + " * " + fixtof(y) + " = " + fixtof(z));

      /* fixed point trig and square root are also available, eg: */
      z = fixsqrt(x);
      allegro_message("fixsqrt(" + fixtof(x) + ") = " + fixtof(z));

      return 0;
    }
  }
}
