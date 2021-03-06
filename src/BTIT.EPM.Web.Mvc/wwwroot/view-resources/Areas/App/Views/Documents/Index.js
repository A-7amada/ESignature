(function () {
    $(function () {

        var _$documentsTable = $('#DocumentsTable');
        var _documentsService = abp.services.app.documents;

        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Documents.Create'),
            edit: abp.auth.hasPermission('Pages.Documents.Edit'),
            'delete': abp.auth.hasPermission('Pages.Documents.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Documents/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Documents/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditDocumentModal'
        });

        var _viewDocumentModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Documents/ViewdocumentModal',
            modalClass: 'ViewDocumentModal'
        });

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        }

        var dataTable = _$documentsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _documentsService.getUserDocuments,
                inputFilter: function () {
                    return {
                        filter: $('#DocumentsTableFilter').val(),
                        fileNameFilter: $('#FileNameFilterId').val(),
                        //  extensionFilter: $('#ExtensionFilterId').val(),
                        //  minSizeFilter: $('#MinSizeFilterId').val(),
                        //   maxSizeFilter: $('#MaxSizeFilterId').val(),
                        //   contentTypeFilter: $('#ContentTypeFilterId').val(),
                        //   isActiveFilter: $('#IsActiveFilterId').val(),
                        //   binaryObjectTenantIdFilter: $('#BinaryObjectTenantIdFilterId').val(),
                        documentRequestDocumentTitleFilter: $('#DocumentRequestDocumentTitleFilterId').val()
                    };
                }
            },
            columnDefs: [
                {
                    width: 120,
                    targets: 0,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    defaultContent: '',
                    rowAction: {
                        cssClass: 'btn btn-brand dropdown-toggle',
                        text: '<i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span>',
                        items: [
                            {
                                text: app.localize('AuditTrail'),
                                action: function (data) {
                                    //ToDo: add action
                                    window.location.href = abp.appPath + 'App/DocumentRequestAuditTrails/DocumentRequestAuditTrail?documentRequestId='
                                        + data.record.document.documentRequestId + "&documentId=" + data.record.document.id;
                                }
                            },
                            {
                                text: app.localize('Continue'),
                                visible: function (data) {
                                    return data.record.document.status == 0;
                                },
                                action: function (data) {
                                    //ToDo: add action
                                }
                            },
                            {
                                text: app.localize('Delete'),
                                visible: function (data) {
                                    return (data.record.document.status == 0
                                        && _permissions.delete);
                                },
                                action: function (data) {
                                    deleteDocument(data.record.document);
                                }
                            },
                            {
                                text: app.localize('SendReminder'),
                                visible: function (data) {
                                    return data.record.document.status == 1;
                                },
                                action: function (data) {
                                    //ToDo: add action
                                }
                            }
                        ]
                    }
                },
                {
                    targets: 1,
                    data: "document",
                    name: "fileName",
                    render: function (data) {
                        var url = abp.appPath + 'App/Documents/DownloadFile?documentId=' + data.id;
                        return "<a href=" + url + " target='blank'>" + data.fileName + "</a>"
                    }
                },
                {
                    targets: 2,
                    data: "document.createdDate",
                    name: "extension"
                },
                {
                    targets: 3,
                    data: "document.recipients",
                    name: "size"
                },
                {
                    targets: 4,
                    data: "document.status",
                    name: "status",
                    render: function (status) {
                        return app.localize('Enum_DocumentRequestStatus_' + status);
                    }
                },
                {
                    targets: 5,
                    data: "documentRequestDocumentTitle",
                    name: "documentRequestFk.documentTitle"
                }
            ]
        });

        function getDocuments() {
            dataTable.ajax.reload();
        }

        function deleteDocument(document) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _documentsService.delete({
                            id: document.id
                        }).done(function () {
                            getDocuments(true);
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                        });
                    }
                }
            );
        }

        $('#ShowAdvancedFiltersSpan').click(function () {
            $('#ShowAdvancedFiltersSpan').hide();
            $('#HideAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideDown();
        });

        $('#HideAdvancedFiltersSpan').click(function () {
            $('#HideAdvancedFiltersSpan').hide();
            $('#ShowAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideUp();
        });

        $('#CreateNewDocumentButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportToExcelButton').click(function () {
            _documentsService
                .getDocumentsToExcel({
                    filter: $('#DocumentsTableFilter').val(),
                    fileNameFilter: $('#FileNameFilterId').val(),
                    extensionFilter: $('#ExtensionFilterId').val(),
                    minSizeFilter: $('#MinSizeFilterId').val(),
                    maxSizeFilter: $('#MaxSizeFilterId').val(),
                    contentTypeFilter: $('#ContentTypeFilterId').val(),
                    isActiveFilter: $('#IsActiveFilterId').val(),
                    binaryObjectTenantIdFilter: $('#BinaryObjectTenantIdFilterId').val(),
                    documentRequestDocumentTitleFilter: $('#DocumentRequestDocumentTitleFilterId').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditDocumentModalSaved', function () {
            getDocuments();
        });

        $('#GetDocumentsButton').click(function (e) {
            e.preventDefault();
            getDocuments();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getDocuments();
            }
        });
    });
})();