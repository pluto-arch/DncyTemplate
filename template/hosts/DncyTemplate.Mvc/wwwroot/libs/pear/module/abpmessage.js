layui.define(['jquery', 'abpcore', 'layer'], function (exports) {
    "use strict";

    var $ = layui.jquery;
    var abp = layui.abpcore;

    /* MESSAGE **************************************************/

    var showMessage = function (message, title, callback, icon) {
        var option = {
            icon: icon,
            closeBtn: 0,
            end: callback
        };

        if (title) {
            option.title = title;
        }

        return parent.layer.alert(message, option);
    };

    abp.message.warn = function (message, title) {
        return showMessage(message, title, null, 0);
    };

    abp.message.success = function (message, title) {
        return showMessage(message, title, null, options, 1);
    };

    abp.message.error = function (message, title) {
        return showMessage(message, title, null, 2);
    };

    abp.message.confirm = function (message, title, callback) {
        return showMessage(message, title, callback, 3);
    };

    abp.message.info = function (message, title) {
        return showMessage(message, title, null, 6);
    };

    exports('abpmessage', abp);
});