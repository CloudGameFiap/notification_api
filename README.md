# notification_api
A **NotificationAPI** é um serviço responsável por consumir eventos publicados por outros serviços da plataforma e disparar notificações por e-mail para os usuários finais.

Ela atua como um **consumer** dentro da arquitetura orientada a eventos, desacoplando a lógica de envio de notificações dos serviços que originam esses eventos (como `userapi` e `paymentapi`).

## Como funciona

1. O serviço se inscreve (subscribe) em tópicos/filas específicos.
2. Ao receber um evento, identifica o tipo de notificação necessária.
3. Monta o conteúdo do e-mail (template) com base nos dados do evento.
4. Envia o e-mail para o destinatário através de um provedor de envio.

## Eventos consumidos

| Serviço de origem | Evento                            | Ação disparada                                             |
|--------------------|----------------------------------|--------------------------------------------------          |
| `userapi`          | Conta criada                     | Envia e-mail de boas-vindas / confirmação de cadastro      |
| `paymentapi`          | Pagamento aprovado/ negado       | Envia e-mail confirmando a aprovação/Negado do pagamento   |


## Arquitetura (visão geral)

```
userapi  ──▶ (evento: conta criada)      ──▶ ┐
                                              ├─▶ NotificationAPI ──▶ Envio de e-mail
paymentapi  ──▶ (evento: pagamento aprovado/negado) ──▶ ┘
```

