(function ($) {
    app.modals.CreateOrEditContactModal = function () {

        var _contactsService = abp.services.app.contacts;

        var _modalManager;
        var _$contactInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$contactInformationForm = _modalManager.getModal().find('form[name=ContactInformationsForm]');
            _$contactInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$contactInformationForm.valid()) {
                return;
            }

            var contact = _$contactInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _contactsService.createOrEdit(
				contact
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditContactModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);