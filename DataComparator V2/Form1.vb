Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.SqlClient.SqlBulkCopy


Public Class Form1
    Dim com As SqlCommand
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim sqlcp As SqlBulkCopy = New SqlBulkCopy(conn)
        com = New SqlCommand("Text_File_Bulk_Import", conn)
        com.CommandType = CommandType.StoredProcedure
        com.Parameters.Add("@SourceFilePath", SqlDbType.VarChar, 50).Value = Trim(TextBox1.Text)
        com.Parameters.Add("@TargetTable", SqlDbType.VarChar, 30).Value = ListBox1.Text

        Dim pr As SqlParameter = com.Parameters.Add("@VALS", SqlDbType.Int)

        pr.Direction = ParameterDirection.Output

        com.ExecuteScalar()

        MsgBox(com.Parameters("@VALS").Value)
        '@SourceFilePath varchar(50),
        '@TargetTable varchar(30),
        '@VALS int=0 output
        'sqlcp .WriteToServer(
    End Sub

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'conn_open()
        Dim obj As New cls_sqlconnect(conn)

        Dim dtb As New DataTable
        dtb = obj.get_all_table
        ListBox1.DataSource = dtb
        ListBox1.DisplayMember = dtb.Columns(2).ColumnName
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim opn As New OpenFileDialog
        opn.Filter = "Text files (*.txt)(*.dat)(*.csv)|*.txt;*.dat;*.csv"
        If opn.ShowDialog = Windows.Forms.DialogResult.OK Then
            TextBox1.Text = opn.FileName
           
        Else
            MsgBox("No file Selected")
        End If
    End Sub
End Class
