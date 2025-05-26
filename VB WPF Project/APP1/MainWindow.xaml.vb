Imports Microsoft.Win32
Imports System.Text.RegularExpressions
Imports System.IO
Imports System.Threading.Tasks
Imports System.Windows.Threading

Class MainWindow

    Private lastWindowState As WindowState
    Private ffmpegProcess As Process = Nothing

    Private Sub Window_SizeChanged(sender As Object, e As SizeChangedEventArgs) Handles Me.SizeChanged
        If lastWindowState = WindowState.Minimized AndAlso
           (Me.WindowState = WindowState.Normal OrElse Me.WindowState = WindowState.Maximized) Then
            ResizeControls()
        End If

        If Me.WindowState <> WindowState.Minimized Then
            ResizeControls()
        End If

        lastWindowState = Me.WindowState
    End Sub

    Private Sub ResizeControls()
        Dim newWidth = Me.ActualWidth - 150
        If newWidth < 100 Then newWidth = 100
        TextBox1.Width = newWidth
        TextBox2.Width = newWidth
        ProgressBar1.Width = newWidth
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

    Private Sub Format_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles Format.SelectionChanged
        If Format.SelectedItem IsNot Nothing AndAlso Format.SelectedItem.ToString() = "mp3" Then
            Resolution.Visibility = Visibility.Collapsed
            GpuEncoder.Visibility = Visibility.Collapsed
        Else
            Resolution.Visibility = Visibility.Visible
            GpuEncoder.Visibility = Visibility.Visible
        End If
    End Sub

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        lastWindowState = Me.WindowState

        GpuEncoder.ItemsSource = New String() {
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
        }
        GpuEncoder.SelectedIndex = 0

        Format.ItemsSource = New String() {"mp4", "avi", "mkv", "mov", "flv", "mp3"}
        Format.SelectedIndex = 0

        Resolution.ItemsSource = New String() {"Same as input", "2560x1440", "1920x1080", "1280x720", "640x360"}
        Resolution.SelectedIndex = 0

        ProgressBar1.Visibility = Visibility.Hidden
        ProgressBar1.IsIndeterminate = False
    End Sub

    Private Sub Browse_Click(sender As Object, e As RoutedEventArgs) Handles Browse.Click
        Dim openFile As New OpenFileDialog()
        openFile.Filter = "Video Files|*.mp4;*.avi;*.mov;*.mkv;*.flv|All Files|*.*"
        openFile.Title = "Select a Video File"

        If openFile.ShowDialog() = True Then
            TextBox1.Text = openFile.FileName
        End If
    End Sub

    Private Sub Save_TO_Click(sender As Object, e As RoutedEventArgs) Handles Save_TO.Click
        Dim saveFile As New SaveFileDialog()
        saveFile.Filter = "All Files|*.*"
        saveFile.Title = "Select Save Location"

        If saveFile.ShowDialog() = True Then
            TextBox2.Text = saveFile.FileName
        End If
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles [STOP].Click
        If ffmpegProcess IsNot Nothing AndAlso Not ffmpegProcess.HasExited Then
            ffmpegProcess.Kill()
        End If
    End Sub
    Private Async Sub CONVERT_Click(sender As Object, e As RoutedEventArgs) Handles CONVERT.Click
        If ffmpegProcess IsNot Nothing AndAlso Not ffmpegProcess.HasExited Then
            Try
                ffmpegProcess.Kill()
                ffmpegProcess.WaitForExit()
            Catch ex As Exception
            End Try
        End If

        Dim inputPath As String = TextBox1.Text.Trim()
        Dim outputPath As String = TextBox2.Text.Trim()
        If Format.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a format.")
            Return
        End If

        Dim selectedFormat As String = Format.SelectedItem.ToString()
        Dim ffmpegPath As String = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ffmpeg.exe")

        If String.IsNullOrEmpty(inputPath) OrElse String.IsNullOrEmpty(outputPath) Then
            MessageBox.Show("Please select input and output paths.")
            Return
        End If

        Dim finalOutputPath As String = Path.ChangeExtension(outputPath, selectedFormat)
        Dim arguments As String = ""
        Dim videoEncoder As String = ""
        Select Case GpuEncoder.SelectedItem.ToString()
            Case "Software (libx264)" : videoEncoder = "libx264"
            Case "Software (libx265)" : videoEncoder = "libx265"
            Case "Software (libaom-av1)" : videoEncoder = "libaom-av1"
            Case "NVIDIA H.264 (h264_nvenc)" : videoEncoder = "h264_nvenc"
            Case "NVIDIA HEVC (hevc_nvenc)" : videoEncoder = "hevc_nvenc"
            Case "NVIDIA AV1 (av1_nvenc)" : videoEncoder = "av1_nvenc"
            Case "AMD H.264 (h264_amf)" : videoEncoder = "h264_amf"
            Case "AMD HEVC (hevc_amf)" : videoEncoder = "hevc_amf"
            Case "AMD AV1 (av1_amf)" : videoEncoder = "av1_amf"
            Case "Intel H.264 (h264_qsv)" : videoEncoder = "h264_qsv"
            Case "Intel HEVC (hevc_qsv)" : videoEncoder = "hevc_qsv"
            Case "Intel AV1 (av1_qsv)" : videoEncoder = "av1_qsv"
        End Select

        Dim audioEncoder As String = If(selectedFormat = "mp3", "libmp3lame", "aac")

        Dim scaleFilter As String = ""
        If selectedFormat <> "mp3" AndAlso Resolution.SelectedItem IsNot Nothing Then
            If Resolution.SelectedItem.ToString() <> "Same as input" Then
                scaleFilter = "-vf scale=" & Resolution.SelectedItem.ToString()
            End If
        End If

        If videoEncoder.Contains("nvenc") Then
            arguments = "-i """ & inputPath & """ " & scaleFilter & " -c:v " & videoEncoder &
                " -preset slow -crf 27 -pix_fmt yuv420p -c:a " & audioEncoder &
                " -b:a 128k """ & finalOutputPath & """"
        ElseIf videoEncoder.Contains("amf") Then
            arguments = "-i """ & inputPath & """ " & scaleFilter & " -c:v " & videoEncoder &
                " -crf 27 -pix_fmt yuv420p -c:a " & audioEncoder & " -b:a 128k """ & finalOutputPath & """"
        Else
            arguments = "-i """ & inputPath & """ " & scaleFilter & " -c:v " & videoEncoder &
                " -preset veryslow -crf 27 -c:a " & audioEncoder & " -b:a 128k """ & finalOutputPath & """"
        End If

        Dim durationInSeconds As Double = GetVideoDuration(ffmpegPath, inputPath)
        If durationInSeconds <= 0 Then
            MessageBox.Show("Failed to get video duration.")
            Return
        End If

        ProgressBar1.Visibility = Visibility.Visible
        ProgressBar1.Minimum = 0
        ProgressBar1.Maximum = 100
        ProgressBar1.Value = 0
        ProgressBar1.IsIndeterminate = False

        Try
            Await Task.Run(Sub()
                               ffmpegProcess = New Process()
                               Dim process = ffmpegProcess

                               process.StartInfo.FileName = ffmpegPath
                               process.StartInfo.Arguments = arguments
                               process.StartInfo.CreateNoWindow = True
                               process.StartInfo.UseShellExecute = False
                               process.StartInfo.RedirectStandardError = True
                               process.StartInfo.RedirectStandardOutput = False

                               AddHandler process.ErrorDataReceived, Sub(senderObj, eArgs)
                                                                         If eArgs.Data IsNot Nothing AndAlso eArgs.Data.Contains("time=") Then
                                                                             Dim match = Regex.Match(eArgs.Data, "time=(\d+):(\d+):(\d+\.\d+)")
                                                                             If match.Success Then
                                                                                 Dim h = Integer.Parse(match.Groups(1).Value)
                                                                                 Dim m = Integer.Parse(match.Groups(2).Value)
                                                                                 Dim s = Double.Parse(match.Groups(3).Value)
                                                                                 Dim currentSec = h * 3600 + m * 60 + s
                                                                                 Dim progress = CInt((currentSec / durationInSeconds) * 100)
                                                                                 Dispatcher.Invoke(Sub()
                                                                                                       ProgressBar1.Value = Math.Min(progress, 100)
                                                                                                   End Sub)
                                                                             End If
                                                                         End If
                                                                     End Sub

                               process.Start()
                               process.BeginErrorReadLine()
                               process.WaitForExit()

                               Dispatcher.Invoke(Sub()
                                                     ProgressBar1.Value = 100
                                                     MessageBox.Show("Conversion complete!" & vbCrLf & finalOutputPath)
                                                     ProgressBar1.Visibility = Visibility.Hidden
                                                 End Sub)
                           End Sub)
        Catch ex As Exception
            ProgressBar1.Visibility = Visibility.Hidden
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub

End Class
