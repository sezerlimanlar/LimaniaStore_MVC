var dataTable;

$(document).ready(function () {
    loadDataTables();
});

function loadDataTables() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/user/getall' },
        "columns": [
            { data: 'name', width: "15%" },
            { data: 'email', width: "15%" },
            { data: 'phoneNumber', width: "10%" },
            { data: 'company.name', width: "10%" },
            { data: 'role', width: "10%" },
            {
                data: { id: "id", lockoutEnd: "lockoutEnd" },
                "render": function (data) {
                    var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();

                    if (lockout > today) {
                        return `<div class="text-center">
        <a onclick="LockUnlock('${data.id}')" class="btn btn-danger text-white" style="cursor:pointer; width:100px; height:40px;">
            <i class="bi bi-lock-fill"></i> Lock </a>
        <a href="/admin/user/rolemanager?userId=${data.id}" class="btn btn-danger text-white" style="cursor:pointer; width:100px;height:40px;">
            <i class="bi bi-pencil-square"></i> Permission </a>
    </div>`;
                    } else {
                        return `<div class="text-center">
        <a onclick="LockUnlock('${data.id}')" class="btn btn-success text-white" style="cursor:pointer; width:100px;height:40px;">
            <i class="bi bi-unlock-fill"></i> Unlock </a>
        <a href="/admin/user/rolemanager?userId=${data.id}" class="btn btn-danger text-white" style="cursor:pointer; width:100px;height:40px;">
            <i class="bi bi-pencil-square"></i> Permission </a>
    </div>`;
                    }
                },
                width: "20%"
            },

        ]
    });
}

function LockUnlock(id) {
    $.ajax({
        type: "POST",
        url: '/admin/user/lockunlock',
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                dataTable.ajax.reload();
            }
        }
    });
}
