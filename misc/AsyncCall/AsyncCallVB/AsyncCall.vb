Imports System.Runtime.Remoting.Messaging

''' <summary>
''' Make an async call look like a sync call
''' </summary>
Public Class SyncAsyncAction(Of Args)

#Region "Fields and types"

    ' Fields
    Private AsyncActionHandler As SyncAsyncActionHandlerDelegate(Of Args)

    ' Nested Types
    Public Delegate Sub SyncAsyncActionHandlerDelegate(Of MArgs)(ByVal args As MArgs)

#End Region

#Region "Constructor"

    Sub New(resultHandler As SyncAsyncAction(Of Args).SyncAsyncActionHandlerDelegate(Of Args))
        Me.AsyncActionHandler = resultHandler
    End Sub

#End Region

#Region "Shared call wrapper"

    ''' <summary>
    ''' Call this to make an sync call from an async action
    ''' </summary>
    Public Shared Sub CallAction(Of CArgs)(ByVal callback As SyncAsyncAction(Of CArgs).SyncAsyncActionHandlerDelegate(Of CArgs), ByVal args As CArgs)
        Try
            Dim caller As New SyncAsyncAction(Of CArgs)(callback)
            caller.CallAction(caller, args)
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
            Debug.WriteLine(ex.StackTrace)
            Throw
        End Try
    End Sub

    ''' <summary>
    ''' Call this to make an async call
    ''' </summary>
    Public Shared Sub CallActionAsync(Of CArgs)(ByVal callback As SyncAsyncAction(Of CArgs).SyncAsyncActionHandlerDelegate(Of CArgs), ByVal args As CArgs)
        Try
            Dim caller As New SyncAsyncAction(Of CArgs)(callback)
            caller.CallActionAsync(caller, args)
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
            Debug.WriteLine(ex.StackTrace)
            Throw
        End Try
    End Sub

#End Region

#Region "Instance call"

    ''' <summary>
    ''' Sync call
    ''' </summary>
    Public Sub CallAction(ByVal sender As Object, ByVal args As Args)
        CallAction(sender, args, True)
    End Sub

    ''' <summary>
    ''' Async call
    ''' </summary>
    Public Sub CallActionAsync(ByVal sender As Object, ByVal args As Args)
        CallAction(sender, args, False)
    End Sub

    ''' <summary>
    ''' Will either call async or sync, depending on what the caller specified.
    ''' </summary>
    Private Sub CallAction(ByVal sender As Object, ByVal args As Args, sync As Boolean)
        Dim mcall As IAsyncResult = Me.AsyncActionHandler.BeginInvoke( _
            args, _
            Function(ar As IAsyncResult)
                Dim aresult As AsyncResult = DirectCast(ar, AsyncResult)
                DirectCast(aresult.AsyncDelegate, SyncAsyncActionHandlerDelegate(Of Args)).EndInvoke(aresult)
                Return Nothing
            End Function, sender)

        If (sync) Then
            mcall.AsyncWaitHandle.WaitOne() ''make the call synchronous
        End If
    End Sub

#End Region

End Class


