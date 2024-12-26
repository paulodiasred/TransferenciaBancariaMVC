using System.Collections.Generic;
using System.Linq;

namespace TransferenciaBancariaMVC.Models
{
    public static class ContaRepository
    {
        private static List<Conta> contas = new List<Conta>();

        public static List<Conta> GetContas()
        {
            if (contas.Count == 0)
            {
                
            }
            return contas;
        }

        public static void AddConta(Conta conta)
        {
            conta.Id = contas.Count + 1; // Atribui um ID incremental
            contas.Add(conta);
        }
    }
}
