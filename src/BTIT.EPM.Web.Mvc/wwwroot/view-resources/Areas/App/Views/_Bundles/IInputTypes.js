(function () {
    abp.inputTypeProviders = new function () {
        var _providers = {};

        this.addInputTypeProvider = function (provider) {
            if (!provider) {
                throw new Error("Input type provider can not be null or undefined.");
            }

            if (typeof provider.name !== 'string') {
                throw new Error("Input type provider should have \"name\" property which is same unique name of InputType");
            }

            if (typeof provider.get !== 'function') {
                throw new Error("Input type provider should have \"get\" method which returns new manager for input type.");
            }

            _providers[provider.name] = provider;
        }

        this.getInputTypeInstance = function (args) {
            if (typeof args === "string") {
                return _providers[args].get();
            } else if (typeof args === "object" && typeof args.inputType === "object") {
                var provider = _providers[args.inputType.name].get();
                provider.init(args.inputType, args.options);
                return provider;
            }
            throw new Error("Parameter should be type of string (InputTypeName),or object which includes inputType and options")
        }
    }();
})();
var SingleLineStringInputType = (function () {
    return function () {
        var _inputTypeInfo;
        var _options;
        function init(inputTypeInfo, options) {
            _inputTypeInfo = inputTypeInfo;
            _options = options;
        }
        var $textbox;
        function getView(selectedValues, allItems) {
            var type = 'text';
            if (_inputTypeInfo.validator) {
                if (_inputTypeInfo.validator.name == 'NUMERIC') {
                    type = 'number';
                }
            }
            $textbox = $('<input class="form-control" type="' + type + '" />')
                .on('change', function () {
                    if (_options && typeof (_options.onChange) === "function") {
                        _options.onChange($textbox.val());
                    }
                });

            if (type == 'number') {
                $textbox.attr('min', feature._inputType.validator.minValue);
                $textbox.attr('max', feature._inputType.validator.maxValue);
            } else {
                if (_inputTypeInfo.validator && _inputTypeInfo.validator.name == 'STRING') {
                    if (_inputTypeInfo.validator.maxLength > 0) {
                        $textbox.attr('maxlength', _inputTypeInfo.validator.maxLength);
                    }
                    if (_inputTypeInfo.validator.minLength > 0) {
                        $textbox.attr('required', 'required');
                    }
                    if (_inputTypeInfo.validator.regularExpression) {
                        $textbox.attr('pattern', _inputTypeInfo.validator.regularExpression);
                    }
                }
            }

            $textbox.on('input propertychange paste',
                function () {
                    if (isValueValid()) {
                        $textbox.removeClass('input-textbox-invalid');
                    } else {
                        $textbox.addClass('input-textbox-invalid');
                    }
                });

            if (selectedValues && selectedValues.length > 0) {
                $textbox.val(selectedValues[0]);
            }

            return $textbox[0];
        }

        function isValueValid() {
            value = $textbox.val();
            if (_inputTypeInfo || !_inputTypeInfo.validator) {
                return true;
            }

            var validator = _inputTypeInfo.validator;
            if (validator.name == 'STRING') {
                if (value == undefined || value == null) {
                    return validator.allowNull;
                }

                if (typeof value != 'string') {
                    return false;
                }

                if (validator.minLength > 0 && value.length < validator.minLength) {
                    return false;
                }

                if (validator.maxLength > 0 && value.length > validator.maxLength) {
                    return false;
                }

                if (validator.regularExpression) {
                    return (new RegExp(validator.regularExpression)).test(value);
                }
            } else if (validator.name == 'NUMERIC') {
                var numValue = parseInt(value);

                if (isNaN(numValue)) {
                    return false;
                }

                var minValue = validator.minValue;
                if (minValue > numValue) {
                    return false;
                }

                var maxValue = validator.maxValue;
                if (maxValue > 0 && numValue > maxValue) {
                    return false;
                }
            }

            return true;
        }

        function getSelectedValues() {
            return [$textbox.val()];
        }

        /*
         * {
                name: "",//unique name of InputType (string)
                init: init,//initialize function
                getSelectedValues: getSelectedValues,//function that returns selected value(s) for returned view (returns list of string)
                getView: getView,//function that returns html view for input type (gets parameter named selectedValues)
                hasValues: false //is that input type need values to work. For example dropdown need values to select.
            }
         */
        return {
            name: "SINGLE_LINE_STRING",
            init: init,
            getSelectedValues: getSelectedValues,
            getView: getView,
            hasValues: false//is that input type need values to work. For example dropdown need values to select.
        };
    };
})();

