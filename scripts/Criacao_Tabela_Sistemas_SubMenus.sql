--------------------------------------------------------------------------------------------------------
-- SISTEMA    	: ACTWEB 
-- TABELA       : SISTEMAS_SUBMENUS
-- SEQUENCE    	: SISTEMAS_SUBMENUS_ID
-- CRIADO    EM	: 15/04/2016
-- ANALISTA    	: DENER VIANA
--------------------------------------------------------------------------------------------------------

--drop table SISTEMAS_SUBMENUS;
--drop sequence SISTEMAS_SUBMENUS_ID;

CREATE TABLE SISTEMAS_SUBMENUS (																	-- Cria a tabela trechos
  SUB_ID							NUMBER							NOT NULL PRIMARY KEY,	-- Identificador do submenu
  MEN_ID							NUMBER							NOT NULL,							-- Identificador do menu
  SUB_POSICAO					NUMBER							NOT NULL,							-- Posição do submenu 
  SUB_NOME						VARCHAR2(070 BYTE)	NOT NULL,							-- Nome do submenu
  SUB_ICONE_MOUSE_ON	VARCHAR2(200 BYTE)	NULL,									-- Endereço e nome do arquivo. Mouse posicionado no icone
  SUB_ICONE_MOUSE_OFF	VARCHAR2(200 BYTE)	NULL,									-- Endereço e nome do arquivo. Mouse posicionado fora do icone  
	SUB_ARQUIVO					VARCHAR2(200 BYTE)	NULL,									-- Endereço e nome da página
	SUB_POPUP						VARCHAR2(1 BYTE)		NOT NULL,							-- Status [ S = Sim   | N = Não ]
  SUB_ATIVO						VARCHAR2(1 BYTE)		NOT NULL);						-- Status [ S = Ativo | N = Inativo ]
 
CREATE SEQUENCE SISTEMAS_SUBMENUS_ID															-- Cria sequence (contador) da tabela
  START WITH 1                                                                                                    
  MAXVALUE 999999999999999999999999999                                                    
  MINVALUE 1
  NOCYCLE
  CACHE 20
  NOORDER;
  

INSERT INTO SISTEMAS_SUBMENUS (SUB_ID, MEN_ID, SUB_POSICAO, SUB_NOME, SUB_ICONE_MOUSE_ON, SUB_ICONE_MOUSE_OFF, SUB_ARQUIVO, SUB_POPUP, SUB_ATIVO) VALUES (SISTEMAS_MENUS_ID.NEXTVAL, 1, 1, 'Consulta', null, null, null, 'N', 'S');
INSERT INTO SISTEMAS_SUBMENUS (SUB_ID, MEN_ID, SUB_POSICAO, SUB_NOME, SUB_ICONE_MOUSE_ON, SUB_ICONE_MOUSE_OFF, SUB_ARQUIVO, SUB_POPUP, SUB_ATIVO) VALUES (SISTEMAS_MENUS_ID.NEXTVAL, 1, 2, 'Relatório VMA', null, null, null, 'N', 'S');
INSERT INTO SISTEMAS_SUBMENUS (SUB_ID, MEN_ID, SUB_POSICAO, SUB_NOME, SUB_ICONE_MOUSE_ON, SUB_ICONE_MOUSE_OFF, SUB_ARQUIVO, SUB_POPUP, SUB_ATIVO) VALUES (SISTEMAS_MENUS_ID.NEXTVAL, 2, 1, 'Abreviaturas', null, null, null, 'N', 'S');
INSERT INTO SISTEMAS_SUBMENUS (SUB_ID, MEN_ID, SUB_POSICAO, SUB_NOME, SUB_ICONE_MOUSE_ON, SUB_ICONE_MOUSE_OFF, SUB_ARQUIVO, SUB_POPUP, SUB_ATIVO) VALUES (SISTEMAS_MENUS_ID.NEXTVAL, 2, 2, 'Banners', null, null, null, 'N', 'S');
INSERT INTO SISTEMAS_SUBMENUS (SUB_ID, MEN_ID, SUB_POSICAO, SUB_NOME, SUB_ICONE_MOUSE_ON, SUB_ICONE_MOUSE_OFF, SUB_ARQUIVO, SUB_POPUP, SUB_ATIVO) VALUES (SISTEMAS_MENUS_ID.NEXTVAL, 2, 3, 'Display', null, null, null, 'N', 'S');
INSERT INTO SISTEMAS_SUBMENUS (SUB_ID, MEN_ID, SUB_POSICAO, SUB_NOME, SUB_ICONE_MOUSE_ON, SUB_ICONE_MOUSE_OFF, SUB_ARQUIVO, SUB_POPUP, SUB_ATIVO) VALUES (SISTEMAS_MENUS_ID.NEXTVAL, 2, 4, 'Downloads', null, null, null, 'N', 'S');
INSERT INTO SISTEMAS_SUBMENUS (SUB_ID, MEN_ID, SUB_POSICAO, SUB_NOME, SUB_ICONE_MOUSE_ON, SUB_ICONE_MOUSE_OFF, SUB_ARQUIVO, SUB_POPUP, SUB_ATIVO) VALUES (SISTEMAS_MENUS_ID.NEXTVAL, 2, 5, 'Importa Corredores', null, null, null, 'N', 'S');
