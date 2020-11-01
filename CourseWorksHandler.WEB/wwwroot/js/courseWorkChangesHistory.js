var h = new Vue({
    el: "#history",
    data: {
        seen: false,
        history: []
    },
    methods: {
        updateHistory: function () {
            let url = "/Students/CourseWorkChanges?id=" + studentId;
            this.seen = !this.seen;

            if (this.seen) {
                axios.get(url).then(response => {
                    this.history = response.data;
                });
            }
        }
    }
});