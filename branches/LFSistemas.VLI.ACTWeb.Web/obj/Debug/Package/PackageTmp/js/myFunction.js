function fnValidaNroDoisPontos(e) {
    var charCode;

    if (e && e.which) {
        charCode = e.which;
    }
    else if (window.event) {
        e = window.event;
        charCode = e.keyCode;
    }
    if ((charCode >= 48) && (charCode <= 57)) {
        return;
    }
    else {
        if ((charCode == 8) || (!charCode == 46) || (!charCode == 47)) {
            return;
        }
        else {
            return false;
        }
    }
}
function validaData(campo, valor) {
    data = new Date();
    var date=valor;
    var ardt=new Array;
    var ExpReg=new RegExp("(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[012])/[12][0-9]{3}");
    ardt=date.split("/");
    erro=false;
    if ( date.search(ExpReg)==-1){
        erro = true;
    }
    else if (((ardt[1]==4)||(ardt[1]==6)||(ardt[1]==9)||(ardt[1]==11))&&(ardt[0]>30))
        erro = true;
    else if ( ardt[1]==2) {
        if ((ardt[0]>28)&&((ardt[2]%4)!=0))
            erro = true;
        if ((ardt[0]>29)&&((ardt[2]%4)==0))
            erro = true;
    }
    if (erro) {
        alert('Informe uma data válida!!!');
        campo.focus();
        campo.value = "";
        return false;
    }
    return true;
}
