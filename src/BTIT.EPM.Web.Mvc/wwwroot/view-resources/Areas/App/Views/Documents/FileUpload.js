$(function () {
    'use strict';
    var _documentsService = abp.services.app.documents;
    var _viewFileSignatureModal = new app.ModalManager({
        viewUrl: abp.appPath + 'App/FileSignatures/ViewFilePreviewModal',
        modalClass: 'ViewFileSignatureModal',
        modalSize: 'modal-xl'
    });
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

    $('#documentsList').on('click', '#getDocumentBtn', function () {
        var elem = $(this);
        var documentId = elem.data('id');
        var contentType = elem.data('contentType');
        if (documentId && documentId != '') {
            _documentsService.getDocumentForView(documentId)
                .done(function (res) {
                    console.log("res", res);
                    let fileUrl = abp.appPath + 'App/Documents/GetFile?documentId=' + res.document.id + '&contentType=' + res.document.contentType;
                    _viewFileSignatureModal.open({ fileUrl: fileUrl });
                    //var options = {
                    //    height: "400px",
                    //    page: '2',
                    //    pdfOpenParams: {
                    //        view: 'FitV',
                    //        pagemode: 'thumbs',
                    //        search: 'lorem ipsum'
                    //    }
                    //};
                    //  PDFObject.embed(fileUrl, "#viewer", options);

                });
        }


    }
    );



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
                uploadedfile = uploadedfile + '<a id="getDocumentBtn" name="getDocumentBtn" href="javascript:void(0);" data-id="' + jsonResult.result.id + '" class="pull-center text-danger" style="margin-right: 10px" >Preview<i class="fa fa-id-card" aria-hidden="true"></i></a></div ></li>';
                uploadedfile = uploadedfile + '<a id="deleteDocumentBtn" name="deleteDocumentBtn" href="javascript:void(0);" data-id="' + jsonResult.result.id + '" class="pull-right text-danger" style="margin-right: 10px"><i class="fa fa-trash" aria-hidden="true"></i></a></div ></li>';
                $("#documentsList").append(uploadedfile);
                $('#DocPaths').val(fileUrl);

                //alert($('#DocPaths').val());
                //PDFObject.embed(fileUrl, "#viewer");
                //console.log("fileUrl 1", fileUrl);
                //console.log(PDFObject.pdfobjectversion);
                ////PDFObject.embed("https://unec.edu.az/application/uploads/2014/12/pdf-sample.pdf", "#viewer");
                //console.log(PDFObject.pdfobjectversion);
                //    var seedocument = jsonResult.result.id;
                //    //console.log('&lt;iframe src=&quot;https://docs.google.com/viewer?embedded=true&amp;url=&#39; + seedocument + &#39;&quot; width=&quot;&#39; + settings.width + &#39;&quot; height=&quot;&#39; + settings.height + &#39;&quot; style=&quot;border: none;margin : 0 auto; display : block;&quot;&gt;&lt;/iframe&gt;');
                //    alert('&lt;iframe src=&quot;https://docs.google.com/viewer?embedded=true&amp;url=&#39; + seedocument + &#39;&quot; width=&quot;&#39; + settings.width + &#39;&quot; height=&quot;&#39; + settings.height + &#39;&quot; style=&quot;border: none;margin : 0 auto; display : block;&quot;&gt;&lt;/iframe&gt;')

                //    $('#viewer').append('&lt;iframe src=&quot;https://docs.google.com/viewer?embedded=true&amp;url=&#39; + seedocument + &#39;&quot; width=&quot;&#39; + settings.width + &#39;&quot; height=&quot;&#39; + settings.height + &#39;&quot; style=&quot;border: none;margin : 0 auto; display : block;&quot;&gt;&lt;/iframe&gt;');

                //    //pass the name of the document to be save latter
                //    //var hv = $('input[id$=hidClientField]').val();
                //    $('input[id$=DocPaths]').val(seedocument);

                ///*alert($('input[id$=DocPaths]').val());*/


                //    $("#viewer").dropzone();

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
