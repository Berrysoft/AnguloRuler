Module Helper
    Public Function IsNearby(center As Point, mouse As Point) As Boolean
        Dim dx = center.X - mouse.X
        Dim dy = center.Y - mouse.Y
        Return dx >= -5 AndAlso dx <= 5 AndAlso dy >= -5 AndAlso dy <= 5
    End Function

    Public Function GetArcAngle(startp As Point, endp1 As Point, endp2 As Point) As Double
        Return Math.Abs(Vector.AngleBetween(endp2 - startp, endp1 - startp)) / 180 * Math.PI
    End Function
End Module
