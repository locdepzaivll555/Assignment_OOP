namespace CinemaManagement.Entities
{
    public class Movie
    {
        public string Title { get; set; }       // Tên phim
        public string Director { get; set; }    // Đạo diễn
        public string Genre { get; set; }       // Thể loại
        public string Subtitle { get; set; }    // Phụ đề (VD: "Tiếng Việt", "Tiếng Anh")
        public int Duration { get; set; }       // Thời lượng (phút)
        public DateTime ReleaseDate { get; set; } // Ngày phát hành
        public DateTime ShowTime { get; set; }  // Ngày giờ chiếu phim
        public string Status { get; set; }      // Trạng thái ("Đang chiếu", "Sắp chiếu", "Ngừng chiếu")

        public Movie(string title, string director, string genre, string subtitle, int duration, DateTime releaseDate, DateTime showTime, string status)
        {
            Title = title;
            Director = director;
            Genre = genre;
            Subtitle = subtitle;
            Duration = duration;
            ReleaseDate = releaseDate;
            ShowTime = showTime;
            Status = status;
        }

        public override string ToString()
        {
            return $"🎬 {Title} | Đạo diễn: {Director} | Thể loại: {Genre} | Phụ đề: {Subtitle} | " +
                   $"Thời lượng: {Duration} phút | Phát hành: {ReleaseDate:dd/MM/yyyy} | Chiếu lúc: {ShowTime:HH:mm dd/MM/yyyy} | " +
                   $"Trạng thái: {Status}";
        }
    }
}
