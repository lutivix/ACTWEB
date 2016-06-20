select count(*) from usuario order by id;
select * from usuario order by nivel;
select * from nivel_acesso;

alter table nivel_acesso add(ABREVIADO VARCHAR2(10) null;

select uu.MATRICULA, uu.SENHA, uu.NOME, uu.NIVEL, nn.ID, nn.ABREVIADO from usuario uu, nivel_acesso nn
where uu.NIVEL = nn.ID order by uu.NIVEL;


insert into nivel_acesso (ID, NOME, ATIVO, DATACRIACAO, DATAALTERACAO, ABREVIADO) values (1, 'ADMINISTRADOR', 'S', sysdate, sysdate, 'ADM');
insert into nivel_acesso (ID, NOME, ATIVO, DATACRIACAO, DATAALTERACAO, ABREVIADO) values (2, 'PADRÃO', 'S', sysdate, sysdate, 'PAD');
insert into nivel_acesso (ID, NOME, ATIVO, DATACRIACAO, DATAALTERACAO, ABREVIADO) values (3, 'CENTRO DE APOIO AO TREM', 'S', sysdate, sysdate, 'CAT');
insert into nivel_acesso (ID, NOME, ATIVO, DATACRIACAO, DATAALTERACAO, ABREVIADO) values (4, 'CENTRO DE TOMADA DE DECISÃO', 'S', sysdate, sysdate, 'CTD');
insert into nivel_acesso (ID, NOME, ATIVO, DATACRIACAO, DATAALTERACAO, ABREVIADO) values (5, 'HELPDESK', 'S', sysdate, sysdate, 'PROG');
insert into nivel_acesso (ID, NOME, ATIVO, DATACRIACAO, DATAALTERACAO, ABREVIADO) values (6, 'OPERADOR DE VIA PERMANENTE', 'S', sysdate, sysdate, 'OP VP');
insert into nivel_acesso (ID, NOME, ATIVO, DATACRIACAO, DATAALTERACAO, ABREVIADO) values (7, 'OPERADOR ELETROELETRÔNICO', 'S', sysdate, sysdate, 'OP ELE');
insert into nivel_acesso (ID, NOME, ATIVO, DATACRIACAO, DATAALTERACAO, ABREVIADO) values (8, 'CENTRO DE CONTROLE DE EMERGÊNCIA', 'S', sysdate, sysdate, 'CCE');
insert into nivel_acesso (ID, NOME, ATIVO, DATACRIACAO, DATAALTERACAO, ABREVIADO) values (9, 'PROGRAMAÇÃO E CONTROLE DE MANUTENÇÃO', 'S', sysdate, sysdate, 'PCM');
insert into nivel_acesso (ID, NOME, ATIVO, DATACRIACAO, DATAALTERACAO, ABREVIADO) values (10, 'ADMINISTRADOR DE VIA PERMANENTE', 'S', sysdate, sysdate, 'VP ADM');

update usuario set nivel = 1001 where nivel = 1;
update usuario set nivel = 1002 where nivel = 2;
update usuario set nivel = 1003 where nivel = 3;
update usuario set nivel = 1004 where nivel = 4;
update usuario set nivel = 1005 where nivel = 5;
update usuario set nivel = 1006 where nivel = 6;
update usuario set nivel = 1007 where nivel = 7;
update usuario set nivel = 1008 where nivel = 8;
update usuario set nivel = 1009 where nivel = 9;
update usuario set nivel = 1010 where nivel = 10;
update usuario set nivel = 1011 where nivel = 11;
update usuario set nivel = 1012 where nivel = 12;
update usuario set nivel = 1013 where nivel = 13;
update usuario set nivel = 1014 where nivel = 14;
update usuario set nivel = 1015 where nivel = 15;
update usuario set nivel = 1016 where nivel = 16;
update usuario set nivel = 1017 where nivel = 17;
update usuario set nivel = 1018 where nivel = 18;
update usuario set nivel = 1019 where nivel = 19;
update usuario set nivel = 1020 where nivel = 20;

update usuario set nivel = 1 where nivel = 1018;
update usuario set nivel = 2 where nivel = 1001;
update usuario set nivel = 2 where nivel = 1002;
update usuario set nivel = 2 where nivel = 1005;
update usuario set nivel = 2 where nivel = 1012;
update usuario set nivel = 2 where nivel = 1015;
update usuario set nivel = 3 where nivel = 1003;
update usuario set nivel = 3 where nivel = 1007;
update usuario set nivel = 4 where nivel = 1004;
update usuario set nivel = 5 where nivel = 1006;
update usuario set nivel = 6 where nivel = 1011;
update usuario set nivel = 7 where nivel = 1009;
update usuario set nivel = 7 where nivel = 1016;
update usuario set nivel = 8 where nivel = 1008;
update usuario set nivel = 9 where nivel = 1013;
update usuario set nivel = 9 where nivel = 1014;
update usuario set nivel = 10 where nivel = 1019;


