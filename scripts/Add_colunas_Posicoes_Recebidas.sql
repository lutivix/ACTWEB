--------------------------------------------------------------------------------------------------------
-- SISTEMA    	: ACTWEB - Sistema de controle de trens
-- TABELA       : POSICOES_RECEBIDAS 
-- SEQUENCE    	: POSICOES_RECEBIDAS_ID
-- CRIADO    EM	: 29/04/2016
-- ANALISTA    	: DENER VIANA
--------------------------------------------------------------------------------------------------------

ALTER TABLE ACTPP.POSICOES_RECEBIDAS ADD (PB_KM 			VARCHAR2(14 BYTE) NULL);
ALTER TABLE ACTPP.POSICOES_RECEBIDAS ADD (PB_CORREDOR VARCHAR2(14 BYTE) NULL);
ALTER TABLE ACTPP.POSICOES_RECEBIDAS ADD (PB_NOME_SB 	VARCHAR2(14 BYTE) NULL);
