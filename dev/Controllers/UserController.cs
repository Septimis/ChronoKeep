using System.Data.SqlClient;

class UserController {
    private User loggedInUser = new User();
    public User getUser => this.loggedInUser;
    
    public void queryUser(string a_email, string a_plainTextPassword) {
        try {
            using(SqlConnection connection = new SqlConnection()) {
                connection.ConnectionString = "Server=.\\SQLExpress;Database=ChronoKeep;Trusted_Connection=true";
                connection.Open();

                using(SqlCommand queryCmd = new SqlCommand("SELECT * FROM ChronoUser WHERE email = @a_email", connection)) {
                    queryCmd.Parameters.AddWithValue("@a_email", a_email);

                    using(SqlDataReader queryReader = queryCmd.ExecuteReader()) {
                        queryReader.Read();
                        if(this.loggedInUser.hashPassword(a_plainTextPassword) != queryReader["password"].ToString()) {
                            System.Console.WriteLine("\n!!! Password did not match !!!\n");
                            return;
                        }

                        this.loggedInUser.name = queryReader["name"].ToString() ?? "empty";
                        this.loggedInUser.millisecondsTotal = long.Parse(queryReader["millisecondsTotal"].ToString() ?? "0");
                        this.loggedInUser.Email = queryReader["email"].ToString() ?? "empty";
                        this.loggedInUser.setPreHashedPassword(queryReader["password"].ToString() ?? "empty");
                    }
                }
            }
        } catch(SqlException e) {
            System.Console.WriteLine($"There was a Database error:\n{e.Message}");
            return;
        }
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

                using(SqlCommand insertCmd = new SqlCommand("INSERT INTO ChronoUser (name, millisecondsTotal, email, password) VALUES (@a_name, @a_msTot, @email, @a_password)", connection)) {
                    insertCmd.Parameters.AddWithValue("@a_name", a_newUser.name);
                    insertCmd.Parameters.AddWithValue("@a_msTot", 0);
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

    public void modifyUser(User a_modifiedUser) {
        try {
            using(SqlConnection connection = new SqlConnection("Server=.\\SQLExpress;Database=ChronoKeep;Trusted_Connection=true")) {
                connection.Open();

                using(SqlCommand modifyCmd = new SqlCommand("UPDATE ChronoUser SET name = '@a_name', millisecondsTotal = '@a_msTot', email = '@a_email', password = '@a_password' WHERE email = '@a_email'", connection)) {
                    modifyCmd.Parameters.AddWithValue("@a_name", a_modifiedUser.name);
                    modifyCmd.Parameters.AddWithValue("@a_msTot", a_modifiedUser.millisecondsTotal);
                    modifyCmd.Parameters.AddWithValue("@a_email", a_modifiedUser.Email);
                    modifyCmd.Parameters.AddWithValue("@a_password", a_modifiedUser.Password);
                    modifyCmd.ExecuteNonQuery();
                }
            }
        } catch(SqlException e) {
            System.Console.WriteLine($"There was a Database error:\n{e.Message}");
            return;
        }
        this.loggedInUser = a_modifiedUser;
    }
}