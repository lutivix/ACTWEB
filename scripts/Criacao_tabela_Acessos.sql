select count(*) from ACESSOS;
select * from ACESSOS;

drop table ACESSOS;
drop sequence ACESSOS_ID;

CREATE TABLE ACESSOS
(
  ACESSOS_ID       number              not null,
  MATRICULA        varchar2(30 byte)   not null,
  DATA_ACESSO      date                not null
);

ALTER TABLE ACESSOS ADD ( CONSTRAINT PK_ACESSOS PRIMARY KEY (ACESSOS_ID) );

CREATE SEQUENCE ACESSOS_ID
  START WITH 1
  MAXVALUE 999999999999999999999999999
  MINVALUE 1
  NOCYCLE
  CACHE 20
  NOORDER;