Imports System.Xml
Imports System.Data

Public Class frm_add_2
    Dim obj As cls_sqlconnect
    '************** xml file *********************
    Dim xfl As String = ""
    Dim xreader As XmlTextReader

    Dim nm As String = ""
    Dim db_nm As String = ""
    Dim pth As String = ""
    Dim delm As String = ""
    Dim cols As List(Of String)
    Dim dtyp As List(Of String)
    Dim fllen As Long
    Dim is_header As Boolean

    Dim sttm As Date
    Dim res As Boolean
    Private Sub frm_add_2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
    Sub New(ByVal fl As String)

        xfl = fl
        InitializeComponent()
        xreader = New XmlTextReader(fl)
    End Sub

    Public Sub read_thread_start()
        sttm = Now



        'My.Computer.FileSystem.CreateDirectory("D:\DC_" & opr_num)

       


        cols = New List(Of String)
        dtyp = New List(Of String)

        Dim xreader As New XmlTextReader(xfl)

        Do While (xreader.Read())
            Select Case xreader.NodeType
                Case XmlNodeType.Element 'Display beginning of element.
                    Select Case xreader.Name
                        Case Is = "FILENAME"
                            xreader.Read()
                            nm = xreader.Value
                        Case Is = "PATH"
                            xreader.Read()
                            pth = xreader.Value
                        Case Is = "FILESIZE"
                            xreader.Read()
                            ' fllen = Val(xreader.Value)
                        Case Is = "DELIM"
                            xreader.Read()
                            delm = xreader.Value
                            '
                        Case Is = "HEADER"
                            xreader.Read()
                            is_header = CType(xreader.Value, Boolean)
                        Case Is = "DB_NAME"
                            xreader.Read()
                            db_nm = xreader.Value
                        Case Is = "ITEM"
                            xreader.Read()
                            cols.Add(xreader.Value)
                        Case Is = "DITEM"
                            xreader.Read()
                            dtyp.Add(xreader.Value)
                    End Select


            End Select
        Loop

        lbl_path.Text = "Path: " & pth
        lbl_delm.Text = "Deliminator: " & delm
        Label1.Text = "Header: " & is_header.ToString

        My.Computer.FileSystem.CopyFile(pth, "d:\TMP\DATA\" & nm & ".txt", True)

        pth = "d:\TMP\DATA\" & nm & ".txt"

    End Sub
    Sub read_thread_run()
        Timer1.Enabled = False
        obj = New cls_sqlconnect(conn)
        If obj.table_create(cols, dtyp, db_nm) = False Then
            MsgBox(obj.opr_msg)
            Exit Sub
        End If
        If is_header = True Then
            res = obj.exec_proc_2(pth, "D:\TMP\FMT\" & nm & ".FMT", 2, db_nm)
        Else
            res = obj.exec_proc_2(pth, "D:\TMP\FMT\" & nm & ".FMT", 1, db_nm)
        End If


        If res = True Then


            obj.remove_double(db_nm)
            If obj.opr_flag = True Then
                MsgBox("Data loaded. Time taken " & DateDiff(DateInterval.Second, sttm, Now).ToString & " Seconds")
            Else
                MsgBox("Data loaded. Time taken " & DateDiff(DateInterval.Second, sttm, Now).ToString & " Seconds" & vbNewLine & "Warning!!!" & vbNewLine & obj.opr_msg)
            End If
        Else
            MsgBox("Data load failed." & vbNewLine & pth & vbNewLine & "D:\TMP\FMT\" & nm & ".FMT" & vbNewLine & "tbl_test_2")
        End If

        Me.Close()
        frmMain.ref()
    End Sub
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Select Case fllen
            Case Is = 3
                read_thread_start()
                fllen += 1
            Case Is = 8
                read_thread_run()
                fllen += 1
                Timer1.Enabled = False
                Exit Sub
            Case Else
                fllen += 1
        End Select

        Me.Text = "Please wait....." & fllen
    End Sub
End Class