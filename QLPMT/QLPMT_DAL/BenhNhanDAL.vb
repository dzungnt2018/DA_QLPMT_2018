Imports System.Configuration
Imports System.Data.SqlClient
Imports QLPMT_DTO
Imports Utility

Public Class BenhNhanDAL
    Private connectionString As String

    Public Sub New()
        ' Read ConnectionString value from App.config file
        connectionString = ConfigurationManager.AppSettings("ConnectionString")
    End Sub

    Public Sub New(ConnectionString As String)
        Me.connectionString = ConnectionString
    End Sub

    Public Function buildMaBenhNhan(ByRef nextMaBenhNhan As String) As Result

        nextMaBenhNhan = String.Empty
        Dim y = DateTime.Now.Year
        Dim x = y.ToString().Substring(2)
        nextMaBenhNhan = x + "000000"

        Dim query As String = String.Empty
        query &= "SELECT TOP 1 [mabenhnhan] "
        query &= "FROM [tblBenhNhan] "
        query &= "ORDER BY [mabenhnhan] DESC"

        Using conn As New SqlConnection(connectionString)
            Using comm As New SqlCommand()
                With comm
                    .Connection = conn
                    .CommandType = CommandType.Text
                    .CommandText = query
                End With
                Try
                    conn.Open()
                    Dim reader As SqlDataReader
                    reader = comm.ExecuteReader()
                    Dim msOnDB As String
                    msOnDB = Nothing
                    If reader.HasRows = True Then
                        While reader.Read()
                            msOnDB = reader("mabenhnhan")
                        End While
                    End If
                    If (msOnDB <> Nothing And msOnDB.Length >= 8) Then
                        Dim currentYear = DateTime.Now.Year.ToString().Substring(2)
                        Dim iCurrentYear = Integer.Parse(currentYear)
                        Dim currentYearOnDB = msOnDB.Substring(0, 2)
                        Dim icurrentYearOnDB = Integer.Parse(currentYearOnDB)
                        Dim year = iCurrentYear
                        If (year < icurrentYearOnDB) Then
                            year = icurrentYearOnDB
                        End If
                        nextMaBenhNhan = year.ToString()
                        Dim v = msOnDB.Substring(2)
                        Dim convertDecimal = Convert.ToDecimal(v)
                        convertDecimal = convertDecimal + 1
                        Dim tmp = convertDecimal.ToString()
                        tmp = tmp.PadLeft(msOnDB.Length - 2, "0")
                        nextMaBenhNhan = nextMaBenhNhan + tmp
                        System.Console.WriteLine(nextMaBenhNhan)
                    End If

                Catch ex As Exception
                    conn.Close()
                    System.Console.WriteLine(ex.StackTrace)
                    Return New Result(False, "Lấy tự động mã bệnh nhân kế tiếp không thành công!", ex.StackTrace)
                End Try
            End Using
        End Using
        Return New Result(True)
    End Function

    Public Function selectALL(ByRef listBenhNhan As List(Of BenhNhanDTO)) As Result

        Dim query As String = String.Empty
        query &= "SELECT [mabenhnhan], [hoten], [maloaigioitinh], [namsinh], [diachi] "
        query &= "FROM [tblBenhNhan]"

        Using conn As New SqlConnection(connectionString)
            Using comm As New SqlCommand()
                With comm
                    .Connection = conn
                    .CommandType = CommandType.Text
                    .CommandText = query
                End With
                Try
                    conn.Open()
                    Dim reader As SqlDataReader
                    reader = comm.ExecuteReader()
                    If reader.HasRows = True Then
                        listBenhNhan.Clear()
                        While reader.Read()
                            listBenhNhan.Add(New BenhNhanDTO(reader("mabenhnhan"), reader("hoten"), reader("maloaigioitinh"), reader("namsinh"), reader("diachi")))
                        End While
                    End If

                Catch ex As Exception
                    conn.Close()
                    System.Console.WriteLine(ex.StackTrace)
                    Return New Result(False, "Lấy tất cả bệnh nhân không thành công!", ex.StackTrace)
                End Try
            End Using
        End Using
        Return New Result(True)
    End Function

    Public Function insert(bn As BenhNhanDTO) As Result

        Dim query As String = String.Empty
        query &= "INSERT INTO [tblBenhNhan] ([mabenhnhan], [hoten], [maloaigioitinh], [namsinh], [diachi]) "
        query &= "VALUES (@mabenhnhan,@hoten,@maloaigioitinh,@namsinh,@diachi)"

        'get MSHS
        Dim nextMaBenhNhan = "1"
        buildMaBenhNhan(nextMaBenhNhan)
        bn.MaBenhNhan = nextMaBenhNhan

        Using conn As New SqlConnection(connectionString)
            Using comm As New SqlCommand()
                With comm
                    .Connection = conn
                    .CommandType = CommandType.Text
                    .CommandText = query
                    .Parameters.AddWithValue("@mabenhnhan", bn.MaBenhNhan)
                    .Parameters.AddWithValue("@hoten", bn.HoTen)
                    .Parameters.AddWithValue("@maloaigioitinh", bn.MaLoaiGioiTinh)
                    .Parameters.AddWithValue("@namsinh", bn.NamSinh)
                    .Parameters.AddWithValue("@diachi", bn.DiaChi)
                End With
                Try
                    conn.Open()
                    comm.ExecuteNonQuery()
                Catch ex As Exception
                    conn.Close()
                    System.Console.WriteLine(ex.StackTrace)
                    Return New Result(False, "Thêm bệnh nhân không thành công!", ex.StackTrace)
                End Try
            End Using
        End Using
        Return New Result(True)
    End Function

    Public Function update(bn As BenhNhanDTO) As Result

        Dim query As String = String.Empty
        query &= "UPDATE [tblBenhNhan] SET "
        query &= "[hoten] = @hoten"
        query &= ",[maloaigioitinh] = @maloaigioitinh"
        query &= ",[namsinh] = @namsinh"
        query &= ",[diachi] = @diachi"
        query &= "WHERE "
        query &= "[mabenhnhan] = @mabenhnhan"

        Using conn As New SqlConnection(connectionString)
            Using comm As New SqlCommand()
                With comm
                    .Connection = conn
                    .CommandType = CommandType.Text
                    .CommandText = query
                    .Parameters.AddWithValue("@hoten", bn.HoTen)
                    .Parameters.AddWithValue("@maloaigioitinh", bn.MaLoaiGioiTinh)
                    .Parameters.AddWithValue("@namsinh", bn.NamSinh)
                    .Parameters.AddWithValue("@diachi", bn.DiaChi)
                    .Parameters.AddWithValue("@mabenhnhan", bn.MaBenhNhan)
                End With
                Try
                    conn.Open()
                    comm.ExecuteNonQuery()
                Catch ex As Exception
                    Console.WriteLine(ex.StackTrace)
                    conn.Close()
                    System.Console.WriteLine(ex.StackTrace)
                    Return New Result(False, "Cập nhật bệnh nhân không thành công!", ex.StackTrace)
                End Try
            End Using
        End Using
        Return New Result(True)
    End Function

    Public Function delete(maBenhNhan As String) As Result

        Dim query As String = String.Empty
        query &= "DELETE FROM [tblBenhNhan] "
        query &= "WHERE "
        query &= "[mabenhnhan] = @mabenhnhan"

        Using conn As New SqlConnection(connectionString)
            Using comm As New SqlCommand()
                With comm
                    .Connection = conn
                    .CommandType = CommandType.Text
                    .CommandText = query
                    .Parameters.AddWithValue("@mabenhnhan", maBenhNhan)
                End With
                Try
                    conn.Open()
                    comm.ExecuteNonQuery()
                Catch ex As Exception
                    Console.WriteLine(ex.StackTrace)
                    conn.Close()
                    System.Console.WriteLine(ex.StackTrace)
                    Return New Result(False, "Xóa bệnh nhân không thành công!", ex.StackTrace)
                End Try
            End Using
        End Using
        Return New Result(True)
    End Function
End Class