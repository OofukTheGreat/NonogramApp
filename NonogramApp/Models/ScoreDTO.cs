namespace NonogramApp.Models
{
    public class ScoreDTO
    {
        public int PlayerId { get; set; }

        public int LevelId { get; set; }

        public string? CurrentProgress { get; set; }

        public int Time { get; set; }
        public bool HasWon { get; set; }
        public ScoreDTO() { }
        public ScoreDTO(int playerid, int levelid, string currentprogress, int time, bool haswon)
        {
            this.PlayerId = playerid;
            this.LevelId = levelid;
            this.CurrentProgress = currentprogress;
            this.Time = time;
            this.HasWon = haswon;
        }
    }
}
