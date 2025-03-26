using System;
using System.Collections.Generic;

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
        public Dictionary<DateTime, List<(DateTime Start, DateTime End)>> Showtimes { get; set; } // Lưu suất chiếu theo ngày
        public string Status { get; set; }      // Trạng thái ("Đang chiếu", "Sắp chiếu", "Ngừng chiếu")

        // Constructor mới
        public Movie(string title, string director, string genre, string subtitle, int duration,
                     DateTime releaseDate, Dictionary<DateTime, List<(DateTime Start, DateTime End)>> showtimes, string status)
        {
            Title = title;
            Director = director;
            Genre = genre;
            Subtitle = subtitle;
            Duration = duration;
            ReleaseDate = releaseDate;
            Showtimes = showtimes ?? new Dictionary<DateTime, List<(DateTime, DateTime)>>();
            Status = status;
        }

        public override string ToString()
        {
            string showtimeStr = Showtimes.Count > 0
                ? string.Join("\n", Showtimes.Select(day =>
                    $"📅 {day.Key:dd/MM/yyyy}: " + string.Join(", ", day.Value.Select(s => $"{s.Start:HH:mm} - {s.End:HH:mm}"))))
                : "Chưa có suất chiếu";

            return $"🎬 {Title} | Đạo diễn: {Director} | Thể loại: {Genre} | Phụ đề: {Subtitle} | " +
                   $"Thời lượng: {Duration} phút | Phát hành: {ReleaseDate:dd/MM/yyyy} | " +
                   $"Trạng thái: {Status}\nSuất chiếu:\n{showtimeStr}";
        }
    }
}


