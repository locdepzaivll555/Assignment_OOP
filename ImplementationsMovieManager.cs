using addaccount.Interface;
using CinemaManagement.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CinemaManagement.Implementations
{
    public class MovieManager : IMovieManager
    {
        private const string FilePath = "film.txt";
        private List<Movie> movies = new List<Movie>();
        public List<Movie> GetAllMovies()
        {
            return movies ?? new List<Movie>(); // Trả về danh sách rỗng nếu movies bị null
        }

        public MovieManager()
        {
            LoadMovies();
        }

        private void LoadMovies()
        {
            if (!File.Exists(FilePath)) return;

            string[] lines = File.ReadAllLines(FilePath);
            foreach (var line in lines)
            {
                string[] data = line.Split(',');
                if (data.Length == 8) // Kiểm tra đúng số lượng trường
                {
                    DateTime releaseDate = DateTime.ParseExact(data[5], "dd/MM/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture);

                    // ✅ Tạo Dictionary<DateTime, List<(DateTime Start, DateTime End)>> để lưu suất chiếu theo ngày
                    Dictionary<DateTime, List<(DateTime Start, DateTime End)>> showtimesByDate = new();

                    // ✅ Xử lý danh sách suất chiếu theo từng ngày
                    string[] dateShowtimes = data[6].Split(';'); // Tách các ngày chiếu bằng dấu ";"

                    foreach (var dateEntry in dateShowtimes)
                    {
                        string[] dateAndShowtimes = dateEntry.Split('|'); // Tách ngày chiếu và suất chiếu
                        if (dateAndShowtimes.Length == 2)
                        {
                            if (DateTime.TryParseExact(dateAndShowtimes[0], "dd/MM/yyyy",
                                System.Globalization.CultureInfo.InvariantCulture,
                                System.Globalization.DateTimeStyles.None, out DateTime showDate))
                            {
                                List<(DateTime Start, DateTime End)> sessions = new List<(DateTime Start, DateTime End)>();
                                string[] sessionsArray = dateAndShowtimes[1].Split(','); // Các suất chiếu cách nhau bằng dấu ","

                                foreach (var session in sessionsArray)
                                {
                                    string[] times = session.Split('-'); // Tách giờ chiếu bằng dấu "-"
                                    if (times.Length == 2 &&
                                        DateTime.TryParseExact(times[0], "HH:mm",
                                            System.Globalization.CultureInfo.InvariantCulture,
                                            System.Globalization.DateTimeStyles.None, out DateTime startTime) &&
                                        DateTime.TryParseExact(times[1], "HH:mm",
                                            System.Globalization.CultureInfo.InvariantCulture,
                                            System.Globalization.DateTimeStyles.None, out DateTime endTime))
                                    {
                                        sessions.Add((startTime, endTime));
                                    }
                                    else
                                    {
                                        Console.WriteLine($"❌ Lỗi khi parse suất chiếu: {session}");
                                    }
                                }

                                showtimesByDate[showDate] = sessions;
                            }
                        }
                    }

                    // ✅ Chuyển thể loại từ chuỗi về danh sách
                    string genre = data[2].Replace("|", ", ");

                    movies.Add(new Movie(
                        data[0], data[1], genre, data[3],
                        int.Parse(data[4]),
                        releaseDate,
                        showtimesByDate, // ✅ Lưu suất chiếu theo ngày
                        data[7]    // Trạng thái
                    ));
                }
            }
        }
        public void SaveMovies()
        {
            File.WriteAllLines(FilePath, movies.Select(m =>
            {
                // ✅ Kiểm tra nếu Showtimes rỗng
                string showtimesString = m.Showtimes.Any()
                    ? string.Join(";", m.Showtimes.Select(kvp =>
                        $"{kvp.Key:dd/MM/yyyy}|{string.Join(",", kvp.Value.Select(s => $"{s.Start:HH:mm}-{s.End:HH:mm}"))}"
                    ))
                    : "NoShowtimes";

                return $"{m.Title},{m.Director},{m.Genre.Replace(",", "|")},{m.Subtitle},{m.Duration},{m.ReleaseDate:dd/MM/yyyy}," +
                       $"{showtimesString},{m.Status}";
            }));
        }


        public void AddMovie(string title, string director, string genre, string subtitle, int duration,
                      DateTime releaseDate, Dictionary<DateTime, List<(DateTime Start, DateTime End)>> showtimesByDate, string status)
        {
            movies.Add(new Movie(title, director, genre, subtitle, duration, releaseDate, showtimesByDate, status));
            SaveMovies();
            Console.WriteLine("✅ Thêm phim thành công!");
        }
        public bool RemoveMovie(string title, string director, string genreInput)
        {
            if (movies == null || movies.Count == 0)
            {
                Console.WriteLine("⚠ Danh sách trống, không thể xóa!");
                return false;
            }

            // Tách thể loại nhập vào thành danh sách
            List<string> inputGenres = genreInput.Split(',').Select(g => g.Trim()).ToList();

            // Lấy tất cả phim có cùng title & director
            var matchedMovies = movies
                .Where(m => m.Title.Equals(title, StringComparison.OrdinalIgnoreCase) &&
                            m.Director.Equals(director, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (matchedMovies.Count == 0)
            {
                Console.WriteLine("⚠ Không tìm thấy phim nào!");
                return false;
            }

            bool found = false;

            foreach (var movie in matchedMovies)
            {
                // Tách thể loại của phim thành danh sách
                List<string> movieGenres = movie.Genre.Split(',').Select(g => g.Trim()).ToList();

                // Kiểm tra nếu thể loại nhập vào khớp 100% với thể loại của phim
                if (movieGenres.OrderBy(x => x).SequenceEqual(inputGenres.OrderBy(x => x)))
                {
                    found = true;
                    Console.WriteLine($"⚠ Bạn có chắc chắn muốn xóa phim '{movie.Title}' ({movie.Director}) với thể loại: {movie.Genre}? (y/n)");
                    string confirm = Console.ReadLine().Trim().ToLower();

                    if (confirm == "y")
                    {
                        movies.Remove(movie);
                        SaveMovies();
                        Console.WriteLine("✅ Phim đã bị xóa!");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("⏳ Hủy thao tác xóa.");
                        return false;
                    }
                }
            }

            if (!found)
            {
                Console.WriteLine("⚠ Không có phim nào khớp hoàn toàn với thể loại bạn nhập!");
            }

            return false;
        }

        public void RemoveAllMovies()
        {
            if (movies == null || movies.Count == 0)
            {
                Console.WriteLine("⚠ Danh sách trống, không có gì để xóa!");
                return;
            }

            movies.Clear();
            SaveMovies();
            Console.WriteLine("✅ Đã xóa toàn bộ phim!");
        }
        public Movie GetMovieByTitle(string title, string director, string genreInput)
        {
            // Chuyển thể loại nhập vào thành danh sách
            List<string> inputGenres = genreInput.Split(',').Select(g => g.Trim()).ToList();

            return movies.FirstOrDefault(m =>
                m.Title.Equals(title, StringComparison.OrdinalIgnoreCase) &&
                m.Director.Equals(director, StringComparison.OrdinalIgnoreCase) &&
                m.Genre.Split(',').Select(g => g.Trim()).OrderBy(x => x).SequenceEqual(inputGenres.OrderBy(x => x)) // Kiểm tra khớp 100%
            );
        }

        public void DisplayMovies()
        {
            if (movies.Count == 0)
            {
                Console.WriteLine("⚠ Hiện không có phim nào.");
                return;
            }

            Console.WriteLine("\n🎬 Danh sách phim:");
            Console.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("STT | Tên phim         | Đạo diễn    | Thể loại        | Phụ đề     | Thời lượng | Ngày công chiếu     | Ngày chiếu           | Giờ chiếu                   | Trạng thái ");
            Console.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------");

            int index = 1;
            foreach (var movie in movies)
            {
                string releaseDay = movie.ReleaseDate.ToString("dddd, dd/MM/yyyy", new System.Globalization.CultureInfo("vi-VN"));
                string[] genres = movie.Genre.Split(',').Select(g => g.Trim()).ToArray();
                bool firstRow = true;

                // ❗ Dùng Showtimes thay vì ShowtimesByDate
                foreach (var showtimeEntry in movie.Showtimes)
                {
                    string showtimeDate = showtimeEntry.Key.ToString("dddd, dd/MM/yyyy", new System.Globalization.CultureInfo("vi-VN"));
                    List<string> showtimeList = showtimeEntry.Value.Select(s => $"{s.Start:HH:mm} - {s.End:HH:mm}").ToList();
                    string showtimeStr = string.Join(", ", showtimeList);

                    if (firstRow)
                    {
                        Console.WriteLine($"{index,-4}| {movie.Title,-16} | {movie.Director,-11} | {genres[0],-15} | {movie.Subtitle,-10} | {movie.Duration,-10} | {releaseDay,-18} | {showtimeDate,-20} | {showtimeStr,-25} | {movie.Status}");
                        firstRow = false;
                    }
                    else
                    {
                        Console.WriteLine($"    | {"".PadRight(16)} | {"".PadRight(11)} | {"".PadRight(15)} | {"".PadRight(10)} | {"".PadRight(10)} | {"".PadRight(18)} | {showtimeDate,-20} | {showtimeStr,-25} |");
                    }
                }

                for (int i = 1; i < genres.Length; i++)
                {
                    Console.WriteLine($"    | {"".PadRight(16)} | {"".PadRight(11)} | {genres[i],-15} | {"".PadRight(10)} | {"".PadRight(10)} | {"".PadRight(18)} | {"".PadRight(25)} | {"".PadRight(25)} |");
                }

                Console.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------");
                index++;
            }

            Console.WriteLine();
        }

        public Movie GetMovieByTitle(string title, string director, List<string> genres)
        {
            return movies.FirstOrDefault(m =>
                m.Title.Equals(title, StringComparison.OrdinalIgnoreCase) &&
                m.Director.Equals(director, StringComparison.OrdinalIgnoreCase) &&
                genres.All(g => m.Genre.Split(',').Select(x => x.Trim()).Contains(g, StringComparer.OrdinalIgnoreCase))
            );
        }


        public bool AddShowtime(string title, DateTime date, (DateTime Start, DateTime End) showtime)
        {
            var movie = movies.FirstOrDefault(m => m.Title == title);
            if (movie == null)
                return false; // Phim không tồn tại

            if (!movie.Showtimes.ContainsKey(date)) // 🔥 Sửa ShowtimesByDate -> Showtimes
            {
                movie.Showtimes[date] = new List<(DateTime Start, DateTime End)>();
            }

            // Kiểm tra xem suất chiếu có bị trùng không
            if (movie.Showtimes[date].Any(s => s.Start < showtime.End && s.End > showtime.Start)) // 🔥 Sửa ShowtimesByDate -> Showtimes
                return false; // Bị trùng suất chiếu

            movie.Showtimes[date].Add(showtime); // 🔥 Sửa ShowtimesByDate -> Showtimes
            return true; // Thêm thành công
        }


        public bool RemoveShowtime(string title, DateTime date, (DateTime Start, DateTime End) showtime)
        {
            var movie = movies.FirstOrDefault(m => m.Title == title);
            if (movie == null || !movie.Showtimes.ContainsKey(date))
                return false; // Phim không tồn tại hoặc không có suất chiếu vào ngày này

            // Xóa suất chiếu nếu tồn tại
            bool removed = movie.Showtimes[date].Remove(showtime);

            // Nếu ngày đó không còn suất chiếu nào, xóa khỏi danh sách
            if (movie.Showtimes[date].Count == 0)
                movie.Showtimes.Remove(date);

            return removed;
        }

        public bool UpdateMovie(string title, string director, List<string> genres,
            string newDirector, List<string> newGenres,
            string newSubtitle, int newDuration, DateTime newReleaseDate,
            Dictionary<DateTime, List<(DateTime Start, DateTime End)>> newShowtimes, string newStatus)
        {
            var movie = GetMovieByTitle(title, director, genres);
            if (movie == null)
            {
                Console.WriteLine("⚠ Không tìm thấy phim!");
                return false;
            }

            Console.WriteLine("\n🔍 Thông tin phim hiện tại:");
            Console.WriteLine($"🎬 Tiêu đề: {movie.Title}");
            Console.WriteLine($"🎬 Đạo diễn: {movie.Director}");
            Console.WriteLine($"🎬 Thể loại: {movie.Genre}");
            Console.WriteLine($"🎬 Phụ đề: {movie.Subtitle}");
            Console.WriteLine($"🎬 Thời lượng: {movie.Duration} phút");
            Console.WriteLine($"🎬 Ngày phát hành: {movie.ReleaseDate.ToString("dddd, dd/MM/yyyy", new System.Globalization.CultureInfo("vi-VN"))}");
            Console.WriteLine($"🎬 Suất chiếu hiện tại:");

            foreach (var date in movie.Showtimes.Keys)
            {
                string showtimeDate = date.ToString("dddd, dd/MM/yyyy", new System.Globalization.CultureInfo("vi-VN"));
                string showtimesStr = string.Join(", ", movie.Showtimes[date].Select(s => $"{s.Start:HH:mm}-{s.End:HH:mm}"));
                Console.WriteLine($"   📅 {showtimeDate}: {showtimesStr}");
            }

            Console.WriteLine($"🎬 Trạng thái: {movie.Status}");

            Console.WriteLine("\n🔄 Thông tin phim mới:");
            Console.WriteLine($"🎬 Đạo diễn: {newDirector}");
            Console.WriteLine($"🎬 Thể loại: {string.Join(", ", newGenres)}");
            Console.WriteLine($"🎬 Phụ đề: {newSubtitle}");
            Console.WriteLine($"🎬 Thời lượng: {newDuration} phút");
            Console.WriteLine($"🎬 Ngày phát hành: {newReleaseDate.ToString("dddd, dd/MM/yyyy", new System.Globalization.CultureInfo("vi-VN"))}");
            Console.WriteLine($"🎬 Suất chiếu mới:");

            // Chuyển đổi danh sách suất chiếu mới thành Dictionary<DateTime, List<(Start, End)>>
            var newShowtimesDict = new Dictionary<DateTime, List<(DateTime Start, DateTime End)>>();
            foreach (var kvp in newShowtimes) // Duyệt từng cặp ngày chiếu -> danh sách suất chiếu
            {
                DateTime date = kvp.Key; // Lấy ngày chiếu
                if (!newShowtimesDict.ContainsKey(date))
                {
                    newShowtimesDict[date] = new List<(DateTime Start, DateTime End)>();
                }

                foreach (var showtime in kvp.Value) // Duyệt từng suất chiếu trong danh sách
                {
                    newShowtimesDict[date].Add(showtime); // ✅ Thêm đúng kiểu (Start, End)
                }
            }


            foreach (var date in newShowtimesDict.Keys)
            {
                string showtimeDate = date.ToString("dddd, dd/MM/yyyy", new System.Globalization.CultureInfo("vi-VN"));
                string showtimesStr = string.Join(", ", newShowtimesDict[date].Select(s => $"{s.Start:HH:mm}-{s.End:HH:mm}"));
                Console.WriteLine($"   📅 {showtimeDate}: {showtimesStr}");
            }

            Console.WriteLine($"🎬 Trạng thái: {newStatus}");

            Console.Write("\n📢 Bạn có chắc muốn cập nhật thông tin này không? (y/n): ");
            string confirmation = Console.ReadLine()?.Trim().ToLower();

            while (confirmation != "y" && confirmation != "n")
            {
                Console.WriteLine("⚠ Chỉ được nhập 'y' hoặc 'n'! Vui lòng nhập lại.");
                Console.Write("📢 Bạn có chắc muốn cập nhật thông tin này không? (y/n): ");
                confirmation = Console.ReadLine()?.Trim().ToLower();
            }

            if (confirmation == "n")
            {
                Console.WriteLine("❌ Bạn đã hủy cập nhật. Trở về màn hình chính.");
                return false;
            }

            // Cập nhật thông tin phim
            movie.Director = newDirector;
            movie.Genre = string.Join(", ", newGenres);
            movie.Subtitle = newSubtitle;
            movie.Duration = newDuration;
            movie.ReleaseDate = newReleaseDate;
            movie.Showtimes = newShowtimesDict; // Gán Dictionary mới
            movie.Status = newStatus;

            SaveMovies();
            Console.WriteLine("✅ Cập nhật thông tin phim thành công!");

            // Hiển thị lại danh sách phim sau khi cập nhật
            DisplayMovies();

            return true;
        }
    }
}
