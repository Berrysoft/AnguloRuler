Imports System.Windows.Threading
Imports Microsoft.Win32

Class MainWindow
    Private openImage As OpenFileDialog
    Private openVideo As OpenFileDialog
    Private WithEvents TimerVideo As DispatcherTimer

    Public Sub New()
        InitializeComponent()

        openImage = New OpenFileDialog()
        openImage.Filter = "图片文件|*.jpg;*.png;*.bmp;*.gif|所有文件|*.*"
        openImage.Title = "打开图片"

        openVideo = New OpenFileDialog()
        openVideo.Filter = "视频文件|*.wmv;*.mov;*.mp4;*.mkv|所有文件|*.*"
        openVideo.Title = "打开视频"

        TimerVideo = New DispatcherTimer()
        TimerVideo.Interval = TimeSpan.FromMilliseconds(10)
    End Sub

    Private Sub ShowImage(path As String)
        Model.BackImagePath = path
        Model.BackVideoPath = Nothing
        Model.Playing = False
        ButtonPause.Visibility = Visibility.Hidden
        SliderVideo.Visibility = Visibility.Hidden
    End Sub

    Private Sub ShowVideo(path As String)
        Model.BackImagePath = Nothing
        Model.BackVideoPath = path
        Model.Playing = True
        ButtonPause.Visibility = Visibility.Visible
        SliderVideo.Visibility = Visibility.Visible
    End Sub

    Private Sub ButtonOpenImage_Click(sender As Object, e As RoutedEventArgs) Handles ButtonOpenImage.Click
        If openImage.ShowDialog() Then
            ShowImage(openImage.FileName)
        End If
    End Sub

    Private Sub ButtonClear_Click(sender As Object, e As RoutedEventArgs) Handles ButtonClear.Click
        Model.BackImagePath = Nothing
        Model.BackVideoPath = Nothing
        Model.Playing = False
        ButtonPause.Visibility = Visibility.Hidden
        SliderVideo.Visibility = Visibility.Hidden
    End Sub

    Private Sub ButtonOpenVideo_Click(sender As Object, e As RoutedEventArgs) Handles ButtonOpenVideo.Click
        If openVideo.ShowDialog() Then
            ShowVideo(openVideo.FileName)
        End If
    End Sub

    Private Sub VideoMedia_MediaOpened(sender As Object, e As RoutedEventArgs) Handles VideoMedia.MediaOpened
        SliderVideo.Maximum = VideoMedia.NaturalDuration.TimeSpan.TotalMilliseconds
    End Sub

    Private Sub TimerVideo_Tick(sender As Object, e As EventArgs) Handles TimerVideo.Tick
        Model.Position = VideoMedia.Position
    End Sub

    Private Sub ButtonPause_Click(sender As Object, e As RoutedEventArgs) Handles ButtonPause.Click
        Model.Playing = Not Model.Playing
    End Sub

    Private Sub MainWindow_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        Select Case e.Key
            Case Key.LeftCtrl
                Angulo.ForceHideCircle = True
            Case Key.LeftShift
                Angulo.SpecialAngle = True
        End Select
    End Sub

    Private Sub MainWindow_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        Select Case e.Key
            Case Key.LeftCtrl
                Angulo.ForceHideCircle = False
            Case Key.LeftShift
                Angulo.SpecialAngle = False
        End Select
    End Sub

    Private Sub Model_PlayingChanged(sender As Object, e As Boolean) Handles Model.PlayingChanged
        If e Then
            TimerVideo.Start()
            ButtonPause.Content = "暂停"
            VideoMedia.Play()
        Else
            TimerVideo.Stop()
            ButtonPause.Content = "播放"
            VideoMedia.Pause()
        End If
    End Sub

    Private Sub Model_PositionChanged(sender As Object, e As TimeSpan) Handles Model.PositionChanged
        VideoMedia.Position = e
    End Sub
End Class
