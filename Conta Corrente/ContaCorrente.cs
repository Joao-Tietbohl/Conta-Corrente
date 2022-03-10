using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Conta_Corrente
{
    public enum TipoMovimentacao
    {
        Credito, Debito
    }

    class Movimentacao
    {
        public float valor;
        public TipoMovimentacao tipo;
    }

    class ContaCorrente
    {
        public string numeroConta;
        public float saldo;
        public bool ehEspecial;
        public float limite;
        public Movimentacao[] movimentacoes;
        public float limiteUsado;




        public void Sacar(float valorSaque)
        {
            //Confere se valor é válido
            if (valorSaque < saldo + limite)
            {
                if (valorSaque > saldo)
                {
                    //atualiza limite e limite usado caso necessario
                    limite = limite - (valorSaque - saldo);
                    limiteUsado = valorSaque - saldo;
                }

                saldo = saldo - valorSaque;

                CriaMovimento(valorSaque, 1);
            }
            else
                Console.WriteLine("Valor ultrapassa limite de saque");
        }

        private void CriaMovimento(float valor, int operacao)
        {
            switch (operacao)
            {

                case 1:
                    Movimentacao novaMovimentacaoDebito = new Movimentacao();
                    novaMovimentacaoDebito.valor = valor;
                    novaMovimentacaoDebito.tipo = TipoMovimentacao.Debito;

                    AdicionaMovimento(novaMovimentacaoDebito);
                    break;

                case 2:
                    Movimentacao novaMovimentacaoCredito = new Movimentacao();
                    novaMovimentacaoCredito.valor = valor;
                    novaMovimentacaoCredito.tipo = TipoMovimentacao.Credito;

                    AdicionaMovimento(novaMovimentacaoCredito);
                    break;


            }
        }

        private void AdicionaMovimento(Movimentacao novaMovimentacao)
        {
            for (int i = 0; i < 10; i++)
            {
                if (movimentacoes[i] == null)
                {
                    movimentacoes[i] = novaMovimentacao;
                    break;
                }
            }
        }

        public void Depositar(float valorDeposito)
        {
            //LimiteUsado2 para possibilitar usar LimiteUsado nos if's 
            //valorDeposito2 = valor do deposito menos a parte negativa do saldo, util para atualizar o saldo
            float limiteUsado2 = limiteUsado;
            float valorDeposito2 = 0;
            if (limiteUsado != 0)
            {
                if (valorDeposito > limiteUsado)
                {
                    saldo = limiteUsado2 + saldo;
                    valorDeposito2 = valorDeposito - limiteUsado2;

                    limite = limiteUsado2 + limite;
                    limiteUsado2 = 0;

                    saldo = valorDeposito2 + saldo;

                    CriaMovimento(valorDeposito, 2);
                }

                if (valorDeposito < limiteUsado)
                {
                    //se valor do deposito é menor do que o limite usado, significa que o saldo esta negativo
                    limiteUsado2 = limiteUsado2 + valorDeposito;
                    saldo = saldo + valorDeposito;

                    CriaMovimento(valorDeposito, 2);
                }
            }

            if (limiteUsado == 0)
            {
                saldo = valorDeposito + saldo;

                CriaMovimento(valorDeposito, 2);
            }

            limiteUsado = limiteUsado2;
        }

        public void EmitirSaldo()
        {
            Console.WriteLine();
            Console.WriteLine("Conta de número: " + numeroConta);
            Console.WriteLine("Saldo: " + saldo);
            Console.WriteLine("Limite: " + limite);
        }

        public void Extrato()
        {
            Console.WriteLine();
            Console.WriteLine("Extrato conta " + numeroConta);
            foreach (Movimentacao item in movimentacoes)
            {
                if (item != null)
                {
                    Console.WriteLine();
                    Console.WriteLine(item.tipo);
                    Console.WriteLine(item.valor);
                }
            }
        }

        public void TransferenciaEntreContas(ContaCorrente contaReceptora, float valorTransferencia)
        {
            if (valorTransferencia <= saldo + limite)
            {
                //Parte do transferidor

                if (valorTransferencia > saldo)
                {
                    limite = limite - (valorTransferencia - saldo);
                    limiteUsado = valorTransferencia - saldo;
                }

                saldo = saldo - valorTransferencia;

                CriaMovimento(valorTransferencia, 1);

                //Parte do receptor
                

                //caso saldo positivo
                if (contaReceptora.saldo >= 0 && contaReceptora.limiteUsado == 0)
                {
                    contaReceptora.saldo = contaReceptora.saldo + valorTransferencia;

                    Movimentacao novaMovimentacaoCreditoContaReceptora = new Movimentacao();
                    novaMovimentacaoCreditoContaReceptora.valor = valorTransferencia;
                    novaMovimentacaoCreditoContaReceptora.tipo = TipoMovimentacao.Credito;

                    for (int i = 0; i < 10; i++)
                    {
                        if (contaReceptora.movimentacoes[i] == null)
                        {
                            contaReceptora.movimentacoes[i] = novaMovimentacaoCreditoContaReceptora;
                            break;
                        }
                    }

                }

                //caso saldo negativo
                float valorTransferencia2 = 0;
                if (contaReceptora.saldo < 0 && contaReceptora.limiteUsado != 0)
                {
                    if (valorTransferencia > contaReceptora.limiteUsado)
                    {
                        contaReceptora.saldo = contaReceptora.limiteUsado + contaReceptora.saldo;
                        valorTransferencia2 = valorTransferencia - contaReceptora.limiteUsado;
                        contaReceptora.saldo = valorTransferencia2 + contaReceptora.saldo;

                        contaReceptora.limite = contaReceptora.limiteUsado + contaReceptora.limite;
                        contaReceptora.limiteUsado = 0;

                        
                        Movimentacao novaMovimentacaoCreditoContaReceptora = new Movimentacao();
                        novaMovimentacaoCreditoContaReceptora.valor = valorTransferencia;
                        novaMovimentacaoCreditoContaReceptora.tipo = TipoMovimentacao.Credito;

                        for (int i = 0; i < 10; i++)
                        {
                            if (contaReceptora.movimentacoes[i] == null)
                            {
                                contaReceptora.movimentacoes[i] = novaMovimentacaoCreditoContaReceptora;
                                break;
                            }

                        }
                    }

                    if (valorTransferencia < contaReceptora.limiteUsado)
                    {

                        //se valor do deposito é menor do que o limite usado, significa que o saldo esta negativo
                        contaReceptora.limiteUsado = contaReceptora.limiteUsado + valorTransferencia;
                        contaReceptora.saldo = contaReceptora.saldo + valorTransferencia;

                        Movimentacao novaMovimentacaoCreditoContaReceptora = new Movimentacao();
                        novaMovimentacaoCreditoContaReceptora.valor = valorTransferencia;
                        novaMovimentacaoCreditoContaReceptora.tipo = TipoMovimentacao.Credito;
                        for (int i = 0; i < 10; i++)
                        {
                            if (contaReceptora.movimentacoes[i] == null)
                            {
                                contaReceptora.movimentacoes[i] = novaMovimentacaoCreditoContaReceptora;
                                break;
                            }

                        }
                    }
                }
            }
            else
                Console.WriteLine("Valor ultrapassa limite de transferencia.");


            


        }
    }

}
