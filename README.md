# microservices-todo-app
Aplicação baseada na arquitetura de microserviços em .NET, MongoDB, RabbitMQ, SignalR, Ocelot e Angular

## Sobre

A aplicação permite manter o controle de tarefas, tipos de tarefas, usuários e monitorar logs

## Microserviços

- Gateway `dotnet-gateway-ocelot`
- Tarefas `dotnet-todo-api`
- Tipos de tarefa `dotnet-tipotarefa-api`
- Usuários `dotnet-usuario-api`
- SignalR `dotnet-todo-signalr`

São instânciados também os containers do front-end, RabbitMQ e os banco de dados em MongoDB

As APIs de tarefa, tipos de tarefa e usuários acessam seus respectivos banco de dados, também em containers (mongodb-1, mongodb-2, mongodb-3)

O front-end comunica-se com as APIs unicamente através do Gateway e recebe notificações de log através do SignalR 

A comunicação entre os microserviços é feita através do Gateway e dos canais do RabbitMQ (consumer e producer)

## Como iniciar a aplicação

- Tenha previamente instalado o Docker em seu compudar
- Clone este repositório ```git clone https://github.com/paulotokarskiglinski/microservices-todo-app```
- Dentro da pasta do projeto, execute o comando ```docker-compose up -d --build```
- Acesse pelo seu navegador o endereço ```http://localhost:8080```

***Observação:*** Caso um ou mais containers não inicie corretamente, execute o comando ```docker-compose up -d``` ou, através do Docker Desktop, inicie os containers manualmente
