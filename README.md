# Cooperativa Digital de Credito Local

Sistema de controle para uma pequena cooperativa digital de crédito local, 
a Ada Credit (Código Bancário 777). 

Esse projeto teve como objetivo desenvolver habilidades relacionadas a 
arquitetura de software e proficiência na linguagem de programação C#.

O sistema foi desenvolvido como aplicativo console com .Net 6 & C#. A persistência
dos dados é feita através de arquivos (.csv) que são salvos no desktop (área de
trabalho) e na pasta raiz do projeto (junto ao .exe).
## Autor

O presente projeto foi desenvolvido por 
[Othon Valentim](https://github.com/thon18valentim) enquanto aluno da escola
de tecnologia Ada, no curso de C# Code@CS.
## Features

- Cadastrar Novo Cliente
- Consultar os Dados de um Cliente existente
- Alterar o Cadastro de um Cliente existente
- Desativar Cadastro de um Cliente existente
- Cadastrar Novo Funcionário
- Alterar Senha de um Funcionário existente
- Desativar Cadastro de um Funcionário existente
- Processar Transações (Reconciliação Bancária)
- Exibir Todos os Clientes Ativos com seus Respectivos Saldos
- Exibir Todos os Clientes Inativos
- Exibir Todos os Funcionários Ativos e sua Última Data e Hora de Login
- Exibir Transações com Erro (Detalhes da transação e do Erro)


## Setup

Para utilizar o projeto, basta clonar o presente repositório e executar a aplicação.

Todas as pastas utilizadas pelo projeto são criadas automaticamente, caso elas ainda
não existam em sua máquina. Vale ressaltar que as pastas de transações estarão salvas
no desktop (área de trabalho) e os arquivos (.csv) de clientes, contas e funcionários
estarão na pasta raiz do projeto (junto ao .exe).
## Transações

O sistema trabalha com arquivos de transações recebidas. Elas devem estar presentes em
"desktop/Transactions/..", onde "Pending" é a pasta que se refere a transações pendentes
de processamento, "Completed" a transações concluídas e "Failed" a transações que falharam
por algum motivo (fica descrito junto a transação).

Para que o sistema entenda o seu arquivo como uma transação pendente ele deve estar 
no seguinte formato: "nome-do-banco-parceiro-aaaammdd.csv". Cada linha se refere a uma
transação enviada pelo respectivo banco (o arquivo não deve conter cabeçalho).

Cada linha no arquivo de transações é composta pelas seguintes informações
AAA,BBBB,CCCCCC,DDD,EEEE,FFFFFF,GGG,H,I

Sendo que:
AAA Número com 3 dígitos representando o Código do Banco de Origem
BBBB Número com 4 dígitos representando a Agência do Banco de Origem
CCCCCC Número com 6 dígitos representando o número da conta do Banco de Origem

DDD Número com 3 dígitos representando o Código do Banco de Destino
EEEE Número com 4 dígitos representando a Agência do Banco de Destino
FFFFFF Número com 6 dígitos representando o número da conta do Banco de Destino

GGG Tipo da Transação (DOC, TED, TEF).

H Número representando o sentido da transação (0 - Débito/Saída, 1 - Crédito/Entrada)

I número real com duas casas decimais, separadas por um . e sem separador de milhar

Após criar o arquivo de transações e o arquitetar seguindo as regras acima o mova
para a pasta "Pending". Rode o sistema, selecione a opção "Transações" e depois
"Processar Transações (Reconciliação Bancária)". O sistema indicará no console
as transações encontradas e indicará sucesso no processamento. Então, você pode
conferir os efeitos dessas transações em "Relatórios" e 
"Exibir Todos os Clientes Ativos com seus Respectivos Saldos", você será capaz de
visualizar a mudança de saldo de seus clientes (por padrão os cliente começam com o
saldo zerado).
## Regras das Transações

- Um cliente não pode fazer transações para si mesmo (mesma conta & banco).
- O cliente deve ter saldo suficiente para transferir recursos.
- Tanto a conta de origem e destino devem estar ativas no sistema.
- TEFs só podem ser realizadas entre clientes do mesmo banco.

## Regras de Tarifas

- Todas transações de crédito são isentas
- Transações de Débito recebidas até 30/11/2022 são isentas
- Transações de Débito recebidas a partir de 01/12/2022 seguem as seguintes regras:
    - TED - Tarifa Única de R$5,00
    - DOC - Tarifa de R$1,00 + (1% da Transação limitado a R$5,00)
    - TEF - Isenta

## Crédito vs Débito

- Crédito: Conta de origem recebe o valor da conta de destino.
- Débito: Conta de origem transfere o valor para conta de destino.

## Testes

Caso queira testar o sistema, há arquivos (.csv) prontos de transações na pasta "Teste".
Basta copiar eles e colar na pasta "Pending" dentro da pasta "Transactions". Caso você
ainda não tenha a pasta "Transactions", basta rodar a aplicação uma vez que as pastas
serão criadas no seu desktop (área de trabalho).

Atenção ao utilizar os arquivos disponibilizados para teste, certifique-se que as contas
descritas nos documentos existam em sua base de dados. As contas de número "123456" se referem
a contas de outros bancos e estão acompanhadas do código do banco "123" e agência "0002", então,
você não deve alterá-las. As demais, verifique se elas existem no banco de dados, pois se referem
a contas de clientes do sistema.

Caso as operações sejam processadas com sucesso quatro arquivos de sucesso serão criados (Transactions/Completed)
e 3 de falha serão criadas (Transactions/Failed).

Mantendo o padrão dos arquivos de testes disponibilizados os clientes envolvidos terminirão com o seguinte
saldo:
- 1° R$ 1.975,00
- 2° R$ 955,50
