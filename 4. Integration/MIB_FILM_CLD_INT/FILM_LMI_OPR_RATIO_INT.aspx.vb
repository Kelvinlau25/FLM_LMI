Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports Oracle.ManagedDataAccess.Client

Partial Class FILM_LMI_OPR_RATIO_INT
    Inherits System.Web.UI.Page

    Dim connectionStringPFRACT As String = Nothing
    Dim connectionStringMIB As String = Nothing
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim ObjReader As New System.IO.StreamReader(Path.Combine(Server.MapPath("~"), "connectionStringPFRACT.txt"))
        Do While ObjReader.Peek <> -1
            connectionStringPFRACT = ObjReader.ReadLine
        Loop
        ObjReader.Close()

        ObjReader = New System.IO.StreamReader(Path.Combine(Server.MapPath("~"), "connectionStringMIB.txt"))
        Do While ObjReader.Peek <> -1
            connectionStringMIB = ObjReader.ReadLine
        Loop
        ObjReader.Close()

        Dim START As DateTime = DateTime.Now

        SP_MIB_OPR_RATIO_INTEGRATION()

        OPR_RATIO_TRANS_DATE_INTEGRATION()

        PSP_MIB_LAST_UPDATE("1", DateTime.Now.ToString("yyyy-MM"), START)
    End Sub

    Public Sub OPR_RATIO_TRANS_DATE_INTEGRATION()
        Dim conn As New OracleConnection(connectionStringPFRACT)
        Dim cmd As OracleCommand = New OracleCommand()
        Using conn
            conn.Open()
            cmd.Connection = conn
            cmd.CommandText = "select TO_CHAR(t.TRANS_TIME, 'yyyy-MM-dd HH24:mi') AS TRANS_TIME, t.FILMMAKINGMACHINECODE from PVIEW_GET_TRANS_DATE t"
            cmd.CommandType = CommandType.Text
            cmd.CommandTimeout = 0
            cmd.Parameters.Clear()
            Dim rdr = cmd.ExecuteReader()
            While rdr.Read
                PSP_SALES_OPR_RATIO_TRANS_DATE_MAINT_INT(rdr("TRANS_TIME").ToString, rdr("FILMMAKINGMACHINECODE").ToString)
            End While
            rdr.Close()
            cmd.Dispose()
        End Using
    End Sub

    Public Sub SP_MIB_OPR_RATIO_INTEGRATION()
        Dim conn As New OracleConnection(connectionStringPFRACT)
        Dim cmd As OracleCommand = New OracleCommand()
        Using conn
            conn.Open()
            cmd.Connection = conn
            cmd.CommandText = "SP_MIB_OPR_RATIO_INTEGRATION"
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandTimeout = 0
            cmd.Parameters.Clear()
            cmd.Parameters.Add(New OracleParameter("SREFDATA", OracleDbType.RefCursor)).Direction = Data.ParameterDirection.Output
            Dim rdr = cmd.ExecuteReader()
            While rdr.Read
                PSP_SALES_OPR_RATIO_MAINT_INT(rdr("YEAR_MONTH").ToString, rdr("FILMMAKINGMACHINECODE").ToString, rdr("DAY_HOURS").ToString, rdr("USEDTIME").ToString)
            End While
            rdr.Close()
            cmd.Dispose()
        End Using
    End Sub

    Public Sub PSP_SALES_OPR_RATIO_MAINT_INT(ByVal Year_Month As String, ByVal FMMCCODE As String, ByVal Day_Hours As Double, ByVal usedTime As Double)
        Dim conn As SqlConnection = New SqlConnection(connectionStringMIB)
        Dim cmd As SqlCommand = New SqlCommand()
        Using conn
            conn.Open()
            cmd.Connection = conn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "PSP_SALES_OPR_RATIO_MAINT_INT"
            cmd.Parameters.Add("P_YEAR_MONTH", SqlDbType.VarChar, 10, Data.ParameterDirection.Input).Value = Year_Month
            cmd.Parameters.Add("P_FMMCCODE", SqlDbType.VarChar, 3, Data.ParameterDirection.Input).Value = FMMCCODE
            cmd.Parameters.Add("P_DAY_HOURS", SqlDbType.Decimal, Data.ParameterDirection.Input).Value = Day_Hours
            cmd.Parameters.Add("P_USED_TIME", SqlDbType.Decimal, Data.ParameterDirection.Input).Value = usedTime
            cmd.ExecuteNonQuery()
            cmd.Dispose()
        End Using
    End Sub

    Public Sub PSP_SALES_OPR_RATIO_TRANS_DATE_MAINT_INT(ByVal TRANS_DATE As String, ByVal FMMCCODE As String)
        Dim conn As SqlConnection = New SqlConnection(connectionStringMIB)
        Dim cmd As SqlCommand = New SqlCommand()
        Using conn
            conn.Open()
            cmd.Connection = conn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "PSP_SALES_OPR_RATIO_TRANS_DATE_MAINT_INT"
            cmd.Parameters.Add("P_TRANS_DATE", SqlDbType.DateTime, Data.ParameterDirection.Input).Value = TRANS_DATE
            cmd.Parameters.Add("P_FMMCCODE", SqlDbType.VarChar, 3, Data.ParameterDirection.Input).Value = FMMCCODE
            cmd.ExecuteNonQuery()
            cmd.Dispose()
        End Using
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