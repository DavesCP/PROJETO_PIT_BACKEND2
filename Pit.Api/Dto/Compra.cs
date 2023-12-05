namespace Pit.Api.Dto
{
    public class Compra
    {
        public int Quantidade { get; set; }
        public decimal Valor { get; set; }
        public string? Endereco { get; set; }    
        public long? Telefone { get; set; }
        public string? Produto { get; set; }
    }
}
