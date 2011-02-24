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
  using SKU;

  /// <summary>
  /// This class represents a discount to a set of products.
  /// </summary>
  public class DiscountUnit
  {
    /// <summary>
    /// 
    /// </summary>
    public DiscountUnit(string discountName, StockKeepingUnit[] targeted, StockKeepingUnit[] discounted, double discount, int targetLevel)
    {
      Name = discountName;
      TargetItems = new StockKeepingUnits(targeted);
      DiscountedItems = new StockKeepingUnits(discounted);
      Discount = discount;
      TargetLevel = targetLevel;

    }

    public string Name { get; protected set; }

    /// <summary>
    /// A list of items that make up the discount
    /// </summary>
    public StockKeepingUnits TargetItems { get; protected set; }

    /// <summary>
    /// A list of items that will have the discount applied. Only
    /// one matching item from this list per discount will be applied.
    /// </summary>
    public StockKeepingUnits DiscountedItems { get; protected set; }

    /// <summary>
    /// The amount the discount is. The meaning of this property
    /// varies slightly depending on the type of discount
    /// </summary>
    public double Discount { get; protected set; }

    //This is used to configure the amount of items in the target that will trigger the discount
    public int TargetLevel { get; protected set; }

    /// <summary>
    /// 
    /// </summary>
    protected virtual bool CanApplyDiscount(StockKeepingUnits items)
    {
      return false;
    }

    /// <summary>
    /// 
    /// </summary>
    protected virtual double CalculatedDiscount(StockKeepingUnits items)
    {
      return 0.00;
    }

    /// <summary>
    /// Returns the discounted amount
    /// </summary>
    public virtual DiscountResult ApplyDiscount(StockKeepingUnits items)
    {
      double discount = 0.00;
      if (CanApplyDiscount(items))
      {
        discount = CalculatedDiscount(items);
        return new DiscountResult(Name, discount, true);
      }
      else
        return new DiscountResult();
      
    }
  }

  /// <summary>
  /// Buy target
  /// </summary>
  public class BuyItemsGetOneFreeDiscountUnit : DiscountUnit
  {

    public BuyItemsGetOneFreeDiscountUnit(string name, StockKeepingUnit target, int targetCount) :
      base(name, new StockKeepingUnit[] { target }, new StockKeepingUnit[] { target }, 0.00, targetCount)
    {

    }

    /// <summary>
    /// 
    /// </summary>
    protected override bool CanApplyDiscount(StockKeepingUnits items)
    {
      //basically, we want to know that the list contains at least the target and discountItem

      //The contract we have is that this class will only ever have a single target sku type... so 
      //the amount of items of the specific type to qualify is TargetLevel + 1

      var sku = items.FindSKUs(TargetItems[0].Name);
      int count = sku.Count;

      //int discountItems = ((count > TargetLevel) ? count / TargetLevel : 0);

      int targetValue = (TargetLevel + 1); //this is how many items we need for each discount to apply

      int discountItems = ((count > TargetLevel) ? count / targetValue : 0);

      return discountItems > 0;
    }

    /// <summary>
    /// 
    /// </summary>
    protected override double CalculatedDiscount(StockKeepingUnits items)
    {
      //This is inefficient. I think I could have made this in to a more streamlines routine, but keeping the logic
      //in two chunks will simplify the code readibility.
      var sku = items.FindSKUs(TargetItems[0].Name);
      int count = sku.Count;

      int targetValue = (TargetLevel + 1); //this is how many items we need for each discount to apply

      int discountItems = ((count > TargetLevel) ? count / targetValue : 0);

      return (TargetItems[0].Price * discountItems);
    }

  }


  /// <summary>
  /// Buy target at x% of usual price
  /// </summary>
  public class BuyItemsLessPercentageDiscountUnit : DiscountUnit
  {

    public BuyItemsLessPercentageDiscountUnit(string name, StockKeepingUnit target, int targetCount, double percentage) :
      base(name, new StockKeepingUnit[] { target }, new StockKeepingUnit[] { target }, percentage, targetCount)
    {

    }

    /// <summary>
    /// 
    /// </summary>
    protected override bool CanApplyDiscount(StockKeepingUnits items)
    {
      //basically, we want to know that the list contains at least the target and discountItem

      //The contract we have is that this class will only ever have a single target sku type... so 
      //the amount of items of the specific type to qualify is TargetLevel + 1

      var sku = items.FindSKUs(TargetItems[0].Name);
      int discountItems = sku.Count;

      return discountItems > 0;
    }

    /// <summary>
    /// 
    /// </summary>
    protected override double CalculatedDiscount(StockKeepingUnits items)
    {
      //This is inefficient. I think I could have made this in to a more streamlines routine, but keeping the logic
      //in two chunks will simplify the code readibility.
      var sku = items.FindSKUs(TargetItems[0].Name);
      int discountItems = sku.Count;

      //we need to calculate how many items we need to qualify...
      if (TargetLevel == 1)
      {
        //discount applied for each item
        double actualPrice = (TargetItems[0].Price * discountItems); //what we should charge
        double actualDiscount = (actualPrice * (Discount / 100));
        return actualDiscount;
      }
      else
      {
        //we need more than one item to get the discount
        int qualifyingCount = (discountItems / (TargetLevel + 1));
        double actualPrice = (TargetItems[0].Price * qualifyingCount); //what we should charge
        double actualDiscount = (actualPrice * (Discount / 100));
        return actualDiscount;
      }

      
    }

  }


  /// <summary>
  /// Buy target(s) and get item at x% of usual price
  /// </summary>
  public class BuyItemsGetPercentageFromItemDiscountUnit : DiscountUnit
  {

    public BuyItemsGetPercentageFromItemDiscountUnit(string name, StockKeepingUnit target, StockKeepingUnit discounted, int targetCount, double percentage) :
      base(name, new StockKeepingUnit[] { target }, new StockKeepingUnit[] { discounted }, percentage, targetCount)
    {

    }

    /// <summary>
    /// 
    /// </summary>
    protected override bool CanApplyDiscount(StockKeepingUnits items)
    {
      //basically, we want to know that the list contains at least the target and discountItem

      //The contract we have is that this class will only ever have a single target sku type... so 
      //the amount of items of the specific type to qualify is TargetLevel + 1

      var sku = items.FindSKUs(TargetItems[0].Name);
      int targetItems = sku.Count;  //((targetItems > TargetLevel) ? targetItems / 2 : 0);
      sku = items.FindSKUs(DiscountedItems[0].Name);
      int discountItem = sku.Count;

      return (targetItems >= TargetLevel /* we have at least enough items to meet our quota*/ &&
              discountItem > 0 /* we have at least one item to discount */); 
    }

    /// <summary>
    /// 
    /// </summary>
    protected override double CalculatedDiscount(StockKeepingUnits items)
    {
      //This is inefficient. I think I could have made this in to a more streamlines routine, but keeping the logic
      //in two chunks will simplify the code readibility.
      
      var sku = items.FindSKUs(TargetItems[0].Name);
      int targetItems = sku.Count;  //((targetItems > TargetLevel) ? targetItems / 2 : 0);
      
      sku = items.FindSKUs(DiscountedItems[0].Name);
      int discountItem = sku.Count;

      //so, we need to know how many discounts we can apply
      int totalDiscountableItems = (targetItems > 0 ? targetItems / TargetLevel : 0);

      int numberOfActualDiscountItems = (totalDiscountableItems > discountItem ? discountItem : totalDiscountableItems);

      double actualPrice = (DiscountedItems[0].Price * numberOfActualDiscountItems); //what we should charge
      double actualDiscount = (actualPrice * (Discount / 100));
      return actualDiscount;
    }

  }
}
