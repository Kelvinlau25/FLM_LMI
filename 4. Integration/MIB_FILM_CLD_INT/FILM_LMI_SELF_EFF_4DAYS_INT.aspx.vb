Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports Oracle.ManagedDataAccess.Client

Partial Class FILM_LMI_SELF_EFF_4DAYS_INT
    Inherits System.Web.UI.Page

    Dim connectionStringMIB As String = Nothing
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim ObjReader = New System.IO.StreamReader(Path.Combine(Server.MapPath("~"), "connectionStringMIB.txt"))
        Do While ObjReader.Peek <> -1
            connectionStringMIB = ObjReader.ReadLine
        Loop
        ObjReader.Close()

        Dim START As DateTime = DateTime.Now

        Dim date_Param As DateTime = DateTime.Now.AddDays(-4)

        Dim FMMCCODE As ArrayList = New ArrayList()
        FMMCCODE.Add("F1")
        FMMCCODE.Add("F2")
        FMMCCODE.Add("F3")

        Dim prodData As System.Data.DataTable = GetProdData()

        For i = 0 To 2
            date_Param = date_Param.AddDays(i)
            Dim Year_Month As String = date_Param.ToString("yyyy-MM-dd")

            For ii = 0 To 2
                PVIEW3141(Year_Month, FMMCCODE(ii), date_Param.Day.ToString()) 'NEED DBLINK

                PVIEWFM(Year_Month, FMMCCODE(ii), date_Param.Day.ToString()) ' NEED DBLINK

                PVIEWFM2(Year_Month, FMMCCODE(ii), date_Param.Day.ToString()) 'NEED DBLINK

                PVIEWHOPPER(Year_Month, FMMCCODE(ii), date_Param.Day.ToString()) 'NEED DBLINK

                PVIEW3015(Year_Month, FMMCCODE(ii), date_Param.Day.ToString()) 'NEED DBLINK

                'PVIEW2061(Year_Month, FMMCCODE(ii), date_Param.Day.ToString()) HAVENT DONE. VERY COMPLICATE

                Dim typeData As System.Data.DataTable = GetTypeData(FMMCCODE(ii))

                For Each x As DataRow In prodData.Rows
                    For Each y As DataRow In typeData.Rows
                        CalcSummary(Year_Month, FMMCCODE(ii), x("PROD").ToString(), y("TYPE").ToString(), y("THICK").ToString())
                    Next
                Next

                CalcSummaryTTL(Year_Month, FMMCCODE(ii))

                For Each x As DataRow In prodData.Rows
                    For Each y As DataRow In typeData.Rows
                        CalcRawUsage(Year_Month, FMMCCODE(ii), x("PROD").ToString(), y("TYPE").ToString(), y("THICK").ToString())
                        CalcRawComp(Year_Month, FMMCCODE(ii), x("PROD").ToString(), y("TYPE").ToString(), y("THICK").ToString())
                    Next
                Next

                For Each x As DataRow In prodData.Rows
                    CalcRawCalc(Year_Month, FMMCCODE(ii), x("PROD").ToString())
                Next

                CalcWaste(Year_Month, FMMCCODE(ii), "C")
                CalcWaste(Year_Month, FMMCCODE(ii), "B")

                CalcQty(Year_Month, FMMCCODE(ii))

            Next
        Next

        PSP_MIB_LAST_UPDATE("3", DateTime.Now.ToString("yyyy-MM"), START)
    End Sub

    Public Function GetProdData() As DataTable
        Dim conn = New SqlConnection(connectionStringMIB)
        Dim cmd As SqlCommand = New SqlCommand()
        Dim tbl As DataTable = New DataTable
        Using conn
            conn.Open()
            cmd.Connection = conn
            'cmd.CommandText = "SP_MIB_SELFEF_PROD_SEL"
            cmd.CommandText = "PSP_SELFEF_PROD_SEL"
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandTimeout = 0
            cmd.Parameters().Clear()
            tbl.Load(cmd.ExecuteReader())
            cmd.Dispose()
            conn.Dispose()
        End Using
        conn.Close()
        Return tbl
    End Function

    Public Function GetTypeData(ByVal fmmccode As String) As DataTable
        Dim conn = New SqlConnection(connectionStringMIB)
        Dim cmd As SqlCommand = New SqlCommand()
        Dim tbl As DataTable = New DataTable()
        Using conn
            conn.Open()
            cmd.Connection = conn
            'cmd.CommandText = "SP_MIB_SELFEF_TYPE_SEL"
            cmd.CommandText = "PSP_SELFEF_TYPE_SEL"
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandTimeout = 0
            cmd.Parameters().Clear()
            cmd.Parameters.Add("P_FMMCCODE", SqlDbType.VarChar, 3, Data.ParameterDirection.Input).Value = fmmccode
            tbl.Load(cmd.ExecuteReader())
            cmd.Dispose()
            conn.Dispose()
        End Using
        conn.Close()
        Return tbl
    End Function

    Public Sub CalcQty(ByVal yearmonth As String, ByVal fmmccode As String)
        Dim conn = New SqlConnection(connectionStringMIB)
        Dim cmd As SqlCommand = New SqlCommand()
        Using conn
            conn.Open()
            cmd.Connection = conn
            'cmd.CommandText = "SP_MIB_SELFEF_CALC_QTY"
            cmd.CommandText = "PSP_SELFEF_CALC_QTY_INT"
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandTimeout = 0
            cmd.Parameters().Clear()
            cmd.Parameters.Add("P_YEARMTH", SqlDbType.VarChar, 10, Data.ParameterDirection.Input).Value = yearmonth
            cmd.Parameters.Add("P_FMMCCODE", SqlDbType.VarChar, 3, Data.ParameterDirection.Input).Value = fmmccode
            cmd.ExecuteNonQuery()
            cmd.Dispose()
            conn.Dispose()
        End Using
        conn.Close()
    End Sub

    Public Sub PVIEW3141(ByVal yearmonth As String, ByVal fmmccode As String, ByVal day As String)
        Dim conn = New SqlConnection(connectionStringMIB)
        Dim cmd As SqlCommand = New SqlCommand()
        Using conn
            conn.Open()
            cmd.Connection = conn
            'cmd.CommandText = "SP_MIB_SELFEF_CALC_PVIEW3141"
            cmd.CommandText = "PSP_SELFEF_CALC_PVIEW3141_INT"
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandTimeout = 0
            cmd.Parameters().Clear()
            cmd.Parameters.Add("P_YEARMTH", SqlDbType.VarChar, 10, Data.ParameterDirection.Input).Value = yearmonth
            cmd.Parameters.Add("P_FMMCCODE", SqlDbType.VarChar, 3, Data.ParameterDirection.Input).Value = fmmccode
            cmd.Parameters.Add("P_DAY", SqlDbType.VarChar, 2, Data.ParameterDirection.Input).Value = day
            cmd.ExecuteNonQuery()
            cmd.Dispose()
            conn.Dispose()
        End Using
        conn.Close()
    End Sub

    Public Sub PVIEWFM(ByVal yearmonth As String, ByVal fmmccode As String, ByVal day As String)
        Dim conn = New SqlConnection(connectionStringMIB)
        Dim cmd As SqlCommand = New SqlCommand()
        Using conn
            conn.Open()
            cmd.Connection = conn
            'cmd.CommandText = "SP_MIB_SELFEF_CALC_PVIEWFM"
            cmd.CommandText = "PSP_SELFEF_CALC_PVIEWFM_INT"
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandTimeout = 0
            cmd.Parameters().Clear()
            cmd.Parameters.Add("P_YEARMTH", SqlDbType.VarChar, 10, Data.ParameterDirection.Input).Value = yearmonth
            cmd.Parameters.Add("P_FMMCCODE", SqlDbType.VarChar, 3, Data.ParameterDirection.Input).Value = fmmccode
            cmd.Parameters.Add("P_DAY", SqlDbType.VarChar, 2, Data.ParameterDirection.Input).Value = day
            cmd.ExecuteNonQuery()
            cmd.Dispose()
            conn.Dispose()
        End Using
        conn.Close()
    End Sub

    Public Sub PVIEWFM2(ByVal yearmonth As String, ByVal fmmccode As String, ByVal day As String)
        Dim conn = New SqlConnection(connectionStringMIB)
        Dim cmd As SqlCommand = New SqlCommand()
        Using conn
            conn.Open()
            cmd.Connection = conn
            'cmd.CommandText = "SP_MIB_SELFEF_CALC_PVIEWFM2"
            cmd.CommandText = "PSP_SELEFF_CALC_PVIEWFM2_INT"
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandTimeout = 0
            cmd.Parameters().Clear()
            cmd.Parameters.Add("P_YEARMTH", SqlDbType.VarChar, 10, Data.ParameterDirection.Input).Value = yearmonth
            cmd.Parameters.Add("P_FMMCCODE", SqlDbType.VarChar, 3, Data.ParameterDirection.Input).Value = fmmccode
            cmd.Parameters.Add("P_DAY", SqlDbType.VarChar, 2, Data.ParameterDirection.Input).Value = day
            cmd.ExecuteNonQuery()
            cmd.Dispose()
            conn.Dispose()
        End Using
        conn.Close()
    End Sub

    Public Sub PVIEWHOPPER(ByVal yearmonth As String, ByVal fmmccode As String, ByVal day As String)
        Dim conn = New SqlConnection(connectionStringMIB)
        Dim cmd As SqlCommand = New SqlCommand()
        Using conn
            conn.Open()
            cmd.Connection = conn
            'cmd.CommandText = "SP_MIB_SELFEF_CALC_PVIEWHOPPER"
            cmd.CommandText = "PSP_SELFEF_CALC_PVIEWHOPPER_INT"
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandTimeout = 0
            cmd.Parameters().Clear()
            cmd.Parameters.Add("P_YEARMTH", SqlDbType.VarChar, 10, Data.ParameterDirection.Input).Value = yearmonth
            cmd.Parameters.Add("P_FMMCCODE", SqlDbType.VarChar, 3, Data.ParameterDirection.Input).Value = fmmccode
            cmd.Parameters.Add("P_DAY", SqlDbType.VarChar, 2, Data.ParameterDirection.Input).Value = day
            cmd.ExecuteNonQuery()
            cmd.Dispose()
            conn.Dispose()
        End Using
        conn.Close()
    End Sub

    Public Sub PVIEW3015(ByVal yearmonth As String, ByVal fmmccode As String, ByVal day As String)
        Dim conn = New SqlConnection(connectionStringMIB)
        Dim cmd As SqlCommand = New SqlCommand()
        Using conn
            conn.Open()
            cmd.Connection = conn
            'cmd.CommandText = "SP_MIB_SELFEF_CALC_PVIEW3015"
            cmd.CommandText = "PSP_SELFEF_CALC_PVIEW3015_INT"
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandTimeout = 0
            cmd.Parameters().Clear()
            cmd.Parameters.Add("P_YEARMTH", SqlDbType.VarChar, 10, Data.ParameterDirection.Input).Value = yearmonth
            cmd.Parameters.Add("P_FMMCCODE", SqlDbType.VarChar, 3, Data.ParameterDirection.Input).Value = fmmccode
            cmd.Parameters.Add("P_DAY", SqlDbType.VarChar, 2, Data.ParameterDirection.Input).Value = day
            cmd.ExecuteNonQuery()
            cmd.Dispose()
            conn.Dispose()
        End Using
        conn.Close()
    End Sub

    Public Sub PVIEW2061(ByVal yearmonth As String, ByVal fmmccode As String, ByVal day As String)
        Dim conn = New SqlConnection(connectionStringMIB)
        Dim cmd As SqlCommand = New SqlCommand()
        Using conn
            conn.Open()
            cmd.Connection = conn
            'cmd.CommandText = "SP_MIB_SELFEF_CALC_PVIEW2061"
            cmd.CommandText = "SP_MIB_SELFEF_CALC_PVIEW2061"
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandTimeout = 0
            cmd.Parameters().Clear()
            cmd.Parameters.Add("P_YEARMTH", SqlDbType.VarChar, 10, Data.ParameterDirection.Input).Value = yearmonth
            cmd.Parameters.Add("P_FMMCCODE", SqlDbType.VarChar, 3, Data.ParameterDirection.Input).Value = fmmccode
            cmd.Parameters.Add("P_DAY", SqlDbType.VarChar, 2, Data.ParameterDirection.Input).Value = day
            cmd.ExecuteNonQuery()
            cmd.Dispose()
            conn.Dispose()
        End Using
        conn.Close()
    End Sub

    Public Sub CalcSummary(ByVal yearmonth As String, ByVal fmmccode As String, ByVal prod As String, ByVal type As String, ByVal thick As String)
        Dim conn = New SqlConnection(connectionStringMIB)
        Dim cmd As SqlCommand = New SqlCommand()
        Using conn
            conn.Open()
            cmd.Connection = conn
            'cmd.CommandText = "SP_MIB_SELFEF_CALC_SUMMARY"
            cmd.CommandText = "PSP_SELFEF_CALC_SUMMARY_INT"
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandTimeout = 0
            cmd.Parameters().Clear()
            cmd.Parameters.Add("P_YEARMTH", SqlDbType.VarChar, 10, Data.ParameterDirection.Input).Value = yearmonth
            cmd.Parameters.Add("P_FMMCCODE", SqlDbType.VarChar, 3, Data.ParameterDirection.Input).Value = fmmccode
            cmd.Parameters.Add("P_PROD", SqlDbType.VarChar, 10, Data.ParameterDirection.Input).Value = prod
            cmd.Parameters.Add("P_TYPE", SqlDbType.VarChar, 10, Data.ParameterDirection.Input).Value = type
            cmd.Parameters.Add("P_THICK", SqlDbType.VarChar, 10, Data.ParameterDirection.Input).Value = thick
            cmd.ExecuteNonQuery()
            cmd.Dispose()
            conn.Dispose()
        End Using
        conn.Close()
    End Sub

    Public Sub CalcRawUsage(ByVal yearmonth As String, ByVal fmmccode As String, ByVal prod As String, ByVal type As String, ByVal thick As String)
        Dim conn = New SqlConnection(connectionStringMIB)
        Dim cmd As SqlCommand = New SqlCommand()
        Using conn
            conn.Open()
            cmd.Connection = conn
            'cmd.CommandText = "SP_MIB_SELFEF_CALC_RAWUSAGE"
            cmd.CommandText = "PSP_SELFEF_CALC_RAWUSAGE_INT"
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandTimeout = 0
            cmd.Parameters().Clear()
            cmd.Parameters.Add("P_YEARMTH", SqlDbType.VarChar, 10, Data.ParameterDirection.Input).Value = yearmonth
            cmd.Parameters.Add("P_FMMCCODE", SqlDbType.VarChar, 3, Data.ParameterDirection.Input).Value = fmmccode
            cmd.Parameters.Add("P_PROD", SqlDbType.VarChar, 10, Data.ParameterDirection.Input).Value = prod
            cmd.Parameters.Add("P_TYPE", SqlDbType.VarChar, 10, Data.ParameterDirection.Input).Value = type
            cmd.Parameters.Add("P_THICK", SqlDbType.VarChar, 10, Data.ParameterDirection.Input).Value = thick
            cmd.ExecuteNonQuery()
            cmd.Dispose()
            conn.Dispose()
        End Using
        conn.Close()
    End Sub

    Public Sub CalcRawComp(ByVal yearmonth As String, ByVal fmmccode As String, ByVal prod As String, ByVal type As String, ByVal thick As String)
        Dim conn = New SqlConnection(connectionStringMIB)
        Dim cmd As SqlCommand = New SqlCommand()
        Using conn
            conn.Open()
            cmd.Connection = conn
            'cmd.CommandText = "SP_MIB_SELFEF_CALC_COMP"
            cmd.CommandText = "PSP_SELFEF_CALC_COMP_INT"
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandTimeout = 0
            cmd.Parameters().Clear()
            cmd.Parameters.Add("P_YEARMTH", SqlDbType.VarChar, 10, Data.ParameterDirection.Input).Value = yearmonth
            cmd.Parameters.Add("P_FMMCCODE", SqlDbType.VarChar, 3, Data.ParameterDirection.Input).Value = fmmccode
            cmd.Parameters.Add("P_PROD", SqlDbType.VarChar, 10, Data.ParameterDirection.Input).Value = prod
            cmd.Parameters.Add("P_TYPE", SqlDbType.VarChar, 10, Data.ParameterDirection.Input).Value = type
            cmd.Parameters.Add("P_THICK", SqlDbType.VarChar, 10, Data.ParameterDirection.Input).Value = thick
            cmd.ExecuteNonQuery()
            cmd.Dispose()
            conn.Dispose()
        End Using
        conn.Close()
    End Sub

    Public Sub CalcSummaryTTL(ByVal yearmonth As String, ByVal fmmccode As String)
        Dim conn = New SqlConnection(connectionStringMIB)
        Dim cmd As SqlCommand = New SqlCommand()
        Using conn
            conn.Open()
            cmd.Connection = conn
            'cmd.CommandText = "SP_MIB_SELFEF_CALC_SUMMARYTTL"
            cmd.CommandText = "PSP_SELFEF_CALC_SUMMARYTTL_INT"
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandTimeout = 0
            cmd.Parameters().Clear()
            cmd.Parameters.Add("P_YEARMONTH", SqlDbType.VarChar, 10, Data.ParameterDirection.Input).Value = yearmonth
            cmd.Parameters.Add("P_FMMCCODE", SqlDbType.VarChar, 3, Data.ParameterDirection.Input).Value = fmmccode
            cmd.ExecuteNonQuery()
            cmd.Dispose()
            conn.Dispose()
        End Using
        conn.Close()
    End Sub

    Public Sub CalcRawCalc(ByVal yearmonth As String, ByVal fmmccode As String, ByVal prod As String)
        Dim conn = New SqlConnection(connectionStringMIB)
        Dim cmd As SqlCommand = New SqlCommand()
        Using conn
            conn.Open()
            cmd.Connection = conn
            'cmd.CommandText = "SP_MIB_SELFEF_CALC_RAWCALC"
            cmd.CommandText = "PSP_SELFEF_CALC_RAWCALC_INT"
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandTimeout = 0
            cmd.Parameters().Clear()
            cmd.Parameters.Add("P_YEARMONTH", SqlDbType.VarChar, 10, Data.ParameterDirection.Input).Value = yearmonth
            cmd.Parameters.Add("P_FMMCCODE", SqlDbType.VarChar, 3, Data.ParameterDirection.Input).Value = fmmccode
            cmd.Parameters.Add("P_PROD", SqlDbType.VarChar, 10, Data.ParameterDirection.Input).Value = prod
            cmd.ExecuteNonQuery()
            cmd.Dispose()
            conn.Dispose()
        End Using
        conn.Close()
    End Sub

    Public Sub CalcWaste(ByVal yearmonth As String, ByVal fmmccode As String, ByVal bctype As String)
        Dim conn = New SqlConnection(connectionStringMIB)
        Dim cmd As SqlCommand = New SqlCommand()
        Using conn
            conn.Open()
            cmd.Connection = conn
            'cmd.CommandText = "SP_MIB_SELFEF_CALC_WASTE"
            cmd.CommandText = "PSP_SELFEF_CALC_WASTE_INT"
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandTimeout = 0
            cmd.Parameters().Clear()
            cmd.Parameters.Add("P_YEARMTH", SqlDbType.VarChar, 10, Data.ParameterDirection.Input).Value = yearmonth
            cmd.Parameters.Add("P_FMMCCODE", SqlDbType.VarChar, 3, Data.ParameterDirection.Input).Value = fmmccode
            cmd.Parameters.Add("P_BCTYPE", SqlDbType.VarChar, 1, Data.ParameterDirection.Input).Value = bctype
            cmd.ExecuteNonQuery()
            cmd.Dispose()
            conn.Dispose()
        End Using
        conn.Close()
    End Sub

    Public Sub PSP_MIB_LAST_UPDATE(ByVal P_CHART_ID As String, ByVal P_DATE As String, ByVal Start As DateTime)
        Dim _conn As SqlConnection = New SqlConnection(connectionStringMIB)
        Dim cmd As SqlCommand = New SqlCommand()
        Using _conn
            _conn.Open()
            cmd.Connection = _conn
            cmd.CommandText = "PSP_MIB_LAST_UPDATE"
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandTimeout = 0
            cmd.Parameters.Clear()
            cmd.Parameters.Add("P_CHART_ID", SqlDbType.VarChar, 50, Data.ParameterDirection.Input).Value = P_CHART_ID
            cmd.Parameters.Add("P_DATE", SqlDbType.VarChar, 50, Data.ParameterDirection.Input).Value = P_DATE
            cmd.Parameters.Add("P_UPDATE_DATE", SqlDbType.VarChar, 50, Data.ParameterDirection.Input).Value = DateTime.Now.ToString("dd MMM yyyy HH:mm:ss")
            cmd.Parameters.Add("P_START_DATE", SqlDbType.VarChar, 50, Data.ParameterDirection.Input).Value = Start.ToString("dd MMM yyyy HH:mm:ss")
            cmd.ExecuteNonQuery()
            cmd.Dispose()
        End Using
    End Sub

End Class