Public Class frm_close
    Dim obj As cls_sqlconnect
    Private Sub frm_close_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        obj = New cls_sqlconnect(conn)

        Dim tbl As DataTable = obj.get_less_used

        If obj.opr_flag = False Then
            Me.Close()
        Else
            For i = 0 To tbl.Rows.Count - 1
                lst.Items.Add(tbl.Rows(i)(0).ToString)
            Next

            If lst.Items.Count = 0 Then
                Me.Close()
            End If
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        For i = 0 To lst.Items.Count - 1
            lst.SetItemChecked(i, True)
        Next
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        For i = 0 To lst.Items.Count - 1
            lst.SetItemChecked(i, False)
        Next
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim strnn As String = ""
        For i = 0 To lst.Items.Count - 1
            If lst.GetItemChecked(i) = True Then
                lst.SelectedIndex = i
                obj.table_drop(lst.Text)
                ' strnn = strnn & vbNewLine & i & " " & lst.Text & " " & obj.opr_msg
            End If
        Next
        ' MsgBox(strnn)
        Me.Close()
    End Sub
End Class