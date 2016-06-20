--------------------------------------------------------------------------------------------------------
-- SISTEMA    	: ACTWEB 
-- TABELA       : CORREDOR
-- SEQUENCE    	: CORREDOR_ID
-- CRIADO    EM	: 08/04/2016
-- ANALISTA    	: DENER VIANA
--------------------------------------------------------------------------------------------------------

--drop table CORREDOR;
--drop sequence CORREDOR_ID;

CREATE TABLE CORREDOR (																				  -- Cria a tabela corredor
  CO_ID_CO					NUMBER							NOT NULL PRIMARY KEY,		-- Identificador do corredor
  CO_TRECHO					NUMBER							NOT NULL,								-- Identificador do trecho
  CO_CORREDOR				VARCHAR2(70 BYTE)		NULL,										-- Nome do corredor
  CO_LAT						NUMBER							NULL,										-- Latitude
  CO_LON						NUMBER							NULL,										-- Longitude
  CO_KM							NUMBER							NULL,										-- KM
  CO_VEL_ASC				NUMBER							NULL,										-- Velocidade ascendente
  CO_VEL_DESC				NUMBER							NULL,										-- Velocidade descendente
  CO_NOME_SB				VARCHAR2(14 BYTE)		NULL);									-- Nome da SB
							
 
CREATE SEQUENCE CORREDOR_ID																			-- Cria sequence (contador) da tabela
  START WITH 1                                                                                                    
  MAXVALUE 999999999999999999999999999                                                    
  MINVALUE 1
  NOCYCLE
  CACHE 20
  NOORDER;
