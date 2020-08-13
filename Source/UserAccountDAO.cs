using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using DBLibrary.Entities;

namespace DBLibrary.DAL
{
    public class UserAccountDAO
    {
        string cnnString;
        public UserAccountDAO(string cnnString)
        {
            this.cnnString = cnnString;
        }

        //add a userAccount records
        public void AddUser(UserAccount userAccount)
        {
            //open connection
            SqlConnection cnn = new SqlConnection(cnnString);
            cnn.Open();

            //add record 
            SqlCommand cmd = new SqlCommand("insert into userAccount (password,firstname,lastname,address,city,postalcode,phone,email) " +
                " values (@Password, @Firstname, @Lastname, @Address, @City, @Postalcode, @Phone, @UserEmail) ");
            
            cmd.Parameters.AddWithValue("@Password", userAccount.Password);
            cmd.Parameters.AddWithValue("@Firstname", userAccount.FirstName);
            cmd.Parameters.AddWithValue("@Lastname", userAccount.LastName);
            cmd.Parameters.AddWithValue("@Address", userAccount.Address);
            cmd.Parameters.AddWithValue("@City", userAccount.City);
            cmd.Parameters.AddWithValue("@Postalcode", userAccount.PostalCode);
            cmd.Parameters.AddWithValue("@Phone", userAccount.Phone);
            cmd.Parameters.AddWithValue("@UserEmail", userAccount.Email);
            cmd.Connection = cnn;
            // close connection
            cmd.ExecuteNonQuery();
            cnn.Close();
        }
        // list a userAccount information
        public List<UserAccount> ReadAll()
        {
            List<UserAccount> userAccounts = new List<UserAccount>();
            using (SqlConnection cnn = new SqlConnection(cnnString))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand($"select userAccountID,password,firstname,lastname,address,city,postalcode,phone,email from userAccount");
                cmd.Connection = cnn;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    userAccounts.Add(new UserAccount(Convert.ToInt32(reader[0]), reader[1].ToString(), reader[2].ToString(), 
                        reader[3].ToString(), reader[4].ToString(), reader[5].ToString(), reader[6].ToString(), 
                        reader[7].ToString(), reader[8].ToString()));
                }
            }
            return userAccounts;
        }

        // delete the userAccount by userAccountID
        public void DeleteRecord(string userAccountID)
        {
            SqlConnection cnn = new SqlConnection(cnnString);
            cnn.Open();

            SqlCommand cmd = new SqlCommand($"delete from userAccount where userAccount = {userAccountID}");
            cmd.Connection = cnn;

            cmd.ExecuteNonQuery();
            cnn.Close();
        }

        //Update the userAccount records
        public void UpdateRecord(UserAccount userAccount)
        {
            SqlConnection cnn = new SqlConnection(cnnString);
            cnn.Open();

            SqlCommand cmd = new SqlCommand($"update userAccount set password=@Password,firstname=@Firstname,lastname=@Lastname, " +
                                        $" address=@Address, city=@City, postalcode=@Postalcode, phone=@Phone " +
                                        $" where userAccountID=@UserAccountID");
           
            cmd.Parameters.AddWithValue("@Password", userAccount.Password);
            cmd.Parameters.AddWithValue("@Firstname", userAccount.FirstName);
            cmd.Parameters.AddWithValue("@Lastname", userAccount.LastName);
            cmd.Parameters.AddWithValue("@Address", userAccount.Address);
            cmd.Parameters.AddWithValue("@City", userAccount.City);
            cmd.Parameters.AddWithValue("@Postalcode", userAccount.PostalCode);
            cmd.Parameters.AddWithValue("@Phone", userAccount.Phone);
            cmd.Parameters.AddWithValue("@UserAccountID", userAccount.UserAccountID);
            cmd.Connection = cnn;

            cmd.ExecuteNonQuery();
            cnn.Close();
        }

        //find userAccount by userID
        public UserAccount FindByUserAccountID(string userAccountID)
        {
            UserAccount userAccount = null;
            using (SqlConnection cnn = new SqlConnection(cnnString))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand($"select userAccountID,password,firstname,lastname,address,city,postalcode,phone,email" +
                                                 $"from userAccount where userAccountID=@UserAccountID");
                cmd.Parameters.AddWithValue("@UserAccountID", userAccountID);

                cmd.Connection = cnn;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    userAccount = new UserAccount(Convert.ToInt32(reader[0]), reader[1].ToString(),reader[2].ToString(), reader[3].ToString(),
                                     reader[4].ToString(),reader[5].ToString(), reader[6].ToString(), reader[7].ToString(), 
                                     reader[8].ToString());
                }
            }
            return userAccount;
        }

        //find userAccount by userID
        public UserAccount FindByUserAccountID(int userAccountID)
        {
            UserAccount userAccount = null;
            using (SqlConnection cnn = new SqlConnection(cnnString))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand($"select userAccountID,password,firstname,lastname,address,city,postalcode,phone,email" +
                                                 $" from userAccount where userAccountID=@UserAccountID");
                cmd.Parameters.AddWithValue("@UserAccountID", userAccountID);

                cmd.Connection = cnn;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    userAccount = new UserAccount(Convert.ToInt32(reader[0]), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(),
                                     reader[4].ToString(), reader[5].ToString(), reader[6].ToString(), reader[7].ToString(),
                                     reader[8].ToString());
                }
            }
            return userAccount;
        }

        //validate the userAccount by email
        public Boolean ValidateUserAccountExistByUserID(string userAccountID)
        {
            return FindByUserAccountID(userAccountID) != null;
        }

        // find the userAccount by email
        public UserAccount FindUserAccountByEmail(string email)
        {
            UserAccount userAccount = null;
            //open connection
            using (SqlConnection cnn = new SqlConnection(cnnString))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand($"select userAccountID,password,firstname,lastname,address,city,postalcode,phone,email " +
                                                $"from userAccount where email=@Email");
                cmd.Parameters.AddWithValue("@Email", email);

                cmd.Connection = cnn;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    userAccount = new UserAccount(Convert.ToInt32(reader[0]), reader[1].ToString(), reader[2].ToString(),
                        reader[3].ToString(), reader[4].ToString(), reader[5].ToString(), reader[6].ToString(),
                        reader[7].ToString(), reader[8].ToString());
                }
            }
            return userAccount;
        }

        //validate userAccount by email
        public Boolean ValidateUserAccountExistByEmail(string Email)
        {
            return FindUserAccountByEmail(Email) != null;
        }

        // find the userAccount by email and password
        public UserAccount FindUserAccountByEmailPassword(string email, string password)
        {
            UserAccount userAccount = null;
            //open connection
            using (SqlConnection cnn = new SqlConnection(cnnString))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand($"select userAccountID,password,firstname,lastname,address,city,postalcode,phone,email from UserAccount where email = @Email and password = @Password");


                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);

                cmd.Connection = cnn;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    userAccount = new UserAccount(Convert.ToInt32(reader[0]), reader[1].ToString(), reader[2].ToString(),
                        reader[3].ToString(), reader[4].ToString(), reader[5].ToString(), reader[6].ToString(),
                        reader[7].ToString(), reader[8].ToString());
                }
            }
            return userAccount;
        }

        //validate userAccount by email and password
        public Boolean ValidateUserAccountExistByEmailPassword(string Email, string password)
        {
            return FindUserAccountByEmailPassword(Email, password) != null;
        }



    }

}

