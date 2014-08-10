Public Class frm_splash

    Private Sub frm_splash_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Me.BringToFront()
        ProgressBar1.Value += 10
        If ProgressBar1.Value = 20 Then
            lbl.Text = "Starting database connection"
            Try
                conn_open()
            Catch ex As Exception
                End
            End Try
            frmMain.obj = New cls_sqlconnect(conn)
            lbl.Text = "Checking Integrity"
        ElseIf ProgressBar1.Value = 40 Then

            frmMain.ref()
            lbl.Text = "Checking directory structure"
        ElseIf ProgressBar1.Value = 60 Then
            If My.Computer.FileSystem.DirectoryExists("D:\TMP") = True Then

                If My.Computer.FileSystem.DirectoryExists("D:\TMP\DATA") = False Then
                    My.Computer.FileSystem.CreateDirectory("D:\TMP\DATA")
                End If

                If My.Computer.FileSystem.DirectoryExists("D:\TMP\XML") = False Then
                    My.Computer.FileSystem.CreateDirectory("D:\TMP\XML")
                End If

                If My.Computer.FileSystem.DirectoryExists("D:\TMP\FMT") = False Then
                    My.Computer.FileSystem.CreateDirectory("D:\TMP\FMT")
                End If
            Else
                My.Computer.FileSystem.CreateDirectory("D:\TMP")
                My.Computer.FileSystem.CreateDirectory("D:\TMP\DATA")
                My.Computer.FileSystem.CreateDirectory("D:\TMP\XML")
                My.Computer.FileSystem.CreateDirectory("D:\TMP\FMT")
            End If
        ElseIf ProgressBar1.Value = 90 Then
            Timer1.Enabled = False
            frmMain.Enabled = True
            Me.Close()
        End If
    End Sub
End Class