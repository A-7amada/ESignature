(function ($) {
    app.modals.CreateOrEditDocumentRequestAuditTrailModal = function () {

        var _documentRequestAuditTrailsService = abp.services.app.documentRequestAuditTrails;

        var _modalManager;
        var _$documentRequestAuditTrailInformationForm = null;

		        var _DocumentRequestAuditTraildocumentRequestLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/DocumentRequestAuditTrails/DocumentRequestLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/DocumentRequestAuditTrails/_DocumentRequestAuditTrailDocumentRequestLookupTableModal.js',
            modalClass: 'DocumentRequestLookupTableModal'
        });        var _DocumentRequestAuditTrailrecipientLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/DocumentRequestAuditTrails/RecipientLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/DocumentRequestAuditTrails/_DocumentRequestAuditTrailRecipientLookupTableModal.js',
            modalClass: 'RecipientLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$documentRequestAuditTrailInformationForm = _modalManager.getModal().find('form[name=DocumentRequestAuditTrailInformationsForm]');
            _$documentRequestAuditTrailInformationForm.validate();
        };

		          $('#OpenDocumentRequestLookupTableButton').click(function () {

            var documentRequestAuditTrail = _$documentRequestAuditTrailInformationForm.serializeFormToObject();

            _DocumentRequestAuditTraildocumentRequestLookupTableModal.open({ id: documentRequestAuditTrail.documentRequestId, displayName: documentRequestAuditTrail.documentRequestDocumentTitle }, function (data) {
                _$documentRequestAuditTrailInformationForm.find('input[name=documentRequestDocumentTitle]').val(data.displayName); 
                _$documentRequestAuditTrailInformationForm.find('input[name=documentRequestId]').val(data.id); 
            });
        });
		
		$('#ClearDocumentRequestDocumentTitleButton').click(function () {
                _$documentRequestAuditTrailInformationForm.find('input[name=documentRequestDocumentTitle]').val(''); 
                _$documentRequestAuditTrailInformationForm.find('input[name=documentRequestId]').val(''); 
        });
		
        $('#OpenRecipientLookupTableButton').click(function () {

            var documentRequestAuditTrail = _$documentRequestAuditTrailInformationForm.serializeFormToObject();

            _DocumentRequestAuditTrailrecipientLookupTableModal.open({ id: documentRequestAuditTrail.recipientId, displayName: documentRequestAuditTrail.recipientFirstName }, function (data) {
                _$documentRequestAuditTrailInformationForm.find('input[name=recipientFirstName]').val(data.displayName); 
                _$documentRequestAuditTrailInformationForm.find('input[name=recipientId]').val(data.id); 
            });
        });
		
		$('#ClearRecipientFirstNameButton').click(function () {
                _$documentRequestAuditTrailInformationForm.find('input[name=recipientFirstName]').val(''); 
                _$documentRequestAuditTrailInformationForm.find('input[name=recipientId]').val(''); 
        });
		


        this.save = function () {
            if (!_$documentRequestAuditTrailInformationForm.valid()) {
                return;
            }
            if ($('#DocumentRequestAuditTrail_DocumentRequestId').prop('required') && $('#DocumentRequestAuditTrail_DocumentRequestId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('DocumentRequest')));
                return;
            }
            if ($('#DocumentRequestAuditTrail_RecipientId').prop('required') && $('#DocumentRequestAuditTrail_RecipientId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Recipient')));
                return;
            }

            var documentRequestAuditTrail = _$documentRequestAuditTrailInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _documentRequestAuditTrailsService.createOrEdit(
				documentRequestAuditTrail
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditDocumentRequestAuditTrailModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);