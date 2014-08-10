Imports System.Data
Imports System.Data.SqlClient
Imports System.Threading

Public Class SqlWorkBook
    Dim com As SqlCommand
    Dim da As SqlDataAdapter
    Dim dr As SqlDataReader
    Dim ds As DataSet

    Dim th As Thread
    Dim is_cancel As Boolean
    Dim is_speedup As Boolean
    Dim strt_tm As Date
    Dim wrt As IO.StreamWriter


    Dim obj_sql As cls_sqlconnect

    Dim rec_count As Long
    Dim str1 As String
    Dim str2 As String
    Dim tmp As Integer
    Dim tbl As DataTable
    Sub New(Optional ByVal spd As Boolean = False)
        obj_sql = New cls_sqlconnect(conn)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Speed_Up = spd
    End Sub
    Public Property Speed_Up As Boolean
        Get
            Return is_speedup
        End Get
        Set(ByVal value As Boolean)
            is_speedup = value
            If value = True Then
                PictureBox1.BackgroundImage = My.Resources.glossy_button_blank_green_circle_T
                Label2.Text = "Query Speed up on."
            Else
                PictureBox1.BackgroundImage = My.Resources.glossy_button_blank_red_circle_T
                Label2.Text = "Query Speed up Off."
            End If
        End Set
    End Property
    Private Sub btn_run_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_run.Click
        If txt.Text = "" Then
            Exit Sub
        End If
        btn_run.Enabled = False
        btn_clear.Enabled = False
        btn_exp.Enabled = False

        is_cancel = False
        dg.DataSource = Nothing
        rec_count = 0
        strt_tm = Now

        Panel1.Visible = True
        lbl_cnnt.Text = "Loading Please wait..."

        If Len(Trim(txt.SelectedText)) = 0 Then
            str1 = txt.Text
        Else
            str1 = txt.SelectedText
        End If







        If Speed_Up = True Then
            th = New Thread(New ThreadStart(AddressOf call_cmd_ds))
            th.Start()
        Else
            th = New Thread(New ThreadStart(AddressOf call_cmd))
            th.Start()
        End If


    End Sub
    Private Delegate Sub DelgUpdate_CMDRUN(ByVal parm As Object(), ByVal thr As SqlWorkBook)
    Private Sub Update_CMDRUN(ByVal parm As Object(), ByVal thr As SqlWorkBook)
        ' Dim dgr As New DataGridViewRow
        If parm(0).ToString = "PROG" Then
            lbl_cnnt.Text = parm(1).ToString & " records read."
            ' dg.Rows.Add(CType(parm(2), DataGridViewRow))
            ' dg.DataSource = tbl

        Else
            dg.DataSource = tbl
        End If
        'dg.Refresh()

        ' DualTextBox1.add_text(parm(0).ToString, parm(1).ToString)

    End Sub
    Private Delegate Sub DelgUpdate_CMDRUNDS()
    Private Sub Update_CMDRUNDS()

        dg.Visible = True
        dg.DataSource = obj_sql.ds.Tables(0)
        rec_count = dg.RowCount


    End Sub
    Private Delegate Sub DelgTask_Done()
    Private Sub Task_Done()
        Panel1.Visible = False
        'ProgressBar1.Style = ProgressBarStyle.Blocks

        lbl_lastrun.Text = "Last Query : " & rec_count.ToString & " Records fetched. Time taken " & DateDiff(DateInterval.Second, strt_tm, Now).ToString & " seconds."
        rec_count = 0

        btn_run.Enabled = True
        btn_clear.Enabled = True
        btn_exp.Enabled = True
    End Sub
    Public Sub Exit_Run()
        Dim thh As Thread
        thh = Thread.CurrentThread
        thh = Nothing
        Try
            dr.Close()
        Catch ex As Exception

        End Try
        Dim del2 As DelgTask_Done = AddressOf Task_Done
        Me.Invoke(del2)
    End Sub
    Private Sub call_cmd()
        rec_count = 0
        Dim del As DelgUpdate_CMDRUN = AddressOf Update_CMDRUN
        If obj_sql.run_query_reader(str1) = False Then
            MsgBox(obj_sql.opr_msg)
            Exit_Run()
            Exit Sub
        End If
        If Strings.Left(Trim(UCase(str1)), 6) <> "SELECT" Then
            MsgBox("Execution done")
            Exit_Run()
            Exit Sub
        End If
        Dim dr As SqlDataReader = obj_sql.dr

        Dim schema_tbl As DataTable = dr.GetSchemaTable
        tbl = New DataTable


        If schema_tbl Is Nothing Then
            Exit_Run()
        End If

        For Each drow As DataRow In schema_tbl.Rows
            Dim col As New DataColumn(drow("ColumnName"))
            tbl.Columns.Add(col)
        Next

        While dr.Read
            If is_cancel = True Then
                Exit While
            End If
            Dim ddrow As DataRow = tbl.NewRow
            For i As Integer = 0 To tbl.Columns.Count - 1
                ddrow.Item(i) = dr.Item(i)
            Next
            tbl.Rows.Add(ddrow)

            rec_count += 1
            Try
                Me.Invoke(del, New Object(1) {"PROG", rec_count.ToString}, Me)
            Catch ex As Exception

            End Try

        End While



        dr.Close()

        Me.Invoke(del, New Object(1) {"DONE", rec_count.ToString}, Me)

        Exit_Run()

    End Sub
    Private Sub call_cmd_ds()
        If obj_sql.run_query_ds(str1) = False Then
            MsgBox(obj_sql.opr_msg)
            Exit_Run()
        Else
            If Strings.Left(Trim(UCase(str1)), 6) <> "SELECT" Then
                MsgBox("Execution done")
                Exit_Run()
                Exit Sub
            End If

            Dim del As DelgUpdate_CMDRUNDS = AddressOf Update_CMDRUNDS
            Me.Invoke(del)

        End If
        Exit_Run()
    End Sub

  

 
    Private Sub SplitContainer1_Panel1_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SplitContainer1.Panel1.Resize
        txt.Width = SplitContainer1.Panel1.Width - 20
        txt.Left = 10
        txt.Top = Panel1.Top + Panel1.Height + 5
        txt.Height = SplitContainer1.Panel1.Height - (txt.Top + 20)
    End Sub

    Private Sub SplitContainer1_Panel2_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SplitContainer1.Panel2.Resize
        dg.Width = SplitContainer1.Panel2.Width - 20
        dg.Height = SplitContainer1.Panel2.Height - 20

        dg.Left = 10
        dg.Top = 10

     
    End Sub

    Private Sub SplitContainer1_Panel2_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles SplitContainer1.Panel2.Paint

    End Sub

    Private Sub btn_exp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_exp.Click
        If dg.DataSource Is Nothing Or dg.RowCount = 0 Then
            Exit Sub
        End If

        Dim dlg As New SaveFileDialog
        dlg.Filter = "Data file (*.Dat)|*.Dat"
        If dlg.ShowDialog = DialogResult.OK Then
            str1 = dlg.FileName
        Else
            Exit Sub
        End If

        Panel1.Visible = True
        btn_run.Enabled = False
        btn_clear.Enabled = False
        btn_exp.Enabled = False
        is_cancel = False

        rec_count = dg.RowCount
        tmp = dg.ColumnCount
        wrt = New IO.StreamWriter(str1)


        For i As Integer = 0 To tmp - 1
            If i = tmp - 1 Then
                str2 = str2 & dg.Columns(i).Name
            Else
                str2 = str2 & dg.Columns(i).Name & "|"
            End If
        Next

        wrt.WriteLine(str2)

        'th = New Thread(New ThreadStart(AddressOf cmd_export))
        ' th.Start()
    End Sub
    Private Delegate Sub DelgExport_dg()
    Private Sub Export_dg()
      
    End Sub

    Private Sub cmd_export()
        For i As Integer = 0 To rec_count - 1
            str2 = ""
            For j As Integer = 0 To tmp - 1

            Next
        Next
    End Sub

    Private Sub SplitContainer1_Panel1_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles SplitContainer1.Panel1.Paint

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        is_cancel = True
    End Sub

    Private Sub txt_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt.TextChanged

    End Sub
End Class
