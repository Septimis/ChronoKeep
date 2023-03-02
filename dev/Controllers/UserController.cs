using System.Data.SqlClient;

class UserController {
    private User? loggedInUser;
    public User getUser => this.loggedInUser;
    
    public void queryUser(string a_email, string a_plainTextPassword) {
        //Query DB to find if user exists & set it
    }

    public void deleteUser(string a_email, string a_plainTextPassword) {
        //remove User and all children from DB
    }

    public void createUser(User a_newUser) {
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
    }

    public void modifyUser(User a_modifiedUser) {
        //modify user in DB & then set local variable to the modified version
    }
}