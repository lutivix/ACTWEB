select * 
    from mensagens_enviadas 
    where me_mac_num in (61) 
        and substr(me_text, 0,4) = '_CCE'
        and me_msg_time >= to_date('01/01/2016 00:00:00', 'dd/mm/yyyy hh24:mi:ss') 
        and me_msg_time <= to_date('31/01/2016 23:59:59', 'dd/mm/yyyy hh24:mi:ss');