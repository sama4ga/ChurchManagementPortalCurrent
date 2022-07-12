using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;

namespace ChurchManagementPortal
{
    public class Parish
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Motto { get; set; }
        public string Logo { get; set; }
        SQL sql = new();
        Utility utility = new();

        public Parish(string name, string address, string motto, string logo="")
        {
            Name = name;
            Address = address;
            Motto = motto;
            Logo = logo;
        }

        public bool addParishioner(string pId, string title, string surname, string othernames, string resAddress,
            string phoneNo, string gender, string maritalStatus, int stateId, int lgaId, int dioceseId,
            int deaneryId, int parishId, int stationId, int organisationId, int societyId, string dob,
            string nextOfKinName, string nextOfKinAddress, string nextOfKinPhoneNo,
            List<int> otherSocietyIds, List<int> piousSocietyIds, List<int> layApostolateIds, string whatCanYouDo = "",
            string officeAddress = "", string spouseName = "", string email = "", string occupation = "",
            string passportFile = "", string baptised = "False", string communicant = "False",
            string confirmed = "False", string wedded = "False", string status = "Active")
        {
            if (utility.checkNullOrEmpty(surname)){ MessageBox.Show("Surname is required", "Error"); return false; }
            if (utility.checkNullOrEmpty(othernames)) { MessageBox.Show("Othernames is required", "Error"); return false; }
            if (utility.checkNullOrEmpty(resAddress)) { MessageBox.Show("Residential address is required", "Error"); return false; }
            if (utility.checkNullOrEmpty(phoneNo)) { MessageBox.Show("Phone number is required", "Error"); return false; }
            if (utility.checkNullOrEmpty(gender)) { MessageBox.Show("Gender is required", "Error"); return false; }
            if (utility.checkNullOrEmpty(title)) { MessageBox.Show("Title status is required", "Error"); return false; }
            if (utility.checkNullOrEmpty(maritalStatus)) { MessageBox.Show("Marital status is required", "Error"); return false; }
            if (stateId == -1) { MessageBox.Show("State of origin is required", "Error"); return false; }
            if (lgaId == -1) { MessageBox.Show("LGA of origin is required", "Error"); return false; }
            if (dioceseId == -1) { MessageBox.Show("Diocese of origin is required", "Error"); return false; }
            if (deaneryId == -1) { MessageBox.Show("Deanery of origin is required", "Error"); return false; }
            if (parishId == -1) { MessageBox.Show("Parish of origin is required", "Error"); return false; }
            if (stationId == -1) { MessageBox.Show("Station is required", "Error"); return false; }
            if (organisationId == -1) { MessageBox.Show("Organisation is required", "Error"); return false; }
            if (societyId == -1) { MessageBox.Show("Society is required", "Error"); return false; }

            if (!utility.isValidPhoneNo(phoneNo)) { MessageBox.Show("Phone number is not a valid phone number", "Error"); return false; }
            if (nextOfKinPhoneNo != "" && !utility.isValidPhoneNo(phoneNo)) { MessageBox.Show("Next of kin phone number is not a valid phone number", "Error"); return false; }
            if (!utility.isValidEmail(email)) { MessageBox.Show("Email is not a valid email address", "Error"); return false; }

            MySqlConnection con = sql.GetCon();
            MySqlTransaction transaction = con.BeginTransaction();
            
            string query = "INSERT INTO `parishioner`(`regNo`,`title`,`surname`,`othernames`,`resAddress`,`officeAddress`,`occupation`,`phoneNo`," +
                "`email`,`gender`,`maritalStatus`,`dob`,`spouseName`,`stateId`,`lgaId`, `dioceseId`,`deaneryId`,`parishId`," +
                "`stationId`,`societyId`,`organisationId`,`baptised`,`confirmed`,`communicant`,`wedded`,`status`,`whatCanYouDo`) VALUES(0," +
                "@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12,@13,@14,@15,@16,@17,@18,@19,@20,@21,@22,@23,@24,@25,@26);";
            MySqlCommand cmd = new(query,con);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@1",title); cmd.Parameters.AddWithValue("@2", surname); cmd.Parameters.AddWithValue("@3", othernames); cmd.Parameters.AddWithValue("@4", resAddress);
            cmd.Parameters.AddWithValue("@5", officeAddress); cmd.Parameters.AddWithValue("@6", occupation); cmd.Parameters.AddWithValue("@7", phoneNo); cmd.Parameters.AddWithValue("@8", email);
            cmd.Parameters.AddWithValue("@9", gender); cmd.Parameters.AddWithValue("@10", maritalStatus); cmd.Parameters.AddWithValue("@11", dob); cmd.Parameters.AddWithValue("@12", spouseName);
            cmd.Parameters.AddWithValue("@13", stateId); cmd.Parameters.AddWithValue("@14", lgaId); cmd.Parameters.AddWithValue("@15", dioceseId); cmd.Parameters.AddWithValue("@16", deaneryId);
            cmd.Parameters.AddWithValue("@17", parishId); cmd.Parameters.AddWithValue("@18", stationId); cmd.Parameters.AddWithValue("@19", societyId); cmd.Parameters.AddWithValue("@20", organisationId);
            cmd.Parameters.AddWithValue("@21", baptised); cmd.Parameters.AddWithValue("@22", confirmed); cmd.Parameters.AddWithValue("@23", communicant); cmd.Parameters.AddWithValue("@24", wedded);
            cmd.Parameters.AddWithValue("@25", status); cmd.Parameters.AddWithValue("@26", whatCanYouDo);
            try
            {
                cmd.ExecuteNonQuery();
                long parishionerId = cmd.LastInsertedId;
                                
                query = "INSERT INTO `nextOfKin`(`pId`,`name`,`phoneNo`,`address`) VALUES(@1,@2,@3,@4);";
                cmd.CommandText = query;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@1", parishionerId); cmd.Parameters.AddWithValue("@2", nextOfKinName); cmd.Parameters.AddWithValue("@3", nextOfKinPhoneNo); cmd.Parameters.AddWithValue("@4", nextOfKinAddress);
                cmd.ExecuteNonQuery();

                query = "INSERT INTO `parishionerGroup`(`groupId`,`pId`) VALUES";
                foreach (int id in piousSocietyIds)
                {
                    query += "("+id+","+parishionerId+"), ";
                }

                foreach (int id in layApostolateIds)
                {
                    query += "(" + id + "," + parishionerId + "), ";
                }

                foreach (int id in otherSocietyIds)
                {
                    query += "(" + id + "," + parishionerId + "), ";
                }

                query = query.Substring(0,query.Length - 2);
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();

                pId = pId == "" ? parishionerId.ToString() : pId;
                if (passportFile != "")
                {
                    DirectoryInfo path = Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/Passport");
                    string filename = (path.FullName + "/parishioner" + parishionerId + ".jpeg").Replace("\\", "/");
                    utility.resizeImage(passportFile, filename, new System.Drawing.Size(150, 150));

                    cmd.CommandText = "UPDATE `parishioner` SET `passport`='" + filename + "',`regNo`=@1 WHERE `pId`=" + parishionerId + ";";
                    cmd.Parameters.Clear(); cmd.Parameters.AddWithValue("@1", pId);
                    cmd.ExecuteNonQuery();
                }

                transaction.Commit();
                return true;
            }
            catch (MySqlException ex)
            {
                transaction.Rollback();
                return false;
            }
            
        }

