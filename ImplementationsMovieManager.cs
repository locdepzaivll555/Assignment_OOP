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
            return movies ?? new List<Movie>(); // Trả về danh sách rỗng nếu `movies` bị null
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
                if (data.Length == 8)
                {
                    movies.Add(new Movie(
                        data[0], data[1], data[2], data[3],
                        int.Parse(data[4]), DateTime.Parse(data[5]), DateTime.Parse(data[6]), data[7]
                    ));
                }
            }
        }

        public void SaveMovies()
        {
            File.WriteAllLines(FilePath, movies.Select(m =>
                $"{m.Title},{m.Director},{m.Genre},{m.Subtitle},{m.Duration},{m.ReleaseDate},{m.ShowTime},{m.Status}"));
        }

        public void AddMovie(string title, string director, string genre, string subtitle, int duration, DateTime releaseDate, DateTime showTime, string status)
        {
            movies.Add(new Movie(title, director, genre, subtitle, duration, releaseDate, showTime, status));
            SaveMovies();
            Console.WriteLine("✅ Thêm phim thành công!");
        }
        public bool RemoveMovie(string title, string director, string genre)
        {
            if (movies == null || movies.Count == 0)
            {
                Console.WriteLine("⚠ Danh sách trống, không thể xóa!");
                return false;
            }

            var movie = movies.FirstOrDefault(m =>
                m.Title.Equals(title, StringComparison.OrdinalIgnoreCase) &&
                m.Director.Equals(director, StringComparison.OrdinalIgnoreCase) &&
                m.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase));

            if (movie != null)
            {
                movies.Remove(movie);
                SaveMovies();
                Console.WriteLine("✅ Xóa phim thành công!");
                return true;
            }
            else
            {
                Console.WriteLine("⚠ Không tìm thấy phim!");
                return false;
            }
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

        public Movie GetMovieByTitle(string title, string director, string genre)
        {
            return movies.FirstOrDefault(m =>
                m.Title.Equals(title, StringComparison.OrdinalIgnoreCase) &&
                m.Director.Equals(director, StringComparison.OrdinalIgnoreCase) &&
                m.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase));
        }

        public void DisplayMovies()
        {
            if (movies.Count == 0)
            {
                Console.WriteLine("⚠ Hiện không có phim nào.");
                return;
            }

            Console.WriteLine("🎬 Danh sách phim:");
            foreach (var movie in movies)
            {
                Console.WriteLine($"- {movie.Title} | {movie.Director} | {movie.Genre} | {movie.Subtitle} | {movie.Duration} phút | {movie.ReleaseDate.ToShortDateString()} | {movie.ShowTime.ToShortTimeString()} | {movie.Status}");
            }
        }

        public bool UpdateMovie(string title, string newDirector, string newGenre, string newSubtitle, int newDuration, DateTime newReleaseDate, DateTime newShowTime, string newStatus)
        {
            var movie = movies.FirstOrDefault(m => m.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
            if (movie == null)
            {
                Console.WriteLine("⚠ Không tìm thấy phim!");
                return false;
            }

            Console.Write("📢 Bạn có chắc muốn thay đổi thông tin phim này không? (y/n): ");
            string confirmation = Console.ReadLine()?.Trim().ToLower();

            while (confirmation != "y" && confirmation != "n")
            {
                Console.WriteLine("⚠ Chỉ được nhập 'y' hoặc 'n'! Vui lòng nhập lại.");
                Console.Write("📢 Bạn có chắc muốn thay đổi thông tin phim này không? (y/n): ");
                confirmation = Console.ReadLine()?.Trim().ToLower();
            }

            if (confirmation == "n")
            {
                Console.WriteLine("❌ Bạn đã hủy lưu thay đổi. Trở về màn hình chính.");
                return false;
            }

            // Cập nhật thông tin phim
            movie.Director = newDirector;
            movie.Genre = newGenre;
            movie.Subtitle = newSubtitle;
            movie.Duration = newDuration;
            movie.ReleaseDate = newReleaseDate;
            movie.ShowTime = newShowTime;
            movie.Status = newStatus;

            SaveMovies();
            Console.WriteLine("✅ Cập nhật thông tin phim thành công!");
            return true;
        }
    }
}
