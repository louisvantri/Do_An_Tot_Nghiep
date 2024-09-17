var statusGet = 1;

function GetDataFirst() {
    $.ajax({
        "url": "/QuanLyThongSo/GetListModel",
        "type": "get",
        "data": (d) => {
            return $.extend({}, d, {
                "OrderArray": "[Name],[NameVNM]",
                "__RequestVerificationToken": $("input[name='__RequestVerificationToken']").val()
            });
        },
        "datatype": "json",
        "success": (json) => {
            $("#tbl__Models").html(`
            <thead class="bg-dark">
                <tr>
                    <th style="text-align:center" colspan="${json.data.length+1}">BẢNG THÔNG SỐ</th>
                </tr>
            </thead>
            <tbody>
                <tr><td>Tên máy</td></tr>
                <tr><td>Thời gian xử lý (giây/1sp)</td></tr>
                <tr><td>Công suất tối đa (giây/1sp)</td></tr>
                <tr><td>Số sản phẩm đã xử lý (chiếc)</td></tr>
                <tr><td>Số sản phẩm lỗi (chiếc)</td></tr>
            </tbody>`
            )
            var TongSPLoi = 0;
            json.data.forEach(item => {
                $("#tbl__Models thead tr").html($("#tbl__Models thead tr").html() + `<th style="display:none">${item.Name}<input class="form-control" name="Name_${item.Name}" value="${item.Name}" hidden/></th>`);
                $("#tbl__Models tbody tr:eq(0)").html($("#tbl__Models tbody tr:eq(0)").html() + `<td ><input class="form-control" name="NameVNM_${item.Name}" value="${item.NameVNM}" /></td>`);
                $("#tbl__Models tbody tr:eq(1)").html($("#tbl__Models tbody tr:eq(1)").html() + (item.IncludeProcTime == 1 ? `<td><input class="form-control" name="ProcTime_${item.Name}" value="${item.ProcTime}" /></td>` : `<td><input class="form-control" hidden name="ProcTime"" value=""/></td>`));
                $("#tbl__Models tbody tr:eq(2)").html($("#tbl__Models tbody tr:eq(2)").html() + `<td><p style="text-align:center">${item.MaxProcTime}</p></td>`);
                $("#tbl__Models tbody tr:eq(3)").html($("#tbl__Models tbody tr:eq(3)").html() + `<td><p style="text-align:center">${item.NumberProcessed}</p></td>`);
                $("#tbl__Models tbody tr:eq(4)").html($("#tbl__Models tbody tr:eq(4)").html() + (item.IncludeFailed == 1 ? `<td><p style="text-align:center">${item.Failed}</p></td>` : `<td><p style="text-align:center"></p></td>`));
                if(item.IncludeFailed == 1) TongSPLoi += item.Failed;
                $("#tbl__Models tbody tr:eq(0) td").css("width", "200px !important");
            })
            
            statusGet = 0;


            if ($('#BieuDo1').length > 0) {
                var ctx_1 = document.getElementById("BieuDo1").getContext("2d");
                var data_1 = {
                    labels: [
                        "Số thành phẩm",
                        "Số sản phẩm lỗi"
                    ],
                    datasets: [
                        {
                            data: [(json.data[json.data.length - 1].NumberProcessed * 100 / (json.data[json.data.length - 1].NumberProcessed + TongSPLoi)).toFixed(2), (TongSPLoi * 100 / (json.data[json.data.length - 1].NumberProcessed + TongSPLoi)).toFixed(2)],
                            backgroundColor: [
                                "#33d633",
                                "#e82525",
                            ],
                            hoverBackgroundColor: [
                                "#497a29",
                                "#722e2e",
                            ]
                        }]
                };

                var pieChart_1 = new Chart(ctx_1, {
                    type: 'pie',
                    data: data_1,
                    options: {
                        animation: {
                            duration: 3000
                        },
                        responsive: true,
                        legend: {
                            labels: {
                                fontFamily: "Nunito Sans",
                                fontColor: "#878787"
                            }
                        },
                        tooltip: {
                            backgroundColor: 'rgba(33,33,33,1)',
                            cornerRadius: 0,
                            footerFontFamily: "'Nunito Sans'"
                        },
                        elements: {
                            arc: {
                                borderWidth: 0
                            }
                        }
                    }
                });
            }


            if ($('#BieuDo2').length > 0) {
                var ctx_2 = document.getElementById("BieuDo2").getContext("2d");
                var data_2 = {
                    labels: [
                        "Trước xử lý bề mặt",
                        "Xử lý bề mặt",
                        "Máy sơn",
                    ],
                    datasets: [
                        {
                            data: [(json.data[3].Failed * 100 / TongSPLoi).toFixed(2), (json.data[5].Failed * 100 / TongSPLoi).toFixed(2), (json.data[7].Failed * 100 / TongSPLoi).toFixed(2)],
                            backgroundColor: [
                                "#ecf902",
                                "#2570f3",
                                "#a63ded",
                            ],
                            hoverBackgroundColor: [
                                "#767a20",
                                "#284983",
                                "#572b74",
                            ]
                        }]
                };

                var pieChart_2 = new Chart(ctx_2, {
                    type: 'pie',
                    data: data_2,
                    options: {
                        animation: {
                            duration: 3000
                        },
                        responsive: true,
                        legend: {
                            labels: {
                                fontFamily: "Nunito Sans",
                                fontColor: "#878787"
                            }
                        },
                        tooltip: {
                            backgroundColor: 'rgba(33,33,33,1)',
                            cornerRadius: 0,
                            footerFontFamily: "'Nunito Sans'"
                        },
                        elements: {
                            arc: {
                                borderWidth: 0
                            }
                        }
                    }
                });
            }
            return json.data;
        }
    })
}

