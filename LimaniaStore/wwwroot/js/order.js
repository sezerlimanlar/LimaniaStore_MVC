var dataTable;

$(document).ready(function () {
    var url = window.location.search;

    if (url.includes("inprocess")) {
        loadDataTables("inprocess");
    } else if (url.includes("completed")) {
        loadDataTables("completed");
    } else if (url.includes("pending")) {
        loadDataTables("pending");
    } else if (url.includes("approved")) {
        loadDataTables("approved");
    } else {
        loadDataTables("all");
    }
});


function loadDataTables(status) {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/order/getall?status=' + status },
        "columns": [
            { data: 'id', width: "5%" },
            { data: 'name', width: "10%" },
            { data: 'phoneNumber', width: "10%" },
            { data: 'applicationUser.email', width: "10%" },
            { data: 'orderStatus', width: "10%" },
            { data: 'orderTotal', width: "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class=w-75 btn-group" role="group">
                    <a href="/admin/order/details?orderId=${data}"class="btn btn-dark mx-2 rounded-start-3"><i class="bi bi-pencil-square"></i> Details</a>
                    </div>`
                },
                width: "20%"
            }

        ]
    });
}
