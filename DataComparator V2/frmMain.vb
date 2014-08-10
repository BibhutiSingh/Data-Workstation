Public Class frmMain
    Public obj As cls_sqlconnect
    Dim dtb As DataTable

    Dim lst As List(Of TabPage)
    Dim curr As TabPage

    Dim is_speepup As Boolean


    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        frm_splash.Show()
        Me.Enabled = False
        'Try
        '    If conn_open() = False Then

        '        End
        '    End If
        'Catch ex As Exception
        '    End
        'End Try
        SplitContainer1.SplitterDistance = 0.2 * Me.Width

        lst = New List(Of TabPage)

        ListBox1.Width = SplitContainer1.Panel1.Width - 20
        TabControl1.Width = SplitContainer1.Panel2.Width - 10

        ' obj = New cls_sqlconnect(conn)

        
        ' ref()


        Button1_Click(Nothing, Nothing)

    End Sub

    Public Sub ref()
        dtb = New DataTable
        dtb = obj.get_all_table
        ListBox1.DataSource = dtb
        ListBox1.DisplayMember = "TABLE_NAME"
    End Sub

    Private Sub frmMain_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        TabControl1.Width = SplitContainer1.Panel2.Width
        TabControl1.Height = SplitContainer1.Panel2.Height - (TabControl1.Top + 30)
    End Sub

    Private Sub ToolStripButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton1.Click
        Me.Enabled = False
        frm_import.Show()
    End Sub

    Private Sub MenuStrip1_ItemClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles MenuStrip1.ItemClicked

    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        If TabControl1.TabPages.Count = 0 Then
            Exit Sub
        End If
        While TabControl1.TabPages.Count <> 0
            Try
                CType(TabControl1.TabPages(0).Controls.Item(0), SqlWorkBook).Exit_Run()
                TabControl1.TabPages(0).Dispose()

            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End While
        lst.Clear()


    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim tabb As New TabPage
        tabb.Name = "WorkBook_" & (lst.Count + 1).ToString
        tabb.Text = "WorkBook " & (lst.Count + 1).ToString
        tabb.Size = TabControl1.Size

        Dim wrkk As New SqlWorkBook(is_speepup)

        wrkk.Width = tabb.Width - 10
        wrkk.Height = tabb.Height - 30
        wrkk.Speed_Up = CheckBox1.Checked
        tabb.Controls.Add(wrkk)


        lst.Add(tabb)
        TabControl1.TabPages.Add(lst.Item(lst.Count - 1))
        curr = tabb
        TabControl1.SelectedTab = curr
    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = True Then
            is_speepup = True
        Else
            is_speepup = False
        End If
        If CheckBox1.Checked = True Then
            MsgBox("Speed up query will speed up query return time but heavily depends on your system hardware. We don't recommend it for querying large files (>50 MB).", MsgBoxStyle.Information)


        End If

        If TabControl1.TabPages.Count = 0 Then
            Exit Sub
        End If

        For i = 0 To TabControl1.TabPages.Count - 1
            Try

                CType(TabControl1.TabPages(i).Controls.Item(0), SqlWorkBook).Speed_Up = is_speepup





            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        Next
    End Sub

    Private Sub ToolStripButton5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton5.Click
        ref()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If TabControl1.TabPages.Count = 0 Then
            Exit Sub
        End If
        Try
            CType(TabControl1.TabPages(TabControl1.SelectedIndex).Controls.Item(0), SqlWorkBook).Exit_Run()
            TabControl1.TabPages(TabControl1.SelectedIndex).Dispose()
            lst.RemoveAt(lst.Count - 1)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub ToolStripButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton3.Click

        If MsgBox("Are you sure to delete this file.", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
            Exit Sub
        End If
        If ListBox1.Items.Count = 0 Then
            Exit Sub
        End If
        Try
            obj.table_drop(ListBox1.Text)
            ref()
            MsgBox("table deleted")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub NewConnectionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewConnectionToolStripMenuItem.Click
        ToolStripButton1_Click(Nothing, Nothing)
    End Sub

    Private Sub RefreshConnectionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RefreshConnectionToolStripMenuItem.Click
        ToolStripButton5_Click(Nothing, Nothing)
    End Sub

    Private Sub CloseConnectionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CloseConnectionToolStripMenuItem.Click
        ToolStripButton3_Click(Nothing, Nothing)
    End Sub

    Private Sub NewWorkbookToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewWorkbookToolStripMenuItem.Click
        Button1_Click(Nothing, Nothing)
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick

    End Sub
    Private Sub frmMain_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        is_close = False
        ' Dim th As New Threading.Thread(New Threading.ThreadStart(AddressOf frm_close.Show))
        'th.Start()
        frm_close.ShowDialog()
        ' Timer1.Enabled = True
        'Me.frmMain_Load()


    End Sub
End Class