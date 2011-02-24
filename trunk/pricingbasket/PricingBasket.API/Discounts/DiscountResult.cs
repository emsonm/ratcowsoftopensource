/*
 * Copyright 2011 Rat Cow Software and Matt Emson. All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without modification, are
 * permitted provided that the following conditions are met:
 * 
 * 1. Redistributions of source code must retain the above copyright notice, this list of
 *    conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright notice, this list
 *    of conditions and the following disclaimer in the documentation and/or other materials
 *    provided with the distribution.
 * 3. Neither the name of the Rat Cow Software nor the names of its contributors may be used 
 *    to endorse or promote products derived from this software without specific prior written 
 *    permission.
 *    
 * THIS SOFTWARE IS PROVIDED BY RAT COW SOFTWARE "AS IS" AND ANY EXPRESS OR IMPLIED
 * WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
 * FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> OR
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
 * ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
 * NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
 * ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 * The views and conclusions contained in the software and documentation are those of the
 * authors and should not be interpreted as representing official policies, either expressed
 * or implied, of Rat Cow Software and Matt Emson.
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PricingBasket.API.Discounts
{
  /// <summary>
  /// This class represents the result of a discount. If the discount is
  /// zero, the Applied flag is set to false.
  /// </summary>
  public class DiscountResult
  {
    public DiscountResult(string description, double discount, bool applied)
    {
      Description = description;
      Discount = discount;
      Applied = applied;
    }

    /// <summary>
    /// Default constructor gives a negative result. I might well move this whole 
    /// choice to a factory with more development time.
    /// </summary>
    public DiscountResult()
      : this(String.Empty, 0.00, false)
    {
    }

    public string Description { get; protected set; }
    public double Discount { get; protected set; }
    public bool Applied { get; protected set; }

  }
}
