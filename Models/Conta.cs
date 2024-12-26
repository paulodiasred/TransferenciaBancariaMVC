using System;

namespace TransferenciaBancariaMVC.Models
{
    public class Conta
    {
        public int Id { get; set; }
        public TipoConta TipoConta { get; set; }
        public double Saldo { get; set; }
        public string Nome { get; set; }

        public override string ToString()
        {
            return $"TipoConta: {TipoConta}, Nome: {Nome}, Saldo: {Saldo}";
        }

    }
}
