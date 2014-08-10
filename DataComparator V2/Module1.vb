Imports System.Data
Imports System.Data.SqlClient
Module Module1
    Public conn As SqlConnection
    Public is_close As Boolean = True
    Dim fln As String
    Public Function conn_open() As Boolean
        conn = New SqlConnection("Data Source=.\SQLEXPRESS;AttachDbFilename=" & Application.StartupPath & "\LocalDB.mdf;Integrated Security=True;User Instance=True")
        'conn = New SqlConnection("Server=(localdb)\v11.0;Integrated Security=true;AttachDbFileName=" & Application.StartupPath & "\LocalDB.mdf;Connect Timeout=30")
        Try
            conn.Open()
            Return True
        Catch ex As Exception
            MsgBox("Error in connecting to database: " & ex.Message)
            ' End

            Return False
        End Try
    End Function


    Public Sub conn_close()
        Try
            conn.Close()
        Catch ex As Exception

        End Try
    End Sub

    Public Sub add_flconnection(ByVal fl As String, Optional ByVal ex_mode As String = "FILE")
        fln = fl
        add_conn_start(ex_mode)
    End Sub
    Private Sub add_conn_start(ByVal md As String)
        Dim frm As New frm_add(fln)

        frm.Show()
        For i = 0 To 10
            frm.BringToFront()
        Next


        frm.read_thread_start()
    End Sub
End Module
