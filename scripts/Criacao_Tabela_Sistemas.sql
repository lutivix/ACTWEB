--------------------------------------------------------------------------------------------------------
-- SISTEMA    	: ACTWEB 
-- TABELA       : SISTEMAS
-- SEQUENCE    	: SISTEMAS_ID
-- CRIADO    EM	: 15/04/2016
-- ANALISTA    	: DENER VIANA
--------------------------------------------------------------------------------------------------------

--drop table SISTEMAS;
--drop sequence SISTEMAS_ID;

CREATE TABLE SISTEMAS (																	  	-- Cria a tabela trechos
  SIS_ID							NUMBER							NOT NULL PRIMARY KEY,		-- Identificador do sistema
  SIS_NOME						VARCHAR2(070 BYTE)	NOT NULL,								-- Nome do sistema
	SIS_DESCRICAO				VARCHAR2(200 BYTE)  NULL,										-- Descrição do sistema
  SIS_ATIVO						VARCHAR2(1 BYTE)		NOT NULL);							-- Status [ S = Ativo | N = Inativo ]
 
CREATE SEQUENCE SISTEMAS_ID																	-- Cria sequence (contador) da tabela
  START WITH 1                                                                                                    
  MAXVALUE 999999999999999999999999999                                                    
  MINVALUE 1
  NOCYCLE
  CACHE 20
  NOORDER;
  
INSERT INTO SISTEMAS (SIS_ID, SIS_NOME, SIS_DESCRICAO, SIS_ATIVO) VALUES (SISTEMAS_ID.NEXTVAL, 'ACTWEB', 'Sistema de automação e controle de trens da VLI', 'S');
INSERT INTO SISTEMAS (SIS_ID, SIS_NOME, SIS_DESCRICAO, SIS_ATIVO) VALUES (SISTEMAS_ID.NEXTVAL, 'SGSWEB', 'Sistema de gerenciamento da sinalização da VLI', 'S');
