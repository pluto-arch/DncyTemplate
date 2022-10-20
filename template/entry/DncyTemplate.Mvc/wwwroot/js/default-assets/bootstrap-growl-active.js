(function ($) {
    "use strict";

    if (!$.cookie('greetings')) {
        notify('Hi, Welcomeback! - Admetro Dashboard', 'inverse');
        $.cookie('greetings', 'yes', { expires: 7, path: '/' });
    }
    
})(jQuery);