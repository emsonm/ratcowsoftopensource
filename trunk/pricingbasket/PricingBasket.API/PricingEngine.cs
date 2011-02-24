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

namespace PricingBasket.API
{

  using SKU;
  using Receipts;
  using Discounts;

  /// <summary>
  /// The pricing engine drives the checkout process.
  /// </summary>
  public class PricingEngine
  {

    StockKeepingUnits fSKUs = new StockKeepingUnits();
    DicsountUnits fDiscounts = new DicsountUnits();

    /// <summary>
    /// The PricingEngine drives the process of pricing the items
    /// in the basket
    /// </summary>
    public PricingEngine()
    {
      InitDefaults();

    }

    /// <summary>
    /// This inits the default SKU's for the engine
    ///   Soup   – 65p per tin
    ///   Bread  – 80p per loaf
    ///   Milk   – £1.30 per bottle
    ///   Apples – £1.00 per bag
    ///   
    /// Had I had slightly longer, I would have added routines to read this info from an XML file
    /// 
    /// </summary>
    private void InitDefaults()
    {
      StockKeepingUnit apples = new StockKeepingUnit("Apples", 1.00);
      fSKUs.Add(apples);

      StockKeepingUnit bread = new StockKeepingUnit("Bread", 0.80);
      fSKUs.Add(bread);

      StockKeepingUnit milk = new StockKeepingUnit(new StockKeepingUnit("Milk", 1.30));
      fSKUs.Add(milk);

      StockKeepingUnit soup = new StockKeepingUnit(new StockKeepingUnit("Soup", 0.65));
      fSKUs.Add(soup);

      //discounts
      fDiscounts.Add(new BuyItemsGetOneFreeDiscountUnit("Buy one pint of milk get one free", milk, 1));
      fDiscounts.Add(new BuyItemsLessPercentageDiscountUnit("Apples have a 10% discount", apples, 1, 10.0));
      fDiscounts.Add(new BuyItemsGetPercentageFromItemDiscountUnit("Buy 2 tins of soup and get a loaf of bread for half price", soup, bread, 2, 50.0));
    }

    /// <summary>
    /// The message we show when no input is given at command line
    /// </summary>
    public string HelpMessage()
    {
      StringBuilder result = new StringBuilder();

      result.AppendLine("PricingBasket - 20110223 Matt Emson\r\n");
      result.AppendLine("How to use:");
      result.AppendLine("\tPricingBasket <item> [<item>....]\r\n");
      
      result.AppendLine("Available items:");
      foreach (var item in fSKUs)
        result.AppendLine(String.Format("\t{0}", item.Name));

      result.AppendLine("\r\nToday's discounts:");
      foreach (var item in fDiscounts)
        result.AppendLine(String.Format("\t{0}", item.Name));



      return result.ToString();
    }


    /// <summary>
    /// 
    /// </summary>
    public Receipt CalculateReceipt(string[] items)
    {
      Receipt result = new Receipt(); //zero the result

      StockKeepingUnits itemSKUs = new StockKeepingUnits();

      foreach (var s in items)
      {
        StockKeepingUnit tempRef = fSKUs.FindSKU(s);
        if (tempRef != null)
          itemSKUs.Add(new StockKeepingUnit(tempRef));
        else
          result.AddRemark(String.Format("\"{0}\" was not recognised and skipped", s));
      }

      result.AddRemark(itemSKUs.ToString());

      //now that we know what we are buting, we should apply the discounts
      result.Subtotal = itemSKUs.Price();

      DiscountResults discountResults = fDiscounts.ApplyDiscounts(itemSKUs);

      result.Total = result.Subtotal - discountResults.Sum();
      result.AddDiscount(discountResults.ToString());

      return result;
    }

  }
}
