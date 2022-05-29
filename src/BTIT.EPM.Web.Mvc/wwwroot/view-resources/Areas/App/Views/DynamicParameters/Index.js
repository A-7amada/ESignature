(function () {
    $(function () {
        var _table = $('#DynamicParametersTable');
        var _dynamicParametersAppService = abp.services.app.dynamicParameter;

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/DynamicParameter/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/DynamicParameters/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditDynamicParameterModal',
            cssClass: 'scrollable-modal'
        });

        var dataTable = _table.DataTable({
            paging: false,
            serverSide: false,
            processing: false,
            listAction: {
                ajaxFunction: _dynamicParametersAppService.getAll,
            },
            columnDefs: [
                {
                    className: 'control responsive',
                    orderable: false,
                    render: function () {
                        return '';
                    },
                    targets: 0
                },
                {
                    targets: 1,
                    data: "parameterName",
                },
                {
                    targets: 2,
                    data: "inputType",
                },
                {
                    targets: 3,
                    data: "permission",
                }
            ],
            drawCallback: function (settings) {
                _table.find('tbody tr').css("cursor", "pointer");
            }
        });

        _table.find('tbody').on('click', 'tr', function () {
            var data = dataTable.row(this).data();
            if (data) {
                window.location = "/App/DynamicParameter/Detail/" + data.id;
            }
        });

        $('#CreateNewDynamicParameter').click(function () {
            _createOrEditModal.open();
        });

        $('#GetDynamicParametersButton').click(function (e) {
            e.preventDefault();
            getDynamicParameters();
        });

        function getDynamicParameters() {
            dataTable.ajax.reload();
        }

        abp.event.on('app.createOrEditDynamicParametersModalSaved', function () {
            getDynamicParameters();
        });
    });
})();
