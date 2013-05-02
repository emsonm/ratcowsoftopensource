using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RatCow.BACS
{
  /// <summary>
  /// If these are excluded, this library can be build with no dependency on Winforms
  /// </summary>
  public class UIHelpers
  {
    /// <summary>
    /// Create the ListViewItem contents
    /// </summary>
    public static void CreateAddacsItem( System.Windows.Forms.ListViewItem eitem, int index, List<Formats.ADDACS.MessagingAdvice> data )
    {
      //ADDACS
      Formats.ADDACS.MessagingAdvice item = data[ index ];

      eitem.Text = item.reference;
      eitem.SubItems.Add( item.payername );
      eitem.SubItems.Add( item.payersortcode );
      eitem.SubItems.Add( item.payeraccountnumber );
      eitem.SubItems.Add( item.recordtype );
      eitem.SubItems.Add( item.effectivedate );
      eitem.SubItems.Add( Processing.DecodeADDACSReasonCode( item.reasoncode ) );
      eitem.SubItems.Add( item.reasoncode );
      eitem.SubItems.Add( item.payernewsortcode );
      eitem.SubItems.Add( item.payernewaccountnumber );
    }

    /// <summary>
    /// Create the ListViewItem contents
    /// </summary>
    public static void CreateAruddItem( System.Windows.Forms.ListViewItem eitem, int index, List<Formats.ARUDD.ReturnedDebitItem> data )
    {
      Formats.ARUDD.ReturnedDebitItem item = data[ index ];

      eitem.Text = item.@ref;
      eitem.SubItems.Add( item.PayerAccount.name );
      eitem.SubItems.Add( item.returnDescription );
      eitem.SubItems.Add( item.returnCode );
      eitem.SubItems.Add( item.transCode );
      eitem.SubItems.Add( item.originalProcessingDate );
      eitem.SubItems.Add( item.valueOf.ToString( "C" ) ); //as we are dealing with the UK here
      eitem.SubItems.Add( item.PayerAccount.@ref );
      eitem.SubItems.Add( item.PayerAccount.sortCode );
      eitem.SubItems.Add( item.PayerAccount.number );
      eitem.SubItems.Add( item.PayerAccount.bankName );
      eitem.SubItems.Add( item.PayerAccount.branchName );
    }
  }
}