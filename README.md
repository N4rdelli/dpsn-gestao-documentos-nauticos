# Gestor de documentos náuticos - DPSN
## Plataforma de gerenciamento de documentos náuticos da empresa DPSN
### CENTRO PAULA SOUZA  
### FACULDADE DE TECNOLOGIA DE JAHU  
### CURSO DE TECNOLOGIA EM DESENVOLVIMENTO DE SOFTWARE MULTIPLATAFORMA 
### Jau, SP, BR
### Inicio: 2° Semestre / 2025
---
# Documento da aplicação web
### Autores
- Anelize Nardelli
- João Pedro Pascuci De Russi
- Luis Eduardo Abdo dos Santos
# Sumário
  - [1. Descrição da aplicação web](#1-descrição-da-aplicação-web)
    - [1.1 Introdução](#11-introdução)
    - [1.2 Métodos da pesquisa](#12-método-de-pesquisa)
  - [2. Documento de requisitos](#2-documento-de-requisitos)
    - [2.1. Requisitos funcionais](#21-requisitos-funcionais)
    - [2.2. Requisitos não funcionais](#22-requisitos-não-funcionais)
  - [3. Regras de negócios](#3-regras-de-negócio)
      - [3.1 Figura 1- Modelo de nogócio canvas](#31-figura-1--modelo-de-nogócio-canvas)
      - [3.2 Proposta de valor](#32-proposta-de-valor)
      - [3.3 Parcerias-chaves](#33-parcerias-chaves)
      - [3.4 Recursos-chave](#34-recursos-chave)
      - [3.5 Atividades-chave](#35-atividades-chave)
      - [3.6 Relacionamento com cliente](#36-relacionamento-com-cliente)
      - [3.7 Canais de distribuição](#37-canais-de-distribuição)
      - [3.8 Segmento de clientes](#38-segmento-de-clientes)
      - [3.9 Estrutura de custos](#39-estrutura-de-custos)
      - [3.10 Fonte de receita](#310-fonte-de-receita)
  - [6. Diagrama Entidade-Relacionamento](#6-diagrama-entidade-relacionamento) 
    - [6 1. Figura 3- Diagrama Entidade-Relacionamento](#61-figura-3-diagrama-entidade-relacionamento) 
  
                
        

## 1. Descrição da aplicação web
## 1.1 Introdução
Esse projeto é uma plataforma web desenvolvida com o objetivo de automatizar o preenchimento de documentos náuticos emitidos por estaleiros parceiros da empresa DPSN. A aplicação visa simplificar e agilizar o processo de emissão e gerenciamento de documentos navais, processo que atualmente na maioria dos casos é feito manualmente. 

A solução proposta é um sistema digital que centraliza e automatiza esse fluxo de trabalho. A plataforma permitirá que o estaleiro insira as informações do cliente e da embarcação de forma padronizada. Com base nesses dados, o sistema irá automaticamente gerar o documento necessário, de acordo com o modelo da embarcação, e convertê-lo para o formato PDF. 

O arquivo PDF gerado ficará disponível para que o tecnólogo naval da empresa possa acessá-lo, aplicar sua assinatura digital de forma segura, e reencaminhá-lo ao estaleiro através da própria plataforma. Isso não apenas elimina a necessidade de múltiplas trocas de e-mail e manipulação manual de arquivos, mas também cria um registro digital de cada documento, facilitando o gerenciamento e a auditoria de todas as embarcações. 

Este sistema, portanto, oferece uma solução robusta para otimizar o processo de documentação náutica, reduzindo o tempo de resposta, minimizando erros e garantindo a conformidade e a segurança dos dados.  
## 1.2 Método de pesquisa 
Ao desenvolver a aplicação web, serão utilizadas diversas ferramentas e tecnologias apresentadas no curso de Desenvolvimento de Software Multiplataforma (DSM), integrando conceitos abordados ao longo das aulas. Algumas dessas ferramentas incluem tecnologias de desenvolvimento front-end e back-end, além de metodologias de engenharia de software. 

As tecnologias usadas no Front-end do projeto incluem HTML, CSS, JavaScript, os frameworks Bootstrap e Tailwind CSS. O Beck-end do nosso projeto vai usar a linguagem C# e o Asp Net Core, estamos fazendo uma aplicação inteiramente no universo .NET. Nosso bando de dados é um banco NoSQL, o MongoDB. O protótipo do projeto foi criado no Figma.
## 2. Documento de Requisitos
## 2.1 Requisitos Funcionais
### Módulo I: Autenticação e Acesso
### RF 01 - Cadastro de Administrador
O sistema deve permitir o login do administrador (tecnólogo naval Hélcio) via e-mail e senha.
### RF 02 - Cadastro de Estaleiros
O sistema deve permitir que o administrador cadastre um estaleiro parceiro.
### RF 03 - Credenciais de Acesso para Estaleiros
O sistema deve gerar credenciais temporárias para o acesso de estaleiro (enviar via e-mail).
### RF 04 - Login e Logout
O sistema deve permitir o login e o logout de qualquer usuário autenticado.

### Módulo II: Gestão de Estaleiros
### RF 05 - Visualização de Estaleiros
O tecnólogo deve poder visualizar a lista de todos os estaleiros parceiro e seus respectivos dados.
O estaleiro deve poder visualizar seus próprios dados (ex: telefone, endereço, logotipo).
### RF 06 - Edição de Estaleiros
O tecnólogo deve poder editar os dados de qualquer estaleiro.
O estaleiro deve poder editar seus próprios dados (ex: telefone, endereço, logotipo).
### Módulo III: Gestão de Embarcações
### RF 07 - Cadastro de Embarcações
O tecnólogo deve poder cadastrar embarcações no sistema.
O estaleiro deve poder cadastrar suas próprias embarcações.
### RF 08 - Visualização de Embarcações
O tecnólogo deve poder visualizar a lista de todas as embarcações no sistema (bem como filtrar por atributos ou estaleiros proprietários).
O estaleiro deve poder visualizar a lista de suas embarcações (bem como filtrar por atributos).
### RF 09 - Edição de Embarcações
O sistema deve permitir a edição de dados de todas as embarcações pelo tecnólogo naval.
O sistema deve permitir a edição de dados de suas embarcações pelo estaleiro.
### Módulo IV: Gestão de Clientes
### RF 10 - Cadastro de Clientes
O tecnólogo deve poder cadastrar clientes de seus estaleiros parceiros.
O estaleiro deve poder cadastrar seus clientes.
### RF 11 - Visualização de Clientes
O sistema deve permitir que o tecnólogo naval visualize todos os clientes cadastrados (bem como filtre por atributos, estaleiros relacionados ou vendas).
O sistema deve permitir que o estaleiro visualize seus clientes (bem como filtre por atributos ou vendas).
### RF 12 - Edição de Clientes
O sistema deve permitir a edição de dados de todos os clientes pelo tecnólogo naval.
O sistema deve permitir a edição de dados de clientes pelo estaleiro relacionado.
### Módulo V: Gestão de Vendas
### RF 13 - Registro de Vendas
O sistema deve permitir o registro de uma venda, vinculando estaleiro, embarcação e cliente.
A venda pode ser registrada pelo tecnólogo naval ou, de preferência, pelo estaleiro parceiro.
### RF 14 - Visualização de Vendas
O sistema deve permitir a visualização de todas as vendas pelo tecnólogo naval (bem como filtrá-las por atributos próprios ou entidades relacionadas).
O sistema deve permitir a visualização de suas vendas por um estaleiro (bem como filtrá-las por atributos próprios ou entidades relacionadas).
### RF 15 - Edição de Vendas
O tecnólogo naval deve poder editar os dados de qualquer venda no sistema.
O estaleiro deve poder editar os dados de suas vendas no sistema.
### Módulo VI: Gestão de Documentos
### RF 16 - Solicitação de Documentos
O estaleiro deve poder solicitar a geração de um documento PDF para uma venda específica.
### RF 17 - Geração Automática de PDFs
O sistema deve gerar automaticamente o PDF pré-preenchido com os dados do banco de dados.
### RF 18 - Download de PDFs pendentes
O tecnólogo deve poder baixar o PDF gerado para assinatura manual/digital externa.
### RF 19 - Upload de PDFs
O tecnólogo deve poder realizar o upload do PDF já assinado para a plataforma.
### RF 20 - Visualização de Documentos assinados
O estaleiro deve poder visualizar a lista de documentos assinados disponíveis para ele.
### RF 21 - Download de PDFs assinados
O estaleiro deve poder realizar o download do PDF assinado.
### Módulo VII: Relatórios e Insights
### RF 22 - Dashboard de Estaleiro
O sistema deve exibir gráficos interativos (vendas por mês) utilizando bibliotecas front-end.
### RF 23 - Dashboard de Tecnólogo
O sistema deve exibir gráficos interativos (vendas por mês, estaleiros que mais vendem) utilizando bibliotecas front-end.
O dashboard do tecnólogo deve permitir filtrar a performance de estaleiros específicos comparando-os entre si.
### RF 24 - Exportação de Relatórios
O sistema deve permitir a exportação de relatórios gerenciais em PDF (contendo os gráficos e tabelas) para o tecnólogo naval e para o estaleiro.
### RF 25 - Análise e Insights
O sistema deve enviar os dados consolidados do MongoDB para um motor de IA para gerar uma análise textual (ex: "Sua venda cresceu 10% em relação ao mês anterior").
### Módulo VIII: Auditoria
### RF 26 - Log de CRUD de objetos
O sistema deve registrar um log sempre que um estaleiro ou o tecnólogo criar, editar ou excluir um registro (Cliente, Embarcação, Venda).
### RF 27 - Log de login e logout
O sistema deve registrar logs de acessos (login e logout) contendo o IP e o horário do usuário.
### RF 28 - Log de Documentos
O sistema deve registrar o histórico de geração e download de PDFs (quem baixou e quando).
### RF 29 - Visualização de Logs
O tecnólogo deve poder visualizar uma trilha de auditoria filtrando por usuário ou por tipo de ação.
