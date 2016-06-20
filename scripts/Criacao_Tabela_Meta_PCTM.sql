--drop table META_PCTM;
--drop sequence ACTWEB.META_PCTM_ID;

CREATE TABLE META_PCTM (    
  MTE_ID_MTE         NUMBER             NOT NULL PRIMARY KEY, 	-- Identificador da meta
  ROT_ID_ROT         NUMBER             NOT NULL,				-- Identificador da rota (tabela: ROTAS_PRODUCAO)
  MTE_DTE_PUB	     DATE               NOT NULL,				-- Data de publicação da meta
  MTE_DTE_VAL        DATE               NOT NULL,				-- Data de validade da meta			
  MTE_MTE_MTE        NUMBER             NOT NULL,				-- Meta a ser alcançada
  MTE_ATV_SN         VARCHAR2(1 BYTE)   NOT NULL);  			-- Identifica se a meta está ativa. S = sim | N = não.
 
CREATE SEQUENCE ACTWEB.META_PCTM_ID
  START WITH 1
  MAXVALUE 999999999999999999999999999
  MINVALUE 1
  NOCYCLE
  CACHE 20
  NOORDER;
  
SELECT * FROM META_PCTM;