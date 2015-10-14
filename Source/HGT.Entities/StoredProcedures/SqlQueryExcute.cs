using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Pattern.Ef6;
using HGT.Entities.Models;
using System.Data;

namespace HGT.Entities.StoredProcedures
{
    /// <summary>
    /// SqlQueryExcute
    /// 
    /// Revision History
    /// Date			Author		                    Reason for Change
    /// -----------------------------------------------------------
    /// 08/07/2014		thuyhk@cateno.no                created.        
    /// </summary>
    /// 

    public partial class SqlQueryExcute : DataContext
    {
        static SqlQueryExcute()
        {
            Database.SetInitializer<SqlQueryExcute>(null);
        }

        public SqlQueryExcute()
            : base("Name=MichiMartContext")
        { }

        //public int CreateDBByTenant(string dbName, string dbUser, string dbPass)
        //{
        //    try
        //    {
        //        var _dbName = dbName != null ? new SqlParameter("@DBName", dbName) : new SqlParameter("@DBName", typeof(string));
        //        var _dbUser = dbUser != null ? new SqlParameter("@DBUsername", dbUser) : new SqlParameter("@DBUsername", typeof(string));
        //        var _dbPass = dbPass != null ? new SqlParameter("@DBPassword", dbPass) : new SqlParameter("@DBPassword", typeof(string));

        //        var result = this.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, "CreateNewDB @DBName, @DBUsername, @DBPassword", _dbName, _dbUser, _dbPass);
        //        return result;
        //    }
        //    catch
        //    {
        //        return 1;
        //    }
        //}
       
    }
}
