let groupsTable = new Vue({
    el: "#groupsTable",
    data: {
        rows: [],
        teacherId: teacherId
    },
    mounted: function () {
        this.updateTable();
    },
    methods: {
        updateTable: function () {
            let url = "/Groups/All";

            axios.get(url).then(response => {
                let data = response.data;
                this.rows = data;
            });
        },
        applyForGroup: function (groupId) {
            let url = "/Teachers/ApplyForGroup?groupId=" + groupId;
            axios.post(url).then(response => {
                let data = response.data;
                if (data.error === "ok") {
                    this.updateTable();
                }
                else if (data.error === "maxGroups") {
                    alert("Викладач не може мати більше 5 груп!");
                }
            });
        },
        discardFromGroup: function (groupId) {
            let url = "/Teachers/DiscardFromGroup?groupId=" + groupId;
            axios.post(url).then(response => {
                let data = response.data;
                if (data.error === "ok") {
                    this.updateTable();
                }
            });
        }
    }
});