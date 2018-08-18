Module Helper
    Public Function IsNearby(center As Point, mouse As Point) As Boolean
        Dim dx = center.X - mouse.X
        Dim dy = center.Y - mouse.Y
        Return dx >= -5 AndAlso dx <= 5 AndAlso dy >= -5 AndAlso dy <= 5
    End Function

    Public Function GetArcAngle(startp As Point, endp1 As Point, endp2 As Point) As Double
        Return Math.Abs(Vector.AngleBetween(endp2 - startp, endp1 - startp)) / 180 * Math.PI
    End Function

    Public Function IsNearby(p1 As Point, p2 As Point, mouse As Point) As Boolean
        Dim a As Double = p2.Y - p1.Y
        Dim b As Double = p1.X - p2.X
        Dim c As Double = p2.X * p1.Y - p1.X * p2.Y
        Dim l As Double = Math.Sqrt(a * a + b * b)
        Dim distance As Double = Math.Abs(a * mouse.X + b * mouse.Y + c) / l
        Return distance < 3 AndAlso mouse.X >= Math.Min(p1.X, p2.X) AndAlso mouse.X <= Math.Max(p1.X, p2.X)
    End Function
End Module
