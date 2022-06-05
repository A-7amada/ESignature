$(function () {
    'use strict';
    var _documentsService = abp.services.app.documents;
   
    $('#documentsList').on('click', '#deleteDocumentBtn', function () {
        var elem = $(this);
        var documentId = elem.data('id');
        if (documentId && documentId != '') {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _documentsService.delete(documentId)
                            .done(function () {
                                elem.parents('li').remove();
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                            });
                    }
                }
            );
        }
    });


    // Change this to the location of your server-side upload handler:
    var url = abp.appPath + 'App/Documents/UploadFile';
    $('#fileupload').fileupload({
        url: url,
        dataType: 'json',
        maxFileSize: 999000,
        dropZone: $('#fileuploadForm2'),
        done: function (e, response) {
            var jsonResult = response.result;

            if (jsonResult.success) {
                var fileUrl = abp.appPath + 'App/Documents/GetFile?documentId=' + jsonResult.result.id + '&contentType=' + jsonResult.result.contentType;

                var uploadedfile = '<li><div class="files" style="max-height: 150px; overflow-y: auto; margin: 15px;"><a href = "' + fileUrl + '" download = "' + jsonResult.result.documentDescription + '" >' + jsonResult.result.documentDescription + '</a >'
                uploadedfile = uploadedfile + '<a id="deleteDocumentBtn" name="deleteDocumentBtn" href="javascript:void(0);" data-id="' + jsonResult.result.id + '" class="pull-right text-danger" style="margin-right: 10px"><i class="fa fa-trash" aria-hidden="true"></i></a></div ></li>';

                $("#documentsList").append(uploadedfile);

                abp.notify.info(app.localize('SavedSuccessfully'));
            } else {
                abp.message.error(jsonResult.error.message);
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
});
