(function () {
    $(function () {

        var _$recipientsTable = $('#RecipientsTable');
        var _recipientsService = abp.services.app.recipients;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Recipients.Create'),
            edit: abp.auth.hasPermission('Pages.Recipients.Edit'),
            'delete': abp.auth.hasPermission('Pages.Recipients.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Recipients/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Recipients/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditRecipientModal'
        });

		 var _viewRecipientModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Recipients/ViewrecipientModal',
            modalClass: 'ViewRecipientModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$recipientsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _recipientsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#RecipientsTableFilter').val(),
					typeFilter: $('#TypeFilterId').val(),
					firstNameFilter: $('#FirstNameFilterId').val(),
					lastNameFilter: $('#LastNameFilterId').val(),
					emailFilter: $('#EmailFilterId').val(),
					isSignerFilter: $('#IsSignerFilterId').val(),
					codeFilter: $('#CodeFilterId').val(),
					minViewDateFilter:  getDateFilter($('#MinViewDateFilterId')),
					maxViewDateFilter:  getDateFilter($('#MaxViewDateFilterId')),
					minSignatureDateFilter:  getDateFilter($('#MinSignatureDateFilterId')),
					maxSignatureDateFilter:  getDateFilter($('#MaxSignatureDateFilterId')),
					signerPinFilter: $('#SignerPinFilterId').val(),
					isSignedFilter: $('#IsSignedFilterId').val(),
					minSigneOrderFilter: $('#MinSigneOrderFilterId').val(),
					maxSigneOrderFilter: $('#MaxSigneOrderFilterId').val(),
					fieldNameFilter: $('#FieldNameFilterId').val(),
					minSentDateFilter:  getDateFilter($('#MinSentDateFilterId')),
					maxSentDateFilter:  getDateFilter($('#MaxSentDateFilterId')),
					isSentFilter: $('#IsSentFilterId').val(),
					userNameFilter: $('#UserNameFilterId').val(),
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
                                text: app.localize('View'),
                                action: function (data) {
                                    _viewRecipientModal.open({ id: data.record.recipient.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.recipient.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteRecipient(data.record.recipient);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "recipient.type",
						 name: "type"   ,
						render: function (type) {
							return app.localize('Enum_RecipientType_' + type);
						}
			
					},
					{
						targets: 2,
						 data: "recipient.firstName",
						 name: "firstName"   
					},
					{
						targets: 3,
						 data: "recipient.lastName",
						 name: "lastName"   
					},
					{
						targets: 4,
						 data: "recipient.email",
						 name: "email"   
					},
					{
						targets: 5,
						 data: "recipient.isSigner",
						 name: "isSigner"  ,
						render: function (isSigner) {
							if (isSigner) {
								return '<div class="text-center"><i class="fa fa-check kt--font-success" title="True"></i></div>';
							}
							return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
					}
			 
					},
					{
						targets: 6,
						 data: "recipient.code",
						 name: "code"   
					},
					{
						targets: 7,
						 data: "recipient.viewDate",
						 name: "viewDate" ,
					render: function (viewDate) {
						if (viewDate) {
							return moment(viewDate).format('L');
						}
						return "";
					}
			  
					},
					{
						targets: 8,
						 data: "recipient.signatureDate",
						 name: "signatureDate" ,
					render: function (signatureDate) {
						if (signatureDate) {
							return moment(signatureDate).format('L');
						}
						return "";
					}
			  
					},
					{
						targets: 9,
						 data: "recipient.signerPin",
						 name: "signerPin"   
					},
					{
						targets: 10,
						 data: "recipient.isSigned",
						 name: "isSigned"  ,
						render: function (isSigned) {
							if (isSigned) {
								return '<div class="text-center"><i class="fa fa-check kt--font-success" title="True"></i></div>';
							}
							return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
					}
			 
					},
					{
						targets: 11,
						 data: "recipient.signeOrder",
						 name: "signeOrder"   
					},
					{
						targets: 12,
						 data: "recipient.fieldName",
						 name: "fieldName"   
					},
					{
						targets: 13,
						 data: "recipient.sentDate",
						 name: "sentDate" ,
					render: function (sentDate) {
						if (sentDate) {
							return moment(sentDate).format('L');
						}
						return "";
					}
			  
					},
					{
						targets: 14,
						 data: "recipient.isSent",
						 name: "isSent"  ,
						render: function (isSent) {
							if (isSent) {
								return '<div class="text-center"><i class="fa fa-check kt--font-success" title="True"></i></div>';
							}
							return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
					}
			 
					},
					{
						targets: 15,
						 data: "userName" ,
						 name: "userFk.name" 
					},
					{
						targets: 16,
						 data: "documentRequestDocumentTitle" ,
						 name: "documentRequestFk.documentTitle" 
					}
            ]
        });

        function getRecipients() {
            dataTable.ajax.reload();
        }

        function deleteRecipient(recipient) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _recipientsService.delete({
                            id: recipient.id
                        }).done(function () {
                            getRecipients(true);
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

        $('#CreateNewRecipientButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _recipientsService
                .getRecipientsToExcel({
				filter : $('#RecipientsTableFilter').val(),
					typeFilter: $('#TypeFilterId').val(),
					firstNameFilter: $('#FirstNameFilterId').val(),
					lastNameFilter: $('#LastNameFilterId').val(),
					emailFilter: $('#EmailFilterId').val(),
					isSignerFilter: $('#IsSignerFilterId').val(),
					codeFilter: $('#CodeFilterId').val(),
					minViewDateFilter:  getDateFilter($('#MinViewDateFilterId')),
					maxViewDateFilter:  getDateFilter($('#MaxViewDateFilterId')),
					minSignatureDateFilter:  getDateFilter($('#MinSignatureDateFilterId')),
					maxSignatureDateFilter:  getDateFilter($('#MaxSignatureDateFilterId')),
					signerPinFilter: $('#SignerPinFilterId').val(),
					isSignedFilter: $('#IsSignedFilterId').val(),
					minSigneOrderFilter: $('#MinSigneOrderFilterId').val(),
					maxSigneOrderFilter: $('#MaxSigneOrderFilterId').val(),
					fieldNameFilter: $('#FieldNameFilterId').val(),
					minSentDateFilter:  getDateFilter($('#MinSentDateFilterId')),
					maxSentDateFilter:  getDateFilter($('#MaxSentDateFilterId')),
					isSentFilter: $('#IsSentFilterId').val(),
					userNameFilter: $('#UserNameFilterId').val(),
					documentRequestDocumentTitleFilter: $('#DocumentRequestDocumentTitleFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditRecipientModalSaved', function () {
            getRecipients();
        });

		$('#GetRecipientsButton').click(function (e) {
            e.preventDefault();
            getRecipients();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getRecipients();
		  }
		});
    });
})();