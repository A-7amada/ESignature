(function () {
    $(function () {

        var _$documentRequestAuditTrailsTable = $('#DocumentRequestAuditTrailsTable');
        var _documentRequestAuditTrailsService = abp.services.app.documentRequestAuditTrails;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.DocumentRequestAuditTrails.Create'),
            edit: abp.auth.hasPermission('Pages.DocumentRequestAuditTrails.Edit'),
            'delete': abp.auth.hasPermission('Pages.DocumentRequestAuditTrails.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/DocumentRequestAuditTrails/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/DocumentRequestAuditTrails/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditDocumentRequestAuditTrailModal'
        });

		 var _viewDocumentRequestAuditTrailModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/DocumentRequestAuditTrails/ViewdocumentRequestAuditTrailModal',
            modalClass: 'ViewDocumentRequestAuditTrailModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$documentRequestAuditTrailsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _documentRequestAuditTrailsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#DocumentRequestAuditTrailsTableFilter').val(),
					typeFilter: $('#TypeFilterId').val(),
					clientIpAddressFilter: $('#ClientIpAddressFilterId').val(),
					documentRequestDocumentTitleFilter: $('#DocumentRequestDocumentTitleFilterId').val(),
					recipientFirstNameFilter: $('#RecipientFirstNameFilterId').val()
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
                                text: app.localize('View'),
                                action: function (data) {
                                    _viewDocumentRequestAuditTrailModal.open({ id: data.record.documentRequestAuditTrail.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.documentRequestAuditTrail.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteDocumentRequestAuditTrail(data.record.documentRequestAuditTrail);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "documentRequestAuditTrail.type",
						 name: "type"   ,
						render: function (type) {
							return app.localize('Enum_AuditTrailType_' + type);
						}
			
					},
					{
						targets: 2,
						 data: "documentRequestAuditTrail.clientIpAddress",
						 name: "clientIpAddress"   
					},
					{
						targets: 3,
						 data: "documentRequestDocumentTitle" ,
						 name: "documentRequestFk.documentTitle" 
					},
					{
						targets: 4,
						 data: "recipientFirstName" ,
						 name: "recipientFk.firstName" 
					}
            ]
        });

        function getDocumentRequestAuditTrails() {
            dataTable.ajax.reload();
        }

        function deleteDocumentRequestAuditTrail(documentRequestAuditTrail) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _documentRequestAuditTrailsService.delete({
                            id: documentRequestAuditTrail.id
                        }).done(function () {
                            getDocumentRequestAuditTrails(true);
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

        $('#CreateNewDocumentRequestAuditTrailButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _documentRequestAuditTrailsService
                .getDocumentRequestAuditTrailsToExcel({
				filter : $('#DocumentRequestAuditTrailsTableFilter').val(),
					typeFilter: $('#TypeFilterId').val(),
					clientIpAddressFilter: $('#ClientIpAddressFilterId').val(),
					documentRequestDocumentTitleFilter: $('#DocumentRequestDocumentTitleFilterId').val(),
					recipientFirstNameFilter: $('#RecipientFirstNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditDocumentRequestAuditTrailModalSaved', function () {
            getDocumentRequestAuditTrails();
        });

		$('#GetDocumentRequestAuditTrailsButton').click(function (e) {
            e.preventDefault();
            getDocumentRequestAuditTrails();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getDocumentRequestAuditTrails();
		  }
		});
    });
})();