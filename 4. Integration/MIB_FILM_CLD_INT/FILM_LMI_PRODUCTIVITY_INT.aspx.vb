Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports Oracle.ManagedDataAccess.Client

Partial Class FILM_LMI_PRODUCTIVITY_INT
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

        SP_MIB_PRODUCTION_INTEGRATION2()

        SP_MIB_PRODUCTION_ALL2()

        PSP_MIB_LAST_UPDATE("4", DateTime.Now.ToString("yyyy-MM"), START)
        PSP_MIB_LAST_UPDATE("5", DateTime.Now.ToString("yyyy-MM"), START)
    End Sub

    Public Sub SP_MIB_PRODUCTION_ALL2()
        Dim conn = New OracleConnection(connectionStringPFRACT)
        Dim cmd As OracleCommand = New OracleCommand()
        Using conn
            conn.Open()
            cmd.Connection = conn
            cmd.CommandText = "SP_MIB_PRODUCTION_ALL2"
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandTimeout = 0
            cmd.Parameters.Clear()
            cmd.Parameters.Add(New OracleParameter("SREFDATA", OracleDbType.RefCursor)).Direction = Data.ParameterDirection.Output
            Dim rdr = cmd.ExecuteReader()
            While rdr.Read
                PSP_PRODUCTION_CR_YIELD_MAINT_INT(rdr("SDATE").ToString, rdr("FILMMAKINGMACHINECODE").ToString, rdr("BUDGET_MILLROLL_WEIGHT").ToString, rdr("BUDGET_EXTRD_WEIGHT").ToString, rdr("RESULT_MILLROLL_WEIGHT").ToString, rdr("RESULT_EXTRD_WEIGHT").ToString,
                                   rdr("BUDGET_SLTOUTPUT_WEIGHT").ToString, rdr("RESULT_SLTOUTPUT_WEIGHT").ToString,
                                   rdr("RESULT_MRCMSM_WEIGHT").ToString, rdr("BUDGET_PASS_WEIGHT_WITHJDG").ToString, rdr("RESULT_PASS_WEIGHT_WITHJDG").ToString, rdr("RESULT_INSPCMCM_WEIGHT_WITHJDG").ToString, rdr("RESULT_ALLEXTRD_WEIGHT").ToString)

                PSP_PRODUCTION_B_MIX_MAINT_INT(rdr("SDATE").ToString, rdr("FILMMAKINGMACHINECODE").ToString, rdr("BUDGET_MAINBRM_WEIGHT").ToString, rdr("BUDGET_EXTRD_WEIGHT").ToString, rdr("RESULT_MAINBRM_WEIGHT").ToString, rdr("RESULT_ALLEXTRD_WEIGHT").ToString)
            End While
            rdr.Close()
        End Using
        conn.Close()
        cmd.Dispose()
    End Sub

    Public Sub SP_MIB_PRODUCTION_INTEGRATION2()
        Dim conn = New OracleConnection(connectionStringPFRACT)
        Dim cmd As OracleCommand = New OracleCommand()
        Dim rdr As OracleDataReader
        Using conn
            conn.Open()
            cmd.Connection = conn
            cmd.CommandText = "SP_MIB_PRODUCTION_INTEGRATION2"
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandTimeout = 0
            cmd.Parameters.Clear()
            cmd.Parameters.Add(New OracleParameter("SREFDATA", OracleDbType.RefCursor)).Direction = Data.ParameterDirection.Output
            rdr = cmd.ExecuteReader()
            While rdr.Read
                PSP_PRODUCTIVITY_MAINT_INT(rdr("SDATE").ToString, rdr("FILMMAKINGMACHINECODE").ToString, rdr("PASS_QTY").ToString)

                PSP_PRODUCTION_CR_YIELD_MAINT_INT(rdr("SDATE").ToString, rdr("FILMMAKINGMACHINECODE").ToString, rdr("BUDGET_MILLROLL_WEIGHT").ToString, rdr("BUDGET_EXTRD_WEIGHT").ToString,
                                      rdr("RESULT_MILLROLL_WEIGHT").ToString, rdr("RESULT_EXTRD_WEIGHT").ToString,
                                      rdr("BUDGET_SLTOUTPUT_WEIGHT").ToString, rdr("RESULT_SLTOUTPUT_WEIGHT").ToString,
                                      rdr("RESULT_MRCMSM_WEIGHT").ToString, rdr("BUDGET_PASS_WEIGHT_WITHJDG").ToString,
                                      rdr("RESULT_PASS_WEIGHT_WITHJDG").ToString, rdr("RESULT_INSPCMCM_WEIGHT_WITHJDG").ToString, rdr("RESULT_ALLEXTRD_WEIGHT").ToString)

                PSP_PRODUCTION_B_MIX_MAINT_INT(rdr("SDATE").ToString, rdr("FILMMAKINGMACHINECODE").ToString, rdr("BUDGET_MAINBRM_WEIGHT").ToString, rdr("BUDGET_EXTRD_WEIGHT").ToString, rdr("RESULT_MAINBRM_WEIGHT").ToString, rdr("RESULT_ALLEXTRD_WEIGHT").ToString)
            End While
            rdr.Close()
            cmd.Dispose()
        End Using
        conn.Close()
    End Sub

    Public Sub PSP_PRODUCTION_CR_YIELD_MAINT_INT(ByVal Year_Month As String, ByVal FMMCCODE As String, ByVal P_BUDGET_MILLROLL_WEIGHT As Double, ByVal P_BUDGET_EXTRD_WEIGHT As Double,
                                     ByVal P_RESULT_MILLROLL_WEIGHT As Double, ByVal P_RESULT_EXTRD_WEIGHT As Double, ByVal P_BUDGET_SLTOUTPUT_WEIGHT As Double,
                                     ByVal P_RESULT_SLTOUTPUT_WEIGHT As Double, ByVal P_RESULT_MRCMSM_WEIGHT As Double, ByVal P_BUDGET_PASS_WEIGHT_WITHJDG As Double,
                                     ByVal P_RESULT_PASS_WEIGHT_WITHJDG As Double, ByVal P_RESULT_INS_WEIGHT_WITHJDG As Double, ByVal P_RESULT_ALLEXTRD_WEIGHT As Double)
        Dim conn As SqlConnection = New SqlConnection(connectionStringMIB)
        Dim cmd As SqlCommand = New SqlCommand()
        Using conn
            conn.Open()
            cmd.Connection = conn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "PSP_PRODUCTION_CR_YIELD_MAINT_INT"
            cmd.CommandTimeout = 0
            cmd.Parameters.Clear()
            cmd.Parameters.Add("P_YEAR_MONTH", SqlDbType.VarChar, 10, Data.ParameterDirection.Input).Value = Year_Month
            cmd.Parameters.Add("P_FMMCCODE", SqlDbType.VarChar, 3, Data.ParameterDirection.Input).Value = FMMCCODE
            cmd.Parameters.Add("P_BUDGET_MILLROLL_WEIGHT", SqlDbType.Decimal, Data.ParameterDirection.Input).Value = P_BUDGET_MILLROLL_WEIGHT
            cmd.Parameters.Add("P_BUDGET_EXTRD_WEIGHT", SqlDbType.Decimal, Data.ParameterDirection.Input).Value = P_BUDGET_EXTRD_WEIGHT
            cmd.Parameters.Add("P_RESULT_MILLROLL_WEIGHT", SqlDbType.Decimal, Data.ParameterDirection.Input).Value = P_RESULT_MILLROLL_WEIGHT
            cmd.Parameters.Add("P_RESULT_EXTRD_WEIGHT", SqlDbType.Decimal, Data.ParameterDirection.Input).Value = P_RESULT_EXTRD_WEIGHT
            cmd.Parameters.Add("P_BUDGET_SLTOUTPUT_WEIGHT", SqlDbType.Decimal, Data.ParameterDirection.Input).Value = P_BUDGET_SLTOUTPUT_WEIGHT
            cmd.Parameters.Add("P_RESULT_SLTOUTPUT_WEIGHT", SqlDbType.Decimal, Data.ParameterDirection.Input).Value = P_RESULT_SLTOUTPUT_WEIGHT
            cmd.Parameters.Add("P_RESULT_MRCMSM_WEIGHT", SqlDbType.Decimal, Data.ParameterDirection.Input).Value = P_RESULT_MRCMSM_WEIGHT
            cmd.Parameters.Add("P_BUDGET_PASS_WEIGHT_WITHJDG", SqlDbType.Decimal, Data.ParameterDirection.Input).Value = P_BUDGET_PASS_WEIGHT_WITHJDG
            cmd.Parameters.Add("P_RESULT_PASS_WEIGHT_WITHJDG", SqlDbType.Decimal, Data.ParameterDirection.Input).Value = P_RESULT_PASS_WEIGHT_WITHJDG
            cmd.Parameters.Add("P_RESULT_INS_WEIGHT_WITHJDG", SqlDbType.Decimal, Data.ParameterDirection.Input).Value = P_RESULT_INS_WEIGHT_WITHJDG
            cmd.Parameters.Add("P_RESULT_ALLEXTRD_WEIGHT", SqlDbType.Decimal, Data.ParameterDirection.Input).Value = P_RESULT_ALLEXTRD_WEIGHT
            cmd.ExecuteNonQuery()
            cmd.Dispose()
        End Using
        conn.Close()
    End Sub

    Public Sub PSP_PRODUCTION_B_MIX_MAINT_INT(ByVal Year_Month As String, ByVal FMMCCODE As String, ByVal P_BUDGET_MAINBRM_WEIGHT As Double, ByVal P_BUDGET_EXTRD_WEIGHT As Double,
                                  ByVal P_RESULT_MAINBRM_WEIGHT As Double, ByVal P_RESULT_ALLEXTRD_WEIGHT As Double)
        Dim conn As SqlConnection = New SqlConnection(connectionStringMIB)
        Dim cmd As SqlCommand = New SqlCommand()
        Using conn
            conn.Open()
            cmd.Connection = conn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "PSP_PRODUCTION_B_MIX_MAINT_INT"
            cmd.CommandTimeout = 0
            cmd.Parameters.Clear()
            cmd.Parameters.Add("P_YEAR_MONTH", SqlDbType.VarChar, 10, Data.ParameterDirection.Input).Value = Year_Month
            cmd.Parameters.Add("P_FMMCCODE", SqlDbType.VarChar, 3, Data.ParameterDirection.Input).Value = FMMCCODE
            cmd.Parameters.Add("P_BUDGET_MAINBRM_WEIGHT", SqlDbType.Decimal, Data.ParameterDirection.Input).Value = P_BUDGET_MAINBRM_WEIGHT
            cmd.Parameters.Add("P_BUDGET_EXTRD_WEIGHT", SqlDbType.Decimal, Data.ParameterDirection.Input).Value = P_BUDGET_EXTRD_WEIGHT
            cmd.Parameters.Add("P_RESULT_MAINBRM_WEIGHT", SqlDbType.Decimal, Data.ParameterDirection.Input).Value = P_RESULT_MAINBRM_WEIGHT
            cmd.Parameters.Add("P_RESULT_ALLEXTRD_WEIGHT", SqlDbType.Decimal, Data.ParameterDirection.Input).Value = P_RESULT_ALLEXTRD_WEIGHT
            cmd.ExecuteNonQuery()
            cmd.Dispose()
        End Using
        conn.Close()
        conn.Dispose()
    End Sub

    Public Sub PSP_PRODUCTIVITY_MAINT_INT(ByVal Year_Month As String, ByVal FMMCCODE As String, ByVal Pass_Qty As Double)
        Try
            Dim conn As SqlConnection = New SqlConnection(connectionStringMIB)
            Dim cmd As SqlCommand = New SqlCommand()
            Using conn
                conn.Open()
                cmd.Connection = conn
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = "PSP_PRODUCTIVITY_MAINT_INT"
                cmd.CommandTimeout = 0
                cmd.Parameters.Clear()
                cmd.Parameters.Add("P_YEAR_MONTH", SqlDbType.VarChar, 10, Data.ParameterDirection.Input).Value = Year_Month
                cmd.Parameters.Add("P_FMMCCODE", SqlDbType.VarChar, 3, Data.ParameterDirection.Input).Value = FMMCCODE
                cmd.Parameters.Add("P_PASS_QTY", SqlDbType.Decimal, Data.ParameterDirection.Input).Value = Pass_Qty
                cmd.ExecuteNonQuery()
                cmd.Dispose()
            End Using
            conn.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
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