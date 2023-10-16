# ByteBusters Shortener Controller

## Redirect Short Link

- **URL:** `/{code}`
- **HTTP Method:** GET
- **Description:** Redireciona para a URL original associada ao código curto fornecido.

### Response

- Se o código curto existir, o cliente será redirecionado para a URL original. Se o código não existir, uma resposta de erro 404 será retornada.

## Get Short Link

- **URL:** `/api/shortener/{code}`
- **HTTP Method:** GET
- **Description:** Recupera informações sobre um link encurtado.

### Response

**Success Response:**
- **HTTP Status Code:** 200 OK
- **Descrição:** Retorna informações sobre o link encurtado.

**Error Response:**
- **HTTP Status Code:** 400 Bad Request
- **Descrição:** Retorna um erro se o link não for encontrado.

## Create Short Link

- **URL:** `/api/shortener`
- **HTTP Method:** POST
- **Description:** Cria um link curto para uma URL original fornecida, caso não sejá informado um Alias será gerado uma código randomico de cinco caracteres.

### Response

**Success Response:**
- **HTTP Status Code:** 201 Created
- **Descrição:** Retorna informações sobre o link curto recém-criado, incluindo o código curto e a data de expiração.

**Error Response:**
- **HTTP Status Code:** 400 Bad Request
- **Descrição:** Retorna um erro se o alias já estiver em uso ou se ocorrer um erro ao criar o link curto.