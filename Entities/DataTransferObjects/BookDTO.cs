namespace Entities.DataTransferObjects
{
    //[Serializable]
    public record BookDTO
    {
        public int Id { get; init; } // init -> set değeri readonly olmasını sağlıyor.
        public string Title { get; init; }
        public decimal Price { get; init; }
    }
}