

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Helpers;


namespace RecordsTransferMgtSystem.Models
{
    public class DBConnect
    {
        
        private string connection = ConfigurationManager.ConnectionStrings["recordconn"].ConnectionString.ToString();
        public SqlCommand cmd;
        public SqlConnection con;
        public SqlDataReader dr;
        public DBConnect(){
            
            con = new SqlConnection(connection);
    }
        public string addUser(string firstname, string lastname, string emailaddress, string username, string password2, string jobtitle) {
            string add = "";
            try {
                cmd = new SqlCommand("Users_CRUD", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Firstname", firstname);
                cmd.Parameters.AddWithValue("@Lastname", lastname);
                cmd.Parameters.AddWithValue("@EmailAddress", emailaddress);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@password",password2);
                cmd.Parameters.AddWithValue("@role", jobtitle);
                cmd.Parameters.AddWithValue("@query", 1);
                con.Open();
                string k = cmd.ExecuteScalar().ToString();
                if (k.Equals("Y")) {
                    add = "Saved successfully";
                }
                else
                {
                  add = "Username Already taken";
                }
                           }
            catch(Exception e){
               add = e.ToString();
            }
            finally
            {
                con.Close();
                cmd.Dispose();
            }
            return add;
        }
        public string getLogin(string username,string Password){
         string add = "";
         Userlog user = new Userlog();
            try {
                cmd = new SqlCommand("Users_CRUD", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();               
                cmd.Parameters.AddWithValue("@Username",username);
                cmd.Parameters.AddWithValue("@password",Password);
                cmd.Parameters.AddWithValue("@query", 3);
                con.Open();
                
                dr = cmd.ExecuteReader();
                while(dr.Read()){ 
                    user.role = dr["role"].ToString(); 
                    user.Username = dr["Username"].ToString();
                        

                  
                } 
              
                 switch (user.role)
                    { 
                        case "StationManager":
                            add = "station";
                            break;
                        case "MainRegistry":
                            add = "registry";
                            break;
                        case "RecordsOfficer":
                            add = "records";
                            break; 
                       case "Admin":
                            add = "admin";
                            break;
                         default:
                           add="Log in failed";
                           break;
                         }                
            }
            catch(Exception e){
                add = e.ToString();
            }
            finally{
            con.Close();
            cmd.Dispose();
            }
            return add;
        }
        public List<Userlog> getUsers()
        {
            List<Userlog> userslist = new List<Userlog>();
            try
            {
                cmd = new SqlCommand("Users_CRUD", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@query", 2);
                con.Open();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Userlog user = new Userlog();
                    user.id = (int)dr["id"];
                    user.Firstname = dr["Firstname"].ToString();
                    user.Lastname = dr["Lastname"].ToString();
                    user.Username = dr["Username"].ToString();
                    user.EmailAddress = dr["EmailAddress"].ToString();             
                    user.role = dr["role"].ToString();
                    user.password = dr["password"].ToString();
                    userslist.Add(user);
                }

            }
            catch (Exception e)
            {
                e.ToString();
            }
            finally
            {
                con.Close();
                cmd.Dispose();
            }
            return userslist;
        }
       public int getRecords(FileTransfer files) { 
        int add = 0;    
            try {
                cmd = new SqlCommand("Files_CRUD", con);
                cmd.CommandType = CommandType.StoredProcedure;
            
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@projectName", files.projectName);
                cmd.Parameters.AddWithValue("@directorate", files.directorate);
                cmd.Parameters.AddWithValue("@department", files.department);
                cmd.Parameters.AddWithValue("@station", files.station);
                cmd.Parameters.AddWithValue("@dateOfTransfer", files.dateOfTransfer);
                cmd.Parameters.AddWithValue("@boxNumber", files.boxNumber);
               // cmd.Parameters.AddWithValue("", files.status);
                cmd.Parameters.AddWithValue("@query", 1);
                con.Open();
                int k = (int)cmd.ExecuteScalar();

                if (!k.Equals(""))
                {
                    add = k;
                   
                }
                else
                {
                    add = -1;
                }

                           }
            catch(Exception e){
                e.ToString();
               add =-1;
            }
            finally
            {
                con.Close();
                cmd.Dispose();
            }
            return add;
        
        }
       public FileTransfer listProjectDetail(int projectid) {
           FileTransfer user = new FileTransfer();
           try
           {
               cmd = new SqlCommand("Files_CRUD", con);
               cmd.CommandType = CommandType.StoredProcedure;
               cmd.Parameters.Clear();
               cmd.Parameters.AddWithValue("@projectID",projectid);
               cmd.Parameters.AddWithValue("@query", 4);
               con.Open();
               dr = cmd.ExecuteReader();
               while (dr.Read())
               {
                   
                   user.projectName = dr["projectName"].ToString();
                   user.directorate = dr["directorate"].ToString();
                   user.department = dr["department"].ToString();
                   user.station = dr["station"].ToString();
                   user.dateOfTransfer = (DateTime)dr["dateOfTransfer"]; 
                   user.boxNumber = dr["boxNumber"].ToString();
                   user.status = dr["status"].ToString();
                   user.projectID =(int)dr["projectID"];
               
               }

           }
           catch (Exception e)
           {
               e.ToString();
           }
           finally
           {
               con.Close();
               cmd.Dispose();
           }
           return user;
              
       }
       [WebMethod]
       public static string SaveData(string[][] array)
       {
           string result = string.Empty;
           try
           {
               DataTable dt = new DataTable();
               dt.Columns.Add("projectID");
               dt.Columns.Add("fileTitle");
               dt.Columns.Add("company");
               dt.Columns.Add("SrNo");
               dt.Columns.Add("ReferenceNo");
               dt.Columns.Add("numberOfCopies");
               foreach (var arr in array)
               {
                   DataRow dr = dt.NewRow();
                   dr["projectID"] = arr[0];
                   dr["fileTitle"] = arr[1];
                   dr["company"] = arr[2];
                   dr["SrNo"] = arr[3];
                   dr["ReferenceNo"] = arr[4];
                   dr["numberOfCopies"] = arr[5];
                 
                   dt.Rows.Add(dr);
               }

               SqlConnection cnn = new SqlConnection();
               cnn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings
               ["recordconn"].ToString();
               cnn.Open();
               SqlCommand cmd = new SqlCommand();
               cmd = new SqlCommand("SaveDetails", cnn);
               cmd.CommandType = CommandType.StoredProcedure;
              
               cmd.Parameters.Add("@TableType", SqlDbType.Structured).SqlValue = dt;

               result = cmd.ExecuteNonQuery().ToString();
           }
           catch (Exception ex)
           {
               result = ex.Message;
           }
           return result;

       }
       public projectFile addFiles(projectFile file)
       {
            
            try
            {
                cmd = new SqlCommand("Files_CRUD", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@projectID", file.projectID);
                cmd.Parameters.AddWithValue("@fileTitle", file.fileTitle);
                cmd.Parameters.AddWithValue("@company", file.company);
                cmd.Parameters.AddWithValue("@SrNo", file.SrNo);
                cmd.Parameters.AddWithValue("@ReferenceNo", file.ReferenceNo);
                cmd.Parameters.AddWithValue("@numberOfCopies",file.numberOfCopies);
                cmd.Parameters.AddWithValue("@query", 3);
                con.Open();
                cmd.ExecuteScalar().ToString();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            finally
            {
                con.Close();
                cmd.Dispose();
            }
            return file;
       }
    
        
       public int updateProject(FileTransfer transfer) {
           int add = 0;
           try {
               cmd = new SqlCommand("Files_CRUD", con);
               cmd.CommandType = CommandType.StoredProcedure;
               cmd.Parameters.Clear();
               cmd.Parameters.AddWithValue("@projectID", transfer.projectID);
               cmd.Parameters.AddWithValue("@projectName", transfer.projectName);
               cmd.Parameters.AddWithValue("@directorate", transfer.directorate);
               cmd.Parameters.AddWithValue("@department", transfer.department);
               cmd.Parameters.AddWithValue("@station", transfer.station);
               cmd.Parameters.AddWithValue("@dateOfTransfer", Convert.ToDateTime(transfer.dateOfTransfer).ToString("dd/MM/yyyy"));
               cmd.Parameters.AddWithValue("@boxNumber", transfer.boxNumber);
               cmd.Parameters.AddWithValue("@query", 2);
               con.Open();
                   int k = (int)cmd.ExecuteScalar();
               if (k.Equals(1))
                {
                    add = k;
                   
                }
                else
                {
                    add = -1;
                }
           }
           catch (Exception e)
           {
               e.ToString();
           }
           finally
           {
               con.Close();
               cmd.Dispose();
               listProjectDetail(add);               
           }
           return add;
       }
       public void DeleteProj(int g){
 
       try
       {
           cmd = new SqlCommand("Files_CRUD", con);
           cmd.CommandType = CommandType.StoredProcedure;
           cmd.Parameters.AddWithValue("@projectID", g);
           cmd.Parameters.AddWithValue("@query",5);
           con.Open();
           dr = cmd.ExecuteReader();
           while (dr.Read())
           {
           }            
       }
       catch (Exception e) {
           e.ToString();
       }
       finally
       {
           con.Close();
           cmd.Dispose();
       }}
        public List<FileTransfer> getProjects()
        {
            List<FileTransfer> projectlist = new List<FileTransfer>();
           
            try
            {
                cmd = new SqlCommand("Files_CRUD", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                
                cmd.Parameters.AddWithValue("@query", 7);
                con.Open();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    FileTransfer project = new FileTransfer();

                    project.projectName = dr["projectName"].ToString();
                    project.directorate = dr["directorate"].ToString();
                    project.department = dr["department"].ToString();
                    project.station = dr["station"].ToString();
                    project.dateOfTransfer = (DateTime)dr["dateOfTransfer"]; 
                    project.boxNumber = dr["boxNumber"].ToString();
                    project.status = dr["status"].ToString();
                    project.projectID = (int)dr["projectID"];
                    
                    projectlist.Add(project);
                }

            }
            catch (Exception e)
            {
                e.ToString();
            }
            finally
            {
                con.Close();
                cmd.Dispose();
            }
            return projectlist ;
        }
        public List<projectFile> getProjectfiles(int f)
        {
            List<projectFile> filelist = new List<projectFile>();
            try
            {
                cmd = new SqlCommand("Files_CRUD", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@projectID", f);
                cmd.Parameters.AddWithValue("@query", 6);               
                con.Open();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    projectFile projectfile = new projectFile();
                                 
                    projectfile.SrNo = dr["SrNo"].ToString();
                    projectfile.ReferenceNo = dr["ReferenceNo"].ToString();
                    projectfile.fileTitle = dr["fileTitle"].ToString();
                    projectfile.company = dr["company"].ToString();
                    projectfile.numberOfCopies = (int)dr["numberOfCopies"];
                    projectfile.projectID = (int)dr["projectID"]; 

                    filelist.Add(projectfile);
                }
                
            }
            catch (Exception e)
            {
                e.ToString();
            }
            finally
            {
                con.Close();
                cmd.Dispose();
            }
            return filelist;
        }
              
        public FileTransfer ApproveProjects(int K)
        {
            FileTransfer projectapproved = new FileTransfer();
            int add = 0;
            try
            {
                cmd = new SqlCommand("Files_CRUD", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@projectID", K);
                cmd.Parameters.AddWithValue("@query", 10);
                con.Open();             
                int l = (int)cmd.ExecuteScalar();
                if (l.Equals(1))
                {
                    add = l;

                }
                else
                {
                    add = -1;
                }
           

            }
            catch (Exception e)
            {
                e.ToString();
            }
            finally
            {
                con.Close();
                cmd.Dispose();
            }
            return projectapproved;
        }
        public List<FileTransfer> getApprovedProjects()
        {
            List<FileTransfer> projectlist = new List<FileTransfer>();

            try
            {
                cmd = new SqlCommand("Files_CRUD", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();

                cmd.Parameters.AddWithValue("@query", 8);
                con.Open();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    FileTransfer project = new FileTransfer();

                    project.projectName = dr["projectName"].ToString();
                    project.directorate = dr["directorate"].ToString();
                    project.department = dr["department"].ToString();
                    project.station = dr["station"].ToString();
                    project.dateOfTransfer = (DateTime)dr["dateOfTransfer"];
                    project.boxNumber = dr["boxNumber"].ToString();
                    project.status = dr["status"].ToString();
                    project.projectID = (int)dr["projectID"];

                    projectlist.Add(project);
                }

            }
            catch (Exception e)
            {
                e.ToString();
            }
            finally
            {
                con.Close();
                cmd.Dispose();
            }
            return projectlist;
        }
        public FileTransfer RejectProjects(int K)
        {
            FileTransfer projectrejected = new FileTransfer();
            int add = 0;
            try
            {
                cmd = new SqlCommand("Files_CRUD", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@projectID", K);
                cmd.Parameters.AddWithValue("@query", 11);
                con.Open();
                int l = (int)cmd.ExecuteScalar();
                if (l.Equals(1))
                {
                    add = l;

                }
                else
                {
                    add = -1;
                }


            }
            catch (Exception e)
            {
                e.ToString();
            }
            finally
            {
                con.Close();
                cmd.Dispose();
            }
            return projectrejected;
        }
        public List<FileTransfer> getRejectedProjects()
        {
            List<FileTransfer> projectlist = new List<FileTransfer>();

            try
            {
                cmd = new SqlCommand("Files_CRUD", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();

                cmd.Parameters.AddWithValue("@query", 9);
                con.Open();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    FileTransfer project = new FileTransfer();

                    project.projectName = dr["projectName"].ToString();
                    project.directorate = dr["directorate"].ToString();
                    project.department = dr["department"].ToString();
                    project.station = dr["station"].ToString();
                    project.dateOfTransfer = (DateTime)dr["dateOfTransfer"];
                    project.boxNumber = dr["boxNumber"].ToString();
                    project.status = dr["status"].ToString();
                    project.projectID = (int)dr["projectID"];

                    projectlist.Add(project);
                }

            }
            catch (Exception e)
            {
                e.ToString();
            }
            finally
            {
                con.Close();
                cmd.Dispose();
            }
            return projectlist;
        }
        public int updateUser(Userlog u)
        {
            int add = 0;
            try
            {
                cmd = new SqlCommand("Users_CRUD", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Firstname", u.Firstname);
                cmd.Parameters.AddWithValue("@Lastname", u.Lastname);
                cmd.Parameters.AddWithValue("@EmailAddress", u.EmailAddress);
                cmd.Parameters.AddWithValue("@Username", u.Username);
                cmd.Parameters.AddWithValue("@password", u.password);
                cmd.Parameters.AddWithValue("@role", u.role);
                cmd.Parameters.AddWithValue("@id", u.id);
                cmd.Parameters.AddWithValue("@query", 4);
                con.Open();
                int k = (int)cmd.ExecuteScalar();
                if (k.Equals(1))
                {
                    add = k;

                }
                else
                {
                    add = -1;
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            finally
            {
                con.Close();
                cmd.Dispose();
              
            }
            return add;
        }
        public void DeleteUser(int g)
        {

            try
            {
                cmd = new SqlCommand("Users_CRUD", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", g);
                cmd.Parameters.AddWithValue("@query", 5);
                con.Open();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            finally
            {
                con.Close();
                cmd.Dispose();
            }
        }
        
    }
    }
