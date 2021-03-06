(function () {
    $(function () {

        var _$contactsTable = $('#ContactsTable');
        var _contactsService = abp.services.app.contacts;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Contacts.Create'),
            edit: abp.auth.hasPermission('Pages.Contacts.Edit'),
            'delete': abp.auth.hasPermission('Pages.Contacts.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Contacts/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Contacts/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditContactModal'
        });

		 var _viewContactModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Contacts/ViewcontactModal',
            modalClass: 'ViewContactModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$contactsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _contactsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#ContactsTableFilter').val(),
					firstNameFilter: $('#FirstNameFilterId').val(),
					lastNameFilter: $('#LastNameFilterId').val(),
					emailFilter: $('#EmailFilterId').val(),
					phoneNumberFilter: $('#PhoneNumberFilterId').val()
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
                                    _viewContactModal.open({ id: data.record.contact.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.contact.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteContact(data.record.contact);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "contact.firstName",
						 name: "firstName"   
					},
					{
						targets: 2,
						 data: "contact.lastName",
						 name: "lastName"   
					},
					{
						targets: 3,
						 data: "contact.email",
						 name: "email"   
					},
					{
						targets: 4,
						 data: "contact.phoneNumber",
						 name: "phoneNumber"   
					}
            ]
        });

        function getContacts() {
            dataTable.ajax.reload();
        }

        function deleteContact(contact) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _contactsService.delete({
                            id: contact.id
                        }).done(function () {
                            getContacts(true);
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

        $('#CreateNewContactButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _contactsService
                .getContactsToExcel({
				filter : $('#ContactsTableFilter').val(),
					firstNameFilter: $('#FirstNameFilterId').val(),
					lastNameFilter: $('#LastNameFilterId').val(),
					emailFilter: $('#EmailFilterId').val(),
					phoneNumberFilter: $('#PhoneNumberFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditContactModalSaved', function () {
            getContacts();
        });

		$('#GetContactsButton').click(function (e) {
            e.preventDefault();
            getContacts();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getContacts();
		  }
		});
    });
})();