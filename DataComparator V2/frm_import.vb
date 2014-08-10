Imports System.Threading
Public Class frm_import



    Dim g_delm As String
    Dim g_is_header As Boolean
    Dim g_col_count As Integer
    Dim g_cols As List(Of String)
    Dim g_fl_name As String


    Dim o_fl As String = ""
    Dim _mode As String
    Public Property ex_mode() As String
        Set(ByVal value As String)

            _mode = value
        End Set
        Get
            Return _mode
        End Get
    End Property
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim opn As New OpenFileDialog
        opn.Filter = "Text files (*.txt)(*.dat)(*.csv)|*.txt;*.dat;*.csv"
        If opn.ShowDialog = Windows.Forms.DialogResult.OK Then
            TextBox1.Text = opn.FileName
            g_fl_name = opn.SafeFileName
            ref(TextBox1.Text, ",", CheckBox1.Checked)
        Else
            MsgBox("No file Selected")
        End If
    End Sub

    Private Sub frm_import_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        frmMain.Enabled = True
    End Sub

    Private Sub frm_import_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        g_cols = New List(Of String)
    End Sub

    Private Sub ref(ByVal fl As String, Optional ByVal delm As String = ",", Optional ByVal is_header As Boolean = True, Optional ByVal rpl As String = """")
        If Len(Trim(fl)) = 0 Then
            Exit Sub
        End If
        g_cols.Clear()
        Dim flrd As New IO.StreamReader(fl)
        Dim line(3) As String
        Dim max_cells As Integer = 0

        Dim tmp_in As Integer = 0
        Dim tmp_str As String = ""

       

        While flrd.EndOfStream = False
            If tmp_in = 4 Then
                Exit While
            End If

            line(tmp_in) = flrd.ReadLine
            
            tmp_in += 1

        End While


        Dim cols(tmp_in)() As String

        max_cells = 0
        For i = 0 To tmp_in - 1
            
            cols(i) = Strings.Split(line(i), delm)
            If cols(i).Length > max_cells Then
                max_cells = cols(i).Length

            End If
            tmp_str = tmp_str & vbNewLine & cols(i).Length
        Next

        Dim tbl As New DataTable
        Dim dt(max_cells) As DataColumn

        Dim dr As DataRow

        ' MsgBox(max_cells.ToString & vbNewLine & tmp_str)
        '  Exit Sub

        For i = 0 To max_cells - 1
            dt(i) = New DataColumn
            If is_header = True Then
                If i = cols(0).Length Then
                    Exit For
                Else
                    dt(i).ColumnName = Strings.Replace(Trim(Strings.Replace(Strings.Replace(Strings.Replace(Strings.Replace(cols(0)(i), rpl, ""), """", ""), "#", ""), "'", "")), " ", "_")
                End If

            Else
                dt(i).ColumnName = "Column_" & (i + 1).ToString

            End If
            tbl.Columns.Add(dt(i))
            g_cols.Add(dt(i).ColumnName)
        Next

        'MsgBox(tbl.Columns.Count)
        'Exit Sub

        If is_header = True Then
            For i = 1 To tmp_in - 1
                dr = tbl.NewRow
                For j As Integer = 0 To max_cells - 1
                    If j = cols(i).Count - 1 Then
                        Exit For
                    Else
                        dr.Item(j) = Strings.Replace(cols(i)(j), rpl, "")
                    End If

                Next
                tbl.Rows.Add(dr)
            Next
        Else
            For i = 0 To tmp_in - 1
                dr = tbl.NewRow
                For j As Integer = 0 To max_cells - 1
                    If j = cols(i).Length Then
                        Exit For
                    Else
                        dr.Item(j) = Strings.Replace(cols(i)(j), rpl, "")
                    End If
                Next
                tbl.Rows.Add(dr)
            Next
        End If

        dg.DataSource = tbl
        g_col_count = dg.ColumnCount

    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = True Then
            g_is_header = True
            ref(TextBox1.Text, g_delm, g_is_header)
        Else
            g_is_header = False
            ref(TextBox1.Text, g_delm, g_is_header)
        End If
    End Sub

    Private Sub RadioButton2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton2.CheckedChanged

        If RadioButton2.Checked = True Then
            dg.DataSource = Nothing
            ref(TextBox1.Text, "|", CheckBox1.Checked)
            g_delm = "|"
        End If
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click


    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        For i = 0 To g_col_count - 1
            ListBox1.Items.Add(g_cols(i))
            ListBox2.Items.Add("Varchar(100)")
        Next

        grp2.Location = grp1.Location
        grp2.Visible = True
        grp1.Visible = False

        txtdbname.Text = Strings.Replace(Strings.Replace(g_fl_name, " ", "_"), ".", "_")

        ListBox1.SelectedIndex = 0

    End Sub

    Private Sub RadioButton1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton1.CheckedChanged
        If CType(sender, RadioButton).Checked = True Then
            dg.DataSource = Nothing
            g_delm = ","
            ref(TextBox1.Text, g_delm, CheckBox1.Checked)

        End If
    End Sub

    Private Sub RadioButton4_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton4.CheckedChanged
        If CType(sender, RadioButton).Checked = True Then
            dg.DataSource = Nothing
            g_delm = ";"
            ref(TextBox1.Text, g_delm, CheckBox1.Checked)

        End If
    End Sub

    Private Sub RadioButton3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton3.CheckedChanged
        If CType(sender, RadioButton).Checked = True Then
            dg.DataSource = Nothing
            g_delm = ":"
            ref(TextBox1.Text, g_delm, CheckBox1.Checked)

        End If
    End Sub

    Private Sub RadioButton5_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton5.CheckedChanged
        If CType(sender, RadioButton).Checked = True Then
            dg.DataSource = Nothing
            g_delm = Trim(TextBox2.Text)
            ref(TextBox1.Text, g_delm, CheckBox1.Checked)

        End If
    End Sub




    Private Sub cmd_edit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_edit.Click
        Me.Enabled = False
        frm_edit_datatype.Show()
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBox1.SelectedIndexChanged
        ListBox2.SelectedIndex = ListBox1.SelectedIndex
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Dim nm As String = "FL_" & Strings.Right((Now.Year.ToString), 2) & Now.DayOfYear.ToString & Now.Hour.ToString & Now.Minute.ToString & Now.Second.ToString
        TextBox3.Text = "<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no""?><CONNECTION><FILE><FILENAME>" & nm & "</FILENAME>"

        TextBox3.Text = TextBox3.Text & vbNewLine & "<PATH>" & TextBox1.Text & "</PATH>"
        TextBox3.Text = TextBox3.Text & vbNewLine & "<FILESIZE>" & FileLen(TextBox1.Text).ToString & "</FILESIZE>"
        TextBox3.Text = TextBox3.Text & vbNewLine & "<DELIM>" & g_delm & "</DELIM>"
        TextBox3.Text = TextBox3.Text & vbNewLine & "<HEADER>" & g_is_header.ToString & "</HEADER></FILE>"
        TextBox3.Text = TextBox3.Text & vbNewLine & "<DATABASE><DB_NAME>" & Trim(txtdbname.Text) & " </DB_NAME>"
        TextBox3.Text = TextBox3.Text & vbNewLine & "<DB_MDATE>" & Now.ToString & "</DB_MDATE>"
        TextBox3.Text = TextBox3.Text & vbNewLine & "<DB_CCOUNT>" & ListBox1.Items.Count & "</DB_CCOUNT>"
        TextBox3.Text = TextBox3.Text & vbNewLine & "<COLUMN>"
        For i = 0 To ListBox1.Items.Count - 1
            TextBox3.Text = TextBox3.Text & vbNewLine & vbTab & "<ITEM>" & ListBox1.Items(i).ToString & "</ITEM>"
        Next
        TextBox3.Text = TextBox3.Text & vbNewLine & "</COLUMN>"
        TextBox3.Text = TextBox3.Text & vbNewLine & "<DATATYPES>"
        For i = 0 To ListBox2.Items.Count - 1
            TextBox3.Text = TextBox3.Text & vbNewLine & vbTab & "<DITEM>" & ListBox2.Items(i).ToString & "</DITEM>"
        Next
        TextBox3.Text = TextBox3.Text & vbNewLine & "</DATATYPES>"
        TextBox3.Text = TextBox3.Text & vbNewLine & "<DB_RCOUNT>0</DB_RCOUNT></DATABASE></CONNECTION>"


        Dim wrt As New IO.StreamWriter("D:\TMP\XML\" & nm & ".XML")

        wrt.WriteLine(TextBox3.Text)


        wrt.Close()

        wrt = New IO.StreamWriter("D:\TMP\FMT\" & nm & ".FMT")
        wrt.WriteLine("8.0")
        wrt.WriteLine(ListBox1.Items.Count)
        For i = 0 To ListBox1.Items.Count - 1
            If i = ListBox1.Items.Count - 1 Then
                wrt.WriteLine((i + 1).ToString & vbTab & "SQLCHAR" & vbTab & "0" & vbTab & "100" & vbTab & """\n""" & vbTab & (i + 1).ToString & vbTab & ListBox1.Items(i) & vbTab & "SQL_Latin1_General_Cp437_BIN")
            Else
                wrt.WriteLine((i + 1).ToString & vbTab & "SQLCHAR" & vbTab & "0" & vbTab & "100" & vbTab & """" & g_delm & """" & vbTab & (i + 1).ToString & vbTab & ListBox1.Items(i) & vbTab & "SQL_Latin1_General_Cp437_BIN")

            End If
            
        Next

        wrt.Close()

        ' Exit Sub
        If CheckBox2.Checked = True Then
            Dim frmm As New frm_add_2("D:\TMP\XML\" & nm & ".XML")
            frmm.Show()
        Else
            add_flconnection("D:\TMP\XML\" & nm & ".XML")
        End If


        Me.Close()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Me.Close()
    End Sub

    Private Sub grp1_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles grp1.Enter

    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        Me.Close()
    End Sub
End Class