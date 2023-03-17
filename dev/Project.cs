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
    public string getTime(long a_baseMillis = -1, uint a_additionalTime = 0) {
        if(a_baseMillis == -1) a_baseMillis = this.millisecondsTotal;
        long remainingMillis = a_baseMillis + a_additionalTime;

        uint numYears = (uint)System.Math.Floor((double)(remainingMillis / (1000L * 60L * 60L * 24L * 365L)));
        remainingMillis -= numYears * 1000 * 60 * 60 * 24 * 365;

        uint numDays = (uint)System.Math.Floor((double)(remainingMillis / (1000 * 60 * 60 * 24)));
        remainingMillis -= numDays * 1000 * 60 * 60 * 24;

        uint numHours = (uint)System.Math.Floor((double)(remainingMillis / (1000 * 60 * 60)));
        remainingMillis -= numHours * 1000 * 60 * 60;

        uint numMinutes = (uint)System.Math.Floor((double)(remainingMillis / (1000 * 60)));
        remainingMillis -= numMinutes * 1000 * 60;

        uint numSeconds = (uint)System.Math.Floor((double)(remainingMillis / 1000));
        remainingMillis -= numSeconds * 1000;

        string displayMin = (numMinutes > 0) ? (numMinutes < 10) ? numMinutes.ToString().Insert(0, "0") : numMinutes.ToString() : "00";
        string displaySec = (numSeconds > 0) ? (numSeconds < 10) ? numSeconds.ToString().Insert(0, "0") : numSeconds.ToString() : "00";
        return $"{((numYears > 0) ? $"{numYears} year(s), " : "")}{((numDays > 0) ? $"{numDays} day(s), " : "")}{((numHours > 0) ? $"{numHours}:" : "00")}:{displayMin}:{displaySec}";
    }
}