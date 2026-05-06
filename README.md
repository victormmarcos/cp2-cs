## 1. Identificação
- Integrante 1: Arthur Bueno — RM558396
- Integrante 2: João Carotta — RM555187
- Integrante 3: Victor Magdaleno — RM556729

## 2. Produto bancário escolhido e justificativa
Como o grupo é trio, foram implementados dois produtos:

1. Empréstimo: exige análise de crédito, score, renda/faturamento e limite de parcela.
2. Máquina de Cartão: exige validação de faturamento/renda mínima e score.

Esses produtos foram escolhidos porque representam contratações bancárias com processamento assíncrono e possibilidade real de aprovação ou reprovação.

## 4. Diagrama de classes
<img width="1284" height="685" alt="Captura de tela 2026-05-05 223426" src="https://github.com/user-attachments/assets/e9cd44f3-96c1-4800-8cad-c807b0f9cf44" />



## 6. Endpoints disponíveis

### Criar agência
POST `/api/agencias`

```json
{
  "numero": "0001",
  "nome": "Agência Paulista",
  "cidade": "São Paulo",
  "uf": "SP"
}
```

### Criar cliente PF
POST `/api/clientes/pf`

```json
{
  "nome": "João Silva",
  "email": "joao@email.com",
  "agenciaId": 1,
  "cpf": "12345678900",
  "dataNascimento": "1995-01-20",
  "rendaMensal": 8000,
  "score": 720
}
```

### Criar cliente PJ
POST `/api/clientes/pj`

```json
{
  "nome": "Empresa ABC",
  "email": "contato@empresaabc.com",
  "agenciaId": 1,
  "cnpj": "12345678000199",
  "razaoSocial": "Empresa ABC LTDA",
  "faturamentoMensal": 50000,
  "score": 700
}
```

### Solicitar contratação de empréstimo
POST `/api/contratacoes`

```json
{
  "clienteId": 1,
  "produtoId": 1,
  "valorSolicitado": 12000,
  "prazoMeses": 24
}
```

### Solicitar contratação de máquina de cartão
POST `/api/contratacoes`

```json
{
  "clienteId": 2,
  "produtoId": 2,
  "valorSolicitado": null,
  "prazoMeses": null
}
```

### Consultar contratação
GET `/api/contratacoes/{id}`

## 7. Como executar os testes

```bash
dotnet test
```

Inserir print do resultado dos testes.

## 8. Print do RabbitMQ
Inserir print do painel do RabbitMQ mostrando a fila `contratacoes` com mensagens processadas.

## 9. Print da API no Swagger
<img width="1476" height="721" alt="image" src="https://github.com/user-attachments/assets/55b6c005-7695-4f08-831e-7b9209429cd4" />
<img width="1426" height="723" alt="image" src="https://github.com/user-attachments/assets/3026c603-54cf-4c5d-a521-3136e35618b6" />


