//Enmascarar
$(document).ready(function () {

    // Numero de tarjeta - Permitir 0-9 y * para enmascarar
    $('#MainContent_tbxCardNbr').inputmask({
        mask: '9{4} 9{4} 9{4} 9{4}',
        definitions: {
            '9': {
                validator: "[0-9*]",
            }
        },
        placeholder: ' ',
        showMaskOnHover: false,
    });

    // Fecha Cadu. - Permitir solo numeros
    $('#MainContent_tbxFC').inputmask({
        mask: '99/99',
        placeholder: 'MM/AA',
        showMaskOnHover: false
    });

    //CVC - Permitir solo numeros y * para enmascarar
    $('#MainContent_tbxCVC').inputmask({
        mask: '999',
        definitions: {
            '9': {
                validator: "[0-9*]",
            }
        },
        placeholder: ' ',
        showMaskOnHover: false,
    });
});