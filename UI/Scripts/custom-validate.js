/**
 * Localized default methods for the jQuery validation plugin.
 * Locale: pt-BR
 */
$.extend($.validator.methods, {
  number: function (value, element) {
    return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:[\s\.,]\d{3})+)(?:[\.,]\d+)?$/.test(value);
  }
});
