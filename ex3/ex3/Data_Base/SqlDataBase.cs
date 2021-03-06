﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.Helpers;
using Newtonsoft.Json;

namespace ex3.Data_Base
{
    public class SqlDataBase
    {
        SqlConnection conn;
        SqlDataReader reader;
        SqlCommand cmd;

        public SqlDataBase()
        {
            this.conn = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = |DataDirectory|DB.mdf; Integrated Security = True");            this.reader = null;
            this.cmd = null;
        }

        public void AddUserToDB(string username, string password,string email, string salt)
        {
            try
            {
                this.conn.Open();
                this.cmd = new SqlCommand("insert into Users (UserName,Password,Email,Salt) values (@username,@password,@email,@salt)", conn);
                this.cmd.Parameters.AddWithValue("@username", username);
                this.cmd.Parameters.AddWithValue("@password", password);
                this.cmd.Parameters.AddWithValue("@email", email);
                this.cmd.Parameters.AddWithValue("@salt", salt);
                cmd.ExecuteNonQuery();            }
            finally
            {
                this.Close();
            }
        }

        public void CreateUserInRankigsTableDB(string username, int wins, int losses)
        {
            try
            {
                int id = this.GetUserIdByName(username);
                this.conn.Open();
                this.cmd = new SqlCommand("insert into UserRankings (ID,UserName,Wins,Losses) values (@id,@username,@wins,@losses)", conn);
                this.cmd.Parameters.AddWithValue("@id", id);
                this.cmd.Parameters.AddWithValue("@username", username);
                this.cmd.Parameters.AddWithValue("@wins", wins);
                this.cmd.Parameters.AddWithValue("@losses", losses);
                cmd.ExecuteNonQuery();            }
            finally
            {
                this.Close();
            }
        }

        public void UpdateWinsByUser(int id)
        {
            try
            {
                int wins = this.GetWinsByUserID(id)+1;
                this.conn.Open();
                this.cmd = new SqlCommand("UPDATE UserRankings SET Wins=@wins WHERE ID =@id", conn);
                this.cmd.Parameters.AddWithValue("@wins", wins);
                this.cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            finally
            {
                this.Close();
            }
        }

        public void UpdateLossesByUser(int id)
        {
            try
            {
                int losses = this.GetLossesByUserID(id) + 1;
                this.conn.Open();
                this.cmd = new SqlCommand("UPDATE UserRankings SET Losses=@losses WHERE ID =@id", conn);
                this.cmd.Parameters.AddWithValue("@losses", losses);
                this.cmd.Parameters.AddWithValue("@id", id);
                this.cmd.ExecuteNonQuery();
            }
            finally
            {
                this.Close();
            }
        }

        public int CheckIfUsernameExist(string username)
        {
            try
            {
                this.conn.Open();
                this.cmd = new SqlCommand("select count(*) from Users where UserName=@username", conn);
                this.cmd.Parameters.AddWithValue("@username", username);
                this.reader = cmd.ExecuteReader();
                string str = "";
                int counter;
                while (this.reader.Read())
                    str += this.reader[0];
                int.TryParse(str, out counter);
                return counter;
            }
            finally
            {
                this.Close();
            }
        }

        public int GetUserIdByName(string username)
        {
            try
            {
                this.conn.Open();
                this.cmd = new SqlCommand("select ID from Users where UserName=@username", conn);
                this.cmd.Parameters.AddWithValue("@username", username);
                this.reader = cmd.ExecuteReader();
                string str = "";
                int id;
                while (this.reader.Read())
                    str += this.reader[0];
                int.TryParse(str, out id);
                return id;
            }
            finally
            {
                this.Close();
            }
        }

        public int GetWinsByUserID(int id)
        {
            try
            {
                this.conn.Open();
                this.cmd = new SqlCommand("select Wins from UserRankings where ID=@id", conn);
                this.cmd.Parameters.AddWithValue("@id", id);
                this.reader = cmd.ExecuteReader();
                string str = "";
                int wins;
                while (this.reader.Read())
                    str += this.reader[0];
                int.TryParse(str, out wins);
                return wins;
            }
            finally
            {
                this.Close();
            }
        }

        public int GetLossesByUserID(int id)
        {
            try
            {
                this.conn.Open();
                this.cmd = new SqlCommand("select Losses from UserRankings where ID=@id", conn);
                this.cmd.Parameters.AddWithValue("@id", id);
                this.reader = cmd.ExecuteReader();
                string str = "";
                int losses;
                while (this.reader.Read())
                    str += this.reader[0];
                int.TryParse(str, out losses);
                return losses;
            }
            finally
            {
                this.Close();
            }
        }

        public bool checkPassword(string username, string password)
        {
            try
            {
                int id = this.GetUserIdByName(username);
                this.conn.Open();
                this.cmd = new SqlCommand("select Password,Salt from Users where ID=@id", conn);
                this.cmd.Parameters.AddWithValue("@id", id);
                this.reader = cmd.ExecuteReader();
                string strPassword = "";
                string strSalt = "";
                while (this.reader.Read())
                {
                    strPassword = this.reader[0].ToString();
                    strSalt = this.reader[1].ToString();
                }
                if(Crypto.SHA256(password + strSalt) == strPassword)
                    return true;
                else
                    return false;             
            }
            finally
            {
                this.Close();
            }
        }

        public List<UserRank> getRankingData()
        {
            try
            {
                List<UserRank> usersRank = new List<UserRank>();                
                this.conn.Open();
                this.cmd = new SqlCommand("select UserName,Wins,Losses from UserRankings order by (Wins-Losses) desc ,Wins desc", conn);
                this.reader = cmd.ExecuteReader();
                int i = 0;
                while (this.reader.Read())
                {
                    UserRank user = new UserRank();
                    user.Id = i;
                    user.Username = this.reader[0].ToString();
                    user.Wins = int.Parse(this.reader[1].ToString());
                    user.Losses = int.Parse(this.reader[2].ToString());
                    usersRank.Add(user);
                    i++;
                }
                return usersRank;
            }
            finally
            {
                this.Close();
            }
        }

        public void Close()
        {
            if (this.reader != null)
                this.reader.Close();
            if (this.conn != null)
                this.conn.Close();
        }
    }

    public class UserRank
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
    }
}