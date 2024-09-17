function GetDataFirst() {
    $.ajax({
        "url": "/QuanLyCanhBao/GetListModel",
        "type": "get",
        "data": (d) => {
            return $.extend({}, d, {
                "OrderArray": "[Name],[NameVNM]",
                "__RequestVerificationToken": $("input[name='__RequestVerificationToken']").val()
            });
        },
        "datatype": "json",
        "success": (json) => {
            $("#tbl__Trams").html(`
            <thead class="bg-dark">
                <tr>
                    <th style="text-align:center" colspan="${json.data.length + 1}">BẢNG THÔNG SỐ CẢNH BÁO</th>
                </tr>
            </thead>
            <tbody>
                <tr><td>Tên máy</td></tr>
                <tr><td>Tỉ lệ lỗi tối đa (%)</td></tr>
            </tbody>`
            )
            json.data.forEach(item => {
                $("#tbl__Trams thead tr").html($("#tbl__Trams thead tr").html() + `<th style="display:none">${item.Name}<input class="form-control" name="Name_${item.Name}" value="${item.Name}" hidden/></th>`);
                $("#tbl__Trams tbody tr:eq(0)").html($("#tbl__Trams tbody tr:eq(0)").html() + `<td><p style="text-align:center">${item.NameVNM}</p> </td>`);
                $("#tbl__Trams tbody tr:eq(1)").html($("#tbl__Trams tbody tr:eq(1)").html() + `<td><input class="form-control" name="MaxErrorRate_${item.Name}" value="${item.MaxErrorRate.toFixed(2)}"/></td>`);

            })
            return json.data;
        }
    })
}

$(document).ready(function () {
    GetDataFirst();
})

$("#btnSubmit").on("click",function () {

    var data = $("#form__qlcb").serialize();
    $.ajax({
        "url": "/QuanLyCanhBao/Update",
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

function fn_CheckLoi() {
    var CheckLoi_Interval = setInterval(function () {
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
