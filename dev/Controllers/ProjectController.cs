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
    public void deleteProject(string a_projectName, int a_userID) {
        try {
            using(SqlConnection connection = new SqlConnection()) {
                connection.ConnectionString = "Server=.\\SQLExpress;Database=ChronoKeep;Trusted_Connection=true";
                connection.Open();

                using(SqlCommand deleteCmd = new SqlCommand("DELETE FROM Project WHERE title = @0, userID = @1")) {
                    deleteCmd.Parameters.AddWithValue("@0", a_projectName);
                    deleteCmd.Parameters.AddWithValue("@1", a_userID);

                    deleteCmd.ExecuteNonQuery();
                }
            }
        } catch(System.Exception) {
            System.Console.WriteLine($"There was an issue with deleting {a_projectName}...");
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

    public void modifyProject(string a_title, string a_description, long a_millisecondsTotal, int a_userID, string a_newTitle = "") {
        try {
            using(SqlConnection connection = new SqlConnection()) {
                connection.ConnectionString = "Server=.\\SQLExpress;Database=ChronoKeep;Trusted_Connection=true";
                connection.Open();

                using(SqlCommand modifyCmd = new SqlCommand("UPDATE Project SET title = @0, description = @1, millisecondsTotal = @2 WHERE title = @3 AND userID = @4", connection)) {
                    modifyCmd.Parameters.AddWithValue("@0", (a_newTitle == "") ? a_title : a_newTitle); //blank new title means title stays the same.
                    modifyCmd.Parameters.AddWithValue("@1", a_description);
                    modifyCmd.Parameters.AddWithValue("@2", a_millisecondsTotal);
                    modifyCmd.Parameters.AddWithValue("@3", a_title);
                    modifyCmd.Parameters.AddWithValue("@4", a_userID);

                    modifyCmd.ExecuteNonQuery();
                }
            }
        } catch(System.Exception) {
            System.Console.WriteLine($"Could not modify {a_title}");
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