function GetData() {
    $.ajax({
        "url": "/QuanLyThongSo/GetListModel",
        "type": "get",
        "data": (d) => {
            return $.extend({}, d, {
                "OrderArray": "[Name],[NameVNM]",
                "__RequestVerificationToken": $("input[name='__RequestVerificationToken']").val()
            });
        },
        "datatype": "json",
        "success": (json) => {
            $("#tbl__Models tbody tr:eq(3)").html(`<td>Số sản phẩm đã xử lý (chiếc)</td>`)
            $("#tbl__Models tbody tr:eq(4)").html(`<td>Số sản phẩm lỗi (chiếc)</td>`)
            var TongSPLoi = 0;
            json.data.forEach(item => {               
                $("#tbl__Models tbody tr:eq(3)").html($("#tbl__Models tbody tr:eq(3)").html()+ `<td><p style="text-align:center">${item.NumberProcessed}</p></td>`);
                $("#tbl__Models tbody tr:eq(4)").html($("#tbl__Models tbody tr:eq(4)").html()+ (item.IncludeFailed == 1 ? `<td><p style="text-align:center">${item.Failed}</p></td>` : `<td><p style="text-align:center"></p></td>`));

                if (item.IncludeFailed == 1) TongSPLoi += item.Failed;

            })

            if ($('#BieuDo1').length > 0) {
                var ctx_1 = document.getElementById("BieuDo1").getContext("2d");
                var data_1 = {
                    labels: [
                        "Số thành phẩm",
                        "Số sản phẩm lỗi"
                    ],
                    datasets: [
                        {
                            data: [(json.data[json.data.length - 1].NumberProcessed * 100 / (json.data[json.data.length - 1].NumberProcessed + TongSPLoi)).toFixed(2), (TongSPLoi * 100 / (json.data[json.data.length - 1].NumberProcessed + TongSPLoi)).toFixed(2)],
                            backgroundColor: [
                                "#33d633",
                                "#e82525",
                            ],
                            hoverBackgroundColor: [
                                "#497a29",
                                "#722e2e",
                            ]
                        }]
                };

                var pieChart_1 = new Chart(ctx_1, {
                    type: 'pie',
                    data: data_1,
                    options: {
                        animation: {
                            duration: 3000
                        },
                        responsive: true,
                        legend: {
                            labels: {
                                fontFamily: "Nunito Sans",
                                fontColor: "#878787"
                            }
                        },
                        tooltip: {
                            backgroundColor: 'rgba(33,33,33,1)',
                            cornerRadius: 0,
                            footerFontFamily: "'Nunito Sans'"
                        },
                        elements: {
                            arc: {
                                borderWidth: 0
                            }
                        }
                    }
                });
            }


            if ($('#BieuDo2').length > 0) {
                var ctx_2 = document.getElementById("BieuDo2").getContext("2d");
                var data_2 = {
                    labels: [
                        "Trước xử lý bề mặt",
                        "Xử lý bề mặt",
                        "Máy sơn",
                    ],
                    datasets: [
                        {
                            data: [(json.data[3].Failed * 100 / TongSPLoi).toFixed(2), (json.data[5].Failed * 100 / TongSPLoi).toFixed(2), (json.data[7].Failed * 100 / TongSPLoi).toFixed(2)],
                            backgroundColor: [
                                "#ecf902",
                                "#2570f3",
                                "#a63ded",
                            ],
                            hoverBackgroundColor: [
                                "#767a20",
                                "#284983",
                                "#572b74",
                            ]
                        }]
                };

                var pieChart_2 = new Chart(ctx_2, {
                    type: 'pie',
                    data: data_2,
                    options: {
                        animation: {
                            duration: 3000
                        },
                        responsive: true,
                        legend: {
                            labels: {
                                fontFamily: "Nunito Sans",
                                fontColor: "#878787"
                            }
                        },
                        tooltip: {
                            backgroundColor: 'rgba(33,33,33,1)',
                            cornerRadius: 0,
                            footerFontFamily: "'Nunito Sans'"
                        },
                        elements: {
                            arc: {
                                borderWidth: 0
                            }
                        }
                    }
                });
            }
            statusGet = 0;
            return json.data;
        }
    })
}
$("#tblresult").on("click", ".btnEdit", function () {
    var obj = $("#tblresult").DataTable().row($(this).parents('tr')).data();
    $("#appdetail").load(`/${obj.Name}/Index`, { __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val() });
    $("#appdetail").css("display", "block");
});

