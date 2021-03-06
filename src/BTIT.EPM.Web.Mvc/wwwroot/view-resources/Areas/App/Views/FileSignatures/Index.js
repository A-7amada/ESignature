(function () {
    $(function () {

        var _$fileSignaturesTable = $('#FileSignaturesTable');
        var _fileSignaturesService = abp.services.app.fileSignatures;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.FileSignatures.Create'),
            edit: abp.auth.hasPermission('Pages.FileSignatures.Edit'),
            'delete': abp.auth.hasPermission('Pages.FileSignatures.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
                    viewUrl: abp.appPath + 'App/FileSignatures/CreateOrEditModal',
                    scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/FileSignatures/_CreateOrEditModal.js',
                    modalClass: 'CreateOrEditFileSignatureModal'
                });
                   

		 var _viewFileSignatureModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/FileSignatures/ViewfileSignatureModal',
            modalClass: 'ViewFileSignatureModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }
        
        var getMaxDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT23:59:59Z"); 
        }

        var dataTable = _$fileSignaturesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _fileSignaturesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#FileSignaturesTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					describtionFilter: $('#DescribtionFilterId').val()
                    };
                }
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
                    width: 120,
                    targets: 1,
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
                                iconStyle: 'far fa-eye mr-2',
                                action: function (data) {
                                    _viewFileSignatureModal.open({ id: data.record.fileSignature.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            iconStyle: 'far fa-edit mr-2',
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.fileSignature.id });
                               
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            iconStyle: 'far fa-trash-alt mr-2',
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteFileSignature(data.record.fileSignature);
                            }
                        }]
                    }
                },
					{
						targets: 2,
						 data: "fileSignature.name",
						 name: "name"   
					},
					{
						targets: 3,
						 data: "fileSignature.describtion",
						 name: "describtion"   
					}
            ]
        });

        function getFileSignatures() {
            dataTable.ajax.reload();
        }

        function deleteFileSignature(fileSignature) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _fileSignaturesService.delete({
                            id: fileSignature.id
                        }).done(function () {
                            getFileSignatures(true);
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

        $('#CreateNewFileSignatureButton').click(function () {
           // _createOrEditModal.open();
            window.location.href = "./FileSignatures/CreateOrEdit";
        });        

		$('#ExportToExcelButton').click(function () {
            _fileSignaturesService
                .getFileSignaturesToExcel({
				filter : $('#FileSignaturesTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					describtionFilter: $('#DescribtionFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditFileSignatureModalSaved', function () {
            getFileSignatures();
        });

		$('#GetFileSignaturesButton').click(function (e) {
            e.preventDefault();
            getFileSignatures();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getFileSignatures();
		  }
		});
		
		
		
    });
})();
