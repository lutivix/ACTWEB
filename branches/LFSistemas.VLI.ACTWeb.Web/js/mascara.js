//adiciona mascara de data
function MascaraData(data) {
    if (mascaraInteiro(data) == false) {
        event.returnValue = false;
    }
    return formataCampo(data, '00/00/0000', event);
}
// permite somente numeros
function PermiteSomenteNumeros(event) {
    var charCode = (event.which) ? event.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}

//permite numeros e virgula
function fnValidaNroVirgula(e) {
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
        if ((charCode == 8) || (charCode == 44)) {
            return;
        }
        else {
            return false;
        }
    }
}


// permite colocar a mascara da maneira que voce quiser
function formatar(src, mask) {
    var i = src.value.length;
    var saida = mask.substring(0, 1);
    var texto = mask.substring(i)
    if (texto.substring(0, 1) != saida) {
        src.value += texto.substring(0, 1);
    }
}

function validateEmail(email) {
    var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}
function ValidaEmail() {
    var obj = eval("document.forms[0].Email");
    var txt = obj.value;
    if ((txt.length != 0) && ((txt.indexOf("@") < 1) || (txt.indexOf('.') < 7))) {
        alert('Email incorreto');
        obj.focus();
    }
}
function numeros() {

    if (document.all) // Internet Explorer  
        var tecla = event.keyCode;
    else if (document.layers) // Nestcape  
        var tecla = e.which;

    if ((tecla > 47 && tecla < 58)) // numeros de 0 a 9  
        return true;
    else {
        if (tecla != 8) // backspace  
            //event.keyCode = 0;  
            return false;
        else
            return true;
    }
}

