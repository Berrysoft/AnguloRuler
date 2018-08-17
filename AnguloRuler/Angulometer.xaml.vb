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

    Public Shared ReadOnly CircleVisibilityProperty As DependencyProperty = DependencyProperty.Register(NameOf(CircleVisibility), GetType(Visibility), GetType(Angulometer), New FrameworkPropertyMetadata(Visibility.Hidden, FrameworkPropertyMetadataOptions.AffectsRender))
    Public Property CircleVisibility As Visibility
        Get
            Return GetValue(CircleVisibilityProperty)
        End Get
        Set(value As Visibility)
            SetValue(CircleVisibilityProperty, value)
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
        ElseIf angulo.moveState <> MouseState.None Then
            angulo.CircleVisibility = Visibility.Visible
        End If
    End Sub

    Public Sub New()
        InitializeComponent()
        Me.DataContext = Me
    End Sub

    Private Enum MouseState
        None
        MoveStart
        MoveEnd1
        MoveEnd2
    End Enum

    Private moveState As MouseState

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
                    EndPoint1 = mouse
                Case MouseState.MoveEnd2
                    EndPoint2 = mouse
            End Select
        Else
            If moveState <> MouseState.None AndAlso Not ForceHideCircle Then
                CircleVisibility = Visibility.Visible
            Else
                CircleVisibility = Visibility.Hidden
            End If
        End If
    End Sub
End Class
