(function () {
    $(function () {
   
        var _fileSignaturesService = abp.services.app.fileSignatures;
        var _modalManager;
        
        var _$fileSignatureInformationForm = $('form[name=FileSignatureInformationsForm]');
        //var _$fileSignatureInformationForm = null;
        this.init = function (modalManager) {
            _modalManager = modalManager;

            var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$fileSignatureInformationForm = _modalManager.getModal().find('form[name=FileSignatureInformationsForm]');
            _$fileSignatureInformationForm.validate();
        };
        $('#saveBtn').click(function () {
            alert("save clicked");
            if (!_$fileSignatureInformationForm.valid()) {
                return;
            }
            var fileSignature = _$fileSignatureInformationForm.serializeFormToObject();
            /* _modalManager.setBusy(true);*/
            abp.ui.setBusy();
            _fileSignaturesService.createOrEdit(
                fileSignature
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                /*_modalManager.close();*/
                //abp.event.trigger('app.createOrEditFileSignatureModalSaved');
                window.location = "/App/FileSignatures/";
                // }).always(function () {
                //            _modalManager.setBusy(false);

            }).always(function () {
                abp.ui.clearBusy();
            });
        });


		
    });
})();