(function ($) {

    var url = abp.appPath + 'App/UploadFileAndSave/UploadFileAndSaveDocumentRequest';
    $('#fileupload').fileupload({
        url: url,
        dataType: 'json',
        maxFileSize: 999000,
        dropZone: $('#fileuploadForm'),
        done: function (e, response) {
            var jsonResult = response.result;

            if (jsonResult.success) {
                //var fileUrl = abp.appPath + 'App/DemoUiComponents/GetFile?id=' + jsonResult.result.id + '&contentType=' + jsonResult.result.contentType;
                //var uploadedFile = '<a href="' + fileUrl + '" target="_blank">' + app.localize('UploadedFile') + '</a><br/><br/>' + ' Free text: ' + jsonResult.result.defaultFileUploadTextInput;

                //abp.message.info(uploadedFile, app.localize('PostedData'), true);
                //abp.notify.info(app.localize('SavedSuccessfully'));
                abp.message.success(app.localize('UploadedSuccessfully'));
            } else {
                //abp.message.error(jsonResult.error.message);
                abp.message.error(app.localize('Error'));
            }
        },
        progressall: function (e, data) {
            var progress = parseInt(data.loaded / data.total * 100, 10);
            $('#progress .progress-bar').css(
                'width',
                progress + '%'
            );
        }
    }).prop('disabled', !$.support.fileInput)
        .parent().addClass($.support.fileInput ? undefined : 'disabled');


    app.modals.CreateOrEditDocumentRequestModal = function () {
        var _documentRequestsService = abp.services.app.documentRequests;

        var _$documentRequestInformationForm = null;

        function Save() {
            var documentRequest = _$documentRequestInformationForm.serializeFormToObject();
            _documentRequestsService.createOrEdit(
                documentRequest
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditDocumentRequestModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        }
    };
})(jQuery);