namespace WebAPI_ExceptionHandling_Net6.Models
{
    public partial class Log
    {
        public int LogId { get; set; }
        public string Source { get; set; }
        public string FilePath { get; set; }
        public int LineNumber { get; set; }
        public string ExceptionMessage { get; set; }
        public DateTime CretedDate { get; set; }
    }
}
