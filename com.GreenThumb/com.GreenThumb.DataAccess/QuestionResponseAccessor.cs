﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.GreenThumb.BusinessObjects;

namespace com.GreenThumb.DataAccess
{
    public class QuestionResponseAccessor
    {
        /// <summary>
        /// Rhett Allen
        /// Created Date: 3/23/16
        /// Inserts a response into the database
        /// </summary>
        /// <param name="response">Response to be added</param>
        /// <returns>True if the response was successfully added</returns>
        public static bool InsertResponse(Response response)
        {
            bool inserted = false;

            var conn = DBConnection.GetDBConnection();
            var query = @"Expert.spInsertQuestionResponse";
            var cmd = new SqlCommand(query, conn);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@QuestionID", response.QuestionID);
            cmd.Parameters.AddWithValue("@Date", response.Date);
            cmd.Parameters.AddWithValue("@Response", response.UserResponse);
            cmd.Parameters.AddWithValue("@UserID", response.UserID);

            try
            {
                conn.Open();

                if (cmd.ExecuteNonQuery() == 1)
                {
                    inserted = true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return inserted;
        }

        /// <summary>
        /// Rhett Allen
        /// Created Date: 3/23/16
        /// Gets all responses to a certain question
        /// </summary>
        /// <param name="questionID">Question ID</param>
        /// <returns>All responses to a certain question</returns>
        public static List<Response> FetchResponsesByQuestionID(int questionID)
        {
            List<Response> responses = new List<Response>();

            var conn = DBConnection.GetDBConnection();
            var query = @"Expert.spSelectResponsesByQuestionID";
            var cmd = new SqlCommand(query, conn);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@QuestionID", questionID);

            try
            {
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Response response = new Response()
                        {
                            QuestionID = reader.GetInt32(0),
                            Date = reader.GetDateTime(1),
                            UserResponse = reader.GetString(2),
                            UserID = reader.GetInt32(3)
                        };

                        responses.Add(response);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return responses;
        }

        /// <summary>
        /// Rhett Allen
        /// Created Date: 3/23/16
        /// Gets a response to a question from a user
        /// </summary>
        /// <param name="questionID">QuestionID</param>
        /// <param name="userID">UserID of user that replied</param>
        /// <returns>A response to a question from a user</returns>
        public static Response FetchResponseByQuestionIDAndUser(int questionID, int userID)
        {
            Response response = new Response();

            var conn = DBConnection.GetDBConnection();
            var query = @"Expert.spSelectResponseByQuestionIDAndUser";
            var cmd = new SqlCommand(query, conn);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@QuestionID", questionID);
            cmd.Parameters.AddWithValue("@UserID", userID);

            try
            {
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    response = new Response()
                    {
                        QuestionID = reader.GetInt32(0),
                        Date = reader.GetDateTime(1),
                        UserResponse = reader.GetString(2),
                        UserID = reader.GetInt32(3)
                    };
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return response;
        }

        /// <summary>
        /// Rhett Allen
        /// Created Date: 3/23/16
        /// Updates an existing response
        /// </summary>
        /// <param name="newResponse">Updated response</param>
        /// <param name="oldResponse">Unchanged response</param>
        /// <returns>True if the response successfully updated</returns>
        public static bool UpdateResponse(Response newResponse, Response oldResponse)
        {
            bool updated = false;

            var conn = DBConnection.GetDBConnection();
            var query = @"Expert.spUpdateQuestionResponse";
            var cmd = new SqlCommand(query, conn);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@QuestionID", newResponse.QuestionID);
            cmd.Parameters.AddWithValue("@UserID", newResponse.UserID);
            cmd.Parameters.AddWithValue("@Response", newResponse.UserResponse);
            cmd.Parameters.AddWithValue("@OriginalResponse", oldResponse.UserResponse);

            try
            {
                conn.Open();

                if (cmd.ExecuteNonQuery() == 1)
                {
                    updated = true;
                }

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return updated;
        }
    }
}