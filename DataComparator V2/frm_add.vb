Imports System.Data
Imports System.Data.SqlClient
Imports System.Xml
Imports System.IO
Imports System.Threading
Public Class frm_add
    Dim bcopy As SqlBulkCopy
    Dim db_nm As String = ""
    Dim is_cancel As Boolean = False
    Dim obj As cls_sqlconnect
    '*****************  Two threads *********
    Dim is_reading As Boolean = False
    Dim is_loading As Boolean = False
    Dim read_path As List(Of String)
    Dim load_res As List(Of Boolean)
    Dim load_res_msg As List(Of String)
    Dim curr_load As Integer = 0

    '**************** File manupulaton **********

    Dim flr As IO.StreamReader
    Dim flwr As IO.StreamWriter

    Dim opr_path As String
    Dim opr_num As String
    Dim flread As Long = 0

    '************** xml file *********************
    Dim xfl As String = ""
    Dim xreader As XmlTextReader

    Dim nm As String = ""
    Dim pth As String = ""
    Dim delm As String = ""
    Dim cols As List(Of String)
    Dim dtyp As List(Of String)
    Dim fllen As Long
    Dim is_header As Boolean

    '**************** thread**************t

    Dim sttm As Date
    Dim th_read As Thread
    Dim th_load As Thread
    Dim th_end As Thread
    Private Delegate Sub DelgUpdate_read_UI(ByVal parm As Object(), ByVal thr As frm_add)
    Private Delegate Sub DelgUpdate_load_UI(ByVal parm As Object(), ByVal thr As frm_add)
    Private Sub Update_read_UI(ByVal parm As Object(), ByVal thr As frm_add)
        lbl_cnnt.Text = parm(0).ToString

        ProgressBar1.Value = Math.Truncate(Val(parm(1).ToString) * 100 / Val(parm(2).ToString))

    End Sub
    Private Sub Update_load_UI(ByVal parm As Object(), ByVal thr As frm_add)

        If is_reading = True Then

            ProgressBar2.Value = (0.5 * ProgressBar1.Value)
        Else
            ProgressBar2.Value = Math.Truncate(Val(parm(0).ToString) * 100 / Val(parm(1).ToString))


        End If


        ListBox1.Text = ListBox1.Text & (parm(2).ToString) & vbNewLine
    End Sub
    Private Delegate Sub Delgend_task()
    Private Sub end_task()
        For i = 0 To read_path.Count - 1
            My.Computer.FileSystem.DeleteFile(read_path(i).ToString, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently)
        Next

        Dim strrn As String = ""
        Dim tsk_res As Boolean = True
        For i = 0 To load_res_msg.Count - 1
            If Strings.Left(load_res_msg(i), 3) = "SUC" Then
                ' tsk_res=True 
            Else
                tsk_res = False
                strrn = load_res_msg(i)
            End If
        Next
        If tsk_res = True Then
            MsgBox("Data loaded. Time taken " & DateDiff(DateInterval.Second, sttm, Now).ToString & " Seconds")
        Else
            obj.table_drop(db_nm)
            MsgBox("Data load failed. ERROR: " & strrn)
        End If
        ' MsgBox(strrn)

        Me.Close()
        frmMain.ref()
    End Sub
    Sub New(ByVal fl As String)

        xfl = fl
        InitializeComponent()
        xreader = New XmlTextReader(fl)
    End Sub

    Private Sub frm_add_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub


    Public Sub read_thread_start()
        sttm = Now
        obj = New cls_sqlconnect(conn)
        opr_num = Strings.Right((Now.Year.ToString), 2) & Now.DayOfYear.ToString & Now.Hour.ToString & Now.Minute.ToString & Now.Second.ToString
        opr_path = My.Computer.FileSystem.SpecialDirectories.Temp

        'My.Computer.FileSystem.CreateDirectory("D:\DC_" & opr_num)

        opr_path = "D:\TMP\DATA\"

        is_reading = True
        read_path = New List(Of String)
        load_res = New List(Of Boolean)
        load_res_msg = New List(Of String)


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
                            fllen = Val(xreader.Value)
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

        If obj.table_create(cols, dtyp, db_nm) = False Then
            MsgBox(obj.opr_msg)
            Exit Sub
        End If

        flr = New StreamReader(pth)
        flwr = New StreamWriter(opr_path & nm & "_1.csv")
        ' read_path.Add(opr_path & "\fl_1.csv")

        th_read = New Thread(New ThreadStart(AddressOf read_thread_run))
        th_read.Start()

        th_load = New Thread(New ThreadStart(AddressOf load_thread_start))
        th_load.Start()

    End Sub

    Sub read_thread_run()
        'th_load = New Thread(New ThreadStart(AddressOf load_thread_run))

        Dim curr_fl_count As Integer = 1
        Dim strln As String = ""
        Dim ln_cnt As Long = 0

        While flr.EndOfStream = False
            If is_cancel = True Then
                Exit While
            End If
            strln = flr.ReadLine
            ln_cnt += 1

            If ln_cnt > 10000 Then
                ln_cnt = 1
                read_path.Add(opr_path & nm & "_" & curr_fl_count.ToString & ".csv")
                curr_fl_count += 1
                flwr.Close()
                flwr = New StreamWriter(opr_path & nm & "_" & curr_fl_count.ToString & ".csv")

                'call sql procedure

               
            End If
            ' strfld = Strings.Split(strln, delm, -1, CompareMethod.Text)
            ' flread = flread + System.Text.Encoding.ASCII.GetByteCount(strln)
            flread += System.Text.ASCIIEncoding.ASCII.GetByteCount(strln)
            
            strln = Strings.Replace(strln, """", "")
            strln = Strings.Replace(strln, "NULL", "")
            'strln = Strings.Replace(strln, " ", "")
            ' strln = Strings.Replace(strln, """", "")
            strln = Strings.Replace(strln, "#", "")
            'strln = Strings.Replace(strln, delm, ",")
            If Len(Trim(strln)) = 0 Then
            Else
                flwr.WriteLine(strln)
            End If




                Dim del As DelgUpdate_read_UI = AddressOf Update_read_UI
                Me.Invoke(del, New Object(2) {(ln_cnt + ((curr_fl_count - 1) * 10000)).ToString, flread.ToString, fllen.ToString}, Me)

        End While



        flr.Close()
        flwr.Close()
        read_path.Add(opr_path & nm & "_" & curr_fl_count.ToString & ".csv")
        is_reading = False

        ' MsgBox("all read done")


        th_read = Nothing

    End Sub
    Sub load_thread_start()
        curr_load = 0
        While is_reading = True
            If read_path.Count <= 0 Then
                Thread.Sleep(50)
            ElseIf read_path.Count > curr_load Then
                curr_load += 1
                load_thread_run(curr_load - 1)

            End If
        End While
        'curr_load += 1
        load_thread_run(curr_load, "ALL")


    End Sub
    Sub load_thread_run(ByVal rd As Integer, Optional ByVal mode As String = "PARALLEL")
        Dim ppth As String '= read_path(rd)
        Dim res As Boolean
        If mode = "PARALLEL" Then
            ppth = read_path(rd)
            If is_header = True And rd = 0 Then
                res = obj.exec_proc_2(ppth, "D:\TMP\FMT\" & nm & ".FMT", 2, db_nm)
                load_res.Add(res)
                load_res_msg.Add(obj.opr_msg)
            Else

                res = obj.exec_proc_2(ppth, "D:\TMP\FMT\" & nm & ".FMT", 1, db_nm)
                load_res.Add(res)
                load_res_msg.Add(obj.opr_msg)
            End If

            Dim del As DelgUpdate_read_UI = AddressOf Update_load_UI
            Me.Invoke(del, New Object(2) {rd, read_path.Count, ppth & " (" & obj.opr_msg & ")"}, Me)
        Else
            If load_res.Count = read_path.Count Then
                Dim del1 As Delgend_task = AddressOf end_task
                Me.Invoke(del1)
                Exit Sub
            End If
            If rd = 0 Then
                For i = 0 To read_path.Count - 1
                    res = obj.exec_proc_2(read_path(i), "D:\TMP\FMT\" & nm & ".FMT", 2, db_nm)

                    load_res.Add(res)
                    load_res_msg.Add(obj.opr_msg)
                    Dim del As DelgUpdate_read_UI = AddressOf Update_load_UI
                    Me.Invoke(del, New Object(2) {i, read_path.Count, read_path(i) & " (" & obj.opr_msg & ")"}, Me)

                Next
            Else
                For i = load_res.Count - 1 To read_path.Count - 1
                    res = obj.exec_proc_2(read_path(i), "D:\TMP\FMT\" & nm & ".FMT", 1, db_nm)
                    load_res.Add(res)
                    load_res_msg.Add(obj.opr_msg)
                    Dim del As DelgUpdate_read_UI = AddressOf Update_load_UI
                    Me.Invoke(del, New Object(2) {i, read_path.Count, read_path(i) & " (" & obj.opr_msg & ")"}, Me)

                Next
            End If




            Dim del2 As Delgend_task = AddressOf end_task
            Me.Invoke(del2)
        End If



    End Sub


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        is_cancel = True
    End Sub
End Class