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

namespace PricingBasket.API.Receipts
{

  /// <summary>
  /// Data container for results.
  /// </summary>
  public class Receipt
  {
    public Receipt()
    {
      Subtotal = 0.00;
      Total = 0.00;
    }

    //exception reporting
    private StringBuilder fremarks = new StringBuilder();
    public string Remarks { get { return fremarks.ToString(); } }
    public void AddRemark(string remark)
    {
      fremarks.AppendLine(remark);
    }

    //discount reporting
    private StringBuilder fdiscount = new StringBuilder();
    public string Discount { get { return (fdiscount.Length == 0 ? "No discount" : fdiscount.ToString()); } }
    public void AddDiscount(string discount)
    {
      if (fdiscount.Length == 0)
      {
        fdiscount.AppendLine(discount);
      }
      else
      {
        fdiscount.AppendFormat(", {0}", discount);
      }
    }

    public double Subtotal { get; set; }
    public double Total { get; set; }

    /// <summary>
    /// Simple formatted output
    /// </summary>
    public override string ToString()
    {
      StringBuilder result = new StringBuilder();

      //we put any additional dialogue here
      result.AppendLine(fremarks.ToString());

      result.AppendFormat("Subtotal : £{0} \r\n\r\n", Subtotal.ToString("0.00"));
      result.AppendLine(Discount);
      result.AppendFormat("Total : £{0}\r\n", Total.ToString("0.00"));

      return result.ToString();
    }
  }
}
                                                