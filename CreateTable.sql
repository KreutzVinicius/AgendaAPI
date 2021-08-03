create table contatos  (
	id serial unique,
    nome varchar(100) not null,
    numero varchar(30) not null,
    avatar varchar(300),
    email varchar(150),
    primary key (id)
);
