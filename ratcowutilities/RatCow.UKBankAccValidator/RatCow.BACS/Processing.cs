using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace RatCow.BACS
{
  public class Processing
  {
    /// <summary>
    /// Give some textual explanation of the reson code.
    /// </summary>
    public static string DecodeADDACSReasonCode( string reasoncode )
    {
      switch ( reasoncode )
      {
        //0 - Instruction cancelled - Refer to Payer
        //Reason: Instruction cancelled - Refer to Payer
        //Circumstances: Paying Bank has cancelled the instruction
        //Special Instruction / Information: Originator cannot collect via Direct Debit on this account. If Direct Debit is to continue the Originator must obtain a new DDI for a new account
        //Action Taken by DataCash: The DDI is marked as cancelled at source
        case "0":
          return "Instruction cancelled - Refer to Payer";

        //1 - Instruction cancelled by Payer
        //Reason: Instruction cancelled by Payer
        //Circumstances: Payer has instructed Paying Bank to cancel DDI
        //Special Instruction / Information: Originator must liase with the Payer to agree the payment method for collection of any outstanding debts
        //Action Taken by DataCash: The DDI is marked as cancelled at source
        case "1":
          return "Instruction cancelled by Payer";

        //2 - Payer deceased
        //Reason: Payer deceased
        //Action Taken by DataCash: The DDI is marked as cancelled at source
        case "2":
          return "Payer deceased";

        //3 - Instruction cancelled account transferred to another Bank / Building Society
        //Reason: Instruction cancelled account transferred to another Bank / Building Society
        //Circumstances: Account transferred to another Bank / Building Society
        //Special Instruction / Information: New DDI to be obtained from Payer. Collection must be suspended until new DDI set-up and Advance Notice issued to Payer
        //Action Taken by DataCash: DDI is marked as cancelled at source
        case "3":
          return "Instruction cancelled account transferred to another Bank / Building Society";

        //B - Account Closed
        //Reason: Account Closed
        //Circumstances: Payer has closed their account for an unknown reason
        //Special Instruction / Information: If the Direct Debit is to continue the Originator must obtain a new DDI for a different / new account
        //Action Taken by DataCash: The DDI is marked as cancelled at source
        case "B":
          return "Account Closed";

        //C - Account transferred to another Bank/Building Society
        //Reason: Account transferred to another Bank/Building Society
        //Circumstances: New account details supplied to the Originator
        //Special Instruction / Information: Originator must apply change to data file and continue with Direct Debit collections. An 0C/0N (0C - cancelled DDI and 0N - new or re-instated DDI) transaction code pair must not be sent on receipt of this message
        //Action Taken by DataCash: The DDI is updated with the new details as provided in the advice
        case "C":
          return "Account transferred to another Bank/Building Society";

        //D - Advance Notice disputed
        //Reason: Advance Notice disputed
        //Circumstances: Payer disputes time, amount or frequency of Advance Notice
        //Special Instruction / Information: Originator should not collect further Direct Debits until it has resolved the dispute with the Payer
        //Action Taken by DataCash: The DDI is cancelled. It will be re-opened later if advice R - Instruction re-instated is received
        case "D":
          return "Advance Notice disputed";

        //E - Instruction amended
        //Reason: Instruction amended
        //Circumstances: Paying Bank will advise amendment via ADDACS message
        //Special Instruction / Information: Originator should collect Direct Debits using new details An 0C/0N (0C - cancelled DDI and 0N - new or re-instated DDI) transaction code pair must not be sent on receipt of this message
        //Action Taken by DataCash: The DDI is updated with the new details as provided in the advice
        case "E":
          return "Instruction amended";

        //L - Incorrect payer's Account Details
        //Reason: Incorrect payer's Account Details
        //Circumstances: Either:
        //                                           • the sort code / account number has failed the modulus check
        //                                           • the sort code does not exist
        //                                           • the account number is not all numeric or is all zeros
        //                                           • the account type is invalid.R - Instruction re-instated
        //Special Instruction / Information: Service user should undertake sort code validation and modulus checking prior to sending the DDI transactions to Bacs or, if already doing this, should ensure that they are using the latest version
        case "L":
          return "Incorrect payer's Account Details";

        //R - Instruction re-instated
        //Reason: Instruction re-instated
        //Circumstances: Paying Bank may re-instate a cancelled DD up to two months from cancellation
        //Special Instruction / Information: Originator may resume direct debiting under the reinstated Instruction. However, a new DDI must be obtained and lodged if re-instatement is identified after the two month period
        //Action Taken by DataCash: The DDI is updated to active DDI, to enable further payments
        case "R":
          return "Instruction re-instated";

        default:
          return "Unknown code";
      }
    }

    /// <summary>
    /// This is a very rough first draft that needs cleaning up a lot.
    /// </summary>
    public static List<RatCow.BACS.Formats.ADDACS.MessagingAdvice> ProcessAddacsFiles( string path, DateTime fromDate, DateTime toDate )
    {
      var directory = new System.IO.DirectoryInfo( path );

      var addacsData = new List<RatCow.BACS.Formats.ADDACS.MessagingAdvice>();

      //does the file time stamp fall in between our criteria?
      var files = ( from f in directory.GetFiles()
                    where f.Extension == ".xml" && ( f.LastWriteTime > fromDate && f.LastWriteTime <= toDate )
                    orderby f.LastWriteTime
                    select f );

      foreach ( var file in files )
      {
        //we read the contents of the file
        try
        {
          var doc = XElement.Load( file.FullName );
          var serializer = new XmlSerializer( typeof( RatCow.BACS.Formats.ADDACS.MessagingAdvice ) );

          var datas = from element in doc.Elements( "Data" )
                      select element;

          foreach ( XElement data in datas.ToArray() )
          {
            var messageAdvices = from element in data.Elements( "MessagingAdvices" )
                                 select element;
            // Read the entire XML
            foreach ( XElement subItems in messageAdvices )
            {
              var messageAdviceItems = from element in subItems.Elements( "MessagingAdvice" )
                                       select element;
              foreach ( XElement messageAdvice in messageAdviceItems )
              {
                using ( var stream = new System.IO.MemoryStream() )
                {
                  var writer = new System.IO.StreamWriter( stream );
                  writer.Write( messageAdvice.ToString() );
                  writer.Flush();

                  stream.Seek( 0, System.IO.SeekOrigin.Begin );
                  try
                  {
                    var item = (RatCow.BACS.Formats.ADDACS.MessagingAdvice)serializer.Deserialize( stream );
                    addacsData.Add( item );
                  }
                  catch
                  {
                    //if there's a deserialisation issue here, we want to skip it for now
                    //but really, we should log issues at a later date
                  }
                }
              }
            }
          }
        }
        catch
        {
          //if we fail here, we probably didn't load the file correctly - so we need
          //to handle that better at some point
        }
      }

      return addacsData;
    }

    /// <summary>
    /// This is a very rough first draft that needs cleaning up a lot.
    /// </summary>
    public static List<RatCow.BACS.Formats.ARUDD.ReturnedDebitItem> ProcessAruddFiles( string path, DateTime fromDate, DateTime toDate )
    {
      var directory = new System.IO.DirectoryInfo( path );

      var aruddData = new List<RatCow.BACS.Formats.ARUDD.ReturnedDebitItem>();

      //does the file time stamp fall in between our criteria?
      var files = ( from f in directory.GetFiles()
                    where f.Extension == ".xml" && ( f.LastWriteTime > fromDate && f.LastWriteTime <= toDate )
                    orderby f.LastWriteTime
                    select f );

      foreach ( var file in files )
      {
        //we read the contents of the file
        try
        {
          var doc = XElement.Load( file.FullName );
          var serializer = new XmlSerializer( typeof( RatCow.BACS.Formats.ARUDD.ReturnedDebitItem ) );

          var datas = from element in doc.Elements( "Data" )
                      select element;

          foreach ( XElement data in datas.ToArray() )
          {
            var arrudItems = from element in data.Elements( "ARUDD" )
                             select element;
            // Read the entire XML
            foreach ( XElement arrudItem in arrudItems )
            {
              var adviceItems = from element in arrudItem.Elements( "Advice" )
                                select element;
              foreach ( XElement adviceItem in adviceItems )
              {
                var originatingAccountRecords = from element in adviceItem.Elements( "OriginatingAccountRecords" ).Elements( "OriginatingAccountRecord" )
                                                select element;
                foreach ( XElement originatingAccountRecordsItems in originatingAccountRecords )
                {
                  var returnedDebitItems = from element in originatingAccountRecordsItems.Elements( "ReturnedDebitItem" )
                                           select element;
                  foreach ( XElement returnedDebitItem in returnedDebitItems )
                  {
                    using ( var stream = new System.IO.MemoryStream() )
                    {
                      var writer = new System.IO.StreamWriter( stream );
                      writer.Write( returnedDebitItem.ToString() );
                      writer.Flush();

                      stream.Seek( 0, System.IO.SeekOrigin.Begin );
                      try
                      {
                        var item = (RatCow.BACS.Formats.ARUDD.ReturnedDebitItem)serializer.Deserialize( stream );
                        aruddData.Add( item );
                      }
                      catch
                      {
                      }
                    }
                  }
                }
              }
            }
          }
        }
        catch
        {
        }
      }
      return aruddData;
    }

    /// <summary>
    /// This is used below
    /// </summary>
    public delegate string FormatItem<T>( T item );

    /// <summary>
    /// generic part.
    ///
    /// usage:
    ///
    /// Processing.ExportList<Formats.ADDACS.MessagingAdvice>( filename, data, Processing.FormatAddacsItem );
    /// </summary>
    public static void ExportList<T>( string filename, List<T> data, FormatItem<T> formatter )
    {
      //create the file
      //process the files
      using ( var file = System.IO.File.CreateText( filename ) )
      {
        foreach ( var item in data )
        {
          var line = formatter( item );
          file.WriteLine( line );
        }
        file.Close();
      }
    }

    /// <summary>
    /// This would probably be better done in a more generic fashion...
    /// </summary>
    public static string FormatAddacsItem( Formats.ADDACS.MessagingAdvice item )
    {
      var result = String.Format( "{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}",
        item.reference,
        item.payername,
        item.payersortcode,
        item.payeraccountnumber,
        item.recordtype,
        item.effectivedate,
        DecodeADDACSReasonCode( item.reasoncode ),
        item.reasoncode,
        item.payernewsortcode,
        item.payernewaccountnumber );

      return result;
    }

    /// <summary>
    /// This would probably be better done in a more generic fashion...
    /// </summary>
    public static string FormatAruddItem( Formats.ARUDD.ReturnedDebitItem item )
    {
      var result = String.Format( "{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}",
        item.@ref,
        item.PayerAccount.name,
        item.returnDescription,
        item.returnCode,
        item.transCode,
        item.originalProcessingDate,
        item.valueOf.ToString( "C" ),
        item.PayerAccount.@ref,
        item.PayerAccount.sortCode,
        item.PayerAccount.number,
        item.PayerAccount.bankName,
        item.PayerAccount.branchName );

      return result;
    }
  }
}