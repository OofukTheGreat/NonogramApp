namespace NonogramApp.Models
{
    public class ScoreDTO
    {
        public int PlayerId { get; set; }

        public int LevelId { get; set; }

        public string? CurrentProgress { get; set; }

        private int time;
        public int Time
        {
            get
            {
                return time;
            }
            set
            {
                time = value;
                Seconds = time % 60;
                Minutes = (time / 60) % 60;
                Hours = time / 3600;
            }       
        }   
        public int Seconds { get; set; }
        public int Minutes { get; set; }
        public int Hours { get; set; }
        public bool HasWon { get; set; }
        public ScoreDTO() { }
        public ScoreDTO(int playerid, int levelid, string currentprogress, int time, bool haswon)
        {
            this.PlayerId = playerid;
            this.LevelId = levelid;
            this.CurrentProgress = currentprogress;
            this.Time = time;
            this.Seconds = time % 60;
            this.Minutes = time / 60 % 60;
            this.Hours = time / 3600;
            this.HasWon = haswon;
        }
    }
}
