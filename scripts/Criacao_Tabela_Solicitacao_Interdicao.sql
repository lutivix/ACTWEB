DROP TABLE ACTWEB.SOLICITACAO_INTERDICAO;

CREATE TABLE ACTWEB.SOLICITACAO_INTERDICAO
(
  SLT_ID_SLT            NUMBER              NOT NULL,
  SLT_ID_SLT_ACT        NUMBER              NOT NULL,
  SLT_ID_SECAO          NUMBER              NOT NULL,
  SLT_ID_MOTIVO					NUMBER							NOT NULL,
  SLT_ID_TP_SITUACAO    NUMBER              NOT NULL,
  SLT_ID_TP_INTERDICAO  NUMBER              NOT NULL,
  SLT_ID_TP_MANUTENCAO  NUMBER              NOT NULL,
  SLT_ID_TP_CIRCULACAO  NUMBER              NULL,
  SLT_ID_ACT_AUT_INTER  NUMBER              NULL,
  SLT_MAT_RESPONSAVEL   VARCHAR2(30 byte)   NOT NULL,
  SLT_DATA              DATE                NOT NULL,
  SLT_DURACAO           NUMBER              NOT NULL,
  SLT_KM                NUMBER(7,3)         NOT NULL,
  SLT_TELEFONE_SN       VARCHAR2(1 byte)    NULL,
  SLT_TELEFONE_NUMERO   VARCHAR2(30 byte)   NULL,
  SLT_RADIO_SN          VARCHAR2(1 byte)    NULL,
  SLT_MACRO_SN          VARCHAR2(1 byte)    NULL,
  SLT_MACRO_NUMERO      VARCHAR2(14 byte)   NULL,
  SLT_EQUIPAMENTOS      VARCHAR2(1024 byte) NULL,  
  SLT_OBSERVACAO        VARCHAR2(1024 byte) NULL,
  SLT_USUARIO_LOGADO    VARCHAR2(30 byte)   NOT NULL,
  SLT_ATIVO_SN          VARCHAR2(1 byte)    NOT NULL
);

ALTER TABLE ACTWEB.SOLICITACAO_INTERDICAO ADD ( CONSTRAINT PK_INTERDICAO PRIMARY KEY (SLT_ID_SLT) );

DROP SEQUENCE ACTWEB.SOLICITACAO_INTERDICAO_ID;

CREATE SEQUENCE ACTWEB.SOLICITACAO_INTERDICAO_ID
  START WITH 1
  MAXVALUE 999999999999999999999999999
  MINVALUE 1
  NOCYCLE
  NOCACHE 
  NOORDER;
   
