using System.Data.SqlClient;
using System.Collections.Generic;

class ProjectController {

    public void createProject(Project a_project, int a_userID) {
        try {
            using(SqlConnection connection = new SqlConnection()) {
                connection.ConnectionString = "Server=.\\SQLExpress;Database=ChronoKeep;Trusted_Connection=true";
                connection.Open();

                using(SqlCommand insertCmd = new SqlCommand("INSERT INTO Project (userID, title, description, millisecondsTotal) VALUES (@0, @1, @2, @3)", connection)) {
                    insertCmd.Parameters.AddWithValue("@0", a_userID);
                    insertCmd.Parameters.AddWithValue("@1", a_project.title);
                    insertCmd.Parameters.AddWithValue("@2", a_project.description);
                    insertCmd.Parameters.AddWithValue("@3", a_project.millisecondsTotal);

                    insertCmd.ExecuteNonQuery();
                }
            }
        } catch(System.Exception e) {
            System.Console.WriteLine($"There was a database issue: {e.Message}");
        }
    }
    public Project queryProject(string a_title, int a_userID) {
        try {
            using(SqlConnection connection = new SqlConnection()) {
                connection.ConnectionString = "Server=.\\SQLExpress;Database=ChronoKeep;Trusted_Connection=true";
                connection.Open();

                using(SqlCommand queryCmd = new SqlCommand("SELECT * FROM Project WHERE title = @0 AND userID = @1", connection)) {
                    queryCmd.Parameters.AddWithValue("@0", a_title);
                    queryCmd.Parameters.AddWithValue("@1", a_userID);

                    using(SqlDataReader queryReader = queryCmd.ExecuteReader()) {
                        queryReader.Read();

                        return new Project(
                            a_title,
                            queryReader["description"].ToString() ?? "",
                            (long)(queryReader["millisecondsTotal"] ?? 0L)
                        );
                    }
                }
            }
        } catch(System.Exception) {
            return null;
        }
    }

    public List<Project> getAllUserProjects(int a_userID) {
        List<Project> userProjects = new List<Project>();

        try {
            using(SqlConnection connection = new SqlConnection()) {
                connection.ConnectionString = "Server=.\\SQLExpress;Database=ChronoKeep;Trusted_Connection=true";
                connection.Open();

                using(SqlCommand queryCmd = new SqlCommand("SELECT * FROM Project WHERE userID = @0", connection)) {
                    queryCmd.Parameters.AddWithValue("@0", a_userID);

                    using(SqlDataReader queryReader = queryCmd.ExecuteReader()) {
                        while(queryReader.Read()) {
                            userProjects.Add(
                                new Project(
                                    queryReader["title"].ToString() ?? "",
                                    queryReader["description"].ToString() ?? "",
                                    (long)(queryReader["millisecondsTotal"] ?? 0L)
                                )
                            );
                        }
                    }
                }
            }
        } catch(System.Exception) {
            return null;
        }

        return userProjects;
    }
}