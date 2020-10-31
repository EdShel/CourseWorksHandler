var app = new Vue({
    el: '#table',
    data: {
        pagesCount: 0,
        pageIndex: 0,
        tableRows: []
    },
    mounted: function () {
        this.updateTablePage(this.pageIndex);
    },
    methods: {
        nextPage() {
            this.pageIndex = Math.min(this.pageIndex + 1, this.pagesCount);
            this.updateTablePage();
        },
        prevPage() {
            this.pageIndex = Math.max(this.pageIndex - 1, 0);
            this.updateTablePage();
        },
        updateTablePage(pageIndex) {
            pageIndex = Math.max(0, Math.min(pageIndex, this.pagesCount));
            let url = "/Students/GetGeneralInfoTable?pageIndex=" + this.pageIndex;

            axios.get(url).then(response => {
                let data = response.data;
                this.pagesCount = data.totalPages;
                this.tableRows = data.rows;
            });
        }
    }
});

