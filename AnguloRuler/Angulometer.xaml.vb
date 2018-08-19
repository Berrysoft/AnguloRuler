Public Class Angulometer
    Public Shared ReadOnly StrokeProperty As DependencyProperty = DependencyProperty.Register(NameOf(Stroke), GetType(Brush), GetType(Angulometer), New FrameworkPropertyMetadata(Nothing, FrameworkPropertyMetadataOptions.AffectsMeasure Or FrameworkPropertyMetadataOptions.AffectsRender Or FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender))
    Public Property Stroke As Brush
        Get
            Return GetValue(StrokeProperty)
        End Get
        Set(value As Brush)
            SetValue(StrokeProperty, value)
        End Set
    End Property

    Public Shared ReadOnly CircleFillProperty As DependencyProperty = DependencyProperty.Register(NameOf(CircleFill), GetType(Brush), GetType(Angulometer), New FrameworkPropertyMetadata(Nothing, FrameworkPropertyMetadataOptions.AffectsRender Or FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender))
    Public Property CircleFill As Brush
        Get
            Return GetValue(CircleFillProperty)
        End Get
        Set(value As Brush)
            SetValue(CircleFillProperty, value)
        End Set
    End Property

    Public Shared ReadOnly StartPointProperty As DependencyProperty = DependencyProperty.Register(NameOf(StartPoint), GetType(Point), GetType(Angulometer), New FrameworkPropertyMetadata(New Point(), FrameworkPropertyMetadataOptions.AffectsMeasure Or FrameworkPropertyMetadataOptions.AffectsRender))
    Public Property StartPoint As Point
        Get
            Return GetValue(StartPointProperty)
        End Get
        Set(value As Point)
            SetValue(StartPointProperty, value)
        End Set
    End Property

    Public Shared ReadOnly EndPoint1Property As DependencyProperty = DependencyProperty.Register(NameOf(EndPoint1), GetType(Point), GetType(Angulometer), New FrameworkPropertyMetadata(New Point(), FrameworkPropertyMetadataOptions.AffectsMeasure Or FrameworkPropertyMetadataOptions.AffectsRender, AddressOf EndPointChangedCallback))
    Public Property EndPoint1 As Point
        Get
            Return GetValue(EndPoint1Property)
        End Get
        Set(value As Point)
            SetValue(EndPoint1Property, value)
        End Set
    End Property

    Public Shared ReadOnly EndPoint2Property As DependencyProperty = DependencyProperty.Register(NameOf(EndPoint2), GetType(Point), GetType(Angulometer), New FrameworkPropertyMetadata(New Point(), FrameworkPropertyMetadataOptions.AffectsMeasure Or FrameworkPropertyMetadataOptions.AffectsRender, AddressOf EndPointChangedCallback))
    Public Property EndPoint2 As Point
        Get
            Return GetValue(EndPoint2Property)
        End Get
        Set(value As Point)
            SetValue(EndPoint2Property, value)
        End Set
    End Property

    Private Shared Sub EndPointChangedCallback(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim angulo As Angulometer = d
        angulo.Angle = GetArcAngle(angulo.StartPoint, angulo.EndPoint1, angulo.EndPoint2)
    End Sub

    Private Shared ReadOnly AnglePropertyKey As DependencyPropertyKey = DependencyProperty.RegisterReadOnly(NameOf(Angle), GetType(Double), GetType(Angulometer), New PropertyMetadata(0.0))
    Public Shared ReadOnly AngleProperty As DependencyProperty = AnglePropertyKey.DependencyProperty
    Public Property Angle As Double
        Get
            Return GetValue(AngleProperty)
        End Get
        Private Set(value As Double)
            SetValue(AnglePropertyKey, value)
        End Set
    End Property

    Public Shared ReadOnly StartRulerPointProperty As DependencyProperty = DependencyProperty.Register(NameOf(StartRulerPoint), GetType(Point), GetType(Angulometer), New FrameworkPropertyMetadata(New Point(), FrameworkPropertyMetadataOptions.AffectsMeasure Or FrameworkPropertyMetadataOptions.AffectsRender, AddressOf RulerPointChangedCallback))
    Public Property StartRulerPoint As Point
        Get
            Return GetValue(StartRulerPointProperty)
        End Get
        Set(value As Point)
            SetValue(StartRulerPointProperty, value)
        End Set
    End Property

    Public Shared ReadOnly EndRulerPointProperty As DependencyProperty = DependencyProperty.Register(NameOf(EndRulerPoint), GetType(Point), GetType(Angulometer), New FrameworkPropertyMetadata(New Point(), FrameworkPropertyMetadataOptions.AffectsMeasure Or FrameworkPropertyMetadataOptions.AffectsRender, AddressOf RulerPointChangedCallback))
    Public Property EndRulerPoint As Point
        Get
            Return GetValue(EndRulerPointProperty)
        End Get
        Set(value As Point)
            SetValue(EndRulerPointProperty, value)
        End Set
    End Property

    Private Shared Sub RulerPointChangedCallback(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim angulo As Angulometer = d
        angulo.Length = (angulo.StartRulerPoint - angulo.EndRulerPoint).Length
    End Sub

    Private Shared ReadOnly LengthPropertyKey As DependencyPropertyKey = DependencyProperty.RegisterReadOnly(NameOf(Length), GetType(Double), GetType(Angulometer), New PropertyMetadata(0.0))
    Public Shared ReadOnly LengthProperty As DependencyProperty = LengthPropertyKey.DependencyProperty
    Public Property Length As Double
        Get
            Return GetValue(LengthProperty)
        End Get
        Private Set(value As Double)
            SetValue(LengthPropertyKey, value)
        End Set
    End Property

    Public Shared ReadOnly CircleVisibilityProperty As DependencyProperty = DependencyProperty.Register(NameOf(CircleVisibility), GetType(Visibility), GetType(Angulometer), New FrameworkPropertyMetadata(Visibility.Hidden, FrameworkPropertyMetadataOptions.AffectsRender))
    Public Property CircleVisibility As Visibility
        Get
            Return GetValue(CircleVisibilityProperty)
        End Get
        Set(value As Visibility)
            SetValue(CircleVisibilityProperty, value)
        End Set
    End Property

    Public Shared ReadOnly CircleRulerVisibilityProperty As DependencyProperty = DependencyProperty.Register(NameOf(CircleRulerVisibility), GetType(Visibility), GetType(Angulometer), New FrameworkPropertyMetadata(Visibility.Hidden, FrameworkPropertyMetadataOptions.AffectsRender))
    Public Property CircleRulerVisibility As Visibility
        Get
            Return GetValue(CircleRulerVisibilityProperty)
        End Get
        Set(value As Visibility)
            SetValue(CircleRulerVisibilityProperty, value)
        End Set
    End Property

    Public Shared ReadOnly ForceHideCircleProperty As DependencyProperty = DependencyProperty.Register(NameOf(ForceHideCircle), GetType(Boolean), GetType(Angulometer), New FrameworkPropertyMetadata(False, FrameworkPropertyMetadataOptions.AffectsRender, AddressOf ForceHideCircleChangedCallback))
    Public Property ForceHideCircle As Boolean
        Get
            Return GetValue(ForceHideCircleProperty)
        End Get
        Set(value As Boolean)
            SetValue(ForceHideCircleProperty, value)
        End Set
    End Property
    Private Shared Sub ForceHideCircleChangedCallback(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim angulo As Angulometer = d
        If CBool(e.NewValue) Then
            angulo.CircleVisibility = Visibility.Hidden
            angulo.CircleRulerVisibility = Visibility.Hidden
        Else
            Select Case angulo.moveState
                Case MouseState.MoveStart, MouseState.MoveEnd1, MouseState.MoveEnd2
                    angulo.CircleVisibility = Visibility.Visible
                Case MouseState.MoveRuler, MouseState.MoveRulerStart, MouseState.MoveRulerEnd
                    angulo.CircleRulerVisibility = Visibility.Visible
            End Select
        End If
    End Sub

    Public Shared ReadOnly SpecialAngleProperty As DependencyProperty = DependencyProperty.Register(NameOf(SpecialAngle), GetType(Boolean), GetType(Angulometer))
    Public Property SpecialAngle As Boolean
        Get
            Return GetValue(SpecialAngleProperty)
        End Get
        Set(value As Boolean)
            SetValue(SpecialAngleProperty, value)
        End Set
    End Property

    Public Sub New()
        InitializeComponent()
        Me.DataContext = Me
    End Sub

    Private Enum MouseState
        None
        MoveStart
        MoveEnd1
        MoveEnd2
        MoveRuler
        MoveRulerStart
        MoveRulerEnd
    End Enum

    Private moveState As MouseState
    Private oldMouse As Point

    Protected Overrides Sub OnMouseDown(e As MouseButtonEventArgs)
        MyBase.OnMouseDown(e)
        If e.LeftButton = MouseButtonState.Pressed Then
            oldMouse = e.GetPosition(Me)
        End If
    End Sub

    Private Function SpecialMoveAngleEndPoint(start As Point, otherEnd As Point, mouse As Point) As Point
        Const AngleUnit As Double = 15
        Dim actualV = mouse - start
        Dim baseV = otherEnd - start
        Dim oldAng = Vector.AngleBetween(baseV, actualV)
        Dim newArg = AngleUnit * Math.Round(oldAng / AngleUnit) / 180 * Math.PI
        Dim arg = Math.Atan2(baseV.Y, baseV.X) + newArg
        Dim newV As New Vector(actualV.Length * Math.Cos(arg), actualV.Length * Math.Sin(arg))
        Return start + newV
    End Function

    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        MyBase.OnMouseMove(e)
        Dim mouse = e.GetPosition(Me)
        If e.LeftButton <> MouseButtonState.Pressed Then
            If IsNearby(StartPoint, mouse) Then
                moveState = MouseState.MoveStart
            ElseIf IsNearby(EndPoint1, mouse) Then
                moveState = MouseState.MoveEnd1
            ElseIf IsNearby(EndPoint2, mouse) Then
                moveState = MouseState.MoveEnd2
            ElseIf IsNearby(StartRulerPoint, mouse) Then
                moveState = MouseState.MoveRulerStart
            ElseIf IsNearby(EndRulerPoint, mouse) Then
                moveState = MouseState.MoveRulerEnd
            ElseIf IsNearby(StartRulerPoint, EndRulerPoint, mouse) Then
                moveState = MouseState.MoveRuler
            Else
                moveState = MouseState.None
            End If
        End If
        If e.LeftButton = MouseButtonState.Pressed Then
            Select Case moveState
                Case MouseState.MoveStart
                    Dim delta = mouse - StartPoint
                    StartPoint = mouse
                    EndPoint1 += delta
                    EndPoint2 += delta
                Case MouseState.MoveEnd1
                    If SpecialAngle Then
                        EndPoint1 = SpecialMoveAngleEndPoint(StartPoint, EndPoint2, mouse)
                    Else
                        EndPoint1 = mouse
                    End If
                Case MouseState.MoveEnd2
                    If SpecialAngle Then
                        EndPoint2 = SpecialMoveAngleEndPoint(StartPoint, EndPoint1, mouse)
                    Else
                        EndPoint2 = mouse
                    End If
                Case MouseState.MoveRuler
                    Dim delta = mouse - oldMouse
                    StartRulerPoint += delta
                    EndRulerPoint += delta
                    oldMouse = mouse
                Case MouseState.MoveRulerStart
                    StartRulerPoint = mouse
                Case MouseState.MoveRulerEnd
                    EndRulerPoint = mouse
            End Select
        ElseIf Not ForceHideCircle Then
            Select Case moveState
                Case MouseState.MoveStart, MouseState.MoveEnd1, MouseState.MoveEnd2
                    CircleVisibility = Visibility.Visible
                Case MouseState.MoveRuler, MouseState.MoveRulerStart, MouseState.MoveRulerEnd
                    CircleRulerVisibility = Visibility.Visible
                Case Else
                    CircleVisibility = Visibility.Hidden
                    CircleRulerVisibility = Visibility.Hidden
            End Select
        End If
    End Sub
End Class
