(function () {
    $(function () {
        var _$documentRequestsTable = $('#ViewAndSignDocumentTable');
        var _documentRequestsService = abp.services.app.documentRequests;

        ShowPoupPin();


        $(document).keypress(function (event) {

            _signerPin = $('#verificationCode').val();
            var keycode = (event.keyCode ? event.keyCode : event.which);
            if (keycode == '13') {
                _documentRequestsService
                    .confirmPin({
                        recipientCode: $('#RecipientCode').val(),
                        documentRequestId: $('#DocumentRequestId').val(),
                        recipientId: $('#RecipientId').val(),
                        signerPin: $('#verificationCode').val()
                    })
                    .done(function (result) {
                        if (result == true || result == "True") {
                            var url = window.location.href.slice(window.location.href.indexOf('?'));
                            url = url.split("&")[0];
                            var fullURL = abp.appPath + "APP/DocumentRequests/ViewAndSignDocumentConfirmedPin" + url + "&SignerPin=" + _signerPin;                               
                            document.location.href = fullURL;
                        }
                        else {
                            ShowPoupPin();
                        }

                    });
            }
        });
        $(document).on("click", ".swal-button.swal-button--confirm", function () {
            _signerPin = $('#verificationCode').val();
            _documentRequestsService
                .confirmPin({
                    recipientCode: $('#RecipientCode').val(),
                    documentRequestId: $('#DocumentRequestId').val(),
                    recipientId: $('#RecipientId').val(),
                    signerPin: $('#verificationCode').val()
                })
                .done(function (result) {
                    if (result == true || result == "True") {
                        var url = window.location.href.slice(window.location.href.indexOf('?'));
                        url = url.split("&")[0];
                        var fullURL = abp.appPath + "APP/DocumentRequests/ViewAndSignDocumentConfirmedPin" + url + "&SignerPin=" + _signerPin;    
                        document.location.href = fullURL;
                    }
                    else {
                        ShowPoupPin();
                    }

                });

        });

        function ShowPoupPin() {
            $(".swal-overlay swal-overlay--show-modal").css('background-color', 'rgba(255, 255, 255, 0.95)');
            swal(app.localize("PleaseEnterTheVerificationCode"), {
                title: app.localize("Authentication"),
                text: app.localize("PleaseEnterTheVerificationCode"),
                closeOnClickOutside: false,
                allowEnterKey: true,

                content: {
                    element: "input",
                    attributes: {
                        placeholder: app.localize("PleaseEnterTheVerificationCode"),
                        type: "password",
                        id: "verificationCode",
                        allowEnterKey: true,
                        allowEscapeKey: true,
                    },
                },

            });

        }
    });
})();