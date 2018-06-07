Imports System.Configuration
Imports System.Data.SqlClient
Imports QLPMT_DTO
Imports Utility

Public Class LoaiGioiTinhDAL
    Private connectionString As String

    Public Sub New()
        ' Read ConnectionString value from App.config file
        connectionString = ConfigurationManager.AppSettings("ConnectionString")
    End Sub
    Public Sub New(ConnectionString As String)
        Me.connectionString = ConnectionString
    End Sub

    Public Function buildMaLoaiGioiTinh(ByRef nextMaLoaiGioiTinh As String) As Result

        nextMaLoaiGioiTinh = String.Empty
        Dim y = DateTime.Now.Year
        Dim x = y.ToString().Substring(2)
        nextMaLoaiGioiTinh = x + "000000"

        Dim query As String = String.Empty
        query &= "SELECT TOP 1 [maloaigioitinh] "
        query &= "FROM [tblLoaiGioiTinh] "
        query &= "ORDER BY [maloaigioitinh] DESC"

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
                            msOnDB = reader("maloaigioitinh")
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
                        nextMaLoaiGioiTinh = year.ToString()
                        Dim v = msOnDB.Substring(2)
                        Dim convertDecimal = Convert.ToDecimal(v)
                        convertDecimal = convertDecimal + 1
                        Dim tmp = convertDecimal.ToString()
                        tmp = tmp.PadLeft(msOnDB.Length - 2, "0")
                        nextMaLoaiGioiTinh = nextMaLoaiGioiTinh + tmp
                        System.Console.WriteLine(nextMaLoaiGioiTinh)
                    End If

                Catch ex As Exception
                    conn.Close()
                    System.Console.WriteLine(ex.StackTrace)
                    Return New Result(False, "Lấy tự động mã loại giới tính kế tiếp không thành công!", ex.StackTrace)
                End Try
            End Using
        End Using
        Return New Result(True)
    End Function

    Public Function selectALL(ByRef listLoaiGioiTinh As List(Of LoaiGioiTinhDTO)) As Result

        Dim query As String = String.Empty
        query &= "SELECT [maloaigioitinh], [tenloaigioitinh] "
        query &= "FROM [tblLoaiGioiTinh]"

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
                        listLoaiGioiTinh.Clear()
                        While reader.Read()
                            listLoaiGioiTinh.Add(New LoaiGioiTinhDTO(reader("maloaigioitinh"), reader("tenloaigioitinh")))
                        End While
                    End If
                Catch ex As Exception
                    Console.WriteLine(ex.StackTrace)
                    conn.Close()
                    Return New Result(False, "Lấy tất cả loại giới tính không thành công!", ex.StackTrace)
                End Try
            End Using
        End Using
        Return New Result(True)
    End Function

    Public Function insert(lgt As LoaiGioiTinhDTO) As Result

        Dim query As String = String.Empty
        query &= "INSERT INTO [tblLoaiGioiTinh] ([maloaigioitinh], [tenloaigioitinh]) "
        query &= "VALUES (@maloaigioitinh,@tenloaigioitinh)"

        'get MaLoaiGioiTinh
        Dim nextMaLoaiGioiTinh = "1"
        buildMaLoaiGioiTinh(nextMaLoaiGioiTinh)
        lgt.MaLoaiGioiTinh = nextMaLoaiGioiTinh

        Using conn As New SqlConnection(connectionString)
            Using comm As New SqlCommand()
                With comm
                    .Connection = conn
                    .CommandType = CommandType.Text
                    .CommandText = query
                    .Parameters.AddWithValue("@maloaigioitinh", lgt.MaLoaiGioiTinh)
                    .Parameters.AddWithValue("@tenloaigioitinh", lgt.TenLoaiGioiTinh)
                End With
                Try
                    conn.Open()
                    comm.ExecuteNonQuery()
                Catch ex As Exception
                    conn.Close()
                    Return New Result(False, "Thêm loại giới tính không thành công!", ex.StackTrace)
                End Try
            End Using
        End Using
        Return New Result(True)
    End Function

    Public Function update(lgt As LoaiGioiTinhDTO) As Result

        Dim query As String = String.Empty
        query &= "UPDATE [tblLoaiGioiTinh] SET "
        query &= "[tenloaigioitinh] = @tenloaigioitinh "
        query &= "WHERE "
        query &= "[maloaigioitinh] = @maloaigioitinh"

        Using conn As New SqlConnection(connectionString)
            Using comm As New SqlCommand()
                With comm
                    .Connection = conn
                    .CommandType = CommandType.Text
                    .CommandText = query
                    .Parameters.AddWithValue("@maloaigioitinh", lgt.MaLoaiGioiTinh)
                    .Parameters.AddWithValue("@tenloaigioitinh", lgt.TenLoaiGioiTinh)
                End With
                Try
                    conn.Open()
                    comm.ExecuteNonQuery()
                Catch ex As Exception
                    Console.WriteLine(ex.StackTrace)
                    conn.Close()
                    Return New Result(False, "Cập nhật loại giới tính không thành công!", ex.StackTrace)
                End Try
            End Using
        End Using
        Return New Result(True)
    End Function

    Public Function delete(maLoai As Integer) As Result

        Dim query As String = String.Empty
        query &= "DELETE FROM [tblLoaiGioiTinh] "
        query &= "WHERE "
        query &= "[maloaigioitinh] = @maloaigioitinh"

        Using conn As New SqlConnection(connectionString)
            Using comm As New SqlCommand()
                With comm
                    .Connection = conn
                    .CommandType = CommandType.Text
                    .CommandText = query
                    .Parameters.AddWithValue("@maloaigioitinh", maLoai)
                End With
                Try
                    conn.Open()
                    comm.ExecuteNonQuery()
                Catch ex As Exception
                    Console.WriteLine(ex.StackTrace)
                    conn.Close()
                    Return New Result(False, "Xóa loại giới tính không thành công!", ex.StackTrace)
                End Try
            End Using
        End Using
        Return New Result(True)
    End Function
End Class
