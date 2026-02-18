# Lab.SendEmail

Esta aplicação realiza envio de e-mails utilizando a biblioteca MailKit. Para desenvolvimento e testes, ela oferece um
ambiente **Sandbox** que redireciona todas as mensagens para um endereço configurado, garantindo que nenhum usuário real seja
contatado.

# Tecnologias

- .NET 9;

# Camadas

- Lab.SendEmail.API;
- Lab.SendEmail.Core;
- Lab.SendEmail.Infra.

# Pré-requisitos

## Configurar SMTP para envio de mensagens

A configuração deve ser feita usando `User-Secrets`. Clique com o botão direito em cima do projeto `Lab.SendEmail.API`, procure por
`Manage User Secrets`, cole e defina os valores para a seguinte configuração:

```json
{
  "EmailServiceConfiguration": {
    "Host": "",
    "User": "",
    "Password": "",
    "Port":
  }
}
```

**Obs: é possível colocar essa configuração no appsettings.json, mas não é recomendado.**

### Exemplo configuração gmail

A seguir será apresentado a configuração para envio de mensagens usando Gmail. 

```json
{
  "EmailServiceConfiguration": {
    "Host": "smtp.gmail.com",
    "User": "meuemail@gmail.com",
    "Password": "minha_senha_segura",
    "Port": 587
  }
}
```

# Ambiente SandBox

Pensando em ter um ambiente de desenvolvimento, testes e/ou homologação, foi incluído neste projeto um ambiente **Sandbox** para envio de e-mails
para garantir que as mensagens não sejam enviadas, por exemplo, para um destinatário real, mas para um e-mail de testes.

## Como ocorre a ativação

Localização: `Lab.SendEmail.API` > `Configuration` > `AddInfraLayer.cs`

Em `AddInfraLayer.cs`, procure por `AddEmailService`.

1. O código busca as configurações colocadas através do User Secrets ou que foram definidas no `appsettings.json`:

```c#
var config = builder
                .Configuration
                .GetSection("EmailServiceConfiguration")
                .Get<EmailServiceConfiguration>();

```

**Obs: Se a configuração não é localizada:**

```c#
 if (config is null)
            {
                throw new Exception("Atenção! É necessário definir as configurações de email no arquivo appsettings.json ou via User Secrets.");
            }
```

**uma exceção é lançada.**

Ao buscar a configuração é retornado uma instância de `EmailServiceConfiguration`. 
Obs: `EmailServiceConfiguration` pode ser achado dentro `Lab.SendMail.Infra` > `Services` > `EmailService`.

2. Se o ambiente de execução é desenvolvimento (Development), então ativa o modo sandbox:

```c#
if (Util.IsDesenv())
            {
                config.ActiveSandboxMode("destinatario@desenv.com");
            }
```

**Importante: o método ActiveSandboxMode (método de EmailServiceConfiguration) que ativa o modo SandBox e, com base no parâmetro passado, define o email destinatário de todas
as mensagens.**

`Util.IsDesenv()` descobre qual o ambiente de execução através da variável de ambiente `ASPNETCORE_ENVIRONMENT`. Você pode descobrir o valor
dessa variável através do caminho: `Lab.SendEmail.API` > `Properties` > `launchSettings.json`.
Esta aplicação segue a seguinte configuração:

`launchSettings.json`
```json
{
  "$schema": "https://json.schemastore.org/launchsettings.json",
  "profiles": {
    "Development": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": false,
      "applicationUrl": "http://localhost:5110",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "Production": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": false,
      "applicationUrl": "https://localhost:7129;http://localhost:5110",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Production"
      }
    }
  }
}
```

**De forma geral, rodar esta aplicação com o perfil (profile) `Development` fará com que a variável `ASPNETCORE_ENVIRONMENT` seja igual
a `Development` o que, consequentemente, ativa o SandBox de envio de mensagens.**.

