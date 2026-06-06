<div align="center">

# Gestor de Documentos Náuticos - DPSN

## Plataforma de gerenciamento de documentos náuticos da empresa DPSN

### CENTRO PAULA SOUZA
### FACULDADE DE TECNOLOGIA DE JAHU
### CURSO DE TECNOLOGIA EM DESENVOLVIMENTO DE SOFTWARE MULTIPLATAFORMA

### Jaú, SP, BR
### Início: 3º Semestre / 2026

</div>

---

### Integrantes

| Nome | Função |
|------|--------|
| Anelize Nardelli | `Scrum Master` |
| João Pedro Pascuci De Russi | `Product Owner` |
| Luis Eduardo Abdo dos Santos | `Desenvolvedor` |

---

## Sumário

- [1. Introdução](#1-introdução)
- [2. Objetivos](#2-objetivos)
- [3. Métodos e Tecnologias](#3-métodos-e-tecnologias)
- [4. Regras de Negócio](#4-regras-de-negócio)
- [5. Estudo de Viabilidade](#5-estudo-de-viabilidade)
- [6. Pesquisa com Usuários](#6-pesquisa-com-usuários)
- [7. Design](#7-design)
- [8. Documento de Requisitos](#8-documento-de-requisitos)
- [9. Caso de Uso](#9-caso-de-uso)
- [10. Modelagem do Banco de Dados](#10-modelagem-do-banco-de-dados)
- [11. Protótipo](#11-protótipo)
- [12. Aplicação](#12-aplicação)
- [13. Considerações Finais](#13-considerações-finais)
- [14. Referências Bibliográficas](#14-referências-bibliográficas)

---

## 1. Introdução

> `Esse projeto é uma plataforma web desenvolvida com o objetivo de automatizar o preenchimento de documentos náuticos emitidos por estaleiros parceiros da empresa DPSN. A aplicação visa simplificar e agilizar o processo de emissão e gerenciamento de documentos navais, processo que atualmente na maioria dos casos é feito manualmente. 
A solução proposta é um sistema digital que centraliza e automatiza esse fluxo de trabalho. A plataforma permitirá que o estaleiro insira as informações do cliente e da embarcação de forma padronizada. Com base nesses dados, o sistema irá automaticamente gerar o documento necessário, de acordo com o modelo da embarcação, e convertê-lo para o formato PDF.
O arquivo PDF gerado ficará disponível para que o tecnólogo naval da empresa possa acessá-lo, aplicar sua assinatura digital de forma segura, e reencaminhá-lo ao estaleiro através da própria plataforma. Isso não apenas elimina a necessidade de múltiplas trocas de e-mail e manipulação manual de arquivos, mas também cria um registro digital de cada documento, facilitando o gerenciamento e a auditoria de todas as embarcações.
Este sistema, portanto, oferece uma solução robusta para otimizar o processo de documentação náutica, reduzindo o tempo de resposta, minimizando erros e garantindo a conformidade e a segurança dos dados."`

---

## 2. Objetivos

### 2.1 Objetivo Geral

> `Desenvolver uma plataforma que seja capaz de agilizar o processo de gerenciamento de documentos Náuticos.`

### 2.2 Objetivos Específicos

**Para os estaleiros parceiros:**
> ` Eliminar processos manuais, centralizar o envio de documentos, reduzir erros de preenchimento e legalizar suas embarcações`

**Para o tecnólogo naval (Hélcio):**
> `Acesso centralizado a todos os documentos, assinatura digital integrada, auditoria completa e organização dos dados`

**Para a equipe de desenvolvimento:**
> `Aplicação prática dos conteúdos do curso, desenvolvimento de uma solução real para a Empresa`

---

## 3. Métodos e Tecnologias

### 3.1 Tecnologias Utilizadas

| Camada | Tecnologia |
|--------|------------|
| Front-end | HTML, CSS, JavaScript, Tailwind CSS |
| Back-end | C#, ASP.NET Core (.NET) |
| Banco de Dados Não Relacional | MongoDB (logs e relatórios) |
| Assinatura Digital | Não Específicada |
| Versionamento | Git, GitHub |
| Prototipação | Figma |
| Editor de Código | Visual Studio  |

### 3.2 Ambiente de Desenvolvimento

> `O projeto foi realizado em laboratórios da FATEC e computadores pessoais, organização foi separada em tarefas para cada integrante.`

---

## 4. Regras de Negócio

### 4.1 Modelo de Negócio Canvas

> *Figura 1 — Modelo de Negócio Canvas*
> `Link com foto do canvas`

---

### 4.2 Proposta de Valor

> `Criar uma aplicação web para facilitar e automatizar o trabalho da empresa DPSN. E criar uma forma de armazenar seus documentos digitalmente, melhorando a organização e o gerenciamento deles.`

---

### 4.3 Parcerias-Chave

> `Empresa DPSN
Faculdade de Tecnologia de Jahu (Fatec Jahu)`

---

### 4.4 Atividades-Chave

> `Preencher automaticamente os documentos náuticos
Desenvolvimento e manutenção da aplicação`

---

### 4.5 Recursos-Chave

> `Ferramentas de desenvolvimento
Documentação
Organização da equipe`

---

### 4.6 Relacionamento com o Cliente

> `Feedback da empresa
Contato com a empresa`

---

### 4.7 Canais de Distribuição

> `O uso da aplicação pela empresa
Uso da aplicação pelos estaleiros parceiros da empresa`

---

### 4.8 Segmento de Clientes

> `Tecnólogos
Estaleiros parceiros da DPSN
Clientes dos estaleiros
Empresas do ramo naval`

---

### 4.9 Estrutura de Custos

> `Manutenção da aplicação
Hospedagem
Suporte`

---

### 4.10 Fontes de Receita

> `Investimentos da própria empresa DPSN`

---

## 5. Estudo de Viabilidade

### 5.1 Viabilidade Técnica

> `A equipe tem os recursos e conhecimentos necessários para desenvolver o projeto? Mencione equipamentos, ferramentas, suporte da instituição e domínio das tecnologias escolhidas.`

### 5.2 Viabilidade Financeira

> `O projeto é financeiramente viável para a DPSN adotar? Quais são os custos estimados de desenvolvimento e manutenção?`

### 5.3 Viabilidade de Mercado

> `Existe demanda para o produto no setor náutico? Como o DPSN se posiciona em relação aos processos manuais existentes atualmente nos estaleiros?`

### 5.4 Viabilidade Operacional

> `O sistema é simples o suficiente para ser usado pelo tecnólogo e pelos estaleiros sem treinamento extensivo? Há limitações operacionais a considerar?`

---

## 6. Pesquisa com Usuários

### 6.1 Personas

**Persona 1 — `Hélcio Marcelo de Russi`**

> *Figura 2 — Persona 1*
> `Insira a imagem da persona aqui.`

| Informações |  |
|----------|-----------|
| Nome | `Hélcio Marcelo de Russi` |
| Idade | `56` |
| Profissão | `Tecnólogo Naval` |
| Objetivo | `Uma ferramenta que automatize o preenchimento de formulários` |
| Frustrações | `Informações desorganizadas e processo lento` |


---

**Persona 2 — `Antonio Carlos`**

> *Figura 3 — Persona 2*
> `Insira a imagem da persona aqui.`

| Atributo | Descrição |
|----------|-----------|
| Nome | `Antonio Carlos` |
| Idade | `43` |
| Profissão | `Gestor de Estaleiros` |
| Objetivo | `Legalizar suas embarcações` |
| Frustrações | `informações enviadas e armazenadas são desorganizadas.` |


---

### 6.2 Mapa de Empatia

> *Figura 4 — Mapa de Empatia*
> `Link mapa de empatia`



---

### 6.3 Diagrama MoLIC

> *Figura 5 — Diagrama MoLIC*
> `Insira a imagem do diagrama MoLIC aqui.`

> `Descreva brevemente os principais fluxos de interação representados no diagrama. Ex: fluxo de cadastro de estaleiro, fluxo de solicitação de documento, fluxo de assinatura digital...`

---

## 7. Design

### 7.1 Paleta de Cores

| Nome | Código Hex |
|------|------------|
| `a preencher` | `a preencher` |
| `a preencher` | `a preencher` |
| `a preencher` | `a preencher` |

### 7.2 Tipografia

| Fonte | Uso |
|-------|-----|
| `a preencher` | Títulos |
| `a preencher` | Textos gerais |
| `a preencher` | Formulários |

### 7.3 Logo

> *Figura 6 — Logotipo DPSN*
> `Link da logo`

### 7.4 Modelo de Navegação

> *Figura 7 — Modelo de Navegação*
> `Insira a imagem do modelo de navegação aqui.`

> `Descrição do modelo de navegação`

---

## 8. Documento de Requisitos

### 8.1 Requisitos Funcionais

#### Módulo I: Autenticação e Acesso

**RF01 — Login de Administrador**
O sistema deve permitir o login do administrador (inserido por seed) via e-mail e senha.

**RF02 — Cadastro de Tecnólogo**
O sistema deve permitir o auto-cadastro de tecnólogo via e-mail e senha.

**RF03 — Cadastro de Estaleiro**
O sistema deve permitir que o tecnólogo cadastre um estaleiro parceiro.

**RF04 — Login e Logout**
O sistema deve permitir o login e o logout de qualquer usuário autenticado.

---

#### Módulo II: Gestão de Estaleiros

**RF05 — Visualização de Estaleiros**
O tecnólogo deve poder visualizar a lista de todos os estaleiros parceiros e seus respectivos dados.
O estaleiro deve poder visualizar seus próprios dados (ex.: telefone, endereço, logotipo).

**RF06 — Edição de Estaleiros**
O tecnólogo deve poder editar os dados de qualquer estaleiro.
O estaleiro deve poder editar seus próprios dados (ex.: telefone, endereço, logotipo).

---

#### Módulo III: Gestão de Embarcações

**RF07 — Cadastro de Embarcações**
O tecnólogo deve poder cadastrar embarcações no sistema.
O estaleiro deve poder cadastrar suas próprias embarcações.

**RF08 — Visualização de Embarcações**
O tecnólogo deve poder visualizar a lista de todas as embarcações no sistema, com opção de filtrar por atributos ou estaleiros proprietários.
O estaleiro deve poder visualizar a lista de suas embarcações, com opção de filtrar por atributos.

**RF09 — Edição de Embarcações**
O sistema deve permitir a edição de dados de todas as embarcações pelo tecnólogo naval.
O sistema deve permitir a edição de dados de suas embarcações pelo estaleiro.

---

#### Módulo IV: Gestão de Clientes

**RF10 — Cadastro de Clientes**
O tecnólogo deve poder cadastrar clientes de seus estaleiros parceiros.
O estaleiro deve poder cadastrar seus clientes.

**RF11 — Visualização de Clientes**
O sistema deve permitir que o tecnólogo naval visualize todos os clientes cadastrados, com opção de filtrar por atributos, estaleiros relacionados ou documentos.
O sistema deve permitir que o estaleiro visualize seus clientes, com opção de filtrar por atributos ou documentos.

**RF12 — Edição de Clientes**
O sistema deve permitir a edição de dados de todos os clientes pelo tecnólogo naval.
O sistema deve permitir a edição de dados de clientes pelo estaleiro relacionado.

---

#### Módulo V: Gestão de Documentos

**RF13 — Registro de Documento**
O sistema deve permitir o registro de um documento, vinculando estaleiro, embarcação e cliente.
O documento pode ser registrado pelo tecnólogo naval ou, de preferência, pelo estaleiro parceiro.

**RF14 — Visualização de Documentos**
O sistema deve permitir a visualização de todos os documentos pelo tecnólogo naval, com opção de filtrá-los por atributos próprios ou entidades relacionadas.
O sistema deve permitir a visualização de seus documentos por um estaleiro, com opção de filtrá-los por atributos próprios ou entidades relacionadas.

**RF15 — Edição de Documentos**
O tecnólogo naval deve poder editar os dados de qualquer documento no sistema.
O estaleiro deve poder editar os dados de seus documentos no sistema.

**RF16 — Solicitação de Revisão**
O estaleiro deve poder solicitar a revisão de um documento PDF.

**RF17 — Integração com Provedor de Assinatura**
O sistema deve integrar-se à API de Assinatura Eletrônica do Gov.br utilizando o protocolo OAuth2 para autenticação e autorização do usuário signatário.
O sistema deve gerenciar de forma segura os tokens de acesso (Access Tokens) temporários necessários para submeter o documento para assinatura.

**RF18 — Assinatura Digital Automatizada Gov.br**
O sistema deve permitir que o usuário (tecnólogo ou estaleiro) assine o documento PDF digitalmente de forma nativa na plataforma, utilizando sua conta Gov.br (nível prata ou ouro).
O sistema deve redirecionar o usuário para o fluxo de autorização do Gov.br e, após a confirmação do código de verificação (PIN), aplicar a assinatura digital válida ao documento.

**RF19 — Processamento e Armazenamento do PDF Assinado**
Após a confirmação da assinatura pelo Gov.br, o sistema deve receber automaticamente o documento assinado e atualizar seu status para "Assinado".
O sistema deve armazenar o PDF assinado e persistir os metadados da assinatura (hash do documento e dados de auditoria) na coleção correspondente do MongoDB.

**RF20 — Histórico e Auditoria de Assinaturas**
O sistema deve manter um registro detalhado de auditoria para cada documento, contendo a data/hora da assinatura e a identificação do signatário via Gov.br.

**RF21 — Download de PDFs Assinados**
O sistema deve permitir que o estaleiro, o tecnólogo naval e o cliente realizem o download do PDF devidamente assinado com a chancela digital do Gov.br.

---

#### Módulo VI: Relatórios e Insights

**RF22 — Dashboard de Estaleiro**
O sistema deve exibir gráficos interativos (documentos por mês) utilizando bibliotecas front-end.

**RF23 — Dashboard de Tecnólogo**
O sistema deve exibir gráficos interativos (documentos por mês e estaleiros com maior volume) utilizando bibliotecas front-end.
O dashboard do tecnólogo deve permitir filtrar a performance de estaleiros específicos, comparando-os entre si.

**RF24 — Exportação de Relatórios**
O sistema deve permitir a exportação de relatórios gerenciais em PDF (contendo gráficos e tabelas) para o tecnólogo naval e para o estaleiro.

**RF25 — Análise e Insights com IA**
O sistema deve enviar os dados consolidados do MongoDB para um motor de IA para gerar uma análise textual (ex.: "Sua emissão de documentos cresceu 10% em relação ao mês anterior").

---

#### Módulo VII: Auditoria

**RF26 — Log de CRUD de Objetos**
O sistema deve registrar um log sempre que um estaleiro ou o tecnólogo criar, editar ou excluir um registro (Cliente, Embarcação, Documento).

**RF27 — Log de Login e Logout**
O sistema deve registrar logs de acessos (login e logout) contendo o IP e o horário do usuário.

**RF28 — Log de Documentos**
O sistema deve registrar o histórico de geração e download de PDFs (quem baixou e quando).

**RF29 — Visualização de Logs**
O tecnólogo deve poder visualizar uma trilha de auditoria, filtrando por usuário ou por tipo de ação.

---

### 8.2 Requisitos Não Funcionais

#### Módulo I: Segurança e Privacidade

**RNF01 — Isolamento de Dados**
Um estaleiro nunca deve ter acesso aos dados (clientes, documentos, embarcações) de outro estaleiro.

**RNF02 — Integridade de Dados**
As credenciais de acesso devem ser criptografadas no banco de dados.

**RNF03 — Autenticação Segura**
O sistema deve utilizar autenticação baseada em token (JWT) para proteger as rotas da API.

---

#### Módulo II: Persistência e Performance

**RNF04 — Banco de Dados Único**
O sistema deve utilizar MongoDB para guardar os dados e armazenar dos documentos gerados.

**RNF05 — Retenção de Arquivos**
O sistema deve possuir uma rotina automática (Cron Job) para excluir arquivos PDF físicos após 30 dias de sua criação.

**RNF06 — Persistência de Metadados**
Mesmo após a exclusão do arquivo PDF, os metadados e informações do documento devem permanecer acessíveis no banco de dados.

**RNF07 — Imutabilidade e Performance dos Logs**
Os logs registrados no MongoDB não devem permitir edição ou exclusão manual via interface do sistema.
Cada log deve ser armazenado como um documento BSON contendo o "antes" e o "depois" (payload) das alterações realizadas.
A escrita dos logs deve ser feita de forma assíncrona para não impactar o tempo de resposta da transação principal no MySQL.

**RNF08 — Integração Assíncrona com IA**
A integração com IA deve ser feita de forma assíncrona para não travar a interface enquanto a resposta é gerada.

---

#### Módulo III: Usabilidade e Interface

**RNF09 — Responsividade**
A interface deve ser responsiva utilizando Tailwind CSS, permitindo o uso em desktops e tablets.

**RNF10 — Padronização de PDFs**
A geração de PDFs deve seguir um template fixo para garantir a padronização visual exigida pelo tecnólogo.

---

## 9. Caso de Uso

### 9.1 Diagrama Geral

> *Figura 8 — Diagrama de Caso de Uso*
> `Insira aqui a imagem do diagrama de caso de uso.`

---

### 9.2 Casos de Uso — Alto Nível

| Caso de Uso | Atores | Referência |
|-------------|--------|------------|
| Login | Administrador, Tecnólogo, Estaleiro | RF01, RF02, RF04 |
| Cadastrar Estaleiro | Tecnólogo | RF03 |
| Visualizar / Editar Estaleiro | Tecnólogo, Estaleiro | RF05, RF06 |
| Cadastrar Embarcação | Tecnólogo, Estaleiro | RF07 |
| Visualizar / Editar Embarcação | Tecnólogo, Estaleiro | RF08, RF09 |
| Cadastrar Cliente | Tecnólogo, Estaleiro | RF10 |
| Visualizar / Editar Cliente | Tecnólogo, Estaleiro | RF11, RF12 |
| Registrar Documento | Tecnólogo, Estaleiro | RF13 |
| Visualizar / Editar Documento | Tecnólogo, Estaleiro | RF14, RF15 |
| Solicitar Revisão de Documento | Estaleiro | RF16 |
| Assinar Documento via Gov.br | Tecnólogo, Estaleiro | RF17, RF18 |
| Download de PDF Assinado | Tecnólogo, Estaleiro, Cliente | RF21 |
| Visualizar Dashboard | Tecnólogo, Estaleiro | RF22, RF23 |
| Exportar Relatório | Tecnólogo, Estaleiro | RF24 |
| Visualizar Logs de Auditoria | Tecnólogo | RF29 |

---

### 9.3 Casos de Uso — Baixo Nível (Expandido)

#### Caso de Uso: Cadastrar Estaleiro

| Campo | Descrição |
|-------|-----------|
| **Ator** | Tecnólogo naval |
| **Finalidade** | Registrar um novo estaleiro parceiro no sistema |
| **Pré-condição** | O tecnólogo está autenticado |
| **Referência** | RF03 |

**Fluxo principal:**
1. O tecnólogo acessa a seção de estaleiros
2. Clica em "Cadastrar Estaleiro"
3. Preenche os dados do estaleiro (nome, CNPJ, telefone, endereço, logotipo)
4. Confirma o cadastro
5. O sistema valida os dados, cria a conta do estaleiro e gera as credenciais de acesso
6. O sistema envia as credenciais por e-mail ao estaleiro

**Fluxo alternativo:**
- Se o CNPJ já estiver cadastrado, o sistema exibe mensagem de erro e não cria o registro

---

#### Caso de Uso: Registrar e Solicitar Documento

| Campo | Descrição |
|-------|-----------|
| **Ator** | Estaleiro (ou Tecnólogo) |
| **Finalidade** | Registrar um novo documento vinculando estaleiro, embarcação e cliente |
| **Pré-condição** | O estaleiro está autenticado; embarcação e cliente já estão cadastrados |
| **Referência** | RF13 |

**Fluxo principal:**
1. O estaleiro acessa a seção de documentos
2. Clica em "Novo Documento"
3. Seleciona a embarcação e o cliente vinculados
4. Confirma o registro
5. O sistema gera o PDF pré-preenchido automaticamente
6. O documento fica disponível para assinatura

**Fluxo alternativo:**
- Se a embarcação ou o cliente não estiverem cadastrados, o sistema exibe aviso e redireciona para o cadastro correspondente

---

#### Caso de Uso: Assinar Documento via Gov.br

| Campo | Descrição |
|-------|-----------|
| **Ator** | Tecnólogo naval |
| **Finalidade** | Aplicar assinatura digital válida ao documento PDF via Gov.br |
| **Pré-condição** | O documento está gerado e com status "Pendente de Assinatura"; o tecnólogo possui conta Gov.br nível prata ou ouro |
| **Referência** | RF17, RF18, RF19 |

**Fluxo principal:**
1. O tecnólogo acessa o documento pendente
2. Clica em "Assinar com Gov.br"
3. O sistema redireciona para o fluxo de autorização OAuth2 do Gov.br
4. O tecnólogo confirma o PIN de verificação
5. O Gov.br retorna o documento assinado
6. O sistema atualiza o status do documento para "Assinado" e armazena os metadados
7. O documento fica disponível para download pelo estaleiro e pelo cliente

**Fluxo alternativo:**
- Se o PIN for inválido ou o tempo expirar, o sistema exibe mensagem de erro e permite nova tentativa

---



## 10. Modelagem do Banco de Dados

### 10.1 Justificativa da Abordagem Única

> `Optamos por utilizar o banco de dados não relacional MongoDB em nosso projeto devido à sua flexibilidade e eficiência no armazenamento e gerenciamento de informações. Por adotar uma estrutura baseada em documentos, o MongoDB permite uma organização mais completa, acessível e intuitiva dos dados, facilitando sua consulta e manutenção. Além disso, sua capacidade de lidar com diferentes formatos e estruturas de informação sem a necessidade de esquemas rígidos torna o desenvolvimento mais ágil e adaptável às necessidades do sistema. Essas características fazem do MongoDB uma excelente escolha para a organização e gerenciamento de documentos, contribuindo para a escalabilidade e o desempenho da aplicação.`

---



## 11. Protótipo


> *Figura 10 — Tela de Login*
> `Insira o print da tela de login aqui.`

> *Figura 11 — Dashboard do Tecnólogo*
> `Insira o print do dashboard aqui.`

> *Figura 12 — Lista de Estaleiros*
> `Insira o print da tela de estaleiros aqui.`

> *Figura 13 — Gestão de Documentos*
> `Insira o print da tela de documentos aqui.`

> *Figura 14 — Fluxo de Assinatura Gov.br*
> `Insira o print do fluxo de assinatura aqui.`

> *Figura 15 — Logs de Auditoria*
> `Insira o print da tela de auditoria aqui.`

---

## 12. Aplicação

**Link do repositório GitHub:** `a preencher`

### 12.1 Status de Desenvolvimento

| Funcionalidade | Status |
|----------------|--------|
| Login e autenticação | `✅ Implementado` |
| Cadastro de estaleiro | `✅ Implementado` |
| Gestão de embarcações | `✅ Implementado` |
| Gestão de clientes | `✅ Implementado` |
| Registro de documentos | `✅ Implementado` |
| Geração automática de PDF | `✅ Implementado` |
| Integração Gov.br (assinatura) | `✅ Implementado` |
| Dashboard e gráficos | `✅ Implementado` |
| Exportação de relatórios | `✅ Implementado` |
| Análise com IA | `⏳ Pendente` |
| Logs de auditoria | `  🔄 Em andamento` |

### 12.2 Prints da Aplicação

> `Espaço dedicado às imagens da aplicação.`

---

## 13. Considerações Finais

> `O sistema DPSN foi concebido para centralizar e automatizar o gerenciamento de informações relacionadas a estaleiros, embarcações, clientes, vendas e documentos náuticos, reduzindo a necessidade de processos manuais e aumentando a eficiência operacional. A plataforma permitirá maior organização, padronização e rastreabilidade dos dados, facilitando tanto a geração de documentos quanto a consulta de informações e relatórios gerenciais. Desenvolvido com MongoDB e Tailwind CSS, o sistema foi projetado para atender às necessidades atuais do tecnólogo naval Hélcio de Russi e de seus parceiros, além de oferecer uma base sólida para futuras expansões, como a implementação de recursos de análise de dados e inteligência artificial, contribuindo para uma gestão mais moderna, segura e eficiente dos processos do setor náutico.`

---

## 14. Referências Bibliográficas

**Ferramentas e Frameworks:**

MICROSOFT. ASP.NET Core documentation. 2026. Disponível em: https://learn.microsoft.com/aspnet/core

MICROSOFT. Entity Framework Core. 2026. Disponível em: https://learn.microsoft.com/ef/core

MONGODB, Inc. MongoDB Documentation. 2026. Disponível em: https://www.mongodb.com/docs/


TAILWIND LABS. Tailwind CSS Documentation. 2025. Disponível em: https://tailwindcss.com/docs


MICROSOFT. Visual Studio. 2026. Disponível em: https://code.visualstudio.com/

GIT SCM. Git Documentation. 2026. Disponível em: https://git-scm.com/doc

GITHUB, Inc. GitHub. 2026. Disponível em: https://github.com/

**Integração Gov.br:**

GOVERNO FEDERAL. API de Assinatura Eletrônica Gov.br. 2026. Disponível em: https://www.gov.br/conecta/catalogo/apis/assinatura-eletronica

