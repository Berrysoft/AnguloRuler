Imports System.Windows.Threading
Imports Microsoft.Win32

Class MainWindow
    Private mouseDowned As Boolean
    Private state As MouseMoveState
    Private openImage As OpenFileDialog
    Private openVideo As OpenFileDialog
    Private WithEvents TimerVideo As DispatcherTimer
    Private showDetails As Boolean
    Private oldmouse As Point
    Private moving As Boolean

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
    End Sub

    Private Sub MainWindow_MouseDown(sender As Object, e As MouseButtonEventArgs) Handles Me.MouseDown
        mouseDowned = (e.LeftButton = MouseButtonState.Pressed)
        oldmouse = e.GetPosition(Me)
        moving = False
    End Sub

    Private Sub MainWindow_MouseUp(sender As Object, e As MouseButtonEventArgs) Handles Me.MouseUp
        mouseDowned = False
        moving = False
    End Sub

    Private Sub MainWindow_MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove
        Dim mouse As Point = e.GetPosition(Me)
        If Not moving OrElse Not mouseDowned Then
            If IsNearby(LineUnit.StartPoint, mouse) Then
                state = MouseMoveState.MoveUnitStart
            ElseIf IsNearby(LineUnit.EndPoint, mouse) Then
                state = MouseMoveState.MoveUnitEnd
            ElseIf IsNearby(LineUnit.StartPoint, LineUnit.EndPoint, mouse) Then
                state = MouseMoveState.MoveUnit
            ElseIf Not mouseDowned Then
                state = MouseMoveState.None
            End If
        End If
        moving = True
        If state = MouseMoveState.None Or showDetails Then
            HideCircle()
        ElseIf Not showDetails Then
            ShowCircle()
        End If
        If mouseDowned Then
            Select Case state
                Case MouseMoveState.MoveUnitStart
                    LineUnit.StartPoint = mouse
                Case MouseMoveState.MoveUnitEnd
                    LineUnit.EndPoint = mouse
                Case MouseMoveState.MoveUnit
                    Dim delta = mouse - oldmouse
                    LineUnit.StartPoint += delta
                    LineUnit.EndPoint += delta
                    oldmouse = mouse
            End Select
            Dim vu = LineUnit.EndPoint - LineUnit.StartPoint
            Length = vu.Length
        End If
    End Sub

    Private Sub ShowCircle()
        If state = MouseMoveState.MoveStart OrElse state = MouseMoveState.MoveEnd OrElse state = MouseMoveState.MoveO Then
        ElseIf state = MouseMoveState.MoveUnit Then
            PathCircleUnit.Fill = Brushes.Red
            PathCircleUnit.Visibility = Visibility.Visible
        ElseIf state = MouseMoveState.MoveUnitStart OrElse state = MouseMoveState.MoveUnitEnd Then
            PathCircleUnit.Visibility = Visibility.Visible
        End If
    End Sub

    Private Sub HideCircle()
        PathCircleUnit.Visibility = Visibility.Hidden
    End Sub

    Private Function IsNearby(center As Point, mouse As Point) As Boolean
        Return Math.Abs(center.X - mouse.X) <= 5 AndAlso Math.Abs(center.Y - mouse.Y) <= 5
    End Function

    Private Function IsNearby(p1 As Point, p2 As Point, mouse As Point) As Boolean
        Dim a As Double = p2.Y - p1.Y
        Dim b As Double = p1.X - p2.X
        Dim c As Double = p2.X * p1.Y - p1.X * p2.Y
        Dim l As Double = Math.Sqrt(a * a + b * b)
        Dim distance As Double = Math.Abs(a * mouse.X + b * mouse.Y + c) / l
        Return distance < 3 AndAlso mouse.X >= Math.Min(p1.X, p2.X) AndAlso mouse.X <= Math.Max(p1.X, p2.X)
    End Function

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
            showDetails = True
            Angulo.ForceHideCircle = True
            HideCircle()
        End If
    End Sub

    Private Sub MainWindow_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        showDetails = False
        Angulo.ForceHideCircle = False
        If e.Key = Key.LeftCtrl AndAlso state <> MouseMoveState.None Then
            ShowCircle()
        End If
    End Sub
End Class

Enum MouseMoveState
    None
    MoveO
    MoveStart
    MoveEnd
    MoveUnit
    MoveUnitStart
    MoveUnitEnd
End Enum
