layui.define([], function (exports) {
    "use strict";

    var fullcalendar = {
        init: function (element,config) {
            return new FullCalendar.Calendar(element, config);
        }
    }

    exports('fullcalendar', fullcalendar);
});