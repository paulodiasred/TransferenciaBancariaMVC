using Microsoft.AspNetCore.Mvc;
using TransferenciaBancariaMVC.Models;
using X.PagedList;

namespace TransferenciaBancariaMVC.Controllers
{
    public class ContaController : Controller
    {
        private static List<Conta> _contas = new List<Conta>(); // Lista de contas simulando um banco de dados.

        public IActionResult Index(int? page)
        {
            int pageSize = 5; // Número de itens por página
            int pageNumber = page ?? 1; // Página atual
            int totalItems = ContaRepository.GetContas().Count; // Total de itens
            var contas = ContaRepository.GetContas()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            return View(contas);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Conta conta)
        {
            if (ModelState.IsValid)
            {
                ContaRepository.AddConta(conta); // Adiciona a conta ao repositório em memória
                return RedirectToAction(nameof(Index));
            }

            // Se algo estiver errado, retorna a mesma view com os erros.
            return View(conta);
        }

        // Adicionando métodos para transferir
        [HttpGet]
        public IActionResult Transferir()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Transferir(TransferenciaViewModel transferencia)
        {
            var contaOrigem = ContaRepository.GetContas().FirstOrDefault(c => c.Id == transferencia.ContaOrigem);
            var contaDestino = ContaRepository.GetContas().FirstOrDefault(c => c.Id == transferencia.ContaDestino);

            if (contaOrigem == null || contaDestino == null)
            {
                ModelState.AddModelError("", "Conta origem ou destino não encontrada.");
                return View(transferencia);
            }

            if (contaOrigem.Saldo < transferencia.Valor)
            {
                ModelState.AddModelError("", "Saldo insuficiente na conta de origem.");
                return View(transferencia);
            }

            // Atualizar saldos
            contaOrigem.Saldo -= transferencia.Valor;
            contaDestino.Saldo += transferencia.Valor;

            return RedirectToAction("Index");
        }


        // Adicionando método para depositar
        [HttpGet]
        public IActionResult Depositar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Depositar(DepositoViewModel deposito)
        {
            var conta = ContaRepository.GetContas().FirstOrDefault(c => c.Id == deposito.ContaId);

            if (conta == null)
            {
                ModelState.AddModelError("", "Conta não encontrada.");
                return View(deposito);
            }

            conta.Saldo += deposito.Valor;

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Sacar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Sacar(SaqueViewModel saque)
        {
            var conta = ContaRepository.GetContas().FirstOrDefault(c => c.Id == saque.ContaId);

            if (conta == null)
            {
                ModelState.AddModelError("", "Conta não encontrada.");
                return View(saque);
            }

            if (conta.Saldo < saque.Valor)
            {
                ModelState.AddModelError("", "Saldo insuficiente.");
                return View(saque);
            }

            conta.Saldo -= saque.Valor;

            return RedirectToAction("Index");
        }

    }
}
