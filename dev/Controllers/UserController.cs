using System.Data.SqlClient;

class UserController {
    private User loggedInUser = new User();
    public User getUser => this.loggedInUser;

    private ProjectController pc = new ProjectController();
    public ProjectController getPC => this.pc;
    
    public bool queryUser(string a_email, string a_plainTextPassword) {
        try {
            using(SqlConnection connection = new SqlConnection()) {
                connection.ConnectionString = "Server=.\\SQLExpress;Database=ChronoKeep;Trusted_Connection=true";
                connection.Open();

                using(SqlCommand queryCmd = new SqlCommand("SELECT * FROM ChronoUser WHERE email = @a_email", connection)) {
                    queryCmd.Parameters.AddWithValue("@a_email", a_email);

                    using(SqlDataReader queryReader = queryCmd.ExecuteReader()) {
                        queryReader.Read();
                        if(this.loggedInUser.hashPassword(a_plainTextPassword) != queryReader["password"].ToString())
                            return false;

                        this.loggedInUser.Id = (int)queryReader["id"];
                        this.loggedInUser.name = queryReader["name"].ToString() ?? "empty";
                        this.loggedInUser.Email = queryReader["email"].ToString() ?? "empty";
                        this.loggedInUser.setPreHashedPassword(queryReader["password"].ToString() ?? "empty");
                        this.loggedInUser.projects = this.pc.getAllUserProjects(this.loggedInUser.Id) ?? new System.Collections.Generic.List<Project>();
                    }
                }
            }
        } catch(SqlException e) {
            System.Console.WriteLine($"There was a Database error:\n{e.Message}");
            return false;
        } catch(System.Exception) {
            return false;
        }
        return true;
    }

    public void deleteUser(string a_email, string a_plainTextPassword) {
        //TODO: ask dad about removing children of User (such as projects)
        if(!(this.loggedInUser.Email.Equals(a_email) && this.loggedInUser.Password.Equals(this.loggedInUser.hashPassword(a_plainTextPassword)))) {
            System.Console.WriteLine("\n!!! Password or Email Invalid !!!\n");
            return;
        }
        try {
            using(SqlConnection connection = new SqlConnection("Server=.\\SQLExpress;Database=ChronoKeep;Trusted_Connection=true")) {
                connection.Open();

                using(SqlCommand deleteCmd = new SqlCommand("DELETE FROM ChronoUser WHERE email = '@a_email'", connection)) {
                    deleteCmd.Parameters.AddWithValue("@a_email", this.loggedInUser.Email);

                    deleteCmd.ExecuteNonQuery();
                    this.loggedInUser = null;
                }
            }
        } catch(SqlException e) {
            System.Console.WriteLine($"There was a Database error:\n{e.Message}");
        }
    }

    public void createUser(User a_newUser) {
        try {
            using(SqlConnection connection = new SqlConnection()) {
                connection.ConnectionString = "Server=.\\SQLExpress;Database=ChronoKeep;Trusted_Connection=true";
                connection.Open();

                using(SqlCommand insertCmd = new SqlCommand("INSERT INTO ChronoUser (name, email, password) VALUES (@a_name, @email, @a_password)", connection)) {
                    insertCmd.Parameters.AddWithValue("@a_name", a_newUser.name);
                    insertCmd.Parameters.AddWithValue("@email", a_newUser.Email);
                    insertCmd.Parameters.AddWithValue("@a_password", a_newUser.Password);
                    insertCmd.ExecuteNonQuery();
                }
            }
        } catch(SqlException e) {
            System.Console.WriteLine($"There was a Database error:\n{e.Message}");
            return;
        }
        this.loggedInUser = a_newUser;
    }

    public void modifyUser(string a_name, string a_email, string a_hashedPassword) {
        try {
            using(SqlConnection connection = new SqlConnection("Server=.\\SQLExpress;Database=ChronoKeep;Trusted_Connection=true")) {
                connection.Open();

                using(SqlCommand modifyCmd = new SqlCommand("UPDATE ChronoUser SET name = @0, email = @2, password = @3 WHERE email = @4", connection)) {
                    modifyCmd.Parameters.AddWithValue("@0", a_name);
                    modifyCmd.Parameters.AddWithValue("@2", a_email);
                    modifyCmd.Parameters.AddWithValue("@3", a_hashedPassword);
                    modifyCmd.Parameters.AddWithValue("@4", this.loggedInUser.Email);
                    modifyCmd.ExecuteNonQuery();
                }
            }
        } catch(SqlException e) {
            System.Console.WriteLine($"There was a Database error:\n{e.Message}");
            return;
        }
        this.loggedInUser.name = a_name;
        this.loggedInUser.Email = a_email;
        this.loggedInUser.setPreHashedPassword(a_hashedPassword);
    }
}