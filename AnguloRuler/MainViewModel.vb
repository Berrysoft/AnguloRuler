Public Class MainViewModel
    Inherits DependencyObject

    Public Shared ReadOnly PlayingProperty As DependencyProperty = DependencyProperty.Register(NameOf(Playing), GetType(Boolean), GetType(MainViewModel), New PropertyMetadata(False, AddressOf PlayingChangedCallback))
    Public Property Playing As Boolean
        Get
            Return GetValue(PlayingProperty)
        End Get
        Set(value As Boolean)
            SetValue(PlayingProperty, value)
        End Set
    End Property
    Private Shared Sub PlayingChangedCallback(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim model As MainViewModel = d
        model.OnPlayingChanged(e.NewValue)
    End Sub
    Public Event PlayingChanged As EventHandler(Of Boolean)
    Protected Sub OnPlayingChanged(e As Boolean)
        RaiseEvent PlayingChanged(Me, e)
    End Sub

    Public Shared ReadOnly BackImagePathProperty As DependencyProperty = DependencyProperty.Register(NameOf(BackImagePath), GetType(String), GetType(MainViewModel))
    Public Property BackImagePath As String
        Get
            Return GetValue(BackImagePathProperty)
        End Get
        Set(value As String)
            SetValue(BackImagePathProperty, value)
        End Set
    End Property

    Public Shared ReadOnly BackVideoPathProperty As DependencyProperty = DependencyProperty.Register(NameOf(BackVideoPath), GetType(String), GetType(MainViewModel))
    Public Property BackVideoPath As String
        Get
            Return GetValue(BackVideoPathProperty)
        End Get
        Set(value As String)
            SetValue(BackVideoPathProperty, value)
        End Set
    End Property

    Public Shared ReadOnly PositionProperty As DependencyProperty = DependencyProperty.Register(NameOf(Position), GetType(TimeSpan), GetType(MainViewModel), New PropertyMetadata(TimeSpan.Zero, AddressOf PositionChangedCallback))
    Public Property Position As TimeSpan
        Get
            Return GetValue(PositionProperty)
        End Get
        Set(value As TimeSpan)
            SetValue(PositionProperty, value)
        End Set
    End Property
    Private Shared Sub PositionChangedCallback(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim model As MainViewModel = d
        model.OnPositionChanged(e.NewValue)
    End Sub
    Public Event PositionChanged As EventHandler(Of TimeSpan)
    Protected Sub OnPositionChanged(e As TimeSpan)
        RaiseEvent PositionChanged(Me, e)
    End Sub
End Class
