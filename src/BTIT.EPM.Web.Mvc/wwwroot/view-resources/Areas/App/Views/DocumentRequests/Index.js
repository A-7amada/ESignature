var status = "-1";
(function () {
    $(function () {

        var _$documentRequestsTable = $('#DocumentRequestsTable');
        var _$documentRequestsNeedToSignTable = $("#DocumentRequestsNeedToSignTable");
        var _documentRequestsService = abp.services.app.documentRequests;

        $("#needToSignTab").hide();

        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.DocumentRequests.Create'),
            edit: abp.auth.hasPermission('Pages.DocumentRequests.Edit'),
            'delete': abp.auth.hasPermission('Pages.DocumentRequests.Delete')
        };

        getDocumentRequestsWhichNeedToSignCount();

        var dataTable = _$documentRequestsTable.DataTable({
            paging: true,
            responsive: true,
            serverSide: true,
            processing: true,
            language: {
                url: abp.appPath + 'Common/Scripts/Datatables/Translations/' + abp.localization.currentCulture.displayNameEnglish + '.json'
            },
            "dom": '<"top">rt<"bottom"i_flp><"clear">',
            listAction: {
                ajaxFunction: _documentRequestsService.getAll,
                inputFilter: function () {
                    return {
                        filter: $('#DocumentRequestsTableFilter').val(),
                        documentTitleFilter: $('#DocumentTitleFilterId').val(),
                        statusFilter: status,
                        isSigningOrderedFilter: $('#IsSigningOrderedFilterId').val()
                    };
                }
            },
            columnDefs: [
                {
                    width: 120,
                    targets: 0,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    defaultContent: '',
                    rowAction: {
                        cssClass: 'btn btn-brand dropdown-toggle',
                        text: '<i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span>',
                        items: [
                            {
                                text: app.localize('AuditTrail'),
                                action: function (data) {
                                    document.location.href = abp.appPath + "App/DocumentRequests/ViewRequestAuditTrail/" + data.record.documentRequest.id;
                                }
                            },
                            {
                                text: app.localize('Continue'),
                                visible: function (data) {
                                    return data.record.documentRequest.status == 0;
                                },
                                action: function (data) {
                                    document.location.href = abp.appPath + "App/DocumentRequests/Create/" + data.record.documentRequest.id;
                                }
                            },
                            {
                                text: app.localize('Delete'),
                                visible: function (data) {
                                    return (data.record.documentRequest.status == 0
                                        && _permissions.delete);
                                },
                                action: function (data) {
                                    deleteDocumentRequest(data.record.documentRequest);
                                }
                            },
                            {
                                text: app.localize('SendReminder'),
                                visible: function (data) {
                                    return data.record.documentRequest.status == 1;
                                },
                                action: function (data) {
                                    _documentRequestsService.sendReminderEmail(data.record.documentRequest.id);
                                }
                            }
                        ]
                    }
                },
                {
                    targets: 1,
                    data: "documentRequest",
                    name: "fileName",
                    render: function (data) {
                        var creationDate = moment(data.creationTime).format("YYYY-MM-DD hh:mm:ss");
                        var url = abp.appPath + 'App/Common/GetFile?id=' + data.fileGuid + '&contentType=application/pdf';
                        return "<span class='date'>" + creationDate + " </span>" +
                            " <a href=" + url + " target='blank'>  " + data.documentTitle + "</a>"
                            + "<br/><span class='reciepient'>" + app.localize('To') + ': ' + data.recipients + "</span>";
                    }
                },
                {
                    targets: 2,
                    data: "documentRequest.status",
                    name: "status",
                    render: function (status) {
                        if (status == 3)
                            return "<button class='btn btn-success' style='pointer-events: none;'>" + app.localize('Enum_DocumentRequestStatus_' + status) + "</button>";
                        else if (status == 0)
                            return "<button class='btn btn-secondary disabled' style='pointer-events: none;'>" + app.localize('Enum_DocumentRequestStatus_' + status) + "</button>";
                        else if (status == 1)
                            return "<button class='btn btn-secondary active' style='pointer-events: none;'>" + app.localize('Enum_DocumentRequestStatus_' + status) + "</button>";
                    }
                },
                {
                    targets: 3,
                    data: "documentRequest.isSigningOrdered",
                    name: "isSigningOrdered",
                    render: function (isSigningOrdered) {
                        var $span = $("<span/>").addClass("label");
                        if (isSigningOrdered) {
                            $span.addClass("kt-badge kt-badge--success kt-badge--inline").text(app.localize('Yes'));
                        } else {
                            $span.addClass("kt-badge kt-badge--dark kt-badge--inline").text(app.localize('No'));
                        }

                        return $span[0].outerHTML;
                    }
                }
            ]
        });

        function getDocumentRequests() {
            dataTable.ajax.reload();
        }

        $("#All").click(function () {
            $("#needToSignTab").hide();
            $("#DocumentRequestsTableTab").show();
            status = $(this).attr("data-StatusId");
            getDocumentRequests();
        });

        $("#Draft").click(function () {
            $("#needToSignTab").hide();
            $("#DocumentRequestsTableTab").show();
            status = $(this).attr("data-StatusId");
            getDocumentRequests();
        });

        $("#Completed").click(function () {
            $("#needToSignTab").hide();
            $("#DocumentRequestsTableTab").show();
            status = $(this).attr("data-StatusId");
            getDocumentRequests();
        });

        $("#InProgress").click(function () {
            $("#needToSignTab").hide();
            $("#DocumentRequestsTableTab").show();
            status = $(this).attr("data-StatusId");
            getDocumentRequests();
        });

        $("#NeedToSign").click(function () {
            $("#needToSignTab").show();
            $("#DocumentRequestsTableTab").hide();
            getDocumentRequestsWhichNeedToSign();
        });

        $("#Cancelled").click(function () {
            $("#needToSignTab").hide();
            $("#DocumentRequestsTableTab").show();
            status = $(this).attr("data-StatusId");
            getDocumentRequests();
        });

        function getDocumentRequests() {
            dataTable.ajax.reload();
        }

        function getDocumentRequestsWhichNeedToSign() {
            var dT = _$documentRequestsNeedToSignTable.DataTable({
                retrieve: true,
                paging: true,
                responsive: true,
                serverSide: true,
                processing: true,
                language: {
                    url: abp.appPath + 'Common/Scripts/Datatables/Translations/' + abp.localization.currentCulture.displayNameEnglish + '.json'
                },
                "dom": '<"top">rt<"bottom"i_flp><"clear">',
                listAction: {
                    ajaxFunction: _documentRequestsService.getAllDocumentRequestWhichNeedToSign,
                    inputFilter: function () {
                        return {
                            filter: $('#DocumentRequestsTableFilter').val(),
                            documentTitleFilter: $('#DocumentTitleFilterId').val(),
                            statusFilter: status,
                            isSigningOrderedFilter: $('#IsSigningOrderedFilterId').val()
                        };
                    }
                },
                columnDefs: [
                    {
                        width: 120,
                        targets: 0,
                        data: null,
                        orderable: false,
                        autoWidth: false,
                        defaultContent: '',
                        rowAction: {
                            cssClass: 'btn btn-brand dropdown-toggle',
                            text: '<i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span>',
                            items: [
                                {
                                    text: app.localize('AuditTrail'),
                                    action: function (data) {
                                        document.location.href = abp.appPath + "App/DocumentRequests/ViewRequestAuditTrail/" + data.record.documentRequest.id;
                                    }
                                },
                                {
                                    text: app.localize('Continue'),
                                    visible: function (data) {
                                        return data.record.documentRequest.status == 0;
                                    },
                                    action: function (data) {
                                        document.location.href = abp.appPath + "App/DocumentRequests/Create/" + data.record.documentRequest.id;
                                    }
                                },
                                {
                                    text: app.localize('Delete'),
                                    visible: function (data) {
                                        return (data.record.documentRequest.status == 0
                                            && _permissions.delete);
                                    },
                                    action: function (data) {
                                        deleteDocumentRequest(data.record.documentRequest);
                                    }
                                },
                                {
                                    text: app.localize('SignDocument'),
                                    visible: function (data) {
                                        return data.record.documentRequest.status == 1;
                                    },
                                    action: function (data) {                                       
                                        _documentRequestsService.getDocumentLink(data.record.documentRequest.id).done(function (result) {
                                            document.location.href = result; 
                                        });
                                    }
                                }
                            ]
                        }
                    },
                    {
                        targets: 1,
                        data: "documentRequest",
                        name: "fileName",
                        render: function (data) {
                            var creationDate = moment(data.creationTime).format("YYYY-MM-DD hh:mm:ss");
                            var url = abp.appPath + 'App/Common/GetFile?id=' + data.fileGuid + '&contentType=application/pdf';
                            return "<span class='date'>" + creationDate + " </span>" +
                                " <a href=" + url + " target='blank'>  " + data.documentTitle + "</a>"
                                + "<br/><span class='reciepient'>" + app.localize('To') + ': ' + data.recipients + "</span>";
                        }
                    },
                    {
                        targets: 2,
                        data: "documentRequest.status",
                        name: "status",
                        render: function (status) {
                            if (status == 3)
                                return "<button class='btn btn-success' style='pointer-events: none;'>" + app.localize('Enum_DocumentRequestStatus_' + status) + "</button>";
                            else if (status == 0)
                                return "<button class='btn btn-secondary disabled' style='pointer-events: none;'>" + app.localize('Enum_DocumentRequestStatus_' + status) + "</button>";
                            else if (status == 1)
                                return "<button class='btn btn-secondary active' style='pointer-events: none;'>" + app.localize('Enum_DocumentRequestStatus_' + status) + "</button>";
                        }
                    },
                    {
                        targets: 3,
                        data: "documentRequest.isSigningOrdered",
                        name: "isSigningOrdered",
                        render: function (isSigningOrdered) {
                            var $span = $("<span/>").addClass("label");
                            if (isSigningOrdered) {
                                $span.addClass("kt-badge kt-badge--success kt-badge--inline").text(app.localize('Yes'));
                            } else {
                                $span.addClass("kt-badge kt-badge--dark kt-badge--inline").text(app.localize('No'));
                            }
                            return $span[0].outerHTML;
                        }
                    },
                ]
            });
        }

        function getDocumentRequestsWhichNeedToSignCount() {
            _documentRequestsService.getAllDocumentRequestWhichNeedToSignCount({}).done(function (data) {
                $("#NeedToSignCount").text(data);
            });
        }

        function deleteDocumentRequest(documentRequest) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _documentRequestsService.delete({
                            id: documentRequest.id
                        }).done(function () {
                            getDocumentRequests(true);
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                        });
                    }
                }
            );
        }

        $('#ShowAdvancedFiltersSpan').click(function () {
            $('#ShowAdvancedFiltersSpan').hide();
            $('#HideAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideDown();
        });

        $('#HideAdvancedFiltersSpan').click(function () {
            $('#HideAdvancedFiltersSpan').hide();
            $('#ShowAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideUp();
        });

        $('#CreateNewDocumentRequestButton').click(function () {
            window.location.href = abp.appPath + 'App/DocumentRequests/Create';
        });

        $('#ExportToExcelButton').click(function () {
            _documentRequestsService
                .getDocumentRequestsToExcel({
                    filter: $('#DocumentRequestsTableFilter').val(),
                    documentTitleFilter: $('#DocumentTitleFilterId').val(),
                    statusFilter: $('#StatusFilterId').val(),
                    isSigningOrderedFilter: $('#IsSigningOrderedFilterId').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditDocumentRequestModalSaved', function () {
            getDocumentRequests();
        });

        $('#GetDocumentRequestsButton').click(function (e) {
            e.preventDefault();
            getDocumentRequests();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getDocumentRequests();
            }
        });
    });
})();