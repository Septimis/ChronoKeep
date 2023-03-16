class Project {
    public Project(string a_title, string a_description, long a_millisecondsTotal) {
        this.title = a_title;
        this.description = a_description;
        this.millisecondsTotal = a_millisecondsTotal;
    }
    public string title { get; set; } = "[empty]";
    public string description { get; set; } = "";
    public long millisecondsTotal { get; set; } = 0L;

    public override string ToString() {
        long remainingMillis = this.millisecondsTotal;
        int numYears = (int)(remainingMillis / (1000L * 60L * 60L * 24L * 365L));
        remainingMillis -= numYears * (1000L * 60L * 60L * 24L * 365L);

        int numDays = (int)(remainingMillis / (1000 * 60 * 24));
        remainingMillis -= numDays * (1000 * 60 * 60 * 24);

        int numHours = (int)(remainingMillis / (1000 * 60 * 60));
        remainingMillis -= numHours * (1000 * 60 * 60);

        int numMinutes = (int)(remainingMillis / (1000 * 60));
        remainingMillis -= numMinutes * (1000 * 60);

        int numSeconds = (int)(remainingMillis / 1000);
        remainingMillis -= numSeconds * 1000;

        return $"{this.title}\n------------------------------------------------\n{this.description}\n\tTime: {((numYears > 0) ? $"{numYears} year(s), " : "")}{((numDays > 0) ? $"{numDays} day(s), " : "")}{((numHours > 0) ? $"{numHours} hour(s), " : "")}{((numMinutes > 0) ? $"{numMinutes} minute(s), " : "")}{((numSeconds > 0) ? $"{numSeconds} second(s), " : "")}{((remainingMillis > 0) ? $"{remainingMillis} millisecond(s), " : "")}";
    }
    public string getTime() {
        long remainingMillis = this.millisecondsTotal;
        int numYears = (int)(remainingMillis / (1000L * 60L * 60L * 24L * 365L));
        remainingMillis -= numYears * (1000L * 60L * 60L * 24L * 365L);

        int numDays = (int)(remainingMillis / (1000 * 60 * 24));
        remainingMillis -= numDays * (1000 * 60 * 60 * 24);

        int numHours = (int)(remainingMillis / (1000 * 60 * 60));
        remainingMillis -= numHours * (1000 * 60 * 60);

        int numMinutes = (int)(remainingMillis / (1000 * 60));
        remainingMillis -= numMinutes * (1000 * 60);

        int numSeconds = (int)(remainingMillis / 1000);
        remainingMillis -= numSeconds * 1000;

        return $"{((numYears > 0) ? $"{numYears} year(s), " : "")}{((numDays > 0) ? $"{numDays} day(s), " : "")}{((numHours > 0) ? $"{numHours} hour(s), " : "")}{((numMinutes > 0) ? $"{numMinutes} minute(s), " : "")}{((numSeconds > 0) ? $"{numSeconds} second(s), " : "")}{((remainingMillis > 0) ? $"{remainingMillis} millisecond(s), " : "")}";
    }
}