/*
 * Localized default methods for the jQuery validation plugin.
 * Locale: PT_BR
 * Incluir aqui validações customizadas e adaptações de moeda, data, etc
 */
jQuery.extend(jQuery.validator.methods, {
    date: function (value, element) {
        return this.optional(element) || /^\d\d?\/\d\d?\/\d\d\d?\d?$/.test(value);
    },
    number: function (value, element) {
        return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:\.\d{3})+)(?:,\d+)?$/.test(value);
    },
    
});

// Validação do CPF
$.validator.addMethod("cpf", function (value, element, param) {
    var i;
    var l = '';
    for (i = 0; i < value.length; i++) if (!isNaN(value.charAt(i))) l += value.charAt(i);
    value = l;
    if (value.length != 11) return false;
    var c = value.substr(0, 9);
    var dv = value.substr(9, 2);
    var d1 = 0;
    for (i = 0; i < 9; i++) d1 += c.charAt(i) * (10 - i);
    if (d1 == 0) return false;
    d1 = 11 - (d1 % 11);
    if (d1 > 9) d1 = 0;
    if (dv.charAt(0) != d1) return false;
    d1 *= 2;
    for (i = 0; i < 9; i++) d1 += c.charAt(i) * (11 - i)
    d1 = 11 - (d1 % 11);
    if (d1 > 9) d1 = 0;
    if (dv.charAt(1) != d1) return false;
    return true;
});
$.validator.unobtrusive.adapters.addBool("cpf");
