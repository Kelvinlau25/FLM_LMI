using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using DBModel;
using PAB.Models;

namespace PAB.Repository
{
    public class CommonRepo
    {
        DatabaseModel.Database db = new DatabaseModel.Database();
        public List<T> ConvertToList<T>(DataTable dt)
        {
            var columnNames = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName.ToLower()).ToList();
            var properties = typeof(T).GetProperties();
            return dt.AsEnumerable().Select(row => {
                var objT = Activator.CreateInstance<T>();
                foreach (var pro in properties)
                {
                    if (columnNames.Contains(pro.Name.ToLower()))
                    {
                        try
                        {
                            if (pro.PropertyType.Name.Equals("Boolean"))
                            {
                                if (row[pro.Name].ToString().ToUpper().Equals("TRUE")) { pro.SetValue(objT, true); }
                                else { pro.SetValue(objT, false); }
                            }
                            else if (pro.PropertyType.Name.Equals("Integer"))
                            {
                                pro.SetValue(objT, Convert.ToInt32(row[pro.Name]));
                            }
                            else if (pro.PropertyType.Name.Equals("DateTime"))
                            {
                                pro.SetValue(objT, Convert.ToDateTime(row[pro.Name]));
                            }
                            else if (pro.PropertyType.Name.Equals("Double"))
                            {
                                pro.SetValue(objT, Convert.ToDouble(row[pro.Name]));
                            }
                            else if (pro.PropertyType.Name.Equals("Decimal"))
                            {
                                pro.SetValue(objT, Convert.ToDecimal(row[pro.Name]));
                            }
                            else { pro.SetValue(objT, row[pro.Name]); }
                        }
                        catch (Exception ex) { }
                    }
                }
                return objT;
            }).ToList();
        }

        public string ConvertSearchValue(string[] Scol, string str)
        {
            var val = "'%" + str + "%'";
            str = "";
            var additonal = "";
            foreach (var col in Scol)
            {
                string[] c = col.Split(new Char[] { '/' });
                str += (additonal + " UPPER(" + c[1] + ") " + "LIKE" + " UPPER(" + val + ") ");
                additonal = " OR";
            }
            return str;
        }

      public DataTable List(string Table, string TableID, string Search,
      string Value, string SortField, string Direction,
      string FrmRowno, string ToRowno, string Deleted, string Conn = "PAB_BB")
        {
            db.OpenConnection(Conn);
            db.command.CommandText = "PSP_PAB_LIST";
            db.command.CommandType = CommandType.StoredProcedure;
            db.command.CommandTimeout = 0;
            db.command.Parameters.Clear();
            db.command.Parameters.Add(new SqlParameter("@Table", Table)).Direction = System.Data.ParameterDirection.Input;
            db.command.Parameters.Add(new SqlParameter("@TableID", TableID)).Direction = System.Data.ParameterDirection.Input;
            db.command.Parameters.Add(new SqlParameter("@Search", Search)).Direction = System.Data.ParameterDirection.Input;
            db.command.Parameters.Add(new SqlParameter("@Value", Value)).Direction = System.Data.ParameterDirection.Input;
            db.command.Parameters.Add(new SqlParameter("@SortField", SortField)).Direction = System.Data.ParameterDirection.Input;
            db.command.Parameters.Add(new SqlParameter("@Direction", Direction)).Direction = System.Data.ParameterDirection.Input;
            db.command.Parameters.Add(new SqlParameter("@FrmRowno", FrmRowno)).Direction = System.Data.ParameterDirection.Input;
            db.command.Parameters.Add(new SqlParameter("@ToRowno", ToRowno)).Direction = System.Data.ParameterDirection.Input;
            db.command.Parameters.Add(new SqlParameter("@Deleted", Deleted)).Direction = System.Data.ParameterDirection.Input;
            db.reader = db.command.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(db.reader);

            db.CloseReader();
            db.CloseConnection();

            return dt;
        }
        
    }
}