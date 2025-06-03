CREATE TABLE clientes
(
  codigo SERIAL PRIMARY KEY,
  cpf varchar(11) NOT NULL,
  nome varchar(150) NOT NULL,
  email varchar(200) NOT NULL,
  CONSTRAINT cpf UNIQUE(cpf),
  CONSTRAINT email UNIQUE(email)
);

CREATE TABLE produtos (
    codigo SERIAL PRIMARY KEY,
    nome VARCHAR(255) NOT NULL,
    descricao TEXT,
    preco NUMERIC(10, 2) NOT NULL,
    categoria INTEGER NOT NULL,
    situacao INTEGER NOT NULL
);

CREATE TABLE pedidos (
    codigo SERIAL PRIMARY KEY,
    codigo_cliente INT REFERENCES clientes(codigo) ON DELETE SET NULL
);

CREATE TABLE pedido_combos (
    codigo SERIAL PRIMARY KEY,
    codigo_pedido INT REFERENCES pedidos(codigo) ON DELETE CASCADE NOT NULL,
    preco FLOAT NOT NULL
);

CREATE TABLE pedido_combo_items (
    codigo SERIAL PRIMARY KEY,
    pedido_combo_codigo INT REFERENCES pedido_combos(codigo) ON DELETE CASCADE NOT NULL,
    codigo_produto INT REFERENCES produtos(codigo),
    preco FLOAT NOT NULL
);

CREATE TABLE pedido_pagamento (
    codigo SERIAL PRIMARY KEY,
    codigo_pedido INT REFERENCES pedidos(codigo) ON DELETE CASCADE NOT NULL,
    valor DECIMAL(10, 2) NOT NULL,
    forma_pagamento VARCHAR(50),
    data_pagamento TIMESTAMP
);

CREATE TABLE pedido_status (
    codigo SERIAL PRIMARY KEY,
    codigo_pedido INT REFERENCES pedidos(codigo) ON DELETE CASCADE NOT NULL,
    status int NOT NULL,
    data_criacao TIMESTAMP DEFAULT NOW() NOT NULL
);

INSERT INTO "produtos" ("nome", "descricao", "preco", "categoria", "situacao")
VALUES
('Happy Meal Plus', 'Mini hambúrguer, batatas pequenas e surpresa infantil', 22.90, 1, 1),
('Mega Burger', 'Hambúrguer duplo, queijo cheddar, bacon e molho especial', 34.90, 1, 1),
('Super Cheese', 'Hambúrguer bovino, queijo duplo, cebola crispy e molho barbecue', 32.50, 1, 1),
('Fritas P', 'Porção pequena de batatas fritas crocantes. 90g', 8.90, 2, 1),
('Fritas M', 'Porção média de batatas fritas crocantes. 180g', 12.50, 2, 1),
('Fritas G', 'Porção grande de batatas fritas crocantes. 280g', 15.90, 2, 1),
('Refil Livre', 'Refrigerante à vontade, diversas opções disponíveis', 13.90, 3, 1),
('Suco Natural', 'Suco natural de laranja, maçã ou abacaxi. 300ml', 14.50, 3, 1),
('Cone Simples', 'Sorvete em cone. Sabores: Chocolate, Morango ou Creme', 4.50, 4, 1),
('Sundae Especial', 'Sorvete com calda e cobertura crocante. Diversos sabores', 10.90, 4, 1),
('Super Shake', 'Milk-shake cremoso. Sabores: Ovomaltine, Chocolate, Morango ou Baunilha', 18.50, 4, 1);


INSERT INTO "clientes" ("cpf", "nome", "email") VALUES
('39592262098',	'Teste 1',	'teste1@email.com'),
('49783597086',	'Teste 2',	'teste2@email.com');

INSERT INTO "pedidos" ("codigo_cliente") VALUES
(NULL),
(1),
(2),
(NULL);

INSERT INTO "pedido_status" (codigo_pedido, "status", "data_criacao") VALUES
(1,	1,	'2025-01-01 21:41:42.681931'),
(2,	1,	'2025-01-01 21:41:50.691848'),
(2,	2,	'2025-01-01 21:41:59.801704'),
(3,	1,	'2025-01-01 21:42:08.12827'),
(4,	1,	'2025-01-01 21:44:08.12827'),
(1, 2,	'2025-01-01 21:44:26.020703'),
(1,	4,	'2025-01-01 21:44:45.576766'),
(1,	5,	'2025-01-01 21:59:45.576766');

INSERT INTO "pedido_pagamento" (codigo_pedido, "valor", "forma_pagamento", "data_pagamento") VALUES
(1,	50.7,	'1',	'2025-01-01 21:45:27.806018'),
(2,	11.9,	'1',	'2025-01-01 21:45:51.00446');


INSERT INTO "pedido_combos" (codigo_pedido, "preco") VALUES
(1,	50.7),
(2,	11.9),
(3,	50.7),
(3,	11.9),
(4,	11.9),
(4,	50.7);


INSERT INTO "pedido_combo_items" ("pedido_combo_codigo", "codigo_produto", "preco") VALUES
(1,	1,	24.9),
(1,	4,	9.9),
(1,	8,	15.9),
(2,	5,	11.9),
(3,	5,	11.9),
(4,	3,	29.9),
(4,	5,	11.9),
(4,	10,	8.9),
(5,	5,	11.9),
(6,	3,	29.9),
(6,	5,	11.9),
(6,	10,	8.9);