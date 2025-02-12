namespace NonogramApp.Models
{
    public class LevelDTO
    {
        public int LevelId { get; set; }
        public string Title { get; set; }
        public string Layout { get; set; }
        public int CreatorId { get; set; }
        public int StatusId { get; set; }
        public int Size { get; set; }
        public LevelDTO() { }
        public LevelDTO(int levelId, string title, string layout, int size, int creatorId, int statusId)
        {
            this.LevelId = levelId;
            this.Title = title;
            this.Layout = layout;
            this.Size = size;
            this.CreatorId = creatorId;
            this.StatusId = statusId;
        }
    }
}