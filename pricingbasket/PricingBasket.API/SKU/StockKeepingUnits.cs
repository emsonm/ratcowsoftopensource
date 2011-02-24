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

namespace PricingBasket.API.SKU
{
  /// <summary>
  /// Simple list of SKU
  /// </summary>
  public class StockKeepingUnits: List<StockKeepingUnit>
  {
    public StockKeepingUnits()
      : base()
    {
    }

    public StockKeepingUnits(IEnumerable<StockKeepingUnit> collection) :
      base(collection)
    {
    }

    /// <summary>
    /// This prices the list of SKUs in the list.
    /// </summary>
    /// <returns></returns>
    public double Price()
    {
      double result = 0.00;

      foreach (var sku in this)
      {
        result += sku.Price;
      }

      return result;
    }

    /// <summary>
    /// This looks for a specific set SKUs
    /// 
    /// match incoming string names to SKUs
    /// </summary>
    public  StockKeepingUnits FindSKUs(string skuname)
    {
      StockKeepingUnits result = new StockKeepingUnits();

      var foundSKUs = from sku in this
                      where sku.Name.ToLower() == skuname.ToLower()
                      select sku;


      foreach (StockKeepingUnit item in foundSKUs)
        result.Add(new StockKeepingUnit(item));

      return result;
    }


    /// <summary>
    /// This looks for a specific SKU in the list
    /// 
    /// match incoming string names to SKUs
    /// </summary>
    public StockKeepingUnit FindSKU(string skuname)
    {
      var foundSKUs = from sku in this
                      where sku.Name.ToLower() == skuname.ToLower()
                      select sku;

      //here we really only want one item... so we double check
      int count = foundSKUs.Count<StockKeepingUnit>();
      if (count > 0 && count <= 1)
      {
        return foundSKUs.First<StockKeepingUnit>(); //return the first item
      }
      else
        return null;
    }


    /// <summary>
    /// We roll out the bill
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
      StringBuilder result = new StringBuilder();

      //get the till receipt - tally of what we have purchased.
      var bill = from sku in this
                 group sku by sku.Name into g
                 select new { Name = g.Min(c => c.Name), Price = g.Min(c => c.Price), Count = g.Count(), TotalPrice = g.Sum(c => c.Price) };


      foreach (var item in bill)
      {
        result.AppendLine(String.Format("{0}\tx {1}\t@ £{2}\t: £{3}", item.Name, item.Count, item.Price.ToString("0.00"), item.TotalPrice.ToString("0.00")));
      }

      //foreach (StockKeepingUnit item in foundSKUs)
      //  result.Add(new StockKeepingUnit(item));
      return result.ToString();
    }
  }
}
