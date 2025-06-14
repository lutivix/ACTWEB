select count(*) from TIPO_SITUACAO;
select * from TIPO_SITUACAO;

DROP TABLE TIPO_SITUACAO;
DROP SEQUENCE TIPO_SITUACAO_ID;

CREATE TABLE TIPO_SITUACAO
(
  TP_SIT_ID      number              not null, -- Identificador do tipo de situa��o
  TP_SIT_CODIGO  number              null, -- C�digo do tipo de situa��o
  TP_SIT_NOME    varchar2(30 byte)   null, -- Nome ou descri��o do tipo de situa��o
  TP_SIT_ATIVO   varchar2(1 byte)    not null  -- Se a situa��o estiver ativa marque "S" caso contrario marque "N"
);
ALTER TABLE TIPO_SITUACAO ADD ( CONSTRAINT PK_TIPO_SITUACAO PRIMARY KEY (TP_SIT_ID) );

CREATE SEQUENCE TIPO_SITUACAO_ID
  START WITH 1
  MAXVALUE 999999999999999999999999999
  MINVALUE 1
  NOCYCLE
  CACHE 20
  NOORDER;
  
INSERT INTO TIPO_SITUACAO (TP_SIT_ID, TP_SIT_CODIGO, TP_SIT_NOME, TP_SIT_ATIVO) VALUES (TIPO_SITUACAO_ID.NEXTVAL, 0, NULL, 'S' );
INSERT INTO TIPO_SITUACAO (TP_SIT_ID, TP_SIT_CODIGO, TP_SIT_NOME, TP_SIT_ATIVO) VALUES (TIPO_SITUACAO_ID.NEXTVAL, 1, 'S - Solicitada', 'S' );
INSERT INTO TIPO_SITUACAO (TP_SIT_ID, TP_SIT_CODIGO, TP_SIT_NOME, TP_SIT_ATIVO) VALUES (TIPO_SITUACAO_ID.NEXTVAL, 2, 'C - Confirmada', 'S' );
INSERT INTO TIPO_SITUACAO (TP_SIT_ID, TP_SIT_CODIGO, TP_SIT_NOME, TP_SIT_ATIVO) VALUES (TIPO_SITUACAO_ID.NEXTVAL, 3, 'X - Cancelada pelo Solicitante', 'S' );
INSERT INTO TIPO_SITUACAO (TP_SIT_ID, TP_SIT_CODIGO, TP_SIT_NOME, TP_SIT_ATIVO) VALUES (TIPO_SITUACAO_ID.NEXTVAL, 4, 'N - Negada', 'S' );
INSERT INTO TIPO_SITUACAO (TP_SIT_ID, TP_SIT_CODIGO, TP_SIT_NOME, TP_SIT_ATIVO) VALUES (TIPO_SITUACAO_ID.NEXTVAL, 5, 'R - Retirada', 'S' );
INSERT INTO TIPO_SITUACAO (TP_SIT_ID, TP_SIT_CODIGO, TP_SIT_NOME, TP_SIT_ATIVO) VALUES (TIPO_SITUACAO_ID.NEXTVAL, 6, 'A - ARetirar', 'S' );
  
SELECT TP_SIT_ID, TP_SIT_CODIGO, TP_SIT_NOME, TP_SIT_ATIVO FROM TIPO_SITUACAO      
                                       WHERE TP_SIT_ATIVO = 'S' ; 