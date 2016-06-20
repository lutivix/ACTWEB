alter table actpp.mensagens_enviadas add (ME_KM VARCHAR2(14 BYTE) NULL);
alter table actpp.mensagens_enviadas add (ME_CORREDOR VARCHAR2(14 BYTE) NULL);
alter table actpp.mensagens_enviadas add (ME_NOME_SB VARCHAR2(14 BYTE) NULL);

-- Chamado 412 - Relatório de Macro 50
-- Dener Viana
-- 23/09/15
alter table actpp.mensagens_enviadas add (ME_MAT_OPER VARCHAR2(20 BYTE) DEFAULT 0);
alter table actpp.mensagens_enviadas add (ME_MSG_LIDA DATE NULL);
alter table actpp.mensagens_enviadas add (ME_MSG_RESP DATE NULL);


CREATE TRIGGER TRG_MACROS_50_E BEFORE INSERT ON MENSAGENS_ENVIADAS FOR EACH ROW
BEGIN
    IF :NEW.ME_MAC_NUM = 50 THEN
        IF :NEW.ME_MAT_OPER IS NULL THEN
           :NEW.ME_MAT_OPER := '0';
        END IF;
    END IF;
END;
/