Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports Oracle.ManagedDataAccess.Client

Partial Class FILM_LMI_SALES_ORDER_INT
    Inherits System.Web.UI.Page

    Private connectionStringXXTRY As String
    Private connectionStringMIB As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim streamReader As System.IO.StreamReader = New System.IO.StreamReader(Path.Combine(Server.MapPath("~"), "connectionStringXXTRY.txt"))
        While streamReader.Peek() <> -1
            connectionStringXXTRY = streamReader.ReadLine()
        End While
        streamReader.Close()

        streamReader = New System.IO.StreamReader(Path.Combine(Server.MapPath("~"), "connectionStringMIB.txt"))
        While streamReader.Peek() <> -1
            connectionStringMIB = streamReader.ReadLine()
        End While
        streamReader.Close()

        Dim START As DateTime = DateTime.Now

        SP_MIB_SALES_ORDER_SITUATION()

        PSP_MIB_LAST_UPDATE("2", DateTime.Now.ToString("yyyy-MM"), START)

    End Sub

    Public Sub SP_MIB_SALES_ORDER_SITUATION()
        Dim conn = New OracleConnection(connectionStringXXTRY)
        Dim cmd As OracleCommand = New OracleCommand()
        Using conn
            conn.Open()
            cmd.Connection = conn
            cmd.CommandText = "SP_MIB_SALES_ORDER_SITUATION"
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandTimeout = 0
            cmd.Parameters.Clear()
            cmd.Parameters.Add(New OracleParameter("SREFDATA", OracleDbType.RefCursor)).Direction = Data.ParameterDirection.Output
            Dim rdr = cmd.ExecuteReader()
            While rdr.Read
                PSP_SALES_SALES_ORDER_MAINT_INT(rdr("YearMth").ToString, rdr("Prod_line").ToString, rdr("REGION_CD").ToString, rdr("BUDGET").ToString, rdr("FORECAST_QTY").ToString, rdr("DS_QTY").ToString, rdr("LA_QTY").ToString, rdr("BUDGET_AMT").ToString, rdr("FORECAST_AMT").ToString, rdr("DS_AMT").ToString, rdr("LA_AMT").ToString)
            End While
            rdr.Close()
            cmd.Dispose()
        End Using
        conn.Close()
    End Sub

    Public Sub PSP_SALES_SALES_ORDER_MAINT_INT(ByVal YEARMTH As String, ByVal PROD_LINE As String, ByVal REGION_CD As String, ByVal BUDGET As Double,
                                                  ByVal FORECAST_QTY As Double, ByVal DS_QTY As Double, ByVal LA_QTY As Double, ByVal BUDGET_AMT As Double,
                                                  ByVal FORECAST_AMT As Double, ByVal DS_AMT As Double, ByVal LA_AMT As Double)
        Dim conn As SqlConnection = New SqlConnection(connectionStringMIB)
        Dim cmd As SqlCommand = New SqlCommand()
        Using conn
            conn.Open()
            cmd.Connection = conn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "PSP_SALES_SALES_ORDER_MAINT_INT"
            cmd.CommandTimeout = 0
            cmd.Parameters.Clear()
            cmd.Parameters.Add("P_YEARMTH", SqlDbType.VarChar, 25, Data.ParameterDirection.Input).Value = YEARMTH
            cmd.Parameters.Add("P_PROD_LINE", SqlDbType.VarChar, 3, Data.ParameterDirection.Input).Value = PROD_LINE
            cmd.Parameters.Add("P_REGION_CD", SqlDbType.VarChar, 20, Data.ParameterDirection.Input).Value = REGION_CD
            cmd.Parameters.Add("P_BUDGET", SqlDbType.Decimal, Data.ParameterDirection.Input).Value = BUDGET
            cmd.Parameters.Add("P_FORECAST_QTY", SqlDbType.Decimal, Data.ParameterDirection.Input).Value = FORECAST_QTY
            cmd.Parameters.Add("P_DS_QTY", SqlDbType.Decimal, Data.ParameterDirection.Input).Value = DS_QTY
            cmd.Parameters.Add("P_LA_QTY", SqlDbType.Decimal, Data.ParameterDirection.Input).Value = LA_QTY
            cmd.Parameters.Add("P_BUDGET_AMT", SqlDbType.Decimal, Data.ParameterDirection.Input).Value = BUDGET_AMT
            cmd.Parameters.Add("P_FORECAST_AMT", SqlDbType.Decimal, Data.ParameterDirection.Input).Value = FORECAST_AMT
            cmd.Parameters.Add("P_DS_AMT", SqlDbType.Decimal, Data.ParameterDirection.Input).Value = DS_AMT
            cmd.Parameters.Add("P_LA_AMT", SqlDbType.Decimal, Data.ParameterDirection.Input).Value = LA_AMT
            cmd.ExecuteNonQuery()
            cmd.Dispose()
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
