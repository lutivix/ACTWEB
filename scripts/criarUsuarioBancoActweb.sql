CREATE TABLE USUARIO
(
  ID             INTEGER,
  MATRICULA      VARCHAR2(70 BYTE),
  NOME           VARCHAR2(70 BYTE),
  SENHA          VARCHAR2(70 BYTE),
  NIVEL          INTEGER,
  MALETA         INTEGER,
  EMAIL          CHAR(70 BYTE),
  ACESSOS        INTEGER,
  DATACRIACAO    DATE,
  DATAALTERACAO  DATE
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



ALTER TABLE USUARIO ADD (
  PRIMARY KEY
 ("MATRICULA")
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
