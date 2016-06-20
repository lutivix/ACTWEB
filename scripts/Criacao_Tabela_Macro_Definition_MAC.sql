CREATE TABLE MACRODEFINITION_MAC
(
  MAC_Num_Id                INTEGER,
  ACC_Num_AccountNumber     INTEGER,
  MAC_Num_MacroNumber       INTEGER,
  MAC_Num_MacroVersion      INTEGER,
  MAC_Bol_IsForward         INTEGER,
  MAC_Bol_AutoPrint         INTEGER,
  MAC_Nom_MacroName         VARCHAR2(25 CHAR),
  MAC_Des_MacroDescription  VARCHAR2(300 CHAR),
  MAC_Dtm_CreateDate        DATE,
  MAC_Dtm_DefinitionDate    DATE,
  MAC_Txt_MacroDefinition   VARCHAR2(3000 CHAR),
  MAC_Num_DataType          INTEGER,
  MAC_Num_MacroType         INTEGER,
  MAC_Num_ReplyMacro        INTEGER,
  MAC_Num_Color             INTEGER,
  MAC_Num_IconId            INTEGER,
  MAC_Bol_Forward           INTEGER
)
TABLESPACE ACTWEB
PCTUSED    40
PCTFREE    10
INITRANS   1
MAXTRANS   255
STORAGE    (
            INITIAL          64K
            MINEXTENTS       1
            MAXEXTENTS       2147483645
            PCTINCREASE      0
            FREELISTS        1
            FREELIST GROUPS  1
            BUFFER_POOL      DEFAULT
           )
LOGGING 
NOCOMPRESS 
NOCACHE
NOPARALLEL
MONITORING;


ALTER TABLE MACRODEFINITION_MAC ADD (
  PRIMARY KEY
 ("MAC_Num_Id")
    USING INDEX 
    TABLESPACE ACTWEB
    PCTFREE    10
    INITRANS   2
    MAXTRANS   255
    STORAGE    (
                INITIAL          64K
                MINEXTENTS       1
                MAXEXTENTS       2147483645
                PCTINCREASE      0
                FREELISTS        1
                FREELIST GROUPS  1
               ));