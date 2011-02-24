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
  /// This class represents a specific item in the store.
  /// </summary>
  public class StockKeepingUnit
  {
    /// <summary>
    /// Default constructor for Reflection
    /// </summary>
    public StockKeepingUnit()
    {
      //This is generally only going to be used for reflection and so I'd
      //not add in default values here - the issue with doing that is that
      //you'd get a double allocation to the reference and give extra toll
      //on the garbage collector - when reflecting, we know the properties
      //are going to be filled at the next stage.
  
      //An alternative would be adding ": this(String.Empty, String.Empty, 0.00)
   
    }

    /// <summary>
    /// This is a simplified constructor. 
    /// This exists more to simplify the code. If I were to expland this
    /// project in to a fuller system with SKU numbers rather than a name, 
    /// I'd more than likely opt for the fuller constructor. 
    /// </summary>
    public StockKeepingUnit(string name, double price) :
      this (name, name, price)
    {
    }

    /// <summary>
    /// This constructor sets all of the property values.
    /// As the property values are read only, we are unable to
    /// alter them after construction.
    /// </summary>
    public StockKeepingUnit(string name, string description, double price)
    {
      Name = name;
      Description = description;
      Price = price;
    }

    /// <summary>
    /// Clone constructor, else we get issues with shared references
    /// </summary>
    /// <param name="clone"></param>
    public StockKeepingUnit(StockKeepingUnit clone)
      : this(clone.Name, clone.Description, clone.Price)
    {
    }


    //The name of the stock item - as provided to us via input
    //This would be the a single word or space less string 
    //(with in the confines of te assignment)
    //In reality, I would probably have gone for an actual
    //SKU style ID here....
    public string Name { get; protected set; }

    //The name we would like to present to the user -
    //This is meant to be a more plain text description.
    public string Description { get; protected set; }

    //The base price. This would be the RRP.
    public double Price { get; protected set; }

  }
}
