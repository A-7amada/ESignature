(function ($) {
    app.modals.CreateOrEditRecipientModal = function () {

        var _recipientsService = abp.services.app.recipients;

        var _modalManager;
        var _$recipientInformationForm = null;

		        var _RecipientuserLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Recipients/UserLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Recipients/_RecipientUserLookupTableModal.js',
            modalClass: 'UserLookupTableModal'
        });        var _RecipientdocumentRequestLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Recipients/DocumentRequestLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Recipients/_RecipientDocumentRequestLookupTableModal.js',
            modalClass: 'DocumentRequestLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$recipientInformationForm = _modalManager.getModal().find('form[name=RecipientInformationsForm]');
            _$recipientInformationForm.validate();
        };

		          $('#OpenUserLookupTableButton').click(function () {

            var recipient = _$recipientInformationForm.serializeFormToObject();

            _RecipientuserLookupTableModal.open({ id: recipient.userId, displayName: recipient.userName }, function (data) {
                _$recipientInformationForm.find('input[name=userName]').val(data.displayName); 
                _$recipientInformationForm.find('input[name=userId]').val(data.id); 
            });
        });
		
		$('#ClearUserNameButton').click(function () {
                _$recipientInformationForm.find('input[name=userName]').val(''); 
                _$recipientInformationForm.find('input[name=userId]').val(''); 
        });
		
        $('#OpenDocumentRequestLookupTableButton').click(function () {

            var recipient = _$recipientInformationForm.serializeFormToObject();

            _RecipientdocumentRequestLookupTableModal.open({ id: recipient.documentRequestId, displayName: recipient.documentRequestDocumentTitle }, function (data) {
                _$recipientInformationForm.find('input[name=documentRequestDocumentTitle]').val(data.displayName); 
                _$recipientInformationForm.find('input[name=documentRequestId]').val(data.id); 
            });
        });
		
		$('#ClearDocumentRequestDocumentTitleButton').click(function () {
                _$recipientInformationForm.find('input[name=documentRequestDocumentTitle]').val(''); 
                _$recipientInformationForm.find('input[name=documentRequestId]').val(''); 
        });
		


        this.save = function () {
            if (!_$recipientInformationForm.valid()) {
                return;
            }
            if ($('#Recipient_UserId').prop('required') && $('#Recipient_UserId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
                return;
            }
            if ($('#Recipient_DocumentRequestId').prop('required') && $('#Recipient_DocumentRequestId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('DocumentRequest')));
                return;
            }

            var recipient = _$recipientInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _recipientsService.createOrEdit(
				recipient
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditRecipientModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);