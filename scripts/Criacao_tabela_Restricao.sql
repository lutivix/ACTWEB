select count(*) from Restricao;
select * from Restricao;

drop table actweb.Restricao;
drop sequence actweb.Restricao_id;

CREATE TABLE actweb.Restricao
(
  RE_RestricaoID      number            not null,
  RE_Secao_Elemento   varchar2(30 byte) null,
  RE_Secao_ElementoID number            null,
  RE_Tipo_Restricao   varchar2(30 byte) null,
  RE_Tipo_RestricaoID number            null,
  RE_SubTipo_VR       varchar2(30 byte) null,
  RE_SubTipo_VRID     number            null,
  RE_Data_Inicial     date              null,
  RE_Data_Final       date              null,
  RE_Km_Inicial       number            null,
  RE_Km_Final         number            null,
  RE_Lat_Inicial      varchar2(14 byte) null,
  RE_Lat_Final        varchar2(14 byte) null,
  RE_Lon_Inicial      varchar2(14 byte) null,
  RE_Lon_Final        varchar2(14 byte) null,
  RE_Duracao          number            null,  
  RE_Velocidade       number            null,
  RE_Responsavel      varchar2(50 byte) null,
  RE_Obsercacao       varchar2(50 byte) null
);

ALTER TABLE actweb.Restricao ADD ( CONSTRAINT PK_Restricao PRIMARY KEY (RE_RestricaoID) );

CREATE SEQUENCE actweb.Restricao_ID
  START WITH 1
  MAXVALUE 999999999999999999999999999
  MINVALUE 1
  NOCYCLE
  CACHE 20
  NOORDER;
  
  