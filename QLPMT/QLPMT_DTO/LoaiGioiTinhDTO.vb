Public Class LoaiGioiTinhDTO
    Private strMaLoaiGioiTinh As String
    Private strTenLoaiGioiTinh As String

    Public Sub New()
        With Me
            .strMaLoaiGioiTinh = String.Empty
            .strTenLoaiGioiTinh = String.Empty
        End With
    End Sub

    Public Sub New(strMaLoaiGioiTinh As String, strTenLoaiGioiTinh As String)
        With Me
            .strMaLoaiGioiTinh = strMaLoaiGioiTinh
            .strTenLoaiGioiTinh = strTenLoaiGioiTinh
        End With
    End Sub

    Public Property MaLoaiGioiTinh As String
        Get
            Return strMaLoaiGioiTinh
        End Get
        Set(value As String)
            strMaLoaiGioiTinh = value
        End Set
    End Property

    Public Property TenLoaiGioiTinh As String
        Get
            Return strTenLoaiGioiTinh
        End Get
        Set(value As String)
            strTenLoaiGioiTinh = value
        End Set
    End Property
End Class
