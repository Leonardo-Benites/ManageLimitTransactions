Sistema de transação de Pagamentos

Descrição

Este sistema de transações via pix permite a criação de uma conta e limite pix, ajuste do limite pix, transferência e remoção da conta.

Tecnologias Utilizadas

Back-end: .NET 8.0; Banco de Dados: DynamoDB

Arquitetura: Segregação por responsabilidades: Application: Contém a regra de negócio Domain: Modelos e entidades WebAPI: Endpoints da aplicação Infrastructure: Persistência no banco de dados e configuração das tabelas

Funcionalidades

1- Cadastro de informações referentes à gestão de limite.
2- Buscar as informações de limite para uma conta já cadastrada. 
3- Alterar o limite para transações PIX de uma conta já cadastrada.
4- Remover um registro de informações de conta e limite.
5- Transação de PIX

Como configurar o projeto:
Para executar este projeto somente será necessário a configuração do banco de dados DynamoDb Local.

Passo 1: AWS CLI 
-Instale o AWS CLI https://aws.amazon.com/pt/cli/

Passo 2: AWS Configure
Abra o CMD e rode o comando "aws configure"
Irá solicitar Access Key e Secret Key que você pode encontrar dentro do IAM da sua conta.
Informe também a região "sa-eat-1" e format "json"

Passo 3: (DynamoDB)
Baixe o DynamoDb local: https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/DynamoDBLocal.DownloadingAndRunning.html
Crie uma pasta e extraia os arquivos para a mesma.
Abra o cmd dentro da pasta com os arquivos do DynamoDB e execute o seguinte comando "java -Djava.library.path=./DynamoDBLocal_lib -jar DynamoDBLocal.jar -sharedDb"
Verifique a porta e confirme que será localhost:8000 (meu caso) em que o DynamoDB estará rodando que será informada no CMD após o comando. 

PASSO 3 B:(CASO O DYNAMODB SEJA EXECUTADO EM OUTRA PORTA)
Acesso o código fonte, no arquivo Program, busque por AmazonDynamoDBConfig e altere a URL para a correta.

Passo 4: (Criar tabela)
Execute o comando  "aws dynamodb create-table --table-name ClientAccount --attribute-definitions AttributeName=accountNumber,AttributeType=N --key-schema AttributeName=accountNumber,KeyType=HASH --provisioned-throughput ReadCapacityUnits=5,WriteCapacityUnits=5 --endpoint-url http://localhost:8000 --region sa-east-1"
Verifique se funcionou: "aws dynamodb list-tables --endpoint-url http://localhost:8000" - Irá listar a tabela que você acabou de criar.

Agora você poderá executar as funções conforme o esperado. 
Obrigado!
