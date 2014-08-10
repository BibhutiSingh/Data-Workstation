Imports System.Data
Imports System.Data.SqlClient
Public Class cls_sqlconnect
    Dim con As SqlConnection
    Dim da As SqlDataAdapter
    Public dr As SqlDataReader
    Dim com As SqlCommand

    Public ds As DataSet
    Public opr_flag As Boolean
    Public opr_msg As String

    Dim comstr As String
    Sub New(ByVal cnn As SqlConnection)
        con = cnn
    End Sub
    Private Sub opr_res(ByVal rs As Boolean, Optional ByVal msg As String = "Success")
        opr_flag = rs
        opr_msg = msg
    End Sub
    Public Function exec_proc(ByVal pth As String, ByVal tbl As String) As Boolean
        com = New SqlCommand("Text_File_Bulk_Import", con)
        com.CommandType = CommandType.StoredProcedure
        com.Parameters.Add("@SourceFilePath", SqlDbType.VarChar, 50).Value = Trim(pth)
        com.Parameters.Add("@TargetTable", SqlDbType.VarChar, 30).Value = tbl

        Dim pr As SqlParameter = com.Parameters.Add("@VALS", SqlDbType.Int)

        pr.Direction = ParameterDirection.Output

        com.ExecuteScalar()

        If Val(com.Parameters("@VALS").Value) = 0 Then
            Return False
        Else
            Return True
        End If
    End Function
    Public Function exec_proc_2(ByVal spth As String, ByVal fpth As String, ByVal is_head As Integer, ByVal tbl As String) As Boolean
        Dim mmms As String
        com = New SqlCommand("TextBulk2", con)

        com.CommandType = CommandType.StoredProcedure
        com.Parameters.Add("@TargetTable", SqlDbType.VarChar, 30).Value = tbl
        com.Parameters.Add("@SourceFilePath", SqlDbType.VarChar, 50).Value = Trim(spth)
        com.Parameters.Add("@FormatFilePath", SqlDbType.VarChar, 50).Value = Trim(fpth)
        com.Parameters.Add("@RowNumber", SqlDbType.Int).Value = is_head


        Dim pr As SqlParameter = com.Parameters.Add("@VALS", SqlDbType.VarChar, 500)

        pr.Direction = ParameterDirection.Output


        com.ExecuteScalar()

        mmms = com.Parameters("@VALS").Value
        If UCase(Strings.Left(mmms, 3)) = "ERR" Then
            opr_res(False, mmms)
            Return False
        Else
            opr_res(True, mmms)
            Return True
        End If
    End Function
    Public Function get_all_table() As DataTable
        ' Dim lst As New DataTable
        ' lst = con.GetSchema("Tables")
        da = New SqlDataAdapter(" SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES", con)
        ds = New DataSet
        da.Fill(ds)

        Return ds.Tables(0)

    End Function
    Public Function table_create(ByVal cols As List(Of String), ByVal typ As List(Of String), ByVal tbl As String) As Boolean
        Try
            com = New SqlCommand("drop table " & tbl, con)
            com.ExecuteNonQuery()
        Catch ex As Exception
            'opr_res(False, "Error in deleting : " & ex.Message)
            ' Return False
        End Try
        comstr = "create table " & tbl & " ("
        For i = 0 To cols.Count - 1
            comstr = comstr & cols(i).ToString & " VARCHAR(500),"

        Next
        comstr = Strings.Left(comstr, Len(comstr) - 1) & ")"


        Try
            com = New SqlCommand(comstr, con)
            com.ExecuteNonQuery()
            opr_res(True, "Done")
        Catch ex As Exception
            opr_res(False, "Error in creating table: " & ex.Message & vbNewLine & comstr)

        End Try

        Return opr_flag

    End Function
    Public Function table_insert(ByVal cols() As String, ByVal vals() As String, ByVal tbl As String) As Boolean

        comstr = "insert into " & tbl & "(" & Strings.Join(cols, ",") & ") values"

        Return True

    End Function
    Public Function table_drop(ByVal tbl As String) As Boolean

        comstr = "Drop table " & tbl

        Try
            com = New SqlCommand(comstr, con)
            com.ExecuteNonQuery()
            opr_res(True, "Done")
        Catch ex As Exception
            opr_res(False, "Error in creating table: " & ex.Message)

        End Try
        Return opr_flag

    End Function
    Public Function run_query_reader(ByVal cmm As String) As Boolean
        com = New SqlCommand(cmm, conn)
        ' Dim res As IAsyncResult = com.BeginExecuteReader




        Try
            dr = com.ExecuteReader
            Return True
        Catch ex As Exception
            opr_res(False, "Error in Query: " & vbNewLine & "Query: " & cmm & vbNewLine & "Error: " & ex.Message)
            Return False
        End Try
    End Function

    Public Function run_query_ds(ByVal cmm As String) As Boolean
        da = New SqlDataAdapter(cmm, conn)
        ds = New DataSet

        Try
            da.Fill(ds)

            Return True
        Catch ex As Exception
            opr_res(False, "Error in Query: " & vbNewLine & "Query: " & cmm & vbNewLine & "Error: " & ex.Message)
            Return False
        End Try
    End Function

    Public Function get_less_used() As DataTable
        Dim tbl As DataTable = Nothing

        '  da = New SqlDataAdapter("SELECT name, create_date, modify_date, DATEDIFF(d, create_date, modify_date) AS Used_Since FROM(sys.tables) where Used_since>=3", conn)
        da = New SqlDataAdapter("SELECT name FROM sys.tables", conn)

        ds = New DataSet
        Try
            da.Fill(ds)
            tbl = ds.Tables(0)
            opr_res(True)
        Catch ex As Exception
            opr_res(False, ex.Message)
        End Try



        Return tbl
    End Function

    Public Sub remove_double(ByVal tbl As String)
        da = New SqlDataAdapter("select * from " & tbl & " where 1=2", conn)
        ds = New DataSet


        da.Fill(ds)

        Dim com_str = "UPDATE " & tbl & " SET"

        For i = 0 To ds.Tables(0).Columns.Count - 1
            If i = 0 Then
                com_str = com_str & " " & ds.Tables(0).Columns(i).ColumnName & "=REPLACE(" & ds.Tables(0).Columns(i).ColumnName & ",'""','')"
            Else
                com_str = com_str & "," & ds.Tables(0).Columns(i).ColumnName & "=REPLACE(" & ds.Tables(0).Columns(i).ColumnName & ",'""','')"
            End If


        Next

        ' MsgBox(com_str)
        com = New SqlCommand(com_str, conn)
        Try
            com.ExecuteNonQuery()
            opr_res(True)
        Catch ex As Exception
            opr_res(False, ex.Message)
        End Try

    End Sub
    Public Sub post_load(ByVal cols As List(Of String), ByVal typ As List(Of String), ByVal tbl As String)

        Dim com_msg As String = ""

        comstr = "update " & tbl & " set "

        For i = 0 To typ.Count - 1
            Select Case UCase(Strings.Left(typ(i), 3))
                Case Is = "VAR"
                    comstr = comstr & cols(i).ToString & "=LTRIM(RTRIM(" & cols(i).ToString & ")),"
                Case Is = "DAT"
                    comstr = comstr & cols(i).ToString & " DateTime,"
                Case Else
                    comstr = comstr & cols(i).ToString & " " & typ(i).ToString & ","
            End Select
        Next

    End Sub
End Class
