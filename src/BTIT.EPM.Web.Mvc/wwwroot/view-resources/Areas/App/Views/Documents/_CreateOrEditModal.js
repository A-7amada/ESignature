(function ($) {
    app.modals.CreateOrEditDocumentModal = function () {

        var _documentsService = abp.services.app.documents;

        var _modalManager;
        var _$documentInformationForm = null;

		        var _DocumentbinaryObjectLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Documents/BinaryObjectLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Documents/_DocumentBinaryObjectLookupTableModal.js',
            modalClass: 'BinaryObjectLookupTableModal'
        });        var _DocumentdocumentRequestLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Documents/DocumentRequestLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Documents/_DocumentDocumentRequestLookupTableModal.js',
            modalClass: 'DocumentRequestLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$documentInformationForm = _modalManager.getModal().find('form[name=DocumentInformationsForm]');
            _$documentInformationForm.validate();
        };

		          $('#OpenBinaryObjectLookupTableButton').click(function () {

            var document = _$documentInformationForm.serializeFormToObject();

            _DocumentbinaryObjectLookupTableModal.open({ id: document.binaryObjectId, displayName: document.binaryObjectTenantId }, function (data) {
                _$documentInformationForm.find('input[name=binaryObjectTenantId]').val(data.displayName); 
                _$documentInformationForm.find('input[name=binaryObjectId]').val(data.id); 
            });
        });
		
		$('#ClearBinaryObjectTenantIdButton').click(function () {
                _$documentInformationForm.find('input[name=binaryObjectTenantId]').val(''); 
                _$documentInformationForm.find('input[name=binaryObjectId]').val(''); 
        });
		
        $('#OpenDocumentRequestLookupTableButton').click(function () {

            var document = _$documentInformationForm.serializeFormToObject();

            _DocumentdocumentRequestLookupTableModal.open({ id: document.documentRequestId, displayName: document.documentRequestDocumentTitle }, function (data) {
                _$documentInformationForm.find('input[name=documentRequestDocumentTitle]').val(data.displayName); 
                _$documentInformationForm.find('input[name=documentRequestId]').val(data.id); 
            });
        });
		
		$('#ClearDocumentRequestDocumentTitleButton').click(function () {
                _$documentInformationForm.find('input[name=documentRequestDocumentTitle]').val(''); 
                _$documentInformationForm.find('input[name=documentRequestId]').val(''); 
        });
		


        this.save = function () {
            if (!_$documentInformationForm.valid()) {
                return;
            }
            if ($('#Document_BinaryObjectId').prop('required') && $('#Document_BinaryObjectId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('BinaryObject')));
                return;
            }
            if ($('#Document_DocumentRequestId').prop('required') && $('#Document_DocumentRequestId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('DocumentRequest')));
                return;
            }

            var document = _$documentInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _documentsService.createOrEdit(
				document
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditDocumentModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);