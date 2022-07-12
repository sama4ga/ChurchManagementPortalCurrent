using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data;
using System.IO;
using System.Diagnostics;
using System.Windows.Controls;

namespace ChurchManagementPortal
{
    class SQL:IDisposable
    {
        private MySqlConnection con;
        private MySqlCommand cmd;
        private MySqlDataAdapter da;
        public DataSet ds;
        private DataTable dt;
        private string uid = Properties.Settings.Default.user;
        private string password = Properties.Settings.Default.password;
        private string server = Properties.Settings.Default.server;
        private uint port = Properties.Settings.Default.port;
        private string database = Properties.Settings.Default.database;
        public Error error;

        /// <summary>
        /// Opens a connection to the mysql database using default parameters
        /// </summary>
        /// <returns></returns>
        private bool OpenCon()
        {
            MySqlConnectionStringBuilder connectionstring = new()
            {
                Port = port,
                Database = database,
                Server = server,
                Password = password,
                UserID = uid
            };
            con = new MySqlConnection(connectionstring.ToString());
            error = null;
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                return true;
            }
            catch (MySqlException e)
            {
                error = new Error(e.Message, e.Number, "MYSQL");
                return false;
            }

        }


        /// <summary>
        /// Opens a connection using the parameters defined
        /// </summary>
        /// <param name="server">Server to connect to</param>
        /// <param name="username">UserId for the connection</param>
        /// <param name="password">Password associated with this UserId</param>
        /// <param name="port">Port to use for connection</param>
        /// <param name="database">Database to connect to </param>
        /// <returns>true or false</returns>
        public bool OpenCon(string server = "localhost", string username = "root", string password = "", uint port = 3308, string database = "cmp")
        {
            this.server = server;
            this.uid = username;
            this.password = password;
            this.port = port;
            this.database = database;
            return OpenCon();

        }

        /// <summary>
        /// Used to close any available connection to a database
        /// </summary>
        /// <returns></returns>
        private bool CloseCon()
        {
            try
            {
                con.Close();
                return true;
            }
            catch (MySqlException e)
            {
                error = new Error(e.Message, e.Number, "MYSQL");
                return false;
            }
        }

        /// <summary>
        /// Returns connection for external use
        /// </summary>
        /// <returns></returns>
        public MySqlConnection GetCon()
        {
            OpenCon();
            return con;
        }