(function () {
    var SingleLineStringInputTypeProvider = new function () {
        this.name = "SINGLE_LINE_STRING";
        this.get = function () {
            return new SingleLineStringInputType();
        }
    }();

    abp.inputTypeProviders.addInputTypeProvider(SingleLineStringInputTypeProvider);
})();
var ComboBoxInputType = (function () {
    return function () {
        var _options;
        function init(inputTypeInfo, options) {
            _options = options;
        }

        var $combobox;
        function getView(selectedValues, allItems) {
            $combobox = $('<select class="form-control" />');
            $('<option></option>').appendTo($combobox);

            if (allItems && allItems.length > 0) {
                for (var i = 0; i < allItems.length; i++) {
                    $('<option></option>')
                        .attr('value', allItems[i])
                        .text(allItems[i])
                        .appendTo($combobox);
                }
            }

            $combobox
                .on('change', function () {
                    if (_options && typeof (_options.onChange) === "function") {
                        _options.onChange($combobox.val());
                    }
                });

            if (selectedValues && selectedValues.length > 0) {
                $combobox.val(selectedValues[0]);
            }

            return $combobox[0];
        }

        function getSelectedValues() {
            return [$combobox.val()];
        }

        /*
         * {
                name: "",//unique name of InputType (string)
                init: init,//initialize function
                getSelectedValues: getSelectedValues,//function that returns selected value(s) for returned view (returns list of string)
                getView: getView,//function that returns html view for input type (gets parameter named selectedValues)
                hasValues: false //is that input type need values to work. For example dropdown need values to select.
            }
         */
        return {
            name: "COMBOBOX",
            init: init,
            getSelectedValues: getSelectedValues,
            getView: getView,
            hasValues: true//is that input type need values to work. For example dropdown need values to select.
        };
    };
})();

(function () {
    var ComboBoxInputTypeProvider = new function () {
        this.name = "COMBOBOX";
        this.get = function () {
            return new ComboBoxInputType();
        }
    }();

    abp.inputTypeProviders.addInputTypeProvider(ComboBoxInputTypeProvider);
})();
var CheckBoxInputType = (function () {
    return function () {
        var _options;
        function init(inputTypeInfo, options) {
            _options = options;
        }

        var $checkbox;
        function getView(selectedValues, allItems) {
            $div = $('<div class="form-group kt-checkbox-list">');
            $label = $('<label class="kt-checkbox kt-checkbox--solid">').appendTo($div)
            $checkbox = $('<input type="checkbox"/>').appendTo($label);
            $span = $('<span></span>').appendTo($label);
            $checkbox
                .on('change', function () {
                    if (_options && typeof (_options.onChange) === "function") {
                        _options.onChange($checkbox.val());
                    }
                });

            if (selectedValues && selectedValues.length > 0) {
                $checkbox.prop("checked", selectedValues[0]);
            }
            return $div[0];
        }

        function getSelectedValues() {
            return [$checkbox.prop("checked")];
        }

        /*
         * {
                name: "",//unique name of InputType (string)
                init: init,//initialize function
                getSelectedValues: getSelectedValues,//function that returns selected value(s) for returned view (returns list of string)
                getView: getView,//function that returns html view for input type (gets parameter named selectedValues)
                hasValues: false //is that input type need values to work. For example dropdown need values to select.
            }
         */
        return {
            name: "CHECKBOX",
            init: init,
            getSelectedValues: getSelectedValues,
            getView: getView,
            hasValues: false//is that input type need values to work. For example dropdown need values to select.
        };
    };
})();

(function () {
    var CheckBoxInputTypeProvider = new function () {
        this.name = "CHECKBOX";
        this.get = function () {
            return new CheckBoxInputType();
        }
    }();

    abp.inputTypeProviders.addInputTypeProvider(CheckBoxInputTypeProvider);
})();