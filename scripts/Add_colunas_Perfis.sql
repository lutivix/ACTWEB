--------------------------------------------------------------------------------------------------------
-- SISTEMA    	: SGS - Sistema de Gestão da Sinalização
-- TABELA       : PERFIS 
-- SEQUENCE    	: PERFIS_ID
-- CRIADO    EM	: 16/05/2016
-- ANALISTA    	: DENER VIANA
--------------------------------------------------------------------------------------------------------

ALTER TABLE PERFIS ADD (PER_QTDE_MC61 NUMBER DEFAULT 1 NOT NULL);
