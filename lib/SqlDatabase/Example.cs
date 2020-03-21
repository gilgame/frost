/* -----------------------------------------------------------------------------
 * Example.cs
 * 
 * Copyright (c) 2020 TD Fellows, TD Frost. All rights reserved.
 * 
 * Permission is hereby granted, free of charge, to any person obtaining
 * a copy of this software and associated documentation files (the "Software"),
 * to deal in the Software without restriction, including without limitation
 * the rights to use, copy, modify, merge, publish, distribute, sublicense,
 * and/or sell copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subjectto the following conditions:
 * 
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
 * OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
 * IN THE SOFTWARE.
 * -----------------------------------------------------------------------------
 */

using System;
using System.Data;

namespace Frost.Lib.SqlDatabase
{
    public class Example
    {
        public DataTable Companies()
        {
            DataTable companies = new DataTable("Companies");
            Post(c =>
            {
                companies = c.ExecuteQuery("sp_Companies_get", CommandType.StoredProcedure);
            });
            return companies;
        }

        public bool ModifyCompany(int companyid, string name, int userid, bool disabled = false)
        {
            var success = false;
            Post(c =>
            {
                var p = new ParameterCollection
                {
                    {"@CompanyID", SqlDbType.Int, userid },
                    {"@Name", SqlDbType.NVarChar, 60, userid },
                    {"@Disabled", SqlDbType.Bit, userid },
                    {"@UserID", SqlDbType.Int, userid },
                }
                .Set(c);
                c.ExecuteNonQuery("sp_Company_upd", CommandType.StoredProcedure);

                success = true;
            });
            return success;
        }

        public int CreateSubscription(int userid, int settingid)
        {
            int result = -1;
            Post(c =>
            {
                var p = new ParameterCollection
                {
                    {"@UserID", SqlDbType.Int, userid },
                    {"@SettingID", SqlDbType.Int, settingid },
                }
                .Set(c);

                object o = c.ExecuteScalar("sp_Subscription_ins", CommandType.StoredProcedure);
                int.TryParse(o.ToString(), out result);
            });
            return result;
        }

        public void UpdateCompanies(DataTable companies, int userid)
        {
            Post(c =>
            {
                var p = new ParameterCollection
                {
                    {"@Companies", SqlDbType.Structured, "sp_Companies_upd", companies },
                    {"@UserID", SqlDbType.Int, userid },
                }
                .Set(c);
                c.ExecuteNonQuery("sp_Companies_upd", CommandType.StoredProcedure);
            });
        }

        private void Post(Action<Client> action)
        {
            try
            {
                using (var c = new Client(Connections.SecureIQ))
                {
                    action(c);
                }
            }
            catch (Exception ex)
            {
                if (ex is SqlException)
                {
                    var sqlex = ex as SqlException;

                    //do error handling
                }
            }
        }

        public static class Connections
        {
            public static string Default { get; } = "-c=Server=FME-DEV-SQL-01.FMYN.COM\\SQL2014; Initial Catalog=StrattonWarren; Integrated Security=true";

            public static string Badging => GetConnectionString("SecureIQ");

            public static string Cage => GetConnectionString("Cage");

            public static string CgAdmin => GetConnectionString("CgAdmin");

            public static string Humaxx => GetConnectionString("Humaxx");

            public static string SecureIQ => GetConnectionString("SecureIQ");

            private static string GetConnectionString(string name)
            {
                //get connection string by name logic

                return String.Empty;
            }
        }
    }
}
