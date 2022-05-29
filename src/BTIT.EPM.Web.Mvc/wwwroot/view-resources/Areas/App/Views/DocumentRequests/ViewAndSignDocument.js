(function () {
    $(function () {
        var _$documentRequestsTable = $('#ViewAndSignDocumentTable');
        var _documentRequestsService = abp.services.app.documentRequests;

        //if (!$('#PinCode').val()) {
        //    FillDataTabel();
           
        //}

        var _digitalSignature = new app.ModalManager({
            viewUrl: abp.appPath + 'App/DocumentRequests/DigitalSignature',
            //scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/DocumentRequests/_DigitalSignature.js',
            modalClass: 'CreateOrEditDocumentRequestModal'
        });

        $(document).on("click", ".SignButton", function () {
            _digitalSignature.open();
        });

        function FillDataTabel() {
            var dataTable = _$documentRequestsTable.DataTable({
                paging: false,
                serverSide: true,
                processing: true,
                "bInfo": false,
                "ordering": false,
                listAction: {
                    ajaxFunction: _documentRequestsService.getDocumentRequestForViewAndSign,
                    inputFilter: function () {
                        return {
                            recipientId: $('#RecipientId').val(),
                            documentRequestId: $('#DocumentRequestId').val(),
                            recipientCode: $('#RecipientCode').val()
                        };
                    }
                },
                columnDefs: [
                    {
                        className: 'control responsive',
                        orderable: false,
                        render: function () {
                            return '';
                        },
                        visible: false,
                        targets: 0
                    },
                    {
                        targets: 1,
                        data: "documentRequestViewAndSign.signeOrder",
                        name: "order",

                        render: function (order) {
                            return app.localize('Signer') + "#" + (order + 1);
                        }
                    },
                    {
                        targets: 2,
                        data: "documentRequestViewAndSign",
                        name: "documentRequestViewAndSign",
                        render: function (documentRequestViewAndSign) {
                            return documentRequestViewAndSign.name + "<br/> <i class='flaticon2-email kt-font-danger'></i>  " + documentRequestViewAndSign.email;
                        }
                    },
                    {
                        targets: 3,
                        data: "documentRequestViewAndSign",
                        name: "documentRequestViewAndSign",
                        render: function (documentRequestViewAndSign) {
                            if (documentRequestViewAndSign.isSigned) {
                                var signatureDate = moment(documentRequestViewAndSign.signatureDate).format('MMMM DD YYYY h:mm:ss a');
                                return "<button class='btn btn-success' style='pointer-events: none;'>" + app.localize('Signed') + "</button>" + " on " + signatureDate;
                            }
                            else if (documentRequestViewAndSign.isSent) {
                                var sentDate = moment(documentRequestViewAndSign.sentDate).format('MMMM DD YYYY h:mm:ss a');;
                                return "<button class='btn btn-secondary active' style='pointer-events: none;'>" + app.localize('Outstanding') + "</button>" + " sent on " + sentDate;
                            }
                            else {
                                return "<button class='btn btn-secondary disabled' style='pointer-events: none;'>" + app.localize('Queued') + "</button>";
                            }
                        }

                    }
                ]
            });
        }
    });
})();