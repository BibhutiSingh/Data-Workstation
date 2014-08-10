Public Class frm_edit_datatype

    Private Sub frm_edit_datatype_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        frm_import.Enabled = True
    End Sub

    Private Sub frm_edit_datatype_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub cmd_edit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_edit.Click
        Dim lent As Integer = Val(TextBox1.Text)

        If ComboBox1.Text <> "Date" And lent = 0 Then
            MsgBox("Size can't be 0")
            Exit Sub

        End If
        Dim tmp As Integer = frm_import.ListBox2.SelectedIndex
        frm_import.ListBox2.Items.RemoveAt(tmp)
        If lent = 0 Then
            frm_import.ListBox2.Items.Insert(tmp, ComboBox1.Text)
        Else
            frm_import.ListBox2.Items.Insert(tmp, ComboBox1.Text & "(" & lent.ToString & ")")
        End If

        Me.Close()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.Text = "Date" Then
            TextBox1.Text = ""
        End If
    End Sub
End Class