Public Class BenhNhanDTO
    Private strMaBenhNhan As String
    Private strHoTen As String
    Private strMaLoaiGioiTinh As String
    Private iNamSinh As Integer
    Private strDiaChi As String

    Public Sub New()
        With Me
            .strMaBenhNhan = String.Empty
            .strHoTen = String.Empty
            .strMaLoaiGioiTinh = String.Empty
            .iNamSinh = 0
            .strDiaChi = String.Empty
        End With
    End Sub

    Public Sub New(strMaBenhNhan As String, strHoTen As String, strMaLoaiGioiTinh As String, iNamSinh As Integer, strDiaChi As String)
        With Me
            .strMaBenhNhan = strMaBenhNhan
            .strHoTen = strHoTen
            .strMaLoaiGioiTinh = strMaLoaiGioiTinh
            .iNamSinh = iNamSinh
            .strDiaChi = strDiaChi
        End With
    End Sub

    Public Property MaBenhNhan As String
        Get
            Return strMaBenhNhan
        End Get
        Set(value As String)
            strMaBenhNhan = value
        End Set
    End Property

    Public Property HoTen As String
        Get
            Return strHoTen
        End Get
        Set(value As String)
            strHoTen = value
        End Set
    End Property

    Public Property MaLoaiGioiTinh As String
        Get
            Return strMaLoaiGioiTinh
        End Get
        Set(value As String)
            strMaLoaiGioiTinh = value
        End Set
    End Property

    Public Property NamSinh As Integer
        Get
            Return iNamSinh
        End Get
        Set(value As Integer)
            iNamSinh = value
        End Set
    End Property

    Public Property DiaChi As String
        Get
            Return strDiaChi
        End Get
        Set(value As String)
            strDiaChi = value
        End Set
    End Property
End Class
