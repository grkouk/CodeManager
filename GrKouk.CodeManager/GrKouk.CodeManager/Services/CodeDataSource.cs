using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using GrKouk.CodeManager.Models;

namespace GrKouk.CodeManager.Services
{
    class CodeDataSource : ICodeDataSource
    {
        public async Task<IEnumerable<CodeDto>> GetCodesAsync(string codeBase)
        { 
            var list = new List<CodeDto>();
            try
            {
               
                var cnn = new SqlConnection 
                {
                    ConnectionString = "Data Source=winzone51.grserver.gr,1555;Initial Catalog = angelikasdb2;User ID=grkoukdbUser ;Password=Villa8561#!"
                };
                cnn.Open();
                string cmdText;
                cmdText = string.IsNullOrEmpty(codeBase)
                    ? "select name,sku from product order by sku desc"
                    : "select name,sku from product where sku like '%" + codeBase + "%' order by sku desc";
                var com= new SqlCommand
                {
                    Connection = cnn,
                    CommandText = cmdText
                };

                var reader = await com.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    int i = 0;
                    while (reader.Read())
                    {
                       
                        //var a = reader.GetString(reader.GetOrdinal("Name"));
                        try
                        {
                            string b = "";
                            try
                            {
                                b = reader.GetString(reader.GetOrdinal("Sku"));
                            }
                            catch (Exception e)
                            {
                                b = "";
                            }
                            var cd = new CodeDto
                            {
                                Code = b
                            };
                            list.Add(cd);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            throw;
                        }
                       
                      
                    }
                }
               
                reader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return list;

        }
    }
}