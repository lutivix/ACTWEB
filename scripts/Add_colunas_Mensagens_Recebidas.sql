alter table actpp.mensagens_recebidas add (MR_KM VARCHAR2(14 BYTE) NULL);
alter table actpp.mensagens_recebidas add (MR_CORREDOR VARCHAR2(14 BYTE) NULL);
alter table actpp.mensagens_recebidas add (MR_NOME_SB VARCHAR2(14 BYTE) NULL);

-- Chamado 412 - Relatório de Macro 50
-- Dener Viana
-- 23/09/15
alter table actpp.mensagens_recebidas add (MR_MAT_OPER VARCHAR2(20 BYTE) DEFAULT 0);
alter table actpp.mensagens_recebidas add (MR_MSG_LIDA DATE NULL);
alter table actpp.mensagens_recebidas add (MR_MSG_RESP DATE NULL);


CREATE TRIGGER TRG_MACROS_50_R BEFORE INSERT ON MENSAGENS_RECEBIDAS FOR EACH ROW
BEGIN
    IF :NEW.MR_MC_NUM = 50 THEN
        IF :NEW.MR_MAT_OPER IS NULL THEN
           :NEW.MR_MAT_OPER := '0';
        END IF;
    END IF;
END;
/