$(document).ready(function () {
    GetDataFirst();
})


var checkloi__num = 0;
function fn_CheckLoi() {
    var CheckLoi_Interval = setInterval(function () {
        GetData();
        checkloi__num += 1;
        $.ajax({
            url: "/QuanLyThongSo/CheckLoi",
            method: "get",
            success: function (data) {
                if (data.msg != "Ok") {
                    checkloi__num = 0;
                    //$("#notification").css("display", "block");
                    //$("#notification__audio").attr("muted", "muted")
                    //$("#notification__content").html(data.msg);

                    showMessageWarning("Cảnh báo", data.msg, 10000);
                    document.getElementById("notification__audio").play();
                    setTimeout(function () {
                        $("#notification").css("display", "none");
                        fn_CheckLoi();
                    }, 10000)


                    clearInterval(CheckLoi_Interval);
                }
            }
        })
    }, 15000);
}
fn_CheckLoi();

$("#btnSubmit").on("click",function () {

    var data = $("#form__qlts").serialize();
    $.ajax({
        "url": "/QuanLyThongSo/Update",
        "type": "Post",
        "data": data,
        "success": (data) => {
            if (data.error == 0) {
                showMessageSuccess("Success", "Cập nhật thành công");
                GetData();
            }
            else showMessageError("Có lỗi khi cập nhật", data.msg);
        }
    })
})


$("#btnDownloadExcel").on("click", function () {
    window.location.href="/QuanLyThongSo/GetExcel"
})
