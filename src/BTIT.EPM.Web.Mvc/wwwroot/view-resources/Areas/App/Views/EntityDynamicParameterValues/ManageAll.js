(function () {
    $(function () {
        var _table = $('#EntityDynamicParameterValuesTable');
        var _entityDynamicParameterValueAppService = abp.services.app.entityDynamicParameterValue;
        var dataAndInputTypes = [];

        var _permissions = {
            edit: abp.auth.hasPermission('Pages.Administration.EntityDynamicParameterValue.Edit'),
            delete: abp.auth.hasPermission('Pages.Administration.EntityDynamicParameterValue.Delete')
        };

        function initializePage() {
            abp.ui.setBusy();

            _entityDynamicParameterValueAppService
                .getAllEntityDynamicParameterValues({
                    entityFullName: $("#EntityFullName").val(),
                    entityId: $("#EntityId").val()
                })
                .done(function (data) {
                    if (!data || !data.items || data.items.length == 0) {
                        return;
                    }

                    var body = _table.find("tbody");
                    for (var i = 0; i < data.items.length; i++) {
                        var item = data.items[i];
                        var inputTypeManager = abp.inputTypeProviders.getInputTypeInstance({ inputType: item.inputType });
                        var view = inputTypeManager.getView(item.selectedValues, item.allValuesInputTypeHas);

                        body.append(
                            $("<tr></tr>")
                                .append(
                                    $("<td></td>").text(item.parameterName)
                                )
                                .append(
                                    $("<td></td>").append(view)
                                )
                                .append(
                                    $("<td></td>")
                                        .append(_permissions.delete ?
                                            ($("<button data-index=\"" + i + "\" class=\"btn btn-danger\"></button>")
                                                .text(app.localize("Delete"))
                                                .click(function () {
                                                    var index = $(this).data("index");
                                                    deleteAllValuesOfEntityDynamicParameterId({
                                                        dynamicParameterName: data.items[index].parameterName,
                                                        entityDynamicParameterId: data.items[index].entityDynamicParameterId
                                                    });
                                                }))
                                            : null
                                        )

                                )
                        );

                        dataAndInputTypes.push({
                            data: item,
                            inputTypeManager: inputTypeManager
                        });
                    }
                })
                .always(function () {
                    abp.ui.clearBusy();
                });
        }


        function deleteAllValuesOfEntityDynamicParameterId(data) {
            abp.message.confirm(
                app.localize('DeleteEntityDynamicParameterValueMessage', data.dynamicParameterName),
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        abp.ui.setBusy();
                        _entityDynamicParameterValueAppService.cleanValues({
                            entityDynamicParameterId: data.entityDynamicParameterId,
                            entityId: $("#EntityId").val()
                        })
                            .done(function () {
                                abp.notify.success(
                                    app.localize('SuccessfullyDeleted')
                                );
                                window.location.reload();

                            }).always(function () {
                                abp.ui.clearBusy();
                            });
                    }
                }
            );
        }

        function saveParameters() {
            if (!dataAndInputTypes) {
                return;
            }

            abp.ui.setBusy();

            var newValues = [];
            for (var i = 0; i < dataAndInputTypes.length; i++) {
                newValues.push({
                    entityId: $("#EntityId").val(),
                    entityDynamicParameterId: dataAndInputTypes[i].data.entityDynamicParameterId,
                    values: dataAndInputTypes[i].inputTypeManager.getSelectedValues()
                });
            }

            _entityDynamicParameterValueAppService.insertOrUpdateAllValues({ Items: newValues })
                .done(function () {
                    abp.notify.success(app.localize("SavedSuccessfully"));
                }).always(function () {
                    abp.ui.clearBusy();
                });
        }

        $("#saveParameters").click(saveParameters);

        initializePage();
    });
})();