        /// <summary>
        /// Used to insert data without returning id
        /// </summary>
        /// <param name="query">Query string</param>
        /// <param name="ParamValues">A list containing values for parameters used in the query</param>
        /// <returns>true if query is successful and false if otherwise</returns>
        public bool InsertQuery(string query, List<object> ParamValues)
        {
            try
            {
                if (OpenCon())
                {
                    cmd = new MySqlCommand(query, con);
                    string paramName = "";

                    //if (cmd.IsPrepared) {
                    cmd.Parameters.Clear();
                    int count = 1;
                    foreach (var item in ParamValues)
                    {
                        paramName = "@" + count;
                        cmd.Parameters.AddWithValue(paramName, item);
                        count++;
                    }

                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (MySqlException e)
            {
                error = new Error(e.Message, e.Number, "MYSQL");
                return false;
            }
            finally
            {
                cmd.Dispose();
                CloseCon();
            }

        }

        /// <summary>
        /// Used to insert query and at the same time return the id of the insertion
        /// </summary>
        /// <param name="query">string containing the query</param>
        /// <param name="ParamValues">values for parameters used in the query</param>
        /// <param name="insertId">Output which signifies the LastInsertId </param>
        /// <returns>true if query is successful and false otherwise</returns>
        public bool InsertQuery(string query, List<object> ParamValues, out long insertId)
        {
            try
            {
                if (OpenCon())
                {
                    cmd = new MySqlCommand(query, con);
                    cmd.Prepare();
                    string paramName = "";

                    //if (cmd.IsPrepared) {
                    cmd.Parameters.Clear();
                    int count = 1;
                    foreach (var item in ParamValues)
                    {
                        paramName = "@" + count;
                        cmd.Parameters.AddWithValue(paramName, item);
                        count++;
                    }

                    cmd.ExecuteNonQuery();
                    insertId = cmd.LastInsertedId;
                    return true;
                }
                else
                {
                    insertId = 0;
                    return false;
                }

            }
            catch (MySqlException e)
            {
                insertId = 0;
                error = new Error(e.Message, e.Number, "MYSQL");
                return false;
            }
            finally
            {
                cmd.Dispose();
                CloseCon();
            }

        }


        /// <summary>
        /// Used to update record
        /// </summary>
        /// <param name="query">Query string</param>
        /// <param name="ParamValues">A list containing values for parameters used in the query</param>
        /// <returns>true if query is successful and false otherwise</returns>
        public bool UpdateQuery(string query, List<object> ParamValues)
        {
            try
            {
                if (OpenCon())
                {
                    cmd = new MySqlCommand(query, con);
                    cmd.Prepare();

                    //if (cmd.IsPrepared) {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddRange(ParamValues.ToArray());

                    cmd.ExecuteNonQuery();
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (MySqlException e)
            {
                error = new Error(e.Message, e.Number, "MYSQL");
                return false;
            }
            finally
            {
                cmd.Dispose();
                CloseCon();
            }

        }

        /// <summary>
        /// Executes query against a database
        /// </summary>
        /// <param name="query">query string</param>
        /// <returns>true if query is successful and false othewise</returns>
        public bool ExecuteQuery(string query)
        {
            try
            {
                if (OpenCon())
                {
                    cmd = new MySqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (MySqlException e)
            {
                error = new Error(e.Message, e.Number, "MYSQL");
                return false;
            }
            finally
            {
                cmd.Dispose();
                CloseCon();
            }
        }

        /// <summary>
        /// Reads data
        /// </summary>
        /// <param name="query">string holding the query</param>
        /// <param name="datasetName">string to assign name to the dataset that holds this record</param>
        /// <returns></returns>
        public bool ReadData(string query, string datasetName = "")
        {
            try
            {
                if (OpenCon())
                {
                    cmd = new MySqlCommand(query, con);
                    //dr = cmd.ExecuteReader();
                    da = new MySqlDataAdapter(cmd);
                    dt = new DataTable(datasetName);
                    ds = new DataSet();

                    da.Fill(dt);
                    ds.Tables.Add(dt);
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (MySqlException e)
            {
                error = new Error(e.Message, e.Number, "MYSQL");
                return false;
            }
            finally
            {
                da.Dispose();
                cmd.Dispose();
                CloseCon();
            }
        }

        public void fillComboBox(string query, ComboBox comboBox, string datasetName="")
        {
            try
            {
                if (OpenCon())
                {
                    cmd = new MySqlCommand(query, con);
                    //dr = cmd.ExecuteReader();
                    da = new MySqlDataAdapter(cmd);
                    dt = new DataTable(datasetName);
                    ds = new DataSet();

                    da.Fill(dt);
                    ds.Tables.Add(dt);

                    comboBox.DisplayMemberPath = dt.Columns[1].ColumnName;
                    comboBox.SelectedValuePath = dt.Columns[0].ColumnName;
                    comboBox.ItemsSource = dt.DefaultView;
                }

            }
            catch (MySqlException e)
            {
                error = new Error(e.Message, e.Number, "MYSQL");
            }
            finally
            {
                da.Dispose();
                cmd.Dispose();
                CloseCon();
            }
        }

        public void fillListBox(string query, ListBox listBox, string datasetName = "")
        {
            try
            {
                if (OpenCon())
                {
                    cmd = new MySqlCommand(query, con);
                    //dr = cmd.ExecuteReader();
                    da = new MySqlDataAdapter(cmd);
                    dt = new DataTable(datasetName);
                    ds = new DataSet();

                    da.Fill(dt);
                    ds.Tables.Add(dt);

                    listBox.DisplayMemberPath = dt.Columns[1].ColumnName;
                    listBox.SelectedValuePath = dt.Columns[0].ColumnName;
                    listBox.ItemsSource = dt.DefaultView;
                }

            }
            catch (MySqlException e)
            {
                error = new Error(e.Message, e.Number, "MYSQL");
            }
            finally
            {
                da.Dispose();
                cmd.Dispose();
                CloseCon();
            }
        }

        /// <summary>
        /// Backup a database to 'Backup' folder in the current working directory
        /// </summary>
        public bool Backup()
        {
            try
            {
                //DateTime Time = DateTime.Now;
                //int year = Time.Year;
                //int month = Time.Month;
                //int day = Time.Day;
                //int hour = Time.Hour;
                //int minute = Time.Minute;
                //int second = Time.Second;
                //int millisecond = Time.Millisecond;

                //Save file to C:\ with the current date as a filename
                string path;
                if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\Backup"))
                {
                    _ = Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Backup");
                }
                path = Directory.GetCurrentDirectory() + "\\Backup\\MySqlBackup.sql"; // + year + month + day + hour + minute + second + millisecond + ".sql";
                StreamWriter file = new(path);

                ProcessStartInfo psi = new()
                {
                    FileName = "mysqldump",
                    RedirectStandardInput = false,
                    RedirectStandardOutput = true,
                    Arguments = string.Format("-u{0} -p{1} -h{2} {3}", uid, password, server, database),
                    UseShellExecute = false
                };

                Process process = Process.Start(psi);

                string output;
                output = process.StandardOutput.ReadToEnd();
                file.WriteLine(output);
                process.WaitForExit();
                file.Close();
                process.Close();
                return true;
            }
            catch (IOException e)
            {
                error = new Error(e.Message, e.HResult, "IO");
                return false;
            }
        }

        /// <summary>
        /// Restore database from 'Backup' folder in the current working directory
        /// </summary>
        public bool Restore()
        {
            try
            {
                string path;
                if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\Backup"))
                {
                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Backup");
                }
                path = Directory.GetCurrentDirectory() + "\\Backup\\MySqlBackup.sql";
                StreamReader file = new(path);
                string input = file.ReadToEnd();
                file.Close();

                ProcessStartInfo psi = new();
                psi.FileName = "mysql";
                psi.RedirectStandardInput = true;
                psi.RedirectStandardOutput = false;
                psi.Arguments = string.Format(@"-u{0} -p{1} -h{2} {3}", uid, password, server, database);
                psi.UseShellExecute = false;


                Process process = Process.Start(psi);
                process.StandardInput.WriteLine(input);
                process.StandardInput.Close();
                process.WaitForExit();
                process.Close();
                return true;
            }
            catch (IOException e)
            {
                error = new Error(e.Message, e.HResult, "IO");
                return false;
            }
        }

        public void Dispose()
        {
            da.Dispose();
            dt.Dispose();
            ds.Dispose();
            cmd.Dispose();
            con.Dispose();
        }

    }
}
