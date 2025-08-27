namespace bgtpactual.DTO.Request
{
    public class SubcribeFundRequest
    {
        public string ClientId { get; set; }
        public string FundId { get; set; }
        public decimal Amount { get; set; }
        public string ComunicationChannel { get; set; }
    }
}