        /// <summary>
        /// Add a station to the Parish
        /// </summary>
        /// <param name="stationName">Name of the station</param>
        /// <param name="stationAddress">Address of the station</param>
        /// <param name="creationDate">Date the station was created</param>
        /// <param name="stationPicture">Path to the image file to be used as station picture</param>
        /// <returns>True on success and false otherwise</returns>
        public bool addStation(string stationName, string stationAddress, string creationDate, string stationPicture="")
        {
            string stationPic = "";
            string codename = utility.createAcronym(stationName);
            DirectoryInfo path = Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/StationPictures");
            if (!string.IsNullOrWhiteSpace(stationPicture))
            {
                stationPic = (path.FullName + "/" + codename.ToUpper() + ".jpeg").Replace("\\", "/");
                utility.resizeImage(stationPicture, stationPic, new System.Drawing.Size(200, 200));
            }
            bool result = sql.InsertQuery("INSERT INTO `station`(`name`,`address`,`creationDate`,`picture`) VALUES(@1,@2,@3,@4);",
                new List<object> {stationName,stationAddress,creationDate,stationPic });
            if (result)
            {
                return true;
            }

            File.Delete(stationPic);
            return false;
        }

        public bool addOrganisation(string organisationName, string organisationAcronym, string organisationSlogan)
        {
            bool result = sql.InsertQuery("INSERT INTO `organisation`(`name`,`acronym`,`slogan`) VALUES(@1,@2,@3);",
               new List<object> { organisationName, organisationAcronym, organisationSlogan});
            return result;
        }

