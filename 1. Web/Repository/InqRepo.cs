using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;

namespace PAB.Repository
{
    public class InqRepo
    {
        DatabaseModel.Database db = new DatabaseModel.Database();
        string SqlCon = "PAB_BB";

        public object NulltoEmpty(object param)
        {
            if (param == null)
            {
                param = "";
            }
            return param;
        }
    }   
}