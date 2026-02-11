var depositFund = function () {
    var requestDepositFund = function () {
        /*    requestQR('CPHF1AT1SWJXRIALWIGC9OMHLE');*/
        $("#DepositFundRequest").validate({
            rules: {
                "ChainType": { required: true },
                "RequestAmount": { required: true, min: 100 },
            },
            errorElement: "span",
            errorPlacement: function (error, element) {
                if (element.attr("name") == "RequestAmount") {
                    error.insertAfter(element.parent());
                } else {
                    error.insertAfter(element);
                }
            },
            submitHandler: function (form, e) {
                $(".preloader").fadeIn();
                var f = $(form);
                var data = f.serializeArray();
                $.ajax({
                    type: f[0].method,
                    url: f[0].action,
                    data: data,
                    dataType: 'json',
                    success: function (data, strStatus) {

                        if (data.status === true && data.statusCode === 1) {
                            $('#PaymentId').val(data.data.paymentId);
                            showMessage("Success!", "Requested Successfully", 'success');
                            requestQR(data.data.paymentId);
                        }
                    },
                });
            }
        });

    }


    var requestQR = function (val) {
        $.ajax({
            type: "GET",
            url: "/Deposit/GetPaymentInfo",
            data: { txnId: val },
            datatype: "HTML",
            success: function (result) {

                if (result != null || result === undefined) {
                    $('body').append(result);
                    $("#createdResult").modal('show');
                    $(".preloader").fadeOut();
                    $("#btnCopyAddress").on("click", function () {
                        var copyAddress = $(this).data("clipboard-text");
                        navigator.clipboard.writeText(copyAddress)
                    });

                    $("#btnCopyAmt").on("click", function () {
                        var copyAddress = $(this).data("clipboard-text");
                        navigator.clipboard.writeText(copyAddress)
                    });

                } else {

                }
            }
        });
    };


    this.init = function () {
        requestDepositFund();
        $("#noticemodal").modal('show');
    }
}
function OpenModal(id) {

    $("#" + id).modal({ show: true, backdrop: 'static' })
        .on('hidden.bs.modal', function (e) {
            $("#" + id).remove();
        });
}

