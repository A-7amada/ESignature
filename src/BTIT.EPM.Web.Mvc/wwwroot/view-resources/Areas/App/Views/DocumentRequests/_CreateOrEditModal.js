(function ($) {
    app.modals.CreateOrEditDocumentRequestModal = function () {

        var _documentRequestsService = abp.services.app.documentRequests;

        var _modalManager;
        var _$documentRequestInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$documentRequestInformationForm = _modalManager.getModal().find('form[name=DocumentRequestInformationsForm]');
            _$documentRequestInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$documentRequestInformationForm.valid()) {
                return;
            }

            var documentRequest = _$documentRequestInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _documentRequestsService.createOrEdit(
				documentRequest
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditDocumentRequestModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);