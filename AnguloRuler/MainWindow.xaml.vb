Imports System.Windows.Threading
Imports Microsoft.Win32

Class MainWindow
    Private openImage As OpenFileDialog
    Private openVideo As OpenFileDialog
    Private WithEvents TimerVideo As DispatcherTimer

    Friend Property Playing As Boolean
        Get
            Return GetValue(PlayingProperty)
        End Get
        Set(value As Boolean)
            SetValue(PlayingProperty, value)
        End Set
    End Property

    Friend Property BackImagePath As String
        Get
            Return GetValue(BackImagePathProperty)
        End Get
        Set(value As String)
            SetValue(BackImagePathProperty, value)
        End Set
    End Property

    Friend Property BackVideoPath As String
        Get
            Return GetValue(BackVideoPathProperty)
        End Get
        Set(value As String)
            SetValue(BackVideoPathProperty, value)
            If value Is Nothing Then
                Playing = False
                ButtonPause.Visibility = Visibility.Hidden
            Else
                Playing = True
            End If
        End Set
    End Property

    Friend Property Theta As Double
        Get
            Return GetValue(ThetaProperty)
        End Get
        Set(value As Double)
            SetValue(ThetaProperty, value)
        End Set
    End Property

    Friend Property Length As Double
        Get
            Return GetValue(LengthProperty)
        End Get
        Set(value As Double)
            SetValue(LengthProperty, value)
        End Set
    End Property

    Friend Property Position As TimeSpan
        Get
            Return GetValue(PositionProperty)
        End Get
        Set(value As TimeSpan)
            SetValue(PositionProperty, value)
        End Set
    End Property

    Public Shared ReadOnly PlayingProperty As DependencyProperty = DependencyProperty.Register(NameOf(Playing), GetType(Boolean), GetType(MainWindow), New PropertyMetadata(False, AddressOf PlayingChangeCallback))
    Public Shared ReadOnly BackImagePathProperty As DependencyProperty = DependencyProperty.Register(NameOf(BackImagePath), GetType(String), GetType(MainWindow))
    Public Shared ReadOnly BackVideoPathProperty As DependencyProperty = DependencyProperty.Register(NameOf(BackVideoPath), GetType(String), GetType(MainWindow))
    Public Shared ReadOnly ThetaProperty As DependencyProperty = DependencyProperty.Register(NameOf(Theta), GetType(Double), GetType(MainWindow))
    Public Shared ReadOnly LengthProperty As DependencyProperty = DependencyProperty.Register(NameOf(Length), GetType(Double), GetType(MainWindow))
    Public Shared ReadOnly PositionProperty As DependencyProperty = DependencyProperty.Register(NameOf(Position), GetType(TimeSpan), GetType(MainWindow), New PropertyMetadata(TimeSpan.Zero, AddressOf PositionChangeCallback))

    Private Shared Sub PlayingChangeCallback(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim w As MainWindow = TryCast(d, MainWindow)
        Dim flag As Boolean = e.NewValue
        If w IsNot Nothing Then
            If flag Then
                w.TimerVideo.Start()
                w.ButtonPause.Content = "暂停"
                w.ButtonPause.Visibility = Visibility.Visible
                w.VideoMedia.Play()
            Else
                w.TimerVideo.Stop()
                w.ButtonPause.Content = "播放"
                w.VideoMedia.Pause()
            End If
        End If
    End Sub
    Private Shared Sub PositionChangeCallback(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim w As MainWindow = TryCast(d, MainWindow)
        If w IsNot Nothing Then
            w.VideoMedia.Position = CType(e.NewValue, TimeSpan)
        End If
    End Sub

    Public Sub New()
        InitializeComponent()

        openImage = New OpenFileDialog()
        openImage.Filter = "图片文件|*.jpg;*.png;*.bmp;*.gif|所有文件|*.*"
        openImage.Title = "打开图片"

        openVideo = New OpenFileDialog()
        openVideo.Filter = "视频文件|*.wmv;*.mov;*.mp4|所有文件|*.*"
        openVideo.Title = "打开视频"

        TimerVideo = New DispatcherTimer()
        TimerVideo.Interval = TimeSpan.FromMilliseconds(10)

        ContentGrid.DataContext = Me

        Length = 100

        SetBinding(ThetaProperty, New Binding("Angle") With {.ElementName = "Angulo"})
        SetBinding(LengthProperty, New Binding("Length") With {.ElementName = "Angulo"})
    End Sub

    Private Sub ButtonOpenImage_Click(sender As Object, e As RoutedEventArgs) Handles ButtonOpenImage.Click
        If openImage.ShowDialog() Then
            BackImagePath = openImage.FileName
            BackVideoPath = Nothing
        End If
    End Sub

    Private Sub ButtonClear_Click(sender As Object, e As RoutedEventArgs) Handles ButtonClear.Click
        BackImagePath = Nothing
        BackVideoPath = Nothing
    End Sub

    Private Sub ButtonOpenVideo_Click(sender As Object, e As RoutedEventArgs) Handles ButtonOpenVideo.Click
        If openVideo.ShowDialog() Then
            BackVideoPath = openVideo.FileName
            BackImagePath = Nothing
        End If
    End Sub

    Private Sub VideoMedia_MediaOpened(sender As Object, e As RoutedEventArgs) Handles VideoMedia.MediaOpened
        SliderVideo.Maximum = VideoMedia.NaturalDuration.TimeSpan.TotalMilliseconds
    End Sub

    Private Sub TimerVideo_Tick(sender As Object, e As EventArgs) Handles TimerVideo.Tick
        Position = VideoMedia.Position
    End Sub

    Private Sub ButtonPause_Click(sender As Object, e As RoutedEventArgs) Handles ButtonPause.Click
        Playing = Not Playing
    End Sub

    Private Sub MainWindow_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.Key = Key.LeftCtrl Then
            Angulo.ForceHideCircle = True
        End If
    End Sub

    Private Sub MainWindow_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        Angulo.ForceHideCircle = False
    End Sub
End Class
