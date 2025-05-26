Imports System.Text.RegularExpressions
Imports System.Threading.Tasks
Imports System.IO

Public Class Form1
    Private lastWindowState As FormWindowState
    Private ffmpegProcess As Process ' Global FFmpeg process

    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        If lastWindowState = FormWindowState.Minimized AndAlso
           (Me.WindowState = FormWindowState.Normal OrElse Me.WindowState = FormWindowState.Maximized) Then

            TextBox1.Width = Me.ClientSize.Width - 150
            TextBox2.Width = Me.ClientSize.Width - 150
            ProgressBar1.Width = Me.ClientSize.Width - 150
            CONVERT.Left = (Me.ClientSize.Width - CONVERT.Width) \ 2
        End If

        If Me.WindowState <> FormWindowState.Minimized Then
            TextBox1.Width = Me.ClientSize.Width - 150
            TextBox2.Width = Me.ClientSize.Width - 150
            ProgressBar1.Width = Me.ClientSize.Width - 150
            CONVERT.Left = (Me.ClientSize.Width - CONVERT.Width) \ 2
        End If

        lastWindowState = Me.WindowState
    End Sub

    Private Function GetVideoDuration(ffmpegPath As String, inputFile As String) As Double
        Dim proc As New Process()
        proc.StartInfo.FileName = ffmpegPath
        proc.StartInfo.Arguments = "-i """ & inputFile & """"
        proc.StartInfo.UseShellExecute = False
        proc.StartInfo.RedirectStandardError = True
        proc.StartInfo.CreateNoWindow = True
        proc.Start()

        Dim output As String = proc.StandardError.ReadToEnd()
        proc.WaitForExit()

        Dim match As Match = Regex.Match(output, "Duration: (\d+):(\d+):(\d+\.\d+)")
        If match.Success Then
            Dim hours = Double.Parse(match.Groups(1).Value)
            Dim minutes = Double.Parse(match.Groups(2).Value)
            Dim seconds = Double.Parse(match.Groups(3).Value)
            Return hours * 3600 + minutes * 60 + seconds
        End If

        Return -1
    End Function

    Private Sub Format_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Format.SelectedIndexChanged
        If Format.SelectedItem.ToString() = "mp3" Then
            Resolution.Visible = False
            GpuEncoder.Visible = False
        Else
            Resolution.Visible = True
            GpuEncoder.Visible = True
        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lastWindowState = Me.WindowState

        GpuEncoder.Items.AddRange(New String() {
            "Software (libx264)",
            "Software (libx265)",
            "Software (libaom-av1)",
            "NVIDIA H.264 (h264_nvenc)",
            "NVIDIA HEVC (hevc_nvenc)",
            "NVIDIA AV1 (av1_nvenc)",
            "AMD H.264 (h264_amf)",
            "AMD HEVC (hevc_amf)",
            "AMD AV1 (av1_amf)",
            "Intel H.264 (h264_qsv)",
            "Intel HEVC (hevc_qsv)",
            "Intel AV1 (av1_qsv)"
        })
        GpuEncoder.SelectedIndex = 0

        Format.Items.AddRange(New String() {"mp4", "avi", "mkv", "mov", "flv", "mp3"})
        Resolution.Items.AddRange(New String() {"Same as input", "2560x1440", "1920x1080", "1280x720", "640x360"})
        Resolution.SelectedIndex = 0
        ProgressBar1.Style = ProgressBarStyle.Marquee
        ProgressBar1.Visible = False

        Panel1.BackColor = Color.FromArgb(100, 255, 255, 255)

        Browse.FlatStyle = FlatStyle.Flat
        Browse.FlatAppearance.BorderSize = 2
        Browse.FlatAppearance.BorderColor = Color.White
        Browse.BackColor = Color.LightSkyBlue
        Browse.ForeColor = Color.Black

        Save_TO.FlatStyle = FlatStyle.Flat
        Save_TO.FlatAppearance.BorderSize = 2
        Save_TO.FlatAppearance.BorderColor = Color.White
        Save_TO.BackColor = Color.LightSkyBlue
        Save_TO.ForeColor = Color.Black

        CONVERT.FlatStyle = FlatStyle.Flat
        CONVERT.FlatAppearance.BorderSize = 2
        CONVERT.FlatAppearance.BorderColor = Color.White
        CONVERT.BackColor = Color.LightSkyBlue
        CONVERT.ForeColor = Color.Black

        TextBox1.BorderStyle = BorderStyle.FixedSingle
        TextBox1.BackColor = Color.LightSkyBlue
        TextBox1.ForeColor = Color.Black
        TextBox1.Font = New Font("Segoe UI", 10, FontStyle.Regular)

        TextBox2.BorderStyle = BorderStyle.FixedSingle
        TextBox2.BackColor = Color.LightSkyBlue
        TextBox2.ForeColor = Color.Black
        TextBox2.Font = New Font("Segoe UI", 10, FontStyle.Regular)
    End Sub

    Private Sub Browse_Click(sender As Object, e As EventArgs) Handles Browse.Click
        Dim openFile As New OpenFileDialog()
        openFile.Filter = "Video Files|*.mp4;*.avi;*.mov;*.mkv;*.flv|All Files|*.*"
        openFile.Title = "Select a Video File"

        If openFile.ShowDialog() = DialogResult.OK Then
            TextBox1.Text = openFile.FileName
        End If
    End Sub

    Private Sub Save_TO_Click(sender As Object, e As EventArgs) Handles Save_TO.Click
        Dim saveFile As New SaveFileDialog()
        saveFile.Filter = "All Files|*.*"
        saveFile.Title = "Select Save Location"

        If saveFile.ShowDialog() = DialogResult.OK Then
            TextBox2.Text = saveFile.FileName
        End If
    End Sub

    Private Sub CONVERT_Click(sender As Object, e As EventArgs) Handles CONVERT.Click
        Dim inputPath As String = TextBox1.Text
        Dim outputPath As String = TextBox2.Text
        If Format.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a format.")
            Return
        End If

        Dim selectedFormat As String = Format.SelectedItem.ToString()
        Dim ffmpegPath As String = Application.StartupPath & "\ffmpeg.exe"

        If inputPath = "" Or outputPath = "" Then
            MessageBox.Show("Please select input and output paths.")
            Return
        End If

        Dim finalOutputPath As String = Path.ChangeExtension(outputPath, selectedFormat)
        Dim arguments As String = ""
        Dim videoEncoder As String = ""

        Select Case GpuEncoder.SelectedItem.ToString()
            Case "Software (libx264)"
                videoEncoder = "libx264"
            Case "Software (libx265)"
                videoEncoder = "libx265"
            Case "Software (libaom-av1)"
                videoEncoder = "libaom-av1"
            Case "NVIDIA H.264 (h264_nvenc)"
                videoEncoder = "h264_nvenc"
            Case "NVIDIA HEVC (hevc_nvenc)"
                videoEncoder = "hevc_nvenc"
            Case "NVIDIA AV1 (av1_nvenc)"
                videoEncoder = "av1_nvenc"
            Case "AMD H.264 (h264_amf)"
                videoEncoder = "h264_amf"
            Case "AMD HEVC (hevc_amf)"
                videoEncoder = "hevc_amf"
            Case "AMD AV1 (av1_amf)"
                videoEncoder = "av1_amf"
            Case "Intel H.264 (h264_qsv)"
                videoEncoder = "h264_qsv"
            Case "Intel HEVC (hevc_qsv)"
                videoEncoder = "hevc_qsv"
            Case "Intel AV1 (av1_qsv)"
                videoEncoder = "av1_qsv"
        End Select

        Dim audioEncoder As String = If(selectedFormat = "mp3", "libmp3lame", "aac")

        Dim scaleFilter As String = ""
        If selectedFormat <> "mp3" AndAlso Resolution.SelectedItem IsNot Nothing Then
            If Resolution.SelectedItem.ToString() <> "Same as input" Then
                scaleFilter = "-vf scale=" & Resolution.SelectedItem.ToString()
            End If
        End If

        If videoEncoder.Contains("nvenc") Then
            arguments = $"-i ""{inputPath}"" {scaleFilter} -c:v {videoEncoder} -preset slow -crf 27 -pix_fmt yuv420p -c:a {audioEncoder} -b:a 128k ""{finalOutputPath}"""
        ElseIf videoEncoder.Contains("amf") Then
            arguments = $"-i ""{inputPath}"" {scaleFilter} -c:v {videoEncoder} -crf 27 -pix_fmt yuv420p -c:a {audioEncoder} -b:a 128k ""{finalOutputPath}"""
        Else
            arguments = $"-i ""{inputPath}"" {scaleFilter} -c:v {videoEncoder} -preset veryslow -crf 27 -c:a {audioEncoder} -b:a 128k ""{finalOutputPath}"""
        End If

        Dim durationInSeconds As Double = GetVideoDuration(ffmpegPath, inputPath)
        If durationInSeconds <= 0 Then
            MessageBox.Show("Failed to get video duration.")
            Return
        End If

        ProgressBar1.Visible = True
        ProgressBar1.Minimum = 0
        ProgressBar1.Maximum = 100
        ProgressBar1.Value = 0
        ProgressBar1.Style = ProgressBarStyle.Continuous

        Dim conversionTask As New Task(
        Sub()
            ffmpegProcess = New Process()
            ffmpegProcess.StartInfo.FileName = ffmpegPath
            ffmpegProcess.StartInfo.Arguments = arguments
            ffmpegProcess.StartInfo.CreateNoWindow = True
            ffmpegProcess.StartInfo.UseShellExecute = False
            ffmpegProcess.StartInfo.RedirectStandardError = True
            ffmpegProcess.StartInfo.RedirectStandardOutput = False

            AddHandler ffmpegProcess.ErrorDataReceived, Sub(senderObj, eArgs)
                                                            If eArgs.Data IsNot Nothing AndAlso eArgs.Data.Contains("time=") Then
                                                                Dim match = Regex.Match(eArgs.Data, "time=(\d+):(\d+):(\d+\.\d+)")
                                                                If match.Success Then
                                                                    Dim h = Integer.Parse(match.Groups(1).Value)
                                                                    Dim m = Integer.Parse(match.Groups(2).Value)
                                                                    Dim s = Double.Parse(match.Groups(3).Value)
                                                                    Dim currentSec = h * 3600 + m * 60 + s
                                                                    Dim progress = CInt((currentSec / durationInSeconds) * 100)
                                                                    Me.Invoke(Sub()
                                                                                  ProgressBar1.Value = Math.Min(progress, 100)
                                                                              End Sub)
                                                                End If
                                                            End If
                                                        End Sub

            Try
                ffmpegProcess.Start()
                ffmpegProcess.BeginErrorReadLine()
                ffmpegProcess.WaitForExit()
                Me.Invoke(Sub()
                              ProgressBar1.Value = 100
                              MessageBox.Show("Conversion complete!" & vbCrLf & finalOutputPath)
                              ProgressBar1.Visible = False
                          End Sub)
            Catch ex As Exception
                Me.Invoke(Sub()
                              ProgressBar1.Visible = False
                              MessageBox.Show("Error: " & ex.Message)
                          End Sub)
            End Try
        End Sub)
        conversionTask.Start()
    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If ffmpegProcess IsNot Nothing AndAlso Not ffmpegProcess.HasExited Then
            ffmpegProcess.Kill()
        End If
    End Sub
End Class
