using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Messaging;

namespace AsyncCall
{

    //synchranous call
    public class AsyncAction<Args>
    {
        //Constructor
        public AsyncAction(AsyncActionResultDelegate<Args> resultHandler)
        {
            AsyncActionResult = resultHandler;
        }

        //delegate to handle the call
        public delegate void AsyncActionResultDelegate<MArgs>(MArgs args);

        //despatcher
        public void CallAsyncAction(object sender, Args args)
        {
            IAsyncResult result = AsyncActionResult.BeginInvoke(
                args,
                new AsyncCallback(                   
                    delegate(IAsyncResult ar)
                    {
                        AsyncResult aresult = (AsyncResult)ar;
                        AsyncActionResultDelegate<Args> caller = (AsyncActionResultDelegate<Args>)aresult.AsyncDelegate;
                       
                        caller.EndInvoke(aresult);
                    }

                    ),
                    sender
                );

            //wait for the action to complete
            result.AsyncWaitHandle.WaitOne();
        }

        //internal callback
        private AsyncActionResultDelegate<Args> AsyncActionResult = null;

        /// <summary>
        /// Nice easy static method
        /// </summary>
        public static void CallAsync<CArgs>(AsyncAction<CArgs>.AsyncActionResultDelegate<CArgs> callback, CArgs args)
        {
            try
            {
                var caller = new AsyncAction<CArgs>(callback);
                caller.CallAsyncAction(caller, args);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                throw;
            }
        }
    }
}
