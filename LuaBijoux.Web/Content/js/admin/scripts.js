/* Main JS */

$(document).ready(function () {
    // Configurações do plugin 'datepicker bootstrap' - configurações para data de nascimento
    $('.birthdate').datepicker({
        format: 'dd/mm/yyyy',
        autoclose: true,
        endDate: new Date(),
        language: 'pt-BR',
        startDate: "01/01/1900",
        startView: 2,
        onClose: function () {
            $('.birthdate').valid();
        }
    });

    // Configurações do plugin 'masked input plugin'
    $(".datepicker").mask("99/99/9999");
    $(".cpf").mask("999.999.999-99");
    $(".phone").mask("(99) 9999-9999?9");
});