        public bool addWorkingSociety(string societyName, string societySlogan, string stationId, string organisationId, string meetingDay, string meetingType, string meetingFrequency)
        {
            string societyAcronym = utility.createAcronym(societyName);
            bool result = sql.InsertQuery("INSERT INTO `society`(`name`,`acronym`,`slogan`,`stationId`,`organisationId`,`meetingDay`,`meetingType`,`meetingFrequency`) VALUES(@1,@2,@3,@4,@5,@6,@7,@8);",
                new List<object> {societyName, societyAcronym, societySlogan, stationId, organisationId, meetingDay, meetingType, meetingFrequency});
            return result;
        }

        public bool addSociety(string societyName, string societySlogan, string meetingDay, string meetingType, string meetingFrequency, string groupType)
        {
            string societyAcronym = utility.createAcronym(societyName);
            bool result = sql.InsertQuery("INSERT INTO `group`(`name`,`acronym`,`slogan`,`meetingDay`,`meetingType`,`meetingFrequency`,`groupType`) VALUES(@1,@2,@3,@4,@5,@6,@7);",
                new List<object> { societyName, societyAcronym, societySlogan, meetingDay, meetingType, meetingFrequency, groupType});
            return result;
        }

        public bool addPriest(string priestName, string priestPhoneNo, string priestType, string dateResumed, string priestPicture="", string priestEmail="")
        {
            bool hasError = false;
            if (utility.checkNullOrEmpty(priestName)) { MessageBox.Show("Enter Priest name is to proceed", "Input Required", MessageBoxButton.OK, MessageBoxImage.Error); hasError = true; }
            if (utility.checkNullOrEmpty(priestType)) { MessageBox.Show("Choose priest type to proceed", "Input Required", MessageBoxButton.OK, MessageBoxImage.Error); hasError = true; }
            if (utility.checkNullOrEmpty(dateResumed)) { MessageBox.Show("Choose date priest resumed to proceed", "Input Required", MessageBoxButton.OK, MessageBoxImage.Error); hasError = true; }
            if (priestEmail != "" && !utility.isValidEmail(priestEmail)) { MessageBox.Show("Invalid email entered", "Input Required", MessageBoxButton.OK, MessageBoxImage.Error); hasError = true; }
            
            if (!hasError)
            {
                MySqlConnection con = sql.GetCon();
                MySqlCommand cmd = con.CreateCommand();
                MySqlTransaction transaction = con.BeginTransaction();

                try
                {
                    cmd.CommandText = "INSERT INTO `priest`(`name`,`phoneNo`,`email`,`dateResumed`,`type`) VALUES(@1,@2,@3,@4,@5);";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@1", priestName); cmd.Parameters.AddWithValue("@2", priestPhoneNo); cmd.Parameters.AddWithValue("@3", priestEmail);
                    cmd.Parameters.AddWithValue("@4", dateResumed); cmd.Parameters.AddWithValue("@5", priestType);
                    cmd.ExecuteNonQuery();
                    long priestId = cmd.LastInsertedId;

                    if (priestPicture != "")
                    {
                        DirectoryInfo path = Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/Passport");
                        string filename = (path.FullName + "/priest" + priestId + ".jpeg").Replace("\\", "/");
                        utility.resizeImage(priestPicture, filename, new System.Drawing.Size(150, 150));

                        cmd.CommandText = "UPDATE `priest` SET `picture`='" + filename + "' WHERE `priestId`=" + priestId + ";";
                        cmd.ExecuteNonQuery();

                        transaction.Commit();
                        return true;
                    }
                }
                catch(MySqlException ex)
                {
                    transaction.Rollback();
                }
            }
            return false;
        }
    }
}
