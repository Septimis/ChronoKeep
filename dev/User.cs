using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

class User {
    public int Id { get; set; } = 0;
    public string name { get; set; } = "empty";
    private string email = "empty";
    public string Email {
        get => this.email.Equals("empty") ? "No email exists..." : this.email;
        set {
            //validate email
            Regex emailRegex = new Regex("^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$");
            if(!emailRegex.IsMatch(value)) throw new System.ArgumentException($"{value} is not a valid email...");
            this.email = value;
        }
    }
    public List<Project> projects { get; set; }
    private string password = "empty";
    public string Password {
        get { return this.password; }
        set { this.password = hashPassword(value); } 
    }
    public void setPreHashedPassword(string a_hashedPassword) { this.password = a_hashedPassword; }

    public User() { /* Empty on purpose */ }

    //Initialized user Constructor
    public User(string a_name, string a_email, string a_plainTextPassword) {
        this.name = a_name;
        this.Email = a_email;
        this.Password = a_plainTextPassword;
    }

    public string hashPassword(string a_plainTextPassword) {
        SHA256 hash = SHA256.Create();
        byte[] pwBytes = Encoding.Default.GetBytes(a_plainTextPassword);
        return System.Convert.ToHexString(hash.ComputeHash(pwBytes));
    }

    public override string ToString() {
        return $"Name: {this.name ?? "[empty]"}\nemail: {this.email}\n";
    }
}