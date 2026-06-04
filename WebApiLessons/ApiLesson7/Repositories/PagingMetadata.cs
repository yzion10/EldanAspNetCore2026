namespace ApiLesson7.Repositories
{
    /// <summary>
    /// מחלקה שמייצגת את המידע על הדף הנוכחי
    /// מספר הדפים הכולל, מספר הפריטים הכולל וכו' - מידע שיכול להיות מועיל ללקוח כדי להבין את ההקשר של התוצאות שהוא מקבל
    /// </summary>
    public class PagingMetadata
    {
        public int TotalItemCount { get; set; }
        public int TotalPageCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }

        public PagingMetadata(int totalItemCount, int pageSize, int currentPage)
        {
            TotalItemCount = totalItemCount;
            PageSize = pageSize;
            CurrentPage = currentPage;
            TotalPageCount = (int)Math.Ceiling(totalItemCount / (double)pageSize);
        }
    }
}
