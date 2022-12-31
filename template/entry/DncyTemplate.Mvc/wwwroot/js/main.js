(function ($) {
    //serializeFormToObject plugin for jQuery
    $.fn.serializeFormToObject = function (camelCased = false) {
        //serialize to array
        var data = $(this).serializeArray();

        //add also disabled items
        $(':disabled[name]', this).each(function () {
            data.push({ name: this.name, value: $(this).val() });
        });

        //map to object
        var obj = {};
        data.map(function (x) { obj[x.name] = x.value; });

        if (camelCased && camelCased === true) {
            return convertToCamelCasedObject(obj);
        }

        return obj;
    };

    function convertToCamelCasedObject(obj) {
        var newObj, origKey, newKey, value;
        if (obj instanceof Array) {
            return obj.map(value => {
                if (typeof value === 'object') {
                    value = convertToCamelCasedObject(value);
                }
                return value;
            });
        } else {
            newObj = {};
            for (origKey in obj) {
                if (obj.hasOwnProperty(origKey)) {
                    newKey = (
                        origKey.charAt(0).toLowerCase() + origKey.slice(1) || origKey
                    ).toString();
                    value = obj[origKey];
                    if (
                        value instanceof Array ||
                        (value !== null && value.constructor === Object)
                    ) {
                        value = convertToCamelCasedObject(value);
                    }
                    newObj[newKey] = value;
                }
            }
        }
        return newObj;
    }

    $.fn.clearForm = function () {
        var $this = $(this);
        $this.validate().resetForm();
        $('[name]', $this).each((i, obj) => {
            $(obj).removeClass('is-invalid');
        });
        $this[0].reset();
    };
})(jQuery);
