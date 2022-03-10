using System;

namespace Conta_Corrente
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ContaCorrente conta1 = new ContaCorrente();
            conta1.numeroConta = "123456";
            conta1.ehEspecial = false;
            conta1.limite = 100;
            conta1.saldo = 200;
            conta1.movimentacoes = new Movimentacao[10];

            conta1.Sacar(250);
            conta1.Depositar(100);

            ContaCorrente conta2 = new ContaCorrente();
            conta2.numeroConta = "987654";
            conta2.ehEspecial = true;
            conta2.limite = 100;
            conta2.saldo = 300;
            conta2.movimentacoes = new Movimentacao[10];

            conta2.TransferenciaEntreContas(conta1, 350);
            conta1.TransferenciaEntreContas(conta2, 600);
            conta1.TransferenciaEntreContas(conta2, 450);
            conta1.EmitirSaldo();

            conta1.Extrato();
            conta2.Extrato();
        }
    }
}
