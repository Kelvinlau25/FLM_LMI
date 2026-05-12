using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using HomeModel;
using PAB.Models;
namespace PAB.Repository
{
    public class ACLRepo
    {
        DatabaseModel.ACLDatabase db = new DatabaseModel.ACLDatabase();
       
        public AuthenticatorModel ValidateUserInfo(string userAD, string systemName)
        {
            db.OpenConnection();
            db.command.CommandText = "PSP_ACL_USER_SEL_KENGE";
            db.command.CommandType = CommandType.StoredProcedure;
            db.command.CommandTimeout = 0;
            db.command.Parameters.Clear();
            db.command.Parameters.Add(new SqlParameter("@pUserID", userAD)).Direction = System.Data.ParameterDirection.Input;
            db.command.Parameters.Add(new SqlParameter("@pSystemName", systemName)).Direction = System.Data.ParameterDirection.Input;
            db.reader = db.command.ExecuteReader();

            AuthenticatorModel AuthenticatorModel = null;
            AuthenticatorModel = new AuthenticatorModel();
            AuthenticatorModel.VALID_USER = false;
            while (db.reader.Read())
            {
                if (db.reader.HasRows)
                {

                    AuthenticatorModel.ID_ACL_USER = Convert.ToInt16(db.reader["ID_ACL_USER"]);
                    AuthenticatorModel.ID_ACL_ROLE = Convert.ToInt16(db.reader["ID_ACL_ROLE"]);
                    AuthenticatorModel.ID_ACL_RESOURCE = Convert.ToInt16(db.reader["ID_ACL_RESOURCE"]);
                    AuthenticatorModel.USER_ID = db.reader["USER_ID"].ToString();
                    AuthenticatorModel.USR_EMAIL = db.reader["USR_EMAIL"].ToString();
                    AuthenticatorModel.COMPANY = db.reader["COMPANY"].ToString();
                    AuthenticatorModel.EMP_NO = db.reader["EMP_NO"].ToString();
                    AuthenticatorModel.EMP_NAME = db.reader["EMP_NAME"].ToString();
                    AuthenticatorModel.ROLE_NAME = db.reader["ROLE_NAME"].ToString();
                    AuthenticatorModel.ROLE_DESC = db.reader["ROLE_DESC"].ToString();
                    AuthenticatorModel.RESOURCE_NAME = db.reader["RESOURCE_NAME"].ToString();
                    AuthenticatorModel.RESOURCE_DESC = db.reader["RESOURCE_DESC"].ToString();
                    AuthenticatorModel.PASSWORD = db.reader["USR_PASSWORD"].ToString();
                    AuthenticatorModel.VALID_USER = true;
                }
                else
                {
                    AuthenticatorModel.VALID_USER = false;
                }
            }


            db.CloseReader();
            db.CloseConnection();


            return AuthenticatorModel;

        }

        #region menu
        public DataTable sideBarDB(Int64 roleID, string SystemName)
        {
            db.OpenConnection();
            db.command.CommandText = "PSP_ACL_SIDEBAR";
            db.command.CommandType = CommandType.StoredProcedure;
            db.command.CommandTimeout = 0;
            db.command.Parameters.Clear();
            db.command.Parameters.Add(new SqlParameter("@ID_ACL_ROLE", roleID)).Direction = System.Data.ParameterDirection.Input;
            db.command.Parameters.Add(new SqlParameter("@pSystemName", SystemName)).Direction = System.Data.ParameterDirection.Input;
            db.reader = db.command.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(db.reader);

            db.CloseReader();
            db.CloseConnection();

            return dt;
        }

        public DataTable oldPassword(int userID)
        {
            db.OpenConnection();
            db.command.CommandText = "PSP_ACL_CHANGE_PASSWORD";
            db.command.CommandType = CommandType.StoredProcedure;
            db.command.CommandTimeout = 0;
            db.command.Parameters.Clear();
            db.command.Parameters.Add(new SqlParameter("@userID", userID)).Direction = System.Data.ParameterDirection.Input;
            db.reader = db.command.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(db.reader);

            db.CloseReader();
            db.CloseConnection();

            return dt;
        }

        public string NewPassWord(int userID, string newPassword)
        {
            db.OpenConnection();
            db.command.CommandText = "PSP_ACL_CHANGE_PASSWORD_MAINT";
            db.command.CommandType = CommandType.StoredProcedure;
            db.command.CommandTimeout = 0;
            db.command.Parameters.Clear();
            db.command.Parameters.Add(new SqlParameter("@userID", userID)).Direction = System.Data.ParameterDirection.Input;
            db.command.Parameters.Add(new SqlParameter("@newPassword", newPassword)).Direction = System.Data.ParameterDirection.Input;
            db.command.Parameters.Add(new SqlParameter("@returnID", SqlDbType.VarChar, 1)).Direction = System.Data.ParameterDirection.Output;
            db.reader = db.command.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(db.reader);

            db.CloseReader();
            db.CloseConnection();

            return db.command.Parameters["@returnID"].Value.ToString();


        }
    }
    #endregion
}