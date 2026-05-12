Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports Oracle.ManagedDataAccess.Client

Partial Class FILM_LMI_STOCK_CONTROL_INT
    Inherits System.Web.UI.Page

    Dim connectionStringXXTRY As String = Nothing
    Dim connectionStringMIB As String = Nothing
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim ObjReader As New System.IO.StreamReader(Path.Combine(Server.MapPath("~"), "connectionStringXXTRY.txt"))
        Do While ObjReader.Peek <> -1
            connectionStringXXTRY = ObjReader.ReadLine
        Loop
        ObjReader.Close()

        ObjReader = New System.IO.StreamReader(Path.Combine(Server.MapPath("~"), "connectionStringMIB.txt"))
        Do While ObjReader.Peek <> -1
            connectionStringMIB = ObjReader.ReadLine
        Loop
        ObjReader.Close()

        Dim START As DateTime = DateTime.Now

        SP_MIB_STOCK_CONTROL_LIST()

        PSP_MIB_LAST_UPDATE("3", DateTime.Now.ToString("yyyy-MM"), START)
    End Sub

    Public Sub SP_MIB_STOCK_CONTROL_LIST()
        Dim conn = New OracleConnection(connectionStringXXTRY)
        Dim cmd As OracleCommand = New OracleCommand()
        Using conn
            conn.Open()
            cmd.Connection = conn
            cmd.CommandText = "SP_MIB_STOCK_CONTROL_LIST"
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandTimeout = 0
            cmd.Parameters().Clear()
            cmd.Parameters.Add(New OracleParameter("SREFDATA", OracleDbType.RefCursor)).Direction = 2
            Dim rdr As OracleDataReader = cmd.ExecuteReader()
            While rdr.Read()
                PSP_INV_STOCK_CONTROL_MAINT_INT(rdr("SDATE").ToString(), rdr("PROD_LINE").ToString(), rdr("PROD_GROUP").ToString(), rdr("SV").ToString(), rdr("PICK").ToString(), rdr("HOLD").ToString(), rdr("NOINCOMING").ToString(), rdr("ASG").ToString(), rdr("ATP").ToString(), rdr("NORECEIVING").ToString())
            End While
            RDR.Close()
            cmd.Dispose()
        End Using
    End Sub

    Public Sub PSP_INV_STOCK_CONTROL_MAINT_INT(ByVal SDATE As String, ByVal PROD_LINE As String, ByVal PROD_GROUP As String, ByVal SV As Double, ByVal PICK As Double, ByVal HOLD As Double, ByVal NO_INC As Double, ByVal ASG As Double, ByVal ATP As Double, ByVal NORECEIVING As Double)
        Dim conn As SqlConnection = New SqlConnection(connectionStringMIB)
        Dim cmd As SqlCommand = New SqlCommand()
        Using conn
            conn.Open()
            cmd.Connection = conn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "PSP_INV_STOCK_CONTROL_MAINT_INT"
            cmd.Parameters.Add("P_SDATE", SqlDbType.VarChar, 7, Data.ParameterDirection.Input).Value = SDATE.Substring(0, 4) & "-" & SDATE.Substring(4)
            cmd.Parameters.Add("P_PROD_LINE", SqlDbType.VarChar, 3, Data.ParameterDirection.Input).Value = PROD_LINE
            cmd.Parameters.Add("P_PROD_GROUP", SqlDbType.VarChar, 20, Data.ParameterDirection.Input).Value = PROD_GROUP
            cmd.Parameters.Add("P_SV", SqlDbType.Decimal, Data.ParameterDirection.Input).Value = SV
            cmd.Parameters.Add("P_PICK", SqlDbType.Decimal, Data.ParameterDirection.Input).Value = PICK
            cmd.Parameters.Add("P_HOLD", SqlDbType.Decimal, Data.ParameterDirection.Input).Value = HOLD
            cmd.Parameters.Add("P_NO_INC", SqlDbType.Decimal, Data.ParameterDirection.Input).Value = NO_INC
            cmd.Parameters.Add("P_ASG", SqlDbType.Decimal, Data.ParameterDirection.Input).Value = ASG
            cmd.Parameters.Add("P_ATP", SqlDbType.Decimal, Data.ParameterDirection.Input).Value = ATP
            cmd.Parameters.Add("P_NORECEIVING", SqlDbType.Decimal, Data.ParameterDirection.Input).Value = NORECEIVING
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