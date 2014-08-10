<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frm_add
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.lbl_cnnt = New System.Windows.Forms.Label()
        Me.lbl_delm = New System.Windows.Forms.Label()
        Me.lbl_path = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.ProgressBar2 = New System.Windows.Forms.ProgressBar()
        Me.ListBox1 = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(33, 100)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(479, 18)
        Me.ProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.ProgressBar1.TabIndex = 15
        '
        'lbl_cnnt
        '
        Me.lbl_cnnt.AutoSize = True
        Me.lbl_cnnt.Location = New System.Drawing.Point(108, 77)
        Me.lbl_cnnt.Name = "lbl_cnnt"
        Me.lbl_cnnt.Size = New System.Drawing.Size(29, 19)
        Me.lbl_cnnt.TabIndex = 14
        Me.lbl_cnnt.Text = "##"
        '
        'lbl_delm
        '
        Me.lbl_delm.AutoSize = True
        Me.lbl_delm.Location = New System.Drawing.Point(29, 48)
        Me.lbl_delm.Name = "lbl_delm"
        Me.lbl_delm.Size = New System.Drawing.Size(119, 19)
        Me.lbl_delm.TabIndex = 13
        Me.lbl_delm.Text = "Deliminator: ##"
        '
        'lbl_path
        '
        Me.lbl_path.AutoSize = True
        Me.lbl_path.Location = New System.Drawing.Point(29, 22)
        Me.lbl_path.Name = "lbl_path"
        Me.lbl_path.Size = New System.Drawing.Size(88, 19)
        Me.lbl_path.TabIndex = 12
        Me.lbl_path.Text = "Path:#### "
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(29, 77)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(71, 19)
        Me.Label1.TabIndex = 11
        Me.Label1.Text = "Progress"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(545, 124)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(76, 29)
        Me.Button1.TabIndex = 10
        Me.Button1.Text = "Cancel"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'ProgressBar2
        '
        Me.ProgressBar2.Location = New System.Drawing.Point(33, 135)
        Me.ProgressBar2.Name = "ProgressBar2"
        Me.ProgressBar2.Size = New System.Drawing.Size(479, 18)
        Me.ProgressBar2.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.ProgressBar2.TabIndex = 16
        '
        'ListBox1
        '
        Me.ListBox1.Location = New System.Drawing.Point(33, 178)
        Me.ListBox1.Multiline = True
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.ListBox1.Size = New System.Drawing.Size(588, 108)
        Me.ListBox1.TabIndex = 18
        '
        'frm_add
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 19.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(660, 310)
        Me.Controls.Add(Me.ListBox1)
        Me.Controls.Add(Me.ProgressBar2)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Controls.Add(Me.lbl_cnnt)
        Me.Controls.Add(Me.lbl_delm)
        Me.Controls.Add(Me.lbl_path)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Button1)
        Me.Font = New System.Drawing.Font("Cambria", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "frm_add"
        Me.Text = "Importing to database"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents lbl_cnnt As System.Windows.Forms.Label
    Friend WithEvents lbl_delm As System.Windows.Forms.Label
    Friend WithEvents lbl_path As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents ProgressBar2 As System.Windows.Forms.ProgressBar
    Friend WithEvents ListBox1 As System.Windows.Forms.TextBox
End Class
