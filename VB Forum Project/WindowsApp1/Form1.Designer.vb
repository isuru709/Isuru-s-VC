<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Browse = New System.Windows.Forms.Button()
        Me.Format = New System.Windows.Forms.ComboBox()
        Me.Save_TO = New System.Windows.Forms.Button()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.CONVERT = New System.Windows.Forms.Button()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Resolution = New System.Windows.Forms.ComboBox()
        Me.GpuEncoder = New System.Windows.Forms.ComboBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(191, 64)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(606, 29)
        Me.TextBox1.TabIndex = 0
        '
        'Browse
        '
        Me.Browse.Font = New System.Drawing.Font("Segoe UI Black", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Browse.ImageAlign = System.Drawing.ContentAlignment.TopRight
        Me.Browse.Location = New System.Drawing.Point(0, 0)
        Me.Browse.Name = "Browse"
        Me.Browse.Size = New System.Drawing.Size(118, 29)
        Me.Browse.TabIndex = 1
        Me.Browse.Text = "Browse"
        Me.Browse.UseVisualStyleBackColor = True
        '
        'Format
        '
        Me.Format.Font = New System.Drawing.Font("Segoe UI Black", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Format.FormattingEnabled = True
        Me.Format.Location = New System.Drawing.Point(191, 115)
        Me.Format.Name = "Format"
        Me.Format.Size = New System.Drawing.Size(152, 25)
        Me.Format.TabIndex = 2
        Me.Format.Text = "Choose Format"
        '
        'Save_TO
        '
        Me.Save_TO.Font = New System.Drawing.Font("Segoe UI Black", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Save_TO.Location = New System.Drawing.Point(15, 158)
        Me.Save_TO.Name = "Save_TO"
        Me.Save_TO.Size = New System.Drawing.Size(118, 29)
        Me.Save_TO.TabIndex = 3
        Me.Save_TO.Text = "Save_TO"
        Me.Save_TO.UseVisualStyleBackColor = True
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(191, 158)
        Me.TextBox2.Multiline = True
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(606, 29)
        Me.TextBox2.TabIndex = 4
        '
        'CONVERT
        '
        Me.CONVERT.Font = New System.Drawing.Font("Segoe UI Black", 20.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CONVERT.Location = New System.Drawing.Point(367, 205)
        Me.CONVERT.Name = "CONVERT"
        Me.CONVERT.Size = New System.Drawing.Size(234, 63)
        Me.CONVERT.TabIndex = 5
        Me.CONVERT.Text = "CONVERT"
        Me.CONVERT.UseVisualStyleBackColor = True
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(191, 304)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(607, 23)
        Me.ProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee
        Me.ProgressBar1.TabIndex = 6
        Me.ProgressBar1.Visible = False
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.Indigo
        Me.Panel1.Controls.Add(Me.Browse)
        Me.Panel1.Location = New System.Drawing.Point(15, 64)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(118, 29)
        Me.Panel1.TabIndex = 7
        '
        'Resolution
        '
        Me.Resolution.Font = New System.Drawing.Font("Segoe UI Black", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Resolution.FormattingEnabled = True
        Me.Resolution.Location = New System.Drawing.Point(386, 115)
        Me.Resolution.Name = "Resolution"
        Me.Resolution.Size = New System.Drawing.Size(152, 25)
        Me.Resolution.TabIndex = 8
        Me.Resolution.Text = "Resolution"
        '
        'GpuEncoder
        '
        Me.GpuEncoder.Font = New System.Drawing.Font("Segoe UI Black", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GpuEncoder.FormattingEnabled = True
        Me.GpuEncoder.Location = New System.Drawing.Point(573, 114)
        Me.GpuEncoder.Name = "GpuEncoder"
        Me.GpuEncoder.Size = New System.Drawing.Size(152, 25)
        Me.GpuEncoder.TabIndex = 9
        Me.GpuEncoder.Text = "GpuEncoder"
        '
        'Button1
        '
        Me.Button1.Font = New System.Drawing.Font("Segoe UI Black", 20.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.Location = New System.Drawing.Point(15, 205)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(118, 63)
        Me.Button1.TabIndex = 10
        Me.Button1.Text = "STOP"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.DarkSlateGray
        Me.BackgroundImage = Global.WindowsApp1.My.Resources.Resources.sliderbg
        Me.ClientSize = New System.Drawing.Size(933, 450)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.GpuEncoder)
        Me.Controls.Add(Me.Resolution)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Controls.Add(Me.CONVERT)
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.Save_TO)
        Me.Controls.Add(Me.Format)
        Me.Controls.Add(Me.TextBox1)
        Me.Font = New System.Drawing.Font("Segoe UI Black", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Form1"
        Me.Text = "Isuru's Video Converter"
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents Browse As Button
    Friend WithEvents Format As ComboBox
    Friend WithEvents Save_TO As Button
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents CONVERT As Button
    Friend WithEvents ProgressBar1 As ProgressBar
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Resolution As ComboBox
    Friend WithEvents GpuEncoder As ComboBox
    Friend WithEvents Button1 As Button
End Class
